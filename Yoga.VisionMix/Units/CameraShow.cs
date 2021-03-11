using System.Windows.Forms;
using Yoga.ImageControl;
using System;

namespace Yoga.VisionMix.Units
{
    public class CameraShow
    {
        public HWndUnit HWndUnit;
        //public DataGridView DgvTool;
        public int cameraIndex;
        public event EventHandler<HWndUnitMouseEventArgs> CameraShowMouseDoubleClick;
        public void Init(int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
            HWndUnit = new HWndUnit();
            HWndUnit.Dock = DockStyle.Fill;
            HWndUnit.Name = cameraIndex.ToString();
            HWndUnit.CameraMessage = string.Format("相机{0}",cameraIndex);           
            HWndUnit.HWndCtrl.SetBackgroundColor("#262626");//
            HWndUnit.HWndCtrl.MouseDoubleClick += HWndCtrl_MouseDoubleClick;
        }
        private void HWndCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(CameraShowMouseDoubleClick != null)
            {
                CameraShowMouseDoubleClick(this, new HWndUnitMouseEventArgs(e,cameraIndex));
            }
        }
    }
}
