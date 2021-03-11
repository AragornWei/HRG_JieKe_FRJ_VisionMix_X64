using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Yoga.Tools.Factory
{   
    public partial class ToolShowUnit : UserControl
    {
        Dictionary<int, TreeNode> ToolTreeNodeDic = new Dictionary<int, TreeNode>();
        Dictionary<int, int> ToolsCameraIndexDic = new Dictionary<int, int>();
        public event EventHandler<ToolGroupsChangeEventArgs> ToolGroupsChangeEvent;
        ToolGroupsChangeEventArgs tgce = new ToolGroupsChangeEventArgs();
        //int cameracount;
        int currentSettingKey;
        bool isRunning;
        //bool isMinistrator;

        public ToolShowUnit(/*int cameracount*/)
        {
            InitializeComponent();
            //this.cameracount = cameracount;
        }

        #region 类的属性
        public int CurrentCameraIndex
        {
            get
            {
                return GetCameraIndex(CurrentSettingKey);
            }
        }
        public int CurrentSettingKey
        {
            get
            {
                if (currentSettingKey < 1)
                {
                    currentSettingKey = 1;
                }
                return currentSettingKey;
            }
            private set
            {
                currentSettingKey = value;
            }
        }

        public bool IsRunning
        {
            set
            {
                isRunning = value;
            }
        }
        #endregion

        private void GetCurrentSettingkey(TreeNode nodeSelect)
        {
            TreeNode nodeSetting = null;
            if (nodeSelect.Parent == null)
            {
                nodeSetting = nodeSelect;
            }
            else
            {
                nodeSetting = nodeSelect.Parent; //因为该树视图只有2级，所以子树的父亲一定是工具组顶端。
            }
            CurrentSettingKey = int.Parse(nodeSetting.Name);
        }

        public int GetCameraIndex(int settingKey)    //获取对应相机号
        {
            if (ToolsCameraIndexDic.ContainsKey(settingKey))
            {
                return ToolsCameraIndexDic[settingKey];
            }
            //List<ToolBase> toolList = ToolsFactory.GetToolList(settingKey);
            //CreateImage.CreateImageTool tool = toolList[0] as CreateImage.CreateImageTool;
            //int cameraIndex = tool.CameraIndex;
            //ToolsCameraIndexDic.Add(settingKey, cameraIndex);
            return -1;
        }

        public void LoginSetting(bool isLogin)
        {
            //this.groupBoxUserSetting.Enabled = isLogin;
            //this.groupBox1.Enabled = isLogin;
            //this.treeView1.Enabled = isLogin;
            //this.btnRun.Enabled = isLogin;
            //this.btnInterlocking.Enabled = isLogin;
            //this.toolShowUnit1.Enabled = isLogin;
            if (isLogin)
            {
                this.treeViewAllTools.ContextMenuStrip = this.contextMenuStrip1;
                //isMinistrator = true;
            }
                
            else
            {
                this.treeViewAllTools.ContextMenuStrip = null;
                //isMinistrator = false;
            }                
        }

        public void CloseOffLineMode()
        {
            if(ToolsFactory.ToolsDic.Count>0)
            {
                foreach(int key in ToolsFactory.ToolsDic.Keys)
                {
                    List<ToolBase> toollist = ToolsFactory.ToolsDic[key];
                    if(toollist.Count>0)
                    {
                        if (toollist[0] is CreateImage.CreateImageTool)
                            ((CreateImage.CreateImageTool)toollist[0]).OffLineMode = false;
                    }
                }
            }
        }

        #region 工具视图初始化
        public void InitTreeView()
        {
            try
            {
                int count = ToolsFactory.ToolsDic.Count;
                //ToolGroupsChangeEventArgs args = new ToolGroupsChangeEventArgs();
                if (ToolGroupsChangeEvent != null)
                {
                    tgce.ToolGroupsNum = 0;
                    ToolGroupsChangeEvent(this, tgce);
                }
                ToolsCameraIndexDic.Clear();
                treeViewAllTools.Nodes.Clear();
                foreach (int settingKey in ToolsFactory.ToolsDic.Keys)
                {
                    CreateImage.CreateImageTool tool = ToolsFactory.ToolsDic[settingKey][0] as CreateImage.CreateImageTool;
                    int cameraIndex = tool.CameraIndex;
                    TreeNode nodeSetting = AddSettingNode(settingKey, cameraIndex);
                    InitToolsByIndex(nodeSetting, settingKey);
                    if (ToolGroupsChangeEvent != null)
                    {
                        tgce.ToolGroupsNum = settingKey;
                        ToolGroupsChangeEvent(this, tgce);
                    }
                    ToolsCameraIndexDic.Add(settingKey, cameraIndex);
                    //if (settingKey == cameracount)
                    //    break;
                }
                CurrentSettingKey = 1;
                treeViewAllTools.ExpandAll();
                if (treeViewAllTools.Nodes.Count > 0)
                {
                    treeViewAllTools.SelectedNode = treeViewAllTools.Nodes[0];
                }
            }
            catch(Exception)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "工具视图初始化失败！");
            }            
        }

        private TreeNode AddSettingNode(int settingKey,int cameraIndex)
        {
            TreeNode nodeSetting = new TreeNode();
            nodeSetting.Name = settingKey.ToString();  //settingKey是由ToolsDic中的Key来确定的。
            nodeSetting.Text = string.Format("工具组{0}:[{1}#相机]", settingKey, cameraIndex);
            nodeSetting.SelectedImageIndex = 0;
            treeViewAllTools.Nodes.Add(nodeSetting);

            if (ToolTreeNodeDic.ContainsKey(settingKey))
            {
                ToolTreeNodeDic[settingKey] = nodeSetting;
            }
            else
            {
                ToolTreeNodeDic.Add(settingKey, nodeSetting);
            }
            return nodeSetting;
        }

        private void InitToolsByIndex(TreeNode nodeSetting, int settingKey)
        {
            nodeSetting.Nodes.Clear();
            int index = 0;
            CreateImage.CreateImageTool tool = ToolsFactory.GetToolList(settingKey)[0] as CreateImage.CreateImageTool;
            string isOffLineStr = tool.OffLineMode ? "--离线" : "";
            foreach (ToolBase item in ToolsFactory.GetToolList(settingKey))
            {
                TreeNode nodeTool = new TreeNode();
                nodeTool.Name = index.ToString();    //有了这个Name,可以根据name的int值来确定该节点对应的工具。
                nodeTool.Text = string.Format("{0} {1}", item.Name, item.Note+ isOffLineStr);
                nodeTool.SelectedImageIndex = 1;       //处于选定状态的图标
                nodeTool.ForeColor = tool.OffLineMode ? Color.Blue : Color.Black;
                nodeSetting.Nodes.Add(nodeTool);
                index++;
            }
            InitStatus(settingKey);
        }
        #endregion

        #region 工具测试结果更新显示
        public void InitStatus(int settingKey)
        {
            if (ToolTreeNodeDic.ContainsKey(settingKey) == false)
            {
                Common.Util.Notify(string.Format("工具组{0}显示未初始化!!", settingKey));
                return;
            }
            foreach (TreeNode item in ToolTreeNodeDic[settingKey].Nodes)
            {
                item.ImageIndex = 1;    //这是处于未选定状态时的图标。
            }
        }
        public void ShowStatus(RunStatus runStatus)
        {
            textBox2.Text = runStatus.ResultMessage;
            int index = 0;
            treeViewAllTools.BeginUpdate();
            treeViewAllTools.SelectedNode = null;

            foreach (TreeNode item in ToolTreeNodeDic[runStatus.SettingIndex].Nodes)
            {
                int indexImage = 0;
                if (runStatus.RunStatusList[index] == true)
                {
                    indexImage = 2;
                    item.BackColor = Color.Empty;
                }
                else
                {
                    indexImage = 3;
                    item.BackColor = Color.Red;
                }
                item.ImageIndex = indexImage;
                index++;
            }
            treeViewAllTools.EndUpdate();
            //textBox2.Refresh();
            //treeViewAllTools.Refresh();
        }
        public void ShowStatus(int settingKey)
        {
            if (ToolTreeNodeDic.ContainsKey(settingKey) == false)
            {
                Common.Util.Notify(string.Format("工具组{0}显示未初始化!!", settingKey));
                return;
            }
            int index = 0;
            treeViewAllTools.SelectedNode = null;
            List<ToolBase> tools = ToolsFactory.GetToolList(settingKey);
            foreach (TreeNode item in ToolTreeNodeDic[settingKey].Nodes)
            {
                int indexImage = 0;
                if (tools[index].IsOk||(tools[index] is IToolRun ==false))
                {
                    indexImage = 2;
                }
                else
                {
                    indexImage = 3;
                }
                item.ImageIndex = indexImage;
                index++;
            }
        }
        #endregion    

        #region 鼠标右键菜单操作
        private void 添加工具组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "运行中禁止操作");
                return;
            }

            //if (treeViewAllTools.Nodes.Count >= cameracount)
            //{
            //    MessageBox.Show("超过相机窗口数，不能添加工具组", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            int settingKey = ToolsFactory.ToolsDic.Keys.Max() + 1;  //工具组名称自增。

            int cameraWant = settingKey;
            //if (cameraWant > Camera.CameraManger.CameraDic.Count)
            //{
            //    cameraWant = Camera.CameraManger.CameraDic.Count;
            //}
            for(int i=0; i<settingKey; i++)
            {
                if(ToolsFactory.ToolsDic.ContainsKey(i+1)==false)
                {
                    cameraWant = i + 1;
                    break;
                }
            }

            //frmSelectCameraIndex frmSelect = new frmSelectCameraIndex(cameraWant);
            //frmSelect.ShowDialog();
            //if (frmSelect.DialogResult != DialogResult.OK)
            //{
            //    return;
            //}
            //List<ToolBase> toolsNew = ToolsFactory.GetToolList(cameraWant);
            //CreateImage.CreateImageTool toolCamera = toolsNew[0] as CreateImage.CreateImageTool;
            //toolCamera.CameraIndex = cameraWant;//这个实际上不需要，因为在动态生成时，已经传过去。//frmSelect.CameraIndex;
            //TreeNode nodeSetting = AddSettingNode(cameraWant);
            //InitToolsByIndex(nodeSetting, cameraWant);
            //nodeSetting.ExpandAll();
            ToolsFactory.GetToolList(cameraWant);
            InitTreeView();
        }

        private void 添加工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "运行中禁止操作");
                return;
            }
            //首先判断是否选定组件中节点的位置
            if (treeViewAllTools.SelectedNode == null)
            {
                MessageBox.Show("请先选择工具节点", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            TreeNode nodeSetting = null;
            if (treeViewAllTools.SelectedNode.Parent == null)
            {
                nodeSetting = treeViewAllTools.SelectedNode;
            }
            else
            {
                nodeSetting = treeViewAllTools.SelectedNode.Parent;
            }
            int settingKey = int.Parse(nodeSetting.Name);
            frmToolSelect select = new frmToolSelect(settingKey);
            DialogResult result = select.ShowDialog();
            if (result == DialogResult.OK)
            {
                InitToolsByIndex(nodeSetting, settingKey);
                nodeSetting.ExpandAll();
            }
        }

        private void 删除工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "运行中禁止操作");
                return;
            }

            if (treeViewAllTools.SelectedNode == null || treeViewAllTools.SelectedNode.Parent == null)
            {
                MessageBox.Show("请先选择工具节点", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            TreeNode nodeSetting = treeViewAllTools.SelectedNode.Parent;
            TreeNode nodeTool = treeViewAllTools.SelectedNode;
            int settingKey = int.Parse(nodeSetting.Name);

            int index = int.Parse(nodeTool.Name);
            if (index == 0)
            {
                MessageBox.Show("工具1为图像采集工具,无法删除", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ToolBase tool = ToolsFactory.GetToolList(settingKey)[index];
            string message = "";
            if (tool is IToolRun)
            {
                message = string.Format("是否删除工具{0}?", tool.Name);
            }
            else
            {
                message = string.Format("{0}为设置类工具,若删除需要保存后重新打开!",tool.Name);
            }
            DialogResult result = MessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result != DialogResult.Yes)
            {
                return;
            }
            ToolsFactory.DeleteTool(settingKey, index);
            List<ToolBase> toollist = ToolsFactory.GetToolList(settingKey);
            int name = 1;
            string nametemp;
            foreach(ToolBase item in toollist)
            {
                nametemp = string.Format("C{0}{1:00}", settingKey, name);
                item.SetToolName(nametemp);
                name++;
            }
            //InitTreeView();  //只是改变某个工具组中的工具，不用整个都刷新。
            InitToolsByIndex(nodeSetting, settingKey);  //该函数执行中，相应每个工具节点Name再重新赋值。
        }

        private void 删除工具组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "运行中禁止操作");
                return;
            }

            if (treeViewAllTools.SelectedNode == null)
            {
                MessageBox.Show("请先选择工具节点", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeNode nodeSetting = null;
            if (treeViewAllTools.SelectedNode.Parent == null)
            {
                nodeSetting = treeViewAllTools.SelectedNode;
            }
            else
            {
                nodeSetting = treeViewAllTools.SelectedNode.Parent;
            }
            int settingKey = int.Parse(nodeSetting.Name);

            if (settingKey == 1)
            {
                MessageBox.Show("工具组1为默认工具组无法删除", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult result = MessageBox.Show("是否删除当前工具组合? ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result != DialogResult.Yes)
            {
                return;
            }
            ToolsFactory.DeleteToolList(settingKey);
            treeViewAllTools.Nodes.Remove(nodeSetting);
            if (ToolsCameraIndexDic.ContainsKey(settingKey))
            {
                ToolsCameraIndexDic.Remove(settingKey);
            }
            InitTreeView();
        }
        #endregion

        #region 鼠标左键操作响应
        //单击
        private void treeViewAllTools_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (isRunning)
            {               
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                GetCurrentSettingkey(e.Node);                
                if (ToolGroupsChangeEvent != null)
                {
                    tgce.ToolGroupsNum = currentSettingKey;
                    ToolGroupsChangeEvent(this, tgce);
                }
                                
                TreeNode nodeTool = e.Node;
                textBox2.Text = "";
                if (nodeTool.Parent == null)
                {
                    textBox2.Text = string.Format("当前工具组:{0}", nodeTool.Text);
                    return;
                }

                TreeNode nodeSetting = nodeTool.Parent;

                int settingKey = int.Parse(nodeSetting.Name);

                int index = int.Parse(nodeTool.Name);

                ToolBase tool = ToolsFactory.GetToolList(settingKey)[index];
                treeViewAllTools.PathSeparator = "/";
                string str1 = "";

                if (tool != null)
                {
                    str1 += "当前工具:" + nodeTool.FullPath;
                    str1 += Environment.NewLine + "运行结果:" + (tool.Result == null || tool.Result == "" ? "空" : tool.Result);
                    str1 += Environment.NewLine + "运行状态:" + ((tool.IsOk == true||(tool is IToolRun==false)) ? "OK" : "NG");
                    str1 += Environment.NewLine + "耗时(ms):" + tool.ExecutionTime.ToString("f2");
                }
                textBox2.Text = str1;
            }
        }
        //双击开启参数设定单元
        private void treeViewAllTools_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            if (isRunning)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "运行中禁止操作");
                return;
            }
            if (treeViewAllTools.ContextMenuStrip == null)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "非管理员用户禁止操作");
                return;
            }            

            TreeNode selectNode = e.Node;
            if (selectNode == null || selectNode.Parent == null)
            {
                //MessageBox.Show("请先选择工具节点", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeNode nodeParent = selectNode.Parent;
            //TreeNode nodeTool = selectNode;
            int settingKey = int.Parse(nodeParent.Name);
            int index = int.Parse(selectNode.Name);

            ToolBase tool = ToolsFactory.GetToolList(settingKey)[index];
            if (tool != null)
            {
                tool.ClearTestData();
                ToolsSettingUnit settingUnit = tool.GetSettingUnit() as ToolsSettingUnit;
                if (settingUnit != null)
                {
                    frmToolSetting toolSetting = new frmToolSetting(settingUnit);
                    toolSetting.StartPosition = FormStartPosition.CenterScreen;
                    toolSetting.MaximizeBox = false;
                    toolSetting.ShowDialog();
                    selectNode.Text = string.Format("{0} {1}", tool.Name, tool.Note);
                }
            }
            //if(tool is CreateImage.CreateImageTool)
            //{
            //    InitTreeView();
            //}
            InitTreeView();
            if (ToolGroupsChangeEvent != null)
            {
                tgce.ToolGroupsNum = settingKey;
                ToolGroupsChangeEvent(this, tgce);
            }
        }

        #endregion
    }
}
