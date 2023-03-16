namespace Demo1
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRobotManager = new System.Windows.Forms.Button();
            this.btnIOMonitor = new System.Windows.Forms.Button();
            this.btnTaskManager = new System.Windows.Forms.Button();
            this.btnControllerTools = new System.Windows.Forms.Button();
            this.btnProgramMode = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnTeachPoint = new System.Windows.Forms.Button();
            this.gBoxRunTasks = new System.Windows.Forms.GroupBox();
            this.btnCont = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cmbFunc = new System.Windows.Forms.ComboBox();
            this.lblFuncToStart = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblEvents = new System.Windows.Forms.Label();
            this.btnSimulator = new System.Windows.Forms.Button();
            this.btnServoOn = new System.Windows.Forms.Button();
            this.btnServoOff = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnMovetoLaserMeasurement = new System.Windows.Forms.Button();
            this.btnCcdMoveDetectionElements = new System.Windows.Forms.Button();
            this.btnMovetoElementPickAndBlowWaitArea = new System.Windows.Forms.Button();
            this.btnPickBlowElements = new System.Windows.Forms.Button();
            this.btnMovetoBlowAirPlace = new System.Windows.Forms.Button();
            this.BtnMvoeNgPosition = new System.Windows.Forms.Button();
            this.btnMovePutElementsPosition = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStep1_g_CcdX = new System.Windows.Forms.TextBox();
            this.txtStep1_g_CcdY = new System.Windows.Forms.TextBox();
            this.txtStep2_g_CcdY = new System.Windows.Forms.TextBox();
            this.txtStep2_g_CcdX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStep2_g_CcdZ = new System.Windows.Forms.TextBox();
            this.txtStep3_g_CcdZ = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStep3_g_CcdY = new System.Windows.Forms.TextBox();
            this.txtStep3_g_CcdX = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtStep6_id = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtStep7_id = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtStep5_g_CcdY = new System.Windows.Forms.TextBox();
            this.txtStep5_g_CcdX = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnAutoRun = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnContinue = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button8 = new System.Windows.Forms.Button();
            this.btnResetData = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnCylceRunAssign = new System.Windows.Forms.Button();
            this.lbleCycleRun = new System.Windows.Forms.Label();
            this.btnIsVirtualModel = new System.Windows.Forms.Button();
            this.lblIsVirtualModel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStageStatus = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RobotElementId = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.StageElementId2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StageElementId1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gBoxRunTasks.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(315, 13);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 22);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRobotManager
            // 
            this.btnRobotManager.Location = new System.Drawing.Point(315, 50);
            this.btnRobotManager.Name = "btnRobotManager";
            this.btnRobotManager.Size = new System.Drawing.Size(120, 26);
            this.btnRobotManager.TabIndex = 3;
            this.btnRobotManager.Text = "Robot Manager";
            this.btnRobotManager.UseVisualStyleBackColor = true;
            this.btnRobotManager.Click += new System.EventHandler(this.btnRobotManager_Click);
            // 
            // btnIOMonitor
            // 
            this.btnIOMonitor.Location = new System.Drawing.Point(315, 95);
            this.btnIOMonitor.Name = "btnIOMonitor";
            this.btnIOMonitor.Size = new System.Drawing.Size(120, 26);
            this.btnIOMonitor.TabIndex = 4;
            this.btnIOMonitor.Text = "I/O Monitor";
            this.btnIOMonitor.UseVisualStyleBackColor = true;
            this.btnIOMonitor.Click += new System.EventHandler(this.btnIOMonitor_Click);
            // 
            // btnTaskManager
            // 
            this.btnTaskManager.Location = new System.Drawing.Point(315, 138);
            this.btnTaskManager.Name = "btnTaskManager";
            this.btnTaskManager.Size = new System.Drawing.Size(118, 24);
            this.btnTaskManager.TabIndex = 5;
            this.btnTaskManager.Text = "Task Manager";
            this.btnTaskManager.UseVisualStyleBackColor = true;
            this.btnTaskManager.Click += new System.EventHandler(this.btnTaskManager_Click);
            // 
            // btnControllerTools
            // 
            this.btnControllerTools.Location = new System.Drawing.Point(315, 220);
            this.btnControllerTools.Name = "btnControllerTools";
            this.btnControllerTools.Size = new System.Drawing.Size(118, 24);
            this.btnControllerTools.TabIndex = 7;
            this.btnControllerTools.Text = "Controller Tools";
            this.btnControllerTools.UseVisualStyleBackColor = true;
            this.btnControllerTools.Click += new System.EventHandler(this.btnControllerTools_Click);
            // 
            // btnProgramMode
            // 
            this.btnProgramMode.Location = new System.Drawing.Point(315, 262);
            this.btnProgramMode.Name = "btnProgramMode";
            this.btnProgramMode.Size = new System.Drawing.Size(118, 24);
            this.btnProgramMode.TabIndex = 8;
            this.btnProgramMode.Text = "Program Mode";
            this.btnProgramMode.UseVisualStyleBackColor = true;
            this.btnProgramMode.Click += new System.EventHandler(this.btnProgramMode_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(317, 305);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(118, 24);
            this.btnReset.TabIndex = 9;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnTeachPoint
            // 
            this.btnTeachPoint.Location = new System.Drawing.Point(317, 346);
            this.btnTeachPoint.Name = "btnTeachPoint";
            this.btnTeachPoint.Size = new System.Drawing.Size(118, 24);
            this.btnTeachPoint.TabIndex = 10;
            this.btnTeachPoint.Text = "Teach Point";
            this.btnTeachPoint.UseVisualStyleBackColor = true;
            this.btnTeachPoint.Click += new System.EventHandler(this.btnTeachPoint_Click);
            // 
            // gBoxRunTasks
            // 
            this.gBoxRunTasks.Controls.Add(this.btnCont);
            this.gBoxRunTasks.Controls.Add(this.btnStop);
            this.gBoxRunTasks.Controls.Add(this.btnPause);
            this.gBoxRunTasks.Controls.Add(this.btnStart);
            this.gBoxRunTasks.Controls.Add(this.cmbFunc);
            this.gBoxRunTasks.Controls.Add(this.lblFuncToStart);
            this.gBoxRunTasks.Controls.Add(this.textBox1);
            this.gBoxRunTasks.Controls.Add(this.lblEvents);
            this.gBoxRunTasks.Location = new System.Drawing.Point(12, 223);
            this.gBoxRunTasks.Name = "gBoxRunTasks";
            this.gBoxRunTasks.Size = new System.Drawing.Size(284, 286);
            this.gBoxRunTasks.TabIndex = 1;
            this.gBoxRunTasks.TabStop = false;
            this.gBoxRunTasks.Text = "Run Tasks";
            // 
            // btnCont
            // 
            this.btnCont.Location = new System.Drawing.Point(153, 238);
            this.btnCont.Name = "btnCont";
            this.btnCont.Size = new System.Drawing.Size(103, 27);
            this.btnCont.TabIndex = 7;
            this.btnCont.Text = "Continue";
            this.btnCont.UseVisualStyleBackColor = true;
            this.btnCont.Click += new System.EventHandler(this.btnCont_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(22, 238);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(103, 27);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(153, 205);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(103, 27);
            this.btnPause.TabIndex = 5;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(22, 205);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 27);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cmbFunc
            // 
            this.cmbFunc.FormattingEnabled = true;
            this.cmbFunc.Location = new System.Drawing.Point(123, 168);
            this.cmbFunc.Name = "cmbFunc";
            this.cmbFunc.Size = new System.Drawing.Size(133, 21);
            this.cmbFunc.TabIndex = 3;
            // 
            // lblFuncToStart
            // 
            this.lblFuncToStart.AutoSize = true;
            this.lblFuncToStart.Location = new System.Drawing.Point(21, 176);
            this.lblFuncToStart.Name = "lblFuncToStart";
            this.lblFuncToStart.Size = new System.Drawing.Size(86, 13);
            this.lblFuncToStart.TabIndex = 2;
            this.lblFuncToStart.Text = "Function to start:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 42);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(237, 112);
            this.textBox1.TabIndex = 1;
            // 
            // lblEvents
            // 
            this.lblEvents.AutoSize = true;
            this.lblEvents.Location = new System.Drawing.Point(5, 17);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(40, 13);
            this.lblEvents.TabIndex = 0;
            this.lblEvents.Text = "Events";
            // 
            // btnSimulator
            // 
            this.btnSimulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimulator.Location = new System.Drawing.Point(316, 179);
            this.btnSimulator.Name = "btnSimulator";
            this.btnSimulator.Size = new System.Drawing.Size(116, 24);
            this.btnSimulator.TabIndex = 6;
            this.btnSimulator.Text = "Simulator";
            this.btnSimulator.UseVisualStyleBackColor = true;
            this.btnSimulator.Click += new System.EventHandler(this.btnSimulator_Click);
            // 
            // btnServoOn
            // 
            this.btnServoOn.Location = new System.Drawing.Point(13, 194);
            this.btnServoOn.Margin = new System.Windows.Forms.Padding(2);
            this.btnServoOn.Name = "btnServoOn";
            this.btnServoOn.Size = new System.Drawing.Size(84, 24);
            this.btnServoOn.TabIndex = 11;
            this.btnServoOn.Text = "Servo On";
            this.btnServoOn.UseVisualStyleBackColor = true;
            this.btnServoOn.Click += new System.EventHandler(this.btnServoOn_Click);
            // 
            // btnServoOff
            // 
            this.btnServoOff.Location = new System.Drawing.Point(115, 194);
            this.btnServoOff.Margin = new System.Windows.Forms.Padding(2);
            this.btnServoOff.Name = "btnServoOff";
            this.btnServoOff.Size = new System.Drawing.Size(59, 24);
            this.btnServoOff.TabIndex = 12;
            this.btnServoOff.Text = "Servo Off";
            this.btnServoOff.UseVisualStyleBackColor = true;
            this.btnServoOff.Click += new System.EventHandler(this.btnServoOff_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(205, 194);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 24);
            this.button1.TabIndex = 13;
            this.button1.Text = "Test Use";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnMovetoLaserMeasurement
            // 
            this.btnMovetoLaserMeasurement.Location = new System.Drawing.Point(452, 13);
            this.btnMovetoLaserMeasurement.Margin = new System.Windows.Forms.Padding(2);
            this.btnMovetoLaserMeasurement.Name = "btnMovetoLaserMeasurement";
            this.btnMovetoLaserMeasurement.Size = new System.Drawing.Size(161, 34);
            this.btnMovetoLaserMeasurement.TabIndex = 2;
            this.btnMovetoLaserMeasurement.Text = "AE01_SensorMoveGetElements";
            this.btnMovetoLaserMeasurement.UseVisualStyleBackColor = true;
            this.btnMovetoLaserMeasurement.Click += new System.EventHandler(this.btnMovetoLasermeasurement_Click);
            // 
            // btnCcdMoveDetectionElements
            // 
            this.btnCcdMoveDetectionElements.Location = new System.Drawing.Point(452, 63);
            this.btnCcdMoveDetectionElements.Margin = new System.Windows.Forms.Padding(2);
            this.btnCcdMoveDetectionElements.Name = "btnCcdMoveDetectionElements";
            this.btnCcdMoveDetectionElements.Size = new System.Drawing.Size(134, 34);
            this.btnCcdMoveDetectionElements.TabIndex = 21;
            this.btnCcdMoveDetectionElements.Text = "AE02_CcdMoveDetectionElements";
            this.btnCcdMoveDetectionElements.UseVisualStyleBackColor = true;
            this.btnCcdMoveDetectionElements.Click += new System.EventHandler(this.btnMovetoElementTop_Click);
            // 
            // btnMovetoElementPickAndBlowWaitArea
            // 
            this.btnMovetoElementPickAndBlowWaitArea.Location = new System.Drawing.Point(452, 116);
            this.btnMovetoElementPickAndBlowWaitArea.Margin = new System.Windows.Forms.Padding(2);
            this.btnMovetoElementPickAndBlowWaitArea.Name = "btnMovetoElementPickAndBlowWaitArea";
            this.btnMovetoElementPickAndBlowWaitArea.Size = new System.Drawing.Size(145, 34);
            this.btnMovetoElementPickAndBlowWaitArea.TabIndex = 22;
            this.btnMovetoElementPickAndBlowWaitArea.Text = "AE03_NozzleMoveGetElement";
            this.btnMovetoElementPickAndBlowWaitArea.UseVisualStyleBackColor = true;
            this.btnMovetoElementPickAndBlowWaitArea.Click += new System.EventHandler(this.btnMovetoElementPickAndBlowWaitArea_Click);
            // 
            // btnPickBlowElements
            // 
            this.btnPickBlowElements.Location = new System.Drawing.Point(452, 253);
            this.btnPickBlowElements.Margin = new System.Windows.Forms.Padding(2);
            this.btnPickBlowElements.Name = "btnPickBlowElements";
            this.btnPickBlowElements.Size = new System.Drawing.Size(145, 34);
            this.btnPickBlowElements.TabIndex = 23;
            this.btnPickBlowElements.Text = "AE06_PickBlowElements";
            this.btnPickBlowElements.UseVisualStyleBackColor = true;
            this.btnPickBlowElements.Click += new System.EventHandler(this.btnMovetoBlowAirPick_Click);
            // 
            // btnMovetoBlowAirPlace
            // 
            this.btnMovetoBlowAirPlace.Location = new System.Drawing.Point(452, 215);
            this.btnMovetoBlowAirPlace.Margin = new System.Windows.Forms.Padding(2);
            this.btnMovetoBlowAirPlace.Name = "btnMovetoBlowAirPlace";
            this.btnMovetoBlowAirPlace.Size = new System.Drawing.Size(145, 34);
            this.btnMovetoBlowAirPlace.TabIndex = 24;
            this.btnMovetoBlowAirPlace.Text = "AE07_PlaceBlowElements";
            this.btnMovetoBlowAirPlace.UseVisualStyleBackColor = true;
            this.btnMovetoBlowAirPlace.Click += new System.EventHandler(this.btnMovetoBlowAirPlace_Click);
            // 
            // BtnMvoeNgPosition
            // 
            this.BtnMvoeNgPosition.Location = new System.Drawing.Point(452, 163);
            this.BtnMvoeNgPosition.Margin = new System.Windows.Forms.Padding(2);
            this.BtnMvoeNgPosition.Name = "BtnMvoeNgPosition";
            this.BtnMvoeNgPosition.Size = new System.Drawing.Size(113, 34);
            this.BtnMvoeNgPosition.TabIndex = 25;
            this.BtnMvoeNgPosition.Text = "AE04_MoveNgPosition";
            this.BtnMvoeNgPosition.UseVisualStyleBackColor = true;
            this.BtnMvoeNgPosition.Click += new System.EventHandler(this.BtnMovetoNGArea_Click);
            // 
            // btnMovePutElementsPosition
            // 
            this.btnMovePutElementsPosition.Location = new System.Drawing.Point(452, 307);
            this.btnMovePutElementsPosition.Margin = new System.Windows.Forms.Padding(2);
            this.btnMovePutElementsPosition.Name = "btnMovePutElementsPosition";
            this.btnMovePutElementsPosition.Size = new System.Drawing.Size(145, 34);
            this.btnMovePutElementsPosition.TabIndex = 27;
            this.btnMovePutElementsPosition.Text = "AE05_MovePutElementsPosition";
            this.btnMovePutElementsPosition.UseVisualStyleBackColor = true;
            this.btnMovePutElementsPosition.Click += new System.EventHandler(this.btnMovePutElementsPosition_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(618, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "offset X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(715, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "offset Y";
            // 
            // txtStep1_g_CcdX
            // 
            this.txtStep1_g_CcdX.Location = new System.Drawing.Point(661, 19);
            this.txtStep1_g_CcdX.Name = "txtStep1_g_CcdX";
            this.txtStep1_g_CcdX.Size = new System.Drawing.Size(50, 20);
            this.txtStep1_g_CcdX.TabIndex = 15;
            this.txtStep1_g_CcdX.Text = "285.808";
            // 
            // txtStep1_g_CcdY
            // 
            this.txtStep1_g_CcdY.Location = new System.Drawing.Point(764, 19);
            this.txtStep1_g_CcdY.Name = "txtStep1_g_CcdY";
            this.txtStep1_g_CcdY.Size = new System.Drawing.Size(50, 20);
            this.txtStep1_g_CcdY.TabIndex = 31;
            this.txtStep1_g_CcdY.Text = "-14.416";
            // 
            // txtStep2_g_CcdY
            // 
            this.txtStep2_g_CcdY.Location = new System.Drawing.Point(737, 72);
            this.txtStep2_g_CcdY.Name = "txtStep2_g_CcdY";
            this.txtStep2_g_CcdY.Size = new System.Drawing.Size(50, 20);
            this.txtStep2_g_CcdY.TabIndex = 35;
            this.txtStep2_g_CcdY.Text = "-14.416";
            // 
            // txtStep2_g_CcdX
            // 
            this.txtStep2_g_CcdX.Location = new System.Drawing.Point(633, 72);
            this.txtStep2_g_CcdX.Name = "txtStep2_g_CcdX";
            this.txtStep2_g_CcdX.Size = new System.Drawing.Size(50, 20);
            this.txtStep2_g_CcdX.TabIndex = 32;
            this.txtStep2_g_CcdX.Text = "285.808";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(688, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "offset Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(591, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "offset X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(791, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "offset Z";
            // 
            // txtStep2_g_CcdZ
            // 
            this.txtStep2_g_CcdZ.Location = new System.Drawing.Point(832, 72);
            this.txtStep2_g_CcdZ.Name = "txtStep2_g_CcdZ";
            this.txtStep2_g_CcdZ.Size = new System.Drawing.Size(50, 20);
            this.txtStep2_g_CcdZ.TabIndex = 37;
            this.txtStep2_g_CcdZ.Text = "-30.000";
            // 
            // txtStep3_g_CcdZ
            // 
            this.txtStep3_g_CcdZ.Location = new System.Drawing.Point(843, 124);
            this.txtStep3_g_CcdZ.Name = "txtStep3_g_CcdZ";
            this.txtStep3_g_CcdZ.Size = new System.Drawing.Size(50, 20);
            this.txtStep3_g_CcdZ.TabIndex = 43;
            this.txtStep3_g_CcdZ.Text = "-104.416";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(801, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "offset Z";
            // 
            // txtStep3_g_CcdY
            // 
            this.txtStep3_g_CcdY.Location = new System.Drawing.Point(747, 124);
            this.txtStep3_g_CcdY.Name = "txtStep3_g_CcdY";
            this.txtStep3_g_CcdY.Size = new System.Drawing.Size(50, 20);
            this.txtStep3_g_CcdY.TabIndex = 41;
            this.txtStep3_g_CcdY.Text = "35.384";
            // 
            // txtStep3_g_CcdX
            // 
            this.txtStep3_g_CcdX.Location = new System.Drawing.Point(644, 124);
            this.txtStep3_g_CcdX.Name = "txtStep3_g_CcdX";
            this.txtStep3_g_CcdX.Size = new System.Drawing.Size(50, 20);
            this.txtStep3_g_CcdX.TabIndex = 38;
            this.txtStep3_g_CcdX.Text = "285.808";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(699, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "offset Y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(601, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "offset X";
            // 
            // txtStep6_id
            // 
            this.txtStep6_id.Location = new System.Drawing.Point(631, 261);
            this.txtStep6_id.Name = "txtStep6_id";
            this.txtStep6_id.Size = new System.Drawing.Size(50, 20);
            this.txtStep6_id.TabIndex = 44;
            this.txtStep6_id.Text = "1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(607, 263);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Id";
            // 
            // txtStep7_id
            // 
            this.txtStep7_id.Location = new System.Drawing.Point(631, 223);
            this.txtStep7_id.Name = "txtStep7_id";
            this.txtStep7_id.Size = new System.Drawing.Size(50, 20);
            this.txtStep7_id.TabIndex = 46;
            this.txtStep7_id.Text = "1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(607, 225);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 13);
            this.label13.TabIndex = 47;
            this.label13.Text = "Id";
            // 
            // txtStep5_g_CcdY
            // 
            this.txtStep5_g_CcdY.Location = new System.Drawing.Point(753, 315);
            this.txtStep5_g_CcdY.Name = "txtStep5_g_CcdY";
            this.txtStep5_g_CcdY.Size = new System.Drawing.Size(50, 20);
            this.txtStep5_g_CcdY.TabIndex = 51;
            this.txtStep5_g_CcdY.Text = "196.114";
            // 
            // txtStep5_g_CcdX
            // 
            this.txtStep5_g_CcdX.Location = new System.Drawing.Point(653, 315);
            this.txtStep5_g_CcdX.Name = "txtStep5_g_CcdX";
            this.txtStep5_g_CcdX.Size = new System.Drawing.Size(50, 20);
            this.txtStep5_g_CcdX.TabIndex = 48;
            this.txtStep5_g_CcdX.Text = "-252.433";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(705, 317);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(43, 13);
            this.label14.TabIndex = 50;
            this.label14.Text = "offset Y";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(607, 318);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(43, 13);
            this.label15.TabIndex = 49;
            this.label15.Text = "offset X";
            // 
            // btnAutoRun
            // 
            this.btnAutoRun.Location = new System.Drawing.Point(459, 348);
            this.btnAutoRun.Name = "btnAutoRun";
            this.btnAutoRun.Size = new System.Drawing.Size(103, 27);
            this.btnAutoRun.TabIndex = 8;
            this.btnAutoRun.Text = "AutoRun";
            this.btnAutoRun.UseVisualStyleBackColor = true;
            this.btnAutoRun.Click += new System.EventHandler(this.btnAutoRun_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(317, 452);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(531, 103);
            this.txtLog.TabIndex = 8;
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(697, 347);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(2);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(61, 42);
            this.btnContinue.TabIndex = 52;
            this.btnContinue.Text = "Single \r\nContinue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(832, 10);
            this.button8.Margin = new System.Windows.Forms.Padding(2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(77, 23);
            this.button8.TabIndex = 54;
            this.button8.Text = "Cmd Test";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnResetData
            // 
            this.btnResetData.Location = new System.Drawing.Point(566, 347);
            this.btnResetData.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetData.Name = "btnResetData";
            this.btnResetData.Size = new System.Drawing.Size(97, 29);
            this.btnResetData.TabIndex = 55;
            this.btnResetData.Text = "Reset Data";
            this.btnResetData.UseVisualStyleBackColor = true;
            this.btnResetData.Click += new System.EventHandler(this.btnResetData_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(762, 347);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 42);
            this.button2.TabIndex = 56;
            this.button2.Text = "Continue\r\nMode";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(825, 347);
            this.button9.Margin = new System.Windows.Forms.Padding(2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(61, 42);
            this.button9.TabIndex = 57;
            this.button9.Text = "Single\r\nMode";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(731, 419);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(103, 27);
            this.button10.TabIndex = 60;
            this.button10.Text = "Continue";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Enabled = false;
            this.button11.Location = new System.Drawing.Point(623, 419);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(103, 27);
            this.button11.TabIndex = 59;
            this.button11.Text = "Stop";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(515, 419);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(103, 27);
            this.button12.TabIndex = 58;
            this.button12.Text = "Pause";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(566, 380);
            this.button13.Margin = new System.Windows.Forms.Padding(2);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(97, 29);
            this.button13.TabIndex = 61;
            this.button13.Text = "Assgin Data";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // btnAddRow
            // 
            this.btnAddRow.Location = new System.Drawing.Point(729, 393);
            this.btnAddRow.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(105, 20);
            this.btnAddRow.TabIndex = 62;
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.UseVisualStyleBackColor = true;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnCylceRunAssign
            // 
            this.btnCylceRunAssign.Location = new System.Drawing.Point(776, 256);
            this.btnCylceRunAssign.Margin = new System.Windows.Forms.Padding(2);
            this.btnCylceRunAssign.Name = "btnCylceRunAssign";
            this.btnCylceRunAssign.Size = new System.Drawing.Size(105, 20);
            this.btnCylceRunAssign.TabIndex = 63;
            this.btnCylceRunAssign.Text = "Cycle Run";
            this.btnCylceRunAssign.UseVisualStyleBackColor = true;
            this.btnCylceRunAssign.Click += new System.EventHandler(this.btnCylceRunAssign_Click);
            // 
            // lbleCycleRun
            // 
            this.lbleCycleRun.AutoSize = true;
            this.lbleCycleRun.Location = new System.Drawing.Point(783, 278);
            this.lbleCycleRun.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbleCycleRun.Name = "lbleCycleRun";
            this.lbleCycleRun.Size = new System.Drawing.Size(87, 13);
            this.lbleCycleRun.TabIndex = 64;
            this.lbleCycleRun.Text = "Cycle Run=False";
            // 
            // btnIsVirtualModel
            // 
            this.btnIsVirtualModel.Location = new System.Drawing.Point(743, 184);
            this.btnIsVirtualModel.Margin = new System.Windows.Forms.Padding(2);
            this.btnIsVirtualModel.Name = "btnIsVirtualModel";
            this.btnIsVirtualModel.Size = new System.Drawing.Size(101, 19);
            this.btnIsVirtualModel.TabIndex = 65;
            this.btnIsVirtualModel.Text = "IsVirtualModel";
            this.btnIsVirtualModel.UseVisualStyleBackColor = true;
            this.btnIsVirtualModel.Click += new System.EventHandler(this.btnIsVirtualModel_Click);
            // 
            // lblIsVirtualModel
            // 
            this.lblIsVirtualModel.AutoSize = true;
            this.lblIsVirtualModel.Location = new System.Drawing.Point(751, 205);
            this.lblIsVirtualModel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIsVirtualModel.Name = "lblIsVirtualModel";
            this.lblIsVirtualModel.Size = new System.Drawing.Size(104, 13);
            this.lblIsVirtualModel.TabIndex = 66;
            this.lblIsVirtualModel.Text = "IsVirtualModel=False";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "StageStatus";
            // 
            // lblStageStatus
            // 
            this.lblStageStatus.AutoSize = true;
            this.lblStageStatus.Location = new System.Drawing.Point(120, 0);
            this.lblStageStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStageStatus.Name = "lblStageStatus";
            this.lblStageStatus.Size = new System.Drawing.Size(41, 13);
            this.lblStageStatus.TabIndex = 2;
            this.lblStageStatus.Text = "label16";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.RobotElementId, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label16, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.StageElementId2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.StageElementId1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStageStatus, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(31, 8);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(236, 81);
            this.tableLayoutPanel1.TabIndex = 67;
            // 
            // RobotElementId
            // 
            this.RobotElementId.AutoSize = true;
            this.RobotElementId.Location = new System.Drawing.Point(120, 48);
            this.RobotElementId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RobotElementId.Name = "RobotElementId";
            this.RobotElementId.Size = new System.Drawing.Size(16, 13);
            this.RobotElementId.TabIndex = 8;
            this.RobotElementId.Text = "0 ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(2, 48);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "Robot Element ID ";
            // 
            // StageElementId2
            // 
            this.StageElementId2.AutoSize = true;
            this.StageElementId2.Location = new System.Drawing.Point(120, 32);
            this.StageElementId2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StageElementId2.Name = "StageElementId2";
            this.StageElementId2.Size = new System.Drawing.Size(16, 13);
            this.StageElementId2.TabIndex = 6;
            this.StageElementId2.Text = "0 ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "#2 Element ID ";
            // 
            // StageElementId1
            // 
            this.StageElementId1.AutoSize = true;
            this.StageElementId1.Location = new System.Drawing.Point(120, 16);
            this.StageElementId1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StageElementId1.Name = "StageElementId1";
            this.StageElementId1.Size = new System.Drawing.Size(16, 13);
            this.StageElementId1.TabIndex = 4;
            this.StageElementId1.Text = "0 ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "#1 Element ID ";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 546);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblIsVirtualModel);
            this.Controls.Add(this.btnIsVirtualModel);
            this.Controls.Add(this.lbleCycleRun);
            this.Controls.Add(this.btnCylceRunAssign);
            this.Controls.Add(this.btnAddRow);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnResetData);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnAutoRun);
            this.Controls.Add(this.txtStep5_g_CcdY);
            this.Controls.Add(this.txtStep5_g_CcdX);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtStep7_id);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtStep6_id);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtStep3_g_CcdZ);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtStep3_g_CcdY);
            this.Controls.Add(this.txtStep3_g_CcdX);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtStep2_g_CcdZ);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtStep2_g_CcdY);
            this.Controls.Add(this.txtStep2_g_CcdX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtStep1_g_CcdY);
            this.Controls.Add(this.txtStep1_g_CcdX);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnMovePutElementsPosition);
            this.Controls.Add(this.BtnMvoeNgPosition);
            this.Controls.Add(this.btnMovetoBlowAirPlace);
            this.Controls.Add(this.btnPickBlowElements);
            this.Controls.Add(this.btnMovetoElementPickAndBlowWaitArea);
            this.Controls.Add(this.btnCcdMoveDetectionElements);
            this.Controls.Add(this.btnMovetoLaserMeasurement);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnServoOff);
            this.Controls.Add(this.btnServoOn);
            this.Controls.Add(this.btnSimulator);
            this.Controls.Add(this.gBoxRunTasks);
            this.Controls.Add(this.btnTeachPoint);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnProgramMode);
            this.Controls.Add(this.btnControllerTools);
            this.Controls.Add(this.btnTaskManager);
            this.Controls.Add(this.btnIOMonitor);
            this.Controls.Add(this.btnRobotManager);
            this.Controls.Add(this.btnExit);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EPSON RC+ 7.0 C# API Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gBoxRunTasks.ResumeLayout(false);
            this.gBoxRunTasks.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRobotManager;
        private System.Windows.Forms.Button btnIOMonitor;
        private System.Windows.Forms.Button btnTaskManager;
        private System.Windows.Forms.Button btnControllerTools;
        private System.Windows.Forms.Button btnProgramMode;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnTeachPoint;
        private System.Windows.Forms.GroupBox gBoxRunTasks;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.ComboBox cmbFunc;
        private System.Windows.Forms.Label lblFuncToStart;
        private System.Windows.Forms.Button btnCont;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.TextBox textBox1;
        internal System.Windows.Forms.Button btnSimulator;
        private System.Windows.Forms.Button btnServoOn;
        private System.Windows.Forms.Button btnServoOff;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnMovetoLaserMeasurement;
        private System.Windows.Forms.Button btnCcdMoveDetectionElements;
        private System.Windows.Forms.Button btnMovetoElementPickAndBlowWaitArea;
        private System.Windows.Forms.Button btnPickBlowElements;
        private System.Windows.Forms.Button btnMovetoBlowAirPlace;
        private System.Windows.Forms.Button BtnMvoeNgPosition;
        private System.Windows.Forms.Button btnMovePutElementsPosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStep1_g_CcdX;
        private System.Windows.Forms.TextBox txtStep1_g_CcdY;
        private System.Windows.Forms.TextBox txtStep2_g_CcdY;
        private System.Windows.Forms.TextBox txtStep2_g_CcdX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStep2_g_CcdZ;
        private System.Windows.Forms.TextBox txtStep3_g_CcdZ;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStep3_g_CcdY;
        private System.Windows.Forms.TextBox txtStep3_g_CcdX;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtStep6_id;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtStep7_id;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtStep5_g_CcdY;
        private System.Windows.Forms.TextBox txtStep5_g_CcdX;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnAutoRun;
        public System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnResetData;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.Button btnCylceRunAssign;
        private System.Windows.Forms.Label lbleCycleRun;
        private System.Windows.Forms.Button btnIsVirtualModel;
        private System.Windows.Forms.Label lblIsVirtualModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStageStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label RobotElementId;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label StageElementId2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label StageElementId1;
        private System.Windows.Forms.Label label1;
    }
}

