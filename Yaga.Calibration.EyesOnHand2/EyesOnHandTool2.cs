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

namespace Yoga.Calibration.EyesOnHand2
{
    [Serializable]
    public class RobotPose
    {
        private double x;
        private double y;
        private double z;
        private double rx;
        private double ry;
        private double rz;

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }
        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }
        public double Z
        {
            get
            {
                return z;
            }

            set
            {
                z = value;
            }
        }
        public double RX
        {
            get
            {
                return rx;
            }

            set
            {
                rx = value;
            }
        }
        public double RY
        {
            get
            {
                return ry;
            }

            set
            {
                ry = value;
            }
        }
        public double RZ
        {
            get
            {
                return rz;
            }

            set
            {
                rz = value;
            }
        }

    }

    public delegate void ExeInfo(string value);
    [Serializable]
    public class EyesOnHandTool2 : ToolBase, IToolEnable
    {
        [NonSerialized]
        public ExeInfo NotifyExcInfo;
        [NonSerialized]
        private CreateImageTool createImageTool;        
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

        #region 相机内参
        public int Focus = 8;
        public double Sx = 3.45;
        public double Sy = 3.45;
        public int ImageWidth = 2448;
        public int ImageHeight = 2048;
        public double CaltabHeight = 3;
        public HTuple cameraParam;
        public HTuple MPInCamPose;
        public string descrPath;
        private HImage mPImage;
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
        #endregion
        #region 机器人与相机标定
        BindingList<RobotPose> RobotCurrentPoseList;
        public BindingList<RobotPose> RbtCurrentPoseList
        {
            get
            {
                if (RobotCurrentPoseList == null)
                {
                    RobotCurrentPoseList = new BindingList<RobotPose>();
                }
                return RobotCurrentPoseList;
            }

            set
            {
                RobotCurrentPoseList = value;
            }
        }
        //标定数据
        public HTuple ToolInCamPose;
        //标定平面在工具的姿势
        public HTuple MPInToolPose;
        [NonSerialized]
        HTuple errR_X_Max, errR_Y_Max;
        #endregion
        #region 反射调用
        protected static string toolType = "眼在手标定模块2";
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
                return toolType + "\r\n版本:2021\r\n说明:3维标定方式";
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
        public EyesOnHandTool2(int settingIndex)
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
