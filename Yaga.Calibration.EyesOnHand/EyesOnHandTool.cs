using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Yoga.ImageControl;
using System.ComponentModel;
using Yoga.Tools.CreateImage;
using Yoga.Tools;
using System.IO;
using System.Windows.Forms;

namespace Yoga.Calibration.EyesOnHand
{
    [Serializable]
    public class CalibPair
    {
        private double c_x;
        private double c_y;
        private double c_z;
        private double r_x;
        private double r_y;

        public double C_x
        {
            get
            {
                return c_x;
            }

            set
            {
                c_x = value;
            }
        }
        public double C_y
        {
            get
            {
                return c_y;
            }

            set
            {
                c_y = value;
            }
        }
        public double C_z
        {
            get
            {
                return c_z;
            }

            set
            {
                c_z = value;
            }
        }
        public double R_X
        {
            get
            {
                return r_x;
            }

            set
            {
                r_x = value;
            }
        }
        public double R_Y
        {
            get
            {
                return r_y;
            }

            set
            {
                r_y = value;
            }
        }
    }

    public delegate void ExeInfo(string value);
    [Serializable]
    public class EyesOnHandTool : ToolBase, IToolEnable
    {
        [NonSerialized]
        public ExeInfo NotifyExcInfo;
        public HTuple cameraParam;
        public HTuple MPPose;
        public HTuple MPInCamToolPose;
        public string descrPath;

        public HTuple MPInCamToolHomMat;
        public double refRow, refCol;
        public double Mask_RC_X, Mask_RC_Y;
        public double Mask_RT_X, Mask_RT_Y;
        public double Tool_Comp_X, Tool_Comp_Y;





        [NonSerialized]
        private CreateImageTool createImageTool;
        [NonSerialized]
        HTuple errR_X_Max ,  errR_Y_Max ;
        public CreateImageTool CreateImageTool
        {
            get
            {
                if (createImageTool == null)
                {
                    createImageTool = ToolsFactory.GetToolList(settingIndex)[0] as CreateImageTool;
                }
                return createImageTool;
            }
        }

        private HImage modelImage;
        private HImage mPImage;
        public HImage ModelImage
        {
            get
            {
                return modelImage;
            }

            set
            {
                modelImage = value;
            }
        }
        public HImage MPImage
        {
            get
            {
                return mPImage;
            }

            set
            {
                mPImage = value;
            }
        }


        #region 相机内参
        public int Focus = 8;
        public double Sx = 3.45;
        public double Sy = 3.45;
        public int ImageWidth = 2448;
        public int ImageHeight = 2048;
        public double CaltabHeight = 3;
        #endregion

        #region 参考平面与手臂坐标
        BindingList<CalibPair> points;
        public BindingList<CalibPair> Points
        {
            get
            {
                if (points == null)
                {
                    points = new BindingList<CalibPair>();
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CalibPair pos = new CalibPair();
                            points.Add(pos);
                        }

                    }
                }
                return points;
            }

            set
            {
                points = value;
            }
        }
        #endregion

        #region 补偿
        //工具Z轴旋转角
        public double angle;
        //旋转首尾两点
        public HTuple CalToolPointX, CalToolPointY;
        //CamTool坐标系下计算得到的工具中心坐标
        public double X0, Y0;
        //标定平面在工具的姿势
        public HTuple MPInToolPose;
        //Mark在Tool坐标系下的位置
        public string MarkInToolCoordStr;
        public HTuple MarkInToolCoord;

        //Mark在CamTool坐标系下的位置
        public HTuple MarkInCamToolCoord;

        //拍照当前姿势
        public HTuple CurrentGrabPose;

        //Mark点姿势
        public HTuple MarkInBasePoseMenu;

        //相机中心在工具坐标系下的位置。
        public double X_Comp_T, Y_Comp_T;


        #endregion


        internal void Calibration()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].C_x == 0 && Points[i].C_y == 0 && Points[i].C_z == 0 && Points[i].R_X == 0 && Points[i].R_Y == 0)
                {
                    MessageBox.Show("第"+(i+1)+"点数据异常", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            HTuple Cam_X_All = new HTuple(), Cam_Y_All = new HTuple(),Cam_Z_ALL=new HTuple();
            HTuple    Robot_X_All = new HTuple(), Robot_Y_All = new HTuple();
            for (int i = 0; i < Points.Count; i++)
            {
                Cam_X_All[i] = Points[i].C_x;
                Cam_Y_All[i] = Points[i].C_y;
                Cam_Z_ALL[i] = Points[i].C_z;
                Robot_X_All[i] = Points[i].R_X;
                Robot_Y_All[i] = Points[i].R_Y;
            }
            //  //九点的矩阵映射
            //  HOperatorSet.VectorToHomMat2d(Cam_X_All, Cam_Y_All, Robot_X_All, Robot_Y_All, out homMat2D);
            ////  homMat2D.VectorToHomMat2d(Cam_X_All, Cam_Y_All, Robot_X_All, Robot_Y_All);

            //  HTuple check_R_XS, check_R_YS;
            //  //将九点直接验证结果
            //  HOperatorSet.AffineTransPoint2d(homMat2D, Cam_X_All, Cam_Y_All, out check_R_XS, out check_R_YS);   
            //  //check_R_XS = homMat2D.AffineTransPoint2d(Cam_X_All, Cam_Y_All, out check_R_YS);
            //  //获取到行及列方向的误差
            HTuple Robot_Z_All;
            HOperatorSet.TupleGenConst(Robot_X_All.L, new HTuple(0.0), out Robot_Z_All);
            HOperatorSet.VectorToHomMat3d("rigid", Cam_X_All, Cam_Y_All, Cam_Z_ALL, Robot_X_All, Robot_Y_All, Robot_Z_All, out MPInCamToolHomMat);
            HOperatorSet.HomMat3dToPose(MPInCamToolHomMat, out MPInCamToolPose);

            HTuple Robot_X_All_chk, Robot_Y_All_chk, Robot_Z_All_chk;
            HOperatorSet.AffineTransPoint3d(MPInCamToolHomMat, Cam_X_All, Cam_Y_All, Cam_Z_ALL, out Robot_X_All_chk, out Robot_Y_All_chk, out Robot_Z_All_chk);
            HTuple errR_X = (Robot_X_All - Robot_X_All_chk).TupleAbs();
            HTuple errR_Y = (Robot_Y_All - Robot_Y_All_chk).TupleAbs();
            errR_X_Max = errR_X.TupleMax();
            errR_Y_Max = errR_Y.TupleMax();
            StringBuilder stb = new StringBuilder();
            stb.Append("X、Y方向误差：" + Environment.NewLine);
            for(int i=0;i< errR_X.Length; i++)
            {
                string temp = errR_X.D.ToString("F6");
                string temp2 = errR_Y.D.ToString("F6");
                stb.Append(temp + ";" + temp2 + Environment.NewLine);
            }
            if (NotifyExcInfo != null)
            {
                NotifyExcInfo(stb.ToString());
                NotifyExcInfo("X方向最大误差：" + errR_X_Max.D.ToString("F6") + ";" + "方向最大误差：" + errR_Y_Max.D.ToString("F6"));
            }
        }

        internal void Calibration_Tool_MP()
        {
            HTuple ToolInCamHomMat,homMat3dIden, CamInTooHomMat,homMatToolToMp;
            HOperatorSet.HomMat3dIdentity(out homMat3dIden);
            HOperatorSet.HomMat3dTranslate(homMat3dIden, new HTuple(X0), new HTuple(Y0), new HTuple(0.0),out ToolInCamHomMat);
            HOperatorSet.HomMat3dInvert(ToolInCamHomMat, out CamInTooHomMat);
            HOperatorSet.HomMat3dCompose(CamInTooHomMat, MPInCamToolHomMat,out homMatToolToMp);
            HOperatorSet.HomMat3dToPose(homMatToolToMp, out MPInToolPose);
        }


        #region 反射调用
        protected static string toolType = "眼在手标定模块";
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
        #region 通用
        public HImage GetImage()
        {
            CreateImageTool.GetImage();
            return CreateImageTool.ImageTestOut;
        }
        public EyesOnHandTool(int settingIndex)
        {
            base.settingIndex = settingIndex;
            this.IsOk = true;
            this.IsSubTool = true;
        }
        public override ToolsSettingUnit GetSettingUnit()
        {
            if (settingUnit == null)
            {
                settingUnit = new CalibrationSetting(this);
            }
            return settingUnit;
        }
        public override void SerializeCheck()
        {
            NotifyExcInfo = null;

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public override string GetSendResult()
        {
            return string.Empty;
        }
        public override void ShowResult(HWndCtrl viewCtrl)
        {
            ;
        }
        #endregion
    }
}
