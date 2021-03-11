using System;
using Yoga.Common;
using Yoga.Common.Basic;
using Yoga.Common.FileAct;

namespace Yoga.VisionMix
{
    [Serializable]
    /// <summary>
    /// 用户自定义数据类
    /// </summary>
    class UserSetting  //只能在本程序集中使用，因为默认是internal类型。
    {
        public static UserSetting Instance = new UserSetting();

        private static readonly string userDataPath = Environment.CurrentDirectory + "\\user.dat";
        private string projectPath = Environment.CurrentDirectory + "\\project\\1.prj";
        private  string projectPathInit = Environment.CurrentDirectory + "\\project\\1.prj";

        private string softKey;
       // private Dictionary<int, CameraPram> cameraPramDic;

        private CommunicationParam mainDeviceComParam;  //主通讯参数
        private CommunicationParam subDeviceComParam;  //准备添加的副通讯参数

        public CommunicationParam MainDeviceComParam
        {
            get
            {
                if (mainDeviceComParam == null)
                {
                    mainDeviceComParam = new CommunicationParam();
                }
                return mainDeviceComParam;
            }

            set
            {
                mainDeviceComParam = value;
            }
        }

        public string ProjectPath
        {
            get
            {
                if (projectPath == null)
                {
                    projectPath = projectPathInit;
                }
                return projectPath;
            }

            set
            {
                projectPath = value;
            }
        }

        public static string UserDataPath
        {
            get
            {
                return userDataPath;
            }
        }

        public string SoftKey
        {
            get
            {
                return softKey;
            }

            set
            {
                softKey = value;
            }
        }

        public void ReadSetting()
        {
            UserSetting setting = SerializationFile.DeserializeObject(UserDataPath) as UserSetting;
            if (setting != null)
            {
                //setting = new UserSetting();
                Instance = setting;
            }
            //Instance = setting;
        }

        public void SaveSetting()
        {
            if (projectPath == projectPathInit)
            {
                projectPath = null;     //保存projectPath为null，当使用时调用ProjectPath返回projectPathInit值。
            }
            if (SerializationFile.TrySerialize(Instance))
            {
                try
                {
                    SerializationFile.SerializeObject(userDataPath, Instance);
                }
                catch(Exception)
                {
                    //Util.WriteLog(this.GetType(), ex);
                    Util.Notify(Level.Err,"用户设置保存失败");
                }
            }
        }
    }
}
