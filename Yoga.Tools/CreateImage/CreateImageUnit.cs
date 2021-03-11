using System;
using System.Windows.Forms;
using HalconDotNet;
namespace Yoga.Tools.CreateImage
{
    public partial class CreateImageUnit : ToolsSettingUnit
    {
        CreateImageTool tool;
                
        public CreateImageUnit(CreateImageTool tool)
        {
            InitializeComponent();
            this.tool = tool;
            locked = true;
            Init();
            HideMax();
            HideMin();
            base.Init(tool.Name, tool);
            locked = false;                        
        }

        private void Init()
        {
            UpDownCameraIndex.Value = tool.CameraIndex;
            chkOffLine.Checked = tool.OffLineMode;
            chkOffLineCycleTest.Visible = chkOffLine.Checked;
            chkOffLineCycleTest.Checked = tool.OffLineCycleTest;
            btnGetImage.Text = tool.OffLineMode & (tool.Camera == null) ? "选取图像" : "采集图像";
            txbCaption.Visible = tool.OffLineMode & (tool.Camera == null);
            //txtOffLinePath.Text = tool.OffLineImagePath;
            if (tool.Camera != null)
                UpDownCameraIndex.BackColor = System.Drawing.Color.Lime;
            if (tool.RefImage == null)
                UpDownCameraIndex.Enabled = true;            
        }
 
        public override void ShowTranResult()
        {
            base.ShowTranResult();
            ShowRefImage();
        }
        private void UpDownCameraIndex_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.CameraIndex = (int)UpDownCameraIndex.Value;
            if (tool.Camera != null)
                UpDownCameraIndex.BackColor = System.Drawing.Color.Lime;
            else
                UpDownCameraIndex.BackColor = System.Drawing.Color.Red;
        }

        private void btnGetImagePath_Click(object sender, EventArgs e)
        {
            if (!chkOffLine.Checked)
                return;
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "选择图片文件存放目录";
            if (tool.OffLineImagePath != "")
            {
                folder.SelectedPath = tool.OffLineImagePath;
            }
            
            if (folder.ShowDialog() == DialogResult.OK)
            {
                string sPath = folder.SelectedPath;
                tool.OffLineImagePath = sPath;         //通过这里执行RefushImagePathList(string offLineImagePath)，刷新图像文件列表
                if (tool.GetOffLineImageFileSum()>0)
                {
                    txtOffLinePath.Text = tool.OffLineImagePath;
                    txtOffLinePath.BackColor = System.Drawing.SystemColors.Control; //注意：SystemColors.是Color的属性集合
                }
                else
                {
                    txtOffLinePath.Text = "";
                    txtOffLinePath.BackColor = System.Drawing.Color.Red;
                }               
            }
        }

        private void chkOffLine_CheckedChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.OffLineMode = chkOffLine.Checked;
            chkOffLineCycleTest.Visible = chkOffLine.Checked;
            btnGetImage.Text = chkOffLine.Checked & (tool.Camera == null) ? "选取图像" : "采集图像";
            txbCaption.Visible = tool.OffLineMode & (tool.Camera == null);
        }

        private void chkOffLineCycleTest_CheckedChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.OffLineCycleTest = chkOffLineCycleTest.Checked;
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            if ((tool.Camera==null)&!tool.OffLineMode)
            {
                MessageBox.Show($"相机{tool.CameraIndex}没有连接", "提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if ((tool.Camera == null) & tool.OffLineMode)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Title = "选择现有图片做为模板";
                    ofd.Filter = "BMP Files (*.bmp)|*.bmp|JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
                    ofd.Multiselect = false;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        HImage hi = new HImage();
                        hi.ReadImage(ofd.FileName);
                        tool.ImageTestOut = hi.Clone();
                        tool.ShowImage(hWndUnit1.HWndCtrl);
                        hWndUnit1.HWndCtrl.Repaint();
                    }
                    return;
                }
            }
            catch(Exception)
            {
                MessageBox.Show("设置离线模板失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }        

            tool.SetExtTriggerDataOff();  //只有设CreateImageTool的isExtTrigger为off,才能触发RunAct()的GrabImage()获取新图片。
            tool.GetImage();
            tool.ShowImage(hWndUnit1.HWndCtrl);
            hWndUnit1.HWndCtrl.Repaint();
        }

        private void btnSetRefImage_Click(object sender, EventArgs e)
        {
            if (tool.ImageTestOut == null|| tool.ImageTestOut.IsInitialized()==false)
            {
                MessageBox.Show("未采集到图像数据,请先采集图像", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tool.RefImage = tool.ImageTestOut.CopyImage();
            MessageBox.Show("模板图像设置完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowRefImage();
            UpDownCameraIndex.Enabled = false;
        }

        private void btnShowRefImage_Click(object sender, EventArgs e)
        {
            ShowRefImage();
        }

        private void  ShowRefImage()
        {
            if (tool.RefImage != null && tool.RefImage.IsInitialized())
            {
                hWndUnit1.HWndCtrl.AddIconicVar(tool.RefImage);
                hWndUnit1.HWndCtrl.AddText("模板图像", 20, 20, 100, "red");
                hWndUnit1.HWndCtrl.Repaint();
            }
        }

        private void btnDeleteRefImage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除模板图像吗?", "确认",MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            if(tool.RefImage != null)
            {
                tool.RefImage.Dispose();
                tool.RefImage = null;
            }
            hWndUnit1.HWndCtrl.ClearList();
            hWndUnit1.HWndCtrl.Repaint();
            UpDownCameraIndex.Enabled = true;
        }
    }
}
