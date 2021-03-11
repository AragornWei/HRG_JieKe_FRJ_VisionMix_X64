using System;
using System.Windows.Forms;

namespace Yoga.ImageControl
{
    public partial class ROIActUnit : UserControl
    {
        private ROIController roiController;
        public ROIActUnit()
        {
            InitializeComponent();
        }

        public ROIController RoiController
        {
            get
            {
                return roiController;
            }

            set
            {
                roiController = value;
            }
        }

        private void addToROIButton_CheckedChanged(object sender, EventArgs e)
        {
            if (addToROIButton.Checked)
                RoiController.SetROISign(ROIOperation.Positive);
        }

        private void subFromROIButton_CheckedChanged(object sender, EventArgs e)
        {
            if (subFromROIButton.Checked)
                RoiController.SetROISign(ROIOperation.Negative);
        }

        private void rect1Button_Click(object sender, EventArgs e)
        {
            if (addToROIButton.Checked)
            {
                RoiController.SetROISign(ROIOperation.Positive);
            }
            else
            {
                RoiController.SetROISign(ROIOperation.Negative);
            }
            RoiController.SetROIShape(new ROIRectangle1());
        }

        private void rect2Button_Click(object sender, EventArgs e)
        {
            if (addToROIButton.Checked)
            {
                RoiController.SetROISign(ROIOperation.Positive);
            }
            else
            {
                RoiController.SetROISign(ROIOperation.Negative);
            }
            RoiController.SetROIShape(new ROIRectangle2());
        }

        private void circleButton_Click(object sender, EventArgs e)
        {
            if (addToROIButton.Checked)
            {
                RoiController.SetROISign(ROIOperation.Positive);
            }
            else
            {
                RoiController.SetROISign(ROIOperation.Negative);
            }
            RoiController.SetROIShape(new ROICircle());
        }

        private void delROIButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除激活的ROI吗", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                RoiController.RemoveActive();
        }

        private void delAllROIButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除所有ROI吗", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                RoiController.Reset();
        }

        private void cMB_TuYa_radius_SelectedIndexChanged(object sender, EventArgs e)
        {
            ROI Tuya = RoiController.ROIList.Find(x => x.OperatorFlag == ROIOperation.Tuya);
            if (Tuya == null)
            {
                return;
            }
            double radius;
            if (!double.TryParse(cMB_TuYa_radius.Text, out radius))
            {
                MessageBox.Show("请输入数值或选择指定涂鸦半径半径", "转换错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RoiController.SetROISign(ROIOperation.Tuya);
            Tuya.SetTuYaRadius(radius);
        }

        private void cMB_TuYa_radius_TextChanged(object sender, EventArgs e)
        {
            ROI Tuya = RoiController.ROIList.Find(x => x.OperatorFlag == ROIOperation.Tuya);
            if (Tuya == null)
            {
                return;
            }
            double radius;
            if (!double.TryParse(cMB_TuYa_radius.Text, out radius))
            {
                MessageBox.Show("请输入数值或选择指定涂鸦半径半径", "转换错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Tuya.SetTuYaRadius(radius);
        }

        private void btn_TuYa_Click(object sender, EventArgs e)
        {


            double radius;
            ROI Tuya = RoiController.ROIList.Find(x => x.OperatorFlag == ROIOperation.Tuya);
            if (Tuya != null)
            {
                RoiController.ActiveRoiIdx = RoiController.ROIList.FindIndex(x => x.OperatorFlag == ROIOperation.Tuya);

                if (!double.TryParse(cMB_TuYa_radius.Text, out radius))
                {
                    MessageBox.Show("请输入数值或选择指定涂鸦半径半径", "转换错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Tuya.SetTuYaRadius(radius);

            }
            else
            {
                if (!double.TryParse(cMB_TuYa_radius.Text, out radius))
                {
                    MessageBox.Show("请输入数值或选择指定涂鸦半径半径", "转换错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                RoiController.SetROISign(ROIOperation.Tuya);
                RoiController.SetROIShape(new TuYa(radius));
            }



        }

        private void btn_ClearTuYA_Click(object sender, EventArgs e)
        {

            RoiController.ResetTuYa();

        }
    }
}
