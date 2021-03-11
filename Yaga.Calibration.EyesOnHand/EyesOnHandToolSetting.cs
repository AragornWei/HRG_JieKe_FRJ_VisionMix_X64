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
using System.Threading;
using System.Text.RegularExpressions;


namespace Yoga.Calibration.EyesOnHand
{
    public partial class CalibrationSetting : ToolsSettingUnit
    {
        private EyesOnHandTool tool;
        public HWndCtrl mView;
        HTuple startCamPar;
        HTuple CalibDataID;
        HObject ho_Caltab, ho_Cross;
        HXLDCont modelPointXld;
        HImage testImage;
        List<HImage> calHImages = new List<HImage>();
        int index;
        Thread acqContinus;
        bool acqIsrun=false;
        public CalibrationSetting(EyesOnHandTool calibraion)
        {
            InitializeComponent();
            this.tool = calibraion;
            mView = hWndUnit1.HWndCtrl;
            tool.NotifyExcInfo = ExecuteInformation;
            base.Init(tool.Name, tool);
            locked = true;
            base.Init(tool.Name, tool);
            base.HideMax();
            base.HideMin();
            Init();
            locked = false;
        }

        #region 初始化
        private void Init()
        {
            gb_SettingPlanPose.Enabled = false;
            InitCameraParam();
            InitCalibrationCam_Robot();
            InitComp();
        }

        private void InitComp()
        {
            nUD_toolRotateAngle.Value = (decimal)tool.angle;
            if (tool.MPInToolPose != null && tool.MPInToolPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPInToolPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPInToolPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPInToolPose[6].D.ToString());
                txt_MPInToolPose.Text = strb.ToString();                
            }

            txt_MarkInToolCoord_Calculation.Text = tool.MarkInToolCoord;
            txt_Cal_Comp_X.Text = tool.X0.ToString("F6");
            txt_Cal_Comp_Y.Text = tool.Y0.ToString("F6");

            //当前拍照姿势
            if(tool.CurrentGrabPose!=null && tool.CurrentGrabPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.CurrentGrabPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.CurrentGrabPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.CurrentGrabPose[6].D.ToString());
                txt_CurrentToolInBasePose.Text = strb.ToString();
            }
            //Mark点姿势
            if (tool.MarkInBasePoseMenu != null && tool.MarkInBasePoseMenu.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MarkInBasePoseMenu[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MarkInBasePoseMenu[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MarkInBasePoseMenu[6].D.ToString());
                txt_MarkInBasePose_Menu.Text = strb.ToString();
            }            

            txt_Men_Comp_X.Text = tool.X_Comp_T.ToString("F6");
            txt_Men_Comp_Y.Text = tool.Y_Comp_T.ToString("F6");
        }

        private void InitCalibrationCam_Robot()
        {
            this.dGV_worldPose.DataSource = tool.Points;
            this.dGV_worldPose.Refresh();
            if (tool.MPInCamToolPose != null && tool.MPInCamToolPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPInCamToolPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPInCamToolPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPInCamToolPose[6].D.ToString());
                txt_MPInCamToolPose.Text = strb.ToString();
            }
        }

        private void InitCameraParam()
        {
            if (tool.cameraParam != null && tool.cameraParam.Length == 9)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(tool.cameraParam[0].S + ",");
                for (int i = 1; i < tool.cameraParam.Length; i++)
                {
                    if (i == 3 || i == 4)
                    {
                        strb.Append((tool.cameraParam[i].D * 1000000).ToString("F3"));
                    }
                    else if (i == 7 || i == 8)
                    {
                        strb.Append(tool.cameraParam[i].I.ToString());
                    }
                    else
                    {
                        if (i == 1)
                        {
                            strb.Append(tool.cameraParam[i].D.ToString("F4"));
                        }
                        else
                        {
                            strb.Append(tool.cameraParam[i].D.ToString("F3"));
                        }
                    }


                    if (i < tool.cameraParam.Length - 1)
                        strb.Append(",");
                }
                txt_CameraParam.Text = strb.ToString();
            }
            else
            {
                tool.cameraParam = null;
            }

            if (tool.MPPose != null && tool.MPPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPPose[6].D.ToString());
                txt_planPose.Text = strb.ToString();
            }
            else
            {
                tool.MPPose = null;
            }

            NUpDown_Focus.Value = (decimal)tool.Focus;
            NUpDown_Sx.Value = (decimal)tool.Sx;
            NUpDown_Sy.Value = (decimal)tool.Sy;
            nUpDown_ImageWidth.Value = (decimal)tool.ImageWidth;
            nUpDown_ImageHeight.Value = (decimal)tool.ImageHeight;
            gb_StartCalibration.Enabled = ckb_StartCalibration.Checked;
            txt_descpPath.Text = tool.descrPath;
            nUP_Caltab_H.Value = (decimal)tool.CaltabHeight;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);

        }

        #region gen_cam_par_area_scan_division
        public void gen_cam_par_area_scan_division(HTuple hv_Focus, HTuple hv_Kappa, HTuple hv_Sx, HTuple hv_Sy, HTuple hv_Cx, HTuple hv_Cy, HTuple hv_ImageWidth, HTuple hv_ImageHeight, out HTuple hv_CameraParam)
        {
            hv_CameraParam = new HTuple();
            hv_CameraParam[0] = "area_scan_division";
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Focus);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Kappa);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Sx);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Sy);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Cx);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_Cy);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_ImageWidth);
            hv_CameraParam = hv_CameraParam.TupleConcat(hv_ImageHeight);
            return;
        }
        #endregion
        #endregion

        #region 相机参数设定
        #region 参数设定
        private void NUpDown_Focus_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.Focus = (int)NUpDown_Focus.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void NUpDown_Sx_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.Sx = (double)NUpDown_Sx.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void NUpDown_Sy_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.Sy = (double)NUpDown_Sy.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void nUpDown_ImageWidth_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.ImageWidth = (int)nUpDown_ImageWidth.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void nUpDown_ImageHeight_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.ImageHeight = (int)nUpDown_ImageHeight.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void nUP_Caltab_H_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.CaltabHeight = (double)nUP_Caltab_H.Value;
        }
        #endregion


        private void btn_ContinusOrStop_Click(object sender, EventArgs e)
        {
            if (acqIsrun)
            {
                acqIsrun = false;
                return;
            }
            acqContinus = new Thread(new ThreadStart(AcqContinusImage));
            acqContinus.IsBackground = true;
            acqIsrun = true;
            acqContinus.Start();         
        }
        public void AcqContinusImage()
        {
            while (acqIsrun)
            {
                if (testImage != null && testImage.IsInitialized())
                {
                    testImage.Dispose();
                }
                testImage = tool.GetImage();
                this.Invoke(new Action(() =>
                {
                    mView.ClearList();
                    mView.AddIconicVar(testImage);
                    mView.Repaint();

                }));
            }
        }

        public void CloseAcq()
        {
            if (acqIsrun)
            {
                acqIsrun = false;
                if (acqContinus != null && acqContinus.IsAlive)
                {
                    try
                    {
                        acqContinus.Abort();
                    }
                    catch
                    {

                    }

                }
            }
            
        }

        //开始标定相机参数
        private void ckb_StartCalibration_CheckedChanged(object sender, EventArgs e)
        {

            if (tool.descrPath == null || tool.descrPath == string.Empty)
            {
                MessageBox.Show("读取描述文件", "请选择读取对应标定板描述文件。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                FileInfo fi = new FileInfo(tool.descrPath);
                if (!fi.Exists)
                {
                    MessageBox.Show("读取描述文件", "描述文件不存在。", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (ckb_StartCalibration.Checked)
            {
                gB_InitCamParam.Enabled = false;
                gb_StartCalibration.Enabled = true;
                btn_grabCamera.Enabled = true;
                if (calHImages == null)
                {
                    calHImages = new List<HImage>();
                }
                else
                {
                    if (calHImages.Count > 0)
                    {
                        for (int i = 0; i < calHImages.Count; i++)
                        {
                            calHImages[i].Dispose();
                        }
                    }
                    calHImages.Clear();
                    cbb_CalibImages.Items.Clear();
                }
            }
            else
            {
                gB_InitCamParam.Enabled = true;
                gb_StartCalibration.Enabled = false;
            }
            index = 1;
            if (CalibDataID != null)
            {
                if (CalibDataID.Length == 0)
                {
                    HOperatorSet.ClearCalibData(CalibDataID);
                }
            }
            if (ckb_StartCalibration.Checked)
            {
                HOperatorSet.CreateCalibData("calibration_object", 1, 1, out CalibDataID);

                HOperatorSet.SetCalibDataCamParam(CalibDataID, 0, new HTuple(), startCamPar);
                try
                {
                    HOperatorSet.SetCalibDataCalibObject(CalibDataID, 0, tool.descrPath);
                }
                catch
                {
                    MessageBox.Show("描述文件错误", "读取描述失败，请检查文件该文件：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }

        }
        //拍照并识别
        private void btn_grabCamera_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                return;
            }
            if (calHImages == null)
            {
                calHImages = new List<HImage>();
            }
            index = calHImages.Count + 1;
            try
            {
                HOperatorSet.FindCalibObject(testImage, CalibDataID, 0, 0, index, new HTuple(),
                    new HTuple());
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HTuple hv_Exception;
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                MessageBox.Show("提取原点错误", "请注意不要遮挡或者拍摄时抖动", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mView.AddIconicVar(testImage);
                mView.Repaint();
                return;
            }

            calHImages.Add(testImage.Clone());

            if (index == 1)
            {
                cbb_CalibImages.Items.Clear();
            }
            cbb_CalibImages.Items.Add(index.ToString());
            cbb_CalibImages.SelectedIndex = index - 1;

        }
        //显示图片与图形
        private void cbb_CalibImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = int.Parse(cbb_CalibImages.SelectedItem.ToString());
            DisplayCalib(calHImages[index - 1]);
        }
        private void DisplayCalib(HImage image)
        {
            if (ho_Caltab == null)
            {
                ho_Caltab = new HObject();
                ho_Caltab.GenEmptyObj();
            }
            ho_Caltab.Dispose();
            HOperatorSet.GetCalibDataObservContours(out ho_Caltab, CalibDataID, "caltab", 0, 0, index);
            HTuple hv_Row, hv_Column, hv_Index, hv_Pose;
            HOperatorSet.GetCalibDataObservPoints(CalibDataID, 0, 0, index, out hv_Row, out hv_Column, out hv_Index, out hv_Pose);
            if (ho_Cross == null)
            {
                ho_Cross = new HObject();
                ho_Cross.GenEmptyObj();
            }
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row, hv_Column, 30, 0.785398);

            mView.ClearList();
            if (image != null && image.IsInitialized())
            {
                mView.AddIconicVar(image);
            }
            if (ho_Caltab != null && ho_Caltab.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "green");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 3);
                mView.AddIconicVar(ho_Caltab);
            }
            if (ho_Cross != null && ho_Cross.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "blue");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                mView.AddIconicVar(ho_Cross);
            }
            mView.AddText(index.ToString(), 100, 100, 80, "green");
            mView.Repaint();
        }
        //设定参考平面
        private void btn_CalibartionPlan_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.MPImage != null && tool.MPImage.IsInitialized())
            {
                tool.MPImage.Dispose();
            }
            tool.MPImage = calHImages[index - 1].CopyImage();
            HTuple tempPose;
            HOperatorSet.GetCalibData(CalibDataID, "calib_obj_pose", (new HTuple(0)).TupleConcat(index), "pose", out tempPose);
            double caltab_H_m = tool.CaltabHeight / 1000;
            HOperatorSet.SetOriginPose(tempPose, 0, 0, new HTuple(caltab_H_m), out tool.MPPose);

            if (tool.MPPose != null && tool.MPPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPPose[6].D.ToString());
                txt_MPInCamToolPose.Text = strb.ToString();
                txt_planPose.Text = strb.ToString();
                ExecuteInformation("参考平面设定成功");
            }
            else
            {
                tool.MPPose = null;
            }
        }
        //打开标定板描述文件
        private void btn_openDescrpPath_Click(object sender, EventArgs e)
        {
            string file;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "标定板描述文件";
            openFileDialog.Filter = "描述(.descr)文件 |*.descr|all files (*.*)|*.*";

            string path = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            openFileDialog.InitialDirectory = path;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tool.descrPath = openFileDialog.FileName;
                txt_descpPath.Text = openFileDialog.FileName;
            }
        }
        //保存相机参数
        private void btn_saveParam_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.cameraParam == null || tool.cameraParam.Length != 9)
            {
                MessageBox.Show("相机参数错误，请重新标定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "相机内参文件(.cal)|*.cal|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WriteCamPar(tool.cameraParam, sfd.FileName);
                ExecuteInformation("相机参数文件保存成功");
            }


        }
        HTuple hv_Error;
        //标定
        private void btn_calibrateCamParam_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (calHImages.Count < 16)
            {
                MessageBox.Show("警告，采集图像过少，请继续采集图像(含多角度图像)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                HOperatorSet.CalibrateCameras(CalibDataID, out hv_Error);
                HOperatorSet.GetCalibData(CalibDataID, "camera", 0, "params", out tool.cameraParam);
                StringBuilder strb = new StringBuilder();
                strb.Append(tool.cameraParam[0].S + ",");
                for (int i = 1; i < tool.cameraParam.Length; i++)
                {
                    if (i == 3 || i == 4)
                    {
                        ExecuteInformation((tool.cameraParam[i].D * 1000000).ToString("F3") + ";" + Environment.NewLine);
                        strb.Append((tool.cameraParam[i].D * 1000000).ToString("F3"));
                    }
                    else if (i == 7 || i == 8)
                    {
                        ExecuteInformation(tool.cameraParam[i].I.ToString() + ";" + Environment.NewLine);
                        strb.Append(tool.cameraParam[i].I.ToString());
                    }
                    else
                    {
                        if (i == 2)
                        {
                            ExecuteInformation(tool.cameraParam[i].D.ToString("F4") + ";" + Environment.NewLine);
                            strb.Append(tool.cameraParam[i].D.ToString("F4"));
                        }
                        else
                        {
                            ExecuteInformation(tool.cameraParam[i].D.ToString("F4") + ";" + Environment.NewLine);
                            strb.Append(tool.cameraParam[i].D.ToString("F3"));
                        }
                    }

                    if (i < tool.cameraParam.Length - 1)
                    {
                        strb.Append(",");
                    }
                }
                txt_CameraParam.Text = strb.ToString();
                gb_SettingPlanPose.Enabled = true;
                btn_grabCamera.Enabled = false;
                // btn_grabCamera.Enabled = false;
            }

            catch (HalconException HDevExpDefaultException1)
            {
                HTuple hv_Exception;
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                MessageBox.Show("标定错误", hv_Exception.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ExecuteInformation("相机参数标定成功");
            try
            {
                ExecuteInformation("标定误差： " + hv_Error.D.ToString("F3") + "pixel");
            }
            catch
            {

            }

        }

        //保存参考平面
        private void btn_SaveWorldPose_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.MPPose == null || tool.MPPose.Length != 7)
            {
                MessageBox.Show("请先参考平面坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "世界\\参考平面坐标系文件(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.MPPose, sfd.FileName);
                ExecuteInformation("参考平面坐标系文件保存成功");
            }
        }

        #endregion

        #region 标定相机与手臂




        //拍照并识别标定板，后转化成相机光轴坐标。
        private void btn_grabImage_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.cameraParam == null || tool.cameraParam.Length != 9)
            {
                MessageBox.Show("相机内参错误", "请先标定相机内参", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MPPose == null || tool.MPPose.Length != 7)
            {
                MessageBox.Show("参考平面姿势错误", "请先标定检测参考平面", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show("获取图像失败", "请检查相机或重新获取图像", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (ho_Caltab == null)
                {
                    ho_Caltab = new HObject();
                    ho_Caltab.GenEmptyObj();
                }
                ho_Caltab.Dispose();
                if (ho_Cross == null)
                {
                    ho_Cross = new HObject();
                    ho_Cross.GenEmptyObj();
                }
                ho_Cross.Dispose();

                HTuple refPlanPose = null;
                HTuple refPlanPoseTemp = null;
                get_calib_plate_pose(testImage, tool.cameraParam, tool.descrPath, out refPlanPose, out ho_Caltab, out ho_Cross);
                HOperatorSet.SetOriginPose(refPlanPose, 0, 0, new HTuple(tool.CaltabHeight / 1000), out refPlanPoseTemp);
                HTuple camInMPPose = null;
                HOperatorSet.PoseInvert(refPlanPoseTemp, out camInMPPose);

                if (dGV_worldPose.SelectedCells.Count < 1)
                {
                    MessageBox.Show("请选择一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int index = dGV_worldPose.SelectedCells[0].RowIndex;
                string message = string.Format("是否修改点{0}的像素数据?", index + 1);
                DialogResult r1 = MessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (r1 != DialogResult.Yes)
                {
                    return;
                }
                txt_Row.Text = string.Format("{0:F6}", camInMPPose[0].D);
                txt_Col.Text = string.Format("{0:F6}", camInMPPose[1].D);
                txt_CamZ.Text = string.Format("{0:F6}", camInMPPose[2].D);

                double cam_x_temp, cam_y_temp, cam_z_temp;
                double.TryParse(txt_Row.Text, out cam_x_temp);
                double.TryParse(txt_Col.Text, out cam_y_temp);
                double.TryParse(txt_CamZ.Text, out cam_z_temp);
                tool.Points[index].C_x = cam_x_temp;
                tool.Points[index].C_y = cam_y_temp;
                tool.Points[index].C_z = cam_z_temp;
                dGV_worldPose.Refresh();
            }
            catch
            {
                mView.ClearList();
                mView.AddIconicVar(testImage);
                mView.Repaint();
                MessageBox.Show("错误！", "无法有效识别标定板", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mView.ClearList();
            mView.AddIconicVar(testImage);
            if (ho_Caltab != null && ho_Caltab.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "green");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 3);
                mView.AddIconicVar(ho_Caltab);
            }
            if (ho_Cross != null && ho_Cross.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "blue");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                mView.AddIconicVar(ho_Cross);
            }
            mView.Repaint();

        }
        public void get_calib_plate_pose(HObject ho_Image, HTuple hv_CameraParam, HTuple hv_CalibObjDescr, out HTuple hv_Pose, out HObject ho_caltab, out HObject ho_cross)
        {
            HTuple hv_CalibDataID = null;

            // Initialize local and output iconic variables 
            HOperatorSet.CreateCalibData("calibration_object", 1, 1, out hv_CalibDataID);
            HOperatorSet.SetCalibDataCamParam(hv_CalibDataID, 0, new HTuple(), hv_CameraParam);
            HOperatorSet.SetCalibDataCalibObject(hv_CalibDataID, 0, hv_CalibObjDescr);
            HOperatorSet.FindCalibObject(ho_Image, hv_CalibDataID, 0, 0, 1, new HTuple(), new HTuple());
            HOperatorSet.GetCalibDataObservPose(hv_CalibDataID, 0, 0, 1, out hv_Pose);
            HOperatorSet.GetCalibDataObservContours(out ho_caltab, hv_CalibDataID, "caltab", 0, 0, 1);
            HTuple row, col, index, pose;
            HOperatorSet.GetCalibDataObservPoints(hv_CalibDataID, 0, 0, 1, out row, out col, out index, out pose);
            HOperatorSet.GenCrossContourXld(out ho_cross, row, col, 30, 0);
            HOperatorSet.ClearCalibData(hv_CalibDataID);
            return;
        }


        //标定
        double row, col;
        private void btn_CalibrationCoord_Click(object sender, EventArgs e)
        {
            CloseAcq();
            tool.Calibration();
            if (tool.MPInCamToolPose == null || tool.MPInCamToolPose.Length!=7)
            {
                MessageBox.Show("标定失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ExecuteInformation("标定成功");
        }
        //复制粘贴数据
        private void dGV_worldPose_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control && e.KeyCode == Keys.V)
            {
                if (sender != null && sender.GetType() == typeof(DataGridView))
                {
                    try
                    {
                        // 获取剪切板的内容，并按行分割
                        string pasteText = Clipboard.GetText();
                        if (string.IsNullOrEmpty(pasteText))
                            return;
                        string[] lines = pasteText.Split(Environment.NewLine.ToCharArray());

                        BindingList<CalibPair> points = new BindingList<CalibPair>();
                        foreach (string line in lines)
                        {
                            if (string.IsNullOrEmpty(line.Trim()))
                                continue;
                            // 按 Tab 分割数据
                            string[] vals = line.Split('\t');
                            CalibPair pos = new CalibPair();
                            pos.C_x = double.Parse(vals[0]);
                            pos.C_y = double.Parse(vals[1]);
                            pos.C_z = double.Parse(vals[2]);
                            pos.R_X = double.Parse(vals[3]);
                            pos.R_Y = double.Parse(vals[4]);
                            points.Add(pos);
                        }
                        if (points.Count == 9)
                        {
                            tool.Points = points;
                            this.dGV_worldPose.DataSource = tool.Points;
                            this.dGV_worldPose.Refresh();
                        }
                        else
                        {
                            throw new Exception("数据格式不对,需要9*9表格数据");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据粘贴异常:" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void dGV_worldPose_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString();
        }
        private void rdbtnPos_CheckedChanged(object sender, EventArgs e)
        {
            int index = int.Parse(((RadioButton)sender).Text);
            dGV_worldPose.ClearSelection();
            dGV_worldPose.Rows[index - 1].Cells[0].Selected = true;
        }

        //保存手眼相对标定
        private void btn_SaveCamRobotCal_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.MPInCamToolHomMat == null)
            {
                MessageBox.Show("请先做九点标定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "九点标定文件(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.MPInCamToolPose, sfd.FileName);
                ExecuteInformation("九点标定参数文件保存成功");
            }
        }



        #endregion

        #region 补偿与测试

        #region 计算补偿==计算tool Z轴位置

        private void nUD_toolRotateAngle_ValueChanged(object sender, EventArgs e)
        {
            if (locked)
                return;
            tool.angle = (double)nUD_toolRotateAngle.Value;
        }
        private void btn_SetCalPoint_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.cameraParam == null || tool.cameraParam.Length != 9)
            {
                MessageBox.Show("相机内参错误", "请先标定相机内参", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MPPose == null || tool.MPPose.Length != 7)
            {
                MessageBox.Show("参考平面姿势错误", "请先标定检测参考平面", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MPInCamToolPose == null || tool.MPInCamToolPose.Length != 7)
            {
                MessageBox.Show( "请先做九点标定", "参考平面姿势错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show( "请检查相机或重新获取图像", "获取图像失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (ho_Caltab == null)
                {
                    ho_Caltab = new HObject();
                    ho_Caltab.GenEmptyObj();
                }
                ho_Caltab.Dispose();
                if (ho_Cross == null)
                {
                    ho_Cross = new HObject();
                    ho_Cross.GenEmptyObj();
                }
                ho_Cross.Dispose();

                HTuple refPlanPose = null;
                HTuple refPlanPoseTemp = null;
                get_calib_plate_pose(testImage, tool.cameraParam, tool.descrPath, out refPlanPose, out ho_Caltab, out ho_Cross);
                HOperatorSet.SetOriginPose(refPlanPose, 0, 0, new HTuple(tool.CaltabHeight / 1000), out refPlanPoseTemp);
                HTuple camInMPPose = null;
                HOperatorSet.PoseInvert(refPlanPoseTemp, out camInMPPose);
                HTuple xtemp, ytemp, ztemp;
                HOperatorSet.AffineTransPoint3d(tool.MPInCamToolHomMat, camInMPPose[0], camInMPPose[1], camInMPPose[2], out xtemp, out ytemp, out ztemp);
                if(tool.CalToolPointX==null || tool.CalToolPointX.Length >= 2 || tool.CalToolPointY == null || tool.CalToolPointY.Length >= 2)
                {
                    tool.CalToolPointX = new HTuple();
                    tool.CalToolPointY = new HTuple();
                }
                tool.CalToolPointX.TupleConcat(xtemp);
                tool.CalToolPointY.TupleConcat(ytemp);
                if(tool.CalToolPointX.Length==1 && tool.CalToolPointY.Length == 1)
                {
                    MessageBox.Show("第一计算点设置完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (tool.CalToolPointX.Length == 2 && tool.CalToolPointY.Length == 2)
                {
                    MessageBox.Show("第二计算点设置完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch
            {
                mView.ClearList();
                mView.AddIconicVar(testImage);
                mView.Repaint();
                MessageBox.Show("错误！", "无法有效识别标定板", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            mView.ClearList();
            mView.AddIconicVar(testImage);
            if (ho_Caltab != null && ho_Caltab.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "green");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 3);
                mView.AddIconicVar(ho_Caltab);
            }
            if (ho_Cross != null && ho_Cross.IsInitialized())
            {
                mView.ChangeGraphicSettings(Mode.COLOR, "blue");
                mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                mView.AddIconicVar(ho_Cross);
            }
            mView.Repaint();

        }
        private void btn_CalToolCoord_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.angle <= 0.0)
            {
                MessageBox.Show("请设置工具Z轴旋转角", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tool.angle <= 5.0)
            {
                MessageBox.Show("工具Z轴旋转角偏小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(tool.CalToolPointX==null || tool.CalToolPointY == null)
            {
                MessageBox.Show("请设置计算位置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if(tool.CalToolPointX.Length!=2 || tool.CalToolPointY.Length != 2)
            {
                if(tool.CalToolPointX.Length == 1 || tool.CalToolPointY.Length == 1)
                {
                    MessageBox.Show("只有一个计算位置参数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("请重新设置计算位置参数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }
            HTuple d;
            HOperatorSet.DistancePp(tool.CalToolPointX[0], tool.CalToolPointY[0], tool.CalToolPointX[1], tool.CalToolPointY[1], out d);
            double Phi = (tool.angle / 2 )/ 180 * Math.PI;
            double R = (d.D / 2) / Math.Sin(Phi);
            double Xt, Yt;
            double Rd = R / d.D;
            Xt = (1 - Rd) * tool.CalToolPointX[0].D + Rd * tool.CalToolPointX[1].D;
            Yt = (1 - Rd) * tool.CalToolPointY[0].D + Rd * tool.CalToolPointY[1].D;

            double Phi_D = Math.PI / 180 * (90 - (tool.angle / 2));
            //计算在CamTool坐标系下，工具中心的位置。
            tool.X0 = Math.Cos(Phi_D) *(Xt- tool.CalToolPointX[0].D)
                     -Math.Sin(Phi_D) *(Yt- tool.CalToolPointY[0].D)
                     + tool.CalToolPointX[0].D;
            tool.Y0= Math.Cos(Phi_D) * (Yt - tool.CalToolPointY[0].D)
                     - Math.Sin(Phi_D) * (Xt - tool.CalToolPointX[0].D)
                     + tool.CalToolPointY[0].D;

            txt_Cal_Comp_X.Text = tool.X0.ToString("F6");
            txt_Cal_Comp_Y.Text = tool.Y0.ToString("F6");


        }
        private void btn_Calib_ToolAndMP_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.cameraParam == null || tool.cameraParam.Length != 9)
            {
                MessageBox.Show("相机内参错误", "请先标定相机内参", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MPPose == null || tool.MPPose.Length != 7)
            {
                MessageBox.Show("参考平面姿势错误", "请先标定检测参考平面", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MPInCamToolPose == null || tool.MPInCamToolPose.Length != 7)
            {
                MessageBox.Show("请先做九点标定", "参考平面姿势错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tool.Calibration_Tool_MP();
            MessageBox.Show("参考平面与工具标定成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mView.viewPort.MouseDown += GetMousePoint;
            mView.viewPort.MouseMove += MouseTestPoint;

        }
        private void btn_SaveToolPose_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.MPInToolPose == null || tool.MPInToolPose.Length!=7)
            {
                MessageBox.Show("请先做补偿标定", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "补偿标定||工具标定(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.MPInToolPose, sfd.FileName);
                ExecuteInformation("补偿标定||工具标定参数文件保存成功");
            }
        }
        private void btn_RestetCal_Click(object sender, EventArgs e)
        {
            CloseAcq();
            tool.X0 = 0;
            tool.Y0 = 0;
            tool.CalToolPointX = null;
            tool.CalToolPointY = null;
            tool.MPInToolPose = null;
        }

        #endregion

        #region 验证
        private void btn_GrabImageForTest_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show("获取图像失败", "请检查相机或重新获取图像", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mView.ClearList();
            mView.AddIconicVar(testImage);
            mView.Repaint();
        }
        private void btn_SetModelImage_Click_1(object sender, EventArgs e)
        {
            CloseAcq();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show("请先获取图像", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            tool.ModelImage = testImage;

            mView.ClearList();
            mView.SetDispLevel(ShowMode.ExcludeROI);
            mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
            mView.AddIconicVar(tool.ModelImage);
            mView.Repaint();
        }
        private void GetMousePoint(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);

                DialogResult r1 = MessageBox.Show("是否设定该点为Mark点", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (r1 != DialogResult.Yes)
                {
                    return;
                }
                tool.refRow = row;
                tool.refCol = col;
                HTuple x_temp, y_temp;
                HOperatorSet.ImagePointsToWorldPlane(tool.cameraParam, tool.MPPose, row, col, 1, out x_temp, out y_temp);
                HTuple robot_shift_x, robot_shift_y, robot_shift_z;                
                HTuple MPInToolHomMat;
                HOperatorSet.PoseToHomMat3d(tool.MPInToolPose, out MPInToolHomMat);
                HOperatorSet.AffineTransPoint3d(MPInToolHomMat, x_temp, y_temp, new HTuple(0.0), out robot_shift_x, out robot_shift_y, out robot_shift_z);

                txt_MarkInToolCoord_Calculation.Text = robot_shift_x.D.ToString("F6") + ";" +
                                                      robot_shift_y.D.ToString("F6") + ";" +
                                                      robot_shift_z.D.ToString("F6") ;

                tool.MarkInToolCoordStr = txt_MarkInToolCoord_Calculation.Text;
                tool.MarkInToolCoord = robot_shift_x.TupleConcat(robot_shift_y).TupleConcat(robot_shift_z);

                //Mark在相机工具坐标系的位置
                HTuple CamTool_shift_x, CamTool_shift_y, CamTool_shift_z;
                HOperatorSet.AffineTransPoint3d(tool.MPInCamToolHomMat, x_temp, y_temp, new HTuple(0.0), out CamTool_shift_x, out CamTool_shift_y, out CamTool_shift_z);
                tool.MarkInCamToolCoord = CamTool_shift_x.TupleConcat(CamTool_shift_y).TupleConcat(CamTool_shift_z);

                ExecuteInformation("Mark点设定成功");
            }
            catch (HalconException)
            {
                return;
            }


        }
        private void MouseTestPoint(object sender, MouseEventArgs e)
        {
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
                if (modelPointXld == null)
                    modelPointXld = new HXLDCont();
                if (modelPointXld != null && modelPointXld.IsInitialized())
                    modelPointXld.Dispose();
                modelPointXld.GenCrossContourXld(row, col, 30, 0);
                mView.ClearList();
                mView.SetDispLevel(ShowMode.ExcludeROI);
                mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
                mView.AddIconicVar(tool.ModelImage);
                if (modelPointXld != null)
                {
                    mView.ChangeGraphicSettings(Mode.COLOR, "red");
                    mView.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                    mView.AddIconicVar(modelPointXld);
                }
                tool.refRow = row;
                tool.refCol = col;
                HTuple x_temp, y_temp;
                HOperatorSet.ImagePointsToWorldPlane(tool.cameraParam, tool.MPPose, row, col, 1, out x_temp, out y_temp);
                HTuple robot_shift_x, robot_shift_y, robot_shift_z;
                HTuple MPInToolHomMat;
                HOperatorSet.PoseToHomMat3d(tool.MPInToolPose, out MPInToolHomMat);
                HOperatorSet.AffineTransPoint3d(MPInToolHomMat, x_temp, y_temp, new HTuple(0.0), out robot_shift_x, out robot_shift_y, out robot_shift_z);                

                mView.AddText("X:=" + (robot_shift_x.D).ToString("F6") + ";" + "Y:= " + (robot_shift_y.D).ToString("F6"), (int)(row + 20), (int)col, 20, "green");

                mView.Repaint();

            }

            catch (HalconException)
            {
                return;
            }

        }
        private void txt_CurrentToolPose_TextChanged(object sender, EventArgs e)
        {
            string[] splicts = Regex.Split(txt_CurrentToolInBasePose.Text, ";");
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
                tool.CurrentGrabPose = tempPose;
            }
        }
        private void txt_Mark_InToolPose_Menu_TextChanged(object sender, EventArgs e)
        {
            string[] splicts = Regex.Split(txt_MarkInBasePose_Menu.Text, ";");
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
                tool.MarkInBasePoseMenu = tempPose;
            }
        }
        private void btn_Calcu_Menu_Click(object sender, EventArgs e)
        {

            if (tool.CurrentGrabPose == null || tool.CurrentGrabPose.Length != 7)
            {
                MessageBox.Show("请先输入拍照位手臂姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tool.MarkInBasePoseMenu == null || tool.MPInCamToolPose.Length != 7)
            {
                MessageBox.Show("请先输入Mark位置手臂姿势参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(tool.MarkInCamToolCoord==null || tool.CurrentGrabPose.Length != 3)
            {
                MessageBox.Show("请先在左侧图像点击选择Mark点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            //矫正工具坐标Z轴偏移。
            HTuple baseInToolPose;
            HOperatorSet.PoseInvert(tool.CurrentGrabPose, out baseInToolPose);
            HTuple markInToolPose;
            HOperatorSet.PoseCompose(baseInToolPose, tool.MarkInBasePoseMenu, out markInToolPose);
            HTuple ToolInMPPose,ToolInMPPose_aft;
            HOperatorSet.PoseInvert(tool.MPInToolPose,out ToolInMPPose);
            HOperatorSet.SetOriginPose(ToolInMPPose, new HTuple(0.0), new HTuple(0.0), tool.MarkInToolCoord[2] - markInToolPose[2], out ToolInMPPose_aft);
            HOperatorSet.PoseInvert(ToolInMPPose_aft, out tool.MPInToolPose);

            //求在Tool坐标系下，相机光轴点的位置
            tool.X_Comp_T = markInToolPose[0].D - tool.MarkInCamToolCoord[0].D;
            tool.Y_Comp_T = markInToolPose[1].D - tool.MarkInCamToolCoord[1].D;
            txt_Men_Comp_X.Text = tool.X_Comp_T.ToString("F6");
            txt_Men_Comp_Y.Text = tool.Y_Comp_T.ToString("F6");

            StringBuilder strb = new StringBuilder();
            strb.Append("计算与手动补偿误差：" + Environment.NewLine);
            strb.Append("X方向：" + ((tool.X_Comp_T + tool.X0) * 1000).ToString("F2") + "mm" + Environment.NewLine);
            strb.Append("Y方向：" + ((tool.Y_Comp_T + tool.X0) * 1000).ToString("F2") + "mm" + Environment.NewLine);
            ExecuteInformation(strb.ToString());

        }

        #endregion
        #endregion

        private void tabCtrl_Func_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrl_Func.SelectedIndex == 1)
            {
                mView.viewPort.MouseDown -= GetMousePoint;
                mView.viewPort.MouseMove -= MouseTestPoint;
                if (tool.cameraParam == null || tool.cameraParam.Length != 9)
                {
                    gB_worldPoseEnable.Enabled = false;
                    MessageBox.Show("相机参数为空", "请先标定相机参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (tool.MPPose == null && tool.MPPose.Length != 7)
                {
                    gB_worldPoseEnable.Enabled = false;
                    MessageBox.Show("相机参考平面为空", "请先量测对象参考平面参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                gB_worldPoseEnable.Enabled = true;

            }
            else if (tabCtrl_Func.SelectedIndex == 2)
            {
                if (tool.cameraParam == null || tool.cameraParam.Length != 9)
                {
                    MessageBox.Show("请先标定相机参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tool.MPPose == null || tool.MPPose.Length != 7)
                {
                    MessageBox.Show("请先标定世界坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (tool.MPInCamToolPose == null || tool.MPInCamToolPose.Length != 7)
                {
                    MessageBox.Show("请先做九点标定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tool.MPInToolPose == null || tool.MPInToolPose.Length != 7)
                {
                    MessageBox.Show("请先做补偿标定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tool.ModelImage == null || !tool.ModelImage.IsInitialized())
                {
                    MessageBox.Show("请先采集图片，来验证与测试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                mView.viewPort.MouseDown += GetMousePoint;
                mView.viewPort.MouseMove += MouseTestPoint;
                mView.ClearList();
                mView.AddIconicVar(tool.ModelImage);
                mView.Repaint();
            }
            else
            {
                mView.viewPort.MouseDown -= GetMousePoint;
                mView.viewPort.MouseMove -= MouseTestPoint;
            }
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
            txt_log.AppendText(strb.ToString());
            //超过1000行就删除500行
            if (messageCount > 1000)
            {
                string[] lines = txt_log.Lines;
                List<string> a = lines.ToList();
                a.RemoveRange(0, 500);           //删除之前必须转换成链表。
                txt_log.Lines = a.ToArray();
                messageCount = 500;
            }
            txt_log.ScrollToCaret();

        }

        public override void Clear()
        {
            CloseAcq();
            mView.viewPort.MouseDown -= GetMousePoint;
            mView.viewPort.MouseMove -= MouseTestPoint;
            base.Clear();
            if (CalibDataID != null && CalibDataID.Length > 0)
            {
                HOperatorSet.ClearCalibData(CalibDataID);
            }
            if (calHImages != null)
            {
                for (int i = 0; i < calHImages.Count; i++)
                {
                    if (calHImages[i] != null && calHImages[i].IsInitialized())
                    {
                        calHImages[i].Dispose();
                    }
                }
                calHImages.Clear();
                calHImages = null;
            }
            if (ho_Caltab != null && ho_Caltab.IsInitialized())
            {
                ho_Caltab.Dispose();
            }
            if (ho_Cross != null && ho_Cross.IsInitialized())
            {
                ho_Cross.Dispose();
            }
            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            if (modelPointXld != null && modelPointXld.IsInitialized())
            {
                modelPointXld.Dispose();
            }
        }
    }
}
