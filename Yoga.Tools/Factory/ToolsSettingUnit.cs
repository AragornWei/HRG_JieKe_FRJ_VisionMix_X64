using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System;

namespace Yoga.Tools
{
    [ToolboxItem(false)]
    public  partial class ToolsSettingUnit : UserControl
    {
        ToolBase tool;
        protected bool locked = false;
        //供工具设置窗口退出时检查是否可序列化
        public ToolBase Tool
        {
            get
            {
                return tool;
            }
        }

        public ToolsSettingUnit()
        {
            InitializeComponent();
        }

        public void HideMax()
        {
            txtCommMax.Visible = false;
            labelComm2.Visible = false;
        }

        public void HideMin()
        {
            txtCommMin.Visible = false;
            labelComm1.Visible = false;
        }
        //工具名称显示
        public void Init(string name, ToolBase tool)
        {
            lblCommToolsName.Text = name+":";
            this.tool = tool;
            txtCommMin.Text = tool.Min.ToString();
            txtCommMax.Text = tool.Max.ToString();
            //工具名称
            txtCommNote.Text = tool.Note;
            //是否输出结果
            chkIsOutputResults.Checked = tool.IsOutputResults;
            //图像工具不显示图像源
            if (tool is CreateImage.CreateImageTool)
            {
                cmbImageSoure.Visible = false;
                labelCommImageSoure.Visible = false;
                txtCommNote.Enabled = false;
            }
            else
            {
                //显示图像源
                List<ToolBase> tools = ToolsFactory.GetToolList(tool.SettingIndex);
                cmbImageSoure.Items.Clear();
                foreach (ToolBase item in tools)
                {
                    if (item==this.tool)
                    {
                        break;
                    }
                    if (item is ICreateImage)
                    {
                        cmbImageSoure.Items.Add(item.Name);
                    }
                }
                cmbImageSoure.Text = tool.ImageSoureToolName;
            }
            if (tool.IsSubTool)
            {
                groupBoxCommSetting.Visible = false;
            }
            txtCommNote.ReadOnly = true;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //窗口显示时调用
        public virtual void ShowTranResult()
        {
            
        }
        //图像工具源变化时调用
        public virtual void ShowResult()
        {

        }
        public virtual void Clear()
        {
            if (tool==null)
            {
                return;
            }
            //释放测试图片、输入图片、参数设定窗口、仿射委托、运行结果
            tool.ClearTestData();
            //将用户控件中的控件显示控件释放
            foreach (var item in this.Controls)
            {
                if (item is ImageControl.HWndUnit)
                {
                    ((ImageControl.HWndUnit)item).Dispose();
                }
                
            }
            //清楚运行结果标志
            tool.ClearResult();
            //foreach (var item in ToolsFactory.ToolsDic[tool.SettingIndex])
            //{
            //    item.ClearResult();
            //}
        }

        private void txtCommMin_TextChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            double min;
            if (double.TryParse(txtCommMin.Text, out min) == false)
            {
                return;
            }
            tool.Min = min;
        }

        private void txtCommMax_TextChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            double max;
            if (double.TryParse(txtCommMax.Text, out max) == false)
            {
                return;
            }
            tool.Max = max;
        }

        private void chkIsOutputResults_CheckedChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.IsOutputResults = chkIsOutputResults.Checked;
        }

        private void txtCommNote_TextChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.Note = txtCommNote.Text;
        }

        private void cmbImageSoure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (locked)
            {
                return;
            }
            tool.ImageSoureToolName = cmbImageSoure.Text;
            //ShowResult();
        }
      
    }
}
