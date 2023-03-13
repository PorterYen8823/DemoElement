using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using RCAPINet;
using static System.Collections.Specialized.BitVector32;

namespace Demo1
{

    public partial class frmMain : Form
    {
        // Debug use
        static public IPEndPoint ipep_s = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666); //  特殊訊息
        static public IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);   //  一般訊息

        static UdpClient uc = new UdpClient();


        // 系統設定參數
        static bool ContinueAction = false;
        static bool ContinueActionMode = false;
        static bool CycleAutoRun = false;
        static int PickProcessMode = 1;   //  0.移載到吹氣區優先 1.吹氣完成pick優先 
        static int RobotTimeOutInterval = 15000;


        // 處理執行續, 已經改用 Task 方式處理
        Thread trdBlowAir = null;
        Thread trdAutoRun = null;


        //  交握點位定義 ()
        List<EpsonMemoryTbl> epsonMemoryTbls = new List<EpsonMemoryTbl>
       {
            new EpsonMemoryTbl {id=31, Label= "SensoMoverGetElement_OK", Vlaue=false},
         new EpsonMemoryTbl {id=32, Label= "SensorMoveGetElement_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=33, Label= "CcdGetMoveElement_OK", Vlaue=false},
         new EpsonMemoryTbl {id=34, Label= "CcdGetMoveElement_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=35, Label= "NozzleMoveGetElement_OK", Vlaue=false},
         new EpsonMemoryTbl {id=36, Label= "NozzleMoveGetElement_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=37, Label= "MoveNgPosition_OK", Vlaue=false},
         new EpsonMemoryTbl {id=38, Label= "MoveNgPosition_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=39, Label= "MovePutElementsPosition_OK", Vlaue=false},
         new EpsonMemoryTbl {id=40, Label= "MovePutElementsPosition_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=41, Label= "MoveBlowElementsPosition1_OK", Vlaue=false},
         new EpsonMemoryTbl {id=42, Label= "MoveBlowElementsPosition1_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=43, Label= "MoveBlowElementsPosition2_OK", Vlaue=false},
         new EpsonMemoryTbl {id=44, Label= "MoveBlowElementsPosition2_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=45, Label= "PickBlowElement_OK", Vlaue=false},
         new EpsonMemoryTbl {id=46, Label= "PickBlowElement_Idle", Vlaue=false},
         new EpsonMemoryTbl {id=47, Label= "PlaceBlowElement_OK", Vlaue=false},
         new EpsonMemoryTbl {id=48, Label= "PlaceBlowElement_Idle", Vlaue=false},
           };


        static List<ProcessAreaTbl> ProcessArea = new List<ProcessAreaTbl>
        {
           new ProcessAreaTbl { ElementId=1, CcdX = 10.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
           new ProcessAreaTbl { ElementId=2, CcdX = 20.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=false},
           new ProcessAreaTbl { ElementId=3, CcdX = 30.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true}

        };

        List<ProcessFinishTbl> FinishArea = new List<ProcessFinishTbl>();

       static List<BlowAirStage> WorkingBlowStage = new List<BlowAirStage>
        {
           new BlowAirStage { Id=1, BlowLocationStatus = BlowLocationState.Init, ElementId=0 },
           new BlowAirStage { Id=2, BlowLocationStatus = BlowLocationState.Init, ElementId=0  }
        };



        bool run_Robot = false;

     
        static CancellationTokenSource cancelTokenSourceBlowAir = new CancellationTokenSource();
        static CancellationTokenSource cancelTokenSourceAutoRun = new CancellationTokenSource();


        // Robot 吸嘴有物
        static bool robotNozzleHaveElement = false;

        //工作區 量測 element X,Y 及 雷射測高度
        static double LaserMeasureValue = 0.0;
        static double ElementPick_X = 0.0;
        static double ElementPick_Y = 0.0;
        static double ElementPick_Z = 0.0;
        static int GO_NG = 0;

        // 吹氣區
        bool BlowAir_Working_loop = false;
        static StageState StageStatus = StageState.WaitPickOrPlaceOrProcess;
        bool Stage_Finished = false;
        static int Pick2FinishedElementID = 0;


        public static bool isExecuteCmdFin { get; internal set; }
        public static bool m_spel_stop = false;


        //
        int NextStep = 0;

        public frmMain()
        {
            InitializeComponent();
        }
        static Spel m_spel;


        delegate void UpdateTextBoxTextAppendChange(string TextBoxName, string Value);
        public static void TextBoxTextAppendChange(string TextBoxName, string Value)
        {
            Form form = Application.OpenForms["frmMain"];
            TextBox W;
            if (form == null)
                return;

            W = form.Controls.Find(TextBoxName, true).FirstOrDefault() as TextBox;

            if (W == null)
                return;

            if (W.InvokeRequired)
            {
                UpdateTextBoxTextAppendChange ph = new UpdateTextBoxTextAppendChange(TextBoxTextAppendChange);
                W.BeginInvoke(ph, TextBoxName, Value);
            }
            else
            {
                if (W.Lines.Count() >= 50)
                {
                    W.Clear();
                }
                W.Text = W.Text + Environment.NewLine + Value;
            }
        }

        delegate void UpdateTextBoxTextChange(string TextBoxName, string Value);
        public static void TextBoxTextChange(string TextBoxName, string Value)
        {
            Form form = Application.OpenForms["frmMain"];
            TextBox W;
            if (form == null)
                return;

            W = form.Controls.Find(TextBoxName, true).FirstOrDefault() as TextBox;

            if (W == null)
                return;

            if (W.InvokeRequired)
            {
                UpdateTextBoxTextChange ph = new UpdateTextBoxTextChange(TextBoxTextChange);
                W.BeginInvoke(ph, TextBoxName, Value);
            }
            else
            {
                W.Text = Value;
            }
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            run_Robot = true;


            //  Task.Factory.StartNew(BlowAirProcess, cancelTokenSourceBlowAir.Token);

            //Task BlowAirTask = new Task(() => BlowAirProcess());
            //BlowAirTask.Start();

            if (trdBlowAir == null)
            {
                trdBlowAir = new Thread(BlowAirProcess);
                trdBlowAir.Start();
            }


            if (run_Robot == true)
            {
                m_spel = new Spel();
                m_spel.Initialize();

                m_spel.Project = @"C:\EpsonRC70\projects\API_DEMO2\Glue_20230307\Glue_20230307.sprj";
                m_spel.EventReceived += new Spel.EventReceivedEventHandler(m_spel_EventReceived);

                m_spel.EnableEvent(SpelEvents.AllTasksStopped, true);
                
                timer1.Start();
            }


            cmbFunc.Items.Add("main");
            cmbFunc.Items.Add("main1");
            cmbFunc.Items.Add("main2");
            cmbFunc.Items.Add("main3");
            cmbFunc.Items.Add("main4");
            cmbFunc.Items.Add("main5");
            cmbFunc.Items.Add("main6");

            cmbFunc.SelectedIndex = 0;

            cmbVars.Items.Add("g_CcdX");
            cmbVars.Items.Add("g_CcdY");
            cmbVars.Items.Add("g_CcdZ");


            cmbVars.SelectedIndex = 0;

            btnPause.Enabled = false;
            btnCont.Enabled = false;
            btnStop.Enabled = false;


        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancelTokenSourceBlowAir.Cancel();
            cancelTokenSourceAutoRun.Cancel();

           
            if (trdAutoRun != null)
            {
                trdAutoRun.Abort();
            }
            if (trdBlowAir != null)
            {
                trdBlowAir.Abort();
            }

            if (run_Robot == true)
            {
                m_spel.Dispose();
            }


        }

        private void btnCont_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Continue();
                btnPause.Enabled = true;
                btnCont.Enabled = false;

            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnControllerTools_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.RunDialog(SpelDialogs.ControllerTools, this);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void btnIOMonitor_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.ShowWindow(SpelWindows.IOMonitor, this);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Pause();
                btnPause.Enabled = false;
                btnCont.Enabled = true;

            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProgramMode_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.OperationMode = SpelOperationMode.Program;
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Object v;
                v = m_spel.GetVar(cmbVars.Text);
                txtVarValue.Text = v.ToString();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Reset();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRobotManager_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.RunDialog(SpelDialogs.RobotManager, this);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSimulator_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.ShowWindow(SpelWindows.Simulator, this);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            try
            {
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnCont.Enabled = false;
                btnStop.Enabled = true;
                m_spel.Start(cmbFunc.SelectedIndex);
                m_spel.WaitCommandComplete();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Stop();
                m_spel.ResetAbort();
                btnStart.Enabled = true;
                btnPause.Enabled = false;
                btnCont.Enabled = false;
                btnStop.Enabled = false;
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.ShowWindow(SpelWindows.TaskManager, this);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTeachPoint_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.TeachPoint("robot1.pts", 1, "Teach Pick Position");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {

            try
            {
                m_spel.SetVar(cmbVars.Text, txtVarValue.Text);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void m_spel_EventReceived(object sender, SpelEventArgs e)
        {

            if (e.Event == SpelEvents.AllTasksStopped)
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
                btnCont.Enabled = false;

                
                if (m_spel.MemSw(31) && m_spel.MemSw(32))
                {
                    isExecuteCmdFin = true;
                }

                if (m_spel.MemSw(33) && m_spel.MemSw(34))
                {
                    isExecuteCmdFin = true;
                }
            }
            // textBox1.SelectedText = e.Event + "\r\n";
            textBox1.SelectedText = e.Message + "\r\n";
        }

        private void btnServoOn_Click(object sender, EventArgs e)
        {
            if (!m_spel.MotorsOn)
                m_spel.MotorsOn = true;
        }

        private void btnServoOff_Click(object sender, EventArgs e)
        {
            m_spel.MotorsOn = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (!m_spel.MotorsOn)
            //    m_spel.MotorsOn = true;
            //m_spel.Accel(50, 50);
            //m_spel.Speed(10);

            SpelPoint pt = new SpelPoint((float)381.660, (float)-387.752, (float)-175.656, (float)-5.667);
            //m_spel.Go(pt);

            //m_spel.ClearPoints();
            //m_spel.SetPoint(0,(float)525.100, (float)-157.164, (float)-32.971, (float)19.095, 0, 0);
            //m_spel.Jump(0);

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    pt = m_spel.GetPoint(i);

                  Console.WriteLine(pt.ToString());
                }
                catch (SpelException ex)
                {
                    //  MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {





        }

        private void btnReadWrite_Click(object sender, EventArgs e)
        {

        }

        private void btnWriteLabel_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                m_spel.SetVar("g_CcdX", txtg_CcdX.Text);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.SetVar("g_CcdY", txtg_CcdY.Text);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.SetVar("g_CcdZ", txtg_CcdZ.Text);
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReadg_CcdX_Click(object sender, EventArgs e)
        {
            try
            {
                Object v;
                v = m_spel.GetVar("g_CcdX");
                txtg_CcdX.Text = v.ToString();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReadg_CcdY_Click(object sender, EventArgs e)
        {
            try
            {
                Object v;
                v = m_spel.GetVar("g_CcdY");
                txtg_CcdY.Text = v.ToString();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReadg_CcdZ_Click(object sender, EventArgs e)
        {
            try
            {
                Object v;
                v = m_spel.GetVar("g_CcdZ");
                txtg_CcdZ.Text = v.ToString();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBit0On_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int BitNumber = Convert.ToInt16(button.Tag);
                m_spel.MemOn(BitNumber);
            }
        }

        private void btnBit0Off_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int BitNumber = Convert.ToInt16(button.Tag);
                m_spel.MemOff(BitNumber);
            }
        }

        private void btnFunctionRun_Click(object sender, EventArgs e)
        {

            try
            {
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnCont.Enabled = false;
                btnStop.Enabled = true;
                Button button = sender as Button;
                int FuncNumber = 0;
                if (button != null)
                {
                    FuncNumber = Convert.ToInt16(button.Tag);
                }
                m_spel.Start(FuncNumber);
                m_spel.WaitCommandComplete();
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void ContinueActionControl()
        {
            DumpSystemStatus();
            SpinWait.SpinUntil(() => ContinueAction, -1);
            if (ContinueActionMode == false)
            {
                ContinueAction = false;
            }
        }

        static void AE01_MovetoLasermeasurement()
        {
       

            SendUdpMessage("===== AE01 =====");
            TextBoxTextAppendChange("txtLog", "AE01​ 移到產品上方用雷射測距儀測距離 (z軸固定位置, 固定角度) ");

            // 1.Set  g_CcdX,  g_CcdY
            TextBoxTextAppendChange("txtLog", "1.Set  g_CcdX,  g_CcdY");
            try
            {
                m_spel.SetVar("g_CcdX", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 2.Move to Point  
            TextBoxTextAppendChange("txtLog", "2.Move to Point LaserMeasurement (Main1)");
            SendUdpMessage("2.Move to Point LaserMeasurement (Main1)");
             
            try
            {               
                //SendUdpMessage("TasksExecuting:"+ m_spel.TasksExecuting());
                m_spel.Start(1);
              
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

             SpinWait.SpinUntil(() => (m_spel.MemSw(31) && m_spel.MemSw(32)), RobotTimeOutInterval);
            

            SendUdpMessage("m_spel.Start(1) Finished");
            

            // 3.LaserMeasurementHeight = Get LaserMeasurement Value
            SendUdpMessage("AE01​ 3.LaserMeasurementHeight");
            TextBoxTextAppendChange("txtLog", "3.LaserMeasurementHeight = Get LaserMeasurement Value ");
            LaserMeasureValue = 10.33;

            SendUdpMessage("----- AE01 -----");
            SendUdpMessage("");
            ContinueActionControl();
        }

        private void btnMovetoLasermeasurement_Click(object sender, EventArgs e)
        {
             AE01_MovetoLasermeasurement();
        }


        static void AE02_CcdMoveDetectionElements()
        {
            // 1.Set  g_CcdX,  g_CcdY
            SendUdpMessage("===== AE02 =====");
            TextBoxTextAppendChange("txtLog", "AE02 移到產品上方");
            TextBoxTextAppendChange("txtLog", "1.Set  g_CcdX,  g_CcdY, g_CcdZ ");
            try
            {
                m_spel.SetVar("g_CcdX", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdZ","100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 2.Move to Point  
            TextBoxTextAppendChange("txtLog", "2.Move to Point ElementTop (Main2)");
            int mainid = 2;
            try
            {
                SendUdpMessage(" m_spel.Start(2)");
                m_spel.Start(mainid);
                
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
            SpinWait.SpinUntil(() => (m_spel.MemSw(33) && m_spel.MemSw(34)), RobotTimeOutInterval);
            TextBoxTextAppendChange("txtLog", "m_spel.Start(2) Finished");


            // 3.LaserMeasurementHeight = Get LaserMeasurement Value
            TextBoxTextAppendChange("txtLog", "3.Snap and Calcuate ElementPick_X, ElementPick_Y, ElementPick_Z, GO_NG ");
            SendUdpMessage("AE02 3.Snap and Calcuate ElementPick_X, ElementPick_Y, ElementPick_Z, GO_NG ");
            ElementPick_X = 11.11;
            ElementPick_Y = 22.22;
            ElementPick_Z = 33.33;

            SendUdpMessage("----- AE02 -----");
            SendUdpMessage("");

            ContinueActionControl();
        }

        private void btnMovetoElementTop_Click(object sender, EventArgs e)
        {
            AE02_CcdMoveDetectionElements();

        }

        static void AE03_MovetoElementPickAndBlowWaitArea(int PickElementID)
        {
            // 1.Set  g_CcdX,  g_CcdY
            SendUdpMessage("===== AE03 =====");
            TextBoxTextAppendChange("txtLog", "AE03 移到產品側邊吸取產品 並移到吹氣等待區");
            TextBoxTextAppendChange("txtLog", "1.Set  g_CcdX,  g_CcdY, g_CcdZ");
            try
            {
                m_spel.SetVar("g_CcdX", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdZ", "100");
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 2.Move to Point 
            TextBoxTextAppendChange("txtLog", "2.Move to Point ElementPick (Main3)");
            
            try
            {
                SendUdpMessage("m_spel.Start(3)");
                m_spel.Start(3);
                
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            SpinWait.SpinUntil(() => (m_spel.MemSw(35) && m_spel.MemSw(36)), RobotTimeOutInterval);
            TextBoxTextAppendChange("txtLog", "m_spel.Start(3) Finished");
            SendUdpMessage("m_spel.Start(3) Finished");

            //  將取走 Element PickFinished 標記為 1
            var SelectProcessArea = ProcessArea.Where(x => x.ElementId.Equals(PickElementID)).FirstOrDefault();
            if (SelectProcessArea != null)
            {
                SelectProcessArea.PickFinished = 1;
                robotNozzleHaveElement = true;
            }
            DumpSystemStatus();
            SendUdpMessage("----- AE03 -----");
            SendUdpMessage("");
            ContinueActionControl();
        }

        private void btnMovetoElementPickAndBlowWaitArea_Click(object sender, EventArgs e)
        {
             AE03_MovetoElementPickAndBlowWaitArea(1);


        }

        static void AE06_PickBlowElements()
        {
            SendUdpMessage("===== AE06 =====");
            TextBoxTextAppendChange("txtLog", "AE06 PickBlowElements");

            TextBoxTextAppendChange("txtLog", "1.Wait Blow Stage  if ready");
            if (StageStatus == StageState.WaitPickOrPlaceOrProcess)
            {
                var myWorkingBlowStagePick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();
                TextBoxTextAppendChange("txtLog", "2.check finished , and get finished id");
                if (myWorkingBlowStagePick != null)
                {
                    TextBoxTextAppendChange("txtLog", "Pick Select Id:" + myWorkingBlowStagePick.Id.ToString());
                    TextBoxTextAppendChange("txtLog", "3.Set id ");
                    if (myWorkingBlowStagePick.Id == 1) // 位置 1
                    {
                        // 位置1,2 互斥, 比免兩個同時true
                        m_spel.MemOn(2);
                        m_spel.MemOff(3);
                    }
                    if (myWorkingBlowStagePick.Id == 2) // 位置 2
                    {
                        // 位置1,2 互斥, 比免兩個同時true
                        m_spel.MemOff(2);
                        m_spel.MemOn(3);
                    }

                    TextBoxTextAppendChange("txtLog", "4.Move to BlowAirPick  (Main6)");
                    
                    try
                    {
                        SendUdpMessage("m_spel.Start(6)");
                        TextBoxTextAppendChange("txtLog", "m_spel.Start(6)");
                   //     SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                        m_spel.Start(6);
                        
                    }
                    catch (SpelException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                  //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                    SpinWait.SpinUntil(() => (m_spel.MemSw(45)), RobotTimeOutInterval);
                    m_spel.MemOn(0);  
                    SpinWait.SpinUntil(() => (m_spel.MemSw(46)), RobotTimeOutInterval);

                    TextBoxTextAppendChange("txtLog", "m_spel.Start(6) Finished");
                    SendUdpMessage("m_spel.Start(6) Finished");

                    Pick2FinishedElementID = myWorkingBlowStagePick.ElementId;

                    myWorkingBlowStagePick.BlowLocationStatus = BlowLocationState.RedayToPlace;
                    myWorkingBlowStagePick.ElementId = 0;
                    robotNozzleHaveElement = true;
                }

            }
            SendUdpMessage("----- AE06 -----");
            SendUdpMessage("");
            ContinueActionControl();
        }
        private  void btnMovetoBlowAirPick_Click(object sender, EventArgs e)
        {
            
             AE06_PickBlowElements();



        }

        static void AE07_PlaceBlowElements(int ProcessElementID)
        {
            SendUdpMessage("===== AE07 =====");
            TextBoxTextAppendChange("txtLog", "AE07 PlaceBlowElements");

            TextBoxTextAppendChange("txtLog", "1.Wait Blow Stage  if ready ,and no finished");
            if (StageStatus == StageState.WaitPickOrPlaceOrProcess)
            {
                var myWorkingBlowStagePick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();

                if (myWorkingBlowStagePick == null)
                {
                    var myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                    if (myWorkingBlowStagePlace != null)
                    {

                        TextBoxTextAppendChange("txtLog", "2.Place Select Id:" + myWorkingBlowStagePlace.Id.ToString());


                        TextBoxTextAppendChange("txtLog", "3.Set id ");
                        if (myWorkingBlowStagePlace.Id == 1) // 位置 1
                        {
                            // 位置1,2 互斥, 比免兩個同時true
                            m_spel.MemOff(3);
                            m_spel.MemOn(2);
                           
                        }
                        if (myWorkingBlowStagePlace.Id == 2) // 位置 2
                        {
                            // 位置1,2 互斥, 比免兩個同時true
                            m_spel.MemOff(2);
                            m_spel.MemOn(3);
                        }

                        //Move to Point  
                        TextBoxTextAppendChange("txtLog", "4.Move to Point PlaceBlowElements (Main7)");
                         
                        try
                        {
                            SendUdpMessage("m_spel.Start(7)");
                            TextBoxTextAppendChange("txtLog", "m_spel.Start(7)");
                        //    SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                            m_spel.Start(7);

                        }
                        catch (SpelException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        //  Main7 'PlaceBlowElements
                        //          MoveBlowElementsPosition1or2 --> PutBlowElement_OK
                        SpinWait.SpinUntil(() => (m_spel.MemSw(47)), RobotTimeOutInterval);

                        if (m_spel.MemSw(47)==false)
                        {
                            // 逾時處理...#001 待討論
                            // 
                        }


                        m_spel.MemOn(1);

                        SpinWait.SpinUntil(() => (m_spel.MemSw(48)), RobotTimeOutInterval);
                      //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                        SendUdpMessage("m_spel.Start(7) Finished");


                        // 將該 Id 狀態改為  WaitProcess
                        myWorkingBlowStagePlace.BlowLocationStatus = BlowLocationState.WaitProcess;
                        myWorkingBlowStagePlace.ElementId = ProcessElementID;

                        ContinueActionControl();

                    }
                }
                else
                {
                    var myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                    if (myWorkingBlowStagePlace != null)
                    {

                        TextBoxTextAppendChange("txtLog", "2.Place Select Id:" + myWorkingBlowStagePlace.Id.ToString());


                        TextBoxTextAppendChange("txtLog", "3.Set id ");
                        if (myWorkingBlowStagePlace.Id == 1) // 位置 1
                        {
                            // 位置1,2 互斥, 比免兩個同時true                           
                            m_spel.MemOff(3);
                            m_spel.MemOn(2);
                        }
                        if (myWorkingBlowStagePlace.Id == 2) // 位置 2
                        {
                            // 位置1,2 互斥, 比免兩個同時true
                            m_spel.MemOff(2);
                            m_spel.MemOn(3);
                        }

                        //Move to Point  
                        TextBoxTextAppendChange("txtLog", "4.Move to Point PlaceBlowElements (Main7)");
                        
                        try
                        {
                            SendUdpMessage("m_spel.Start(7)");
                            TextBoxTextAppendChange("txtLog", "m_spel.Start(7)");
                            m_spel.Start(7);

                        }
                        catch (SpelException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        // MoveBlowElementsPosition1or2 --> PutBlowElement_OK
                        SpinWait.SpinUntil(() => (m_spel.MemSw(47)), RobotTimeOutInterval);

                        if (m_spel.MemSw(47) == false)
                        {
                            // 逾時處理...#001 待討論
                            // 
                        }

                        m_spel.MemOn(1);

                        SpinWait.SpinUntil(() => (m_spel.MemSw(48)), RobotTimeOutInterval);
                        SendUdpMessage("m_spel.Start(7) Finished");


                        // 將該 Id 狀態改為  WaitProcess
                        myWorkingBlowStagePlace.BlowLocationStatus = BlowLocationState.WaitProcess;
                        myWorkingBlowStagePlace.ElementId = ProcessElementID;

                        ContinueActionControl();
                    }
                }

            }
            SendUdpMessage("----- AE07 -----");
            SendUdpMessage("");
        }
        private  void btnMovetoBlowAirPlace_Click(object sender, EventArgs e)
        {
             AE07_PlaceBlowElements(1);

        }

        static void AE04_MvoeNgPosition(int ThrowElementId)
        {
            SendUdpMessage("===== AE04 =====");
            TextBoxTextAppendChange("txtLog", "4.MovetoNGArea");
            int mainid = 4;
            try
            {
                SendUdpMessage("m_spel.Start(4)");
                m_spel.Start(mainid);
                
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            robotNozzleHaveElement = false;

            SpinWait.SpinUntil(() => (m_spel.MemSw(37) && m_spel.MemSw(38)), RobotTimeOutInterval);
            TextBoxTextAppendChange("txtLog", "m_spel.Start(4) Finished");
            SendUdpMessage("m_spel.Start(4) Finished");

            //  將取走 Drop PlaceOrThrowFinished 標記為 1
            var SelectProcessArea = ProcessArea.Where(x => x.ElementId.Equals(ThrowElementId)).FirstOrDefault();
            if (SelectProcessArea != null)
            {
                SelectProcessArea.PlaceOrThrowFinished = 1;
                robotNozzleHaveElement = false;
            }

            SendUdpMessage("----- AE04 -----");
            SendUdpMessage("");
            
            DumpSystemStatus();
        }
        private  void BtnMovetoNGArea_Click(object sender, EventArgs e)
        {
             AE04_MvoeNgPosition(1);

        }

        static void AE05_MovePutElementsPosition()
        {
            SendUdpMessage("===== AE05 =====");
            TextBoxTextAppendChange("txtLog", "AE05 MovePutElementsPosition");
            bool a = m_spel.TasksExecuting();


            try
            {
                SendUdpMessage("m_spel.Start(5)");
                TextBoxTextAppendChange("txtLog", "m_spel.Start(5)");       
                m_spel.Start(5);                 
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
            SpinWait.SpinUntil(() => (m_spel.MemSw(39) && m_spel.MemSw(40)), RobotTimeOutInterval);
           // SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
            TextBoxTextAppendChange("txtLog", "m_spel.Start(5) Finished");
            SendUdpMessage("m_spel.Start(5) Finished");

            var myProcessArea = ProcessArea.Where(p => p.ElementId.Equals(Pick2FinishedElementID)).FirstOrDefault();
            if (myProcessArea != null)
            {
                myProcessArea.PlaceOrThrowFinished = 1;
                robotNozzleHaveElement = false;
            }
            SendUdpMessage("----- AE05 -----");
            SendUdpMessage("");
            ContinueActionControl();
        }
         void btnMovetoFinishAreaPlace_Click(object sender, EventArgs e)
        {
             AE05_MovePutElementsPosition();


        }

       

        private  void BlowAirProcess()
        {
            //  Blow Stage Init
            StageStatus = StageState.Init;

            // 初值化, 將Stage 狀態改為 ReadyToPlace
            var myWorkingBlowStage = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.Init)).ToList();
            foreach (var SelectWorkingBlowStage in myWorkingBlowStage)
            {
                SelectWorkingBlowStage.BlowLocationStatus = BlowLocationState.RedayToPlace;
            }

            //  處理迴圈
            BlowAir_Working_loop = true;
            //  Blow Stage Init
            StageStatus = StageState.WaitPickOrPlaceOrProcess;

          //  while (BlowAir_Working_loop)
            while (!cancelTokenSourceBlowAir.IsCancellationRequested)
                {
                var BlowStage_WaitPick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();
               
                var BlowStage_process = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.WaitProcess)).FirstOrDefault();
                if ((BlowStage_WaitPick == null) && (BlowStage_process != null))
                {
                    BlowStage_process.BlowLocationStatus = BlowLocationState.Process;
                    StageStatus = StageState.Processing;
                    DumpSystemStatus();
                    SpinWait.SpinUntil(() => false, 100);
                    // Motion control 
                    //  1. 移動到吹氣位置
                    //  2. 吹氣 
                    //  3. 吹 5秒結束吹氣, 並將狀態改為 ReadyToPick
                    //  4. 移動到吹氣等待區
                    Console.WriteLine("1. 移動到吹氣位置");
                    Console.WriteLine("2. 吹氣 ElementID:" + BlowStage_process.ElementId.ToString());
                    Console.WriteLine("3. 吹 5秒結束吹氣, 並將狀態改為 ReadyToPick");
                    Console.WriteLine("4. 移動到吹氣等待區");
                    SpinWait.SpinUntil(() => false, 7000);
                    // Change Process --> ReadyPick
                    BlowStage_process.BlowLocationStatus = BlowLocationState.RedayToPick;

                    DumpSystemStatus();
                    StageStatus = StageState.WaitPickOrPlaceOrProcess;
                }
                else
                {
                    SpinWait.SpinUntil(() => false, 1000);
                    DumpSystemStatus();
                    Console.WriteLine("BlowAirProcess loop");
                }
            }
            Console.WriteLine("BlowAirProcess End");
        }

        static void AutoRun()
        {
            while ((!cancelTokenSourceAutoRun.IsCancellationRequested))
            {
                // 如有 BlowAir 有要 pick, 先處理

                var myWorkingBlowStagePick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();
                if (myWorkingBlowStagePick != null)
                {      
                    if (PickProcessMode ==  0) 
                    {
                        AE06_PickBlowElements();
                        AE05_MovePutElementsPosition();
                    }

                    if (PickProcessMode == 1) 
                    {
                        if ((WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).Count() == 1) && (ProcessArea.Where(p => p.PickFinished.Equals(0)).Count() > 0))
                        {
                            Console.WriteLine("優先取料");
                        }
                        else
                        {
                            AE06_PickBlowElements();
                            AE05_MovePutElementsPosition();
                        }
                    }
                        
                   
                }


                var myWorkingBlowStageCount = WorkingBlowStage.Where(p => p.ElementId > 0).Count();
                if (myWorkingBlowStageCount == 2)
                {
                    // 不可再 先取料, 要等 Blow Air 有一個 readyPick
                    var myWorkingBlowStageCountView = WorkingBlowStage.Where(p => p.ElementId > 0).ToList();
                    foreach (var item in myWorkingBlowStageCountView)
                    {
                        Console.WriteLine(String.Format("ElementId={0}, StageStatus={1} ", item.ElementId, item.BlowLocationStatus));
                    }

                }

                else
                {
                    // Check 吹氣區可以 Place 數量
                    var myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                    // 取 element
                    var SelectProcessArea = ProcessArea.Where(p => p.PickFinished.Equals(0)).OrderByDescending(p => p.CcdY).OrderByDescending(p => p.CcdX).FirstOrDefault();
                    if ((SelectProcessArea != null) && (myWorkingBlowStagePlace != null))
                    {

                        TextBoxTextAppendChange("txtLog", "Process Element:" + SelectProcessArea.ElementId);

                        
                         AE01_MovetoLasermeasurement();
                        
                         AE02_CcdMoveDetectionElements();
                        
                         AE03_MovetoElementPickAndBlowWaitArea(SelectProcessArea.ElementId);
                        
                        DumpSystemStatus();

                        if (SelectProcessArea.Go_NG == true)
                        {

                             AE07_PlaceBlowElements(SelectProcessArea.ElementId);
                        }
                        else
                        {
                             AE04_MvoeNgPosition(SelectProcessArea.ElementId);
                        }



                        TextBoxTextAppendChange("txtLog", "--------");
                        TextBoxTextAppendChange("txtLog", "");
                        DumpSystemStatus();
                    }
                    else
                    {
                        // 是否有在 blow Air 作業的數量
                        var BlowStage_Cnt = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).Count();
                        if (BlowStage_Cnt == 2)
                        {
                            if (ProcessArea.Where(p => p.PlaceOrThrowFinished.Equals(0)).Count() > 0)
                            {
                                Console.WriteLine("Still element to process");
                            }
                            else
                            {
                                //break;
                                Console.WriteLine("No element to process");
                                SpinWait.SpinUntil(() => true, 1500);
                            }

                        }

                        var BlowStage_Pick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick) || p.BlowLocationStatus.Equals(BlowLocationState.Process) || p.BlowLocationStatus.Equals(BlowLocationState.WaitProcess)).Count();
                        if (BlowStage_Cnt == 1)
                        {
                            SendUdpMessage("wait BlowStage_Pick");
                            // NextStep = 6;
                            if (CycleAutoRun)
                            {
                                ResetTestData();
                                SpinWait.SpinUntil(() => true, 2500);
                            }
                        }

                    }
                }
                SpinWait.SpinUntil(() => true, 500);
                Application.DoEvents();

            }
        }
        private  void btnAutoRun_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(AutoRun, cancelTokenSourceAutoRun.Token);
            //Task AutoRunTask = new Task(() => AutoRun());
            //AutoRunTask.Start();

            //if (trdAutoRun == null)
            //{
            //    trdAutoRun = new Thread(AutoRun);
            //    trdAutoRun.Start();
            //}
            btnAutoRun.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ContinueAction = true;
            SendUdpMessage("ContinueAction = true");
        }

        static void DumpSystemStatus()
        {
            string jsonString = JsonConvert.SerializeObject(ProcessArea, Formatting.Indented);
            string fileName = Path.Combine(@"c:\temp\", "ProcessArea.Json");
            try
            {
                File.WriteAllText(fileName, jsonString);
            }
            catch { }
            {

            }

            jsonString = JsonConvert.SerializeObject(WorkingBlowStage, Formatting.Indented);
            fileName = Path.Combine(@"c:\temp\", "WorkingBlowStage.Json");

            try
            {
                File.WriteAllText(fileName, jsonString);
            }
            catch { }
            {

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            foreach(var items in epsonMemoryTbls)
            {
                items.Vlaue = m_spel.MemSw(items.id); // 指派記憶體值 

                try
                {
                    if (m_spel.MemSw(47))   // PlaceBlowElement_OK
                    {
                        // Place  吹氣區 Eelemnt 完成                 
                        // 夾住夾爪  MemOn(1);
                        m_spel.MemOn(1);
                    }

                    if (m_spel.MemSw(45))   //  PickBlowElement_OK
                    {

                        // Pick 吹氣區 Eelemnt 完成
                        // 放開夾爪  MemOn(0)

                        m_spel.MemOn(0);
                    }
                }
                catch (SpelException ex)
                {
                    MessageBox.Show(ex.Message);
                }

               
            }

            timer1.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bool Rtn_data = false;


            int mainid = 1;
            isExecuteCmdFin = false;
            try
            {
                SendUdpMessage("m_spel.Start(1)");
                TextBoxTextAppendChange("txtLog", "m_spel.Start(1)");
               m_spel.Start(mainid); 
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }

            // SpinWait.SpinUntil(() => (m_spel.MemSw(31) && m_spel.MemSw(32)) , RobotTimeOutInterval);
            SpinWait.SpinUntil(() => {
                try
                {
                    Rtn_data = (m_spel.MemSw(31) && m_spel.MemSw(32));
                }
                catch
                {
                }

                return Rtn_data;
            },  RobotTimeOutInterval);


            Console.WriteLine("### Rtn_data:" + Rtn_data);
            //  用 controller 記憶體, 或 TasksExecuting 均可判斷作業是否正常完成
            //  1.m_spel.TasksExecuting()
            // 2.m_spel.MemSw(31) && m_spel.MemSw(32)
            if (m_spel.MemSw(31) && m_spel.MemSw(32))
            {
                SendUdpMessage("m_spel.Start(1) Finished Normal");
                TextBoxTextAppendChange("txtLog", "m_spel.Start(1) Finished");
            }
            else
            {
                SendUdpMessage("m_spel.Start(1) Timeout");
                TextBoxTextAppendChange("txtLog", "m_spel.Start(1) Timeout");
            }

            SpelTaskState taskState;
            taskState = m_spel.TaskState(0);

        }


        static void SendUdpMessage(string myMessage)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(myMessage);
            uc.Send(b, b.Length, ipep);    

        }

        static void ResetTestData()
        {
            foreach (var item in ProcessArea)
            {
                item.PickFinished = 0;
                item.PlaceOrThrowFinished = 0;
            }
            DumpSystemStatus();
        }
        
        private void btnResetData_Click(object sender, EventArgs e)
        {
            ResetTestData();         

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            ContinueActionMode = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ContinueActionMode = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Pause();            
            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    m_spel.Stop();              
            //}
            //catch (SpelException ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Continue();              

            }
            catch (SpelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ProcessArea = new List<ProcessAreaTbl>
            {
               new ProcessAreaTbl { ElementId=1, CcdX = 10.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=2, CcdX = 20.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=3, CcdX = 30.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=false},
               new ProcessAreaTbl { ElementId=4, CcdX = 10.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=5, CcdX = 20.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=false},
               new ProcessAreaTbl { ElementId=6, CcdX = 30.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=7, CcdX = 40.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},

            };
            Random rnd = new Random();

            for (int i = 0; i < 2; i++)
            {
                int ElementId = rnd.Next(0, ProcessArea.Count());
                ProcessArea.Where(p => p.ElementId == ElementId).ToList().ForEach(x => x.Go_NG = false);
            }

        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            ProcessFinishTbl processFinishRow = new ProcessFinishTbl();
            processFinishRow.Id = 0;
            processFinishRow.CcdX = 0;
            processFinishRow.CcdY = 0;
            processFinishRow.CcdZ = 0;
            processFinishRow.Finished = 0;
            FinishArea.Add(processFinishRow);

            processFinishRow = new ProcessFinishTbl();
            processFinishRow.Id = 1;
            processFinishRow.CcdX = 0;
            processFinishRow.CcdY = 0;
            processFinishRow.CcdZ = 0;
            processFinishRow.Finished = 1;
            FinishArea.Add(processFinishRow);

        }
    }


    public enum StageState
    {
        Init = 0,
        WaitPickOrPlaceOrProcess = 1,
        Processing = 2
    }
    public enum BlowLocationState
    {
        Init = 1,
        RedayToPlace = 2,
        WaitProcess = 3,
        Process = 4,
        RedayToPick = 5,
        Shutdown = 6
    }

    public class BlowAirStage
    {
        public int Id { get; set; }
        public BlowLocationState BlowLocationStatus { get; set; }

        public int ElementId { get; set; }
    }

    public class EpsonMemoryTbl
    {
        public int id { get; set; }
        public String Label { get; set; }
        public bool Vlaue { get; set; }

    }
        public class ProcessAreaTbl
    {
        public int ElementId { get; set; }
        public double CcdX { get; set; }
        public double CcdY { get; set; }
        public double CcdZ { get; set; }
        public int PickFinished { get; set; }
        public int PlaceOrThrowFinished { get; set; }

        public bool Go_NG { get; set; }
    }

    public class ProcessFinishTbl
    {
        public int Id { get; set; }
        public double CcdX { get; set; }
        public double CcdY { get; set; }
        public double CcdZ { get; set; }
        public int Finished { get; set; }
    }



}
