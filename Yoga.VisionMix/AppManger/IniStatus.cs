using System;
using Yoga.Common.FileAct;

namespace Yoga.VisionMix
{
    public class IniStatus  //允许在其它程序集中访问，所以加public
    {
        public static IniStatus Instance = new IniStatus();
        private string password;
        private string checkPassword;
        private int ngImageCount;
        private int saveNgImage;
        private int saveOkImage;
        private int camearCount;
        private int cameraFlag;

        private int windowWidth;
        private int windowHeigth;
        private int logionDelay;

        private string directShowIndex;
        private string directShowColorSpace;
        private string directShowCameraType;
        

        public string Password
        {
            get
            {
                return password;
            }

            private set
            {
                password = value;
            }
        }

        public int NgImageCount
        {
            get
            {
                return ngImageCount;
            }

            private set
            {
                ngImageCount = value;
            }
        }

        public int CamearCount
        {
            get
            {
                if (camearCount > 9)
                {
                    camearCount = 9;
                }
                return camearCount;
            }

            private set
            {
                camearCount = value;
            }
        }

        public int CameraFlag
        {
            get
            {
                return cameraFlag;
            }

            set
            {
                cameraFlag = value;
            }
        }

        public int LogionDelay
        {
            get
            {
                return logionDelay;
            }

            set
            {
                logionDelay = value;
            }
        }

        public string CheckPassword
        {
            get
            {
                return checkPassword;
            }

            set
            {
                checkPassword = value;
            }
        }

        public string DirectShowIndex
        {
            get
            {
                return directShowIndex;
            }

            set
            {
                directShowIndex = value;
            }
        }

        public string DirectShowColorSpace
        {
            get
            {
                return directShowColorSpace;
            }

            set
            {
                directShowColorSpace = value;
            }
        }

        public string DirectShowCameraType
        {
            get
            {
                return directShowCameraType;
            }

            set
            {
                directShowCameraType = value;
            }
        }

        public int WindowWidth
        {
            get
            {
                return windowWidth;
            }

            set
            {
                windowWidth = value;
            }
        }

        public int WindowHeigth
        {
            get
            {
                return windowHeigth;
            }

            set
            {
                windowHeigth = value;
            }
        }

        public int SaveNgImage
        {
            get
            {
                return saveNgImage;
            }

            set
            {
                saveNgImage = value;
            }
        }
        public int SaveOkImage
        {
            get
            {
                return saveOkImage;
            }

            set
            {
                saveOkImage = value;
            }
        }

        public void ReadINI()
        {
            string iniPath = Environment.CurrentDirectory + "\\user.ini";
            string ngImageCountTmp = null;

            string saveNgImageTmp = null;
            string saveOkImageTmp = null;
            string camearCountTmp = null;
            string cameraFlagTmp = null;

            string windowWidthWidthTmp = null;
            string windowHeigthTmp = null;
            string logionDelayTmp = null;

            Password = INIOperation.GetValue(iniPath, "passWord", "admin", "123");

            CheckPassword = INIOperation.GetValue(iniPath, "passWord", "checker", "999");

            ngImageCountTmp = INIOperation.GetValue(iniPath, "setting", "ngImageCount", null);

            saveNgImageTmp = INIOperation.GetValue(iniPath, "setting", "saveNgImage", null);
            saveOkImageTmp = INIOperation.GetValue(iniPath, "setting", "saveOKImage", null);

            camearCountTmp = INIOperation.GetValue(iniPath, "setting", "camearCount", null);
            cameraFlagTmp = INIOperation.GetValue(iniPath, "setting", "cameraFlag", null);

            logionDelayTmp = INIOperation.GetValue(iniPath, "setting", "logionDelay", null);

            windowWidthWidthTmp = INIOperation.GetValue(iniPath, "setting", "WindowsWidth", null);
            windowHeigthTmp = INIOperation.GetValue(iniPath, "setting", "WindowsHeigth", null);

            directShowIndex = INIOperation.GetValue(iniPath, "DirectShow", "Index", "0");
            directShowColorSpace = INIOperation.GetValue(iniPath, "DirectShow", "ColorSpace", "gray");
            directShowCameraType = INIOperation.GetValue(iniPath, "DirectShow", "CameraType", "yuv (1600x1200)");

            int number;
            if (ngImageCountTmp == null || (int.TryParse(ngImageCountTmp, out number) == false))
            {
                NgImageCount = 2000;
                INIOperation.WriteValue(iniPath, "setting", "ngImageCount", NgImageCount.ToString());
            }
            else
            {
                NgImageCount = number;
            }

            if (saveNgImageTmp == null || (int.TryParse(saveNgImageTmp, out saveNgImage) == false))
            {
                SaveNgImage = 0;
                INIOperation.WriteValue(iniPath, "setting", "saveNgImage", SaveNgImage.ToString());
            }

            if (saveOkImageTmp == null || (int.TryParse(saveOkImageTmp, out saveOkImage) == false))
            {
                SaveOkImage = 0;
                INIOperation.WriteValue(iniPath, "setting", "saveOkImage", SaveOkImage.ToString());
            }

            if (camearCountTmp == null || (int.TryParse(camearCountTmp, out camearCount) == false))
            {
                camearCount = 1;
                INIOperation.WriteValue(iniPath, "setting", "camearCount", CamearCount.ToString());
            }


            if (cameraFlagTmp == null || (int.TryParse(cameraFlagTmp, out cameraFlag) == false))
            {
                INIOperation.WriteValue(iniPath, "setting", "cameraFlag", cameraFlag.ToString());
            }


            if (logionDelayTmp == null || (int.TryParse(logionDelayTmp, out logionDelay) == false))
            {
                logionDelay = 2;
                INIOperation.WriteValue(iniPath, "setting", "logionDelay", logionDelay.ToString());
            }


            if (windowWidthWidthTmp == null || (int.TryParse(windowWidthWidthTmp, out windowWidth) == false))
            {
                windowWidth = 800;
                INIOperation.WriteValue(iniPath, "setting", "WindowsWidth", windowWidth.ToString());
            }


            if (windowHeigthTmp == null || (int.TryParse(windowHeigthTmp, out windowHeigth) == false))
            {
                windowHeigth = 600;
                INIOperation.WriteValue(iniPath, "setting", "WindowsHeigth", windowHeigth.ToString());
            }


            if (cameraFlag > 0)
            {
                //cameraFlag = 1;
                Camera.CameraManger.CameraFlag = (Camera.CameraFlag)cameraFlag;
            }
            else
            {
                INIOperation.WriteValue(iniPath, "setting", "cameraFlag",
                    Convert.ToInt32(Camera.CameraManger.CameraFlag).ToString());
            }
            
            Camera.CameraManger.DirectShowIndex = directShowIndex;
            Camera.CameraManger.DirectShowCameraType = directShowCameraType;
            Camera.CameraManger.DirectShowColorSpace = directShowColorSpace;
        }
    }
}
