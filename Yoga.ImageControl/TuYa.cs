using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;


namespace Yoga.ImageControl
{
    [Serializable]
    public class TuYa : ROI
    {
        HRegion reduceRegion;
        double radius = 5;
        public TuYa(double radius)
        {
            this.radius = radius;
            ROIType = ROIType.TuYa;
        }

        public override void SetTuYaRadius(double radius)
        {
            this.radius = radius;
        }

        public override HRegion GetRegion()
        {
            return reduceRegion;
        }

        public override void MoveByHandle(double newX, double newY)
        {
            updateTuYaRegion(newY, newX);
        }

        public void updateTuYaRegion(double row, double col)
        {
            if (reduceRegion == null)
            {
                reduceRegion = new HRegion();
                reduceRegion.GenEmptyRegion();
            }
            HRegion temp = new HRegion();
            temp.GenEmptyRegion();
            if (row < 1  && col < 1)
                return;
            temp.GenCircle(row, col, radius);
            reduceRegion = reduceRegion.Union2(temp);
        }

        public override void ClearTuYa()
        {
            if(reduceRegion!=null && reduceRegion.IsInitialized())
            {
                reduceRegion.Dispose();
            }
            reduceRegion = new HRegion();
            reduceRegion.GenEmptyRegion();
        }

        public override double DistToClosestHandle(double x, double y)
        {
            double distMin, distMax;
            reduceRegion.DistancePr(y, x, out distMin, out distMax);
            return distMin;
        }

        public override void Draw(HWindow window)
        {

            if(reduceRegion!=null && reduceRegion.IsInitialized())
            {
                window.DispRegion(reduceRegion);
            }
            

        }

    }
}
