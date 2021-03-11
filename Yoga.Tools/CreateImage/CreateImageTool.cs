using HalconDotNet;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Yoga.Camera;
using Yoga.Common;
using Yoga.ImageControl;

namespace Yoga.Tools.CreateImage
{
    [Serializable]
    public class CreateImageTool : ToolBase, IToolRun, ICreateImage
    {
        #region 类的字段
        private static string toolType = "图像采集";  //务必要定义静态的，即所有对象共享该变量。

        private int cameraIndex = 1;

        private int offLineImageIndex = 0;

        private string offLineImagePath;

        private HTuple imageFiles;

        private HImage refImage;  //可以序列化保存的，所有工具载入时都是以这个保存的图像。

        [NonSerialized]
        private bool offLineMode;
        
        [NonSerialized]
        private bool offLineCycleTest;

        [NonSerialized]
        private double startTime;

        private bool allReadFinish = false;
        [NonSerialized]
        private CameraBase camera;
        [NonSerialized]
        bool isExtTrigger = false;
        [NonSerialized]
        bool isStopOffLineTest = false;

        #endregion 

        public CreateImageTool(int settingIndex)   //settingIndex代表第几号工具。
        {
            base.settingIndex = settingIndex;
            //cameraIndex = settingIndex;   //要让被连接的相机编号与工具编号相同。
        }

        #region 类的属性

        public bool AllReadFinish
        {
            get
            {
                return allReadFinish;
            }
            set
            {
                allReadFinish = value;
            }
        }

        #region 供反射调用属性
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
                return toolType + "\r\n版本:20191129\r\n说明:";
            }
        }
        #endregion

        public int CameraIndex    //针对该工具的相机号。
        {
            get
            {
                return cameraIndex;
            }

            set
            {
                cameraIndex = value;
            }
        }

        public bool OffLineMode
        {
            get
            {
                return offLineMode;
            }

            set
            {
                offLineMode = value;
            }
        }

        public bool OffLineCycleTest
        {
            get
            {
                return offLineCycleTest;
            }
            set
            {
                offLineCycleTest = value;
            }
        }

        public bool IsStopOffLineTest
        {
            set
            {
                isStopOffLineTest=value;
            }
        }

        public string OffLineImagePath
        {
            get
            {
                if (offLineImagePath ==null)
                    offLineImagePath = "D:\\SaveImage";
                return offLineImagePath;
            }

            set
            {
                if(RefushImagePathList(value))
                    offLineImagePath = value;               
            }
        }

        public CameraBase Camera
        {
            get
            {
                if (CameraManger.CameraDic.ContainsKey(cameraIndex))
                    camera = CameraManger.CameraDic[cameraIndex];
                else
                    camera = null;

                return camera;
            }
        }

        public double StartTime
        {
            get
            {
                return startTime;
            }
            private set
            {
                startTime = value;
            }
        }

        public HImage RefImage
        {
            get
            {
                return refImage;
            }
            set
            {
                refImage = value;
            }
        }
        #endregion

        public override HImage GetImageResultRefOut()
        {
            return refImage;
        }

        public void GetImage()   //在工具设置界面采集图像时调用该函数采集图像。
        {
            if (Camera == null)
            {
                MessageBox.Show("当前无相机", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ImageTestOut = Camera.GrabImage(1500);
        }
        public override void LoadTool()
        {
            base.LoadTool();
            //try
            //{
            //    RefushImagePathList(offLineImagePath);
            //}
            //catch (Exception) { }
        }
        public bool RefushImagePathList(string offLineImagePath)
        {
            //if(imageFiles.Type!=HTupleType.EMPTY)
            //if (imageFiles != null)
            //{
            //    HOperatorSet.DeleteFile(imageFiles);
            //}
            offLineImageIndex = 0;
            if (offLineImagePath!=null)
            {
                HOperatorSet.ListFiles(offLineImagePath, (new HTuple("files")).TupleConcat("follow_links"), out imageFiles);   //“follow_links”表示一个接一个连在一起
                HOperatorSet.TupleRegexpSelect(imageFiles, (new HTuple("\\.(bmp|jpg|jpeg|png)$")).TupleConcat("ignore_case"), out imageFiles); //"ignore_case"表示忽略大小写。

                //HOperatorSet.TupleRegexpSelect(imageFiles, (new HTuple("\\.(tif|tiff|gif|bmp|jpg|jpeg|jp2|png|pcx|pgm|ppm|pbm|xwd|ima|hobj)$")).TupleConcat(
                //    "ignore_case"), out imageFiles);   //"ignore_case"表示忽略大小写。               
            }
            if (imageFiles.TupleLength() > 0)
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show("当前文件夹没有照片");
                return false;
            }
        }      

        public override ToolsSettingUnit GetSettingUnit()
        {
            if (settingUnit == null)
            {
                settingUnit = new CreateImageUnit(this);
            }
            return settingUnit;
        }

        public override string GetToolType()
        {
            return toolType;
        }

        public void SettExtTriggerData(ImageEventArgs imageData) //工具组实际运行(处理图像)时调用该函数获取实时图像
        {
            isExtTrigger = true;
            DisposeHobject(ImageTestOut);
            ImageTestOut = imageData.CameraImage;
            StartTime = imageData.StartTime.D;           //获取图像处理开始时的时间
        }
        public void SetExtTriggerDataOff()
        {
            isExtTrigger = false;
        }
        protected override void RunAct()
        {
            try
            {
                if (isExtTrigger == false)
                {                  
                    HOperatorSet.CountSeconds(out runStartTime);
                    StartTime = runStartTime.D;
                    //由于这里是图像的源头,可以直接清除图像
                    DisposeHobject(ImageTestOut);
                    if (OffLineMode == true)
                    {
                        ImageTestOut = new HImage();

                        if (imageFiles == null || imageFiles.TupleLength() < 1)
                        {
                            Util.Notify(string.Format("工具{0}无离线数据", this.Name));
                            return;
                        }               
                        ImageTestOut.ReadImage(imageFiles.TupleSelect(offLineImageIndex));
                        Result = "图像采集完成";
                        offLineImageIndex++;                       

                        if ((offLineImageIndex > (imageFiles.TupleLength() - 1))||isStopOffLineTest)
                        {
                            allReadFinish = true;
                            offLineImageIndex = 0;
                        }
                        else
                        {
                            allReadFinish = false;
                        }
                    }

                }
                else
                {
                    //ImageTestOut已经通过RunThreadData中的RunAllTool()调用SettExtTriggerData()获取。
                    //外触发,收到图像时间就是图像处理的开始时间
                    runStartTime = StartTime;
                }
                if (ImageTestOut != null && ImageTestOut.IsInitialized())
                {
                    IsOk = true;
                    isRealOk = true;
                    Result = "OK";
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                string message = "";
                if (OffLineMode)
                {
                    message = "离线图像路径异常,请检查设置";
                }
                else
                {
                    message = "相机图像采集异常,请检查设置";
                }
                Util.Notify(string.Format("工具{0}运行异常:{1}", Name, message));
            }
        }

        public override void SerializeCheck()
        {
            if (refImage != null && refImage.IsInitialized() == false)
            {
                refImage = null;
            }
            base.SerializeCheck();
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }

        public void ShowImage(HWndCtrl viewCtrl)
        {
            if (ImageTestOut != null && ImageTestOut.IsInitialized())
            {
                viewCtrl.AddIconicVar(ImageTestOut);
            }
        }
        public override void ShowResult(HWndCtrl viewCtrl)
        {
            //if (Enable == false)
            //{
            //    return;
            //}
            //viewCtrl.ClearList();
            //if (RuningFinish == false)
            //{
            //    return;
            //}
            //if (testImage != null && testImage.IsInitialized())
            //{
            //    viewCtrl.AddIconicVar(testImage);
            //}
        }
        public override string GetSendResult()
        {
            return string.Empty;
        }

        public int GetOffLineImageFileSum()
        {
            if (imageFiles == null)
                return 0;
            else
            {
                if (imageFiles.TupleLength() > 0)
                    return imageFiles.TupleLength();
                else
                    return 0;
            }
                
        }

    }
}
