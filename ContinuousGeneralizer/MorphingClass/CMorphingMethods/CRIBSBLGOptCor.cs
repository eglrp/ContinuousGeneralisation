using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using MorphingClass.CCorrepondObjects;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>
    /// ���ʹ�ö��������ṹ(Recursive Independent Bend Structures)����ʹ��BLG����OptCor
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CRIBSBLGOptCor
    {
        
        
        private CTriangulator _Triangulator = new CTriangulator();
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private double _dblCDTNum = 1;   //���ڱ�����Ҫ��δ���CDT��Ϊ��ʹ�����ɵ�CDT���������������𣬹��ô˱���

        private CParameterInitialize _ParameterInitialize;

        public CRIBSBLGOptCor()
        {

        }

        public CRIBSBLGOptCor(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CRIBSBLGOptCor(CParameterInitialize ParameterInitialize)
        {
            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            //�������Ҫ��ͼ��
            IFeatureLayer pBSFLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLargerScaleLayer.SelectedIndex);
                                                                       
            //С������Ҫ��ͼ��
            IFeatureLayer pSSFLayer =(IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboSmallerScaleLayer.SelectedIndex);
                                                           


            ParameterInitialize.pBSFLayer = pBSFLayer;
            ParameterInitialize.pSSFLayer = pSSFLayer;
            ParameterInitialize.intMaxBackK = Convert.ToInt32(ParameterInitialize.txtMaxBackK.Text);
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            _LSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        //����������Morphing����
        public void RIBSBLGOptCorMorphing()
        {
            //var ParameterInitialize = _ParameterInitialize;
            //CMPBBSL OptMPBBSL = new CMPBBSL();

            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            //_FromCpl = frcpl;
            //_ToCpl = tocpl;

            ////���㼫Сֵ
            //
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��

            ////������ֵ����
            //double dblBound = 0.95;
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblAngleBound = 0.262;
            //ParameterThreshold.dblDLengthBound = dblBound;
            //ParameterThreshold.dblULengthBound = 1 / dblBound;
            //ParameterThreshold.dblVerySmall = dblVerySmall;

            ////**************RIBS������������**************//
            //CRIBS OptRIBS = new CRIBS(frcpl, tocpl);
            //C5.LinkedList<CCorrSegment> pComplexCorrespondSegmentLk = OptRIBS.DWByRIBSMorphing(ParameterInitialize, ParameterThreshold);

            ////**************BLG����������**************//
            //CMPBDPBL OptMPBDPBL = new CMPBDPBL();//����
            //C5.LinkedList<CCorrSegment> pBLGCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            ////�ȶ������߶ν��д���
            //pBLGCorrespondSegmentLk.AddRange(OptMPBDPBL.DWByDPLADefine(frcpl, tocpl, ParameterThreshold));
            ////�ٶ������������׶λ�õĶ�Ӧ�߶ν��зֶδ���
            //for (int j = 0; j < pComplexCorrespondSegmentLk.Count; j++)
            //{
            //    pBLGCorrespondSegmentLk.AddRange(OptMPBDPBL.DWByDPLADefine(pComplexCorrespondSegmentLk[j].CFrPolyline, pComplexCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //}

            ////��ȡ���ڽṹ�ָ��Ķ�Ӧ�߶�
            //C5.LinkedList<CCorrSegment> pCorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, pBLGCorrespondSegmentLk);

            ////**************OptCor��������**************//
            //COptCor OptOptCor = new COptCor();
            //C5.LinkedList<CCorrSegment> pOptCorCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //for (int j = 0; j < pCorrespondSegmentLk.Count; j++)
            //{
            //    List<CPolyline> CFrEdgeLt = CGeoFunc.CreateCplLt(pCorrespondSegmentLk[j].CFrPolyline.CptLt);
            //    List<CPolyline> CToEdgeLt = CGeoFunc.CreateCplLt(pCorrespondSegmentLk[j].CToPolyline.CptLt);

            //    pOptCorCorrespondSegmentLk.AddRange(OptOptCor.DWByOptCor(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, ParameterInitialize.intMaxBackK));
            //}

            ////���¸���
            //pCorrespondSegmentLk = pOptCorCorrespondSegmentLk;

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //List<CPoint> pResultPtLt = new List<CPoint>();
            //pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, "CRIBSBLGOptCorControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "CRIBSBLGOptCorCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //ParameterResult.pCorrespondSegmentLk = pCorrespondSegmentLk;
            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;
        }

        /// <summary>���ԣ��������</summary>
        public CParameterInitialize ParameterInitialize
        {
            get { return _ParameterInitialize; }
            set { _ParameterInitialize = value; }
        }

        /// <summary>���ԣ��������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}