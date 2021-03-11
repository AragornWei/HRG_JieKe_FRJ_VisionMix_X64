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
    public class Grab_Intervene
    {
        public List<ROI> Grab_Intervene_ROIList = new List<ROI>();
        public HRegion Grab_Intervene_Region;

        [NonSerialized]
        HRegion ResultRegion;
        [NonSerialized]
        HRegion Grab_Intervene_Region_Affine;
        [NonSerialized]
        HRegion Grab_Intervene_Region_Affines;

        public int minThreshold = 100;
        public double filterRadiu = 3.5;
        public int minAreaThread = 100;


        [NonSerialized]
        public HTuple grabInterveneOkNg;


        //程序调用
        [NonSerialized]
        HDevProcedure Grab_Intervene_Procedure;
        [NonSerialized]
        HDevProcedureCall Grab_Intervene_ProcedureCall;
        [NonSerialized]
        bool bIsInitial = false;
        public Grab_Intervene()
        {
            Initial_Grab_Intervene();
        }

        public void Initial_Grab_Intervene()
        {
            Grab_Intervene_Procedure = new HDevProcedure("Grab_Intervene");
            Grab_Intervene_ProcedureCall = Grab_Intervene_Procedure.CreateCall();
            bIsInitial = true;
        }
        public bool Grab_Intervene_Act(HImage image, List<HHomMat2D> Mat2Ds)
        {
            try
            {
                if (!bIsInitial)
                {
                    Initial_Grab_Intervene();
                }
                if (Grab_Intervene_Region == null || !Grab_Intervene_Region.IsInitialized())
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

                grabInterveneOkNg = new HTuple();

                if (Grab_Intervene_Region_Affines == null)
                    Grab_Intervene_Region_Affines = new HRegion();
                if (Grab_Intervene_Region_Affines != null && Grab_Intervene_Region_Affines.IsInitialized())
                {
                    Grab_Intervene_Region_Affines.Dispose();
                }
                Grab_Intervene_Region_Affines.GenEmptyObj();

                for (int i = 0; i < Mat2Ds.Count(); i++)
                {
                    if (Grab_Intervene_Region_Affine == null)
                        Grab_Intervene_Region_Affine = new HRegion();
                    if (Grab_Intervene_Region_Affine != null && Grab_Intervene_Region_Affine.IsInitialized())
                    {
                        Grab_Intervene_Region_Affine.Dispose();
                    }
                    Grab_Intervene_Region_Affine.GenEmptyObj();
                    Grab_Intervene_Region_Affine = Mat2Ds[i].AffineTransRegion(Grab_Intervene_Region, "nearest_neighbor");
                    Grab_Intervene_Region_Affines= Grab_Intervene_Region_Affines.ConcatObj(Grab_Intervene_Region_Affine);
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
            Grab_Intervene_ProcedureCall.SetInputIconicParamObject("G", image);
            Grab_Intervene_ProcedureCall.SetInputIconicParamObject("ROI_Grab_T", Grab_Intervene_Region_Affine);
            Grab_Intervene_ProcedureCall.SetInputCtrlParamTuple("minThreshold", new HTuple(minThreshold));
            Grab_Intervene_ProcedureCall.SetInputCtrlParamTuple("filterRadiu", new HTuple(filterRadiu));
            Grab_Intervene_ProcedureCall.SetInputCtrlParamTuple("minAreaThread", new HTuple(minAreaThread));
            Grab_Intervene_ProcedureCall.Execute();
            if (grabInterveneOkNg.Length == 0)
            {
                grabInterveneOkNg = (Grab_Intervene_ProcedureCall.GetOutputCtrlParamTuple("grabInterveneOkNg"));
            }
            else
            {
                grabInterveneOkNg = grabInterveneOkNg.TupleConcat(Grab_Intervene_ProcedureCall.GetOutputCtrlParamTuple("grabInterveneOkNg"));
            }
            HRegion temp = new HRegion();
            temp = Grab_Intervene_ProcedureCall.GetOutputIconicParamRegion("ResultRegion");
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
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "red");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 3);
                viewCtrl.AddIconicVar(ResultRegion);
            }
            if (Grab_Intervene_Region_Affines != null && Grab_Intervene_Region_Affines.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "blue");               
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(Grab_Intervene_Region_Affines);
            }


        }
        public void SerializeCheck()
        {
            if (Grab_Intervene_Region_Affine != null && Grab_Intervene_Region_Affine.IsInitialized())
            {
                Grab_Intervene_Region_Affine.Dispose();
            }
            Grab_Intervene_Region_Affine = null;

            if (Grab_Intervene_Region_Affines != null && Grab_Intervene_Region_Affines.IsInitialized())
            {
                Grab_Intervene_Region_Affines.Dispose();
            }
            Grab_Intervene_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (Grab_Intervene_Region != null && !Grab_Intervene_Region.IsInitialized())
            {
                Grab_Intervene_Region = null;
            }

            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (Grab_Intervene_Region_Affine != null && Grab_Intervene_Region_Affine.IsInitialized())
            {
                Grab_Intervene_Region_Affine.Dispose();
            }
            Grab_Intervene_Region_Affine = null;

            if (Grab_Intervene_Region_Affines != null && Grab_Intervene_Region_Affines.IsInitialized())
            {
                Grab_Intervene_Region_Affines.Dispose();
            }
            Grab_Intervene_Region_Affines = null;

            if (ResultRegion != null && ResultRegion.IsInitialized())
            {
                ResultRegion.Dispose();
            }
            ResultRegion = null;

            if (Grab_Intervene_Region != null && !Grab_Intervene_Region.IsInitialized())
            {
                Grab_Intervene_Region = null;
            }

            Grab_Intervene_ROIList.Clear();
            Grab_Intervene_ROIList = null;
        }
        public void Reset()
        {
            Grab_Intervene_ROIList = new List<ROI>();

            if (Grab_Intervene_Region != null && !Grab_Intervene_Region.IsInitialized())
            {
                Grab_Intervene_Region.Dispose();
            }
            Grab_Intervene_Region = null;
        }
    }
}
