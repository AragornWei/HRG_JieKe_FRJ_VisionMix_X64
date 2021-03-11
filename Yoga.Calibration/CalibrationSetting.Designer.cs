namespace Yoga.Calibration
{
    partial class CalibrationSetting
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.hWndUnit1 = new Yoga.ImageControl.HWndUnit();
            this.tabCtrl_Func = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_savePlanPose = new System.Windows.Forms.Button();
            this.txt_planPose = new System.Windows.Forms.TextBox();
            this.gb_SettingPlanPose = new System.Windows.Forms.GroupBox();
            this.btn_CalibartionPlan = new System.Windows.Forms.Button();
            this.gb_StartCalibration = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_calibrateCamParam = new System.Windows.Forms.Button();
            this.cbb_CalibImages = new System.Windows.Forms.ComboBox();
            this.btn_grabCamera = new System.Windows.Forms.Button();
            this.ckb_StartCalibration = new System.Windows.Forms.CheckBox();
            this.btn_openDescrpPath = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_descpPath = new System.Windows.Forms.TextBox();
            this.btn_saveParam = new System.Windows.Forms.Button();
            this.gB_InitCamParam = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nUpDown_ImageHeight = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nUpDown_ImageWidth = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.NUpDown_Sy = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.NUpDown_Sx = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.NUpDown_Focus = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_CameraParam = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.txt_Col = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_Row = new System.Windows.Forms.TextBox();
            this.gB_worldPoseEnable = new System.Windows.Forms.GroupBox();
            this.btn_grabImage = new System.Windows.Forms.Button();
            this.btn_SetModelImage = new System.Windows.Forms.Button();
            this.btn_SaveWorldPose = new System.Windows.Forms.Button();
            this.btn_CalibrationCoord = new System.Windows.Forms.Button();
            this.txt_WorldPose = new System.Windows.Forms.TextBox();
            this.groupBoxPointPos = new System.Windows.Forms.GroupBox();
            this.rdbtn9 = new System.Windows.Forms.RadioButton();
            this.rdbtn8 = new System.Windows.Forms.RadioButton();
            this.rdbtn7 = new System.Windows.Forms.RadioButton();
            this.rdbtn6 = new System.Windows.Forms.RadioButton();
            this.rdbtn5 = new System.Windows.Forms.RadioButton();
            this.rdbtn4 = new System.Windows.Forms.RadioButton();
            this.rdbtn3 = new System.Windows.Forms.RadioButton();
            this.rdbtn2 = new System.Windows.Forms.RadioButton();
            this.rdbtn1 = new System.Windows.Forms.RadioButton();
            this.dGV_worldPose = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_Verify_Y = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Verify_X = new System.Windows.Forms.TextBox();
            this.txt_log = new System.Windows.Forms.TextBox();
            this.btn_GrabImageForTest = new System.Windows.Forms.Button();
            this.tabCtrl_Func.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gb_SettingPlanPose.SuspendLayout();
            this.gb_StartCalibration.SuspendLayout();
            this.gB_InitCamParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Focus)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.gB_worldPoseEnable.SuspendLayout();
            this.groupBoxPointPos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_worldPose)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWndUnit1
            // 
            this.hWndUnit1.BackColor = System.Drawing.SystemColors.Control;
            this.hWndUnit1.CameraMessage = null;
            this.hWndUnit1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.hWndUnit1.Location = new System.Drawing.Point(8, 66);
            this.hWndUnit1.MinimumSize = new System.Drawing.Size(10, 10);
            this.hWndUnit1.Name = "hWndUnit1";
            this.hWndUnit1.Size = new System.Drawing.Size(659, 561);
            this.hWndUnit1.TabIndex = 10;
            // 
            // tabCtrl_Func
            // 
            this.tabCtrl_Func.Controls.Add(this.tabPage1);
            this.tabCtrl_Func.Controls.Add(this.tabPage2);
            this.tabCtrl_Func.Controls.Add(this.tabPage3);
            this.tabCtrl_Func.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabCtrl_Func.Location = new System.Drawing.Point(673, 66);
            this.tabCtrl_Func.Name = "tabCtrl_Func";
            this.tabCtrl_Func.SelectedIndex = 0;
            this.tabCtrl_Func.Size = new System.Drawing.Size(564, 533);
            this.tabCtrl_Func.TabIndex = 11;
            this.tabCtrl_Func.SelectedIndexChanged += new System.EventHandler(this.tabCtrl_Func_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.btn_savePlanPose);
            this.tabPage1.Controls.Add(this.txt_planPose);
            this.tabPage1.Controls.Add(this.gb_SettingPlanPose);
            this.tabPage1.Controls.Add(this.gb_StartCalibration);
            this.tabPage1.Controls.Add(this.ckb_StartCalibration);
            this.tabPage1.Controls.Add(this.btn_openDescrpPath);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txt_descpPath);
            this.tabPage1.Controls.Add(this.btn_saveParam);
            this.tabPage1.Controls.Add(this.gB_InitCamParam);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txt_CameraParam);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(556, 505);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "相机内参";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(10, 416);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 16);
            this.label13.TabIndex = 109;
            this.label13.Text = "参考平面姿势：";
            // 
            // btn_savePlanPose
            // 
            this.btn_savePlanPose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_savePlanPose.Location = new System.Drawing.Point(434, 435);
            this.btn_savePlanPose.Name = "btn_savePlanPose";
            this.btn_savePlanPose.Size = new System.Drawing.Size(106, 55);
            this.btn_savePlanPose.TabIndex = 108;
            this.btn_savePlanPose.Text = "保存参考平面   (世界坐标系)";
            this.btn_savePlanPose.UseVisualStyleBackColor = true;
            this.btn_savePlanPose.Click += new System.EventHandler(this.btn_SaveWorldPose_Click);
            // 
            // txt_planPose
            // 
            this.txt_planPose.Location = new System.Drawing.Point(9, 435);
            this.txt_planPose.Multiline = true;
            this.txt_planPose.Name = "txt_planPose";
            this.txt_planPose.Size = new System.Drawing.Size(419, 55);
            this.txt_planPose.TabIndex = 106;
            // 
            // gb_SettingPlanPose
            // 
            this.gb_SettingPlanPose.Controls.Add(this.btn_CalibartionPlan);
            this.gb_SettingPlanPose.Location = new System.Drawing.Point(374, 102);
            this.gb_SettingPlanPose.Name = "gb_SettingPlanPose";
            this.gb_SettingPlanPose.Size = new System.Drawing.Size(136, 219);
            this.gb_SettingPlanPose.TabIndex = 105;
            this.gb_SettingPlanPose.TabStop = false;
            // 
            // btn_CalibartionPlan
            // 
            this.btn_CalibartionPlan.BackColor = System.Drawing.Color.Transparent;
            this.btn_CalibartionPlan.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CalibartionPlan.Location = new System.Drawing.Point(6, 82);
            this.btn_CalibartionPlan.Name = "btn_CalibartionPlan";
            this.btn_CalibartionPlan.Size = new System.Drawing.Size(121, 55);
            this.btn_CalibartionPlan.TabIndex = 99;
            this.btn_CalibartionPlan.Text = "设为参考平面";
            this.btn_CalibartionPlan.UseVisualStyleBackColor = false;
            this.btn_CalibartionPlan.Click += new System.EventHandler(this.btn_CalibartionPlan_Click);
            // 
            // gb_StartCalibration
            // 
            this.gb_StartCalibration.Controls.Add(this.label12);
            this.gb_StartCalibration.Controls.Add(this.btn_calibrateCamParam);
            this.gb_StartCalibration.Controls.Add(this.cbb_CalibImages);
            this.gb_StartCalibration.Controls.Add(this.btn_grabCamera);
            this.gb_StartCalibration.Location = new System.Drawing.Point(218, 102);
            this.gb_StartCalibration.Name = "gb_StartCalibration";
            this.gb_StartCalibration.Size = new System.Drawing.Size(136, 219);
            this.gb_StartCalibration.TabIndex = 104;
            this.gb_StartCalibration.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(6, 89);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 106;
            this.label12.Text = "图像序号";
            // 
            // btn_calibrateCamParam
            // 
            this.btn_calibrateCamParam.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_calibrateCamParam.Location = new System.Drawing.Point(6, 141);
            this.btn_calibrateCamParam.Name = "btn_calibrateCamParam";
            this.btn_calibrateCamParam.Size = new System.Drawing.Size(121, 55);
            this.btn_calibrateCamParam.TabIndex = 99;
            this.btn_calibrateCamParam.Text = "标定相机参数";
            this.btn_calibrateCamParam.UseVisualStyleBackColor = true;
            this.btn_calibrateCamParam.Click += new System.EventHandler(this.btn_calibrateCamParam_Click);
            // 
            // cbb_CalibImages
            // 
            this.cbb_CalibImages.FormattingEnabled = true;
            this.cbb_CalibImages.Location = new System.Drawing.Point(6, 108);
            this.cbb_CalibImages.Name = "cbb_CalibImages";
            this.cbb_CalibImages.Size = new System.Drawing.Size(121, 22);
            this.cbb_CalibImages.TabIndex = 100;
            this.cbb_CalibImages.SelectedIndexChanged += new System.EventHandler(this.cbb_CalibImages_SelectedIndexChanged);
            // 
            // btn_grabCamera
            // 
            this.btn_grabCamera.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_grabCamera.Location = new System.Drawing.Point(6, 22);
            this.btn_grabCamera.Name = "btn_grabCamera";
            this.btn_grabCamera.Size = new System.Drawing.Size(121, 55);
            this.btn_grabCamera.TabIndex = 98;
            this.btn_grabCamera.Text = "采集图像并识别";
            this.btn_grabCamera.UseVisualStyleBackColor = true;
            this.btn_grabCamera.Click += new System.EventHandler(this.btn_grabCamera_Click);
            // 
            // ckb_StartCalibration
            // 
            this.ckb_StartCalibration.AutoSize = true;
            this.ckb_StartCalibration.Location = new System.Drawing.Point(225, 80);
            this.ckb_StartCalibration.Name = "ckb_StartCalibration";
            this.ckb_StartCalibration.Size = new System.Drawing.Size(82, 18);
            this.ckb_StartCalibration.TabIndex = 103;
            this.ckb_StartCalibration.Text = "开始标定";
            this.ckb_StartCalibration.UseVisualStyleBackColor = true;
            this.ckb_StartCalibration.CheckedChanged += new System.EventHandler(this.ckb_StartCalibration_CheckedChanged);
            // 
            // btn_openDescrpPath
            // 
            this.btn_openDescrpPath.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_openDescrpPath.Location = new System.Drawing.Point(457, 26);
            this.btn_openDescrpPath.Name = "btn_openDescrpPath";
            this.btn_openDescrpPath.Size = new System.Drawing.Size(83, 41);
            this.btn_openDescrpPath.TabIndex = 102;
            this.btn_openDescrpPath.Text = "...";
            this.btn_openDescrpPath.UseVisualStyleBackColor = true;
            this.btn_openDescrpPath.Click += new System.EventHandler(this.btn_openDescrpPath_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(11, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 16);
            this.label7.TabIndex = 101;
            this.label7.Text = "描述文件路径：";
            // 
            // txt_descpPath
            // 
            this.txt_descpPath.Location = new System.Drawing.Point(10, 26);
            this.txt_descpPath.Multiline = true;
            this.txt_descpPath.Name = "txt_descpPath";
            this.txt_descpPath.Size = new System.Drawing.Size(441, 41);
            this.txt_descpPath.TabIndex = 100;
            // 
            // btn_saveParam
            // 
            this.btn_saveParam.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_saveParam.Location = new System.Drawing.Point(434, 348);
            this.btn_saveParam.Name = "btn_saveParam";
            this.btn_saveParam.Size = new System.Drawing.Size(106, 55);
            this.btn_saveParam.TabIndex = 97;
            this.btn_saveParam.Text = "保存相机参数";
            this.btn_saveParam.UseVisualStyleBackColor = true;
            this.btn_saveParam.Click += new System.EventHandler(this.btn_saveParam_Click);
            // 
            // gB_InitCamParam
            // 
            this.gB_InitCamParam.Controls.Add(this.label2);
            this.gB_InitCamParam.Controls.Add(this.nUpDown_ImageHeight);
            this.gB_InitCamParam.Controls.Add(this.label3);
            this.gB_InitCamParam.Controls.Add(this.nUpDown_ImageWidth);
            this.gB_InitCamParam.Controls.Add(this.label4);
            this.gB_InitCamParam.Controls.Add(this.NUpDown_Sy);
            this.gB_InitCamParam.Controls.Add(this.label5);
            this.gB_InitCamParam.Controls.Add(this.NUpDown_Sx);
            this.gB_InitCamParam.Controls.Add(this.label6);
            this.gB_InitCamParam.Controls.Add(this.NUpDown_Focus);
            this.gB_InitCamParam.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gB_InitCamParam.Location = new System.Drawing.Point(13, 92);
            this.gB_InitCamParam.Name = "gB_InitCamParam";
            this.gB_InitCamParam.Size = new System.Drawing.Size(195, 229);
            this.gB_InitCamParam.TabIndex = 96;
            this.gB_InitCamParam.TabStop = false;
            this.gB_InitCamParam.Text = "相机初始参数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "焦距mm";
            // 
            // nUpDown_ImageHeight
            // 
            this.nUpDown_ImageHeight.Location = new System.Drawing.Point(94, 180);
            this.nUpDown_ImageHeight.Margin = new System.Windows.Forms.Padding(4);
            this.nUpDown_ImageHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nUpDown_ImageHeight.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nUpDown_ImageHeight.Name = "nUpDown_ImageHeight";
            this.nUpDown_ImageHeight.Size = new System.Drawing.Size(77, 26);
            this.nUpDown_ImageHeight.TabIndex = 95;
            this.nUpDown_ImageHeight.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.nUpDown_ImageHeight.ValueChanged += new System.EventHandler(this.nUpDown_ImageHeight_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(15, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sx(um)";
            // 
            // nUpDown_ImageWidth
            // 
            this.nUpDown_ImageWidth.Location = new System.Drawing.Point(94, 143);
            this.nUpDown_ImageWidth.Margin = new System.Windows.Forms.Padding(4);
            this.nUpDown_ImageWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nUpDown_ImageWidth.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nUpDown_ImageWidth.Name = "nUpDown_ImageWidth";
            this.nUpDown_ImageWidth.Size = new System.Drawing.Size(77, 26);
            this.nUpDown_ImageWidth.TabIndex = 94;
            this.nUpDown_ImageWidth.Value = new decimal(new int[] {
            2448,
            0,
            0,
            0});
            this.nUpDown_ImageWidth.ValueChanged += new System.EventHandler(this.nUpDown_ImageWidth_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(16, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Sy(um)";
            // 
            // NUpDown_Sy
            // 
            this.NUpDown_Sy.DecimalPlaces = 2;
            this.NUpDown_Sy.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NUpDown_Sy.Location = new System.Drawing.Point(94, 108);
            this.NUpDown_Sy.Margin = new System.Windows.Forms.Padding(4);
            this.NUpDown_Sy.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUpDown_Sy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUpDown_Sy.Name = "NUpDown_Sy";
            this.NUpDown_Sy.Size = new System.Drawing.Size(77, 26);
            this.NUpDown_Sy.TabIndex = 93;
            this.NUpDown_Sy.Value = new decimal(new int[] {
            345,
            0,
            0,
            131072});
            this.NUpDown_Sy.ValueChanged += new System.EventHandler(this.NUpDown_Sy_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(16, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "像长";
            // 
            // NUpDown_Sx
            // 
            this.NUpDown_Sx.DecimalPlaces = 2;
            this.NUpDown_Sx.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NUpDown_Sx.Location = new System.Drawing.Point(94, 73);
            this.NUpDown_Sx.Margin = new System.Windows.Forms.Padding(4);
            this.NUpDown_Sx.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUpDown_Sx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUpDown_Sx.Name = "NUpDown_Sx";
            this.NUpDown_Sx.Size = new System.Drawing.Size(77, 26);
            this.NUpDown_Sx.TabIndex = 92;
            this.NUpDown_Sx.Value = new decimal(new int[] {
            345,
            0,
            0,
            131072});
            this.NUpDown_Sx.ValueChanged += new System.EventHandler(this.NUpDown_Sx_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(16, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "像宽";
            // 
            // NUpDown_Focus
            // 
            this.NUpDown_Focus.Location = new System.Drawing.Point(94, 34);
            this.NUpDown_Focus.Margin = new System.Windows.Forms.Padding(4);
            this.NUpDown_Focus.Maximum = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.NUpDown_Focus.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.NUpDown_Focus.Name = "NUpDown_Focus";
            this.NUpDown_Focus.Size = new System.Drawing.Size(77, 26);
            this.NUpDown_Focus.TabIndex = 91;
            this.NUpDown_Focus.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.NUpDown_Focus.ValueChanged += new System.EventHandler(this.NUpDown_Focus_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 331);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "相机参数：";
            // 
            // txt_CameraParam
            // 
            this.txt_CameraParam.Location = new System.Drawing.Point(9, 348);
            this.txt_CameraParam.Multiline = true;
            this.txt_CameraParam.Name = "txt_CameraParam";
            this.txt_CameraParam.Size = new System.Drawing.Size(419, 55);
            this.txt_CameraParam.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txt_Col);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txt_Row);
            this.tabPage2.Controls.Add(this.gB_worldPoseEnable);
            this.tabPage2.Controls.Add(this.txt_WorldPose);
            this.tabPage2.Controls.Add(this.groupBoxPointPos);
            this.tabPage2.Controls.Add(this.dGV_worldPose);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(556, 505);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "世界坐标系";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(230, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 108;
            this.label9.Text = "Column: ";
            // 
            // txt_Col
            // 
            this.txt_Col.Location = new System.Drawing.Point(296, 202);
            this.txt_Col.Name = "txt_Col";
            this.txt_Col.Size = new System.Drawing.Size(163, 23);
            this.txt_Col.TabIndex = 107;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 208);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 14);
            this.label8.TabIndex = 106;
            this.label8.Text = "Row: ";
            // 
            // txt_Row
            // 
            this.txt_Row.Location = new System.Drawing.Point(49, 203);
            this.txt_Row.Name = "txt_Row";
            this.txt_Row.Size = new System.Drawing.Size(161, 23);
            this.txt_Row.TabIndex = 105;
            // 
            // gB_worldPoseEnable
            // 
            this.gB_worldPoseEnable.Controls.Add(this.btn_grabImage);
            this.gB_worldPoseEnable.Controls.Add(this.btn_SetModelImage);
            this.gB_worldPoseEnable.Controls.Add(this.btn_SaveWorldPose);
            this.gB_worldPoseEnable.Controls.Add(this.btn_CalibrationCoord);
            this.gB_worldPoseEnable.Location = new System.Drawing.Point(221, 6);
            this.gB_worldPoseEnable.Name = "gB_worldPoseEnable";
            this.gB_worldPoseEnable.Size = new System.Drawing.Size(297, 106);
            this.gB_worldPoseEnable.TabIndex = 104;
            this.gB_worldPoseEnable.TabStop = false;
            // 
            // btn_grabImage
            // 
            this.btn_grabImage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_grabImage.Location = new System.Drawing.Point(12, 15);
            this.btn_grabImage.Name = "btn_grabImage";
            this.btn_grabImage.Size = new System.Drawing.Size(121, 36);
            this.btn_grabImage.TabIndex = 99;
            this.btn_grabImage.Text = "采集图像";
            this.btn_grabImage.UseVisualStyleBackColor = true;
            this.btn_grabImage.Click += new System.EventHandler(this.btn_grabImage_Click);
            // 
            // btn_SetModelImage
            // 
            this.btn_SetModelImage.Enabled = false;
            this.btn_SetModelImage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SetModelImage.Location = new System.Drawing.Point(166, 15);
            this.btn_SetModelImage.Name = "btn_SetModelImage";
            this.btn_SetModelImage.Size = new System.Drawing.Size(121, 36);
            this.btn_SetModelImage.TabIndex = 100;
            this.btn_SetModelImage.Text = "设为模板";
            this.btn_SetModelImage.UseVisualStyleBackColor = true;
            this.btn_SetModelImage.Click += new System.EventHandler(this.btn_SetModelImage_Click);
            // 
            // btn_SaveWorldPose
            // 
            this.btn_SaveWorldPose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SaveWorldPose.Location = new System.Drawing.Point(166, 58);
            this.btn_SaveWorldPose.Name = "btn_SaveWorldPose";
            this.btn_SaveWorldPose.Size = new System.Drawing.Size(121, 36);
            this.btn_SaveWorldPose.TabIndex = 102;
            this.btn_SaveWorldPose.Text = "保存世界坐标系";
            this.btn_SaveWorldPose.UseVisualStyleBackColor = true;
            this.btn_SaveWorldPose.Click += new System.EventHandler(this.btn_SaveWorldPose_Click);
            // 
            // btn_CalibrationCoord
            // 
            this.btn_CalibrationCoord.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CalibrationCoord.Location = new System.Drawing.Point(12, 58);
            this.btn_CalibrationCoord.Name = "btn_CalibrationCoord";
            this.btn_CalibrationCoord.Size = new System.Drawing.Size(121, 36);
            this.btn_CalibrationCoord.TabIndex = 101;
            this.btn_CalibrationCoord.Text = "标定世界坐标系";
            this.btn_CalibrationCoord.UseVisualStyleBackColor = true;
            this.btn_CalibrationCoord.Click += new System.EventHandler(this.btn_CalibrationCoord_Click);
            // 
            // txt_WorldPose
            // 
            this.txt_WorldPose.Location = new System.Drawing.Point(221, 118);
            this.txt_WorldPose.Multiline = true;
            this.txt_WorldPose.Name = "txt_WorldPose";
            this.txt_WorldPose.Size = new System.Drawing.Size(300, 66);
            this.txt_WorldPose.TabIndex = 103;
            // 
            // groupBoxPointPos
            // 
            this.groupBoxPointPos.Controls.Add(this.rdbtn9);
            this.groupBoxPointPos.Controls.Add(this.rdbtn8);
            this.groupBoxPointPos.Controls.Add(this.rdbtn7);
            this.groupBoxPointPos.Controls.Add(this.rdbtn6);
            this.groupBoxPointPos.Controls.Add(this.rdbtn5);
            this.groupBoxPointPos.Controls.Add(this.rdbtn4);
            this.groupBoxPointPos.Controls.Add(this.rdbtn3);
            this.groupBoxPointPos.Controls.Add(this.rdbtn2);
            this.groupBoxPointPos.Controls.Add(this.rdbtn1);
            this.groupBoxPointPos.Location = new System.Drawing.Point(6, 16);
            this.groupBoxPointPos.Name = "groupBoxPointPos";
            this.groupBoxPointPos.Size = new System.Drawing.Size(187, 168);
            this.groupBoxPointPos.TabIndex = 86;
            this.groupBoxPointPos.TabStop = false;
            this.groupBoxPointPos.Text = "点位选择";
            // 
            // rdbtn9
            // 
            this.rdbtn9.AutoSize = true;
            this.rdbtn9.Location = new System.Drawing.Point(132, 124);
            this.rdbtn9.Name = "rdbtn9";
            this.rdbtn9.Size = new System.Drawing.Size(32, 18);
            this.rdbtn9.TabIndex = 8;
            this.rdbtn9.TabStop = true;
            this.rdbtn9.Text = "9";
            this.rdbtn9.UseVisualStyleBackColor = true;
            this.rdbtn9.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn8
            // 
            this.rdbtn8.AutoSize = true;
            this.rdbtn8.Location = new System.Drawing.Point(78, 124);
            this.rdbtn8.Name = "rdbtn8";
            this.rdbtn8.Size = new System.Drawing.Size(32, 18);
            this.rdbtn8.TabIndex = 7;
            this.rdbtn8.TabStop = true;
            this.rdbtn8.Text = "8";
            this.rdbtn8.UseVisualStyleBackColor = true;
            this.rdbtn8.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn7
            // 
            this.rdbtn7.AutoSize = true;
            this.rdbtn7.Location = new System.Drawing.Point(24, 124);
            this.rdbtn7.Name = "rdbtn7";
            this.rdbtn7.Size = new System.Drawing.Size(32, 18);
            this.rdbtn7.TabIndex = 6;
            this.rdbtn7.TabStop = true;
            this.rdbtn7.Text = "7";
            this.rdbtn7.UseVisualStyleBackColor = true;
            this.rdbtn7.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn6
            // 
            this.rdbtn6.AutoSize = true;
            this.rdbtn6.Location = new System.Drawing.Point(24, 78);
            this.rdbtn6.Name = "rdbtn6";
            this.rdbtn6.Size = new System.Drawing.Size(32, 18);
            this.rdbtn6.TabIndex = 5;
            this.rdbtn6.TabStop = true;
            this.rdbtn6.Text = "6";
            this.rdbtn6.UseVisualStyleBackColor = true;
            this.rdbtn6.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn5
            // 
            this.rdbtn5.AutoSize = true;
            this.rdbtn5.Location = new System.Drawing.Point(78, 78);
            this.rdbtn5.Name = "rdbtn5";
            this.rdbtn5.Size = new System.Drawing.Size(32, 18);
            this.rdbtn5.TabIndex = 4;
            this.rdbtn5.TabStop = true;
            this.rdbtn5.Text = "5";
            this.rdbtn5.UseVisualStyleBackColor = true;
            this.rdbtn5.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn4
            // 
            this.rdbtn4.AutoSize = true;
            this.rdbtn4.Location = new System.Drawing.Point(132, 78);
            this.rdbtn4.Name = "rdbtn4";
            this.rdbtn4.Size = new System.Drawing.Size(32, 18);
            this.rdbtn4.TabIndex = 3;
            this.rdbtn4.TabStop = true;
            this.rdbtn4.Text = "4";
            this.rdbtn4.UseVisualStyleBackColor = true;
            this.rdbtn4.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn3
            // 
            this.rdbtn3.AutoSize = true;
            this.rdbtn3.Location = new System.Drawing.Point(132, 21);
            this.rdbtn3.Name = "rdbtn3";
            this.rdbtn3.Size = new System.Drawing.Size(32, 18);
            this.rdbtn3.TabIndex = 2;
            this.rdbtn3.TabStop = true;
            this.rdbtn3.Text = "3";
            this.rdbtn3.UseVisualStyleBackColor = true;
            this.rdbtn3.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn2
            // 
            this.rdbtn2.AutoSize = true;
            this.rdbtn2.Location = new System.Drawing.Point(78, 21);
            this.rdbtn2.Name = "rdbtn2";
            this.rdbtn2.Size = new System.Drawing.Size(32, 18);
            this.rdbtn2.TabIndex = 1;
            this.rdbtn2.TabStop = true;
            this.rdbtn2.Text = "2";
            this.rdbtn2.UseVisualStyleBackColor = true;
            this.rdbtn2.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // rdbtn1
            // 
            this.rdbtn1.AutoSize = true;
            this.rdbtn1.Location = new System.Drawing.Point(24, 21);
            this.rdbtn1.Name = "rdbtn1";
            this.rdbtn1.Size = new System.Drawing.Size(32, 18);
            this.rdbtn1.TabIndex = 0;
            this.rdbtn1.TabStop = true;
            this.rdbtn1.Text = "1";
            this.rdbtn1.UseVisualStyleBackColor = true;
            this.rdbtn1.CheckedChanged += new System.EventHandler(this.rdbtnPos_CheckedChanged);
            // 
            // dGV_worldPose
            // 
            this.dGV_worldPose.AllowUserToAddRows = false;
            this.dGV_worldPose.AllowUserToDeleteRows = false;
            this.dGV_worldPose.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dGV_worldPose.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dGV_worldPose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_worldPose.Location = new System.Drawing.Point(6, 243);
            this.dGV_worldPose.Name = "dGV_worldPose";
            this.dGV_worldPose.RowHeadersWidth = 65;
            this.dGV_worldPose.RowTemplate.Height = 23;
            this.dGV_worldPose.Size = new System.Drawing.Size(515, 234);
            this.dGV_worldPose.TabIndex = 0;
            this.dGV_worldPose.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dGV_worldPose_RowStateChanged);
            this.dGV_worldPose.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_worldPose_KeyDown);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_GrabImageForTest);
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.txt_Verify_Y);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txt_Verify_X);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(556, 505);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "测试坐标";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(14, 123);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(530, 115);
            this.textBox1.TabIndex = 113;
            this.textBox1.Text = "鼠标在左侧图像移动到指定位置，并滑动滚轮精准定位，观察坐标是否符合规格。";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 14);
            this.label10.TabIndex = 112;
            this.label10.Text = "验证Y：";
            // 
            // txt_Verify_Y
            // 
            this.txt_Verify_Y.Location = new System.Drawing.Point(73, 80);
            this.txt_Verify_Y.Name = "txt_Verify_Y";
            this.txt_Verify_Y.Size = new System.Drawing.Size(163, 23);
            this.txt_Verify_Y.TabIndex = 111;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 14);
            this.label11.TabIndex = 110;
            this.label11.Text = "验证X：";
            // 
            // txt_Verify_X
            // 
            this.txt_Verify_X.Location = new System.Drawing.Point(73, 35);
            this.txt_Verify_X.Name = "txt_Verify_X";
            this.txt_Verify_X.Size = new System.Drawing.Size(161, 23);
            this.txt_Verify_X.TabIndex = 109;
            // 
            // txt_log
            // 
            this.txt_log.Location = new System.Drawing.Point(3, 633);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_log.Size = new System.Drawing.Size(664, 249);
            this.txt_log.TabIndex = 12;
            // 
            // btn_GrabImageForTest
            // 
            this.btn_GrabImageForTest.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_GrabImageForTest.Location = new System.Drawing.Point(287, 51);
            this.btn_GrabImageForTest.Name = "btn_GrabImageForTest";
            this.btn_GrabImageForTest.Size = new System.Drawing.Size(121, 36);
            this.btn_GrabImageForTest.TabIndex = 114;
            this.btn_GrabImageForTest.Text = "采集测试图像";
            this.btn_GrabImageForTest.UseVisualStyleBackColor = true;
            this.btn_GrabImageForTest.Click += new System.EventHandler(this.btn_GrabImageForTest_Click);
            // 
            // CalibrationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.tabCtrl_Func);
            this.Controls.Add(this.hWndUnit1);
            this.Name = "CalibrationSetting";
            this.Controls.SetChildIndex(this.hWndUnit1, 0);
            this.Controls.SetChildIndex(this.tabCtrl_Func, 0);
            this.Controls.SetChildIndex(this.txt_log, 0);
            this.tabCtrl_Func.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gb_SettingPlanPose.ResumeLayout(false);
            this.gb_StartCalibration.ResumeLayout(false);
            this.gb_StartCalibration.PerformLayout();
            this.gB_InitCamParam.ResumeLayout(false);
            this.gB_InitCamParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Focus)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.gB_worldPoseEnable.ResumeLayout(false);
            this.groupBoxPointPos.ResumeLayout(false);
            this.groupBoxPointPos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_worldPose)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageControl.HWndUnit hWndUnit1;
        private System.Windows.Forms.TabControl tabCtrl_Func;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_CameraParam;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown NUpDown_Focus;
        private System.Windows.Forms.NumericUpDown NUpDown_Sx;
        private System.Windows.Forms.NumericUpDown NUpDown_Sy;
        private System.Windows.Forms.NumericUpDown nUpDown_ImageHeight;
        private System.Windows.Forms.NumericUpDown nUpDown_ImageWidth;
        private System.Windows.Forms.GroupBox gB_InitCamParam;
        private System.Windows.Forms.Button btn_saveParam;
        private System.Windows.Forms.Button btn_calibrateCamParam;
        private System.Windows.Forms.Button btn_grabCamera;
        private System.Windows.Forms.Button btn_openDescrpPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_descpPath;
        private System.Windows.Forms.GroupBox gb_StartCalibration;
        private System.Windows.Forms.CheckBox ckb_StartCalibration;
        private System.Windows.Forms.TextBox txt_log;
        private System.Windows.Forms.DataGridView dGV_worldPose;
        private System.Windows.Forms.TextBox txt_WorldPose;
        private System.Windows.Forms.Button btn_SaveWorldPose;
        private System.Windows.Forms.Button btn_CalibrationCoord;
        private System.Windows.Forms.Button btn_SetModelImage;
        private System.Windows.Forms.Button btn_grabImage;
        private System.Windows.Forms.GroupBox groupBoxPointPos;
        private System.Windows.Forms.RadioButton rdbtn9;
        private System.Windows.Forms.RadioButton rdbtn8;
        private System.Windows.Forms.RadioButton rdbtn7;
        private System.Windows.Forms.RadioButton rdbtn6;
        private System.Windows.Forms.RadioButton rdbtn5;
        private System.Windows.Forms.RadioButton rdbtn4;
        private System.Windows.Forms.RadioButton rdbtn3;
        private System.Windows.Forms.RadioButton rdbtn2;
        private System.Windows.Forms.RadioButton rdbtn1;
        private System.Windows.Forms.GroupBox gB_worldPoseEnable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt_Col;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_Row;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_Verify_Y;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_Verify_X;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox gb_SettingPlanPose;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbb_CalibImages;
        private System.Windows.Forms.Button btn_CalibartionPlan;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_savePlanPose;
        private System.Windows.Forms.TextBox txt_planPose;
        private System.Windows.Forms.Button btn_GrabImageForTest;
    }
}
