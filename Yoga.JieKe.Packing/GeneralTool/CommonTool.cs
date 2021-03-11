using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoga.ImageControl;
using Yoga.Common;
using Yoga.Tools;
using System.IO;
using HalconDotNet;

namespace Yoga.JieKe.Packing.GeneralTool
{

    public delegate void ExeInfo(string value);
    [Serializable]
    public class CommonTool : ToolBase, IToolEnable, IToolRun
    {
        [NonSerialized]
        public ExeInfo NotifyExcInfo;

        #region 反射调用
        protected static string toolType = "杰克-缝纫机打包通用程序";
        public static string ToolType
        {
            get
            {
                return toolType;
            }
        }
        public static string ToolExplanation
        {
            get
            {
                return toolType + "\r\n版本:2021\r\n说明:";
            }
        }
        public override string GetToolType()
        {
            return toolType;
        }
        #endregion

        #region 创建模板

        private CreateShapeModel createShapeModel = new CreateShapeModel();
        public CreateShapeModel MCreateShapeModel
        {
            get
            {
                if (createShapeModel == null)
                    createShapeModel = new CreateShapeModel();
                return createShapeModel;
            }
            set
            {
                createShapeModel = value;
            }

        }
        #endregion

        #region 查找模板
        private FindShapeMode findShapeMode = new FindShapeMode();
        public FindShapeMode MFindShapeMode
        {
            get
            {
                if (findShapeMode == null)
                {
                    findShapeMode = new FindShapeMode();
                }
                return findShapeMode;
            }
            set
            {
                findShapeMode = value;
            }
        }
        [NonSerialized]
        bool bFindShapeMode = false;

        #endregion

        #region 抓取点设定
        public bool bIsCalibration = false;
        GrabPointSetting mGrabPointSetting = new GrabPointSetting();
        public GrabPointSetting MGrabPointSetting
        {
            get
            {
                if (mGrabPointSetting == null)
                    mGrabPointSetting = new GrabPointSetting();
                return mGrabPointSetting;
            }
            set
            {
                mGrabPointSetting = value;
            }
        }
        public string cameraParamPath;
        public string MpInCamPosePath;
        public string MpInToolPosePath;
        public HTuple cameraParam;
        public HTuple MPInCamPose;
        public HTuple MPInToolPose;
        public HTuple Robot_Tool_Comp;
        public double x_Compensation, y_Compensation, angle_Compensation;
        //当前拍照姿势
        public HTuple CurrentShootPose;
        //抓取缝纫机姿势
        public HTuple ObjGrabPose;


        [NonSerialized]
        public bool bGrabPointSetting = false;

        #endregion

        #region 防呆
        FangDai mFangDai;
        public FangDai MFangDai
        {
            get
            {
                if (mFangDai == null)
                {
                    mFangDai = new FangDai();
                }
                return mFangDai;
            }
            set
            {
                mFangDai = value;
            }
        }
        public bool bFangDai_Enable = true;
        [NonSerialized]
        public bool bFangDai_Result = false;

        #endregion

        #region 工程
        [NonSerialized]
        public HDevEngine MyEngine = new HDevEngine();
        [NonSerialized]
        public bool engineIsnitial;
        public void InitialEngine()
        {
            if (MyEngine == null)
            {
                MyEngine = new HDevEngine();
            }
            MyEngine.SetProcedurePath(Environment.CurrentDirectory + "\\Engine\\");
            MyEngine.SetEngineAttribute("execute_procedures_jit_compiled", "true");
            engineIsnitial = true;
        }
        public void StartDebugMode()
        {
            MyEngine.SetEngineAttribute("execute_procedures_jit_compiled", "false");
            MyEngine.SetEngineAttribute("debug_port", 57786);
            MyEngine.StartDebugServer();
        }
        public void StopDebugMode()
        {
            MyEngine.StopDebugServer();
            MyEngine.SetEngineAttribute("execute_procedures_jit_compiled", "true");
        }

        #endregion

        public CommonTool(int settingIndex)
        {
            base.settingIndex = settingIndex;
            IsOutputResults = true;
            InitialEngine();
            createShapeModel.Initial(ImageRefIn);
        }
        public override ToolsSettingUnit GetSettingUnit()
        {
            if (settingUnit == null)
            {
                settingUnit = new CommonToolParamSetting(this);
            }
            return settingUnit;
        }
        //清除数据
        public override void ClearTestData()
        {
            base.ClearTestData();
        }
        //序列化
        public override void SerializeCheck()
        {
            NotifyExcInfo = null;
            MCreateShapeModel.SerializeCheck();
            MFindShapeMode.SerializeCheck();
            MGrabPointSetting.SerializeCheck();
            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        //主执行程序
        protected override void RunAct()
        {
            base.RunAct();


            #region 初始化

            if (!engineIsnitial)
            {
                MyEngine = new HDevEngine();
                InitialEngine();
            }
            RuningFinish = false;
            base.Result = string.Empty;
            base.IsOk = false;
            base.isRealOk = false;
            //功能块
            bFindShapeMode = false;
            bGrabPointSetting = false;

            if (ImageTestIn == null || !ImageTestIn.IsInitialized())
                return;
            int channels = ImageTestIn.CountChannels();

            HImage ImageGray;
            if (channels == 3)
            {

                ImageGray = ImageTestIn.Rgb1ToGray();

            }
            else
            {
                ImageGray = ImageTestIn.Clone();
            }

            #endregion

            #region 创建模板
            if (MCreateShapeModel.hShapeModel == null || !MCreateShapeModel.hShapeModel.IsInitialized()|| MCreateShapeModel.createNewModelID)
            {
                if (!MCreateShapeModel.CreateShapeModelAct(ImageRefIn))
                {
                    Util.Notify("创建模板异常");
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo("创建模板异常");
                    }
                    return;
                }

            }
            #endregion

            #region 模板匹配
            bFindShapeMode = MFindShapeMode.FindShapeModeAct(ImageRefIn, MCreateShapeModel, ImageTestIn);
            if (!bFindShapeMode)
            {
                Util.Notify("查找模板异常");
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo("查找模板异常");
                }
                return;
            }
            List<HHomMat2D> homMat2Ds = new List<HHomMat2D>();
            homMat2Ds = MFindShapeMode.GetHHomMat2Ds();
            if (MFindShapeMode.row == null || MFindShapeMode.row.Length < 1 || homMat2Ds == null)
            {
                Util.Notify("未找到模板");
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo("未找到模板");
                }
                base.IsOk = true;
                base.isRealOk = true;
                return;
            }
            if (bFindShapeMode)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append("查找结果:"+ Environment.NewLine);
                int count = MFindShapeMode.row.Length;
                for (int i = 0; i < count; i++)
                {
                    HTuple phi;
                    HOperatorSet.TupleDeg(MFindShapeMode.angle[i], out phi);
                    strb.Append("第" + (i + 1) + "个:\r\n");
                    string mes = string.Format(
                   "Row:{0:F2}\r\n" + "Col:{1:F2}\r\n" + "角度:{2:F2}\r\n" + "得分:{3:F2}"
                   , MFindShapeMode.row[i].D, MFindShapeMode.column[i].D, phi.D, MFindShapeMode.score[i].D);
                    strb.Append(mes + Environment.NewLine);

                }
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo(strb.ToString());
                }
                Util.Notify(strb.ToString());
            }


            #endregion

            #region 抓取点
            bGrabPointSetting = mGrabPointSetting.setTarget(homMat2Ds);
            if (!bGrabPointSetting)
            {
                Util.Notify("抓取点异常");
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo("抓取点异常");
                }
            }
            else
            {
                StringBuilder strd = new StringBuilder();
                for (int i = 0; i < MGrabPointSetting.GrabRowTarget.Length; i++)
                {
                    string mes = string.Format(
                        "第" + (i + 1) + "个抓取点像素坐标:\r\n" +
                        "Row: {0:F2}\r\n" +
                        "Col: {1:F2}",
                        MGrabPointSetting.GrabRowTarget[i].D,
                        MGrabPointSetting.GrabColTarget[i].D);
                    strd.Append(mes+ Environment.NewLine);
                }
                Util.Notify(strd.ToString());
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo(strd.ToString());
                }
            }
            #endregion     

            #region 防呆
            if (bFangDai_Enable)
            {
                bFangDai_Result = MFangDai.FangDai_Act(ImageGray, homMat2Ds);
                if (bFangDai_Result)
                {
                    StringBuilder strb = new StringBuilder();
                    strb.Append("防呆:"+ Environment.NewLine);
                    int count = MFangDai.Area.Length;
                    for (int i = 0; i < count; i++)
                    {
                        string mes = string.Format("第" + (i + 1) + "个:\r\n" +
                       "面积: {0:F2}"

                       , MFangDai.Area[i].D);
                        strb.Append(mes);
                        strb.Append(Environment.NewLine);
                    }
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo(strb.ToString());
                    }
                    Util.Notify(strb.ToString());
                }
                else
                {
                    Util.Notify("防呆检测异常");
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo("防呆检测异常");
                    }
                }
            }
            else
            {
                bFangDai_Result = true;
            }

            #endregion

            RuningFinish = true;
            base.IsOk = bFindShapeMode && bGrabPointSetting && bFangDai_Result;
            base.isRealOk = true;
            if (ImageGray != null && ImageGray.IsInitialized())
                ImageGray.Dispose();
        }

        //发送结果数据
        [NonSerialized]
        HTuple row_Send, col_Send, Angle_Send, id_Send;
        HTuple NG_Reason;
        public override string GetSendResult()
        {
            row_Send = new HTuple();
            col_Send = new HTuple();
            Angle_Send = new HTuple();
            id_Send = new HTuple();
            NG_Reason = new HTuple();

            if (!bFindShapeMode || !bGrabPointSetting)
            {
                return ("视觉程序异常" + Environment.NewLine);
            }
            if (MFindShapeMode.row == null || MFindShapeMode.row.Length < 1)
            {
                return ("未找到缝纫机" + Environment.NewLine);
            }

            int count = MFindShapeMode.angle.Length;

            #region 子功能运行是否异常
            //防呆
            if (bFangDai_Enable == false)
            {
                HOperatorSet.TupleGenConst(count, 1, out MFangDai.FangDai_OkNg);
            }
            else if (bFangDai_Enable && bFangDai_Result == false)
            {
                HOperatorSet.TupleGenConst(count, 0, out MFangDai.FangDai_OkNg);
            }
            if (count != MFangDai.FangDai_OkNg.Length)
            {
                return ("视觉程序异常" + Environment.NewLine);
            }

            #endregion


            for (int i = 0; i < MFindShapeMode.angle.Length; i++)
            {
                //NG
                if (MFangDai.FangDai_OkNg[i] != 1)
                {
                    //保存图像
                    DateTime dt = DateTime.Now;
                    string timeNow = dt.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    string NGImagePath = "D:\\data\\" + "\\NgImage\\" + "\\工具组" + settingIndex + "\\";
                    SaveImage(NGImagePath + timeNow + ".png", ImageTestIn);

                    NG_Reason = NG_Reason.TupleConcat(new HTuple("防呆"));
                    id_Send = id_Send.TupleConcat(new HTuple(0));
                }
                //OK 
                else
                {
                    id_Send = id_Send.TupleConcat(new HTuple(1));
                }
                row_Send = row_Send.TupleConcat(MGrabPointSetting.GrabRowTarget[i]);
                col_Send = col_Send.TupleConcat(MGrabPointSetting.GrabColTarget[i]);
                Angle_Send = Angle_Send.TupleConcat(MFindShapeMode.angle[i]);
            }

            StringBuilder outCoord = new StringBuilder();
            //坐标转换
            if (bIsCalibration)
            {
                outCoord.Clear();
                if (cameraParam == null || cameraParam.Length != 9)
                {
                    Util.Notify("相机参数异常");
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo("相机参数异常");
                    }                    
                }
                if (MPInCamPose == null || MPInCamPose.Length != 7)
                {
                    Util.Notify("参考坐标系异常");
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo("参考坐标系异常");
                    }
                }
                if (MPInToolPose == null || MPInToolPose.Length != 7)
                {
                    Util.Notify("手臂坐标系异常");
                    if (NotifyExcInfo != null)
                    {
                        NotifyExcInfo("手臂坐标系异常");
                    }
                }


                if (cameraParam != null && cameraParam.Length == 9 && 
                    MPInCamPose != null && MPInCamPose.Length == 7 && 
                    MPInToolPose != null && MPInToolPose.Length == 7)
                {
                    outCoord.Append("Image" + Environment.NewLine);

                    for (int i = 0; i < row_Send.Length; i++)
                    {
                        HTuple mpX, mpY;
                        HOperatorSet.ImagePointsToWorldPlane(cameraParam, MPInCamPose, row_Send[i], col_Send[i], 1, out mpX, out mpY);
                        HTuple homMat3d;
                        HOperatorSet.PoseToHomMat3d(MPInToolPose, out homMat3d);
                        HTuple rx, ry, rz;
                        HOperatorSet.AffineTransPoint3d(homMat3d, mpX, mpY, new HTuple(0.0), out rx, out ry, out rz);
                        HTuple angle;
                        HOperatorSet.TupleDeg(Angle_Send[i].D, out angle);
                        outCoord.Append("[");
                        string temp =
                              "X:" + (1000 * (rx.D+ Robot_Tool_Comp[0].D) + x_Compensation).ToString("F2") + ";"
                            + "Y:" + (1000 * (ry.D+ Robot_Tool_Comp[1].D)+ y_Compensation).ToString("F2") + ";"
                            + "A:" + (angle.D + angle_Compensation).ToString("F2") + ";"
                            + "ID:" + id_Send.I.ToString();
                        outCoord.Append(temp);
                        outCoord.Append("]" + Environment.NewLine);
                    }
                    outCoord.Append("Done" + Environment.NewLine);
                }
                else
                {
                    return ("视觉程序异常" + Environment.NewLine);
                }


            }
            else
            {
                outCoord.Clear();
                outCoord.Append("Image" + Environment.NewLine);

                for (int i = 0; i < row_Send.Length; i++)
                {

                    HTuple angle;
                    HOperatorSet.TupleDeg(Angle_Send[i].D, out angle);
                    outCoord.Append("[");
                    string temp =
                        "X:" + (row_Send[i].D + x_Compensation).ToString("F2") + ";" +
                        "Y:" + (col_Send[i].D + y_Compensation).ToString("F2") + ";" +
                        "A:" + (angle.D + angle_Compensation).ToString("F2") + ";" +
                        "ID:" + id_Send.I.ToString() + ";";
                    outCoord.Append(temp);
                    outCoord.Append("];" + Environment.NewLine);
                }
                outCoord.Append("Done" + Environment.NewLine);
            }
            return (outCoord.ToString());

        }
        public void SetGrabPoint(double x, double y,double z)
        {
            //转换camera 、 projection
            MGrabPointSetting.X = x;
            MGrabPointSetting.Y = y;
            MGrabPointSetting.Z = z;
            HTuple Cx, Cy, Cz, homMat3d;
            HOperatorSet.PoseToHomMat3d(MPInCamPose, out homMat3d);
            HOperatorSet.AffineTransPoint3d(homMat3d, x, y, z, out Cx, out Cy, out Cz);
            HTuple row, column;
            HOperatorSet.Project3dPoint(Cx, Cy, Cz, cameraParam, out row, out column);
            MGrabPointSetting.SetGrabPoint(row.D, column.D);
        }
        public override void ShowResult(HWndCtrl viewCtrl)
        {
            if (!bFindShapeMode)
                return;
            if (MFindShapeMode.row == null || MFindShapeMode.row.Length < 1)
            {
                return;
            }
            if (RuningFinish == false)
                return;
            MFindShapeMode.ShowResult(viewCtrl);
            if (bGrabPointSetting)
            {
                MGrabPointSetting.ShowGrabPoint(viewCtrl);
            }

            if (bFangDai_Enable && bFangDai_Result)
            {
                MFangDai.Show(viewCtrl);
            }
            
            if (NotifyExcInfo != null)
            {
                string temp = GetSendResult();
                NotifyExcInfo("发送结果：" + temp);
            }

            if (row_Send != null && row_Send.Length > 0)
            {
                for (int i = 0; i < row_Send.Length; i++)
                {
                    if (id_Send[i].I == 1)
                    {
                        viewCtrl.AddText("OK", (int)(row_Send[i].D), (int)(col_Send[i].D), 80, "green");
                    }
                    else
                    {
                        viewCtrl.AddText("NG", (int)(row_Send[i].D), (int)(col_Send[i].D), 80, "green");
                    }

                }
            }
        }
        public override void Close()
        {
            try
            {
                base.Close();
                MCreateShapeModel.Close();
                MCreateShapeModel = null;
                MFindShapeMode.Close();
                MFindShapeMode = null;
                MGrabPointSetting.Close();
                MGrabPointSetting = null;
                MFangDai.Close();
                MFangDai = null;
                NotifyExcInfo = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                Util.Notify("工具删除异常");
            }

        }
        private void SaveImage(string files, HImage ngImage)
        {

            if (ngImage == null || ngImage.IsInitialized() == false)
            {
                Util.WriteLog(this.GetType(), "异常图像数据丢失");
                Util.Notify("异常图像数据丢失");
                return;
            }
            HImage imgSave = ngImage.CopyImage();
            Task.Run(() =>
            {
                try
                {
                    FileInfo fi = new FileInfo(files);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }
                    imgSave.WriteImage("png", 0, files);
                    imgSave.Dispose();
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify(string.Format("相机{0}异常图像保存异常", settingIndex));
                }
            });

        }
    }
}
