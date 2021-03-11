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


namespace Yoga.Calibration.EyesOnHand2
{
    public partial class CalibrationSetting : ToolsSettingUnit
    {
        private EyesOnHandTool2 tool;
        public HWndCtrl mView;
        HTuple startCamPar;
        HTuple CalibDataID;
        HObject ho_Caltab, ho_Cross;
        HImage testImage;
        List<HImage> calHImages = new List<HImage>();
        int index;
        Thread acqContinus;
        bool acqIsrun=false;
        public CalibrationSetting(EyesOnHandTool2 calibraion)
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
        }

        private void InitCalibrationCam_Robot()
        {
            this.dGV_worldPose.DataSource = tool.RbtCurrentPoseList;
            this.dGV_worldPose.Refresh();            
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

            if (tool.MPInCamPose != null && tool.MPInCamPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPInCamPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPInCamPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPInCamPose[6].D.ToString());
                txt_planPose.Text = strb.ToString();
            }
            else
            {
                tool.MPInCamPose = null;
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
                HOperatorSet.FindCalibObject(testImage, CalibDataID, 0, 0, index, new HTuple(), new HTuple());
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
            HOperatorSet.SetOriginPose(tempPose, 0, 0, new HTuple(caltab_H_m), out tool.MPInCamPose);

            if (tool.MPInCamPose != null && tool.MPInCamPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    if (i < 3)
                    {
                        strb.Append(tool.MPInCamPose[i].D.ToString("F6"));
                    }
                    else
                    {
                        strb.Append(tool.MPInCamPose[i].D.ToString("F3"));
                    }

                    strb.Append(",");
                }
                strb.Append(tool.MPInCamPose[6].D.ToString());

                txt_planPose.Text = strb.ToString();
                ExecuteInformation("参考平面设定成功");
            }
            else
            {
                tool.MPInCamPose = null;
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
            if (tool.MPInCamPose == null || tool.MPInCamPose.Length != 7)
            {
                MessageBox.Show("请先参考平面坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "参考平面坐标系文件(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.MPInCamPose, sfd.FileName);
                ExecuteInformation("参考平面坐标系(MpInCamPose)文件保存成功");
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
            if (tool.MPInCamPose == null || tool.MPInCamPose.Length != 7)
            {
                MessageBox.Show("请先标定检测参考平面", "参考平面姿势错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show("请检查相机或重新获取图像", "获取图像失败",  MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        HTuple CalibCurrentToolInBasePose;
        private void txt_CurrentToolInBaesPoseCalib_TextChanged(object sender, EventArgs e)
        {
            string[] splicts = Regex.Split(txt_CurrentToolInBaesPoseCalib.Text, ";");
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
                        if(temp!=1 || temp != 2)
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
                CalibCurrentToolInBasePose = tempPose;
            }
            ExecuteInformation("当前机器人姿势已更新");
        }

        HTuple CalID_MovingCam;
        int movingCam_Index;
        private void btn_AddCalibratData_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.cameraParam == null || tool.cameraParam.Length != 9|| tool.descrPath==string.Empty)
            {                
                MessageBox.Show("相机参数为空", "请先标定相机参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (CalID_MovingCam == null || CalID_MovingCam.Length!=1)
            {                
                HOperatorSet.CreateCalibData("hand_eye_moving_cam", 1, 1, out CalID_MovingCam);
                HOperatorSet.SetCalibDataCamParam(CalID_MovingCam, 0, new HTuple(), tool.cameraParam);
                HOperatorSet.SetCalibDataCalibObject(CalID_MovingCam, 0, new HTuple(tool.descrPath));
                HOperatorSet.SetCalibData(CalID_MovingCam, "model", "general", "optimization_method", "nonlinear");
                movingCam_Index = 0;
            }
            if(CalibCurrentToolInBasePose==null || CalibCurrentToolInBasePose.Length != 7)
            {
                MessageBox.Show("请输入当前机器人姿势", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //判断当前姿势是否在表格里面已存在。
            if (tool.RbtCurrentPoseList.Count > 0)
            {
                for(int i=0;i< tool.RbtCurrentPoseList.Count; i++)
                {
                    if( Math.Abs(CalibCurrentToolInBasePose[0].D - tool.RbtCurrentPoseList[i].X) < 0.0001 &&
                        Math.Abs(CalibCurrentToolInBasePose[1].D - tool.RbtCurrentPoseList[i].Y) < 0.0001 &&
                        Math.Abs(CalibCurrentToolInBasePose[2].D - tool.RbtCurrentPoseList[i].Z) < 0.0001 &&
                        Math.Abs(CalibCurrentToolInBasePose[3].D - tool.RbtCurrentPoseList[i].RX) < 0.0001 &&
                        Math.Abs(CalibCurrentToolInBasePose[4].D - tool.RbtCurrentPoseList[i].RY) < 0.0001 &&
                        Math.Abs(CalibCurrentToolInBasePose[5].D - tool.RbtCurrentPoseList[i].RZ) < 0.0001
                        )
                    {
                        MessageBox.Show("当前机器人姿势与此前标定姿势相同或太相近", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
           

            if (testImage!=null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null || !testImage.IsInitialized())
            {
                MessageBox.Show("请检查相机或重新获取图像", "获取图像失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                HOperatorSet.FindCalibObject(testImage, CalID_MovingCam, 0, 0, movingCam_Index, new HTuple(), new HTuple());
                HOperatorSet.SetCalibData(CalID_MovingCam, "tool", movingCam_Index, "tool_in_base_pose", CalibCurrentToolInBasePose);

            }
            catch (Exception ex)
            {
                MessageBox.Show("未找到对应圆心"+ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            RobotPose currentPose = new RobotPose();
            currentPose.X = CalibCurrentToolInBasePose[0].D;
            currentPose.Y = CalibCurrentToolInBasePose[1].D;
            currentPose.Z = CalibCurrentToolInBasePose[2].D;
            currentPose.RX = CalibCurrentToolInBasePose[3].D;
            currentPose.RY = CalibCurrentToolInBasePose[4].D;
            currentPose.RZ = CalibCurrentToolInBasePose[5].D;
            tool.RbtCurrentPoseList.Add(currentPose);
            movingCam_Index++;
            CalibCurrentToolInBasePose = null;
            txt_CurrentToolInBaesPoseCalib.Text = "";
            dGV_worldPose.Refresh();
            ExecuteInformation("当前数据已加入标定数据队列");
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            CloseAcq();
            tool.RbtCurrentPoseList.Clear();
            tool.RbtCurrentPoseList = null;
            dGV_worldPose.Refresh();
            if(CalID_MovingCam!=null && CalID_MovingCam.Length == 1)
            {
                try
                {
                    HOperatorSet.ClearCalibData(CalID_MovingCam);
                }
                catch
                {

                }
            }
            movingCam_Index = 0;
            CalibCurrentToolInBasePose = null;
            txt_CurrentToolInBaesPoseCalib.Text = "";
            ExecuteInformation("已重置");
        }

        private void btn_CalibrationCoord_Click(object sender, EventArgs e)
        {
            CloseAcq();
            HTuple Warnings;
            check_hand_eye_calibration_input_poses(CalID_MovingCam, 0.05, 0.005,out Warnings);
            if(Warnings==null || Warnings.Length == 0)
            {
                ExecuteInformation("标定成功");
            }
            else
            {
                MessageBox.Show("标定失败:"+ Warnings.S, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //calibrate_hand_eye(CalibDataID, Errors)
            HTuple error;
            HOperatorSet.CalibrateHandEye(CalID_MovingCam, out error);
            ExecuteInformation("平移误差RMS:"+ (error[0].D*1000).ToString("F3")+"mm;"+Environment.NewLine+
                               "平移误差Max:" + (error[2].D * 1000).ToString("F3") + "mm;" + Environment.NewLine +
                               "旋转误差RMS:" + (error[1].D * 1000).ToString("F3") + "度;" + Environment.NewLine +
                               "旋转误差Max:" + (error[2].D * 1000).ToString("F3") + "度;" + Environment.NewLine
                               );

            //get_calib_data (CalibDataID, 'camera', 0, 'tool_in_cam_pose', ToolInCamPose)
            HOperatorSet.GetCalibData(CalID_MovingCam, "camera", 0, "tool_in_cam_pose", out tool.ToolInCamPose);
            HOperatorSet.ClearCalibData(CalID_MovingCam);

        }

        //保存手眼相对标定
        private void btn_SaveCamRobotCal_Click(object sender, EventArgs e)
        {
            CloseAcq();
            if (tool.MPInCamPose == null || tool.MPInCamPose.Length != 7)
            {
                MessageBox.Show("请先标定相机内参并设定参考平面", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (tool.ToolInCamPose == null || tool.ToolInCamPose.Length!=7)
            {
                MessageBox.Show("请先标定相机与机器人", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            HTuple CamInToolPose;
            HOperatorSet.PoseInvert(tool.ToolInCamPose,out CamInToolPose);
            HOperatorSet.PoseCompose(CamInToolPose, tool.MPInCamPose, out tool.MPInToolPose);//type 0


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "MpInToolPose(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.MPInToolPose, sfd.FileName);
                ExecuteInformation("MpInToolPose参数文件保存成功");
            }
        }


        //检查标定是否有问题
        public void check_hand_eye_calibration_input_poses(HTuple hv_CalibDataID, HTuple hv_RotationTolerance,
    HTuple hv_TranslationTolerance, out HTuple hv_Warnings)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_MinLargeRotationFraction = null;
            HTuple hv_MinLargeAnglesFraction = null, hv_StdDevFactor = null;
            HTuple hv_Type = new HTuple(), hv_Exception = null, hv_IsHandEyeScara = null;
            HTuple hv_IsHandEyeArticulated = null, hv_NumCameras = null;
            HTuple hv_NumCalibObjs = null, hv_I1 = null, hv_PosesIdx = null;
            HTuple hv_RefCalibDataID = null, hv_UseTemporaryCopy = null;
            HTuple hv_CamPoseCal = new HTuple(), hv_SerializedItemHandle = new HTuple();
            HTuple hv_TmpCalibDataID = new HTuple(), hv_Error = new HTuple();
            HTuple hv_Index = null, hv_CamDualQuatCal = new HTuple();
            HTuple hv_BasePoseTool = new HTuple(), hv_BaseDualQuatTool = new HTuple();
            HTuple hv_NumCalibrationPoses = null, hv_LX2s = null, hv_LY2s = null;
            HTuple hv_LZ2s = null, hv_TranslationToleranceSquared = null;
            HTuple hv_RotationToleranceSquared = null, hv_Index1 = null;
            HTuple hv_CamDualQuatCal1 = new HTuple(), hv_Cal1DualQuatCam = new HTuple();
            HTuple hv_BaseDualQuatTool1 = new HTuple(), hv_Tool1DualQuatBase = new HTuple();
            HTuple hv_Index2 = new HTuple(), hv_CamDualQuatCal2 = new HTuple();
            HTuple hv_DualQuat1 = new HTuple(), hv_BaseDualQuatTool2 = new HTuple();
            HTuple hv_DualQuat2 = new HTuple(), hv_LX1 = new HTuple();
            HTuple hv_LY1 = new HTuple(), hv_LZ1 = new HTuple(), hv_MX1 = new HTuple();
            HTuple hv_MY1 = new HTuple(), hv_MZ1 = new HTuple(), hv_Rot1 = new HTuple();
            HTuple hv_Trans1 = new HTuple(), hv_LX2 = new HTuple();
            HTuple hv_LY2 = new HTuple(), hv_LZ2 = new HTuple(), hv_MX2 = new HTuple();
            HTuple hv_MY2 = new HTuple(), hv_MZ2 = new HTuple(), hv_Rot2 = new HTuple();
            HTuple hv_Trans2 = new HTuple(), hv_MeanRot = new HTuple();
            HTuple hv_MeanTrans = new HTuple(), hv_SinTheta2 = new HTuple();
            HTuple hv_CosTheta2 = new HTuple(), hv_SinTheta2Squared = new HTuple();
            HTuple hv_CosTheta2Squared = new HTuple(), hv_ErrorRot = new HTuple();
            HTuple hv_StdDevQ0 = new HTuple(), hv_ToleranceDualQuat0 = new HTuple();
            HTuple hv_ErrorDualQuat0 = new HTuple(), hv_StdDevQ4 = new HTuple();
            HTuple hv_ToleranceDualQuat4 = new HTuple(), hv_ErrorDualQuat4 = new HTuple();
            HTuple hv_Message = new HTuple(), hv_NumPairs = null, hv_NumPairsMax = null;
            HTuple hv_LargeRotationFraction = null, hv_NumPairPairs = null;
            HTuple hv_NumPairPairsMax = null, hv_Angles = null, hv_Idx = null;
            HTuple hv_LXA = new HTuple(), hv_LYA = new HTuple(), hv_LZA = new HTuple();
            HTuple hv_LXB = new HTuple(), hv_LYB = new HTuple(), hv_LZB = new HTuple();
            HTuple hv_ScalarProduct = new HTuple(), hv_LargeAngles = null;
            HTuple hv_LargeAnglesFraction = null;

            HTupleVector hvec_CamDualQuatsCal = new HTupleVector(1);
            HTupleVector hvec_BaseDualQuatsTool = new HTupleVector(1);
            // Initialize local and output iconic variables 
            //This procedure checks the hand-eye calibration input poses that are stored in
            //the calibration data model CalibDataID for consistency.
            //
            //For this check, it is necessary to know the accuracy of the input poses.
            //Therefore, the RotationTolerance and TranslationTolerance must be
            //specified that approximately describe the error in the rotation and in the
            //translation part of the input poses, respectively. The rotation tolerance must
            //be passed in RotationTolerance in radians. The translation tolerance must be
            //passed in TranslationTolerance in the same unit in which the input poses were
            //given, i.e., typically in meters. Therefore, the more accurate the
            //input poses are, the lower the values for RotationTolerance and
            //TranslationTolerance should be chosen. If the accuracy of the robot's tool
            //poses is different from the accuracy of the calibration object poses, the
            //tolerance values of the poses with the lower accuracy (i.e., the higher
            //tolerance values) should be passed.
            //
            //Typically, check_hand_eye_calibration_input_poses is called after all
            //calibration poses have been set in the calibration data model and before the
            //hand eye calibration is performed. The procedure checks all pairs of robot
            //tool poses and compares them to the corresponding pair of calibration object
            //poses. For each inconsistent pose pair, a string is returned in Warnings that
            //indicates the inconsistent pose pair. For larger values for RotationTolerance
            //or TranslationTolerance, i.e., for less accurate input poses, fewer warnings
            //will be generated because the check is more tolerant, and vice versa. The
            //procedure is also helpful if the errors that are returned by the hand-eye
            //calibration are larger than expected to identify potentially erroneous poses.
            //Note that it is not possible to check the consistency of a single pose but
            //only of pose pairs. Nevertheless, if a certain pose occurs multiple times in
            //different warning messages, it is likely that the pose is erroneous.
            //Erroneous poses that result in inconsistent pose pairs should removed
            //from the calibration data model by using remove_calib_data_observ and
            //remove_calib_data before performing the hand-eye calibration.
            //
            //check_hand_eye_calibration_input_poses also checks whether enough calibration
            //pose pairs are passed with a significant relative rotation angle, which
            //is necessary for a robust hand-eye calibration.
            //
            //check_hand_eye_calibration_input_poses also verifies that the correct
            //calibration model was chosen in create_calib_data. If a model of type
            //'hand_eye_stationary_cam' or 'hand_eye_moving_cam' was chosen, the calibration
            //of an articulated robot is assumed. For 'hand_eye_scara_stationary_cam' or
            //'hand_eye_scara_moving_cam', the calibration of a SCARA robot is assumed.
            //Therefore, if all input poses for an articulated robot are parallel or if some
            //robot poses for a SCARA robot are tilted, a corresponding message is returned
            //in Warnings. Furthermore, if the number of tilted input poses for articulated
            //robots is below a certain value, a corresponding message in Warnings indicates
            //that the accuracy of the result of the hand-eye calibration might be low.
            //
            //If no problems have been detected in the input poses, an empty tuple is
            //returned in Warnings.
            //
            //
            //Define the minimum fraction of pose pairs with a rotation angle exceeding
            //2*RotationTolerance.
            hv_MinLargeRotationFraction = 0.1;
            //Define the minimum fraction of screw axes pairs with an angle exceeding
            //2*RotationTolerance for articulated robots.
            hv_MinLargeAnglesFraction = 0.1;
            //Factor that is used to multiply the standard deviations to obtain an error
            //threshold.
            hv_StdDevFactor = 3.0;
            //
            //Check input control parameters.
            if ((int)(new HTuple((new HTuple(hv_CalibDataID.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 1");
            }
            if ((int)(new HTuple((new HTuple(hv_RotationTolerance.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 2");
            }
            if ((int)(new HTuple((new HTuple(hv_TranslationTolerance.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 3");
            }
            // dev_get_preferences(...); only in hdevelop
            // dev_set_preferences(...); only in hdevelop
            try
            {
                HOperatorSet.GetCalibData(hv_CalibDataID, "model", "general", "type", out hv_Type);
            }
            // catch (Exception) 
            catch (HalconException HDevExpDefaultException1)
            {
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                throw new HalconException("Wrong value of control parameter: 1");
            }
            // dev_set_preferences(...); only in hdevelop
            if ((int)(new HTuple(hv_RotationTolerance.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            if ((int)(new HTuple(hv_TranslationTolerance.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 3");
            }
            //
            //Read out the calibration data model.
            hv_IsHandEyeScara = (new HTuple(hv_Type.TupleEqual("hand_eye_scara_stationary_cam"))).TupleOr(
                new HTuple(hv_Type.TupleEqual("hand_eye_scara_moving_cam")));
            hv_IsHandEyeArticulated = (new HTuple(hv_Type.TupleEqual("hand_eye_stationary_cam"))).TupleOr(
                new HTuple(hv_Type.TupleEqual("hand_eye_moving_cam")));
            //This procedure only works for hand-eye calibration applications.
            if ((int)((new HTuple(hv_IsHandEyeScara.TupleNot())).TupleAnd(hv_IsHandEyeArticulated.TupleNot()
                )) != 0)
            {
                throw new HalconException("check_hand_eye_calibration_input_poses only works for hand-eye calibrations");
            }
            HOperatorSet.GetCalibData(hv_CalibDataID, "model", "general", "num_cameras",
                out hv_NumCameras);
            HOperatorSet.GetCalibData(hv_CalibDataID, "model", "general", "num_calib_objs",
                out hv_NumCalibObjs);
            //
            //Get all valid calibration pose indices.
            HOperatorSet.QueryCalibDataObservIndices(hv_CalibDataID, "camera", 0, out hv_I1,
                out hv_PosesIdx);
            hv_RefCalibDataID = hv_CalibDataID.Clone();
            hv_UseTemporaryCopy = 0;
            //If necessary, calibrate the interior camera parameters.
            if ((int)(hv_IsHandEyeArticulated) != 0)
            {
                //For articulated (non-SCARA) robots, we have to check whether the camera
                //is already calibrated. Otherwise, the queried poses might not be very
                //accurate.
                // dev_get_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                try
                {
                    HOperatorSet.GetCalibData(hv_CalibDataID, "calib_obj_pose", (new HTuple(0)).TupleConcat(
                        hv_PosesIdx.TupleSelect(0)), "pose", out hv_CamPoseCal);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    if ((int)((new HTuple(hv_NumCameras.TupleNotEqual(0))).TupleAnd(new HTuple(hv_NumCalibObjs.TupleNotEqual(
                        0)))) != 0)
                    {
                        //If the interior camera parameters are not calibrated yet, perform
                        //the camera calibration by using a temporary copy of the calibration
                        //data model.
                        HOperatorSet.SerializeCalibData(hv_CalibDataID, out hv_SerializedItemHandle);
                        HOperatorSet.DeserializeCalibData(hv_SerializedItemHandle, out hv_TmpCalibDataID);
                        HOperatorSet.ClearSerializedItem(hv_SerializedItemHandle);
                        hv_RefCalibDataID = hv_TmpCalibDataID.Clone();
                        hv_UseTemporaryCopy = 1;
                        HOperatorSet.CalibrateCameras(hv_TmpCalibDataID, out hv_Error);
                    }
                }
                // dev_set_preferences(...); only in hdevelop
            }
            //Query all robot tool and calibration object poses.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_PosesIdx.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                // dev_get_preferences(...); only in hdevelop
                // dev_set_preferences(...); only in hdevelop
                try
                {
                    //For an articulated robot with a camera and a calibration object,
                    //a calibrated poses should always be available.
                    HOperatorSet.GetCalibData(hv_RefCalibDataID, "calib_obj_pose", (new HTuple(0)).TupleConcat(
                        hv_PosesIdx.TupleSelect(hv_Index)), "pose", out hv_CamPoseCal);
                }
                // catch (Exception) 
                catch (HalconException HDevExpDefaultException1)
                {
                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
                    //For a SCARA robot or for an articulated robots with a general
                    //sensor and no calibration object, directly use the observed poses.
                    HOperatorSet.GetCalibDataObservPose(hv_RefCalibDataID, 0, 0, hv_PosesIdx.TupleSelect(
                        hv_Index), out hv_CamPoseCal);
                }
                // dev_set_preferences(...); only in hdevelop
                //Transform the calibration object poses to dual quaternions.
                HOperatorSet.PoseToDualQuat(hv_CamPoseCal, out hv_CamDualQuatCal);
                hvec_CamDualQuatsCal[hv_Index] = new HTupleVector(hv_CamDualQuatCal);
                //Transform the robot tool pose to dual quaternions.
                HOperatorSet.GetCalibData(hv_RefCalibDataID, "tool", hv_PosesIdx.TupleSelect(
                    hv_Index), "tool_in_base_pose", out hv_BasePoseTool);
                HOperatorSet.PoseToDualQuat(hv_BasePoseTool, out hv_BaseDualQuatTool);
                hvec_BaseDualQuatsTool[hv_Index] = new HTupleVector(hv_BaseDualQuatTool);
            }
            hv_NumCalibrationPoses = new HTuple(hv_PosesIdx.TupleLength());
            if ((int)(hv_UseTemporaryCopy) != 0)
            {
                HOperatorSet.ClearCalibData(hv_TmpCalibDataID);
            }
            //
            //In the first test, check the poses for consistency. The principle of
            //the hand-eye calibration is that the movement of the robot from time
            //i to time j is represented by the relative pose of the calibration
            //object from i to j in the camera coordinate system and also by the
            //relative pose of the robot tool from i to j in the robot base
            //coordinate system. Because both relative poses represent the same 3D
            //rigid transformation, but only seen from two different coordinate
            //systems, their screw axes differ but their screw angle and their
            //screw translation should be identical. This knowledge can be used to
            //check the consistency of the input poses. Furthermore, remember the
            //screw axes for all robot movements to later check whether the
            //correct calibration model (SCARA or articulated) was selected by the
            //user.
            hv_Warnings = new HTuple();
            hv_LX2s = new HTuple();
            hv_LY2s = new HTuple();
            hv_LZ2s = new HTuple();
            hv_TranslationToleranceSquared = hv_TranslationTolerance * hv_TranslationTolerance;
            hv_RotationToleranceSquared = hv_RotationTolerance * hv_RotationTolerance;
            HTuple end_val171 = hv_NumCalibrationPoses - 2;
            HTuple step_val171 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val171, step_val171); hv_Index1 = hv_Index1.TupleAdd(step_val171))
            {
                hv_CamDualQuatCal1 = hvec_CamDualQuatsCal[hv_Index1].T.Clone();
                HOperatorSet.DualQuatConjugate(hv_CamDualQuatCal1, out hv_Cal1DualQuatCam);
                hv_BaseDualQuatTool1 = hvec_BaseDualQuatsTool[hv_Index1].T.Clone();
                HOperatorSet.DualQuatConjugate(hv_BaseDualQuatTool1, out hv_Tool1DualQuatBase);
                HTuple end_val176 = hv_NumCalibrationPoses - 1;
                HTuple step_val176 = 1;
                for (hv_Index2 = hv_Index1 + 1; hv_Index2.Continue(end_val176, step_val176); hv_Index2 = hv_Index2.TupleAdd(step_val176))
                {
                    //For two robot poses, ...
                    //... compute the movement of the calibration object in the
                    //camera coordinate system.
                    hv_CamDualQuatCal2 = hvec_CamDualQuatsCal[hv_Index2].T.Clone();
                    HOperatorSet.DualQuatCompose(hv_Cal1DualQuatCam, hv_CamDualQuatCal2, out hv_DualQuat1);
                    //
                    //... compute the movement of the tool in the robot base
                    //coordinate system.
                    hv_BaseDualQuatTool2 = hvec_BaseDualQuatsTool[hv_Index2].T.Clone();
                    HOperatorSet.DualQuatCompose(hv_Tool1DualQuatBase, hv_BaseDualQuatTool2,
                        out hv_DualQuat2);
                    //
                    //Check whether the two movements are consistent. If the two
                    //movements are consistent, the scalar parts of the corresponding
                    //dual quaternions should be equal. For the equality check, we
                    //have to take the accuracy of the input poses into account, which
                    //are given by RotationTolerance and TranslationTolerance.
                    HOperatorSet.DualQuatToScrew(hv_DualQuat1, "moment", out hv_LX1, out hv_LY1,
                        out hv_LZ1, out hv_MX1, out hv_MY1, out hv_MZ1, out hv_Rot1, out hv_Trans1);
                    HOperatorSet.DualQuatToScrew(hv_DualQuat2, "moment", out hv_LX2, out hv_LY2,
                        out hv_LZ2, out hv_MX2, out hv_MY2, out hv_MZ2, out hv_Rot2, out hv_Trans2);
                    while ((int)(new HTuple(hv_Rot1.TupleGreater((new HTuple(180.0)).TupleRad()
                        ))) != 0)
                    {
                        hv_Rot1 = hv_Rot1 - ((new HTuple(360.0)).TupleRad());
                    }
                    while ((int)(new HTuple(hv_Rot2.TupleGreater((new HTuple(180.0)).TupleRad()
                        ))) != 0)
                    {
                        hv_Rot2 = hv_Rot2 - ((new HTuple(360.0)).TupleRad());
                    }
                    //
                    hv_Rot1 = hv_Rot1.TupleFabs();
                    hv_Trans1 = hv_Trans1.TupleFabs();
                    hv_Rot2 = hv_Rot2.TupleFabs();
                    hv_Trans2 = hv_Trans2.TupleFabs();
                    hv_MeanRot = 0.5 * (hv_Rot1 + hv_Rot2);
                    hv_MeanTrans = 0.5 * (hv_Trans1 + hv_Trans2);
                    hv_SinTheta2 = ((0.5 * hv_MeanRot)).TupleSin();
                    hv_CosTheta2 = ((0.5 * hv_MeanRot)).TupleCos();
                    hv_SinTheta2Squared = hv_SinTheta2 * hv_SinTheta2;
                    hv_CosTheta2Squared = hv_CosTheta2 * hv_CosTheta2;
                    //
                    //1. Check the scalar part of the real part of the dual quaternion,
                    //which encodes the rotation component of the screw:
                    //  q[0] = cos(theta/2)
                    //Here, theta is the screw rotation angle.
                    hv_ErrorRot = ((hv_Rot1 - hv_Rot2)).TupleFabs();
                    while ((int)(new HTuple(hv_ErrorRot.TupleGreater((new HTuple(180.0)).TupleRad()
                        ))) != 0)
                    {
                        hv_ErrorRot = hv_ErrorRot - ((new HTuple(360.0)).TupleRad());
                    }
                    hv_ErrorRot = hv_ErrorRot.TupleFabs();
                    //Compute the standard deviation of the scalar part of the real part
                    //by applying the law of error propagation.
                    hv_StdDevQ0 = (0.5 * hv_SinTheta2) * hv_RotationTolerance;
                    //Multiply the standard deviation by a factor to increase the certainty.
                    hv_ToleranceDualQuat0 = hv_StdDevFactor * hv_StdDevQ0;
                    hv_ErrorDualQuat0 = (((((hv_DualQuat2.TupleSelect(0))).TupleFabs()) - (((hv_DualQuat1.TupleSelect(
                        0))).TupleFabs()))).TupleFabs();
                    //
                    //2. Check the scalar part of the dual part of the dual quaternion,
                    //which encodes translation and rotation components of the screw:
                    //  q[4] = -d/2*sin(theta/2)
                    //Here, d is the screw translation.
                    //
                    //Compute the standard deviation of the scalar part of the dual part
                    //by applying the law of error propagation.
                    hv_StdDevQ4 = ((((0.25 * hv_SinTheta2Squared) * hv_TranslationToleranceSquared) + ((((0.0625 * hv_MeanTrans) * hv_MeanTrans) * hv_CosTheta2Squared) * hv_RotationToleranceSquared))).TupleSqrt()
                        ;
                    //Multiply the standard deviation by a factor to increase the certainty.
                    hv_ToleranceDualQuat4 = hv_StdDevFactor * hv_StdDevQ4;
                    hv_ErrorDualQuat4 = (((((hv_DualQuat2.TupleSelect(4))).TupleFabs()) - (((hv_DualQuat1.TupleSelect(
                        4))).TupleFabs()))).TupleFabs();
                    //If one of the two errors exceeds the computed thresholds, return
                    //a warning for the current pose pair.
                    if ((int)((new HTuple(hv_ErrorDualQuat0.TupleGreater(hv_ToleranceDualQuat0))).TupleOr(
                        new HTuple(hv_ErrorDualQuat4.TupleGreater(hv_ToleranceDualQuat4)))) != 0)
                    {
                        hv_Message = ((("Inconsistent pose pair (" + (((hv_PosesIdx.TupleSelect(hv_Index1))).TupleString(
                            "2d"))) + new HTuple(",")) + (((hv_PosesIdx.TupleSelect(hv_Index2))).TupleString(
                            "2d"))) + ")";
                        hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
                    }
                    //
                    //Remember the screw axes (of the robot tool movements) for screws
                    //with a significant rotation part. For movements without rotation
                    //the direction of the screw axis is determined by the translation
                    //part only. Hence, the direction of the screw axis cannot be used
                    //to decide whether an articulated or a SCARA robot is used.
                    if ((int)(new HTuple(hv_Rot2.TupleGreater(hv_StdDevFactor * hv_RotationTolerance))) != 0)
                    {
                        hv_LX2s = hv_LX2s.TupleConcat(hv_LX2);
                        hv_LY2s = hv_LY2s.TupleConcat(hv_LY2);
                        hv_LZ2s = hv_LZ2s.TupleConcat(hv_LZ2);
                    }
                }
            }
            //
            //In the second test, we check whether enough calibration poses with a
            //significant rotation part are available for calibration.
            hv_NumPairs = new HTuple(hv_LX2s.TupleLength());
            hv_NumPairsMax = (hv_NumCalibrationPoses * (hv_NumCalibrationPoses - 1)) / 2;
            if ((int)(new HTuple(hv_NumPairs.TupleLess(2))) != 0)
            {
                hv_Message = "There are not enough rotated calibration poses available.";
                hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
                //In this case, we can skip further test.

                return;
            }
            hv_LargeRotationFraction = (hv_NumPairs.TupleReal()) / hv_NumPairsMax;
            if ((int)((new HTuple(hv_NumPairs.TupleLess(4))).TupleOr(new HTuple(hv_LargeRotationFraction.TupleLess(
                hv_MinLargeRotationFraction)))) != 0)
            {
                hv_Message = new HTuple("Only few rotated robot poses available, which might result in a reduced accuracy of the calibration results.");
                hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
            }
            //
            //In the third test, we compute the angle between the screw axes with
            //a significant rotation part. For SCARA robots, this angle must be 0 in
            //all cases. For articulated robots, for a significant fraction of robot
            //poses, this angle should exceed a certain threshold. For this test, we
            //use the robot tool poses as they are assumed to be more accurate than the
            //calibration object poses.
            hv_NumPairPairs = (hv_NumPairs * (hv_NumPairs - 1)) / 2;
            hv_NumPairPairsMax = (hv_NumPairsMax * (hv_NumPairsMax - 1)) / 2;
            hv_Angles = HTuple.TupleGenConst(hv_NumPairPairs, 0);
            hv_Idx = 0;
            HTuple end_val286 = hv_NumPairs - 2;
            HTuple step_val286 = 1;
            for (hv_Index1 = 0; hv_Index1.Continue(end_val286, step_val286); hv_Index1 = hv_Index1.TupleAdd(step_val286))
            {
                hv_LXA = hv_LX2s.TupleSelect(hv_Index1);
                hv_LYA = hv_LY2s.TupleSelect(hv_Index1);
                hv_LZA = hv_LZ2s.TupleSelect(hv_Index1);
                HTuple end_val290 = hv_NumPairs - 1;
                HTuple step_val290 = 1;
                for (hv_Index2 = hv_Index1 + 1; hv_Index2.Continue(end_val290, step_val290); hv_Index2 = hv_Index2.TupleAdd(step_val290))
                {
                    hv_LXB = hv_LX2s.TupleSelect(hv_Index2);
                    hv_LYB = hv_LY2s.TupleSelect(hv_Index2);
                    hv_LZB = hv_LZ2s.TupleSelect(hv_Index2);
                    //Compute the scalar product, i.e. the cosine of the screw
                    //axes. To obtain valid values, crop the cosine to the
                    //interval [-1,1].
                    hv_ScalarProduct = ((((((((((hv_LXA * hv_LXB) + (hv_LYA * hv_LYB)) + (hv_LZA * hv_LZB))).TupleConcat(
                        1))).TupleMin())).TupleConcat(-1))).TupleMax();
                    //Compute the angle between the axes in the range [0,pi/2].
                    if (hv_Angles == null)
                        hv_Angles = new HTuple();
                    hv_Angles[hv_Idx] = ((hv_ScalarProduct.TupleFabs())).TupleAcos();
                    hv_Idx = hv_Idx + 1;
                }
            }
            //Large angles should significantly exceed the RotationTolerance.
            hv_LargeAngles = ((hv_Angles.TupleGreaterElem(hv_StdDevFactor * hv_RotationTolerance))).TupleSum()
                ;
            //Calculate the fraction of pairs of movements, i.e., pairs of pose
            //pairs, that have a large angle between their corresponding screw
            //axes.
            hv_LargeAnglesFraction = (hv_LargeAngles.TupleReal()) / hv_NumPairPairsMax;
            //For SCARA robots, all screw axes should be parallel, i.e., no
            //two screw axes should have a large angle.
            if ((int)(hv_IsHandEyeScara.TupleAnd(new HTuple(hv_LargeAngles.TupleGreater(0)))) != 0)
            {
                hv_Message = new HTuple("The robot poses indicate that this might be an articulated robot, although a SCARA robot was selected in the calibration data model.");
                hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
            }
            //For articulated robots, the screw axes should have a large
            //angles.
            if ((int)(hv_IsHandEyeArticulated) != 0)
            {
                if ((int)(new HTuple(hv_LargeAngles.TupleEqual(0))) != 0)
                {
                    //If there is no pair of movements with a large angle between
                    //their corresponding screw axes, this might be a SCARA robot.
                    hv_Message = new HTuple("The robot poses indicate that this might be a SCARA robot (no tilted robot poses available), although an articulated robot was selected in the calibration data model.");
                    hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
                }
                else if ((int)(new HTuple(hv_LargeAngles.TupleLess(3))) != 0)
                {
                    //If there are at most 2 movements with a large angle between
                    //their corresponding screw axes, the calibration might be
                    //unstable.
                    hv_Message = "Not enough tilted robot poses available for an accurate calibration of an articulated robot.";
                    hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
                }
                else if ((int)(new HTuple(hv_LargeAnglesFraction.TupleLess(hv_MinLargeAnglesFraction))) != 0)
                {
                    //If there is only a low fraction of pairs of movements with
                    //a large angle between their corresponding screw axes, the
                    //accuracy of the calibration might be low.
                    hv_Message = new HTuple("Only few tilted robot poses available, which might result in a reduced accuracy of the calibration results.");
                    hv_Warnings = hv_Warnings.TupleConcat(hv_Message);
                }
            }

            return;
        }

        #endregion

        private void tabCtrl_Func_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrl_Func.SelectedIndex == 1)
            {
                if (tool.cameraParam == null || tool.cameraParam.Length != 9)
                {
                    gB_worldPoseEnable.Enabled = false;
                    MessageBox.Show("相机参数为空", "请先标定相机参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (tool.MPInCamPose == null && tool.MPInCamPose.Length != 7)
                {
                    gB_worldPoseEnable.Enabled = false;
                    MessageBox.Show("相机参考平面为空", "请先量测对象参考平面参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                gB_worldPoseEnable.Enabled = true;
                InitCalibrationCam_Robot();

            }
            else
            {
                InitCameraParam();
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

        }
    }
}
