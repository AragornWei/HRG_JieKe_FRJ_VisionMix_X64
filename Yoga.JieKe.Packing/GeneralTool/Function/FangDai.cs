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
    public class FangDai
    {
        public List<ROI> FangDai_ROIList = new List<ROI>();
        public HRegion FangDai_Region;

        [NonSerialized]
        HRegion ResultRegion;
        [NonSerialized]
        HRegion FangDai_Region_Affine;
        [NonSerialized]
        HRegion FangDai_Region_Affines;

        public int minThreshold = 100;
        public int maxArea = 500;
        public int minArea = 50;


        [NonSerialized]
        public HTuple FangDai_OkNg;
        [NonSerialized]
        public HTuple Area;


        //程序调用
        [NonSerialized]
        HDevProcedure FangDai_Procedure;
        [NonSerialized]
        HDevProcedureCall FangDai_ProcedureCall;
        [NonSerialized]
        bool bIsInitial = false;
        public FangDai()
        {
            Initial_FangDai();
        }

        public void Initial_FangDai()
        {
            FangDai_Procedure = new HDevProcedure("FD_Detection");
            FangDai_ProcedureCall = FangDai_Procedure.CreateCall();
            bIsInitial = true;
        }
        public bool FangDai_Act(HImage image, List<HHomMat2D> Mat2Ds)
        {
            try
            {
                if (!bIsInitial)
                {
                    Initial_FangDai();
                }
                if (FangDai_Region == null || !FangDai_Region.IsInitialized())
                {
                    return false;
                }
                if (Mat2Ds == null || Mat2Ds.Count() < 1)
                {
                    return false;
                }
                if (ResultRegion == null)
                    ResultRegion = new HRegion();
                if (ResultRegion != null && ResultRegion.IsInitialized())
                    ResultRegion.Dispose();
                ResultRegion.GenEmptyObj();

                FangDai_OkNg = new HTuple();
                Area = new HTuple();

                if (FangDai_Region_Affines == null)
                    FangDai_Region_Affines = new HRegion();
                if (FangDai_Region_Affines != null && FangDai_Region_Affines.IsInitialized())
                {
                    FangDai_Region_Affines.Dispose();
                }
                FangDai_Region_Affines.GenEmptyObj();

                for (int i = 0; i < Mat2Ds.Count(); i++)
                {
                    if (FangDai_Region_Affine == null)
                        FangDai_Region_Affine = new HRegion();
                    if (FangDai_Region_Affine != null && FangDai_Region_Affine.IsInitialized())
                    {
                        FangDai_Region_Affine.Dispose();
                    }
                    FangDai_Region_Affine.GenEmptyObj();
                    FangDai_Region_Affine = Mat2Ds[i].AffineTransRegion(FangDai_Region, "nearest_neighbor");
                    FangDai_Region_Affines = FangDai_Region_Affines.ConcatObj(FangDai_Region_Affine);
                    Act_Engine(image);
                }

            }
            catch
            {
                return false;
            }
            return true;

        }
        private void Act_Engine(HImage image)
        {
            FangDai_ProcedureCall.SetInputIconicParamObject("G", image);
            FangDai_ProcedureCall.SetInputIconicParamObject("RegionFD_T", FangDai_Region_Affine);
            FangDai_ProcedureCall.SetInputCtrlParamTuple("minThreshold", new HTuple(minThreshold));
            FangDai_ProcedureCall.SetInputCtrlParamTuple("minArea", new HTuple(minArea));
            FangDai_ProcedureCall.SetInputCtrlParamTuple("maxArea", new HTuple(maxArea));
            FangDai_ProcedureCall.Execute();
            if (FangDai_OkNg.Length == 0)
            {
                FangDai_OkNg = (FangDai_ProcedureCall.GetOutputCtrlParamTuple("fD_OkNg"));
                Area = FangDai_ProcedureCall.GetOutputCtrlParamTuple("Area");
            }
            else
            {
                FangDai_OkNg= FangDai_OkNg.TupleConcat(FangDai_ProcedureCall.GetOutputCtrlParamTuple("fD_OkNg"));
                Area = Area.TupleConcat(FangDai_ProcedureCall.GetOutputCtrlParamTuple("Area"));
            }
            HRegion temp = new HRegion();
            temp = FangDai_ProcedureCall.GetOutputIconicParamRegion("ResultRegion");
            if (temp != null && temp.IsInitialized())
            {
                ResultRegion = ResultRegion.ConcatObj(temp);
                temp.Dispose();
            }
        }



        public void Show(HWndCtrl viewCtrl)
        {

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "green");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 3);
                viewCtrl.AddIconicVar(ResultRegion);
            }
            if (FangDai_Region_Affines != null && FangDai_Region_Affines.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(FangDai_Region_Affines);
            }
        }
        public void SerializeCheck()
        {
            if (FangDai_Region_Affine != null && FangDai_Region_Affine.IsInitialized())
            {
                FangDai_Region_Affine.Dispose();
            }
            FangDai_Region_Affine = null;

            if (FangDai_Region_Affines != null && FangDai_Region_Affines.IsInitialized())
            {
                FangDai_Region_Affines.Dispose();
            }
            FangDai_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (FangDai_Region != null && !FangDai_Region.IsInitialized())
            {
                FangDai_Region = null;
            }

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (FangDai_Region_Affine != null && FangDai_Region_Affine.IsInitialized())
            {
                FangDai_Region_Affine.Dispose();
            }
            FangDai_Region_Affine = null;

            if (FangDai_Region_Affines != null && FangDai_Region_Affines.IsInitialized())
            {
                FangDai_Region_Affines.Dispose();
            }
            FangDai_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (FangDai_Region != null && !FangDai_Region.IsInitialized())
            {
                FangDai_Region = null;
            }

            FangDai_ROIList.Clear();
            FangDai_ROIList = null;
        }
        public void Reset()
        {
            FangDai_ROIList = new List<ROI>();

            if (FangDai_Region != null && !FangDai_Region.IsInitialized())
            {
                FangDai_Region.Dispose();
            }
            FangDai_Region = null;
        }
    }
}
