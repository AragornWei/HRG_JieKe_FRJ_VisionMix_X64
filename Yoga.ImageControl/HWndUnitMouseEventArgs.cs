using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Yoga.ImageControl
{
    public class HWndUnitMouseEventArgs
    {
        private int cameraIndex;
        private MouseEventArgs mouseEventArgs;

        public HWndUnitMouseEventArgs(MouseEventArgs me,int cameraIndex)
        {
            this.cameraIndex = cameraIndex;
            this.mouseEventArgs = me;
        }
        public int CameraIndex
        {
            get
            {
                return cameraIndex;
            }

            set
            {
                CameraIndex = value;
            }
        }
        public MouseEventArgs MouseEventArgs
        {
            get
            {               
                return mouseEventArgs;
            }

            set
            {
                mouseEventArgs = value;
            }
        }
    }
}
