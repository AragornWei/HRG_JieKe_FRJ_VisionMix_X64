namespace Yoga.VisionMix.Frame
{
    partial class frmLogin
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
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.Btn_Ok = new System.Windows.Forms.Button();
            this.Text_PassWord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.drpOperator = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(196, 119);
            this.Btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(131, 31);
            this.Btn_Cancel.TabIndex = 7;
            this.Btn_Cancel.Text = "取消";
            this.Btn_Cancel.UseVisualStyleBackColor = true;
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
            // 
            // Btn_Ok
            // 
            this.Btn_Ok.Location = new System.Drawing.Point(53, 119);
            this.Btn_Ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Btn_Ok.Name = "Btn_Ok";
            this.Btn_Ok.Size = new System.Drawing.Size(131, 31);
            this.Btn_Ok.TabIndex = 8;
            this.Btn_Ok.Text = "确定";
            this.Btn_Ok.UseVisualStyleBackColor = true;
            this.Btn_Ok.Click += new System.EventHandler(this.Btn_Ok_Click);
            // 
            // Text_PassWord
            // 
            this.Text_PassWord.Location = new System.Drawing.Point(105, 71);
            this.Text_PassWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Text_PassWord.Name = "Text_PassWord";
            this.Text_PassWord.PasswordChar = '*';
            this.Text_PassWord.Size = new System.Drawing.Size(223, 25);
            this.Text_PassWord.TabIndex = 5;
            this.Text_PassWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_PassWord_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "密码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "账号:";
            // 
            // drpOperator
            // 
            this.drpOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpOperator.FormattingEnabled = true;
            this.drpOperator.Items.AddRange(new object[] {
            "操作员",
            "管理员"});
            this.drpOperator.Location = new System.Drawing.Point(105, 22);
            this.drpOperator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.drpOperator.Name = "drpOperator";
            this.drpOperator.Size = new System.Drawing.Size(220, 23);
            this.drpOperator.TabIndex = 38;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 216);
            this.Controls.Add(this.drpOperator);
            this.Controls.Add(this.Btn_Cancel);
            this.Controls.Add(this.Btn_Ok);
            this.Controls.Add(this.Text_PassWord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmLogin";
            this.Text = "登陆";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.Button Btn_Ok;
        private System.Windows.Forms.TextBox Text_PassWord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox drpOperator;
    }
}