using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using Yoga.ImageControl;
using System.IO;

namespace Yoga.JieKe.Packing.GeneralTool
{
    [Serializable]
    public class CreateShapeModel
    {
        public List<ROI> shapeModelROIList = new List<ROI>();
        public HRegion modelRegion;
        public HShapeModel hShapeModel;

        public int numLevels = 4;
        public double angleStart = (0) * Math.PI / 180.0;
        public double angleExtent = (360) * Math.PI / 180.0;
        public double angleStep = 0.2 * Math.PI / 180.0;
        public double scaleMin = 1.0;
        public double scaleMax = 1.0;
        public double scaleStep = 3 / 1000.0;
        public string optimization = "none";
        public string metric = "use_polarity";

        public int contrastHigh = 60;
        public int contrastLow = 30;
        public int minLength = 15;
        public int minContrast = 10;

        [NonSerialized]
        public bool createNewModelID = false;
        public HTuple refCoordinates;


        [NonSerialized]
        HXLDCont modelXLDCont;
        [NonSerialized]
        HXLDCont modelXLDContAffine;
        [NonSerialized]
        HImage modelImage;

        public HRegion ModelRegion
        {
            get
            {
                if (modelRegion == null)
                    modelRegion = new HRegion();
                return modelRegion;
            }
            set
            {
                modelRegion = value;
                CreateShapeModelAct(modelImage);
            }
        }
        public HXLDCont ModelXLDCont
        {
            get
            {
                if (hShapeModel == null || !hShapeModel.IsInitialized())
                    return null;
                if (modelXLDCont == null || !modelXLDCont.IsInitialized())
                {
                    modelXLDCont = hShapeModel.GetShapeModelContours(1);
                }
                return modelXLDCont;
            }
        }

        public void Initial(HImage modelImage)
        {
            this.modelImage = modelImage;
        }

        public bool CreateShapeModelAct(HImage modelImage)
        {
            if (modelImage == null || !modelImage.IsInitialized())
                return false;
            this.modelImage = modelImage;

            if (modelRegion == null || !modelRegion.IsInitialized())
                return false;
            try
            {

                HImage ROIImage ;
                ROIImage = modelImage.ReduceDomain(modelRegion);
                hShapeModel = ROIImage.CreateScaledShapeModel(
                    new HTuple(numLevels),
                    angleStart, angleExtent, new HTuple(angleStep),
                    scaleMin, scaleMax, new HTuple(scaleStep),
                    new HTuple(optimization).TupleConcat("no_pregeneration"),
                    metric,
                    ((new HTuple(contrastLow)).TupleConcat(contrastHigh)).TupleConcat(minLength),
                    new HTuple(minContrast));
                ROIImage.Dispose();
                double row, col;
                modelRegion.AreaCenter(out row, out col);
                refCoordinates = new HTuple(row, col, 0.0);
                createNewModelID = false;
                //初始化modelContours
                if (modelXLDCont != null && modelXLDCont.IsInitialized())
                    modelXLDCont.Dispose();
                modelXLDCont = hShapeModel.GetShapeModelContours(1);
            }
            catch
            {
                hShapeModel = null;
                createNewModelID = true;
                return false;
            }

            return true;
        }

        public void ShowShapeModel(HWndCtrl viewCtrl)
        {

            if (modelRegion == null || !modelRegion.IsInitialized())
                return;
            if (ModelXLDCont == null || !ModelXLDCont.IsInitialized())
                return;
            viewCtrl.ChangeGraphicSettings(Mode.DRAWMODE, "margin");
            if (modelRegion != null && modelRegion.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(modelRegion);
            }

            if (modelXLDContAffine != null && modelXLDContAffine.IsInitialized())
                modelXLDContAffine.Dispose();
            if (refCoordinates == null || refCoordinates.Length != 3)
                return;

            HHomMat2D homMat2D = new HHomMat2D();
            homMat2D.VectorAngleToRigid(0.0, 0.0, 0.0, refCoordinates[0].D, refCoordinates[1].D, refCoordinates[2].D);
            modelXLDContAffine = homMat2D.AffineTransContourXld(ModelXLDCont);

            if (modelXLDContAffine != null && modelXLDContAffine.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "red");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 1);
                viewCtrl.AddIconicVar(modelXLDContAffine);
            }
        }

        public void SerializeCheck()
        {
            if (modelXLDCont != null && modelXLDCont.IsInitialized())
            {
                modelXLDCont.Dispose();
            }
            if (modelXLDContAffine != null && modelXLDContAffine.IsInitialized())
            {
                modelXLDContAffine.Dispose();
            }

            modelXLDCont = null;
            modelXLDContAffine = null;

            if (hShapeModel != null && !hShapeModel.IsInitialized())
                hShapeModel = null;
            if (modelRegion != null && !modelRegion.IsInitialized())
                modelRegion = null;

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }

        }

        public void Close()
        {
            if (hShapeModel != null && hShapeModel.IsInitialized())
                hShapeModel.Dispose();
            hShapeModel = null;
            if (modelRegion != null && modelRegion.IsInitialized())
                modelRegion.Dispose();
            modelRegion = null;

            if (modelXLDCont != null && modelXLDCont.IsInitialized())
            {
                modelXLDCont.Dispose();
            }
            modelXLDCont = null;
            if (modelXLDContAffine != null && modelXLDContAffine.IsInitialized())
            {
                modelXLDContAffine.Dispose();
            }
            modelXLDContAffine = null;

            shapeModelROIList = null;
        }

        public void Reset()
        {
            shapeModelROIList = new List<ROI>();

            if (modelRegion != null && !modelRegion.IsInitialized())
            {
                modelRegion.Dispose();
            }
            modelRegion = null;

            if (hShapeModel != null && !hShapeModel.IsInitialized())
            {
                hShapeModel.Dispose();
            }
            hShapeModel = null;

            refCoordinates = new HTuple();

        }
    }
}
