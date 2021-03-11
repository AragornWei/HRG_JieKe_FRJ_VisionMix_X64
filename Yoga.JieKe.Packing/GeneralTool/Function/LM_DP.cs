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
    public class LM_DP
    {
        public List<ROI> LM_DP_ROIList = new List<ROI>();
        public HRegion LM_DP_Region;

        [NonSerialized]
        HRegion ResultRegion;
        [NonSerialized]
        HRegion LM_DP_Region_Affine;
        [NonSerialized]
        HRegion LM_DP_Region_Affines;

        public int minThreshold = 100;
        public double filterRadiu = 3.5;
        public int minAreaThread = 50;
        public int luoMuMaxArea = 1000;
        public double luoMuWidth=92.0;

        [NonSerialized]
        public HTuple lM_DP_OkNg, lM_DP_Num, lM_DP_Area;
        


        //程序调用
        [NonSerialized]
        HDevProcedure LM_DP_Procedure;
        [NonSerialized]
        HDevProcedureCall LM_DP_ProcedureCall;
        [NonSerialized]
        bool bIsInitial = false;
        

        public LM_DP()
        {
            Initial_LM_DP();
        }

        public void Initial_LM_DP()
        {
            LM_DP_Procedure = new HDevProcedure("LuoMuDianpianDetection");
            LM_DP_ProcedureCall = LM_DP_Procedure.CreateCall();
            bIsInitial = true;
        }
        public bool LM_DP_Act(HImage image, List<HHomMat2D> Mat2Ds)
        {
            try
            {
                if (!bIsInitial)
                {
                    Initial_LM_DP();
                }
                if (LM_DP_Region == null || !LM_DP_Region.IsInitialized())
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

                lM_DP_OkNg = new HTuple();
                lM_DP_Num = new HTuple();
                lM_DP_Area = new HTuple();

                if (LM_DP_Region_Affines == null)
                    LM_DP_Region_Affines = new HRegion();
                if (LM_DP_Region_Affines != null && LM_DP_Region_Affines.IsInitialized())
                {
                    LM_DP_Region_Affines.Dispose();
                }
                LM_DP_Region_Affines.GenEmptyObj();

                for (int i = 0; i < Mat2Ds.Count(); i++)
                {
                    if (LM_DP_Region_Affine == null)
                        LM_DP_Region_Affine = new HRegion();
                    if (LM_DP_Region_Affine != null && LM_DP_Region_Affine.IsInitialized())
                    {
                        LM_DP_Region_Affine.Dispose();
                    }
                    LM_DP_Region_Affine.GenEmptyObj();
                    LM_DP_Region_Affine = Mat2Ds[i].AffineTransRegion(LM_DP_Region, "nearest_neighbor");
                    LM_DP_Region_Affines = LM_DP_Region_Affines.ConcatObj(LM_DP_Region_Affine);
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
            LM_DP_ProcedureCall.SetInputIconicParamObject("R", image);
            LM_DP_ProcedureCall.SetInputIconicParamObject("ROI_LuoMuDiePian_T", LM_DP_Region_Affine);
            LM_DP_ProcedureCall.SetInputCtrlParamTuple("minThreshold", new HTuple(minThreshold));
            LM_DP_ProcedureCall.SetInputCtrlParamTuple("filterRadiu", new HTuple(filterRadiu));
            LM_DP_ProcedureCall.SetInputCtrlParamTuple("minAreaThreshold", new HTuple(minAreaThread));
            LM_DP_ProcedureCall.SetInputCtrlParamTuple("luoMuMaxArea", new HTuple(luoMuMaxArea));
            LM_DP_ProcedureCall.SetInputCtrlParamTuple("luoMuWidth", new HTuple(luoMuWidth));
            LM_DP_ProcedureCall.Execute();
            if (lM_DP_OkNg.Length == 0)
            {
                lM_DP_OkNg = (LM_DP_ProcedureCall.GetOutputCtrlParamTuple("luoMuDiePian_OKNG"));
                lM_DP_Num = (LM_DP_ProcedureCall.GetOutputCtrlParamTuple("lM_DP_Num"));
                lM_DP_Area = (LM_DP_ProcedureCall.GetOutputCtrlParamTuple("lM_DP_Area"));
            }
            else
            {
                lM_DP_OkNg = lM_DP_OkNg.TupleConcat(LM_DP_ProcedureCall.GetOutputCtrlParamTuple("luoMuDiePian_OKNG"));
                lM_DP_Num = lM_DP_Num.TupleConcat(LM_DP_ProcedureCall.GetOutputCtrlParamTuple("lM_DP_Num"));
                lM_DP_Area = lM_DP_Area.TupleConcat(LM_DP_ProcedureCall.GetOutputCtrlParamTuple("lM_DP_Area"));
            }
            HRegion temp = new HRegion();
            temp = LM_DP_ProcedureCall.GetOutputIconicParamRegion("Result_Region");
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
            //if (LM_DP_Region_Affines != null && LM_DP_Region_Affines.IsInitialized())
            //{
            //    viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");               
            //    viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
            //    viewCtrl.AddIconicVar(LM_DP_Region_Affines);
            //}
        }
        public void SerializeCheck()
        {
            if (LM_DP_Region_Affine != null && LM_DP_Region_Affine.IsInitialized())
            {
                LM_DP_Region_Affine.Dispose();
            }
            LM_DP_Region_Affine = null;

            if (LM_DP_Region_Affines != null && LM_DP_Region_Affines.IsInitialized())
            {
                LM_DP_Region_Affines.Dispose();
            }
            LM_DP_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (LM_DP_Region != null && !LM_DP_Region.IsInitialized())
            {
                LM_DP_Region = null;
            }

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (LM_DP_Region_Affine != null && LM_DP_Region_Affine.IsInitialized())
            {
                LM_DP_Region_Affine.Dispose();
            }
            LM_DP_Region_Affine = null;

            if (LM_DP_Region_Affines != null && LM_DP_Region_Affines.IsInitialized())
            {
                LM_DP_Region_Affines.Dispose();
            }
            LM_DP_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (LM_DP_Region != null && !LM_DP_Region.IsInitialized())
            {
                LM_DP_Region = null;
            }

            LM_DP_ROIList.Clear();
            LM_DP_ROIList = null;
        }
        public void Reset()
        {
            LM_DP_ROIList = new List<ROI>();

            if (LM_DP_Region != null && !LM_DP_Region.IsInitialized())
            {
                LM_DP_Region.Dispose();
            }
            LM_DP_Region = null;
        }
    }
}
