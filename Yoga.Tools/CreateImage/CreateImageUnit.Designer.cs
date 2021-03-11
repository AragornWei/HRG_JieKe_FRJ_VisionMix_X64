using Yoga.ImageControl;

namespace Yoga.Tools.CreateImage
{
    partial class CreateImageUnit
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
            this.label1 = new System.Windows.Forms.Label();
            this.UpDownCameraIndex = new System.Windows.Forms.NumericUpDown();
            this.btnGetImage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkOffLineCycleTest = new System.Windows.Forms.CheckBox();
            this.txtOffLinePath = new System.Windows.Forms.TextBox();
            this.btnGetImagePath = new System.Windows.Forms.Button();
            this.chkOffLine = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSetRefImage = new System.Windows.Forms.Button();
            this.btnShowRefImage = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnDeleteRefImage = new System.Windows.Forms.Button();
            this.txbCaption = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCameraIndex)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWndUnit1
            // 
            this.hWndUnit1.BackColor = System.Drawing.SystemColors.Control;
            this.hWndUnit1.CameraMessage = null;
            this.hWndUnit1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hWndUnit1.Location = new System.Drawing.Point(4, 99);
            this.hWndUnit1.Margin = new System.Windows.Forms.Padding(5);
            this.hWndUnit1.MinimumSize = new System.Drawing.Size(13, 12);
            this.hWndUnit1.Name = "hWndUnit1";
            this.hWndUnit1.Size = new System.Drawing.Size(565, 458);
            this.hWndUnit1.TabIndex = 81;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 83;
            this.label1.Text = "相机编号";
            // 
            // UpDownCameraIndex
            // 
            this.UpDownCameraIndex.BackColor = System.Drawing.Color.Red;
            this.UpDownCameraIndex.Enabled = false;
            this.UpDownCameraIndex.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UpDownCameraIndex.ForeColor = System.Drawing.Color.Blue;
            this.UpDownCameraIndex.Location = new System.Drawing.Point(121, 13);
            this.UpDownCameraIndex.Margin = new System.Windows.Forms.Padding(4);
            this.UpDownCameraIndex.Name = "UpDownCameraIndex";
            this.UpDownCameraIndex.Size = new System.Drawing.Size(160, 26);
            this.UpDownCameraIndex.TabIndex = 82;
            this.UpDownCameraIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UpDownCameraIndex.ValueChanged += new System.EventHandler(this.UpDownCameraIndex_ValueChanged);
            // 
            // btnGetImage
            // 
            this.btnGetImage.Location = new System.Drawing.Point(23, 50);
            this.btnGetImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetImage.Name = "btnGetImage";
            this.btnGetImage.Size = new System.Drawing.Size(100, 29);
            this.btnGetImage.TabIndex = 84;
            this.btnGetImage.Text = "采集图像";
            this.btnGetImage.UseVisualStyleBackColor = true;
            this.btnGetImage.Click += new System.EventHandler(this.btnGetImage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkOffLineCycleTest);
            this.groupBox1.Controls.Add(this.txtOffLinePath);
            this.groupBox1.Controls.Add(this.btnGetImagePath);
            this.groupBox1.Controls.Add(this.chkOffLine);
            this.groupBox1.Location = new System.Drawing.Point(25, 54);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(473, 125);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "离线模式";
            // 
            // chkOffLineCycleTest
            // 
            this.chkOffLineCycleTest.AutoSize = true;
            this.chkOffLineCycleTest.Location = new System.Drawing.Point(186, 25);
            this.chkOffLineCycleTest.Name = "chkOffLineCycleTest";
            this.chkOffLineCycleTest.Size = new System.Drawing.Size(96, 16);
            this.chkOffLineCycleTest.TabIndex = 86;
            this.chkOffLineCycleTest.Text = "离线循环测试";
            this.chkOffLineCycleTest.UseVisualStyleBackColor = true;
            this.chkOffLineCycleTest.Visible = false;
            this.chkOffLineCycleTest.CheckedChanged += new System.EventHandler(this.chkOffLineCycleTest_CheckedChanged);
            // 
            // txtOffLinePath
            // 
            this.txtOffLinePath.Location = new System.Drawing.Point(9, 70);
            this.txtOffLinePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtOffLinePath.Name = "txtOffLinePath";
            this.txtOffLinePath.ReadOnly = true;
            this.txtOffLinePath.Size = new System.Drawing.Size(444, 21);
            this.txtOffLinePath.TabIndex = 85;
            // 
            // btnGetImagePath
            // 
            this.btnGetImagePath.Location = new System.Drawing.Point(409, 36);
            this.btnGetImagePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetImagePath.Name = "btnGetImagePath";
            this.btnGetImagePath.Size = new System.Drawing.Size(56, 29);
            this.btnGetImagePath.TabIndex = 1;
            this.btnGetImagePath.Text = "...";
            this.btnGetImagePath.UseVisualStyleBackColor = true;
            this.btnGetImagePath.Click += new System.EventHandler(this.btnGetImagePath_Click);
            // 
            // chkOffLine
            // 
            this.chkOffLine.AutoSize = true;
            this.chkOffLine.Location = new System.Drawing.Point(9, 26);
            this.chkOffLine.Margin = new System.Windows.Forms.Padding(4);
            this.chkOffLine.Name = "chkOffLine";
            this.chkOffLine.Size = new System.Drawing.Size(72, 16);
            this.chkOffLine.TabIndex = 0;
            this.chkOffLine.Text = "离线使能";
            this.chkOffLine.UseVisualStyleBackColor = true;
            this.chkOffLine.CheckedChanged += new System.EventHandler(this.chkOffLine_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 84;
            this.label2.Text = "文件夹";
            // 
            // btnSetRefImage
            // 
            this.btnSetRefImage.Location = new System.Drawing.Point(180, 49);
            this.btnSetRefImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetRefImage.Name = "btnSetRefImage";
            this.btnSetRefImage.Size = new System.Drawing.Size(100, 29);
            this.btnSetRefImage.TabIndex = 86;
            this.btnSetRefImage.Text = "设为模板";
            this.btnSetRefImage.UseVisualStyleBackColor = true;
            this.btnSetRefImage.Click += new System.EventHandler(this.btnSetRefImage_Click);
            // 
            // btnShowRefImage
            // 
            this.btnShowRefImage.Location = new System.Drawing.Point(344, 48);
            this.btnShowRefImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowRefImage.Name = "btnShowRefImage";
            this.btnShowRefImage.Size = new System.Drawing.Size(100, 29);
            this.btnShowRefImage.TabIndex = 87;
            this.btnShowRefImage.Text = "显示模板";
            this.btnShowRefImage.UseVisualStyleBackColor = true;
            this.btnShowRefImage.Click += new System.EventHandler(this.btnShowRefImage_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGetImage);
            this.groupBox2.Controls.Add(this.btnShowRefImage);
            this.groupBox2.Controls.Add(this.btnSetRefImage);
            this.groupBox2.Location = new System.Drawing.Point(25, 201);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(473, 125);
            this.groupBox2.TabIndex = 88;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "图像操作";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(574, 81);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(575, 632);
            this.tabControl1.TabIndex = 89;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.btnDeleteRefImage);
            this.tabPage1.Controls.Add(this.txbCaption);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.UpDownCameraIndex);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(567, 606);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "模板设置";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(390, 391);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 57);
            this.textBox1.TabIndex = 90;
            this.textBox1.Text = "提示：删除模板后可以重新指定相机编号";
            // 
            // btnDeleteRefImage
            // 
            this.btnDeleteRefImage.Location = new System.Drawing.Point(390, 334);
            this.btnDeleteRefImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteRefImage.Name = "btnDeleteRefImage";
            this.btnDeleteRefImage.Size = new System.Drawing.Size(100, 29);
            this.btnDeleteRefImage.TabIndex = 88;
            this.btnDeleteRefImage.Text = "删除模板";
            this.btnDeleteRefImage.UseVisualStyleBackColor = true;
            this.btnDeleteRefImage.Click += new System.EventHandler(this.btnDeleteRefImage_Click);
            // 
            // txbCaption
            // 
            this.txbCaption.BackColor = System.Drawing.SystemColors.Control;
            this.txbCaption.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txbCaption.Location = new System.Drawing.Point(22, 376);
            this.txbCaption.Multiline = true;
            this.txbCaption.Name = "txbCaption";
            this.txbCaption.Size = new System.Drawing.Size(316, 220);
            this.txbCaption.TabIndex = 89;
            this.txbCaption.Text = "离线功能说明：\r\n\r\n    选中离线功能时，如果该工具组还没有模板，并且对应相机没有连接，可以点击“选取图像”按钮来选取已有图片作为新的模板。\r\n\r\n    关" +
    "闭该页面后，在主页面点击“测试按钮”，即开始该工具组的离线测试，同时在相机对应窗口显示离线测试结果。";
            this.txbCaption.Visible = false;
            // 
            // CreateImageUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.hWndUnit1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "CreateImageUnit";
            this.Size = new System.Drawing.Size(1153, 794);
            this.Controls.SetChildIndex(this.hWndUnit1, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.UpDownCameraIndex)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown UpDownCameraIndex;
        private System.Windows.Forms.Button btnGetImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkOffLine;
        private System.Windows.Forms.Button btnGetImagePath;
        private System.Windows.Forms.TextBox txtOffLinePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSetRefImage;
        private System.Windows.Forms.Button btnShowRefImage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private ImageControl.HWndUnit hWndUnit1;
        private System.Windows.Forms.CheckBox chkOffLineCycleTest;
        private System.Windows.Forms.TextBox txbCaption;
        private System.Windows.Forms.Button btnDeleteRefImage;
        private System.Windows.Forms.TextBox textBox1;
    }
}
