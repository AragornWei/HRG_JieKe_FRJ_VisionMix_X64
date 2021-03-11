using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yoga.Common.Basic;
using Yoga.Tools;

namespace Yoga.VisionMix.Frame
{
    public partial class frmProjectManger : Form
    {
        string path = Environment.CurrentDirectory + "\\project";
        public frmProjectManger()
        {
            InitializeComponent();
        }

        private void btnAddNewProject_Click(object sender, EventArgs e)
        {
            frmImputText frm = new frmImputText("项目名称");
            DialogResult result = frm.ShowDialog();
            if (result != DialogResult.OK || frm.ImputText.Trim()==string.Empty)
            {
                return;
            }
            if (lsbAllProject.Items.Contains(frm.ImputText.Trim()))
            {
                MessageBox.Show("项目重名，不允许新建");
                return;
            }
            lsbAllProject.Items.Add(frm.ImputText.Trim());
            txtCurrentProject.Text = frm.ImputText.Trim();
            #region//清楚上次工具
            SortedDictionary<int, List<ToolBase>> toolsDic = ToolsFactory.ToolsDic;
            List<int> keys = new List<int>();
            foreach (int index in toolsDic.Keys)
            {
                keys.Add(index);
            }
            foreach (int i in keys)
            {
                ToolsFactory.DeleteToolList(i);
            }
            #endregion

            ToolsFactory.GetToolList(1);
            if (AppManger.ProjectData.Instance.CameraPramDic != null)
            {
                AppManger.ProjectData.Instance.CameraPramDic.Clear();
                AppManger.ProjectData.Instance.CameraPramDic = null;
            }
            
            UserSetting.Instance.ProjectPath = Environment.CurrentDirectory + "\\project\\" + frm.ImputText.Trim()+ ".prj";
            AppManger.ProjectData.Instance.SaveProject(UserSetting.Instance.ProjectPath);
            MessageBox.Show("新建项目成功","新建项目", MessageBoxButtons.OK);
            this.Close();
        }

        private void frmProjectManger_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(path))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(path); //新建文件夹   
            }
            List<string> picPathList = new List<string>();
            lsbAllProject.Items.Clear();
            //获取指定文件夹的所有文件  
            string[] paths = Directory.GetFiles(path);
            foreach (var item in paths)
            {
                //获取文件后缀名  
                string extension = Path.GetExtension(item).ToLower();
                if (extension == ".prj")
                {
                    string prj = Path.GetFileNameWithoutExtension(item);
                    lsbAllProject.Items.Add(prj);
                }
            }
            txtCurrentProject.Text = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);

        }

        private void btnSetCurrentProject_Click(object sender, EventArgs e)
        {
            string name = lsbAllProject.SelectedItem.ToString();
            if (txtCurrentProject.Text== name)
            {
                return;
            }
            UserSetting.Instance.ProjectPath = Environment.CurrentDirectory + "\\project\\"+name+ ".prj";
            txtCurrentProject.Text = name;
        }
    }
}
