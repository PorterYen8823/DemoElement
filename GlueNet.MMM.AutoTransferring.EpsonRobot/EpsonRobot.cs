using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using RCAPINet;


namespace GlueNet.MMM.AutoTransferring.EpsonRobot
{
    public class EpsonRobot
    {
        // Debug use
        public IPEndPoint ipep_s = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666); //  特殊訊息
        public IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);   //  一般訊息

        UdpClient uc = new UdpClient();


        // Epson Spel
        static Spel m_spel;

        // 相關參數
        int RobotTimeOutInterval = 15000;


        public ObservableCollection<string> Messages { get; set; }

        public EpsonRobot()
        {
           
            m_spel = new Spel();
            m_spel.ServerInstance = 1;
            Messages = new ObservableCollection<string>();
        }

        ~EpsonRobot()
        {
            m_spel.Dispose();
        }
        public void Dispose()
        {
            m_spel.Dispose();
        }

        public void AE01_SensorMoveGetElements(float ccdX, float ccdY)
        {

            AddLog(" ===== AE01 =====");
            AddLog("AE01.1 Set  g_CcdX,  g_CcdY ");

            try
            {
                m_spel.SetVar("g_CcdX", ccdX);
            }
            catch (SpelException ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", ccdY);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            AddLog("AE01.2 Move to Point LaserMeasurement (Main1)");
            try
            {
                //SendUdpMessage("TasksExecuting:"+ m_spel.TasksExecuting());
                m_spel.Start(1);

            }
            catch (SpelException ex)
            {
                throw ex;
                //   MessageBox.Show(ex.Message);
            }

            SpinWait.SpinUntil(() => (m_spel.MemSw(31) && m_spel.MemSw(32)), RobotTimeOutInterval);

            AddLog("m_spel.Start(1) Finished");
            AddLog("----- AE01 -----");
            AddLog("");


        }

        public void AE02_CcdMoveDetectionElements(float ccdX, float ccdY, float ccdZ)
        {
            AddLog("===== AE02 =====");
            AddLog("AE02.1 Set  g_CcdX,  g_CcdY, g_CcdZ ");
            try
            {
                m_spel.SetVar("g_CcdX", ccdX);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", ccdY);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            try
            {

                m_spel.SetVar("g_CcdZ", ccdY);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            // 2.Move to Point  
            AddLog("AE02.2 Move to Point ElementTop (Main2)");
            int mainid = 2;
            try
            {
                SendUdpMessage(" m_spel.Start(2)");
                m_spel.Start(mainid);

            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }
            SpinWait.SpinUntil(() => (m_spel.MemSw(33) && m_spel.MemSw(34)), RobotTimeOutInterval);

            AddLog("m_spel.Start(2) Finished");
            AddLog("----- AE02 -----");
            AddLog("");

        }

        public void AE03_NozzleMoveGetElement(float g_CcdX, float g_CcdY, float g_CcdZ)
        {
            // 1.Set  g_CcdX,  g_CcdY
            AddLog("===== AE03 =====");
            AddLog("AE03.1 Set  g_CcdX,  g_CcdY, g_CcdZ");
            try
            {
                m_spel.SetVar("g_CcdX", g_CcdX);
            }
            catch (SpelException ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", g_CcdY);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdZ", g_CcdZ);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            // 2.Move to Point 
            AddLog("AE03.2 Move to Point ElementPick (Main3)");

            try
            {
                SendUdpMessage("m_spel.Start(3)");
                m_spel.Start(3);

            }
            catch (SpelException ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
            }

            SpinWait.SpinUntil(() => (m_spel.MemSw(35) && m_spel.MemSw(36)), RobotTimeOutInterval);

            AddLog("m_spel.Start(3) Finished");
            AddLog("----- AE03 -----");
            AddLog("");

        }

        public void AE04_MoveNgPosition()
        {
            AddLog("===== AE04 =====");

            int mainid = 4;
            try
            {
                AddLog("m_spel.Start(4)");
                m_spel.Start(mainid);

            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            SpinWait.SpinUntil(() => (m_spel.MemSw(37) && m_spel.MemSw(38)), RobotTimeOutInterval);

            AddLog("m_spel.Start(4) Finished");
            SendUdpMessage("----- AE04 -----");
            SendUdpMessage("");
        }

        public void AE05_MovePutElementsPosition(float ccdX, float ccdY)
        {
            AddLog("===== AE05 =====");
            AddLog("AE05.1 Assign ccdX, ccdY");
            try
            {
                m_spel.SetVar("g_CcdX", ccdX);
            }
            catch (SpelException ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
            }

            try
            {
                m_spel.SetVar("g_CcdY", ccdY);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            AddLog("AE05.2 MovePutElementsPosition");

            try
            {
                AddLog("m_spel.Start(5)");
                m_spel.Start(5);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            SpinWait.SpinUntil(() => (m_spel.MemSw(39) && m_spel.MemSw(40)), RobotTimeOutInterval);


            AddLog("m_spel.Start(5) Finished");
            AddLog("----- AE05 -----");
            AddLog("");

        }

        public void AE06_PickBlowElements(int StagePickID)
        {
            AddLog("===== AE06 =====");
            AddLog("AE06.1 Stage Pick ID:" + StagePickID.ToString());

            //if (StagePickID == 1) // 位置 1
            //{
            //    // 位置1,2 互斥, 比免兩個同時true
            //    m_spel.MemOn(2);
            //    m_spel.MemOff(3);
            //}
            //if (StagePickID == 2) // 位置 2
            //{
            //    // 位置1,2 互斥, 比免兩個同時true
            //    m_spel.MemOff(2);
            //    m_spel.MemOn(3);
            //}

            try
            {
                m_spel.SetVar("g_BlowElementPosition", StagePickID);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            AddLog("AE06.2 Move to BlowAirPick  (Main6)");

            try
            {

                AddLog("m_spel.Start(6)");
                //     SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                m_spel.Start(6);

            }
            catch (SpelException ex)
            {
                throw ex;
                //MessageBox.Show(ex.Message);
            }
            //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
            SpinWait.SpinUntil(() => (m_spel.MemSw(45)), RobotTimeOutInterval);
            m_spel.MemOn(0);
            SpinWait.SpinUntil(() => (m_spel.MemSw(46)), RobotTimeOutInterval);

            AddLog("m_spel.Start(6) Finished");
            AddLog("----- AE06 -----");
            AddLog("");


        }

        public void AE07_PlaceBlowElements(int StagePlaceId)
        {
            AddLog("===== AE07 =====");
            AddLog("AE07.1 Stage Place Id " + StagePlaceId.ToString());
            //if (StagePlaceId == 1) // 位置 1
            //{
            //    // 位置1,2 互斥, 比免兩個同時true
            //    m_spel.MemOff(3);
            //    m_spel.MemOn(2);

            //}
            //if (StagePlaceId == 2) // 位置 2
            //{
            //    // 位置1,2 互斥, 比免兩個同時true
            //    m_spel.MemOff(2);
            //    m_spel.MemOn(3);
            //}



            try
            {
                m_spel.SetVar("g_BlowElementPosition", StagePlaceId);
            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }

            //Move to Point  
            AddLog("AE07.2 Move to Point PlaceBlowElements (Main7)");

            try
            {

                AddLog("m_spel.Start(7)");
                //    SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
                m_spel.Start(7);

            }
            catch (SpelException ex)
            {
                throw ex;
                // MessageBox.Show(ex.Message);
            }
            //  Main7 'PlaceBlowElements
            //          MoveBlowElementsPosition1or2 --> PutBlowElement_OK
            SpinWait.SpinUntil(() => (m_spel.MemSw(47)), RobotTimeOutInterval);

            if (m_spel.MemSw(47) == false)
            {
                // 逾時處理...#001 待討論
                // 
            }

            // 
            m_spel.MemOn(1);

            SpinWait.SpinUntil(() => (m_spel.MemSw(48)), RobotTimeOutInterval);
            //  SendUdpMessage("TasksExecuting:" + m_spel.TasksExecuting());
            if (m_spel.MemSw(48) == false)
            {
                // 逾時處理...#002 待討論
                // 
            }
            AddLog("m_spel.Start(7) Finished");
            AddLog("----- AE07 -----");
            AddLog("");
        }
        private void AddLog(string msg)
        {
            if (Messages.Count() >100)
            { 
                Messages.Clear();
            }
            Messages.Add(msg);
            SendUdpMessage(msg);
        }

        private void SendUdpMessage(string msg)
        {
            byte[] b = System.Text.Encoding.ASCII.GetBytes(msg);
            uc.Send(b, b.Length, ipep);
        }


       
    }
}
