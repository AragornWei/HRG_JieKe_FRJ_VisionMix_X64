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

namespace Yoga.Calibration
{
    public partial class CalibrationSetting : ToolsSettingUnit
    {
        private Calibraion tool;
        public HWndCtrl mView;
        HTuple startCamPar;
        HTuple CalibDataID;
        public CalibrationSetting(Calibraion calibraion)
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

        private void Init()
        {
            gb_SettingPlanPose.Enabled = false;
            InitCameraParam();
            InitWorldPose();
        }

        private void InitWorldPose()
        {
            this.dGV_worldPose.DataSource = tool.Points;
            this.dGV_worldPose.Refresh();
            if(tool.worldPose!=null && tool.worldPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for(int i = 0; i < 6; i++)
                {
                    strb.Append(tool.worldPose[i].D.ToString("F3"));
                    strb.Append(",");
                }
                strb.Append(tool.worldPose[6].D.ToString());
                txt_WorldPose.Text = strb.ToString();
                txt_planPose.Text= strb.ToString();
            }
            else
            {
                tool.worldPose = null;
            }
        }

        private void InitCameraParam()
        {
            if (tool.cameraParam != null && tool.cameraParam.Length == 9)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(tool.cameraParam[0].S+",");
                for (int i = 1; i < tool.cameraParam.Length; i++)
                {
                    if (i == 3 || i==4)
                    {
                        strb.Append((tool.cameraParam[i].D*1000000).ToString("F3"));
                    }
                    else if (i == 7 || i==8)
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

                    
                    if (i < tool.cameraParam.Length-1)
                        strb.Append(",");
                }
                txt_CameraParam.Text = strb.ToString();
            }
            else
            {
                tool.cameraParam = null;
            }
            NUpDown_Focus.Value = (decimal)tool.Focus;
            NUpDown_Sx.Value = (decimal)tool.Sx;
            NUpDown_Sy.Value = (decimal)tool.Sy;
            nUpDown_ImageWidth.Value = (decimal)tool.ImageWidth;
            nUpDown_ImageHeight.Value = (decimal)tool.ImageHeight;
            gb_StartCalibration.Enabled = ckb_StartCalibration.Checked;
            txt_descpPath.Text = tool.descrPath;

            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx/ 1000000, (double)tool.Sy/ 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);

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

        #region 相机参数设定
        #region 参数设定
        private void NUpDown_Focus_ValueChanged(object sender, EventArgs e)
        {
            tool.Focus = (int)NUpDown_Focus.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void NUpDown_Sx_ValueChanged(object sender, EventArgs e)
        {
            tool.Sx = (double)NUpDown_Sx.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void NUpDown_Sy_ValueChanged(object sender, EventArgs e)
        {
            tool.Sy = (double)NUpDown_Sy.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void nUpDown_ImageWidth_ValueChanged(object sender, EventArgs e)
        {
            tool.ImageWidth = (int)nUpDown_ImageWidth.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        private void nUpDown_ImageHeight_ValueChanged(object sender, EventArgs e)
        {
            tool.ImageHeight = (int)nUpDown_ImageHeight.Value;
            gen_cam_par_area_scan_division((double)tool.Focus / 1000, 0, (double)tool.Sx / 1000000, (double)tool.Sy / 1000000, tool.ImageWidth / 2, tool.ImageHeight / 2, tool.ImageWidth, tool.ImageHeight, out startCamPar);
        }

        #endregion
        int index ;
        HObject ho_Caltab, ho_Cross;
        HImage testImage;
        List<HImage> calHImages = new List<HImage>();
        private void btn_grabCamera_Click(object sender, EventArgs e)
        {
            if (testImage != null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            testImage = tool.GetImage();
            if (testImage == null)
            {
                return;
            }
            if (testImage.IsInitialized() == false)
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

        private void DisplayCalib( HImage image)
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

        private void btn_CalibartionPlan_Click(object sender, EventArgs e)
        {
            if (tool.ModelImage != null && tool.ModelImage.IsInitialized()) ;
            tool.ModelImage = calHImages[index - 1].CopyImage();
            HOperatorSet.GetCalibData(CalibDataID, "calib_obj_pose", (new HTuple(0)).TupleConcat(index), "pose",out tool.worldPose);
            if (tool.worldPose != null && tool.worldPose.Length == 7)
            {
                StringBuilder strb = new StringBuilder();
                for (int i = 0; i < 6; i++)
                {
                    strb.Append(tool.worldPose[i].D.ToString("F3"));
                    strb.Append(",");
                }
                strb.Append(tool.worldPose[6].D.ToString());
                txt_WorldPose.Text = strb.ToString();
                txt_planPose.Text = strb.ToString();
                ExecuteInformation("参考平面设定成功");
            }
            else
            {
                tool.worldPose = null;
            }
        }

        private void cbb_CalibImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = int.Parse(cbb_CalibImages.SelectedItem.ToString());
            DisplayCalib(calHImages[index - 1]);
        }

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
        private void btn_saveParam_Click(object sender, EventArgs e)
        {
            if(tool.cameraParam==null || tool.cameraParam.Length != 9)
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
        private void btn_calibrateCamParam_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.CalibrateCameras(CalibDataID, out hv_Error);
                HOperatorSet.GetCalibData(CalibDataID, "camera", 0, "params", out tool.cameraParam);
                StringBuilder strb = new StringBuilder();
                strb.Append(tool.cameraParam[0].S+",");
                for (int i = 1; i < tool.cameraParam.Length; i++)
                {
                    if (i == 3 || i==4)
                    {
                        ExecuteInformation((tool.cameraParam[i].D * 1000000).ToString("F3") + ";" + Environment.NewLine);
                        strb.Append((tool.cameraParam[i].D * 1000000).ToString("F3"));
                    }
                    else if (i == 7 || i== 8)
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
                    
                    if (i< tool.cameraParam.Length-1)
                    {
                        strb.Append(",");
                    }
                }
                txt_CameraParam.Text = strb.ToString();
                gb_SettingPlanPose.Enabled = true;
               // btn_grabCamera.Enabled = false;
            }

            catch (HalconException HDevExpDefaultException1 )
            {
                HTuple hv_Exception;
                HDevExpDefaultException1.ToHTuple(out hv_Exception);
                MessageBox.Show("标定错误", hv_Exception.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ExecuteInformation("相机参数标定成功");
            try
            {
                ExecuteInformation("标定误差： "+hv_Error.D.ToString("F3")+"pixel");
            }
            catch
            {

            }
            
        }

        private void ckb_StartCalibration_CheckedChanged(object sender, EventArgs e)
        {
            
            if (tool.descrPath == null || tool.descrPath == string.Empty)
            {
                MessageBox.Show("读取描述文件", "请选择读取对应标定板描述文件", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }            
            if (ckb_StartCalibration.Checked)
            {
                gB_InitCamParam.Enabled = false;
                gb_StartCalibration.Enabled = true;
            }
            else
            {
                gB_InitCamParam.Enabled = true;
                gb_StartCalibration.Enabled = false;
            }
            index = 1;
            if (CalibDataID != null)
            {
                if(CalibDataID.Length == 0)
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
                    HOperatorSet.SetCalibDataCalibObject(CalibDataID, 0,tool.descrPath);
                }
                catch
                {
                    MessageBox.Show("描述文件错误", "读取描述失败，请检查文件该文件：" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }

        }


        #endregion
        #region 世界坐标系设定

        HImage modelImage;
        private void btn_grabImage_Click(object sender, EventArgs e)
        {
            if(modelImage!=null && modelImage.IsInitialized())
            {
                modelImage.Dispose();
            }
            modelImage = tool.GetImage();
            if(modelImage==null || !modelImage.IsInitialized())
            {
                MessageBox.Show("获取图像失败", "请检查相机或重新获取图像", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            mView.ClearList();
            mView.AddIconicVar(modelImage);
            mView.Repaint();
            btn_SetModelImage.Enabled = true;
        }
        private void MouseMovePoint(object sender, MouseEventArgs e)
        {
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
                txt_Row.Text = string.Format("{0:F3}", row);
                txt_Col.Text = string.Format("{0:F3}", col);
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
                mView.AddIconicVar(tool.ModelImage);
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
        private void GetMousePoint(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
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
                txt_Row.Text = string.Format("{0:F3}", row);
                txt_Col.Text = string.Format("{0:F3}", col);
                double row_temp, col_temp;
                double.TryParse(txt_Row.Text, out row_temp);
                double.TryParse(txt_Col.Text, out col_temp);
                tool.Points[index].Row = row_temp;
                tool.Points[index].Col = col_temp;
                dGV_worldPose.Refresh();

            }
            catch (HalconException)
            {
                return;
            }


        }
        HXLDCont modelPointXld;
        private void btn_SetModelImage_Click(object sender, EventArgs e)
        {
            tool.ModelImage = modelImage;

            MessageBox.Show("右侧选择标定点后，在左侧窗口选择对应点，单击更新对应点图像坐标，亦可在表格输入", "提示",  MessageBoxButtons.OK, MessageBoxIcon.Information);

            mView.ClearList();
            mView.SetDispLevel(ShowMode.ExcludeROI);
            mView.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
            mView.AddIconicVar(tool.ModelImage);
            mView.Repaint();

        }
        double row, col;
        private void btn_CalibrationCoord_Click(object sender, EventArgs e)
        {
            tool.Calibration();
            StringBuilder strb = new StringBuilder();
            if(tool.worldPose==null || tool.worldPose.Length != 7)
            {
                MessageBox.Show("标定失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ExecuteInformation("标定成功");
            strb.Append("X轴：" + tool.worldPose[0].D.ToString("F3"));
            strb.Append(";");
            strb.Append("Y轴：" + tool.worldPose[1].D.ToString("F3"));
            strb.Append(";");
            strb.Append("Z轴：" + tool.worldPose[2].D.ToString("F3"));
            strb.Append(";");
            strb.Append("RotX：" + tool.worldPose[3].D.ToString("F5"));
            strb.Append(";");
            strb.Append("RotY：" + tool.worldPose[4].D.ToString("F5"));
            strb.Append(";");
            strb.Append("RotZ：" + tool.worldPose[5].D.ToString("F5"));
            strb.Append(";");
            strb.Append("类型：" + tool.worldPose[6].I.ToString());
        }
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
                            pos.Row = double.Parse(vals[0]);
                            pos.Col = double.Parse(vals[1]);
                            pos.X = double.Parse(vals[2]);
                            pos.Y = double.Parse(vals[3]);
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
        private void btn_SaveWorldPose_Click(object sender, EventArgs e)
        {
            if(tool.worldPose==null || tool.worldPose.Length != 7)
            {
                MessageBox.Show("请先标定世界坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "世界坐标系文件(.dat)|*.dat|all files *.*| *.*";
            sfd.InitialDirectory = Path.Combine(Environment.CurrentDirectory, "CalibrationData");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.WritePose(tool.worldPose, sfd.FileName);
                ExecuteInformation("世界坐标系文件保存成功");
            }
        }

        #endregion
        #region 测试
        private void MouseTestPoint(object sender, MouseEventArgs e)
        {
            int state;
            try
            {
                mView.viewPort.HalconWindow.GetMpositionSubPix(out row, out col, out state);
                txt_Row.Text = string.Format("{0:F3}", row);
                txt_Col.Text = string.Format("{0:F3}", col);
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
                HTuple x_temp, y_temp;
                HOperatorSet.ImagePointsToWorldPlane(tool.cameraParam, tool.worldPose, row, col, 1, out x_temp, out y_temp);
                mView.AddText("X:=" + (1000*x_temp.D).ToString("F2") + ";" + "Y:= " + (1000*y_temp.D).ToString("F2"), (int)(row + 20), (int)col, 20, "green");
                txt_Verify_X.Text = (1000 * x_temp.D).ToString("F2");
                txt_Verify_Y.Text = (1000 * y_temp.D).ToString("F2");
                mView.Repaint();
                

            }

            catch (HalconException)
            {
                return;
            }

        }
        #endregion
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

        
        private void btn_GrabImageForTest_Click(object sender, EventArgs e)
        {
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

        private void tabCtrl_Func_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrl_Func.SelectedIndex == 1)
            {
                mView.viewPort.MouseMove -= MouseTestPoint;
                if (tool.cameraParam == null || tool.cameraParam.Length != 9)
                {
                    gB_worldPoseEnable.Enabled = false;
                    MessageBox.Show("相机参数为空", "请先标定相机参数", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    gB_worldPoseEnable.Enabled = true;
                    if (tool.ModelImage != null && tool.ModelImage.IsInitialized())
                    {
                        mView.ClearList();
                        mView.AddIconicVar(tool.ModelImage);
                    }
                    else
                    {
                        mView.ClearList();
                    }
                    mView.Repaint();
                }
                mView.viewPort.MouseDown += GetMousePoint;
                mView.viewPort.MouseMove += MouseMovePoint;
            }
            else if (tabCtrl_Func.SelectedIndex == 2)
            {
                if(tool.cameraParam==null || tool.cameraParam.Length!=9)
                {
                    MessageBox.Show("请先标定相机参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(tool.ModelImage==null || !tool.ModelImage.IsInitialized())
                {
                    MessageBox.Show("请先采集图片，标定世界坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(tool.worldPose==null || tool.worldPose.Length != 7)
                {
                    MessageBox.Show("请先标定世界坐标系", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                mView.viewPort.MouseDown -= GetMousePoint;
                mView.viewPort.MouseMove -= MouseMovePoint;
                mView.viewPort.MouseMove += MouseTestPoint;
                mView.ClearList();
                mView.AddIconicVar(tool.ModelImage);
                mView.Repaint();
            }
            else
            {
                mView.viewPort.MouseDown -= GetMousePoint;
                mView.viewPort.MouseMove -= MouseMovePoint;
                mView.viewPort.MouseMove -= MouseTestPoint;
            }
        }
        public override void Clear()
        {
            mView.viewPort.MouseDown -= GetMousePoint;
            mView.viewPort.MouseMove -= MouseMovePoint;
            base.Clear();
            if(CalibDataID!=null && CalibDataID.Length > 0)
            {
                HOperatorSet.ClearCalibData(CalibDataID);
            }
            if (calHImages != null)
            {
                for(int i = 0; i < calHImages.Count; i++)
                {
                    if (calHImages[i] != null && calHImages[i].IsInitialized())
                    {
                        calHImages[i].Dispose();
                    }
                }
                calHImages.Clear();
                calHImages = null;
            }
            if (ho_Caltab!=null && ho_Caltab.IsInitialized())
            {
                ho_Caltab.Dispose();
            }
            if (ho_Cross != null && ho_Cross.IsInitialized())
            {
                ho_Cross.Dispose();
            }
            if(testImage !=null && testImage.IsInitialized())
            {
                testImage.Dispose();
            }
            if(modelPointXld!=null && modelPointXld.IsInitialized())
            {
                modelPointXld.Dispose();
            }
        }


    }
}
