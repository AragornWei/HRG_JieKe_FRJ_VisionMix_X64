using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Yoga.Common.Basic
{
    class DrawLamp
    {
        System.Windows.Forms.Control control;
        Graphics g;
        public DrawLamp(System.Windows.Forms.Control control)
        {
            this.control = control;
        }
        public void Repaint(bool isAct)
        {
            if(control !=null)
            {
                Color color = isAct ? Color.Red : Color.Gray;
                int width = control.Width;
                int height = control.Height;
                Point point = control.Location;
                int radius = width >= height ? height : width;
                Rectangle rec = new Rectangle(point.X + width / 2 - radius / 2 - 1, point.Y + height / 2 - radius / 2 - 1, radius - 2, radius - 2);
                g = control.CreateGraphics();
                Pen p = new Pen(color, 1);
                g.DrawEllipse(p, rec);
                g.FillEllipse(new SolidBrush(color), rec);
            }
        }
    }
}
