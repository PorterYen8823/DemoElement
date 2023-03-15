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
using GlueNet.MMM.AutoTransferring.EpsonRobot;
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
        static ProcessMode PickProcessMode = ProcessMode.PlaceElementFirst;   //  0.移載到吹氣區pick優先 1.工作區 pick優先 
        static int RobotTimeOutInterval = 15000;
        static bool IsVirtualModel = true;


        // 處理執行續, 已經改用 Task 方式處理
        Thread trdBlowAir = null;
        Thread trdAutoRun = null;


        //  交握點位定義 
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

        static List<ProcessFinishTbl> FinishArea = new List<ProcessFinishTbl>();

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


        // Epson
        private static GlueNet.MMM.AutoTransferring.EpsonRobot.EpsonRobot epsonRobot;

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
                m_spel.ServerInstance = 1;
                m_spel.Initialize();

                 m_spel.Project = @"C:\EpsonRC70\projects\API_Demos\Glue_20230315_OK\Glue_20230315_OK.sprj";
              // m_spel.Project = @"C:\EpsonRC70\projects\API_DEMO2\Glue_20230307\Glue_20230307.sprj";
                m_spel.EventReceived += new Spel.EventReceivedEventHandler(m_spel_EventReceived);

                m_spel.EnableEvent(SpelEvents.AllTasksStopped, true);
                
                timer1.Start();
                epsonRobot = new GlueNet.MMM.AutoTransferring.EpsonRobot.EpsonRobot();
            }


            cmbFunc.Items.Add("main");
            cmbFunc.Items.Add("main1");
            cmbFunc.Items.Add("main2");
            cmbFunc.Items.Add("main3");
            cmbFunc.Items.Add("main4");
            cmbFunc.Items.Add("main5");
            cmbFunc.Items.Add("main6");

            cmbFunc.SelectedIndex = 0;  
            btnPause.Enabled = false;
            btnCont.Enabled = false;
            btnStop.Enabled = false;


        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            cancelTokenSourceBlowAir.Cancel();
            cancelTokenSourceAutoRun.Cancel();          
           

            if (run_Robot == true)
            {
                epsonRobot.Dispose();
                m_spel.Dispose();
            }


        }

        private void btnCont_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    m_spel.Continue();
            //    btnPause.Enabled = true;
            //    btnCont.Enabled = false;

            //}
            //catch (SpelException ex)
            //{
            //    throw ex;
            //   // MessageBox.Show(ex.Message);
            //}
        }

        private void btnControllerTools_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    m_spel.RunDialog(SpelDialogs.ControllerTools, this);
            //}
            //catch (SpelException ex)
            //{
            //    throw ex;
            //   // MessageBox.Show(ex.Message);
            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void btnIOMonitor_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    m_spel.ShowWindow(SpelWindows.IOMonitor, this);
            //}
            //catch (SpelException ex)
            //{
            //    throw ex;
            //    // MessageBox.Show(ex.Message);
            //}
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    m_spel.Pause();
            //    btnPause.Enabled = false;
            //    btnCont.Enabled = true;

            //}
            //catch (SpelException ex)
            //{
            //    throw ex;
            //    //MessageBox.Show(ex.Message);
            //}
        }

        private void btnProgramMode_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.OperationMode = SpelOperationMode.Program;
            }
            catch (SpelException ex)
            {
                //throw ex;
                 MessageBox.Show(ex.Message);
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Object v;
            //    v = m_spel.GetVar(cmbVars.Text);
            //    txtVarValue.Text = v.ToString();
            //}
            //catch (SpelException ex)
            //{
            //    throw ex;
            //   // MessageBox.Show(ex.Message);
            //}
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                m_spel.Reset();
            }
            catch (SpelException ex)
            {
                throw ex;
               // MessageBox.Show(ex.Message);
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
                throw ex;
               // MessageBox.Show(ex.Message);
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
                throw ex;
                // MessageBox.Show(ex.Message);
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
                throw ex;
                // MessageBox.Show(ex.Message);
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
                throw ex;
                // MessageBox.Show(ex.Message);
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
                throw ex;
                // MessageBox.Show(ex.Message);
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
                throw ex;
                // MessageBox.Show(ex.Message);
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

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }

        private void btnReadg_CcdX_Click(object sender, EventArgs e)
        {
            
        }

        private void btnReadg_CcdY_Click(object sender, EventArgs e)
        {
            
        }

        private void btnReadg_CcdZ_Click(object sender, EventArgs e)
        {
            
        }

        private void btnBit0On_Click(object sender, EventArgs e)
        {
            
        }

        private void btnBit0Off_Click(object sender, EventArgs e)
        {
            
        }

        private void btnFunctionRun_Click(object sender, EventArgs e)
        {

        }

        static void ContinueActionControl()
        {
            DumpSystemStatus();
            if (ContinueAction == false)
            {
                SendUdpMessage("Please press Single Continue button!");
            }
            
            SpinWait.SpinUntil(() => ContinueAction, -1);
            if (ContinueActionMode == false)
            {
                ContinueAction = false;
            }
        }

        //static void AE01_MovetoLasermeasurement(float ccdX, float ccdY)
        //{
        //    AddLog(" ===== AE01 =====");
        //    AddLog("AE01.1 Set  g_CcdX,  g_CcdY "); 

        //    try
        //    {
        //        m_spel.SetVar("g_CcdX", ccdX);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        //MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
        //        m_spel.SetVar("g_CcdY", ccdY);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        // MessageBox.Show(ex.Message);
        //    }
                        
        //    AddLog("AE01.2 Move to Point LaserMeasurement (Main1)");            
        //    try
        //    {               
        //        //SendUdpMessage("TasksExecuting:"+ m_spel.TasksExecuting());
        //        m_spel.Start(1);
              
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //     //   MessageBox.Show(ex.Message);
        //    }

        //    SpinWait.SpinUntil(() => (m_spel.MemSw(31) && m_spel.MemSw(32)), RobotTimeOutInterval);

        //    AddLog("m_spel.Start(1) Finished");
        //    AddLog("----- AE01 -----");
        //    AddLog("");

        //}

        private void btnMovetoLasermeasurement_Click(object sender, EventArgs e)
        {
            float g_CcdX = (float)Convert.ToDouble(txtStep1_g_CcdX.Text);
            float g_CcdY = (float)Convert.ToDouble(txtStep1_g_CcdY.Text);
            epsonRobot.AE01_MovetoLasermeasurement(g_CcdX, g_CcdY);
        }


        //static void AE02_CcdMoveDetectionElements(float ccdX, float ccdY, float ccdZ)
        //{
        //    AddLog("===== AE02 =====");
        //    AddLog("AE02.1 Set  g_CcdX,  g_CcdY, g_CcdZ ");
        //    try
        //    {
        //        m_spel.SetVar("g_CcdX", ccdX);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
        //        m_spel.SetVar("g_CcdY", ccdY);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
               
        //        m_spel.SetVar("g_CcdZ", ccdY);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }

        //    // 2.Move to Point  
        //    AddLog("AE02.2 Move to Point ElementTop (Main2)");
        //    int mainid = 2;
        //    try
        //    {
        //        SendUdpMessage(" m_spel.Start(2)");
        //        m_spel.Start(mainid);
                
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }
        //    SpinWait.SpinUntil(() => (m_spel.MemSw(33) && m_spel.MemSw(34)), RobotTimeOutInterval);
            
        //    AddLog("m_spel.Start(2) Finished");
        //    AddLog("----- AE02 -----");
        //    AddLog("");

        //}

        private void btnMovetoElementTop_Click(object sender, EventArgs e)
        {
            float g_CcdX = (float)Convert.ToDouble(txtStep2_g_CcdX.Text);
            float g_CcdY = (float)Convert.ToDouble(txtStep2_g_CcdY.Text);
            float g_CcdZ = (float)Convert.ToDouble(txtStep2_g_CcdZ.Text);
          

            epsonRobot.AE02_CcdMoveDetectionElements(g_CcdX, g_CcdY, g_CcdZ);

        }


        static void AddLog( string msg )
        {
            TextBoxTextAppendChange("txtLog", msg);
            SendUdpMessage(msg);
        }

        //static void AE03_MovetoElementPickAndBlowWaitArea(float g_CcdX, float g_CcdY, float g_CcdZ)
        //{
        //    // 1.Set  g_CcdX,  g_CcdY
        //    AddLog("===== AE03 =====");          
        //    AddLog("AE03.1 Set  g_CcdX,  g_CcdY, g_CcdZ");
        //    try
        //    {
        //        m_spel.SetVar("g_CcdX", g_CcdX);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        //MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
        //        m_spel.SetVar("g_CcdY", g_CcdY);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        // MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
        //        m_spel.SetVar("g_CcdZ", g_CcdZ);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }

        //    // 2.Move to Point 
        //    AddLog("AE03.2 Move to Point ElementPick (Main3)");
            
        //    try
        //    {
        //        SendUdpMessage("m_spel.Start(3)");
        //        m_spel.Start(3);
                
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        // MessageBox.Show(ex.Message);
        //    }

        //    SpinWait.SpinUntil(() => (m_spel.MemSw(35) && m_spel.MemSw(36)), RobotTimeOutInterval);

        //    AddLog("m_spel.Start(3) Finished");
        //    AddLog("----- AE03 -----");
        //    AddLog("");

        //}

        private void btnMovetoElementPickAndBlowWaitArea_Click(object sender, EventArgs e)
        {
            float g_CcdX = (float)Convert.ToDouble(txtStep3_g_CcdX.Text);
            float g_CcdY = (float)Convert.ToDouble(txtStep3_g_CcdY.Text);
            float g_CcdZ = (float)Convert.ToDouble(txtStep3_g_CcdZ.Text);
            epsonRobot.AE03_MovetoElementPickAndBlowWaitArea(g_CcdX, g_CcdY, g_CcdZ);


        }

        //static void AE06_PickBlowElements(int StagePickID)
        //{
        //    AddLog("===== AE06 =====");
        //    AddLog("AE06.1 Stage Pick ID:" + StagePickID.ToString());
             
        //    if (StagePickID == 1) // 位置 1
        //    {
        //        // 位置1,2 互斥, 比免兩個同時true
        //        m_spel.MemOn(2);
        //        m_spel.MemOff(3);
        //    }
        //    if (StagePickID == 2) // 位置 2
        //    {
        //        // 位置1,2 互斥, 比免兩個同時true
        //        m_spel.MemOff(2);
        //        m_spel.MemOn(3);
        //    }

        //    AddLog("AE06.2 Move to BlowAirPick  (Main6)");

        //    try
        //    {
               
        //        AddLog("m_spel.Start(6)");
        //        //     SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
        //        m_spel.Start(6);

        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        //MessageBox.Show(ex.Message);
        //    }
        //    //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
        //    SpinWait.SpinUntil(() => (m_spel.MemSw(45)), RobotTimeOutInterval);
        //    m_spel.MemOn(0);
        //    SpinWait.SpinUntil(() => (m_spel.MemSw(46)), RobotTimeOutInterval);

        //    AddLog("m_spel.Start(6) Finished");
        //    AddLog("----- AE06 -----");
        //    AddLog("");


        //}

        private  void btnMovetoBlowAirPick_Click(object sender, EventArgs e)
        {
            int Step6_id = Convert.ToInt16(txtStep6_id.Text);
            epsonRobot.AE06_PickBlowElements(Step6_id);



        }

        //static void AE07_PlaceBlowElements(int StagePlaceId)
        //{
        //    AddLog("===== AE07 =====");
        //    AddLog("AE07.1 Stage Place Id " + StagePlaceId.ToString());
        //    if (StagePlaceId == 1) // 位置 1
        //    {
        //        // 位置1,2 互斥, 比免兩個同時true
        //        m_spel.MemOff(3);
        //        m_spel.MemOn(2);

        //    }
        //    if (StagePlaceId == 2) // 位置 2
        //    {
        //        // 位置1,2 互斥, 比免兩個同時true
        //        m_spel.MemOff(2);
        //        m_spel.MemOn(3);
        //    }

        //    //Move to Point  
        //    AddLog("AE07.2 Move to Point PlaceBlowElements (Main7)");

        //    try
        //    {
                
        //        AddLog("m_spel.Start(7)");
        //        //    SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
        //        m_spel.Start(7);

        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        // MessageBox.Show(ex.Message);
        //    }
        //    //  Main7 'PlaceBlowElements
        //    //          MoveBlowElementsPosition1or2 --> PutBlowElement_OK
        //    SpinWait.SpinUntil(() => (m_spel.MemSw(47)), RobotTimeOutInterval);

        //    if (m_spel.MemSw(47) == false)
        //    {
        //        // 逾時處理...#001 待討論
        //        // 
        //    }

        //    // 
        //    m_spel.MemOn(1);

        //    SpinWait.SpinUntil(() => (m_spel.MemSw(48)), RobotTimeOutInterval);
        //    //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
        //    if (m_spel.MemSw(48) == false)
        //    {
        //        // 逾時處理...#002 待討論
        //        // 
        //    }
        //    AddLog("m_spel.Start(7) Finished");
        //    AddLog("----- AE07 -----");
        //    AddLog("");
        //}
       
        private  void btnMovetoBlowAirPlace_Click(object sender, EventArgs e)
        {
            int Step7_id = Convert.ToInt16(txtStep7_id.Text);
            epsonRobot.AE07_PlaceBlowElements(Step7_id);

        }

        //static void AE04_MovetoNGArea()
        //{
        //    AddLog("===== AE04 =====");
            
        //    int mainid = 4;
        //    try
        //    {
        //        AddLog("m_spel.Start(4)");
        //        m_spel.Start(mainid);
                
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }

        //    SpinWait.SpinUntil(() => (m_spel.MemSw(37) && m_spel.MemSw(38)), RobotTimeOutInterval);

        //    AddLog("m_spel.Start(4) Finished");             
        //    SendUdpMessage("----- AE04 -----");
        //    SendUdpMessage("");
        //}

        private  void BtnMovetoNGArea_Click(object sender, EventArgs e)
        {
            epsonRobot.AE04_MovetoNGArea();

        }

        //static void AE05_MovePutElementsPosition(float ccdX, float ccdY)
        //{
        //    AddLog("===== AE05 =====");
        //    AddLog("AE05.1 Assign ccdX, ccdY");
        //    try
        //    {
        //        m_spel.SetVar("g_CcdX", ccdX);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        //MessageBox.Show(ex.Message);
        //    }

        //    try
        //    {
        //        m_spel.SetVar("g_CcdY", ccdY);
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //        // MessageBox.Show(ex.Message);
        //    }

        //    AddLog("AE05.2 MovePutElementsPosition");           

        //    try
        //    {                 
        //        AddLog( "m_spel.Start(5)");       
        //        m_spel.Start(5);                 
        //    }
        //    catch (SpelException ex)
        //    {
        //        throw ex;
        //       // MessageBox.Show(ex.Message);
        //    }
            
        //    SpinWait.SpinUntil(() => (m_spel.MemSw(39) && m_spel.MemSw(40)), RobotTimeOutInterval);
             

        //    AddLog("m_spel.Start(5) Finished");
        //    AddLog("----- AE05 -----");
        //    AddLog("");

        //}

         void btnMovetoFinishAreaPlace_Click(object sender, EventArgs e)
        {
            epsonRobot.AE05_MovePutElementsPosition(10,10);
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
            if (StageStatus == StageState.WaitPickOrPlaceOrProcess)
            {
                this.Invoke(new Action(() => lblStageStatus.BackColor = Color.LimeGreen));
            }
            //  while (BlowAir_Working_loop)
            while (!cancelTokenSourceBlowAir.IsCancellationRequested)
                {

               
              

                var BlowStage_WaitPick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();
               
                var BlowStage_process = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.WaitProcess)).FirstOrDefault();
                
                if ((BlowStage_WaitPick == null) && (BlowStage_process != null))
                {
                    BlowStage_process.BlowLocationStatus = BlowLocationState.Process;
                    StageStatus = StageState.Processing;
                    this.Invoke(new Action(() => lblStageStatus.Text = StageStatus.ToString()));
                    this.Invoke(new Action(() => lblStageStatus.BackColor = Color.LightPink));
                    DumpSystemStatus();
                    SpinWait.SpinUntil(() => false, 100);
                    // Motion control 
                    //  1. 移動到吹氣位置
                    //  2. 吹氣 
                    //  3. 吹 5秒結束吹氣, 並將狀態改為 ReadyToPick
                    //  4. 移動到吹氣等待區
                    Console.WriteLine("1. 移動到吹氣位置");
                    Console.WriteLine("2. 吹氣 ElementID:" + BlowStage_process.ElementId.ToString());
                    Console.WriteLine("3. 吹 N 秒結束吹氣, 並將狀態改為 ReadyToPick");
                    Console.WriteLine("4. 移動到吹氣等待區");
                    SpinWait.SpinUntil(() => false, 12000);
                    // Change Process --> ReadyPick
                    BlowStage_process.BlowLocationStatus = BlowLocationState.RedayToPick;                    
                    StageStatus = StageState.WaitPickOrPlaceOrProcess;
                    this.Invoke(new Action(() => lblStageStatus.Text = StageStatus.ToString()));
                    this.Invoke(new Action(() => lblStageStatus.BackColor = Color.LimeGreen));
                    DumpSystemStatus();
                }
                else
                {
                    SpinWait.SpinUntil(() => false, 2000);
                    DumpSystemStatus();
                    //Console.WriteLine("BlowAirProcess loop");
                }
            }
            Console.WriteLine("BlowAirProcess End");
        }

         void AutoRun()
        {
            while ((!cancelTokenSourceAutoRun.IsCancellationRequested))
            {                

                SendUdpMessage("***** AutoRun Loop");
                var myWorkingBlowStagePick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();
                if (myWorkingBlowStagePick != null)                {

                    if (PickProcessMode == ProcessMode.PlaceElementFirst) 
                    {
                        if ((WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).Count() == 1) && (ProcessArea.Where(p => p.PickFinished.Equals(0)).Count() > 0))
                        {
                            Console.WriteLine("工作區仍有處理element, 優先取料");
                        }
                        else
                        {      
                            // only last element 
                            if (StageStatus == StageState.WaitPickOrPlaceOrProcess)
                            {
                                epsonRobot.AE06_PickBlowElements(myWorkingBlowStagePick.Id);
                                Pick2FinishedElementID = myWorkingBlowStagePick.ElementId;
                                myWorkingBlowStagePick.BlowLocationStatus = BlowLocationState.RedayToPlace;
                                myWorkingBlowStagePick.ElementId = 0;

                                if (myWorkingBlowStagePick.Id == 1) { this.Invoke(new Action(() => StageElementId1.Text = "0")); }
                                if (myWorkingBlowStagePick.Id == 2) { this.Invoke(new Action(() => StageElementId2.Text = "0")); }

                                this.Invoke(new Action(() => RobotElementId.Text = Pick2FinishedElementID.ToString()));

                                robotNozzleHaveElement = true;


                                DumpSystemStatus();
                                ContinueActionControl();
                            }


                            epsonRobot.AE05_MovePutElementsPosition((float)-252.433, (float)191.144);
                            var myProcessArea = ProcessArea.Where(p => p.ElementId.Equals(Pick2FinishedElementID)).FirstOrDefault();
                            if (myProcessArea != null)
                            {
                                myProcessArea.PlaceOrThrowFinished = 1;
                                robotNozzleHaveElement = false;
                                this.Invoke(new Action(() => RobotElementId.Text = ""));

                                // 將完成資料寫入  FinishArea
                                ProcessFinishTbl processFinishRow = new ProcessFinishTbl();
                                processFinishRow.Id = myProcessArea.ElementId;
                                processFinishRow.CcdX = 0;
                                processFinishRow.CcdY = 0;
                                processFinishRow.CcdZ = 0;
                                processFinishRow.Finished = 1;
                                processFinishRow.FinishDateTime = DateTime.Now;
                                processFinishRow.Go_NG = myProcessArea.Go_NG;

                                FinishArea.Add(processFinishRow);
                                this.Invoke(new Action(() => RobotElementId.Text = "0"));
                            }
                           
                            ContinueActionControl();
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
                    SpinWait.SpinUntil(()=> (StageStatus == StageState.WaitPickOrPlaceOrProcess), -1);
                }

                else
                {
                    // Check 吹氣區可以 Place 
                    var myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                    //  取  element
                    //  var SelectProcessArea = ProcessArea.Where(p => p.PickFinished.Equals(0)).OrderByDescending(p => p.CcdY).OrderByDescending(p => p.CcdX).FirstOrDefault();
                    var SelectProcessArea = ProcessArea.Where(p => p.PickFinished.Equals(0)).OrderByDescending(p => p.ElementId).FirstOrDefault();
                    
                    if ((SelectProcessArea != null) && (myWorkingBlowStagePlace != null))
                    {
                        #region 有需要捉取 element ,且 Stage 可以 Place
                        Console.WriteLine("StageStatus:" + StageStatus.ToString());
                        TextBoxTextAppendChange("txtLog", "Process Element:" + SelectProcessArea.ElementId);

                        epsonRobot.AE01_MovetoLasermeasurement((float)285.433, (float)-14.416);

                        // 3.LaserMeasurementHeight = Get LaserMeasurement Value
                        SendUdpMessage("AE01.3 LaserMeasurementHeight");
                        AddLog("AE01.3 LaserMeasurementHeight = Get LaserMeasurement Value ");
                        LaserMeasureValue = 10.33;

                        epsonRobot.AE02_CcdMoveDetectionElements((float)285.808, (float)-14.416, (float)-30);

                        // 3.LaserMeasurementHeight = Get LaserMeasurement Value
                        AddLog("AE02.3 Snap and Calcuate ElementPick_X, ElementPick_Y, ElementPick_Z, GO_NG ");
                        
                        ElementPick_X = 11.11;
                        ElementPick_Y = 22.22;
                        ElementPick_Z = 33.33;

                       

                        epsonRobot.AE03_MovetoElementPickAndBlowWaitArea((float)285.808, (float)35.384, (float)-104.416);

                        //  將取走 Element PickFinished 標記為 1                        
                        if (SelectProcessArea != null)
                        {
                            SelectProcessArea.PickFinished = 1;
                            robotNozzleHaveElement = true;
                            // RobotElementId
                            this.Invoke(new Action(() => RobotElementId.Text = SelectProcessArea.ElementId.ToString()));
                        }

                        DumpSystemStatus();                       
                        ContinueActionControl();


                        if (SelectProcessArea.Go_NG == true)
                        {
                            // Second element , wait Stage ready
                            SpinWait.SpinUntil(() => StageStatus == StageState.WaitPickOrPlaceOrProcess, -1 );

                            #region Element_GO process
                            AddLog("1.Wait Blow Stage  if ready ,and no finished");
                            if (StageStatus == StageState.WaitPickOrPlaceOrProcess)
                            {
                                myWorkingBlowStagePick = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPick)).FirstOrDefault();

                                if (myWorkingBlowStagePick == null)
                                {
                                    myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                                    if (myWorkingBlowStagePlace != null)
                                    {
                                        epsonRobot.AE07_PlaceBlowElements(myWorkingBlowStagePlace.Id);
                                        // 將該 Id 狀態改為  WaitProcess
                                        myWorkingBlowStagePlace.ElementId = SelectProcessArea.ElementId;
                                        myWorkingBlowStagePlace.BlowLocationStatus = BlowLocationState.WaitProcess;
                                        
                                        if (myWorkingBlowStagePlace.Id== 1) { this.Invoke(new Action(() => StageElementId1.Text = myWorkingBlowStagePlace.ElementId.ToString())); }
                                        if (myWorkingBlowStagePlace.Id == 2) { this.Invoke(new Action(() => StageElementId2.Text = myWorkingBlowStagePlace.ElementId.ToString())); }
                                        
                                        DumpSystemStatus();
                                        ContinueActionControl();
                                    }
                                }
                                else
                                {
                                     myWorkingBlowStagePlace = WorkingBlowStage.Where(p => p.BlowLocationStatus.Equals(BlowLocationState.RedayToPlace)).FirstOrDefault();
                                    if (myWorkingBlowStagePlace != null)
                                    {
                                        epsonRobot.AE07_PlaceBlowElements(myWorkingBlowStagePlace.Id);
                                        // 將該 Id 狀態改為  WaitProcess
                                        myWorkingBlowStagePlace.ElementId = SelectProcessArea.ElementId;
                                        myWorkingBlowStagePlace.BlowLocationStatus = BlowLocationState.WaitProcess;
                                        
                                        if (myWorkingBlowStagePlace.Id == 1) { this.Invoke(new Action(() => StageElementId1.Text = myWorkingBlowStagePlace.ElementId.ToString())); }
                                        if (myWorkingBlowStagePlace.Id == 2) { this.Invoke(new Action(() => StageElementId2.Text = myWorkingBlowStagePlace.ElementId.ToString())); }
                                        
                                        DumpSystemStatus();
                                        ContinueActionControl();
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Element_NG process
                            epsonRobot.AE04_MovetoNGArea();

                            //  將取走 Drop PlaceOrThrowFinished 標記為 1                           
                            if (SelectProcessArea != null)
                            {
                                SelectProcessArea.PlaceOrThrowFinished = 1;
                                robotNozzleHaveElement = false;

                                // 將完成資料寫入  FinishArea
                                ProcessFinishTbl processFinishRow = new ProcessFinishTbl();
                                processFinishRow.Id = SelectProcessArea.ElementId;
                                processFinishRow.CcdX = 0;
                                processFinishRow.CcdY = 0;
                                processFinishRow.CcdZ = 0;
                                processFinishRow.Finished = 1;
                                processFinishRow.FinishDateTime = DateTime.Now;
                                processFinishRow.Go_NG = SelectProcessArea.Go_NG;
                                FinishArea.Add(processFinishRow);

                                this.Invoke(new Action(() => RobotElementId.Text = "0"));                                
                            }

                           

                            DumpSystemStatus();
                            ContinueActionControl();
                            #endregion
                        }


                        DumpSystemStatus();

                        #endregion
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
                                SendUdpMessage("No element to process, please assign Data " + DateTime.Now.ToString("HH:mm:ss.fff"));
                                SpinWait.SpinUntil(() => false, TimeSpan.FromSeconds(2));
                                SendUdpMessage("No element to process, please assign Data " + DateTime.Now.ToString("HH:mm:ss.fff"));
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
                                SpinWait.SpinUntil(() => false, 2500);
                            }
                        }

                    }
                }
                SpinWait.SpinUntil(() => false, 500);
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

            jsonString = JsonConvert.SerializeObject(FinishArea, Formatting.Indented);
            fileName = Path.Combine(@"c:\temp\", "FinishArea.Json");

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
                    throw ex;
                    //MessageBox.Show(ex.Message);
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
                AddLog("m_spel.Start(1)");
               m_spel.Start(mainid); 
            }
            catch (SpelException ex)
            {
                throw ex;
               // MessageBox.Show(ex.Message);
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
                AddLog("m_spel.Start(1) Finished");
            }
            else
            {
                SendUdpMessage("m_spel.Start(1) Timeout");
                AddLog("m_spel.Start(1) Timeout");
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
            FinishArea.Clear();
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
                throw ex;
               // MessageBox.Show(ex.Message);
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
                throw ex;
               // MessageBox.Show(ex.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ProcessArea = new List<ProcessAreaTbl>
            {
               new ProcessAreaTbl { ElementId=1, CcdX = 10.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=2, CcdX = 20.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=3, CcdX = 30.01, CcdY = 10.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=4, CcdX = 10.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=5, CcdX = 20.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=6, CcdX = 30.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=7, CcdX = 40.01, CcdY = 20.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=8, CcdX = 10.01, CcdY = 30.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=9, CcdX = 20.01, CcdY = 30.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=10, CcdX = 30.01, CcdY = 30.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
                new ProcessAreaTbl { ElementId=11, CcdX = 10.01, CcdY = 40.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=12, CcdX = 20.01, CcdY = 40.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=13, CcdX = 30.01, CcdY = 40.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=14, CcdX = 10.01, CcdY = 50.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=15, CcdX = 20.01, CcdY = 50.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=16, CcdX = 30.01, CcdY = 50.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=17, CcdX = 40.01, CcdY = 60.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=18, CcdX = 10.01, CcdY = 60.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=19, CcdX = 20.01, CcdY = 60.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
               new ProcessAreaTbl { ElementId=20, CcdX = 30.01, CcdY = 70.0, CcdZ= 0.0, PickFinished=0, PlaceOrThrowFinished=0, Go_NG=true},
            };
            FinishArea.Clear();
            Random rnd = new Random();

            for (int i = 0; i < 4; i++)
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

        private void btnCylceRunAssign_Click(object sender, EventArgs e)
        {
            CycleAutoRun = !CycleAutoRun;
            lbleCycleRun.Text = "Cycle Run=" + CycleAutoRun.ToString();
        }

        private void btnMovePutElementsPosition_Click(object sender, EventArgs e)
        {
            float g_CcdX = (float)Convert.ToDouble(txtStep5_g_CcdX.Text);
            float g_CcdY = (float)Convert.ToDouble(txtStep5_g_CcdY.Text);
            epsonRobot.AE05_MovePutElementsPosition(g_CcdX, g_CcdY);
        }

        private void btnIsVirtualModel_Click(object sender, EventArgs e)
        {
            IsVirtualModel = !IsVirtualModel;
            lblIsVirtualModel.Text = "IsVirtualModel=" + IsVirtualModel.ToString();

            try
            {
                m_spel.SetVar("b_IsVirtualModel", IsVirtualModel);
            }
            catch (SpelException ex)
            {
                // throw ex;
                MessageBox.Show(ex.Message);
            }
             
        }
    }

    public enum ProcessMode
    {
        PickBlowFirst =1,
        PlaceElementFirst = 2
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

        public bool Go_NG { get; set; }
        public DateTime FinishDateTime { get; set; }
    }



}
