//#define NativeCode

using System;
using HalconDotNet;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Yoga.ImageControl
{
    /// <summary>
    ///halcon窗体控制类
    /// </summary>
    public class HWndCtrl
    {
        #region 类的字段
        private IntPtr winHandle;
        /// <summary> 
        /// 最大显示图形个数
        /// </summary>
        private const int MAX_NUM_OBJ_LIST = 50;

        private bool mousePressed = false;

        private bool isShowCross = false;
        private bool isShowMessage = true;
        private double startX, startY;
        private bool inMeasureLine;
        public HWindowControl viewPort;

        /// <summary> 
        /// Halcon图像窗口的显示对象集合,
        /// 超过了最大显示个数则起始位置图像会被移除显示列表
        /// </summary>
        private List<HObjectEntry> HObjList;

        /// <summary>
        /// Instance that describes the graphical context for the
        /// HALCON window. According on the graphical settings
        /// attached to each HALCON object, this graphical context list 
        /// is updated constantly.
        /// </summary>
        private GraphicsContext mGC;

        private ContextMenuStrip        hv_MenuStrip;                                    //右键菜单控件
        // 窗体控件右键菜单内容
        ToolStripMenuItem fit_strip;
        ToolStripMenuItem MoveWindowEnable_strip;

        ToolStripMenuItem fit_showImageOnly;
        ToolStripMenuItem saveImg_strip;
        ToolStripMenuItem saveWindow_strip;

        ToolStripMenuItem ShowHistogram_strip;
        ToolStripMenuItem showCross_shrip;

        ToolStripMenuItem showMessage_strip;
        ToolStripMenuItem measureLine_strip;

        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private ROIController roiManager;
        private string backguoundColor = "black";
        /// <summary>
        /// 显示模式
        /// </summary>
        private ShowMode showMode;

        private ResultShow resultShow = ResultShow.处理后;
        /* Basic parameters, like dimension of window and displayed image part */
        private int windowWidth;
        private int windowHeight;
        private int imageWidth = 0;
        private int imageHeight = 0;
        private double zoomWndFactor;
        private double currentTextSize = 0; 
        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        // 设定图像的窗口显示部分
        private int zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol;
        // 获取图像的当前显示部分                   
        private int current_beginRow, current_beginCol, current_endRow, current_endCol;
        //产生MouseDoubleClick事件。
        private Timer timeMouseDoubleClick;
        private bool isMouseFirstClick;
        // 禁止/允许窗口图片移动
        bool isMoveEnable=false;

        #endregion

        #region 类的属性
        public int WindowWidth
        {
            get
            {
                return windowWidth;
            }

        }

        public int WindowHeight
        {
            get
            {
                return windowHeight;
            }
        }

        public ResultShow ResultShow
        {
            get
            {
                return resultShow;
            }

            set
            {
                resultShow = value;
                Repaint();
            }
        }

        public int ImageWidth
        {
            get
            {
                return imageWidth;
            }
        }

        public int ImageHeight
        {
            get
            {
                return imageHeight;
            }
        }

        public double ZoomWndFactor
        {
            get
            {
                return zoomWndFactor;
            }

            private set
            {
                zoomWndFactor = value;
            }
        }

        #endregion

        public event MouseEventHandler MouseDoubleClick;
        public event EventHandler<ShowMessageEventArgs> ShowMessageEvent;
        private void TriggerShowMessageEvent(ShowMessageEventArgs e)
        {
            if (ShowMessageEvent != null)
            {
                ShowMessageEvent(this, e);
            }
        }
        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        #region 构造函数
        public HWndCtrl(HWindowControl view)
        {
            viewPort = view;
            //winHandle = viewPort.HalconWindow;
            view.HalconWindow.SetDraw("margin");
            view.HalconWindow.SetColor("blue");
            view.HalconWindow.SetLineWidth(1);
            //view.HalconWindow.SetWindowParam("flush", "true");

            timeMouseDoubleClick = new Timer();
            timeMouseDoubleClick.Interval = 300;
            timeMouseDoubleClick.Tick += TimerMouseDoubleClick_Tick;

            windowWidth = viewPort.Size.Width;
            windowHeight = viewPort.Size.Height;

            ZoomWndFactor = (double)imageWidth / viewPort.Width;
            showMode = ShowMode.IncludeROI;

            viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
            viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);
            //新添加滚轮事件
            viewPort.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.mouseWheel);
            
            // graphical stack 
            HObjList = new List<HObjectEntry>();
            mGC = new GraphicsContext();

            fit_strip = new ToolStripMenuItem("适应窗口");
            fit_strip.Click += new EventHandler((s, e) => DispImageFit());   //没有带（s,e）格式参数的函数，事件调用需要Lambd表达式形式。

            MoveWindowEnable_strip = new ToolStripMenuItem("允许移动窗口");
            MoveWindowEnable_strip.Click += new EventHandler((s, e) =>
            {
                isMoveEnable = !isMoveEnable;
            });
            MoveWindowEnable_strip.CheckOnClick = true;

            fit_showImageOnly = new ToolStripMenuItem("显示原图/所有");
            fit_showImageOnly.Click += new EventHandler((s, e) => ShowImageOnly());
            fit_showImageOnly.CheckOnClick = true;

            saveImg_strip = new ToolStripMenuItem("保存原始图像");
            saveImg_strip.Click += new EventHandler((s, e) => SaveImage());

            saveWindow_strip = new ToolStripMenuItem("截图另存");
            saveWindow_strip.Click += new EventHandler((s, e) => SaveWindowDump());

            showCross_shrip = new ToolStripMenuItem("显示/隐藏十字");
            showCross_shrip.Click += new EventHandler((s, e) => ShowCross());
            showCross_shrip.CheckOnClick = true;

            showMessage_strip = new ToolStripMenuItem("显示/隐藏文字");
            showMessage_strip.Click += new EventHandler((s, e) => ShowMessage());
            showMessage_strip.CheckOnClick = true;

            ShowHistogram_strip = new ToolStripMenuItem("灰度直方图");
            ShowHistogram_strip.Click += ShowHistogram_strip_Click;

            measureLine_strip = new ToolStripMenuItem("距离测量");
            measureLine_strip.Click += MeasureLine_strip_Click;

            hv_MenuStrip = new ContextMenuStrip();
            hv_MenuStrip.Items.Add(fit_strip);
            hv_MenuStrip.Items.Add(MoveWindowEnable_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());
            hv_MenuStrip.Items.Add(fit_showImageOnly);
            hv_MenuStrip.Items.Add(showCross_shrip);
            hv_MenuStrip.Items.Add(showMessage_strip);
            hv_MenuStrip.Items.Add(measureLine_strip);
            hv_MenuStrip.Items.Add(ShowHistogram_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());
            hv_MenuStrip.Items.Add(saveImg_strip);
            hv_MenuStrip.Items.Add(saveWindow_strip);

            viewPort.ContextMenuStrip = hv_MenuStrip;
            //m_CtrlHStatusLabelCtrl.BringToFront();
            //viewPort.ResumeLayout(false);
            //viewPort.PerformLayout();
            //HOperatorSet.SetSystem("filename_encoding", "utf8");
        }
        #endregion

        private void TimerMouseDoubleClick_Tick(object sender,EventArgs e)
        {
            timeMouseDoubleClick.Stop();
            isMouseFirstClick = false;
        }

        #region 右键菜单项响应函数
        private void ShowHistogram_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();

            HWndMessage message = new HWndMessage("鼠标左键点击并拉取矩形区域,鼠标右键完成", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);

            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            //HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);
            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();

            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawRectangle1(out r1, out c1, out r2, out c2);
            window.DispRectangle1(r1, c1, r2, c2);
            Form frm = new Form();

            FunctionPlotUnit pointUnit = new FunctionPlotUnit();
            Size size = pointUnit.Size;
            size.Height = (int)(size.Height + 50);
            size.Width = (int)(size.Width + 50);
            frm.Size = size;
            frm.Controls.Add(pointUnit);
            pointUnit.Dock = DockStyle.Fill;
            HTuple grayVals;

            grayVals = GetGrayHisto(new HTuple(r1, c1, r2, c2));

            pointUnit.SetAxisAdaption(FunctionPlot.AXIS_RANGE_INCREASING);
            pointUnit.SetLabel("灰度值", "频率");
            pointUnit.SetFunctionPlotValue(grayVals);
            pointUnit.ComputeStatistics(grayVals);

            frm.ShowDialog();

            //window.DrawLine(out r1, out c1, out r2, out c2);
            //window.DispLine(r1, c1, r2, c2);

            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            //HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            //double dr = Math.Abs(r2 - r1);
            //double dc = Math.Abs(c2 - c1);
            //MessageBox.Show(string.Format("直线距离{0:f2}px\rx轴距离{1:f2}px\ry轴距离{2:f2}px", dd.D, dc, dr), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }

        HTuple GetGrayHisto(HTuple rectangle1)
        {
            if (HObjList == null || HObjList.Count < 1)
            {
                return null;
            }
            HImage hv_image = HObjList[0].HObj as HImage;

            if (hv_image != null)
            {
                try
                {
                    HTuple hv_AbsoluteHisto, hv_RelativeHisto;

                    HTuple channel = hv_image.CountChannels();
                    HImage imgTmp = null;
                    if (channel == 3)
                    {
                        imgTmp = hv_image.Rgb1ToGray();
                    }
                    else
                    {
                        imgTmp = hv_image.Clone();
                    }
                    HRegion region = new HRegion();
                    region.GenRectangle1(rectangle1[0].D, rectangle1[1], rectangle1[2], rectangle1[3]);
                    hv_AbsoluteHisto = imgTmp.GrayHisto(region, out hv_RelativeHisto);
                    imgTmp.Dispose();
                    return hv_AbsoluteHisto;
                }
                catch (Exception)
                {

                }
            }
            return null;
        }

        private void ShowImageOnly()
        {
            if (ResultShow == ResultShow.原图)
            {
                ResultShow = ResultShow.处理后;
            }
            else
            {
                ResultShow = ResultShow.原图;
            }
        }

        public HImage DumpWindows()
        {
            return viewPort.HalconWindow.DumpWindowImage();
        }

        public void DrawPoint(out double x, out double y)
        {
            viewPort.Focus();

            HWndMessage message = new HWndMessage("鼠标左键点击点位置,鼠标右键完成", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);

            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawPoint(out y, out x);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }
        private void MeasureLine_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();

            HWndMessage message = new HWndMessage("鼠标点击两个位置后,单击鼠标右键完成。", 20, 20, 20, "green");
            message.DispMessage(viewPort.HalconWindow, "window", 1);

            inMeasureLine = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(1);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawLine(out r1, out c1, out r2, out c2);
            window.DispLine(r1, c1, r2, c2);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            double dr = Math.Abs(r2 - r1);
            double dc = Math.Abs(c2 - c1);
            MessageBox.Show(string.Format("直线距离{0:f2}px\rx轴距离{1:f2}px\ry轴距离{2:f2}px", dd.D, dc, dr), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            inMeasureLine = false;
            Repaint();
        }

        private void ShowMessage()
        {
            isShowMessage = !isShowMessage;
            Repaint();
        }

        private void ShowCross()
        {
            isShowCross = !isShowCross;
            Repaint();
        }

        private void DispImageFit()        //让图片适应窗口大小。
        {
            ResetWindow();
            Repaint();
        }

        private void SaveWindowDump()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG图像|*.png|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                    return;
                //截取窗口图
                HOperatorSet.DumpWindow(viewPort.HalconWindow, "png best", sfd.FileName);
            }
        }

        private void SaveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(sfd.FileName))
                {
                    return;
                }

                if (HObjList == null || HObjList.Count < 1)
                {
                    return;
                }
                HImage hv_image = HObjList[0].HObj as HImage;
                if (hv_image != null && hv_image.IsInitialized())
                {
                    HOperatorSet.WriteImage(hv_image, "bmp", 0, sfd.FileName);
                }
            }
        }

        #endregion

        /// <summary>
        /// 注册窗体的ROI控制类
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useROIController(ROIController rC)   //在该窗口如果要操作使用ROI，必须调用该函数。
        {
            roiManager = rC;
            rC.setViewController(this);
        }

        /// <summary>
        /// 设置是否显示roi
        /// </summary>
        /// <param name="mode"></param>
        public void SetDispLevel(ShowMode mode)  //在该窗口是否显示ROI.
        {
            showMode = mode;
        }
        void DispImageZoom(double mode, double Mouse_row, double Mouse_col)
        {
            try
            {
                viewPort.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);
            }
            catch
            {
                return;
            }

            if (mode > 0)                 // 放大图像:窗口大小不变，显示指定的部分图像：显示的图像只是图像的一部分。所以坐标值都是正值。
            {
                zoom_beginRow = (int)(current_beginRow + (Mouse_row - current_beginRow) * 0.300d); //起始点要增加
                zoom_beginCol = (int)(current_beginCol + (Mouse_col - current_beginCol) * 0.300d);
                zoom_endRow = (int)(current_endRow - (current_endRow - Mouse_row) * 0.300d);       //结束点要减小
                zoom_endCol = (int)(current_endCol - (current_endCol - Mouse_col) * 0.300d);   
            }
            else                // 缩小图像：窗口大小不变，显示全部图像及图像外围部分。所以坐标值起始部分是负值，结束部分要大于图像的最大坐标。
            {
                zoom_beginRow = (int)(Mouse_row - (Mouse_row - current_beginRow) / 0.700d);  //起始点要减小
                zoom_beginCol = (int)(Mouse_col - (Mouse_col - current_beginCol) / 0.700d);
                zoom_endRow = (int)(Mouse_row + (current_endRow - Mouse_row) / 0.700d);      //结束点要增加。
                zoom_endCol = (int)(Mouse_col + (current_endCol - Mouse_col) / 0.700d);
            }

            try
            {
                int hw_width, hw_height;
                hw_width = viewPort.WindowSize.Width;
                hw_height = viewPort.WindowSize.Height;

                bool _isOutOfArea = true;
                bool _isOutOfSize = true;
                bool _isOutOfPixel = true;  //避免像素过大

                _isOutOfArea = zoom_beginRow >= imageHeight || zoom_endRow <= 0 || zoom_beginCol >= imageWidth || zoom_endCol < 0;//起始点大于右下角，终止点小于左上角
                _isOutOfSize = (zoom_endRow - zoom_beginRow) > imageHeight * 20 || (zoom_endCol - zoom_beginCol) > imageWidth * 20;//发大倍数超图像大小20倍
                _isOutOfPixel = hw_height / (zoom_endRow - zoom_beginRow) > 500 || hw_width / (zoom_endCol - zoom_beginCol) > 500;//缩小超1/500

                if (_isOutOfArea || _isOutOfSize || _isOutOfPixel)
                {
                    return;
                }

                //viewPort.HalconWindow.ClearWindow();
                viewPort.HalconWindow.SetPaint(new HTuple("default"));

                //保持图像显示比例:根据窗口的长宽比例，显示也要做相应调整。
                if (hw_width / hw_height > 1) //因为宽>高。所以要锁定高值（zoom_endRow-zoom_beginRow），再等比例增加宽值=高值* hw_width / hw_height
                    viewPort.HalconWindow.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_beginCol + (zoom_endRow - zoom_beginRow) * hw_width / hw_height);
                else                  //因为高>宽。所以要锁定宽值（zoom_endCol-zoom_beginCol），再等比例增加高值=宽值* hw_height / hw_width
                    viewPort.HalconWindow.SetPart(zoom_beginRow, zoom_beginCol, zoom_beginRow + (zoom_endCol - zoom_beginCol) * hw_height / hw_width, zoom_endCol);

                int w = (zoom_endRow - zoom_beginRow) * hw_width / hw_height;
                int w0 = current_endCol - current_beginCol;
                double scale = (double)w / w0;
                ZoomWndFactor *= scale;
                Repaint();
            }
            catch         //ex.Message;
            {
                //DispImageFit();
            }
        }
       
        private void moveImage(double motionX, double motionY)
        {
            viewPort.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);

            ImgRow1 = current_beginRow - (int)motionY;
            ImgRow2 = current_endRow - (int)motionY;

            ImgCol1 = current_beginCol - (int)motionX;
            ImgCol2 = current_endCol - (int)motionX;

            viewPort.HalconWindow.SetPart((int)ImgRow1, (int)ImgCol1,
                (int)ImgRow2, (int)ImgCol2);
            Repaint();
        }

        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void ResetAll()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            ZoomWndFactor = (double)imageWidth / viewPort.Width;
            Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = imageWidth;
            rect.Height = imageHeight;
            viewPort.ImagePart = rect;

            if (roiManager != null)
                roiManager.Reset();
        }
        /// <summary>
        /// 窗口图像显示区域重置,不刷新图像
        /// </summary>
		public void ResetWindow()
        {
            if (imageHeight == 0)
            {
                return;
            }
            //判断行缩放还是列缩放
            double scaleC = (double)imageWidth / viewPort.Width;
            double scaleR = (double)imageHeight / viewPort.Height;

            double w, h;
            if (scaleC < scaleR)
            {
                h = viewPort.Height * scaleR;
                w = viewPort.Width * scaleR;
                ImgRow1 = 0;
                ImgCol1 = (imageWidth - w) / 2.0;  //为了窗口横向正好显示在中间，左右两边各余留距离：(imageWidth - w) / 2.0
            }
            else
            {
                h = viewPort.Height * scaleC;
                w = viewPort.Width * scaleC;

                ImgRow1 = (imageHeight - h) / 2.0; //为了窗口纵向正好显示在中间，上下两边各余留距离：(imageWidth - w) / 2.0
                ImgCol1 = 0;
            }

            ImgRow2 = ImgRow1 + h - 1;
            ImgCol2 = ImgCol1 + w - 1;

            ZoomWndFactor = w / viewPort.Width;
            viewPort.HalconWindow.SetPart((int)ImgRow1, (int)ImgCol1, (int)ImgRow2, (int)ImgCol2);
        }


        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            if(isMouseFirstClick)
            {
                if (MouseDoubleClick != null)
                     MouseDoubleClick(this, new MouseEventArgs(e.Button, e.Clicks, (int)e.X, (int)e.Y, e.Delta));
            }
            if(isMouseFirstClick==false)
            {
                timeMouseDoubleClick.Start();  //200毫秒定时器
                isMouseFirstClick = true;
            }

            mousePressed = true;
            int state;
            double x, y;
            try
            {
                viewPort.HalconWindow.GetMpositionSubPix(out y, out x, out state);
            }
            catch (HalconException)
            {
                return;
            }
            int activeROIidx = -1;

            if (roiManager != null && (showMode == ShowMode.IncludeROI))
            {
                activeROIidx = roiManager.MouseDownAction(x, y);  //由ROIController处理鼠标按下的事件。
            }

            if (activeROIidx == -1)  //表示鼠标按下没有激活ROI.
            {
                startX = x;         //表示鼠标按下的当前X坐标，由其它鼠标响应事件来利用。
                startY = y;
            }         
        }

        private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            mousePressed = false;

            if (roiManager != null
                && (roiManager.ActiveRoiIdx != -1)
                && (showMode == ShowMode.IncludeROI))
            {
                roiManager.TiggerROINotifyEvent(new ViewEventArgs(ViewMessage.UpdateROI));
            }
        }

        private void mouseWheel(object sender, HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            double Row, Column;
            int Button;
            try
            {
                viewPort.HalconWindow.GetMpositionSubPix(out Row, out Column, out Button);
            }
            catch (HalconException)
            {
                return;
            }

            double mode = 1;
            if (e.Delta > 0)
            {
                mode = 1;
            }
            else
            {
                mode = -1;
            }
            DispImageZoom(mode, Row, Column);
        }

        private void mouseMoved(object sender, HMouseEventArgs e)
        {
            if (inMeasureLine)
            {
                return;
            }
            
            double motionX, motionY;
            double currX, currY;
            //HTuple currX1 = 0, currY1 = 0;
            try
            {
                if (HObjList.Count < 1 || HObjList[0].HObj == null || (HObjList[0].HObj is HImage) == false)
                {
                    return;
                }
                int state;
                viewPort.HalconWindow.GetMpositionSubPix(out currY, out currX, out state);
                HImage hv_image = HObjList[0].HObj as HImage;

                string str_value = "";
                string str_position = "";
                bool _isXOut = true, _isYOut = true;
                int channel_count;
                string str_imgSize = string.Format("{0}*{1}", imageHeight, imageWidth);
                channel_count = hv_image.CountChannels();

                str_position = string.Format("|{0:F0}*{1:F0}", currY, currX);
                _isXOut = (currX < 0 || currX >= imageWidth);
                _isYOut = (currY < 0 || currY >= imageHeight);
                //获取图片当前鼠标位置灰度值。
                if (!_isXOut && !_isYOut)
                {
                    if ((int)channel_count == 1)
                    {
                        double grayVal;
                        grayVal = hv_image.GetGrayval((int)currY, (int)currX);
                        str_value = String.Format("|{0}", grayVal);
                    }
                    else if ((int)channel_count == 3)
                    {
                        double grayValRed, grayValGreen, grayValBlue;

                        HImage _RedChannel, _GreenChannel, _BlueChannel;

                        _RedChannel = hv_image.AccessChannel(1);
                        _GreenChannel = hv_image.AccessChannel(2);
                        _BlueChannel = hv_image.AccessChannel(3);

                        grayValRed = _RedChannel.GetGrayval((int)currY, (int)currX);
                        grayValGreen = _GreenChannel.GetGrayval((int)currY, (int)currX);
                        grayValBlue = _BlueChannel.GetGrayval((int)currY, (int)currX);
                        str_value = String.Format("| R:{0}, G:{1}, B:{2})", grayValRed, grayValGreen, grayValBlue);
                    }
                    else
                    {
                        str_value = "";
                    }
                }
                string message = str_imgSize + str_position + str_value;

                if (message.Length > 0)
                {
                    TriggerShowMessageEvent(new ShowMessageEventArgs(message));
                }
                else
                {
                    return;
                }
                if (!mousePressed)
                    return;
                //if (currX1.Length != 1 || currY1.Length != 1)
                //{
                //    return;
                //}

                if (roiManager != null &&
                    (roiManager.ActiveRoiIdx != -1) && (showMode == ShowMode.IncludeROI))
                {
                    roiManager.MouseMoveAction(currX, currY);
                }
                else
                {
                    if (!isMoveEnable)
                    {
                        return;
                    }
                    //qDebug()<<"xx.D():"<<xx.;
                    motionX = ((currX - startX));
                    motionY = ((currY - startY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = currX - motionX;
                        startY = currY - motionY;
                    }
                }
            }
            catch (HOperatorException)
            {
                return;
            }
            catch (Exception)
            {
                return;
            }
        }

        
        public void ShowOK()
        {
            TriggerShowMessageEvent(new ShowMessageEventArgs(MessageType.ShowOk));
        }
        public void ShowNg()
        {
            TriggerShowMessageEvent(new ShowMessageEventArgs(MessageType.ShowNg));
        }
       
        /// <summary>
        ///控件图像对象刷新
        /// </summary>
        public void Repaint()
        {
            repaint(viewPort.HalconWindow);
        }

        /// <summary>
        /// 重绘halcon窗口
        /// </summary>
        private void repaint(HWindow window)
        {
            if (window.IsInitialized() == false)
            {
                return;
            }
            double scale1 = (1.0) / ZoomWndFactor;
            ShowObject(window);
            int scale = (int)((double)(viewPort.Width) * ZoomWndFactor);

            if (roiManager != null && (showMode == ShowMode.IncludeROI))
            {
                roiManager.PaintData(window, scale, scale1);
            }
            //显示十字架等
            ShowHat(window);
        }

        void ShowObject(HWindow window)
        {
            if (window.IsInitialized() == false)
            {
                return;
            }
            //关闭显示刷新
            //HSystem.SetSystem("flush_graphic", "false");
            //窗体图像清空
            window.ClearWindow();
            mGC.stateOfSettings.Clear();
            try
            {
                int count1 = 0;
                foreach (var item in HObjList)
                {
                    if (ResultShow == ResultShow.原图 && count1 > 0)
                    {
                        break;
                    }
                    if (item.HObj != null && item.HObj.IsInitialized())
                    {
                        mGC.ApplyContext(window, item.gContext);
                        window.DispObj(item.HObj);
                    }
                    else if (item.Message != null && isShowMessage)
                    {
                        //item.Message.DispMessage(window, "image", ((double)imageWidth / (double)(viewPort.Width)) / ZoomWndFactor);
                        double sizeTmp = item.Message.CahangeDisplayFontSize(window, (1.0) / ZoomWndFactor, currentTextSize);
                        currentTextSize = sizeTmp;
                        item.Message.DispMessage(window, "image");
                    }
                    count1++;
                }

            }
            catch (Exception)
            {; }
        }
        void ShowHat(HWindow window)
        {
            //HSystem.SetSystem("flush_graphic", "true");
            if (isShowCross)
            {
                //获取当前显示信息
                HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
                int hv_lineWidth;

                window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

                hv_lineWidth = (int)window.GetLineWidth();
                string hv_Draw = window.GetDraw();
                window.SetLineWidth(1);//设置线宽
                window.SetLineStyle(new HTuple());
                window.SetColor("green");//十字架显示颜色
                double CrossCol = (double)imageWidth / 2.0, CrossRow = (double)imageHeight / 2.0;
                double borderWidth = (double)imageWidth / 50.0;
                CrossCol = (double)imageWidth / 2.0;
                CrossRow = (double)imageHeight / 2.0;
                //竖线1

                window.DispPolygon(new HTuple(0, CrossRow - 50), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow + 50, ImageHeight), new HTuple(CrossCol, CrossCol));


                //中心点
                window.DispPolygon(new HTuple(CrossRow - 2, CrossRow + 2), new HTuple(CrossCol, CrossCol));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol - 2, CrossCol + 2));

                //横线

                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(0, CrossCol - 50));
                window.DispPolygon(new HTuple(CrossRow, CrossRow), new HTuple(CrossCol + 50, ImageWidth));


                //恢复窗口显示信息
                window.SetRgb(hv_Red, hv_Green, hv_Blue);
                window.SetLineWidth(hv_lineWidth);
                window.SetDraw(hv_Draw);
            }
            else
            {
                window.SetColor(backguoundColor);
                window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
            //window.FlushBuffer();
        }


        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/
        public void AddText(string message, int row, int colunm, int size, HTuple color)
        {
            AddIconicVar(new HWndMessage(message, row, colunm, size, color));
        }

        public void AddText(string message, int row, int colunm)
        {
            AddIconicVar(new HWndMessage(message, row, colunm));
        }

        public void AddIconicVar(HWndMessage message)
        {
            HObjectEntry entry;
            if (message == null)
            {
                return;
            }
            entry = new HObjectEntry(message);
            HObjList.Add(entry);
            if (HObjList.Count > MAX_NUM_OBJ_LIST)
                HObjList.RemoveAt(1);
        }

        /// <summary>
        /// 添加图像变量
        /// </summary>
        /// <param name="obj"></param>
        public void AddIconicVar(HObject obj)
        {
            if (obj == null)
                return;
            //图片数据就更新长宽信息
            if (obj is HImage && obj.IsInitialized())
            {
                double r, c;
                int h, w, area;
                string s;

                area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);  //GetDomain该对象的整个作用域，ROI就是通过ReduceDomain()减少作用域来实现的。
                ((HImage)obj).GetImagePointer1(out s, out w, out h);

                //图像无论是否Domain(), w及h不会变化
                if ((h != imageHeight) || (w != imageWidth))
                {
                    //viewPort.HalconWindow.SetWindowParam("background_color", backguoundColor);
                    imageHeight = h;
                    imageWidth = w;
                    ResetWindow();         //没有要求窗口是stretch模式，让该object对象填满整个窗口。
                }

                //面积=长*宽 表示对象作用域与该对象面积是相等的，那就代表整张原图像，而不是ROI对象。
                if (area == (w * h))
                {
                    ClearList();
                    isShowMessage = true;
                    resultShow = ResultShow.处理后;
                }
            }

            HObjectEntry entry;
            entry = new HObjectEntry(obj, mGC.CopyContextList());
            //entry = new HObjectEntry(obj, new Hashtable());
            HObjList.Add(entry);
            if (HObjList.Count > MAX_NUM_OBJ_LIST)
                HObjList.RemoveAt(1);             //HObjList[0]是模板图片，不能删除。
        }

        /// <summary>
        /// 清除图像列表
        /// </summary>
        public void ClearList()
        {           
            HObjList.Clear();
        }

        public void ClearWindowData()
        {
            HObjList.Clear();
        }

        /// <summary>
        /// 修改运行的状态
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="val"></param>
        public void ChangeGraphicSettings(HTuple mode, HTuple val)
        {
            switch (mode.S)
            {
                case GraphicsContext.GC_COLOR:
                    mGC.SetColorAttribute(val);
                    break;
                case GraphicsContext.GC_DRAWMODE:
                    mGC.SetDrawModeAttribute(val);
                    break;
                case GraphicsContext.GC_LUT:
                    mGC.SetLutAttribute(val);
                    break;
                case GraphicsContext.GC_PAINT:
                    mGC.SetPaintAttribute(val);
                    break;
                case GraphicsContext.GC_SHAPE:
                    mGC.SetShapeAttribute(val);
                    break;
                case GraphicsContext.GC_COLORED:
                    mGC.SetColoredAttribute(val);
                    break;
                case GraphicsContext.GC_LINEWIDTH:
                    mGC.SetLineWidthAttribute(val);
                    break;
                case GraphicsContext.GC_LINESTYLE:
                    mGC.SetLineStyleAttribute(val);
                    break;
                default:
                    break;
            }
        }
        public void SetBackgroundColor(string color)
        {
            backguoundColor = color;
            viewPort.HalconWindow.SetWindowParam("background_color", backguoundColor);
            Repaint();
        }
    }//end of class
}//end of namespace
