namespace Yoga.Calibration.EyesOnHand2
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
            this.txt_log = new System.Windows.Forms.TextBox();
            this.hWndUnit1 = new Yoga.ImageControl.HWndUnit();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gB_worldPoseEnable = new System.Windows.Forms.GroupBox();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.btn_AddCalibratData = new System.Windows.Forms.Button();
            this.txt_CurrentToolInBaesPoseCalib = new System.Windows.Forms.TextBox();
            this.btn_ContinusOrStop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_grabImage = new System.Windows.Forms.Button();
            this.btn_SaveCamRobotCal = new System.Windows.Forms.Button();
            this.btn_CalibrationMovingCam = new System.Windows.Forms.Button();
            this.dGV_worldPose = new System.Windows.Forms.DataGridView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_savePlanPose = new System.Windows.Forms.Button();
            this.txt_planPose = new System.Windows.Forms.TextBox();
            this.txt_descpPath = new System.Windows.Forms.TextBox();
            this.txt_CameraParam = new System.Windows.Forms.TextBox();
            this.gb_SettingPlanPose = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.nUP_Caltab_H = new System.Windows.Forms.NumericUpDown();
            this.btn_CalibartionPlan = new System.Windows.Forms.Button();
            this.gb_StartCalibration = new System.Windows.Forms.GroupBox();
            this.btn_AcqImage = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_calibrateCamParam = new System.Windows.Forms.Button();
            this.cbb_CalibImages = new System.Windows.Forms.ComboBox();
            this.btn_grabCamera = new System.Windows.Forms.Button();
            this.ckb_StartCalibration = new System.Windows.Forms.CheckBox();
            this.btn_openDescrpPath = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
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
            this.tabCtrl_Func = new System.Windows.Forms.TabControl();
            this.tabPage2.SuspendLayout();
            this.gB_worldPoseEnable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_worldPose)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.gb_SettingPlanPose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_Caltab_H)).BeginInit();
            this.gb_StartCalibration.SuspendLayout();
            this.gB_InitCamParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Focus)).BeginInit();
            this.tabCtrl_Func.SuspendLayout();
            this.SuspendLayout();
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gB_worldPoseEnable);
            this.tabPage2.Controls.Add(this.dGV_worldPose);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(556, 788);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "标定相机与手臂";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gB_worldPoseEnable
            // 
            this.gB_worldPoseEnable.Controls.Add(this.btn_Reset);
            this.gB_worldPoseEnable.Controls.Add(this.btn_AddCalibratData);
            this.gB_worldPoseEnable.Controls.Add(this.txt_CurrentToolInBaesPoseCalib);
            this.gB_worldPoseEnable.Controls.Add(this.btn_ContinusOrStop);
            this.gB_worldPoseEnable.Controls.Add(this.label8);
            this.gB_worldPoseEnable.Controls.Add(this.btn_grabImage);
            this.gB_worldPoseEnable.Controls.Add(this.btn_SaveCamRobotCal);
            this.gB_worldPoseEnable.Controls.Add(this.btn_CalibrationMovingCam);
            this.gB_worldPoseEnable.Location = new System.Drawing.Point(9, 10);
            this.gB_worldPoseEnable.Name = "gB_worldPoseEnable";
            this.gB_worldPoseEnable.Size = new System.Drawing.Size(541, 170);
            this.gB_worldPoseEnable.TabIndex = 104;
            this.gB_worldPoseEnable.TabStop = false;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Reset.Location = new System.Drawing.Point(412, 16);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(121, 36);
            this.btn_Reset.TabIndex = 108;
            this.btn_Reset.Text = "重置标定数据";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // btn_AddCalibratData
            // 
            this.btn_AddCalibratData.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AddCalibratData.Location = new System.Drawing.Point(277, 16);
            this.btn_AddCalibratData.Name = "btn_AddCalibratData";
            this.btn_AddCalibratData.Size = new System.Drawing.Size(121, 36);
            this.btn_AddCalibratData.TabIndex = 107;
            this.btn_AddCalibratData.Text = "加入当前数据到队列";
            this.btn_AddCalibratData.UseVisualStyleBackColor = true;
            this.btn_AddCalibratData.Click += new System.EventHandler(this.btn_AddCalibratData_Click);
            // 
            // txt_CurrentToolInBaesPoseCalib
            // 
            this.txt_CurrentToolInBaesPoseCalib.Location = new System.Drawing.Point(9, 90);
            this.txt_CurrentToolInBaesPoseCalib.Name = "txt_CurrentToolInBaesPoseCalib";
            this.txt_CurrentToolInBaesPoseCalib.Size = new System.Drawing.Size(532, 23);
            this.txt_CurrentToolInBaesPoseCalib.TabIndex = 105;
            this.txt_CurrentToolInBaesPoseCalib.TextChanged += new System.EventHandler(this.txt_CurrentToolInBaesPoseCalib_TextChanged);
            // 
            // btn_ContinusOrStop
            // 
            this.btn_ContinusOrStop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_ContinusOrStop.Location = new System.Drawing.Point(7, 16);
            this.btn_ContinusOrStop.Name = "btn_ContinusOrStop";
            this.btn_ContinusOrStop.Size = new System.Drawing.Size(121, 36);
            this.btn_ContinusOrStop.TabIndex = 103;
            this.btn_ContinusOrStop.Text = "连续采集图像";
            this.btn_ContinusOrStop.UseVisualStyleBackColor = true;
            this.btn_ContinusOrStop.Click += new System.EventHandler(this.btn_ContinusOrStop_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(189, 14);
            this.label8.TabIndex = 106;
            this.label8.Text = "输入每次拍照当前手臂姿势: ";
            // 
            // btn_grabImage
            // 
            this.btn_grabImage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_grabImage.Location = new System.Drawing.Point(142, 16);
            this.btn_grabImage.Name = "btn_grabImage";
            this.btn_grabImage.Size = new System.Drawing.Size(121, 36);
            this.btn_grabImage.TabIndex = 99;
            this.btn_grabImage.Text = "拍照识别标定板";
            this.btn_grabImage.UseVisualStyleBackColor = true;
            this.btn_grabImage.Click += new System.EventHandler(this.btn_grabImage_Click);
            // 
            // btn_SaveCamRobotCal
            // 
            this.btn_SaveCamRobotCal.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_SaveCamRobotCal.Location = new System.Drawing.Point(150, 125);
            this.btn_SaveCamRobotCal.Name = "btn_SaveCamRobotCal";
            this.btn_SaveCamRobotCal.Size = new System.Drawing.Size(122, 36);
            this.btn_SaveCamRobotCal.TabIndex = 102;
            this.btn_SaveCamRobotCal.Text = "保存标定结果";
            this.btn_SaveCamRobotCal.UseVisualStyleBackColor = true;
            this.btn_SaveCamRobotCal.Click += new System.EventHandler(this.btn_SaveCamRobotCal_Click);
            // 
            // btn_CalibrationMovingCam
            // 
            this.btn_CalibrationMovingCam.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CalibrationMovingCam.Location = new System.Drawing.Point(9, 125);
            this.btn_CalibrationMovingCam.Name = "btn_CalibrationMovingCam";
            this.btn_CalibrationMovingCam.Size = new System.Drawing.Size(121, 36);
            this.btn_CalibrationMovingCam.TabIndex = 101;
            this.btn_CalibrationMovingCam.Text = "标定相机与手臂";
            this.btn_CalibrationMovingCam.UseVisualStyleBackColor = true;
            this.btn_CalibrationMovingCam.Click += new System.EventHandler(this.btn_CalibrationCoord_Click);
            // 
            // dGV_worldPose
            // 
            this.dGV_worldPose.AllowUserToAddRows = false;
            this.dGV_worldPose.AllowUserToDeleteRows = false;
            this.dGV_worldPose.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dGV_worldPose.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dGV_worldPose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_worldPose.Location = new System.Drawing.Point(6, 186);
            this.dGV_worldPose.Name = "dGV_worldPose";
            this.dGV_worldPose.RowHeadersWidth = 65;
            this.dGV_worldPose.RowTemplate.Height = 23;
            this.dGV_worldPose.Size = new System.Drawing.Size(544, 596);
            this.dGV_worldPose.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.btn_savePlanPose);
            this.tabPage1.Controls.Add(this.txt_planPose);
            this.tabPage1.Controls.Add(this.txt_descpPath);
            this.tabPage1.Controls.Add(this.txt_CameraParam);
            this.tabPage1.Controls.Add(this.gb_SettingPlanPose);
            this.tabPage1.Controls.Add(this.gb_StartCalibration);
            this.tabPage1.Controls.Add(this.ckb_StartCalibration);
            this.tabPage1.Controls.Add(this.btn_openDescrpPath);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.btn_saveParam);
            this.tabPage1.Controls.Add(this.gB_InitCamParam);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(556, 788);
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
            this.btn_savePlanPose.Text = "保存参考平面 (MpInCamPose)";
            this.btn_savePlanPose.UseVisualStyleBackColor = true;
            this.btn_savePlanPose.Click += new System.EventHandler(this.btn_SaveWorldPose_Click);
            // 
            // txt_planPose
            // 
            this.txt_planPose.Location = new System.Drawing.Point(9, 435);
            this.txt_planPose.Multiline = true;
            this.txt_planPose.Name = "txt_planPose";
            this.txt_planPose.ReadOnly = true;
            this.txt_planPose.Size = new System.Drawing.Size(419, 55);
            this.txt_planPose.TabIndex = 106;
            // 
            // txt_descpPath
            // 
            this.txt_descpPath.Location = new System.Drawing.Point(10, 26);
            this.txt_descpPath.Multiline = true;
            this.txt_descpPath.Name = "txt_descpPath";
            this.txt_descpPath.Size = new System.Drawing.Size(441, 31);
            this.txt_descpPath.TabIndex = 100;
            // 
            // txt_CameraParam
            // 
            this.txt_CameraParam.Location = new System.Drawing.Point(9, 348);
            this.txt_CameraParam.Multiline = true;
            this.txt_CameraParam.Name = "txt_CameraParam";
            this.txt_CameraParam.ReadOnly = true;
            this.txt_CameraParam.Size = new System.Drawing.Size(419, 55);
            this.txt_CameraParam.TabIndex = 0;
            // 
            // gb_SettingPlanPose
            // 
            this.gb_SettingPlanPose.Controls.Add(this.label14);
            this.gb_SettingPlanPose.Controls.Add(this.nUP_Caltab_H);
            this.gb_SettingPlanPose.Controls.Add(this.btn_CalibartionPlan);
            this.gb_SettingPlanPose.Location = new System.Drawing.Point(374, 78);
            this.gb_SettingPlanPose.Name = "gb_SettingPlanPose";
            this.gb_SettingPlanPose.Size = new System.Drawing.Size(136, 219);
            this.gb_SettingPlanPose.TabIndex = 105;
            this.gb_SettingPlanPose.TabStop = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(6, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 16);
            this.label14.TabIndex = 100;
            this.label14.Text = "标定板厚度";
            // 
            // nUP_Caltab_H
            // 
            this.nUP_Caltab_H.DecimalPlaces = 1;
            this.nUP_Caltab_H.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nUP_Caltab_H.Location = new System.Drawing.Point(10, 45);
            this.nUP_Caltab_H.Margin = new System.Windows.Forms.Padding(4);
            this.nUP_Caltab_H.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nUP_Caltab_H.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nUP_Caltab_H.Name = "nUP_Caltab_H";
            this.nUP_Caltab_H.Size = new System.Drawing.Size(77, 23);
            this.nUP_Caltab_H.TabIndex = 101;
            this.nUP_Caltab_H.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nUP_Caltab_H.ValueChanged += new System.EventHandler(this.nUP_Caltab_H_ValueChanged);
            // 
            // btn_CalibartionPlan
            // 
            this.btn_CalibartionPlan.BackColor = System.Drawing.Color.Transparent;
            this.btn_CalibartionPlan.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CalibartionPlan.Location = new System.Drawing.Point(6, 84);
            this.btn_CalibartionPlan.Name = "btn_CalibartionPlan";
            this.btn_CalibartionPlan.Size = new System.Drawing.Size(121, 55);
            this.btn_CalibartionPlan.TabIndex = 99;
            this.btn_CalibartionPlan.Text = "设为参考平面";
            this.btn_CalibartionPlan.UseVisualStyleBackColor = false;
            this.btn_CalibartionPlan.Click += new System.EventHandler(this.btn_CalibartionPlan_Click);
            // 
            // gb_StartCalibration
            // 
            this.gb_StartCalibration.Controls.Add(this.btn_AcqImage);
            this.gb_StartCalibration.Controls.Add(this.label12);
            this.gb_StartCalibration.Controls.Add(this.btn_calibrateCamParam);
            this.gb_StartCalibration.Controls.Add(this.cbb_CalibImages);
            this.gb_StartCalibration.Controls.Add(this.btn_grabCamera);
            this.gb_StartCalibration.Location = new System.Drawing.Point(218, 81);
            this.gb_StartCalibration.Name = "gb_StartCalibration";
            this.gb_StartCalibration.Size = new System.Drawing.Size(136, 217);
            this.gb_StartCalibration.TabIndex = 104;
            this.gb_StartCalibration.TabStop = false;
            // 
            // btn_AcqImage
            // 
            this.btn_AcqImage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AcqImage.Location = new System.Drawing.Point(6, 18);
            this.btn_AcqImage.Name = "btn_AcqImage";
            this.btn_AcqImage.Size = new System.Drawing.Size(121, 36);
            this.btn_AcqImage.TabIndex = 107;
            this.btn_AcqImage.Text = "连续采集图像";
            this.btn_AcqImage.UseVisualStyleBackColor = true;
            this.btn_AcqImage.Click += new System.EventHandler(this.btn_ContinusOrStop_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(10, 156);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 16);
            this.label12.TabIndex = 106;
            this.label12.Text = "图像序号";
            // 
            // btn_calibrateCamParam
            // 
            this.btn_calibrateCamParam.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_calibrateCamParam.Location = new System.Drawing.Point(8, 106);
            this.btn_calibrateCamParam.Name = "btn_calibrateCamParam";
            this.btn_calibrateCamParam.Size = new System.Drawing.Size(121, 40);
            this.btn_calibrateCamParam.TabIndex = 99;
            this.btn_calibrateCamParam.Text = "标定相机参数";
            this.btn_calibrateCamParam.UseVisualStyleBackColor = true;
            this.btn_calibrateCamParam.Click += new System.EventHandler(this.btn_calibrateCamParam_Click);
            // 
            // cbb_CalibImages
            // 
            this.cbb_CalibImages.FormattingEnabled = true;
            this.cbb_CalibImages.Location = new System.Drawing.Point(10, 176);
            this.cbb_CalibImages.Name = "cbb_CalibImages";
            this.cbb_CalibImages.Size = new System.Drawing.Size(121, 22);
            this.cbb_CalibImages.TabIndex = 100;
            this.cbb_CalibImages.SelectedIndexChanged += new System.EventHandler(this.cbb_CalibImages_SelectedIndexChanged);
            // 
            // btn_grabCamera
            // 
            this.btn_grabCamera.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_grabCamera.Location = new System.Drawing.Point(6, 63);
            this.btn_grabCamera.Name = "btn_grabCamera";
            this.btn_grabCamera.Size = new System.Drawing.Size(121, 35);
            this.btn_grabCamera.TabIndex = 98;
            this.btn_grabCamera.Text = "采集图像并识别";
            this.btn_grabCamera.UseVisualStyleBackColor = true;
            this.btn_grabCamera.Click += new System.EventHandler(this.btn_grabCamera_Click);
            // 
            // ckb_StartCalibration
            // 
            this.ckb_StartCalibration.AutoSize = true;
            this.ckb_StartCalibration.Location = new System.Drawing.Point(231, 64);
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
            this.btn_openDescrpPath.Size = new System.Drawing.Size(83, 31);
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
            this.gB_InitCamParam.Location = new System.Drawing.Point(14, 64);
            this.gB_InitCamParam.Name = "gB_InitCamParam";
            this.gB_InitCamParam.Size = new System.Drawing.Size(195, 234);
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
            // tabCtrl_Func
            // 
            this.tabCtrl_Func.Controls.Add(this.tabPage1);
            this.tabCtrl_Func.Controls.Add(this.tabPage2);
            this.tabCtrl_Func.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabCtrl_Func.Location = new System.Drawing.Point(673, 66);
            this.tabCtrl_Func.Name = "tabCtrl_Func";
            this.tabCtrl_Func.SelectedIndex = 0;
            this.tabCtrl_Func.Size = new System.Drawing.Size(564, 816);
            this.tabCtrl_Func.TabIndex = 11;
            this.tabCtrl_Func.SelectedIndexChanged += new System.EventHandler(this.tabCtrl_Func_SelectedIndexChanged);
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
            this.tabPage2.ResumeLayout(false);
            this.gB_worldPoseEnable.ResumeLayout(false);
            this.gB_worldPoseEnable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_worldPose)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.gb_SettingPlanPose.ResumeLayout(false);
            this.gb_SettingPlanPose.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUP_Caltab_H)).EndInit();
            this.gb_StartCalibration.ResumeLayout(false);
            this.gb_StartCalibration.PerformLayout();
            this.gB_InitCamParam.ResumeLayout(false);
            this.gB_InitCamParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDown_ImageWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Sx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUpDown_Focus)).EndInit();
            this.tabCtrl_Func.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Yoga.ImageControl.HWndUnit hWndUnit1;
        private System.Windows.Forms.TextBox txt_log;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txt_CurrentToolInBaesPoseCalib;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gB_worldPoseEnable;
        private System.Windows.Forms.Button btn_grabImage;
        private System.Windows.Forms.Button btn_SaveCamRobotCal;
        private System.Windows.Forms.Button btn_CalibrationMovingCam;
        private System.Windows.Forms.DataGridView dGV_worldPose;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_savePlanPose;
        private System.Windows.Forms.TextBox txt_planPose;
        private System.Windows.Forms.TextBox txt_descpPath;
        private System.Windows.Forms.TextBox txt_CameraParam;
        private System.Windows.Forms.GroupBox gb_SettingPlanPose;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nUP_Caltab_H;
        private System.Windows.Forms.Button btn_CalibartionPlan;
        private System.Windows.Forms.GroupBox gb_StartCalibration;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btn_calibrateCamParam;
        private System.Windows.Forms.ComboBox cbb_CalibImages;
        private System.Windows.Forms.Button btn_grabCamera;
        private System.Windows.Forms.CheckBox ckb_StartCalibration;
        private System.Windows.Forms.Button btn_openDescrpPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_saveParam;
        private System.Windows.Forms.GroupBox gB_InitCamParam;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nUpDown_ImageHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nUpDown_ImageWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NUpDown_Sy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NUpDown_Sx;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NUpDown_Focus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabCtrl_Func;
        private System.Windows.Forms.Button btn_ContinusOrStop;
        private System.Windows.Forms.Button btn_AcqImage;
        private System.Windows.Forms.Button btn_AddCalibratData;
        private System.Windows.Forms.Button btn_Reset;
    }
}
