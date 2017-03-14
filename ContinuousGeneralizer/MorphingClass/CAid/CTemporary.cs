﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;
using MorphingClass.CMorphingMethods.CMorphingMethodsBase;

namespace MorphingClass.CAid
{
    public class CTemporary : CMorphingBaseCpl
    {
        public CTemporary()
        {

        }


        public CTemporary(CParameterInitialize ParameterInitialize, int intLayerCount = 2)
        {
            Construct<CPolyline, CPolyline>(ParameterInitialize, intLayerCount);

        }


        public void Temporary()
        {
            CParameterInitialize ParameterInitialize = _ParameterInitialize;

            var pLSCPlLt = this.ObjCGeoLtLt[0].AsExpectedClass<CPolyline, object>().ToList();
            //var pSSCPlLt = this.ObjCGeoLtLt[1].AsExpectedClass<CPolyline, object>().ToList();

            var subLSCplLt = new List<CPolyline>(pLSCPlLt.Count / 2);
            var subSSCplLt = new List<CPolyline>(pLSCPlLt.Count / 2);

            int intI = 0;
            while (intI < pLSCPlLt.Count)
            {
                subLSCplLt.Add(pLSCPlLt[intI++]);
                subSSCplLt.Add(pLSCPlLt[intI++]);
            }


            CSaveFeature.SaveCGeoEb(subLSCplLt, esriGeometryType.esriGeometryPolyline, "InterSgLS_203", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            CSaveFeature.SaveCGeoEb(subSSCplLt, esriGeometryType.esriGeometryPolyline, "InterSgSS_203", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

        }
    }
}
