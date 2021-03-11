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
    public class FindShapeMode
    {
        [NonSerialized]
        CreateShapeModel createShapeModel;
        [NonSerialized]
        HImage refImage;


        public List<ROI> FindShapeModeRoiList = new List<ROI>();
        public HRegion SearchRegion;

        public double minScore = 0.7;
        public int numMatches = 0;
        public double maxOverlap = 0.1;
        public string subPixel = "least_squares";
        public int numLevels = -1;
        public double greediness = 0.75;

        [NonSerialized]
        public HTuple row, column, angle, scale, score;

        [NonSerialized]
        public HXLDCont resultXLDCont;


        //查找模板
        public bool FindShapeModeAct(HImage refImage, CreateShapeModel createShapeModel, HImage image)
        {
            this.createShapeModel = createShapeModel;
            this.refImage = refImage;

            if (createShapeModel.hShapeModel == null || !createShapeModel.hShapeModel.IsInitialized() || createShapeModel.createNewModelID)
            {
                if (!createShapeModel.CreateShapeModelAct(refImage))
                    return false;
            }
            try
            {
                HImage searchImage;
                if (SearchRegion != null && SearchRegion.IsInitialized())
                {
                    searchImage = image.ReduceDomain(SearchRegion);
                }
                else
                {
                    searchImage = image.Clone();
                }

                row = new HTuple();
                column = new HTuple();
                angle = new HTuple();
                scale = new HTuple();
                score = new HTuple();
                createShapeModel.hShapeModel.SetShapeModelParam("border_shape_models", "false");
                createShapeModel.hShapeModel.FindScaledShapeModel(
                    searchImage,
                    createShapeModel.angleStart, createShapeModel.angleExtent,
                    createShapeModel.scaleMin, createShapeModel.scaleMax,
                    minScore, numMatches,
                    maxOverlap,
                    new HTuple(subPixel).TupleConcat("max_deformation 1"),
                    new HTuple(new int[] { createShapeModel.numLevels, numLevels }),
                    greediness,
                    out row, out column, out angle, out scale, out score);

                searchImage.Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }
        //获取仿射变换矩阵
        public List<HHomMat2D> GetHHomMat2Ds()
        {
            if (row == null || row.Length < 1)
                return null;
            List<HHomMat2D> mat2Ds = new List<HHomMat2D>();
            for (int i = 0; i < row.Length; i++)
            {
                HHomMat2D homMat2D = new HHomMat2D();
                homMat2D.VectorAngleToRigid(
                    createShapeModel.refCoordinates[0].D, createShapeModel.refCoordinates[1].D, createShapeModel.refCoordinates[2].D,
                    row[i].D, column[i].D, angle[i].D);
                mat2Ds.Add(homMat2D);
            }
            return mat2Ds;
        }

        public void ShowResult(HWndCtrl viewCtrl)
        {
            //搜索范围
            viewCtrl.ChangeGraphicSettings(Mode.DRAWMODE, "margin");
            if (SearchRegion != null && SearchRegion.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(SearchRegion);
            }
            //确保已有模板
            if (createShapeModel.hShapeModel == null || !createShapeModel.hShapeModel.IsInitialized() || createShapeModel.createNewModelID)
            {
                if (!createShapeModel.CreateShapeModelAct(refImage))
                    return;
            }
            //获取仿射变换后的轮廓
            HXLDCont modelXldCont = createShapeModel.ModelXLDCont;

            if (row.Length < 1)
                return;
            GenDetectionXLDResults(modelXldCont);


            if (resultXLDCont != null && resultXLDCont.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(resultXLDCont);
            }

        }
        //仿射变换后的全部轮廓
        public void GenDetectionXLDResults(HXLDCont modelXldCont)
        {
            if (resultXLDCont == null)
            {
                resultXLDCont = new HXLDCont();                
            }
            if (resultXLDCont != null && resultXLDCont.IsInitialized())
            {
                resultXLDCont.Dispose();
            }
            resultXLDCont.GenEmptyObj();

            HXLDCont rContours;

            for (int i = 0; i < row.Length; i++)
            {
                HHomMat2D mat1 = new HHomMat2D();
                mat1.VectorAngleToRigid(0, 0, 0, row[i].D, column[i].D, angle[i].D);
                mat1 = mat1.HomMat2dScale(scale[i].D, scale[i].D, row[i].D, column[i].D);
                //图像偏移
                rContours = mat1.AffineTransContourXld(modelXldCont);
                //获取模板集合
                resultXLDCont = resultXLDCont.ConcatObj(rContours);
                rContours.Dispose();
                rContours.GenCrossContourXld(row[i].D, column[i].D, 10, angle[i].D);
                resultXLDCont = resultXLDCont.ConcatObj(rContours);
                rContours.Dispose();
            }

        }
        public void SerializeCheck()
        {
            if (resultXLDCont != null && resultXLDCont.IsInitialized())
            {
                resultXLDCont.Dispose();
            }

            refImage = null;
            createShapeModel = null;
            if (SearchRegion != null && !SearchRegion.IsInitialized())
                SearchRegion = null;
            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (resultXLDCont != null && resultXLDCont.IsInitialized())
            {
                resultXLDCont.Dispose();
            }
            resultXLDCont = null;

            refImage = null;
            createShapeModel = null;
            if (SearchRegion != null && SearchRegion.IsInitialized())
                SearchRegion.Dispose();
            SearchRegion = null;

        }
        public void Reset()
        {
            if (SearchRegion != null && !SearchRegion.IsInitialized())
            {
                SearchRegion.Dispose();
            }
            SearchRegion = null;
            refImage = null;
            createShapeModel = null;
            FindShapeModeRoiList = new List<ROI>();
        }
    }
}
