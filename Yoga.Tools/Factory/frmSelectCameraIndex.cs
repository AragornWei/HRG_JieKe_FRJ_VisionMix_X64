using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Yoga.Tools.Factory
{
    public partial class frmSelectCameraIndex : Form
    {
        int cameraIndex = 1;
        private List<int> CameraIndexSelList;
        public int CameraIndex
        {
            get
            {
                return cameraIndex;
            }
            private set
            {
                cameraIndex = value;
            }
        }

        public frmSelectCameraIndex(int cameraWant)
        {
            InitializeComponent();

            cmbCameraSelect.Items.Clear();

            /*int count = Camera.CameraManger.CameraDic.Count;
            for (int i = 1; i < count + 1; i++)
            {
                cmbCameraSelect.Items.Add(string.Format("相机{0}", i));
            }
            if (cameraWant< count + 1)
            {
                cmbCameraSelect.SelectedIndex = cameraWant-1;
            }*/
            //CameraIndexSelList = new List<int>();
            //foreach (int key in Camera.CameraManger.CameraDic.Keys)
            //{
            //    cmbCameraSelect.Items.Add(string.Format("相机{0}", key));
            //    CameraIndexSelList.Add(key);
            //}
            cmbCameraSelect.SelectedIndex = 0;

        }

        private void cmbCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CameraIndex = cmbCameraSelect.SelectedIndex+1;
            if(CameraIndexSelList.Count > 0)
            {
                CameraIndex = CameraIndexSelList[cmbCameraSelect.SelectedIndex];
            }
            else
            {
                CameraIndex = cmbCameraSelect.SelectedIndex + 1;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
                this.DialogResult = DialogResult.OK;
            
        }
    }
}
