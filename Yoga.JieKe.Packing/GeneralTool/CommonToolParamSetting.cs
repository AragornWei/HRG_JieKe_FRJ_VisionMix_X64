using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Yoga.ImageControl;
using Yoga.Tools;
using System.IO;
using System.Text.RegularExpressions;

namespace Yoga.JieKe.Packing.GeneralTool
{

    public partial class CommonToolParamSetting : ToolsSettingUnit
    {
        CommonTool commonTool;
        public HWndCtrl mView;
        public ROIController ROIController;
        private HImage CurrentImg;
        public CommonToolParamSetting(CommonTool commonTool)
        {
            InitializeComponent();
            this.commonTool = commonTool;
            mView = hWndUnit1.HWndCtrl;
            ROIController = new ROIController();
            ROIController.ROINotifyEvent += new EventHandler<ViewEventArgs>(UpdateROI);
            roiActUnit1.RoiController = ROIController;
            mView.useROIController(ROIController);
            ROIController.SetROISign(ROIOperation.Positive);
            commonTool.NotifyExcInfo = new ExeInfo(ExecuteInformation);
              
            locked = true;
            if (!commonTool.engineIsnitial)
            {
                commonTool.InitialEngine();
            }
            base.Init(commonTool.Name, commonTool);
            Init();
            locked = false;
            commonTool.NotifyExcInfo = new ExeInfo(ExecuteInformation);
            ExecuteInformation("初始化完成。");
        }

        private void Init()
        {
            InitCreateShapeMode();
            InitFindShapeMode();
            InitGrabPointSetting();
            Init_FangDai();
        }
        int messageCount = 0;
       public void ExecuteInformation(string message)
        {
            StringBuilder strb = new StringBuilder();
            if (message == string.Empty)
            {
                return;
            }
            messageCount++;
            strb.Append(message);
            strb.Append(Environment.NewLine);
            txt_Log.AppendText(strb.ToString());
            //超过1000行就删除500行
            if (messageCount > 1000)
            {
                string[] lines = txt_Log.Lines;
                List<string> a = lines.ToList();
                a.RemoveRange(0, 500);           //删除之前必须转换成链表。
                txt_Log.Lines = a.ToArray();
                messageCount = 500;
            }
            txt_Log.ScrollToCaret();

        }
        #region 初始化控件

        private void Init_FangDai()
        {
            nUpDown_FD_minThreshold.Value = (decimal)commonTool.MFangDai.minThreshold;
            nUpDown_FD_minArea.Value = (decimal)commonTool.MFangDai.minArea;
            nUpDown_FD_maxArea.Value = (decimal)commonTool.MFangDai.maxArea;
            checkBox_FangDai.Checked = commonTool.bFangDai_Enable;
            groupBox_FangDai.Enabled = commonTool.bFangDai_Enable;

        }
        private void InitGrabPointSetting()
        {
            txt_Grab_Row.Text = commonTool.MGrabPointSetting.GrabRowOrg.ToString("f2");
            txt_Grab_Col.Text = commonTool.MGrabPointSetting.GrabColOrg.ToString("f2");
            txt_Grab_Row.ReadOnly = true;
            txt_Grab_Col.ReadOnly = true;

            if (commonTool.CurrentShootPose != null && commonTool.CurrentShootPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(commonTool.CurrentShootPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(commonTool.CurrentShootPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(commonTool.CurrentShootPose[6].D.ToString());
                txt_CurrentGrabPose.Text = strb.ToString();
            }

            if (commonTool.ObjGrabPose != null && commonTool.ObjGrabPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(commonTool.ObjGrabPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(commonTool.ObjGrabPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(commonTool.ObjGrabPose[6].D.ToString());
                txt_GrabObjPose.Text = strb.ToString();
            }

            rbtn_Grab_From_Pictrue.Checked = commonTool.MGrabPointSetting.fromPictrue;
            rbtn_from_Robot.Checked = !rbtn_Grab_From_Pictrue.Checked;

            ckb_IsCalibration.Checked = commonTool.bIsCalibration;
            gb_IsCalibration.Enabled = ckb_IsCalibration.Checked;
            if (ckb_IsCalibration.Checked)
            {
                txt_CameraPath.Text = commonTool.cameraParamPath;
                txt_MpPosePath.Text = commonTool.MpInCamPosePath;
                txt_MpInToolPosePath.Text = commonTool.MpInToolPosePath;
            }

            txt_X_Compensation.Text = commonTool.x_Compensation.ToString();
            txt_Y_Compensation.Text = commonTool.y_Compensation.ToString();
            txt_Angle_Compensation.Text = commonTool.angle_Compensation.ToString();

        }
        private void InitFindShapeMode()
        {
            MinScoreUpDown.Value = (decimal)commonTool.MFindShapeMode.minScore;
            NumMatchesUpDown.Value = (decimal)commonTool.MFindShapeMode.numMatches;
            GreedinessUpDown.Value = (decimal)commonTool.MFindShapeMode.greediness;
            MaxOverlapUpDown.Value = (decimal)commonTool.MFindShapeMode.maxOverlap;
            SubPixelBox.Text = commonTool.MFindShapeMode.subPixel;
            LastPyrLevUpDown.Value = (decimal)commonTool.MFindShapeMode.numLevels;

        }
        private void InitCreateShapeMode()
        {
            if (commonTool.MCreateShapeModel.shapeModelROIList != null)
            {
                ROIController.ROIList = commonTool.MCreateShapeModel.shapeModelROIList;
            }
            ContrastHighUpDown.Value = (decimal)commonTool.MCreateShapeModel.contrastHigh;
            ContrastLowUpDown.Value = (decimal)commonTool.MCreateShapeModel.contrastLow;
            MinLenghtUpDown.Value = (decimal)commonTool.MCreateShapeModel.minLength;
            MinScaleUpDown.Value = (decimal)commonTool.MCreateShapeModel.scaleMin;
            MaxScaleUpDown.Value = (decimal)commonTool.MCreateShapeModel.scaleMax;
            StartingAngleUpDown.Value = (decimal)(commonTool.MCreateShapeModel.angleStart * 180.0 / Math.PI);
            AngleExtentUpDown.Value = (decimal)(commonTool.MCreateShapeModel.angleExtent * 180.0 / Math.PI);
            AngleStepUpDown.Value = (decimal)(commonTool.MCreateShapeModel.angleStep * 180.0 / Math.PI);
            PyramidLevelUpDown.Value = (decimal)commonTool.MCreateShapeModel.numLevels;
            MetricBox.Text = commonTool.MCreateShapeModel.metric;
            OptimizationBox.Text = commonTool.MCreateShapeModel.optimization;
            MinContrastUpDown.Value = (decimal)commonTool.MCreateShapeModel.minContrast;
            ScaleStepUpDown.Value = (decimal)commonTool.MCreateShapeModel.scaleStep;

        }

        #endregion
        //功能设定切换
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (tabControl.SelectedIndex)
            {
                case 0:
                    CreateShapeModeSetting();
                    break;
                case 1:
                    FindShapeModeSetting();
                    break;
                case 2:
                    GrabPointDispSetting();
                    break;
                case 3:
                    FangDai_Setting();
                    break;                
                default:
                    break;
            }
            mView.Repaint();
        }

        #region 功能切换设定

        private void FindShapeModeSetting()
        {
            roiActUnit1.Enabled = true;
            Panel_Test.Enabled = true;
            mView.SetDispLevel(ShowMode.IncludeROI);
            CurrentImg = commonTool.ImageRefIn;
            mView.ClearList();
            mView.AddIconicVar(commonTool.ImageRefIn);
            if (commonTool.MFindShapeMode.FindShapeModeRoiList != null && commonTool.MFindShapeMode.FindShapeModeRoiList.Count>0)
            {
                ROIController.ROIList = commonTool.MFindShapeMode.FindShapeModeRoiList;
            }
            else
            {
                ROIController.ROIList = new List<ROI>();
            }
            ROIController.ActiveRoiIdx = -1;
            bool var = commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, commonTool.ImageRefIn);
            if (var)
                commonTool.MFindShapeMode.ShowResult(mView);
        }

        private void CreateShapeModeSetting()
        {
            roiActUnit1.Enabled = true;
            Panel_Test.Enabled = false;
            mView.SetDispLevel(ShowMode.IncludeROI);
            CurrentImg = commonTool.ImageRefIn;
            mView.ClearList();
            mView.AddIconicVar(CurrentImg);
            if (commonTool.MCreateShapeModel.shapeModelROIList != null && commonTool.MCreateShapeModel.shapeModelROIList.Count>0)
            {
                ROIController.ROIList = commonTool.MCreateShapeModel.shapeModelROIList;
            }
            else
            {
                ROIController.ROIList = new List<ROI>(); ;
            }
            ROIController.ActiveRoiIdx = -1;
            commonTool.MCreateShapeModel.ShowShapeModel(mView);
        }

        private void FangDai_Setting()
        {
            if (checkBox_FangDai.Checked)
            {
                roiActUnit1.Enabled = true;
            }
            else
            {
                roiActUnit1.Enabled = false;
            }
            Panel_Test.Enabled = true;
            mView.SetDispLevel(ShowMode.IncludeROI);
            ROIController.ROIList = null;
            if (commonTool.MFangDai.FangDai_ROIList != null && commonTool.MFangDai.FangDai_ROIList.Count>0)
            {
                ROIController.ROIList = commonTool.MFangDai.FangDai_ROIList;
            }
            else
            {
                ROIController.ROIList = new List<ROI>();
            }
            ROIController.ActiveRoiIdx = -1;
            CurrentImg = commonTool.ImageRefIn;
            mView.AddIconicVar(CurrentImg);
            commonTool.Run(CurrentImg);
            mView.AddIconicVar(CurrentImg);
            if (commonTool.bFangDai_Result)
            {
                commonTool.MFangDai.Show(mView);
            }
        }


        #endregion
        private void UpdateROI(object sender, ViewEventArgs e)
        {
            switch (e.ViewMessage)
            {
                case ViewMessage.DelectedAllROIs:
                    switch (tabControl.SelectedIndex)
                    {
                        case 0:
                            commonTool.MCreateShapeModel.Reset();
                            mView.AddIconicVar(commonTool.ImageRefIn);
                            mView.Repaint();
                            break;
                        case 1:

                            commonTool.MFindShapeMode.Reset();
                            mView.AddIconicVar(commonTool.ImageRefIn);
                            mView.Repaint();
                            break;

                        case 3:
                            commonTool.MFangDai.Reset();
                            mView.AddIconicVar(commonTool.ImageRefIn);
                            mView.Repaint();
                            break;      
                        default:
                            break;
                    }
                    break;
                case ViewMessage.ChangedROISign:
                case ViewMessage.DeletedActROI:
                case ViewMessage.UpdateROI:
                case ViewMessage.CreatedROI:
                    switch (tabControl.SelectedIndex)
                    {
                        case 0:
                            UpdateShapeModelROI();
                            ShowShapeModel();
                            break;
                        case 1:
                            if (!ROIController.DefineModelROI())
                                return;
                            if (commonTool.MFindShapeMode.SearchRegion != null && commonTool.MFindShapeMode.SearchRegion.IsInitialized())
                            {
                                commonTool.MFindShapeMode.SearchRegion.Dispose();
                            }
                            commonTool.MFindShapeMode.SearchRegion = ROIController.GetModelRegion();
                            commonTool.MFindShapeMode.FindShapeModeRoiList = ROIController.ROIList;

                            CurrentImg = commonTool.ImageRefIn;                            

                            if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                            {
                                mView.AddIconicVar(CurrentImg);
                                commonTool.MFindShapeMode.ShowResult(mView);
                                mView.Repaint();
                            }
                            break;

                        case 3:
                            if (!ROIController.DefineModelROI())
                                return;
                            commonTool.MFangDai.FangDai_ROIList = ROIController.ROIList;
                            if (commonTool.MFangDai.FangDai_Region != null && commonTool.MFangDai.FangDai_Region.IsInitialized())
                            {
                                commonTool.MFangDai.FangDai_Region.Dispose();
                            }
                            commonTool.MFangDai.FangDai_Region = ROIController.GetModelRegion();
                            commonTool.MFangDai.FangDai_ROIList = ROIController.ROIList;
                            CurrentImg = commonTool.ImageRefIn;
                            commonTool.Run(CurrentImg);
                            FangDai_DetectionShow();
                            break;

                        default:
                            break;
                    }
                    break;
                case ViewMessage.ErrReadingImage:
                    break;
                default:
                    break;
            }
        }
        //启动时显示
        public override void ShowTranResult()
        {
            base.ShowTranResult();
            UpdateShapeModelROI();
            ShowShapeModel();
        }
        //窗口关闭退出时清除
        public override void Clear()
        {
            base.Clear();
            commonTool.StopDebugMode();
            if (modelPointXld != null && modelPointXld.IsInitialized())
                modelPointXld.Dispose();
            modelPointXld = null;
            CurrentImg = null;
        }

        #region 创建模板
        private void UpdateShapeModelROI()
        {
            if (!ROIController.DefineModelROI())
                return;
            if (commonTool.MCreateShapeModel.ModelRegion != null && commonTool.MCreateShapeModel.ModelRegion.IsInitialized())
            {
                commonTool.MCreateShapeModel.modelRegion.Dispose();
            }
            commonTool.MCreateShapeModel.modelRegion = ROIController.GetModelRegion();
            commonTool.MCreateShapeModel.shapeModelROIList = ROIController.ROIList;



            commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn);
        }
        private void ShowShapeModel()
        {
            mView.ClearList();
            mView.SetDispLevel(ShowMode.IncludeROI);
            mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
            mView.AddIconicVar(commonTool.ImageRefIn);
            mView.ChangeGraphicSettings(Mode.DRAWMODE, "margin");
            commonTool.MCreateShapeModel.ShowShapeModel(mView);
            mView.Repaint();
        }
        private void ContrastHighUpDown_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)ContrastHighUpDown.Value;
            ContrastLowUpDown.Maximum = (decimal)val;
            if (!locked)
            {
                commonTool.MCreateShapeModel.contrastHigh = val;
                if (commonTool.MCreateShapeModel.modelRegion == null || !commonTool.MCreateShapeModel.modelRegion.IsInitialized())
                    return;
                mView.ClearList();
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn))
                {
                    commonTool.MCreateShapeModel.ShowShapeModel(mView);
                }
                else
                {
                    MessageBox.Show("创建模板失败", "创建模板失败请检查参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                mView.Repaint();
            }

        }

        private void ContrastLowUpDown_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)ContrastLowUpDown.Value;
            ContrastHighUpDown.Minimum = (decimal)val;
            MinContrastUpDown.Maximum = (decimal)val;
            if (!locked)
            {
                commonTool.MCreateShapeModel.contrastLow = val;
                if (commonTool.MCreateShapeModel.modelRegion == null || !commonTool.MCreateShapeModel.modelRegion.IsInitialized())
                    return;
                mView.ClearList();
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn))
                {
                    commonTool.MCreateShapeModel.ShowShapeModel(mView);
                }
                else
                {
                    MessageBox.Show("创建模板失败", "创建模板失败请检查参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                mView.Repaint();
            }
        }

        private void MinContrastUpDown_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)MinContrastUpDown.Value;
            ContrastLowUpDown.Minimum = (decimal)val;

            if (!locked)
            {
                commonTool.MCreateShapeModel.minContrast = val;
                if (commonTool.MCreateShapeModel.modelRegion == null || !commonTool.MCreateShapeModel.modelRegion.IsInitialized())
                    return;
                mView.ClearList();
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn))
                {
                    commonTool.MCreateShapeModel.ShowShapeModel(mView);
                }
                else
                {
                    MessageBox.Show("创建模板失败", "创建模板失败请检查参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                mView.Repaint();
            }
        }

        private void MinLenghtUpDown_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)MinLenghtUpDown.Value;
            if (!locked)
            {
                commonTool.MCreateShapeModel.minLength = val;
                mView.ClearList();
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn))
                {
                    commonTool.MCreateShapeModel.ShowShapeModel(mView);
                }
                else
                {
                    MessageBox.Show("创建模板失败", "创建模板失败请检查参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                mView.Repaint();
            }
        }

        private void StartingAngleUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                commonTool.MCreateShapeModel.angleStart = (double)(StartingAngleUpDown.Value) / 180 * Math.PI;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void AngleExtentUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                commonTool.MCreateShapeModel.angleExtent = (double)(AngleExtentUpDown.Value) / 180 * Math.PI;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void AngleStepUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                commonTool.MCreateShapeModel.angleStep = (double)(AngleStepUpDown.Value) / 180 * Math.PI;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void MinScaleUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = MinScaleUpDown.Value;
            //if (val > MaxScaleUpDown.Value)
            //    MaxScaleUpDown.Value = val;
            MaxScaleUpDown.Minimum = val;
            if (!locked)
            {
                commonTool.MCreateShapeModel.scaleMin = (double)val;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void PyramidLevelUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!locked)
            {
                commonTool.MCreateShapeModel.numLevels = (int)PyramidLevelUpDown.Value;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void MaxScaleUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = MaxScaleUpDown.Value;
            MinScaleUpDown.Maximum = val;
            //if (MaxScaleUpDown.Value < MinScaleUpDown.Value)
            //{
            //    MinScaleUpDown.Value = MaxScaleUpDown.Value;
            //}
            if (!locked)
            {
                commonTool.MCreateShapeModel.scaleMax = (double)MaxScaleUpDown.Value;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }
        private void ScaleStepUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = ScaleStepUpDown.Value;


            if (!locked)
            {
                commonTool.MCreateShapeModel.scaleStep = (double)val;
                commonTool.MCreateShapeModel.createNewModelID = true;
            }
        }

        private void MetricBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            commonTool.MCreateShapeModel.metric = MetricBox.Text;
            commonTool.MCreateShapeModel.createNewModelID = true;
        }

        private void OptimizationBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            commonTool.MCreateShapeModel.optimization = OptimizationBox.Text;
            commonTool.MCreateShapeModel.createNewModelID = true;
        }

        private void btn_CreateModeShape_Click(object sender, EventArgs e)
        {
            if (commonTool.MCreateShapeModel.modelRegion != null && commonTool.MCreateShapeModel.modelRegion.IsInitialized())
            {
                mView.ClearList();
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (commonTool.MCreateShapeModel.CreateShapeModelAct(commonTool.ImageRefIn))
                {
                    commonTool.MCreateShapeModel.ShowShapeModel(mView);
                }
                else
                {
                    MessageBox.Show("创建模板失败", "创建模板失败请检查参数", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                mView.Repaint();
                MessageBox.Show("创建模板成功", "创建模板成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("没有设置模板区域，不能创建模板", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        #endregion

        #region 查找模板

        private void MinScoreUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = MinScoreUpDown.Value;
            commonTool.MFindShapeMode.minScore = (double)val;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }

        private void MaxOverlapUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = MaxOverlapUpDown.Value;
            commonTool.MFindShapeMode.maxOverlap = (double)val;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }

        private void NumMatchesUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = NumMatchesUpDown.Value;
            commonTool.MFindShapeMode.numMatches = (int)val;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }

        private void LastPyrLevUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = LastPyrLevUpDown.Value;
            commonTool.MFindShapeMode.numLevels = (int)val;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }

        private void GreedinessUpDown_ValueChanged(object sender, EventArgs e)
        {
            decimal val = GreedinessUpDown.Value;
            commonTool.MFindShapeMode.greediness = (double)val;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }

        private void SubPixelBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            commonTool.MFindShapeMode.subPixel = SubPixelBox.Text;
            if (ckbCycleFind.Checked)
            {
                if (commonTool.MFindShapeMode.FindShapeModeAct(commonTool.ImageRefIn, commonTool.MCreateShapeModel, CurrentImg))
                {
                    mView.AddIconicVar(CurrentImg);
                    commonTool.MFindShapeMode.ShowResult(mView);
                    mView.Repaint();
                }
            }
        }
        #endregion

        #region 测试
        private void loadTestImgButton_Click(object sender, EventArgs e)
        {
            string[] files;
            int count = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "选择图像文件";
            openFileDialog.Filter = "图像文件 |*.bmp;*.png;*.tif;*.jpg|all files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = openFileDialog.FileNames;
                count = files.Length;

                for (int i = 0; i < count; i++)
                {
                    if (commonTool.AddTestImages(files[i]))
                        testImgListBox.Items.Add(files[i]);
                }
                if (testImgListBox.Items.Count > 0)
                    testImgListBox.SelectedIndex = testImgListBox.Items.Count - 1;
                CurrentImg = commonTool.TestImageDic[(string)testImgListBox.SelectedItem];
                mView.SetDispLevel(ShowMode.ExcludeROI);
                mView.ClearList();
                mView.AddIconicVar(CurrentImg);
                mView.Repaint();
            }
        }

        private void deleteTestImgButton_Click(object sender, EventArgs e)
        {
            int count;
            if ((count = testImgListBox.SelectedIndex) < 0)
                return;

            string fileName = (string)testImgListBox.SelectedItem;

            if ((--count) < 0)
                count += 2;

            if ((count < testImgListBox.Items.Count))
            {
                testImgListBox.SelectedIndex = count;
            }

            commonTool.RemoveTestImage(fileName);
            testImgListBox.Items.Remove(fileName);
        }

        private void deleteAllTestImgButton_Click(object sender, EventArgs e)
        {
            if (testImgListBox.Items.Count > 0)
            {
                testImgListBox.Items.Clear();
                commonTool.RemoveTestImage();
                mView.ClearList();
                mView.SetDispLevel(ShowMode.ExcludeROI);
                mView.Repaint();
            }
        }
        bool isTestRunning;
        private int testImageIterator = 0;
        private void findModelButton_Click(object sender, EventArgs e)
        {
            if (testImgListBox.Items.Count == 0)
                return;
            if (isTestRunning)
                return;

            locked = true;
            isTestRunning = true;
            if (testImageIterator > testImgListBox.Items.Count - 1)
                testImageIterator = 0;
            testImgListBox.SelectedIndex = testImageIterator;

            string file;
            file = (string)testImgListBox.SelectedItem;
            CurrentImg = commonTool.TestImageDic[file];
            commonTool.ImageTestIn = CurrentImg;
            mView.SetDispLevel(ShowMode.ExcludeROI);
            mView.ClearList();
            mView.AddIconicVar(CurrentImg);
            commonTool.RunTest();
            commonTool.ShowResult(mView);
            mView.Repaint();
            if (ckbCycleFind.Checked && isTestRunning)
            {
                testImageIterator++;
            }
            isTestRunning = false;
            locked = false;
        }
        private void testImgListBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (testImgListBox.Items.Count == 0)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
                return;
            if (isTestRunning)
                return;

            string file;
            if (testImgListBox.Items.Count > 0)
            {
                file = (string)testImgListBox.SelectedItem;
                CurrentImg = commonTool.TestImageDic[file];
                mView.SetDispLevel(ShowMode.ExcludeROI);
                mView.ClearList();
                mView.AddIconicVar(CurrentImg);
                mView.Repaint();
            }
            else
            {
                mView.ClearList();
                mView.Repaint();
                return;
            }
        }
        private void testImgListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (testImgListBox.Items.Count == 0)
            {
                return;
            }
            if (isTestRunning)
                return;
            testImageIterator = testImgListBox.SelectedIndex;
            findModelButton_Click(null, null);
        }

        private void btn_Debug_Click(object sender, EventArgs e)
        {
            if (!commonTool.engineIsnitial)
            {
                commonTool.MyEngine = new HDevEngine();
                commonTool.InitialEngine();
            }
            if (btn_Debug.Text == "StartDebug")
            {
                commonTool.StartDebugMode();
                btn_Debug.Text = "StopDebug";
            }
            else
            {
                commonTool.StopDebugMode();
                btn_Debug.Text = "StartDebug";
            }

        }
        #endregion

        #region 抓取点设定
        //菜单切换抓取点时初始化设定
        private void GrabPointDispSetting()
        {
            roiActUnit1.Enabled = false;
            Panel_Test.Enabled = true;
            mView.SetDispLevel(ShowMode.ExcludeROI);
            CurrentImg = commonTool.ImageRefIn;
            mView.ClearList();
            mView.AddIconicVar(commonTool.ImageRefIn);

            if (modelPointXld == null)
                modelPointXld = new HXLDCont();
            if (modelPointXld != null && modelPointXld.IsInitialized())
                modelPointXld.Dispose();
            modelPointXld.GenCrossContourXld(commonTool.MGrabPointSetting.GrabRowOrg, commonTool.MGrabPointSetting.GrabColOrg, 30, 0);

            mView.ChangeGraphicSettings(Mode.COLOR, "red");
            mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
            mView.AddIconicVar(modelPointXld);
        }

        //从外部输入，还是从图像选取
        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            panel_From_Robot.Enabled = rbtn_from_Robot.Checked;
            panel_From_Pitrue.Enabled = rbtn_Grab_From_Pictrue.Checked;
            commonTool.MGrabPointSetting.fromPictrue = panel_From_Pitrue.Enabled;


            if (panel_From_Pitrue.Enabled)
            {
                panel_From_Pitrue.BackColor = Color.LightGreen;
            }
            else
            {
                panel_From_Pitrue.BackColor = Color.LightGray;
            }

            if (panel_From_Robot.Enabled)
            {
                panel_From_Robot.BackColor = Color.LightGreen;
            }
            else
            {
                panel_From_Robot.BackColor = Color.LightGray;
            }
        }
        //从图像选取抓取点初始化窗口及注册事件
        private void btn_Set_Grab_P_Click(object sender, EventArgs e)
        {
            MessageBox.Show("使用鼠标在左侧图像窗口，选取模板图像上一点，作为抓取点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mView.viewPort.MouseDown += GetMousePoint;
            mView.viewPort.MouseMove += MouseMovePoint;
            mView.ClearList();
            mView.SetDispLevel(ShowMode.ExcludeROI);
            mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
            mView.AddIconicVar(commonTool.ImageRefIn);
            mView.Repaint();
            btn_Set_Grab_P.Enabled = false;
        }
        //点击窗口设定抓取点
        double row, col;
        private void GetMousePoint(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
                txt_Grab_Row.Text = string.Format("{0:F2}", row);
                txt_Grab_Col.Text = string.Format("{0:F2}", col);
                commonTool.MGrabPointSetting.SetGrabPoint(row, col);
            }
            catch (HalconException)
            {
                return;
            }
            mView.viewPort.MouseDown -= GetMousePoint;
            mView.viewPort.MouseMove -= MouseMovePoint;
            btn_Set_Grab_P.Enabled = true;
        }
        //显示窗口鼠标位置辅助选取点
        HXLDCont modelPointXld;
        private void MouseMovePoint(object sender, MouseEventArgs e)
        {
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
                txt_Grab_Row.Text = string.Format("{0:F2}", row);
                txt_Grab_Col.Text = string.Format("{0:F2}", col);
                if (modelPointXld == null)
                    modelPointXld = new HXLDCont();
                if (modelPointXld != null && modelPointXld.IsInitialized())
                    modelPointXld.Dispose();
                modelPointXld.GenCrossContourXld(row, col, 30, 0);
                HXLDCont CicleXld = new HXLDCont();
                CicleXld.GenCircleContourXld(row, col, 50, 0, 2 * Math.PI, "positive", 2);
                HXLDCont temp = new HXLDCont();
                temp = modelPointXld.ConcatObj(CicleXld);
                modelPointXld.Dispose();
                modelPointXld = temp;
                mView.ClearList();
                mView.SetDispLevel(ShowMode.ExcludeROI);
                mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
                mView.AddIconicVar(commonTool.ImageRefIn);
                if (modelPointXld != null)
                {
                    mView.ChangeGraphicSettings(Mode.COLOR, "red");
                    mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                    mView.AddIconicVar(modelPointXld);
                }
                mView.Repaint();
            }

            catch (HalconException)
            {
                return;
            }

        }

        //读取相机参数文件
        private void btn_cameraParamPath_Click(object sender, EventArgs e)
        {
            string  file; 
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "选择相机参数文件";
            openFileDialog.Filter = "相机参数(cal)文件 |*.cal|all files (*.*)|*.*";

            string path = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
                txt_CameraPath.Text = file;
                commonTool.cameraParamPath = file;
                try
                {
                    HOperatorSet.ReadCamPar(file, out commonTool.cameraParam);
                }
                catch
                {
                    MessageBox.Show("读取相机参数失败，请检查文件该文件：" + file, "读取相机参数失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if(commonTool.cameraParam==null || commonTool.cameraParam.Length != 9)
                {
                    MessageBox.Show("相机参数文件错误，请检查该文件", "相机参数文件错误",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    commonTool.cameraParam = null;
                }

            }
        }
        //读取MpInCamPose文件
        private void btn_MpInCamPosePath_Click(object sender, EventArgs e)
        {
            string file;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "选择MpInCamPose系文件";
            openFileDialog.Filter = "MpInCamPose(dat)文件 |*.dat|all files (*.*)|*.*";

            string path = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
                txt_MpPosePath.Text = file;
                commonTool.MpInCamPosePath = file;
                try
                {
                    HOperatorSet.ReadPose(file, out commonTool.MPInCamPose);
                }
                catch
                {
                    MessageBox.Show("读取MpInCamPose文件失败，请检查文件该文件：" + file, "读取MpInCamPose文件失败",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (commonTool.MPInCamPose == null || commonTool.MPInCamPose.Length != 7)
                {
                    MessageBox.Show("MpInCamPose文件错误，请检查该文件", "MpInCamPose文件错误",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    commonTool.MPInCamPose = null;
                }

            }
        }
        //读取MpInTooPose文件
        private void btn_MPInToolPath_Click(object sender, EventArgs e)
        {
            string file;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "参考平面在Tool下姿势文件";
            openFileDialog.Filter = "MpInToolPose(dat)文件 |*.dat|all files (*.*)|*.*";

            string path = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialog.FileName;
                txt_MpPosePath.Text = file;
                commonTool.MpInToolPosePath = file;
                try
                {
                    HOperatorSet.ReadPose(file, out commonTool.MPInToolPose);
                }
                catch
                {
                    MessageBox.Show("读取MPInToolPose文件失败，请检查文件该文件：" + file, "读取MPInToolPose文件失败",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (commonTool.MPInToolPose == null || commonTool.MPInToolPose.Length != 7)
                {
                    MessageBox.Show("MPInToolPose文件错误，请检查该文件", "MPInToolPose文件错误",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    commonTool.MPInToolPose = null;
                }

            }
        }
        //是否标定输出
        private void ckb_IsCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            commonTool.bIsCalibration = ckb_IsCalibration.Checked;
            gb_IsCalibration.Enabled = ckb_IsCalibration.Checked;

            if (ckb_IsCalibration.Checked)
            {
                txt_CameraPath.Text = commonTool.cameraParamPath;
                txt_MpPosePath.Text = commonTool.MpInCamPosePath;
                txt_MpInToolPosePath.Text = commonTool.MpInToolPosePath;
            }
        }

        //拍照位置姿势
        private void txt_CurrentGrabPose_TextChanged(object sender, EventArgs e)
        {
            string[] splicts = Regex.Split(txt_CurrentGrabPose.Text, ";");
            if (splicts.Length > 7)
            {
                MessageBox.Show("Pose参数大于7", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (splicts.Length != 7)
            {
                return;
            }
            HTuple tempPose = new HTuple();
            for (int i = 0; i < splicts.Length; i++)
            {
                if (i != 6)
                {
                    double temp;
                    if (double.TryParse(splicts[i], out temp))
                    {
                        tempPose.TupleConcat(new HTuple(temp));
                    }
                    else
                    {
                        MessageBox.Show("请输入数值,并用; 隔开", "提示" + (i + 1) + "非数值", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    int temp;
                    if (int.TryParse(splicts[i], out temp))
                    {
                        if (temp != 1 || temp != 2)
                        {
                            MessageBox.Show("请输入整数(根据机器人型号输入对应整数),并用; 隔开", "提示" + (i + 1) + "数值不正确", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        tempPose.TupleConcat(new HTuple(temp));
                    }
                    else
                    {
                        MessageBox.Show("请输入数值,并用; 隔开", "提示" + (i + 1) + "非数值", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            if (tempPose.Length == 7)
            {
                commonTool.CurrentShootPose = tempPose;
            }
        }
        //抓取初始位置姿势
        private void txt_GrabObjPose_TextChanged(object sender, EventArgs e)
        {
            string[] splicts = Regex.Split(txt_GrabObjPose.Text, ";");
            if (splicts.Length > 7)
            {
                MessageBox.Show("Pose参数大于7", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (splicts.Length != 7)
            {
                return;
            }
            HTuple tempPose = new HTuple();
            for (int i = 0; i < splicts.Length; i++)
            {
                if (i != 6)
                {
                    double temp;
                    if (double.TryParse(splicts[i], out temp))
                    {
                        tempPose.TupleConcat(new HTuple(temp));
                    }
                    else
                    {
                        MessageBox.Show("请输入数值,并用; 隔开", "提示" + (i + 1) + "非数值", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    int temp;
                    if (int.TryParse(splicts[i], out temp))
                    {
                        if (temp != 1 || temp != 2)
                        {
                            MessageBox.Show("请输入整数(根据机器人型号输入对应整数),并用; 隔开", "提示" + (i + 1) + "数值不正确", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        tempPose.TupleConcat(new HTuple(temp));
                    }
                    else
                    {
                        MessageBox.Show("请输入数值,并用; 隔开", "提示" + (i + 1) + "非数值", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            if (tempPose.Length == 7)
            {
                commonTool.ObjGrabPose = tempPose;
            }
        }
        //计算抓取
        private void btn_Sure_RobotCoor_Click(object sender, EventArgs e)
        {
            if (commonTool.CurrentShootPose == null || commonTool.CurrentShootPose.Length != 7)
            {
                MessageBox.Show("请先输入拍照位手臂姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (commonTool.ObjGrabPose == null || commonTool.ObjGrabPose.Length != 7)
            {
                MessageBox.Show("请先输入抓取缝纫机位置手臂姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (commonTool.MPInCamPose == null || commonTool.MPInCamPose.Length != 7)
            {
                MessageBox.Show("请加载参考平面姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (commonTool.cameraParam == null || commonTool.cameraParam.Length != 9)
            {
                MessageBox.Show("相机参数文件错误", "相机参数文件错误，请检查该文件", MessageBoxButtons.OK, MessageBoxIcon.Error);
                commonTool.cameraParam = null;
            }
            if (commonTool.MPInToolPose == null || commonTool.MPInToolPose != 7)
            {
                MessageBox.Show("请加载参考平面在工具坐标系下姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HTuple BaseInToolPose;
            HOperatorSet.PoseInvert(commonTool.CurrentShootPose, out BaseInToolPose);
            HTuple ObjInToolPose;
            HOperatorSet.PoseCompose(BaseInToolPose, commonTool.ObjGrabPose, out ObjInToolPose);//type2

            HTuple ToolInMpPose;
            HOperatorSet.PoseInvert(commonTool.MPInToolPose, out ToolInMpPose);//type0
            HTuple ToolInMpHomMat;
            HOperatorSet.PoseToHomMat3d(ToolInMpPose, out ToolInMpHomMat);
            HTuple Obj_MP_X, Obj_MP_Y, Obj_MP_Z;
            HOperatorSet.AffineTransPoint3d(ToolInMpHomMat, ObjInToolPose[0], ObjInToolPose[1], ObjInToolPose[1], out Obj_MP_X, out Obj_MP_Y, out Obj_MP_Z);

            commonTool.SetGrabPoint(Obj_MP_X.D, Obj_MP_Y.D, Obj_MP_Z);

            txt_Grab_Row.Text = string.Format("{0:F2}", commonTool.MGrabPointSetting.GrabRowOrg);
            txt_Grab_Col.Text = string.Format("{0:F2}", commonTool.MGrabPointSetting.GrabColOrg);
            GrabPointDispSetting();
            mView.Repaint();
        }
        private void txt_X_Compensation_TextChanged(object sender, EventArgs e)
        {
            double a;
            if(double.TryParse(txt_X_Compensation.Text,out a))
            {
                if (a < 10 && a>-10)
                {
                    commonTool.x_Compensation = a;
                }
                else
                {
                    MessageBox.Show("输入数值过大", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("请输入数值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txt_Y_Compensation_TextChanged(object sender, EventArgs e)
        {
            double a;
            if (double.TryParse(txt_Y_Compensation.Text, out a))
            {
                if (a < 10 && a > -10)
                {
                    commonTool.y_Compensation = a;
                }
                else
                {
                    MessageBox.Show("输入数值过大", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("请输入数值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txt_Angle_Compensation_TextChanged(object sender, EventArgs e)
        {
            double a;
            if (double.TryParse(txt_Angle_Compensation.Text, out a))
            {
                if (a < 10 && a > -10)
                {
                    commonTool.angle_Compensation = a;
                }
                else
                {
                    MessageBox.Show("输入数值过大", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("请输入数值", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 防呆
        private void FangDai_DetectionShow()
        {
            if (commonTool.bFangDai_Result)
            {
                mView.ClearList();
                mView.AddIconicVar(CurrentImg);
                commonTool.MFangDai.Show(mView);
            }
            mView.Repaint();
        }
        private void checkBox_FangDai_CheckedChanged(object sender, EventArgs e)
        {
            commonTool.bFangDai_Enable = checkBox_FangDai.Checked;
            groupBox_FangDai.Enabled = commonTool.bFangDai_Enable;
            if (checkBox_FangDai.Checked)
            {
                roiActUnit1.Enabled = true;
            }
            else
            {
                roiActUnit1.Enabled = false;
            }
        }
        private void nUpDown_FD_minThreshold_ValueChanged(object sender, EventArgs e)
        {
            decimal val = nUpDown_FD_minThreshold.Value;
            commonTool.MFangDai.minThreshold = (int)val;
            if (ckbCycleFind.Checked)
            {
                commonTool.Run(CurrentImg);
                FangDai_DetectionShow();
            }
        }

        private void nUpDown_FD_minArea_ValueChanged(object sender, EventArgs e)
        {
            decimal val = nUpDown_FD_minArea.Value;
            commonTool.MFangDai.minArea = (int)val;
            if (ckbCycleFind.Checked)
            {
                commonTool.Run(CurrentImg);
                FangDai_DetectionShow();
            }
        }



        private void nUpDown_FD_maxArea_ValueChanged(object sender, EventArgs e)
        {
            decimal val = nUpDown_FD_maxArea.Value;
            commonTool.MFangDai.maxArea = (int)val;
            if (ckbCycleFind.Checked)
            {
                commonTool.Run(CurrentImg);
                FangDai_DetectionShow();
            }
        }
        #endregion
    }
}
