using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Yoga.Common;

namespace Yoga.Tools
{
    public static class ToolsFactory
    {
        private static SortedDictionary<int, List<ToolBase>> toolsDic;  //在ProjectData中的是可序列化的。

        private static List<ToolReflection> allToolsType;
        public static readonly string PlugInsDir = System.IO.Path.Combine(System.Environment.CurrentDirectory, "PlugIns\\");

        private static object lockObj = new object();
        static ToolsFactory()  //对于静态构造函数来说，首先会调用它，才会调用非静态构造函数
        {
            GetToolList(1); //当不存在指定的*.prj项目文件时，就新生成一个：产品1.prj文件，就是用该处生成的toolDic，因为没有读入项目覆盖它。
        }       
        public static SortedDictionary<int, List<ToolBase>> ToolsDic
        {
            get
            {
                if (toolsDic == null)     //一般是先执行Autounit的LoadProject()后再使用ToolsDic，这样，读取的都是上次保存的。
                {
                    toolsDic = new SortedDictionary<int, List<ToolBase>>();
                }
                return toolsDic;
            }

            set
            {
                toolsDic = value;
                foreach (var tools in toolsDic.Values)
                {
                    foreach (var tool in tools)
                    {
                        tool.LoadTool(); //每个工具都会先执行base.LoadTool();该函数中调用LinkTestImage(),调用函数中判断是CreateImageTool工具，立即返回，
                    }                    //如果不是，则ImageTestIn = ImageSoureTool.ImageTestOut;
                }
            }
        }
        /// <summary>
        /// 获取工具组
        /// </summary>
        /// <param name="settingKey"></param>
        /// <returns></returns>
        public static List<ToolBase> GetToolList(int settingKey)
        {
            if (ToolsDic.ContainsKey(settingKey) == false)
            {
                List<ToolBase> toolNew = new List<ToolBase>();
                ToolsDic.Add(settingKey, toolNew);
                CreateTool(settingKey, new ToolReflection(Assembly.GetExecutingAssembly(), typeof(CreateImage.CreateImageTool)));
            }
            return ToolsDic[settingKey];
        }
        private static T CreateInstance<T>(ToolReflection toolReflection, object[] parameters)
        {
            try
            {
                //toolReflection.Type.FullName, Type事实上就是类的名字及全名（完全路径+名字）
                object ect = toolReflection.Assembly.CreateInstance(toolReflection.Type.FullName, true, System.Reflection.BindingFlags.Default, null, 
                    parameters/*如果该参数为空，创建该实例时就会调用默认的不带参数构造函数*/, null, null);//加载程序集，创建该程序集里面的命名空间.类型名 实例                
                return (T)ect;//类型转换并返回
            }
            catch (Exception ex)
            {
                Util.WriteLog(typeof(ToolsFactory), ex);
                //发生异常，返回类型的默认值
                return default(T);
            }
        }
        public static void CreateTool(int settingKey, ToolReflection toolReflection)
        {
            object[] parameters = new object[1];
            parameters[0] = settingKey;
            ToolBase tool = CreateInstance<ToolBase>(toolReflection, parameters);
            if (tool == null)
            {
                throw new Exception("工具创建失败");  //整个程序中断，也用不着return.
            }

            int index = 0;
            int name = 1;
            //GetToolList(settingKey);  //当ToolsDic.ContainsKey(settingKey)为false时，下面的GetToolList(settingKey)已经生成，本处没有任何意义。
            string nameTmp = "";/* = string.Format("C{0}{1:00}", settingKey, name)*/
            //index = GetToolList(settingKey).FindIndex(x => x.Name == nameTmp);  
            while (index != -1)
            {         
                nameTmp = string.Format("C{0}{1:00}", settingKey, name);
                index = GetToolList(settingKey).FindIndex(x => x.Name == nameTmp); //如果这时GetToolList(settingKey)元素为空，则index = -1；
                name++;
            }
            tool.SetToolName(nameTmp);      //把工具名字赋给相应工具。
            if (tool != null)
            {
                GetToolList(settingKey).Add(tool);
            }
        }
        public static void DeleteToolList(int settingIndex)
        {
            if (ToolsDic.ContainsKey(settingIndex))
            {
                foreach (var item in ToolsDic[settingIndex])
                {
                    item.Dispose();            //先释放内存再删除。
                }
            }
            ToolsDic.Remove(settingIndex);
        }
        public static void DeleteTool(int settingIndex, int index)
        {
            List<ToolBase> selectTools = GetToolList(settingIndex);
            if (index < 0 || index > selectTools.Count - 1)
            {
                return;
            }
            //新添加 手动释放工具对象
            selectTools[index].Dispose();
            selectTools.RemoveAt(index);
        }
        public static List<ToolReflection> AllToolsType
        {
            get
            {
                if (allToolsType == null)
                {
                    allToolsType = new List<ToolReflection>();
                    List<Assembly> lstAssembly = new List<Assembly>();
                    //添加当前目录
                    Assembly assem = Assembly.GetExecutingAssembly();
                    lstAssembly.Add(assem);
                    //添加插件目录
                    if (Directory.Exists(PlugInsDir) == false)//判断是否存在
                    {
                        Directory.CreateDirectory(PlugInsDir);//创建新路径
                    }
                    foreach (var dllFile in Directory.GetFiles(PlugInsDir))   //返回的string[],就如打开文件对话框，允许多选，返回文件名。
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(dllFile);
                            if (!fi.Name.EndsWith(".dll")) continue;
                            if (!fi.Name.Contains("Yoga")) continue;
                            Assembly assemPlugIn = Assembly.LoadFile(fi.FullName);
                            lstAssembly.Add(assemPlugIn);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    foreach (Assembly item in lstAssembly)
                    {
                        //Type[] tt = item.GetTypes();
                        try
                        {
                            foreach (Type tChild in item.GetTypes())
                            {
                                if (tChild.BaseType == (typeof(ToolBase)) && typeof(IToolEnable).IsAssignableFrom(tChild))  //由IToolEnable将CreateImageTool排除掉。
                                {
                                    allToolsType.Add(new ToolReflection(item, tChild));
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ;
                        }                        
                    }
                }
                return allToolsType;
            }
        }

        public static List<string> GetAllMatchingTools(int settingIndex)
        {
            List<string> toolNamelist = new List<string>();
            foreach (var item in ToolsDic[settingIndex])
            {
                if (typeof(IMatching).IsAssignableFrom(item.GetType()))
                {
                    toolNamelist.Add(item.Name);
                }
            }
            return toolNamelist;
        }

        public static ToolBase GetTool(string toolName)
        {
            try
            {
                int settingKey = int.Parse(toolName.Substring(1,1));//C101,先找出在哪个工具组。
                List<ToolBase> toolList = GetToolList(settingKey);

                ToolBase tool = toolList.Find(x=>x.Name== toolName.ToUpper());
                return tool;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

    }
}
