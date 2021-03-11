namespace Yoga.ImageControl
{
    partial class ROIActUnit
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
            this.groupBoxCreateROI = new System.Windows.Forms.GroupBox();
            this.btn_ClearTuYA = new System.Windows.Forms.Button();
            this.cMB_TuYa_radius = new System.Windows.Forms.ComboBox();
            this.btn_TuYa = new System.Windows.Forms.Button();
            this.circleButton = new System.Windows.Forms.Button();
            this.delROIButton = new System.Windows.Forms.Button();
            this.delAllROIButton = new System.Windows.Forms.Button();
            this.rect2Button = new System.Windows.Forms.Button();
            this.subFromROIButton = new System.Windows.Forms.RadioButton();
            this.addToROIButton = new System.Windows.Forms.RadioButton();
            this.rect1Button = new System.Windows.Forms.Button();
            this.groupBoxCreateROI.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCreateROI
            // 
            this.groupBoxCreateROI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBoxCreateROI.AutoSize = true;
            this.groupBoxCreateROI.Controls.Add(this.btn_ClearTuYA);
            this.groupBoxCreateROI.Controls.Add(this.cMB_TuYa_radius);
            this.groupBoxCreateROI.Controls.Add(this.btn_TuYa);
            this.groupBoxCreateROI.Controls.Add(this.circleButton);
            this.groupBoxCreateROI.Controls.Add(this.delROIButton);
            this.groupBoxCreateROI.Controls.Add(this.delAllROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect2Button);
            this.groupBoxCreateROI.Controls.Add(this.subFromROIButton);
            this.groupBoxCreateROI.Controls.Add(this.addToROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect1Button);
            this.groupBoxCreateROI.Location = new System.Drawing.Point(14, 12);
            this.groupBoxCreateROI.Name = "groupBoxCreateROI";
            this.groupBoxCreateROI.Size = new System.Drawing.Size(238, 205);
            this.groupBoxCreateROI.TabIndex = 96;
            this.groupBoxCreateROI.TabStop = false;
            this.groupBoxCreateROI.Text = "创建ROI";
            // 
            // btn_ClearTuYA
            // 
            this.btn_ClearTuYA.AutoSize = true;
            this.btn_ClearTuYA.Location = new System.Drawing.Point(125, 159);
            this.btn_ClearTuYA.Name = "btn_ClearTuYA";
            this.btn_ClearTuYA.Size = new System.Drawing.Size(98, 26);
            this.btn_ClearTuYA.TabIndex = 15;
            this.btn_ClearTuYA.Text = "清除涂鸦";
            this.btn_ClearTuYA.Click += new System.EventHandler(this.btn_ClearTuYA_Click);
            // 
            // cMB_TuYa_radius
            // 
            this.cMB_TuYa_radius.FormattingEnabled = true;
            this.cMB_TuYa_radius.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "5",
            "7",
            "10",
            "15",
            "20",
            "30",
            "50"});
            this.cMB_TuYa_radius.Location = new System.Drawing.Point(125, 125);
            this.cMB_TuYa_radius.Name = "cMB_TuYa_radius";
            this.cMB_TuYa_radius.Size = new System.Drawing.Size(98, 20);
            this.cMB_TuYa_radius.TabIndex = 14;
            this.cMB_TuYa_radius.Text = "3";
            this.cMB_TuYa_radius.SelectedIndexChanged += new System.EventHandler(this.cMB_TuYa_radius_SelectedIndexChanged);
            this.cMB_TuYa_radius.TextChanged += new System.EventHandler(this.cMB_TuYa_radius_TextChanged);
            // 
            // btn_TuYa
            // 
            this.btn_TuYa.AutoSize = true;
            this.btn_TuYa.Location = new System.Drawing.Point(125, 85);
            this.btn_TuYa.Name = "btn_TuYa";
            this.btn_TuYa.Size = new System.Drawing.Size(98, 26);
            this.btn_TuYa.TabIndex = 13;
            this.btn_TuYa.Text = "涂鸦";
            this.btn_TuYa.Click += new System.EventHandler(this.btn_TuYa_Click);
            // 
            // circleButton
            // 
            this.circleButton.AutoSize = true;
            this.circleButton.Location = new System.Drawing.Point(6, 121);
            this.circleButton.Name = "circleButton";
            this.circleButton.Size = new System.Drawing.Size(98, 26);
            this.circleButton.TabIndex = 12;
            this.circleButton.Text = "圆形";
            this.circleButton.Click += new System.EventHandler(this.circleButton_Click);
            // 
            // delROIButton
            // 
            this.delROIButton.AutoSize = true;
            this.delROIButton.Location = new System.Drawing.Point(7, 159);
            this.delROIButton.Name = "delROIButton";
            this.delROIButton.Size = new System.Drawing.Size(98, 26);
            this.delROIButton.TabIndex = 11;
            this.delROIButton.Text = "删除激活ROI";
            this.delROIButton.Click += new System.EventHandler(this.delROIButton_Click);
            // 
            // delAllROIButton
            // 
            this.delAllROIButton.AutoSize = true;
            this.delAllROIButton.Location = new System.Drawing.Point(125, 48);
            this.delAllROIButton.Name = "delAllROIButton";
            this.delAllROIButton.Size = new System.Drawing.Size(98, 26);
            this.delAllROIButton.TabIndex = 10;
            this.delAllROIButton.Text = "删除所有ROI";
            this.delAllROIButton.Click += new System.EventHandler(this.delAllROIButton_Click);
            // 
            // rect2Button
            // 
            this.rect2Button.AutoSize = true;
            this.rect2Button.Location = new System.Drawing.Point(6, 85);
            this.rect2Button.Name = "rect2Button";
            this.rect2Button.Size = new System.Drawing.Size(98, 26);
            this.rect2Button.TabIndex = 9;
            this.rect2Button.Text = "带角度矩形";
            this.rect2Button.Click += new System.EventHandler(this.rect2Button_Click);
            // 
            // subFromROIButton
            // 
            this.subFromROIButton.AutoSize = true;
            this.subFromROIButton.Location = new System.Drawing.Point(125, 23);
            this.subFromROIButton.Name = "subFromROIButton";
            this.subFromROIButton.Size = new System.Drawing.Size(41, 16);
            this.subFromROIButton.TabIndex = 8;
            this.subFromROIButton.Text = "(-)";
            this.subFromROIButton.CheckedChanged += new System.EventHandler(this.subFromROIButton_CheckedChanged);
            // 
            // addToROIButton
            // 
            this.addToROIButton.AutoSize = true;
            this.addToROIButton.Checked = true;
            this.addToROIButton.Location = new System.Drawing.Point(36, 24);
            this.addToROIButton.Name = "addToROIButton";
            this.addToROIButton.Size = new System.Drawing.Size(41, 16);
            this.addToROIButton.TabIndex = 7;
            this.addToROIButton.TabStop = true;
            this.addToROIButton.Text = "(+)";
            this.addToROIButton.CheckedChanged += new System.EventHandler(this.addToROIButton_CheckedChanged);
            // 
            // rect1Button
            // 
            this.rect1Button.AutoSize = true;
            this.rect1Button.Location = new System.Drawing.Point(6, 48);
            this.rect1Button.Name = "rect1Button";
            this.rect1Button.Size = new System.Drawing.Size(98, 26);
            this.rect1Button.TabIndex = 5;
            this.rect1Button.Text = "矩形";
            this.rect1Button.Click += new System.EventHandler(this.rect1Button_Click);
            // 
            // ROIActUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxCreateROI);
            this.Name = "ROIActUnit";
            this.Size = new System.Drawing.Size(268, 223);
            this.groupBoxCreateROI.ResumeLayout(false);
            this.groupBoxCreateROI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCreateROI;
        private System.Windows.Forms.Button circleButton;
        private System.Windows.Forms.Button delROIButton;
        private System.Windows.Forms.Button delAllROIButton;
        private System.Windows.Forms.Button rect2Button;
        private System.Windows.Forms.RadioButton subFromROIButton;
        private System.Windows.Forms.RadioButton addToROIButton;
        private System.Windows.Forms.Button rect1Button;
        private System.Windows.Forms.Button btn_ClearTuYA;
        private System.Windows.Forms.ComboBox cMB_TuYa_radius;
        private System.Windows.Forms.Button btn_TuYa;
    }
}
