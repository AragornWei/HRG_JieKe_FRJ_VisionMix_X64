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
    public class PZT_Detection
    {
        public List<ROI> Pzt_ROIList = new List<ROI>();
        public HRegion Pzt_Region;
        public int mesureNumber = 6;
        public int mes_width = 5;
        public double sigma = 1;
        public int threshold = 15;
        public double dist_STD = 300;
        [NonSerialized]
        public HTuple dist_PZT;
        [NonSerialized]
        public HTuple pZTOkNg;
        [NonSerialized]
        public HRegion Pzt_Region_Affine;
        [NonSerialized]
        public HXLD Arrow;

        //程序调用
        [NonSerialized]
        HDevProcedure PTZ_Detection_Procedure;
        [NonSerialized]
        HDevProcedureCall PTZ_Detection_ProcedureCall;
        [NonSerialized]
        bool bIsInitial = false;
        public PZT_Detection()
        {
            Initial_PZT_Detection();
        }
        public void Initial_PZT_Detection()
        {
            PTZ_Detection_Procedure = new HDevProcedure("PZT_Detection");
            PTZ_Detection_ProcedureCall = PTZ_Detection_Procedure.CreateCall();
            bIsInitial = true;
        }
        public bool PZT_Detection_Act(HImage image, List<HHomMat2D> Mat2Ds, HTuple Angle)
        {
            try
            {
                if (!bIsInitial)
                {
                    Initial_PZT_Detection();
                }
                if (Pzt_Region == null || !Pzt_Region.IsInitialized())
                {
                    return false;
                }
                if (Mat2Ds == null || Mat2Ds.Count() < 1)
                {
                    return false;
                }
                if (Arrow == null)
                {
                    Arrow = new HXLD();
                }
                if (Arrow != null && Arrow.IsInitialized())
                {
                    Arrow.Dispose();
                }
                Arrow.GenEmptyObj();
                dist_PZT = new HTuple();

                pZTOkNg = new HTuple();

                for (int i = 0; i < Mat2Ds.Count(); i++)
                {

                    if (Pzt_Region_Affine != null && Pzt_Region_Affine.IsInitialized())
                    {
                        Pzt_Region_Affine.Dispose();
                    }
                    Pzt_Region_Affine = Mat2Ds[i].AffineTransRegion(Pzt_Region, "nearest_neighbor");
                     Act_Engine(image, Angle, i);
                    //Act_Source(image, Angle, i);
                }

            }
            catch
            {
                return false;
            } 
            return true;

        }

        private void Act_Source(HImage image, HTuple angle, int i)
        {
            HObject temp = new HObject();
            temp.GenEmptyObj();
            HTuple dist_PZT_temp, pZTOkNg_temp;
            HTuple hv_Rows1, hv_Cols1, hv_Rows2, hv_Cols2;
            PZT_Detection_ext(Pzt_Region_Affine, image,out temp, 
                new HTuple(mesureNumber), angle[i], new HTuple(mes_width), 
                new HTuple(sigma), new HTuple(threshold), new HTuple(dist_STD),
                out dist_PZT_temp, out pZTOkNg_temp, out hv_Rows1, 
                out hv_Cols1, out hv_Rows2, out hv_Cols2);

            if (pZTOkNg.Length == 0)
            {
                dist_PZT = dist_PZT_temp;
                pZTOkNg = pZTOkNg_temp;
            }
            else
            {
                dist_PZT = dist_PZT.TupleConcat(dist_PZT_temp);
                pZTOkNg = pZTOkNg.TupleConcat(pZTOkNg_temp);
            }
            if (temp != null && temp.IsInitialized())
            {
                HXLD temp2 = new HXLD(temp);
                Arrow = Arrow.ConcatObj(temp2);
                temp.Dispose();
                temp2.Dispose();
            }
        }
        public void PZT_Detection_ext(HObject ho_RegionPZT_T, HObject ho_R, out HObject ho_Arrow,
        HTuple hv_mesureNumber, HTuple hv_angle, HTuple hv_mes_width, HTuple hv_sigma,
        HTuple hv_threshold, HTuple hv_dist_STD, out HTuple hv_dist_PZT, out HTuple hv_pZTOkNg,
        out HTuple hv_Rows1, out HTuple hv_Cols1, out HTuple hv_Rows2, out HTuple hv_Cols2)
            {

                HTuple hv_Dist = null, hv_Row = null, hv_Column = null;
                HTuple hv_Phi = null, hv_Length1 = null, hv_Length2 = null;
                HTuple hv_Inter = null, hv_RowStart = null, hv_ColStart = null;
                HTuple hv_Width = null, hv_Height = null, hv_i = null;
                HTuple hv_Row_Mes = new HTuple(), hv_Col_Mes = new HTuple();
                HTuple hv_MeasureHandle = new HTuple(), hv_RowEdge1 = new HTuple();
                HTuple hv_ColumnEdge1 = new HTuple(), hv_Amplitude1 = new HTuple();
                HTuple hv_Distance1 = new HTuple(), hv_RowEdge2 = new HTuple();
                HTuple hv_ColumnEdge2 = new HTuple(), hv_Amplitude2 = new HTuple();
                HTuple hv_Distance2 = new HTuple(), hv_L1 = new HTuple();
                HTuple hv_L2 = new HTuple(), hv_Distance = new HTuple();
                HTuple hv_Dist_Index = null;
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                hv_pZTOkNg = new HTuple();
                hv_Rows1 = new HTuple();
                hv_Cols1 = new HTuple();
                hv_Rows2 = new HTuple();
                hv_Cols2 = new HTuple();
                hv_Dist = new HTuple();
                ho_Arrow.Dispose();
                HOperatorSet.GenEmptyObj(out ho_Arrow);
                HOperatorSet.SmallestRectangle2(ho_RegionPZT_T, out hv_Row, out hv_Column, out hv_Phi,
                    out hv_Length1, out hv_Length2);
                hv_Inter = (((2 * hv_Length2) / hv_mesureNumber)).TupleFloor();
                hv_RowStart = hv_Row + ((hv_Length2 - (hv_Inter / 2)) * (((hv_angle + ((new HTuple(90)).TupleRad()
                    ))).TupleSin()));
                hv_ColStart = hv_Column - ((hv_Length2 - (hv_Inter / 2)) * (((hv_angle + ((new HTuple(90)).TupleRad()
                    ))).TupleCos()));
                HOperatorSet.GetImageSize(ho_R, out hv_Width, out hv_Height);
                HTuple end_val11 = hv_mesureNumber - 1;
                HTuple step_val11 = 1;
                for (hv_i = 0; hv_i.Continue(end_val11, step_val11); hv_i = hv_i.TupleAdd(step_val11))
                {
                    hv_Row_Mes = hv_RowStart - ((hv_Inter * hv_i) * (((hv_angle + ((new HTuple(90)).TupleRad()
                        ))).TupleSin()));
                    hv_Col_Mes = hv_ColStart + ((hv_Inter * hv_i) * (((hv_angle + ((new HTuple(90)).TupleRad()
                        ))).TupleCos()));
                    HOperatorSet.GenMeasureRectangle2(hv_Row_Mes, hv_Col_Mes, hv_angle, hv_Length1,
                        hv_mes_width, hv_Width, hv_Height, "nearest_neighbor", out hv_MeasureHandle);
                    HOperatorSet.MeasurePos(ho_R, hv_MeasureHandle, hv_sigma, hv_threshold, "positive",
                        "first", out hv_RowEdge1, out hv_ColumnEdge1, out hv_Amplitude1, out hv_Distance1);
                    HOperatorSet.MeasurePos(ho_R, hv_MeasureHandle, hv_sigma, hv_threshold, "negative",
                        "last", out hv_RowEdge2, out hv_ColumnEdge2, out hv_Amplitude2, out hv_Distance2);
                    HOperatorSet.CloseMeasure(hv_MeasureHandle);
                    HOperatorSet.TupleLength(hv_RowEdge1, out hv_L1);
                    HOperatorSet.TupleLength(hv_RowEdge2, out hv_L2);
                    if ((int)((new HTuple(hv_L1.TupleNotEqual(1))).TupleOr(new HTuple(hv_L2.TupleNotEqual(
                        1)))) != 0)
                    {
                        continue;
                    }
                    hv_Rows1 = hv_Rows1.TupleConcat(hv_RowEdge1);
                    hv_Cols1 = hv_Cols1.TupleConcat(hv_ColumnEdge1);
                    hv_Rows2 = hv_Rows2.TupleConcat(hv_RowEdge2);
                    hv_Cols2 = hv_Cols2.TupleConcat(hv_ColumnEdge2);
                    HOperatorSet.DistancePp(hv_RowEdge1, hv_ColumnEdge1, hv_RowEdge2, hv_ColumnEdge2,
                        out hv_Distance);
                    hv_Dist = hv_Dist.TupleConcat(hv_Distance);
                }
                ho_Arrow.Dispose();
                gen_arrow_contour_xld(out ho_Arrow, hv_Rows1, hv_Cols1, hv_Rows2, hv_Cols2, 20,
                    20);
                HOperatorSet.TupleSortIndex(hv_Dist, out hv_Dist_Index);
                hv_dist_PZT = hv_Dist.TupleSelect(hv_Dist_Index.TupleSelectRange(1, (new HTuple(hv_Dist_Index.TupleLength()
                    )) - 2));
                hv_dist_PZT = hv_dist_PZT.TupleMean();
                if ((int)(new HTuple(hv_dist_PZT.TupleGreater(hv_dist_STD))) != 0)
                {
                    hv_pZTOkNg = 1;
                }
                else
                {
                    hv_pZTOkNg = 0;
                }

                return;
            }
        private void Act_Engine(HImage image, HTuple Angle, int i)
        {
            PTZ_Detection_ProcedureCall.SetInputIconicParamObject("R", image);
            PTZ_Detection_ProcedureCall.SetInputIconicParamObject("RegionPZT_T", Pzt_Region_Affine);
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("mesureNumber", new HTuple(mesureNumber));
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("mes_width", new HTuple(mes_width));
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("threshold", new HTuple(threshold));
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("dist_STD", new HTuple(dist_STD));
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("angle", Angle[i]);
            PTZ_Detection_ProcedureCall.SetInputCtrlParamTuple("sigma", new HTuple(sigma));
            PTZ_Detection_ProcedureCall.Execute();
            if (pZTOkNg.Length == 0)
            {
                dist_PZT = (PTZ_Detection_ProcedureCall.GetOutputCtrlParamTuple("dist_PZT"));
                pZTOkNg = (PTZ_Detection_ProcedureCall.GetOutputCtrlParamTuple("pZTOkNg"));
            }
            else
            {
                dist_PZT = dist_PZT.TupleConcat(PTZ_Detection_ProcedureCall.GetOutputCtrlParamTuple("dist_PZT"));
                pZTOkNg = pZTOkNg.TupleConcat(PTZ_Detection_ProcedureCall.GetOutputCtrlParamTuple("pZTOkNg"));
            }
            HXLD temp = new HXLD();
            temp = PTZ_Detection_ProcedureCall.GetOutputIconicParamXld("Arrow");
            if (temp != null && temp.IsInitialized())
            {
                Arrow = Arrow.ConcatObj(temp);
                temp.Dispose();
            }
        }

        public void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
                                           HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple                          hv_HeadWidth)
        {

            HObject[] OTemp = new HObject[20];
            HObject ho_TempArrow = null;
            HTuple hv_Length = null, hv_ZeroLengthIndices = null;
            HTuple hv_DR = null, hv_DC = null, hv_HalfHeadWidth = null;
            HTuple hv_RowP1 = null, hv_ColP1 = null, hv_RowP2 = null;
            HTuple hv_ColP2 = null, hv_Index = null;
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            hv_ZeroLengthIndices = hv_Length.TupleFind(0);
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);

            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                        hv_Column1.TupleSelect(hv_Index));
                }
                else
                {
                    ho_TempArrow.Dispose();
                    HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                        hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                        hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                        ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                        hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                        hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            return;
        }

        public void Show(HWndCtrl viewCtrl)
        {
            if(Arrow!=null && Arrow.IsInitialized())
            {
                viewCtrl.ChangeGraphicSettings(Mode.COLOR, "green");
                viewCtrl.ChangeGraphicSettings(Mode.DRAWMODE, "margin");
                viewCtrl.ChangeGraphicSettings(Mode.LINEWIDTH, 2);
                viewCtrl.AddIconicVar(Arrow);
            }
        }
        public void SerializeCheck()
        {
            if (Pzt_Region_Affine != null && Pzt_Region_Affine.IsInitialized())
            {
                Pzt_Region_Affine.Dispose();
            }
            Pzt_Region_Affine = null;
            if (Arrow != null && Arrow.IsInitialized())
            {
                Arrow.Dispose();
            }
            Arrow = null;

            if (Pzt_Region != null && !Pzt_Region.IsInitialized())
            {
                Pzt_Region = null;
            }
            using (Stream objectStream = new MemoryStream())
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objectStream, this);
            }
        }
        public void Close()
        {
            if (Pzt_Region_Affine != null && Pzt_Region_Affine.IsInitialized())
            {
                Pzt_Region_Affine.Dispose();
            }
            Pzt_Region_Affine = null;
            if (Arrow != null && Arrow.IsInitialized())
            {
                Arrow.Dispose();
            }
            Arrow = null;

            if (Pzt_Region != null && Pzt_Region.IsInitialized())
            {
                Pzt_Region.Dispose();
            }
            Pzt_Region = null;

            Pzt_ROIList.Clear();
            Pzt_ROIList = null;
        }
        public void Reset()
        {
            Pzt_ROIList = new List<ROI>();

            if (Pzt_Region != null && !Pzt_Region.IsInitialized())
            {
                Pzt_Region.Dispose();
            }
            Pzt_Region = null;
        }

    }
}
