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
    public class LanQuan
    {
        public List<ROI> LanQuan_ROIList = new List<ROI>();
        public HRegion LanQuan_Region;

        [NonSerialized]
        HRegion ResultRegion;
        [NonSerialized]
        HRegion LanQuan_Region_Affine;
        [NonSerialized]
        HRegion LanQuan_Region_Affines;

        public int minThreshold = 130;
        public double filterRadiu = 3.5;
        public int minArea = 100;


        [NonSerialized]
        public HTuple LanQuan_OkNg;
        [NonSerialized]
        public HTuple Area;


        //程序调用
        [NonSerialized]
        HDevProcedure LanQuan_Procedure;
        [NonSerialized]
        HDevProcedureCall LanQuan_ProcedureCall;
        [NonSerialized]
        bool bIsInitial = false;
        public LanQuan()
        {
            Initial_LanQuan();
        }

        public void Initial_LanQuan()
        {
            LanQuan_Procedure = new HDevProcedure("LanQuan_Detection");
            LanQuan_ProcedureCall = LanQuan_Procedure.CreateCall();
            bIsInitial = true;
        }
        public bool LanQuan_Act(HImage R, HImage B, List<HHomMat2D> Mat2Ds)
        {
            try
            {
                if (!bIsInitial)
                {
                    Initial_LanQuan();
                }
                if (LanQuan_Region == null || !LanQuan_Region.IsInitialized())
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

                LanQuan_OkNg = new HTuple();
                Area = new HTuple();

                if (LanQuan_Region_Affines == null)
                    LanQuan_Region_Affines = new HRegion();
                if (LanQuan_Region_Affines != null && LanQuan_Region_Affines.IsInitialized())
                {
                    LanQuan_Region_Affines.Dispose();
                }
                LanQuan_Region_Affines.GenEmptyObj();

                for (int i = 0; i < Mat2Ds.Count(); i++)
                {
                    if (LanQuan_Region_Affine == null)
                        LanQuan_Region_Affine = new HRegion();
                    if (LanQuan_Region_Affine != null && LanQuan_Region_Affine.IsInitialized())
                    {
                        LanQuan_Region_Affine.Dispose();
                    }
                    LanQuan_Region_Affine.GenEmptyObj();
                    LanQuan_Region_Affine = Mat2Ds[i].AffineTransRegion(LanQuan_Region, "nearest_neighbor");
                    LanQuan_Region_Affines = LanQuan_Region_Affines.ConcatObj(LanQuan_Region_Affine);
                    Act_Engine(R,B);
                }

            }
            catch
            {
                return false;
            }
            return true;

        }
        private void Act_Engine(HImage R,HImage B)
        {
            LanQuan_ProcedureCall.SetInputIconicParamObject("R", R);
            LanQuan_ProcedureCall.SetInputIconicParamObject("B", B);
            LanQuan_ProcedureCall.SetInputIconicParamObject("ROI_LanQuan_T", LanQuan_Region_Affine);
            LanQuan_ProcedureCall.SetInputCtrlParamTuple("minThreshold", new HTuple(minThreshold));
            LanQuan_ProcedureCall.SetInputCtrlParamTuple("filterRadiu", new HTuple(filterRadiu));
            LanQuan_ProcedureCall.SetInputCtrlParamTuple("minArea", new HTuple(minArea));
            LanQuan_ProcedureCall.Execute();
            if (LanQuan_OkNg.Length == 0)
            {
                LanQuan_OkNg = (LanQuan_ProcedureCall.GetOutputCtrlParamTuple("lanQuan_OKNG"));
                Area = LanQuan_ProcedureCall.GetOutputCtrlParamTuple("Area");
            }
            else
            {
                LanQuan_OkNg = LanQuan_OkNg.TupleConcat(LanQuan_ProcedureCall.GetOutputCtrlParamTuple("lanQuan_OKNG"));
                Area = Area.TupleConcat(LanQuan_ProcedureCall.GetOutputCtrlParamTuple("Area"));
            }
            HRegion temp = new HRegion();
            temp = LanQuan_ProcedureCall.GetOutputIconicParamRegion("Result_Region");
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
            //if (LanQuan_Region_Affines != null && LanQuan_Region_Affines.IsInitialized())
            //{
            //    viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");
            //    viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
            //    viewCtrl.AddIconicVar(LanQuan_Region_Affines);
            //}
        }
        public void SerializeCheck()
        {
            if (LanQuan_Region_Affine != null && LanQuan_Region_Affine.IsInitialized())
            {
                LanQuan_Region_Affine.Dispose();
            }
            LanQuan_Region_Affine = null;

            if (LanQuan_Region_Affines != null && LanQuan_Region_Affines.IsInitialized())
            {
                LanQuan_Region_Affines.Dispose();
            }
            LanQuan_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (LanQuan_Region != null && !LanQuan_Region.IsInitialized())
            {
                LanQuan_Region = null;
            }

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (LanQuan_Region_Affine != null && LanQuan_Region_Affine.IsInitialized())
            {
                LanQuan_Region_Affine.Dispose();
            }
            LanQuan_Region_Affine = null;

            if (LanQuan_Region_Affines != null && LanQuan_Region_Affines.IsInitialized())
            {
                LanQuan_Region_Affines.Dispose();
            }
            LanQuan_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (LanQuan_Region != null && !LanQuan_Region.IsInitialized())
            {
                LanQuan_Region = null;
            }

            LanQuan_ROIList.Clear();
            LanQuan_ROIList = null;
        }
        public void Reset()
        {
            LanQuan_ROIList = new List<ROI>();

            if (LanQuan_Region != null && !LanQuan_Region.IsInitialized())
            {
                LanQuan_Region.Dispose();
            }
            LanQuan_Region = null;
        }
    }
}
