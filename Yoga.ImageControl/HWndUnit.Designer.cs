namespace Yoga.ImageControl
{
    public partial class HWndUnit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HWndUnit));
            this.lblMouseMessage = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.labelResult = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMouseMessage
            // 
            resources.ApplyResources(this.lblMouseMessage, "lblMouseMessage");
            this.lblMouseMessage.Name = "lblMouseMessage";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.BackColor = System.Drawing.Color.Lime;
            this.lblName.Name = "lblName";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lblName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMouseMessage, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hWindowControl1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.DimGray;
            this.hWindowControl1.BorderColor = System.Drawing.Color.DimGray;
            this.tableLayoutPanel1.SetColumnSpan(this.hWindowControl1, 2);
            resources.ApplyResources(this.hWindowControl1, "hWindowControl1");
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 478, 400);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.WindowSize = new System.Drawing.Size(305, 284);
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labelResult, "labelResult");
            this.labelResult.ForeColor = System.Drawing.Color.Black;
            this.labelResult.Name = "labelResult";
            // 
            // HWndUnit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Name = "HWndUnit";
            this.SizeChanged += new System.EventHandler(this.HWndUnit_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMouseMessage;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label labelResult;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public HalconDotNet.HWindowControl hWindowControl1;
    }
}
