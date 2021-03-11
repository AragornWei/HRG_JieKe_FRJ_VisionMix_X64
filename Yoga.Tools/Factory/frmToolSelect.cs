using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Yoga.Tools
{
    public partial class frmToolSelect : Form
    {
        List<ToolReflection> toolsTypeList = ToolsFactory.AllToolsType;
        int settingIndex;
        public frmToolSelect(int settingIndex)
        {
            InitializeComponent();
            this.settingIndex = settingIndex;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            int index = lsbxTools.SelectedIndex;
            if (index > -1)
            {
                try
                {
                    ToolsFactory.CreateTool(settingIndex, toolsTypeList[index]);
                    this.DialogResult = DialogResult.OK;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("添加工具异常："+ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmToolSelect_Load(object sender, EventArgs e)
        {
            lsbxTools.Items.Clear();
            foreach (var item in toolsTypeList)
            {
                ////= item.GetField("toolName", BindingFlags.Static).GetValue(null).ToString();  //GetField("toolName",BindingFlags.Static)只能访问公有类型字段。
                //string tooltype = item.Type.GetField("toolType", System.Reflection.BindingFlags.Static).GetValue(null).ToString();
                string tooltype =(string) item.Type.GetProperty("ToolType").GetValue(null, null);//.ToString();//返回的是泛型Object，所以转换。以上两种方式皆可。
                lsbxTools.Items.Add(tooltype);
            }
        }

        private void lsbxTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lsbxTools.SelectedIndex;
            if (index > -1)
            {
                txtToolExplanation.Text = toolsTypeList[index].Type.GetProperty("ToolExplanation").GetValue(null, null).ToString();
                //txtToolName.Text=ToolsFactory.getf
            }
        }
    }
}
