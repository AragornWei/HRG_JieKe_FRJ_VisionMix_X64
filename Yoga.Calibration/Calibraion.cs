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

namespace Yoga.Calibration
{
    [Serializable]
    public class CalibPair
    {
        private double row;
        private double col;
        private double x;
        private double y;

        public double Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }

        public double Col
        {
            get
            {
                return col;
            }

            set
            {
                col = value;
            }
        }

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
    }

    public delegate void ExeInfo(string value);

    [Serializable]
    public class Calibraion : ToolBase, IToolEnable
    {

        [NonSerialized]
        public ExeInfo NotifyExcInfo;
        public HTuple cameraParam;
        public HTuple worldPose;
        public string descrPath;
        [NonSerialized]
        private CreateImageTool createImageTool;

        private HImage modelImage;
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

        #region 相机内参
        public int Focus = 8;
        public double Sx = 3.45;
        public double Sy = 3.45;
        public int ImageWidth = 2448;
        public int ImageHeight = 2048;
        #endregion

        #region 世界坐标系
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

        public HImage GetImage()
        {
            //CreateImageTool.Run();
            CreateImageTool.GetImage();
            return CreateImageTool.ImageTestOut;
        }

        #region 反射调用
        protected static string toolType = "喜利得相机标定模块";
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
                return toolType + "\r\n版本:2020\r\n说明:";
            }
        }
        public override string GetToolType()
        {
            return toolType;
        }
        #endregion
        public Calibraion(int settingIndex)
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
        #region 无用
        public override string GetSendResult()
        {
            return string.Empty;
        }
        public override void ShowResult(HWndCtrl viewCtrl)
        {
            ;
        }

        public void Calibration()
        {
            if(cameraParam==null || cameraParam.Length != 9)
            {
                if (NotifyExcInfo != null)
                {
                    NotifyExcInfo("请先标定相机参数");
                }
                return;
            }

            HTuple Rows = new HTuple(), Cols = new HTuple(), Xs = new HTuple(), Ys = new HTuple();
            for (int i = 0; i < Points.Count; i++)
            {
                Rows[i] = new HTuple(Points[i].Row);
                Cols[i] = new HTuple(Points[i].Col);
                Xs[i] = new HTuple(Points[i].X);
                Ys[i] = new HTuple(Points[i].Y);
            }
            HTuple Zs = new HTuple();
            HOperatorSet.TupleGenConst(Xs.Length, 0, out Zs);
            HTuple Quality;

            HOperatorSet.VectorToPose(Xs, Ys, Zs, Rows, Cols, cameraParam, new HTuple("analytic"), new HTuple("error"), out worldPose, out Quality);
            if (NotifyExcInfo != null)
            {
                NotifyExcInfo("标定均方根误差： " + Quality.D.ToString("F6") + "m");
            }

            HTuple X_temp, Y_temp;
            HOperatorSet.ImagePointsToWorldPlane(cameraParam, worldPose, Rows, Cols, 1, out X_temp, out Y_temp);


            HTuple errX = (X_temp - Xs).TupleAbs();
            HTuple errY = (Y_temp - Ys).TupleAbs();
            if (NotifyExcInfo != null)
            {
                NotifyExcInfo("最大X方向误差： " + errX.TupleMax().D.ToString("F6") + "m");
                NotifyExcInfo("最大Y方向误差： " + errY.TupleMax().D.ToString("F6") + "m");
            }
        }
        #endregion

    }
}
