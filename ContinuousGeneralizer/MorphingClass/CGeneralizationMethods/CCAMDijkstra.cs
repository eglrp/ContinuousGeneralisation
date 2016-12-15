﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using MorphingClass.CAid;
using MorphingClass.CEntity;
using MorphingClass.CMorphingMethods;
using MorphingClass.CMorphingMethods.CMorphingMethodsBase;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CGeometry.CGeometryBase;
using MorphingClass.CCorrepondObjects;

using ILOG.Concert;
using ILOG.CPLEX;

namespace MorphingClass.CGeneralizationMethods
{
    /// <summary>
    /// Continuous Aggregation of Maps based on Dijkstra: CAMDijkstra
    /// </summary>
    /// <remarks></remarks>
    public class CCAMDijkstra : CMorphingBaseCpg
    {
        public static double dblLamda = 0.5;
        //public static double dblLamda2 = 1 - dblLamda1;

        //for some prompt settings
        private int _intFactor = 2;
        //private int _intFactor = 8;
        private void UpdateStartEnd(ref int intStart, ref int intEnd)
        {
            //intStart = 77;
            //intEnd = intStart + 1;

            dblLamda = 0.5;
            //dblLamda2 = 1 - dblLamda1;
        }

        public List<CRegion> InitialCrgLt { set; get; }
        //public int intTotalTimeNum { set; get; } //Note that intTotalTimeNum may count a step that only changes the type of a polygon (without aggregation)
        
        private double[,] _adblTD;
        private CPairVal_SD<int, int> _TypePVSD;

        public double dblCost { set; get; }
        public CStrObjLtSD StrObjLtSD { set; get; }

        //if we change the list, we may need to change the comparer named CAACCompare
        public static IList<string> strKeyLt = new List<string>
        {
            "ID",
            "n",
            "m",
            "Factor",
            "#Edges",
            "#Nodes",
            "EstType",
            "CostType",
            "RatioTypeCE",
            "EstComp",
            "CostComp",
            "RatioCompCE",
            "RatioTypeComp",
            "WeightedSum",
            "TimeFirst(ms)",
            "TimeLast(ms)",
            "Time(ms)",
            "Memory(MB)"
        };

        private static int _intDigits = 6;

        private static int _intStart = 0;
        private static int _intEnd = _intStart+1;




        #region Preprocessing
        public CCAMDijkstra()
        {

        }

        public CCAMDijkstra(CParameterInitialize ParameterInitialize, string strSpecifiedFieldName = null, string strSpecifiedValue = null)
        {
            Construct<CPolygon, CPolygon>(ParameterInitialize, 2, 0, true,1, strSpecifiedFieldName, strSpecifiedValue);
            CConstants.strShapeConstraint = ParameterInitialize.cboShapeConstraint.Text;
            if (CConstants.strShapeConstraint == "MaximizeMinimumCompactness" || CConstants.strShapeConstraint == "MaximizeMinimumCompactness_Combine")
            {
                CConstants.blnComputeCompactness = true;
            }

            if (ParameterInitialize.chkSmallest.Checked == true)
            {
                ParameterInitialize.strAreaAggregation = "Smallest";
            }
            else
            {
                ParameterInitialize.strAreaAggregation = "All";
            }

            //read type distance
            var aObj = CHelperFunctionExcel.ReadDataFromExcel(ParameterInitialize.strPath + "TypeDistance.xlsx");
            if (aObj == null) throw new ArgumentNullException("Failed to read TypeDistance.xlsx");

            int intDataRow = aObj.GetUpperBound(0);
            int intDataCol = intDataRow;  //note that intDataRow == intDataCol

            //set an index for each type, so that we can access a type distance directly
            //var intTypeIndexSD = new SortedDictionary<int, int>();
            var pTypePVSD = new CPairVal_SD<int, int>();
            int intTypeIndex = 0;
            for (int i = 0; i < intDataRow; i++)
            {
                int intType = Convert.ToInt32(aObj[i + 1][0]);
                if (pTypePVSD.SD.ContainsKey(intType) == false)
                {
                    pTypePVSD.SD.Add(intType, intTypeIndex++);
                }
            }
            pTypePVSD.CreateSD_R();
            _TypePVSD = pTypePVSD;

            var adblTypeDistance = new double[intDataRow, intDataCol];
            for (int i = 0; i < intDataRow; i++)
            {
                for (int j = 0; j < intDataCol; j++)
                {
                    adblTypeDistance[i, j] = Convert.ToDouble(aObj[i + 1][j + 1]);
                }
            }

            _adblTD = adblTypeDistance;
        }


        public void CAMDijkstra(int intQuitCount, string strMethod)
        {
            Stopwatch pStopwatch = Stopwatch.StartNew();
            CParameterInitialize pParameterInitialize = _ParameterInitialize;



            var pLSCPgLt = this.ObjCGeoLtLt[0].ToExpectedClass<CPolygon, object>().ToList();
            var pSSCPgLt = this.ObjCGeoLtLt[1].ToExpectedClass<CPolygon, object>().ToList();

            //this.intTotalTimeNum = pLSCPgLt.Count - pSSCPgLt.Count + 1;

            foreach (var cpg in pLSCPgLt)
            {
                cpg.FormCEdgeLtLt();
                cpg.SetCEdgeLtLtLength();
            }

            foreach (var cpg in pSSCPgLt)
            {
                cpg.FormCEdgeLtLt();
            }

            //get region number for each polygon
            var pstrFieldNameLtLt = this.strFieldNameLtLt;
            var pObjValueLtLtLt = this.ObjValueLtLtLt;
            //var intTypeIndexSD=_intTypeIndexSD;

            var intLSTypeATIndex = CSaveFeature.FindFieldNameIndex(pstrFieldNameLtLt[0], "OBJART");  //RegionNumATIndex: the index of RegionNum in the attribute table 
            var intSSTypeATIndex = CSaveFeature.FindFieldNameIndex(pstrFieldNameLtLt[1], "OBJART");
            //var CgbEb=pLSCPgLk.ToExpectedClass<CGeometricBase<CPolygon>, CGeometricBase<CPolygon>>();
            CHelperFunction.GetCgbTypeAndTypeIndex(pLSCPgLt.ToExpectedClass<CPolygon, CPolygon>(), _ObjValueLtLtLt[0], 0, _TypePVSD);
            CHelperFunction.GetCgbTypeAndTypeIndex(pSSCPgLt.ToExpectedClass<CPolygon, CPolygon>(), _ObjValueLtLtLt[1], 0, _TypePVSD);


            var intLSRegionNumATIndex = CSaveFeature.FindFieldNameIndex(pstrFieldNameLtLt[0], "RegionNum");  //RegionNumATIndex: the index of RegionNum in the attribute table 
            var intSSRegionNumATIndex = CSaveFeature.FindFieldNameIndex(pstrFieldNameLtLt[1], "RegionNum");
            //private CPairVal_SD<int, int> _RegionPVSD;
            var pRegionPVSD = new CPairVal_SD<int, int>();
            int intRegionIndex = 0;
            for (int i = 0; i < pObjValueLtLtLt[1].Count; i++)
            {
                int intRegionNum = Convert.ToInt32(pObjValueLtLtLt[1][i][intSSRegionNumATIndex]);
                if (pRegionPVSD.SD.ContainsKey(intRegionNum) == false)
                {
                    pRegionPVSD.SD.Add(intRegionNum, intRegionIndex++);
                }
            }

            //ssign the polygons as well as attributes from a featureLayer into regions, without considering costs
            var LSCrgLt = GenerateCrgLt(pLSCPgLt, pSSCPgLt.Count, pObjValueLtLtLt[0], intLSTypeATIndex, intLSRegionNumATIndex, _TypePVSD, pRegionPVSD);
            var SSCrgLt = GenerateCrgLt(pSSCPgLt, pSSCPgLt.Count, pObjValueLtLtLt[1], intSSTypeATIndex, intSSRegionNumATIndex, _TypePVSD, pRegionPVSD);

            using (var writer = new System.IO.StreamWriter(_ParameterInitialize.strSavePathBackSlash + CHelperFunction.GetTimeStamp() 
                + "_" + "AreaAggregation.txt", false))
            {
                writer.Write(_ParameterInitialize.strAreaAggregation);
            }

            //apply A* algorithm to each region
            this.InitialCrgLt = new List<CRegion>(pSSCPgLt.Count);
            var ResultCrgLt = new List<CRegion>(pSSCPgLt.Count);
            this.StrObjLtSD = new CStrObjLtSD(CCAMDijkstra.strKeyLt, pSSCPgLt.Count);

            int intStart = 0;
            int intEnd = SSCrgLt.Count;

            UpdateStartEnd(ref intStart, ref intEnd);


             switch (strMethod)
             {
                 case "AStar":
                     CRegion._lngEstimationCountEdgeNumber = 0;
                     CRegion._lngEstimationCountEdgeLength = 0;
                     CRegion._lngEstimationCountEqual = 0;
                     
                     for (int i = intStart; i < intEnd; i++)
                     {
                         ResultCrgLt.Add(AStar(LSCrgLt[i], SSCrgLt[i], this.StrObjLtSD, _ParameterInitialize.strAreaAggregation, intQuitCount));
                     }
                     Console.WriteLine();
                     Console.WriteLine("Estimation functions that we used:");
                     Console.WriteLine("By EdgeNumber: " + CRegion._lngEstimationCountEdgeNumber +
                         ";   By EdgeLength: " + CRegion._lngEstimationCountEdgeLength +
                         ";   EqualCases: " + CRegion._lngEstimationCountEqual);
                     break;
                 case "ILP":
                     for (int i = intStart; i < intEnd; i++)
                     {
                         ResultCrgLt.Add(ILP(LSCrgLt[i], SSCrgLt[i], this.StrObjLtSD, this._adblTD, _ParameterInitialize.strAreaAggregation));
                     }
                     break;
                 case "ILP_Extend":
                     //update the address stored in Path.txt
                     using (var writer = new System.IO.StreamWriter(
                         "C:\\Study\\Programs\\ContinuousGeneralizer\\ContinuousGeneralizer64\\InputPath\\Path.txt", false))
                     {
                         writer.Write(_ParameterInitialize.strSavePath);
                     }
                     ExportadblTD(_ParameterInitialize.strSavePath, this._adblTD);
                     for (int i = intStart; i < intEnd; i++)
                     {
                         ResultCrgLt.Add(ILP_Extend(LSCrgLt[i], SSCrgLt[i], this._adblTD));
                     }
                     //RunContinuousGeneralizer64();
                     break;
                 default:
                     break;
             }


             //this.intTotalTimeNum = pLSCPgLt.Count - pSSCPgLt.Count + 1;

            //for (int i = 0; i < pSSCPgLt.Count; i++)
            //for (int i = 514; i < 515; i++)
            //{
            //    Stopwatch pStopwatch2 = new Stopwatch();
            //    pStopwatch2.Start();
            //    switch (strMethod)
            //    {
            //        case "AStar": ResultCrgLt.Add(AStar(LSCrgLt[i], SSCrgLt[i], intQuitCount));
            //            break;
            //        case "ILP": ResultCrgLt.Add(ILP(LSCrgLt[i], SSCrgLt[i], this._adblTD));
            //            break;
            //        case "ILP_Extend":
            //            //update the address stored in Path.txt
            //            using (var writer = new System.IO.StreamWriter(
            //                "C:\\Study\\Programs\\ContinuousGeneralizer\\ContinuousGeneralizer_ILP\\TempFiles\\Path.txt", false))
            //            {
            //                writer.Write(_ParameterInitialize.strSavePath);
            //            }
            //            ResultCrgLt.Add(ILP_Extend(LSCrgLt[i], SSCrgLt[i], this._adblTD));
            //            break;
            //        default:
            //            break;
            //    }
            //    long lngtime = pStopwatch2.ElapsedMilliseconds;
            //    pParameterInitialize.tsslTime.Text = lngtime.ToString();

            //    //pStopwatch2.Start();
            //}

            //foreach (var crg in ResultCrgLt)
            //{
            //    this.dblCost += crg.dblCostExact;
            //}

            //pParameterInitialize.tsslTime.Text = "Running Time: " + pStopwatch.ElapsedMilliseconds.ToString();

        }
        #endregion

        #region ILP
        public CRegion ILP(CRegion LSCrg, CRegion SSCrg, CStrObjLtSD StrObjLtSD, double[,] adblTD, string strAreaAggregation)
        {
            double dblMemoryInMB1 = CHelperFunction.GetConsumedMemoryInMB(true);

            LSCrg.SetInitialAdjacency();  //also count the number of edges

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("Crg:  ID  " + LSCrg.ID + ";    n  " + LSCrg.CphTypeIndexSD_Area_CphGID.Count + ";    m  " +
                LSCrg.Adjacency_CorrCphsSD.Count + "  " + _ParameterInitialize.strAreaAggregation + "================================="
                + LSCrg.ID + "  " + _ParameterInitialize.strAreaAggregation + "==============================");
            

            Stopwatch pStopwatch = new Stopwatch();
            pStopwatch.Start();
            AddLineToStrObjLtSD(StrObjLtSD, LSCrg);

            double dblMemoryInMB2 = CHelperFunction.GetConsumedMemoryInMB(true);
            Cplex cplex = new Cplex();

            double dblMemoryInMB3 = CHelperFunction.GetConsumedMemoryInMB(true);

            CRegion crg = new CRegion(-1);
            bool blnSoloved = true;
            try
            {
                //Step 3
                //Cplex cplex = new Cplex();
                IIntVar[][][] var2;
                IIntVar[][][][] var3;
                IIntVar[][][][][] var4;
                IRange[][] rng;

                PopulateByRow(cplex, out var2, out var3, out var4, out rng, LSCrg, SSCrg, adblTD, strAreaAggregation);
                double dblMemoryInMB4 = CHelperFunction.GetConsumedMemoryInMB(true);
                // Step 11
                //cplex.ExportModel("lpex1.lp");

                // Step 9
                cplex.SetParam(Cplex.DoubleParam.TiLim, 1170);  //1170 is from _All_MinimizeInteriorBoundaries_200000000
                //cplex.SetParam(Cplex.IntParam.ParallelMode, 1);
                //cplex.SetParam(Cplex.ParallelMode.Deterministic,cplex.pa);

                //ILOG.Concert.

                if (cplex.Solve())
                {

                    //***********Gap for ILP************

                    #region Display x, y, z, and s
                    //for (int i = 0; i < var3[0].GetLength(0); i++)
                    //{

                    //    Console.WriteLine("Variable x; Time: " + (i + 1).ToString());

                    //    foreach (var x1 in var3[0][i])
                    //    {
                    //        //CPatch 



                    //        double[] x = cplex.GetValues(x1);


                    //        foreach (var x0 in x)
                    //        {
                    //            Console.Write(x0 + "     ");
                    //        }
                    //        Console.WriteLine();

                    //    }
                    //    Console.WriteLine();


                    //}

                    #region Display y and z
                    //if (var4[0] != null)
                    //{
                    //    Console.WriteLine("");
                    //    //Console.WriteLine("Variable y:");
                    //    for (int i = 0; i < var4[0].GetLength(0); i++)
                    //    {
                    //        Console.WriteLine("Variable y; Time: " + (i + 1).ToString());
                    //        foreach (var y2 in var4[0][i])
                    //        {
                    //            foreach (var y1 in y2)
                    //            {

                    //                double[] y = cplex.GetValues(y1);


                    //                foreach (var y0 in y)
                    //                {
                    //                    Console.Write(y0 + "     ");
                    //                }
                    //                Console.WriteLine();
                    //            }

                    //            Console.WriteLine();
                    //        }
                    //        //Console.WriteLine();
                    //    }
                    //}

                    //if (var4[1] != null)
                    //{
                    //    Console.WriteLine("");
                    //    //Console.WriteLine("Variable z:");
                    //    for (int i = 0; i < var4[1].GetLength(0); i++)
                    //    {
                    //        Console.WriteLine("Variable z; Time: " + (i + 1).ToString());
                    //        foreach (var z2 in var4[1][i])
                    //        {
                    //            foreach (var z1 in z2)
                    //            {

                    //                double[] z = cplex.GetValues(z1);


                    //                foreach (var z0 in z)
                    //                {
                    //                    Console.Write(z0 + "     ");
                    //                }
                    //                Console.WriteLine();

                    //            }
                    //            Console.WriteLine();
                    //        }
                    //        //Console.WriteLine();
                    //    }
                    //}
                    #endregion


                    //if (_ParameterInitialize.strAreaAggregation == "Smallest")
                    //{
                    //    Console.WriteLine("");
                    //    Console.WriteLine("Variable s:");
                    //    if (var2[0] != null)
                    //    {
                    //        for (int i = 0; i < var2[0].GetLength(0); i++)
                    //        {


                    //            double[] s = cplex.GetValues(var2[0][i]);


                    //            foreach (var s0 in s)
                    //            {
                    //                Console.Write(s0 + "     ");
                    //            }
                    //            Console.WriteLine();

                    //        }
                    //    }
                    //}
                    #endregion

                    #region Display other results
                    //double[] dj = cplex.GetReducedCosts(var3[0][0][0]);
                    //double[] dj2 = cplex.GetReducedCosts((var3);
                    //double[] pi = cplex.GetDuals(rng[0]);
                    //double[] slack = cplex.GetSlacks(rng[0]);
                    //Console.WriteLine("");
                    //cplex.Output().WriteLine("Solution status = "
                    //+ cplex.GetStatus());
                    //cplex.Output().WriteLine("Solution value = "
                    //+ cplex.ObjValue);
                    //objDataLt[13] = cplex.ObjValue;
                    //int nvars = x.Length;
                    //for (int j = 0; j < nvars; ++j)
                    //{
                    //    cplex.Output().WriteLine("Variable :"
                    //    + j
                    //    + " Value = "
                    //    + x[j]
                    //    + " Reduced cost = "
                    //    + dj[j]);
                    //}
                    //int ncons = slack.Length;
                    //for (int i = 0; i < ncons; ++i)
                    //{
                    //    cplex.Output().WriteLine("Constraint:"
                    //    + i
                    //    + " Slack = "
                    //    + slack[i]
                    //    //+ " Pi = "
                    //    //+ pi[i]
                    //    );
                    //}
                    #endregion
                    
                }

                Console.WriteLine("");
                cplex.Output().WriteLine("Solution status = "
                + cplex.GetStatus());
                cplex.Output().WriteLine("Solution value = "
                + cplex.ObjValue);
                string strStatus = cplex.GetStatus().ToString();
                //StrObjLtSD.SetLastObj("#Edges", strStatus);
                StrObjLtSD.SetLastObj("WeightedSum", cplex.ObjValue);

                if (strStatus == "Optimal")
                {
                    StrObjLtSD.SetLastObj("Factor", 1);
                }
                else if (strStatus == "Feasible")
                {
                    StrObjLtSD.SetLastObj("Factor", 5000);
                }
            }
            catch (ILOG.Concert.Exception e)
            {
                blnSoloved = false;
                System.Console.WriteLine("Concert exception '" + e + "' caught");
            }
            catch (System.OutOfMemoryException e2)
            {
                blnSoloved = false;
                System.Console.WriteLine("System exception '" + e2);
            }
            finally
            {
                double dblMemoryInMB = CHelperFunction.GetConsumedMemoryInMB(false);
                if (blnSoloved == false)
                {
                    crg.ID = -2;
                    System.Console.WriteLine("We have used memory " + dblMemoryInMB + "MB.");
                    Console.WriteLine("Crg:  ID  " + LSCrg.ID + ";    n  " + LSCrg.CphTypeIndexSD_Area_CphGID.Count + ";    m  " + 
                        LSCrg.Adjacency_CorrCphsSD.Count + "  could not be solved by ILP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                StrObjLtSD.SetLastObj("TimeLast(ms)", pStopwatch.ElapsedMilliseconds);
                StrObjLtSD.SetLastObj("Time(ms)", pStopwatch.ElapsedMilliseconds);
                StrObjLtSD.SetLastObj("Memory(MB)", dblMemoryInMB);
                cplex.End();
            }



            return crg;
        }

        // Step 4
        internal static void PopulateByRow(IMPModeler model,out
        IIntVar[][][] var2, out
        IIntVar[][][][] var3, out IIntVar[][][][][] var4, out
        IRange[][] rng, CRegion lscrg, CRegion sscrg, double[,] adblTD, string strAreaAggregation)
        {
            var aCph = lscrg.CphTypeIndexSD_Area_CphGID.Keys.ToArray();
            int intCpgCount = lscrg.CphTypeIndexSD_Area_CphGID.Count;
            double dblILPSmallValue = 0;
            
            IIntVar[][][] x = new IIntVar[intCpgCount][][];
            for (int i = 0; i < intCpgCount; i++)
            {
                x[i] = new IIntVar[intCpgCount][];
                for (int j = 0; j < intCpgCount; j++)
                {
                    x[i][j] = model.BoolVarArray(intCpgCount);
                }
            }

            //cost in terms of type change
            var y = Generate4DNumVar(model, intCpgCount - 1, intCpgCount, intCpgCount, intCpgCount);

            //cost in terms of compactness (length of interior boundaries)
            var z = Generate4DNumVar(model, intCpgCount - 2, intCpgCount, intCpgCount, intCpgCount);


            var3 = new IIntVar[1][][][];
            var4 = new IIntVar[2][][][][];
            var3[0] = x;
            var4[0] = y;
            var4[1] = z;

            //add minimizations
            ILinearNumExpr pTypeCostExpr = model.LinearNumExpr();
            //ILinearNumExpr pTypeCostAssitantExpr = model.LinearNumExpr();
            for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int k = 0; k < intCpgCount; k++)
                    {
                        for (int l = 0; l < intCpgCount; l++)
                        {
                            double dblCoe = aCph[j].dblArea * adblTD[aCph[k].intTypeIndex, aCph[l].intTypeIndex];
                            pTypeCostExpr.AddTerm(y[i][j][k][l], dblCoe);
                            //pTypeCostAssitantExpr.AddTerm(y[i][j][k][l], dblILPSmallValueMinimization);
                        }
                    }
                }
            }

            double dblCompCostFirstPart = 1 / Convert.ToDouble(intCpgCount - 1);  //this is actually for t=1, whose compactness is known
            ILinearNumExpr pCompCostSecondPartExpr = model.LinearNumExpr();
            var pAdjacency_CorrCphsSD = lscrg.Adjacency_CorrCphsSD;
            double dblConst = Convert.ToDouble(intCpgCount - 2) / Convert.ToDouble(intCpgCount - 1);
            
            for (int i = 2; i < intCpgCount; i++)   //i represents time
            {
                double dblNminusT = intCpgCount - i;
                double dblTemp = (intCpgCount - i) * dblConst;
                dblCompCostFirstPart += 1 / dblNminusT;
                double dblNorm = lscrg.dblInteriorSegmentLength * dblNminusT;

                foreach (var pCorrCphs in pAdjacency_CorrCphsSD.Keys)  //we don't need to divide the value by 2 because every boundary is only counted once
                {
                    for (int l = 0; l < intCpgCount; l++)
                    {
                        pCompCostSecondPartExpr.AddTerm(pCorrCphs.dblSharedSegmentLength / dblNorm, z[i - 2][pCorrCphs.FrCph.ID][pCorrCphs.ToCph.ID][l]);
                    }
                }
            }

            if (intCpgCount == 1)
            {
                model.AddMinimize(pTypeCostExpr);  //we just use an empty expression
            }
            else
            {
                //Our Model***************************************
                var Ftp = model.Prod(pTypeCostExpr, 1 / lscrg.dblArea);
                var Fcp = model.Sum(dblCompCostFirstPart, model.Negative(pCompCostSecondPartExpr));
                model.AddMinimize(model.Prod(model.Sum(Ftp, Fcp), 0.5));
            }




            //constraints
            IList<IRange> IRangeLt = new List<IRange>();
            //to restrict y
            for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int k = 0; k < intCpgCount; k++)
                    {
                        //IRangeLt.Add(model.AddLe(model.Sum(y[i][j][k][k], x[i][j][k], x[i + 1][j][k]), 2.0 , "RestrictY"));

                        for (int l = 0; l < intCpgCount; l++)
                        {
                            if (k != l)
                            {
                                IRangeLt.Add(model.AddLe(model.Sum(model.Negative(y[i][j][k][l]), x[i][j][k], x[i + 1][j][l]), 1.0 + dblILPSmallValue, "RestrictY"));
                            }
                        }
                    }
                }
            }

            //model.
            //to restrict z
            for (int i = 0; i < intCpgCount - 2; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int k = j; k < intCpgCount; k++)
                    {
                        for (int l = 0; l < intCpgCount; l++)
                        {
                            IRangeLt.Add(model.AddLe(model.Sum(z[i][j][k][l], model.Negative(x[i + 1][j][l])), dblILPSmallValue, "RestrictZ1"));
                            IRangeLt.Add(model.AddLe(model.Sum(z[i][j][k][l], model.Negative(x[i + 1][k][l])), dblILPSmallValue, "RestrictZ2"));

                            //z[i][j][k][l]=z[i][k][j][l]
                            IRangeLt.Add(model.AddEq(model.Sum(z[i][j][k][l], model.Negative(z[i][k][j][l])), 0.0, "RestrictZ3"));
                        }
                    }
                }
            }

            //a polygon $p$ is assigned to exactly one polygon at a step $\tau$
            for (int i = 0; i < intCpgCount; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    ILinearNumExpr pOneCenterExpr = model.LinearNumExpr();
                    for (int l = 0; l < intCpgCount; l++)
                    {
                        pOneCenterExpr.AddTerm(x[i][j][l], 1.0);
                    }

                    IRangeLt.Add(model.AddEq(pOneCenterExpr, 1.0, "AssignToOnePg"));
                }
            }

            //a polygon must be assigned to a center
            for (int i = 0; i < intCpgCount; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int l = 0; l < intCpgCount; l++)
                    {
                        IRangeLt.Add(model.AddLe(model.Sum(x[i][j][l], model.Negative(x[i][l][l])), dblILPSmallValue, "AssignedIsCenter__" + i + "__" + j + "__" + l));
                    }
                }
            }


            //If two polygons have been aggregated into one polygon, then they will 
            //be aggregated together in later steps. Our sixth constraint achieve this by requiring
            for (int i = 1; i < intCpgCount - 2; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int k = 0; k < intCpgCount; k++)
                    {
                        ILinearNumExpr pAssignTogetherExprPre = model.LinearNumExpr();
                        ILinearNumExpr pAssignTogetherExprAfter = model.LinearNumExpr();
                        for (int l = 0; l < intCpgCount; l++)
                        {
                            pAssignTogetherExprPre.AddTerm(z[i - 1][j][k][l], 1.0);
                            pAssignTogetherExprAfter.AddTerm(z[i][j][k][l], -1.0);
                        }
                        IRangeLt.Add(model.AddLe(model.Sum(pAssignTogetherExprPre, pAssignTogetherExprAfter), 0.0, "AssignTogether"));
                    }
                }
            }

            //polygon $p$ can be assigned to center $o$ if at least one of $p$'s neighbors has already been assigned to center $o$            
            for (int i = 1; i < intCpgCount - 1; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    for (int k = 0; k < intCpgCount; k++)
                    {
                        if (j==k)  //the considered point is free to be assigned to itself
                        {
                            continue;
                        }

                        ILinearNumExpr pContiguityExpr = model.LinearNumExpr();
                        //pContiguityExpr.AddTerm(x[i][j][k], 1.0);  //including polygon j itself
                        foreach (var pAdjacentCph in aCph[j].AdjacentCphSS)
                        {
                            pContiguityExpr.AddTerm(x[i][pAdjacentCph.ID][k], 1.0);
                        }
                        IRangeLt.Add(model.AddLe(model.Sum(x[i][j][k], model.Negative(pContiguityExpr)), 0.0, "Contiguity"));
                    }
                }
            }

            //only one patch is aggregated into another patch at each step
            for (int i = 0; i < intCpgCount; i++)   //i represents indices
            {
                ILinearNumExpr pOneAggregationExpr = model.LinearNumExpr();
                for (int j = 0; j < intCpgCount; j++)
                {
                    pOneAggregationExpr.AddTerm(x[i][j][j], 1.0);
                }
                IRangeLt.Add(model.AddEq(pOneAggregationExpr, intCpgCount - i, "CountCenters"));
            }

            //a center can disappear, but will never reappear afterwards
            for (int i = 1; i < intCpgCount; i++)   //i represents indices
            {
                for (int j = 0; j < intCpgCount; j++)
                {
                    IRangeLt.Add(model.AddGe(model.Sum(x[i - 1][j][j], model.Negative(x[i][j][j])), 0.0, "SteadyCenters"));
                }
            }


            //to make sure that the final aggregated polygon has the same color as the target polygon
            ILinearNumExpr pFinalStateExpr = model.LinearNumExpr();
            for (int i = 0; i < intCpgCount; i++)
            {
                if (aCph[i].intTypeIndex == sscrg.CphTypeIndexSD_Area_CphGID.Keys.GetFirstT().intTypeIndex)
                {
                    pFinalStateExpr.AddTerm(x[intCpgCount - 1][i][i], 1.0);
                }
            }
            IRangeLt.Add(model.AddEq(pFinalStateExpr, 1.0, "EnsureTarget"));

            var2 = new IIntVar[1][][];
            if (strAreaAggregation == "Smallest")
            {
                //add more constraints to y
                for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
                {
                    for (int j = 0; j < intCpgCount; j++)
                    {
                        for (int k = 0; k < intCpgCount; k++)
                        {
                            for (int l = 0; l < intCpgCount; l++)
                            {
                                if (k != l)
                                {
                                    IRangeLt.Add(model.AddLe(model.Sum(y[i][j][k][l], model.Negative(x[i][j][k])), dblILPSmallValue, "RestrictY1_S"));
                                    IRangeLt.Add(model.AddLe(model.Sum(y[i][j][k][l], model.Negative(x[i + 1][j][l])), dblILPSmallValue, "RestrictY2_S"));
                                }
                            }
                        }
                    }
                }


                //add more constraints to z
                for (int i = 0; i < intCpgCount - 2; i++)   //i represents indices
                {
                    for (int j = 0; j < intCpgCount; j++)
                    {
                        for (int k = j+1; k < intCpgCount; k++)
                        {
                            for (int l = 0; l < intCpgCount; l++)
                            {
                                IRangeLt.Add(model.AddLe(model.Sum(model.Negative(z[i][j][k][l]), x[i][j][l], x[i][k][l]), 1.0 + dblILPSmallValue, "RestrictZ_S"));

                            }
                        }
                    }
                }

                IIntVar[][] s = new IIntVar[intCpgCount - 1][];
                for (int i = 0; i < intCpgCount - 1; i++)
                {
                    s[i] = model.BoolVarArray(intCpgCount);
                }
                var2[0] = s;

                //there is only one smallest patch will be involved in each aggregation step
                for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
                {
                    ILinearNumExpr pOneSmallestExpr = model.LinearNumExpr();
                    for (int j = 0; j < intCpgCount; j++)
                    {
                        pOneSmallestExpr.AddTerm(s[i][j], 1.0);
                    }

                    IRangeLt.Add(model.AddEq(pOneSmallestExpr, 1.0, "OneSmallest"));
                }

                //forces that the aggregation must involve the smallest patch.
                for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
                {
                    for (int j = 0; j < intCpgCount; j++)
                    {
                        ILinearNumExpr pInvolveSmallestExpr = model.LinearNumExpr();
                        for (int k = 0; k < intCpgCount; k++)
                        {
                            for (int l = 0; l < intCpgCount; l++)
                            {
                                if (j != k && j != l)
                                {
                                    pInvolveSmallestExpr.AddTerm(y[i][k][l][j], 1.0);
                                }

                                if (l != k && l != j)
                                {
                                    pInvolveSmallestExpr.AddTerm(y[i][k][j][l], 1.0);
                                }

                                //pInvolveSmallestExpr.AddTerm(y[i][k][l][j], 1.0);
                                //pInvolveSmallestExpr.AddTerm(y[i][k][j][l], 1.0);
                            }
                        }

                        IRangeLt.Add(model.AddLe(model.Sum(s[i][j], model.Negative(pInvolveSmallestExpr)), 0.0, "InvolveSmallest"));
                    }
                }

                //To guarantee that patch $o$ involved in aggregation is indeed the smallest patch
                for (int i = 0; i < intCpgCount - 1; i++)   //i represents indices
                {
                    var aAreaExpr = ComputeAreaExpr(model, x[i], aCph);
                    for (int j = 0; j < intCpgCount; j++)
                    {
                        for (int k = 0; k < intCpgCount; k++)
                        {
                            var pSumExpr = model.Sum(model.Negative(model.Sum(s[i][j], x[i][k][k])), 2.0);
                            var pProdExpr = model.Prod(pSumExpr, lscrg.dblArea);

                            //A_{t,o}-A_{t,r}-M(2-S_{t,o}-x_{t,r,r}) <= 0
                            IRangeLt.Add(model.AddLe(model.Sum(aAreaExpr[j], model.Negative(aAreaExpr[k]), model.Negative(pProdExpr)), 0.0, "IndeedSmallest"));
                        }
                    }
                }
            }



            //***************compare with number of constraints counted manually************
            rng=new IRange[1][];
            rng[0] = new IRange[IRangeLt.Count];
            for (int i = 0; i < IRangeLt.Count; i++)
            {
                rng[0][i] = IRangeLt[i];
            }
        }

        internal static ILinearNumExpr[] ComputeAreaExpr(IMPModeler model, IIntVar[][] x, CPatch[] aCph)
        {
            int intCpgCount = aCph.GetLength(0);
            ILinearNumExpr[] aAreaExpr = new ILinearNumExpr[intCpgCount];
            for (int i = 0; i < intCpgCount; i++)
            {
                aAreaExpr[i] = model.LinearNumExpr();
                for (int j = 0; j < intCpgCount; j++)
                {
                    aAreaExpr[i].AddTerm(x[j][i], aCph[j].dblArea);
                }
            }

            return aAreaExpr;
        }

        internal static IIntVar[][][][] Generate4DNumVar(IMPModeler model, int intCount1, int intCount2, int intCount3, int intCount4)
        {
            if (intCount1<0)
            {
                intCount1 = 0;
            }

            IIntVar[][][][] x = new IIntVar[intCount1][][][];
            for (int i = 0; i < intCount1; i++)
            {
                x[i] = new IIntVar[intCount2][][];
                for (int j = 0; j < intCount2; j++)
                {
                    x[i][j] = new IIntVar[intCount3][];
                    for (int k = 0; k < intCount3; k++)
                    {
                        x[i][j][k] = model.BoolVarArray(intCount4);
                    }
                }
            }

            return x;
        }
        #endregion

        #region ILP_Extend


        public CRegion ILP_Extend(CRegion LSCrg, CRegion SSCrg, double[,] adblTD)
        {
            var ExistingCorrCphsSD0 = LSCrg.SetInitialAdjacency();  //also count the number of edges
            var aCph = LSCrg.CphTypeIndexSD_Area_CphGID.Keys.ToArray();
            int intCpgCount = LSCrg.CphTypeIndexSD_Area_CphGID.Count;

            //create a directory for current LSCrg
            System.IO.Directory.CreateDirectory(_ParameterInitialize.strSavePath + "\\" + LSCrg.ID);

            //Output aCph: GID Area intTypeIndex {adjacentcph.ID ...}           
            string strCphs = "";
            foreach (var cph in aCph)
            {
                strCphs += cph.GID + " " + cph.dblArea + " " + cph.intTypeIndex;
                foreach (var adjacentcph in cph.AdjacentCphSS)
                {
                    strCphs += " " + adjacentcph.ID;
                }
                strCphs += "\n";
            }
            using (var writer = new StreamWriter(_ParameterInitialize.strSavePath + "\\" + LSCrg.ID + "\\" + "aCph.txt", false))
            {
                writer.Write(strCphs);
            }

            //Output LSCrg: ID Area dblInteriorSegmentLength intTargetTypeIndex {corrcphs: GID FrCph.ID ToCph.ID dblSharedSegmentLength ...}
            string strLSCrg = LSCrg.ID + " " + LSCrg.dblArea + " " + LSCrg.dblInteriorSegmentLength + " " + SSCrg.CphTypeIndexSD_Area_CphGID.GetFirstT().Key.intTypeIndex + "\n";
            foreach (var corrcphs in LSCrg.Adjacency_CorrCphsSD.Keys)
            {
                strLSCrg +=corrcphs.GID + " " + corrcphs.FrCph.ID + " " + corrcphs.ToCph.ID + " " + corrcphs.dblSharedSegmentLength + "\n";
            }
            using (var writer = new StreamWriter(_ParameterInitialize.strSavePath + "\\" + LSCrg.ID + "\\" + "LSCrg.txt", false))
            {
                writer.Write(strLSCrg);
            }

            

            //System.Console.WriteLine();
            //System.Console.WriteLine();
            //System.Console.WriteLine(LSCrg.ID + "  " + _ParameterInitialize.strAreaAggregation + "============================================="
            //    + LSCrg.ID + "  " + _ParameterInitialize.strAreaAggregation + "=================================================");
            


            return null;
        }

        private void ExportadblTD(string strSavePath, double[,] adblTD)
        {
            //Output adblTD
            string strTD = adblTD.GetLength(0) + " " + adblTD.GetLength(1) +"\n";
            for (int i = 0; i < adblTD.GetLength(0); i++)
            {
                strTD += adblTD[i, 0];
                for (int j = 1; j < adblTD.GetLength(1); j++)
                {
                    strTD += " " + adblTD[i, j];
                }
                strTD += "\n";
            }
            using (var writer = new StreamWriter(strSavePath + "\\" + "adblTD.txt", false))
            {
                writer.Write(strTD);
            }
        }

        private void RunContinuousGeneralizer64()
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = @"C:\Study\Programs\ContinuousGeneralizer\ContinuousGeneralizer64\ContinuousGeneralizer64\bin\Debug\ContinuousGeneralizer64.exe";
            process.StartInfo.Arguments = "-n";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.Start();
            process.WaitForExit();// Waits here for the process to exit.
        }
                      
        #endregion


        #region AStar
        public CRegion AStar(CRegion LSCrg, CRegion SSCrg, CStrObjLtSD StrObjLtSD, string strAreaAggregation, int intQuitCount = 200000)
        {
            var ExistingCorrCphsSD0 = LSCrg.SetInitialAdjacency();  //also count the number of edges
            
            Stopwatch pStopwatchOverHead = new Stopwatch();
            pStopwatchOverHead.Start();
            int intFactor = _intFactor;
            CRegion resultcrg = new CRegion(-2);
            CRegion._intStartStaticGIDAll = CRegion._intStaticGID;
            

            AddLineToStrObjLtSD(StrObjLtSD, LSCrg);

            long lngStartMemory = 0;
            Console.WriteLine();
            Console.WriteLine("Crg:  ID  " + LSCrg.ID + ";    n  " + LSCrg.CphTypeIndexSD_Area_CphGID.Count + ";    m  " +
                    LSCrg.Adjacency_CorrCphsSD.Count + "   " + intQuitCount + "   " + CConstants.strShapeConstraint + "   " + strAreaAggregation);

            lngStartMemory = GC.GetTotalMemory(true);
            long lngTimeOverHead = pStopwatchOverHead.ElapsedMilliseconds;
            pStopwatchOverHead.Stop();

            Stopwatch pStopwatchLast=new Stopwatch ();
            bool blnRecordTimeFirst = false;
            long lngTimeFirst = 0;
            long lngTimeLast = 0;
            long lngTimeAll = lngTimeOverHead;
            do
            {
                try
                {
                    CRegion._intNodesCount = 0;
                    CRegion._intStartStaticGIDLast = CRegion._intStaticGID;
                    pStopwatchLast.Restart();
                    var ExistingCorrCphsSD = new SortedDictionary<CCorrCphs, CCorrCphs>(ExistingCorrCphsSD0, ExistingCorrCphsSD0.Comparer);
                    LSCrg.cenumColor = CEnumColor.white;

                    resultcrg = ComputeAccordFactor(LSCrg, SSCrg, strAreaAggregation, ExistingCorrCphsSD, intFactor, StrObjLtSD, intQuitCount);
                }
                catch (System.OutOfMemoryException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (blnRecordTimeFirst == false)
                {
                    lngTimeFirst = pStopwatchLast.ElapsedMilliseconds + lngTimeOverHead;
                    blnRecordTimeFirst = true;
                }
                lngTimeLast = pStopwatchLast.ElapsedMilliseconds + lngTimeOverHead;
                lngTimeAll += pStopwatchLast.ElapsedMilliseconds;

                intFactor *= 2;
            } while (resultcrg.ID == -2);
            intFactor /= 2;
            StrObjLtSD.SetLastObj("Factor", intFactor);
            Console.WriteLine("d: " + resultcrg.d 
                + "            Type: " + resultcrg.dblCostExactType 
                + "            Compactness: " + resultcrg.dblCostExactCompactness);

            int intExploredRegionAll = CRegion._intStaticGID - CRegion._intStartStaticGIDLast;  //we don't need to +1 because +1 is already included in _intStaticGID
            double dblConsumedMemoryInMB = CHelperFunction.GetConsumedMemoryInMB(false);

            StrObjLtSD.SetLastObj("#Edges", intExploredRegionAll);
            StrObjLtSD.SetLastObj("TimeFirst(ms)", lngTimeFirst);
            StrObjLtSD.SetLastObj("TimeLast(ms)", lngTimeLast);
            StrObjLtSD.SetLastObj("Time(ms)", lngTimeAll);
            StrObjLtSD.SetLastObj("Memory(MB)", CHelperFunction.GetConsumedMemoryInMB(false, lngStartMemory));

            Console.WriteLine("Factor:" + intFactor + "      We have visited " + intExploredRegionAll + " Regions.");

            return resultcrg;
        }

        private CRegion ComputeAccordFactor(CRegion LSCrg, CRegion SSCrg, string strAreaAggregation,
            SortedDictionary<CCorrCphs, CCorrCphs> ExistingCorrCphsSD, int intFactor, CStrObjLtSD StrObjLtSD, int intQuitCount = 200000)
        {
            int intRegionID = LSCrg.ID;  //all the regions generated in this function will have the same intRegionID
            int intSSTypeIndex = SSCrg.CphTypeIndexSD_Area_CphGID.GetFirstT().Value;

            LSCrg.InitialEstimatedCost(SSCrg, _adblTD, intFactor);
            //LSCrg.SetCoreCph(intSSTypeIndex);

            //a region represents a node in graph, ExistingCrgSD stores all the nodes
            var ExistingCphSDLt = new List<SortedDictionary<CPatch, CPatch>>(LSCrg.CphTypeIndexSD_Area_CphGID.Count + 1);  //we use this dictionary to make sure that if the two patches have the same cpgs, then they have the same GID
            for (int i = 0; i < ExistingCphSDLt.Capacity; i++)
            {
                var Element = new SortedDictionary<CPatch, CPatch>(CPatch.pCompareCPatch_CpgGID);
                ExistingCphSDLt.Add(Element);
            }

            var ExistingCrgSDLt = new List<SortedDictionary<CRegion, CRegion>>(LSCrg.CphTypeIndexSD_Area_CphGID.Count + 1);
            for (int i = 0; i < ExistingCrgSDLt.Capacity; i++)
            {
                var Element = new SortedDictionary<CRegion, CRegion>(CRegion.pCompareCRegion_CphGIDTypeIndex);  //we don't compare exact cost first because of there may be rounding problems 
                ExistingCrgSDLt.Add(Element);
            }
            ExistingCrgSDLt[LSCrg.CphTypeIndexSD_Area_CphGID.Count].Add(LSCrg, LSCrg);

            var FinalOneCphCrg = new CRegion(intRegionID);
            var Q = new SortedSet<CRegion>(CRegion.pCompareCRegion_Cost_CphGIDTypeIndex);
            int intCount = 0;
            Q.Add(LSCrg);
            while (true)
            {
                intCount++;
                var u = Q.Min;
                if (Q.Remove(u) == false)
                {
                    throw new ArgumentException("cannot move an element in this queue! A solution might be make dblVerySmall smaller!");
                }

                //List<CRegion> crgcol = new List<CRegion>();
                //crgcol.Add(u);

                //OutputMap(crgcol, this._TypePVSD, u.d, intCount, pParameterInitialize);

                //MessageBox.Show("click for next!");

                //if (CConstants.strShapeConstraint == "MaximizeMinimumCompactness" || CConstants.strShapeConstraint == "MinimizeInteriorBoundaries")
                //{
                //    Console.WriteLine("Crg:  ID  " + u.ID + ";      GID:" + u.GID + ";      CphNum:" + u.CphTypeIndexSD_Area_CphGID.Count + ";      d:" + u.d +
                //    ";      ExactCost:" + u.dblCostExact + ";      Compactness:" + u.dblCostExactCompactness + ";      Type:" + u.dblCostExactType / u.dblArea);
                //}
                //else if (CConstants.strShapeConstraint == "NonShape")
                //{
                //    Console.WriteLine("Crg:  ID  " + u.ID + ";      GID:" + u.GID + ";      CphNum:" + u.CphTypeIndexSD_Area_CphGID.Count + ";      d:" + u.d +
                //    ";      ExactCost:" + u.dblCostExactType);
                //}

                //at the beginning, resultcrg.d is double.MaxValue. Later, when we first encounter that there is only one CPatch in LSCrg, resultcrg.d will be changed to the real cost
                //u.d contains estimation, and resultcrg.d doesn't contains. if u.d > resultcrg.d, then resultcrg.d must already be the smallest cost
                if (u.CphTypeIndexSD_Area_CphGID.Count == 1)
                {
                    FinalOneCphCrg = u;
                    break;
                }


                foreach (var newcrg in u.AggregateAndUpdateQ(LSCrg, SSCrg, Q, strAreaAggregation, ExistingCrgSDLt, ExistingCphSDLt, ExistingCorrCphsSD, intSSTypeIndex, _adblTD, intFactor))
                {
                    int intExploredRegionLast = CRegion._intStaticGID - CRegion._intStartStaticGIDLast;  //we don't need to +1 because +1 is already included in _intStaticGID

                    if (intExploredRegionLast > intQuitCount)
                    {
                        return new CRegion(-2);  //if we have visited 2000000 regions but haven't found an optimum aggregation sequence, then we return null and overestimate in the heuristic function 
                    }
                }
                u.cenumColor = CEnumColor.black;
            }

            if (FinalOneCphCrg.d > double.MaxValue / 3 || FinalOneCphCrg.CphTypeIndexSD_Area_CphGID.GetFirstT().Value != intSSTypeIndex)
            {
                throw new ArgumentException("incredible large cost or type is not correct!");
            }

            SetRegionChild(FinalOneCphCrg);
            AdjustCost(FinalOneCphCrg, 2);
            //double dblMemory = CHelperFunction.ConsumedMemoryInMB(true , lngStartMemory);



            //var UncoloredCrgSS = new SortedSet<CRegion>(CRegion.pCompareCRegion_CphGID);
            //foreach (var ExistingCrgSD in ExistingCrgSDLt)
            //{
            //    foreach (var ExistingCrgkvp in ExistingCrgSD)
            //    {
            //        UncoloredCrgSS.Add(ExistingCrgkvp.Key);
            //    }
            //}
            double dblRoundedCostEstimatedType = Math.Round(LSCrg.dblCostEstimatedType, _intDigits);
            double dblRoundedCostExactType = Math.Round(FinalOneCphCrg.dblCostExactType, _intDigits);
            double dblRoundedCostEstimatedCompactness = Math.Round(LSCrg.dblCostEstimatedCompactness, _intDigits);
            double dblRoundedCostExactCompactness = Math.Round(FinalOneCphCrg.dblCostExactCompactness, _intDigits);

            double dblRatioTypeCE = 1;
            double dblRatioCompCE = 1;
            double dblRatioTypeComp = 1;

            if (LSCrg.GetCphCount()>1)
            {
                if (LSCrg.dblCostEstimatedType > 0)
                {
                    dblRatioTypeCE = Math.Round(FinalOneCphCrg.dblCostExactType / LSCrg.dblCostEstimatedType, _intDigits);
                }

                dblRatioCompCE = Math.Round(FinalOneCphCrg.dblCostExactCompactness / LSCrg.dblCostEstimatedCompactness, _intDigits);

                dblRatioTypeComp = Math.Round(FinalOneCphCrg.dblCostExactType / FinalOneCphCrg.dblCostExactCompactness, _intDigits);
            }

            StrObjLtSD.SetLastObj("#Nodes", CRegion._intNodesCount + 1); //+1 is for LSCrg
            StrObjLtSD.SetLastObj("EstType", dblRoundedCostEstimatedType);
            StrObjLtSD.SetLastObj("CostType", dblRoundedCostExactType);
            StrObjLtSD.SetLastObj("RatioTypeCE", dblRatioTypeCE);
            StrObjLtSD.SetLastObj("EstComp", dblRoundedCostEstimatedCompactness);
            StrObjLtSD.SetLastObj("CostComp", dblRoundedCostExactCompactness);
            StrObjLtSD.SetLastObj("RatioCompCE", dblRatioCompCE);
            StrObjLtSD.SetLastObj("RatioTypeComp", dblRatioTypeComp);
            StrObjLtSD.SetLastObj("WeightedSum", Math.Round(FinalOneCphCrg.d, _intDigits));

            return FinalOneCphCrg;
        }

        private void AddVirtualItem(List<List<object>> pobjDataLtLt, CRegion LSCrg, string strAreaAggregation, int intCount)
        {
            List<object> objDataLt = new List<object>(14);
            objDataLt.Add(LSCrg.ID);
            objDataLt.Add(LSCrg.CphTypeIndexSD_Area_CphGID.Count);
            objDataLt.Add(LSCrg.Adjacency_CorrCphsSD.Count);
            objDataLt.Add(strAreaAggregation);
            for (int i = 4; i < intCount; i++)
            {
                objDataLt.Add(-1);
            }
            pobjDataLtLt.Add(objDataLt);
        }






        /// <summary>
        /// after A star algorithm, we set the aggregation chain for each region
        /// </summary>
        /// <param name="pCrgLt"></param>
        private void SetRegionChild(CRegion crg)
        {
            while (crg.parent != null)
            {
                var parentcrg = crg.parent;
                parentcrg.child = crg;

                crg = parentcrg;
            }

            this.InitialCrgLt.Add(crg);
        }

        private void AdjustCost(CRegion crg, int intEvaluationNum)
        {
            double dblAdjust = crg.dblArea;
            do
            {
                crg.dblCostEstimated /= dblAdjust;
                crg.dblCostExact /= dblAdjust;
                crg.dblCostExactType /= dblAdjust;
                crg.dblCostEstimatedType /= dblAdjust;
                crg.d /= (dblAdjust);

                crg = crg.parent;
            } while (crg != null);
        }
        #endregion

        #region Common

        private void AddLineToStrObjLtSD(CStrObjLtSD StrObjLtSD, CRegion LSCrg)
        {
            var et = StrObjLtSD.GetEnumerator();
            while (et.MoveNext())
            {
                et.Current.Value.Add(-1);
            }

            StrObjLtSD.SetLastObj("ID", LSCrg.ID);
            StrObjLtSD.SetLastObj("n", LSCrg.CphTypeIndexSD_Area_CphGID.Count);
            StrObjLtSD.SetLastObj("m", LSCrg.Adjacency_CorrCphsSD.Count);
            StrObjLtSD.SetLastObj("Factor", 100000000);
        }


        /// <summary>
        /// assign the polygons as well as attributes from a featureLayer into regions, without considering costs
        /// </summary>
        /// <param name="pCpgLt"></param>
        /// <param name="intCrgNum"></param>
        /// <param name="pObjValueLtLt"></param>
        /// <param name="intTypeATIndex"></param>
        /// <param name="intRegionNumATIndex"></param>
        /// <param name="pTypePVSD"></param>
        /// <param name="pRegionPVSD"></param>
        /// <returns></returns>
        private List<CRegion> GenerateCrgLt(List<CPolygon> pCpgLt, int intCrgNum, List<List<object>> pObjValueLtLt, int intTypeATIndex, int intRegionNumATIndex, CPairVal_SD<int, int> pTypePVSD, CPairVal_SD<int, int> pRegionPVSD)
        {
            var pCrgLt = new List<CRegion>(intCrgNum);
            pCrgLt.EveryElementNew();

            for (int i = 0; i < pCpgLt.Count; i++)
            {
                //get the type index
                int intType = Convert.ToInt32(pObjValueLtLt[i][intTypeATIndex]);
                int intTypeIndex;
                pTypePVSD.SD.TryGetValue(intType, out intTypeIndex);

                //get the RegionNum index
                var intRegionNum = Convert.ToInt32(pObjValueLtLt[i][intRegionNumATIndex]);
                int intRegionIndex;
                pRegionPVSD.SD.TryGetValue(intRegionNum, out intRegionIndex);

                //add the Cph into the corresponding Region
                pCrgLt[intRegionIndex].AddCph(new CPatch(pCpgLt[i], -1, intTypeIndex), intTypeIndex);
                pCrgLt[intRegionIndex].ID = intRegionNum;  //set the ID for each region
            }

            //set the ID of patches
            foreach (var crg in pCrgLt)
            {
                int intCount = 0;
                foreach (var cph in crg.CphTypeIndexSD_Area_CphGID.Keys)
                {
                    cph.ID = intCount++;
                }
            }
            return pCrgLt;
        }
        #endregion

        #region Output
        public void Output(double dblProportion)
        {
            var pParameterInitialize = _ParameterInitialize;
            var pInitialCrgLt = this.InitialCrgLt;
            int intTotalTimeNum = 1;
            for (int i = 0; i < InitialCrgLt.Count; i++)
            {
                intTotalTimeNum += InitialCrgLt[i].GetCphCount()-1;
            }
            int intOutputStepNum = Convert.ToInt32(Math.Floor((intTotalTimeNum - 1) * dblProportion));

            var OutputCrgLt = new List<CRegion>(this.InitialCrgLt.Count);
            var CrgSS = new SortedSet<CRegion>();

            if (pParameterInitialize.strAreaAggregation == "Smallest")
            {
                CrgSS = new SortedSet<CRegion>(this.InitialCrgLt, CRegion.pCompareCRegion_MinArea_CphGIDTypeIndex);
            }
            else
            {
                CrgSS = new SortedSet<CRegion>(this.InitialCrgLt, CRegion.pCompareCRegion_CostExact_CphGIDTypeIndex);  //*******************we may need to change comparator here to smallest area**********//
            }


            for (int i = 1; i <= intOutputStepNum; i++)
            {
                var currentMinCrg = CrgSS.Min;
                CrgSS.Remove(currentMinCrg);
                var newCrg = currentMinCrg.child;

                if (newCrg == null)  //if there is no child anymore, then we must output this Crg
                {
                    OutputCrgLt.Add(currentMinCrg);
                    i--;
                }
                else
                {
                    CrgSS.Add(newCrg);
                }
            }

            OutputCrgLt.AddRange(CrgSS);

            OutputMap(OutputCrgLt, this._TypePVSD, dblProportion, intOutputStepNum +1, pParameterInitialize);
        }


        public static void OutputMap(IEnumerable<CRegion> OutputCrgLt, CPairVal_SD<int, int> pTypePVSD, double dblProportion, 
            int intTime, CParameterInitialize pParameterInitialize)
        {
            int intAttributeNum = 2;
            var pstrFieldNameLt = new List<string>(intAttributeNum);
            pstrFieldNameLt.Add("OBJART");
            pstrFieldNameLt.Add("RegionNum");

            var pesriFieldTypeLt = new List<esriFieldType>(intAttributeNum);
            pesriFieldTypeLt.Add(esriFieldType.esriFieldTypeInteger);
            pesriFieldTypeLt.Add(esriFieldType.esriFieldTypeInteger);

            var pobjectValueLtLt = new List<List<object>>();
            var CpgLt = new List<CPolygon>();
            var IpgLt = new List<IPolygon4>();
            foreach (var crg in OutputCrgLt)
            {
                foreach (var CphTypeIndexKVP in crg.CphTypeIndexSD_Area_CphGID)
                {
                    IpgLt.Add(CphTypeIndexKVP.Key.MergeCpgSS());
                    var pobjectValueLt = new List<object>(intAttributeNum);
                    int intType;
                    pTypePVSD.SD_R.TryGetValue(CphTypeIndexKVP.Value, out intType);
                    pobjectValueLt.Add(intType);
                    pobjectValueLt.Add(crg.ID);
                    pobjectValueLtLt.Add(pobjectValueLt);
                }
            }

            CSaveFeature.SaveIGeoEb(IpgLt, esriGeometryType.esriGeometryPolygon, dblProportion.ToString() + "_#" + IpgLt.Count + "_Step" + intTime.ToString() + "_" + CHelperFunction.GetTimeStamp(),
                pParameterInitialize, pstrFieldNameLt, pesriFieldTypeLt, pobjectValueLtLt, strSymbolLayerPath: pParameterInitialize.strPath + "complete.lyr");
        }



        public static void SaveData(CStrObjLtSD StrObjLtSD, CParameterInitialize pParameterInitialize, string strMethod, int intQuitCount)
        {
            int intAtrNum = StrObjLtSD.Count;
            int intCrgNum=StrObjLtSD.Values.GetFirstT().Count;

            var pobjDataLtLt = new List<IList<object>>(intCrgNum);
            var TempobjDataLtLt = new List<IList<object>>(intAtrNum);

            //order the the lists according to the order of the keys
            foreach (var strKey in strKeyLt)
            {
                List <object> valuelt;
                StrObjLtSD.TryGetValue(strKey, out valuelt);
                TempobjDataLtLt.Add(valuelt);
            }

            //
            for (int j = 0; j < intCrgNum; j++)
            {
                var pobjDataLt = new List<object>(intAtrNum);
                for (int i = 0; i < intAtrNum; i++)
                {
                    pobjDataLt.Add(TempobjDataLtLt[i][j]);
                }
                pobjDataLtLt.Add(pobjDataLt);
            }

            SortedSet<IList<object>> objDataLtSS = new SortedSet<IList<object>>(pobjDataLtLt, new CAACCompare());

            CHelperFunctionExcel.ExportToExcel(objDataLtSS,
                CHelperFunction.GetTimeStamp() + "_" + strMethod + "_" + pParameterInitialize.strAreaAggregation + "_" + 
                CConstants.strShapeConstraint + "_" + intQuitCount, pParameterInitialize.strSavePath, CCAMDijkstra.strKeyLt);
            ExportForLatex(objDataLtSS, CCAMDijkstra.strKeyLt, pParameterInitialize.strSavePath);
            ExportIDOverEstimation(objDataLtSS, pParameterInitialize.strSavePath);
            ExportStatistic(StrObjLtSD, pParameterInitialize.strSavePath);
        }

        public static void ExportStatistic(CStrObjLtSD StrObjLtSD, string strSavePath)
        {
            string strData = "";

            List<object> objFactorLt;
            StrObjLtSD.TryGetValue("Factor", out objFactorLt);
            double dblLogFactorSum = 0;
            int intOverestimationCount = 0;
            var intFactorCountlt = new List<int>(15);
            intFactorCountlt.EveryElementNew();
            for (int i = 0; i < objFactorLt.Count; i++)
            {
                double dblFactor = Convert.ToDouble(objFactorLt[i]);
                double dblLogFactor = Math.Log(dblFactor, 2);
                dblLogFactorSum += dblLogFactor;
                intFactorCountlt[Convert.ToInt16(dblLogFactor)]++;
                if (dblFactor > 1)
                {
                    intOverestimationCount++;
                }
            }

            strData += ("& " + string.Format("{0,3}", intOverestimationCount));
            strData += (" & " + string.Format("{0,3}", dblLogFactorSum));
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "#Edges", "{0,10}",0);
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "#Nodes", "{0,8}",0);
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "CostType", "{0,4}", 1);
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "CostComp", "{0,4}", 1);
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "WeightedSum", "{0,4}", 1);
            strData += GetSumWithSpecifiedStyle(StrObjLtSD, "Time(ms)", "{0,4}",1, 3600000);
            //strData += ;

            //to generate coordinates like (1,6), where x is for the index of overestimation factor, 
            //and y is for the number of domains that used the factor 
            for (int i = 0; i < intFactorCountlt.Count; i++)
            {
                strData += "\n(" + i + "," + intFactorCountlt[i] + ")";
            }

            using (var writer = new StreamWriter(strSavePath + "\\" + CHelperFunction.GetTimeStamp() + "_" + "StatisticsForLatex" + ".txt", true))
            {
                writer.Write(strData);
            }
        }

        private static string GetSumWithSpecifiedStyle(CStrObjLtSD StrObjLtSD, string strKey, string strformat, int intRound, double dblTime = 1)
        {
            List<object> objLt;
            StrObjLtSD.TryGetValue(strKey, out objLt);
            double dblSum = 0;
            for (int i = 0; i < objLt.Count; i++)
            {
                dblSum += Convert.ToDouble(objLt[i]);
            }
            dblSum /= dblTime;

            string strData = Uniquedigits(dblSum, intRound);
            return " & " + string.Format(strformat, strData);
        }

        private static string Uniquedigits(double dblValue, int intRound)
        {
            string strData = "";
            if (intRound > 0)
            {
                string strformatdigits = "0.";
                while (intRound > 0)
                {
                    strformatdigits += "0";
                    intRound--;
                }
                strData = dblValue.ToString(strformatdigits);
            }
            else
            {
                strData = dblValue.ToString();
            }
            return strData;
        }

        public static void ExportForLatex(IEnumerable<IList<object>> objDataLtEb, IEnumerable<object> objHeadEb, string strSavePath)
        {
            //fetch some values that we want to exprot for latex
            IList<string> strFieldLt = new List<string>
            {
                "ID",
                "n",
                "m",
                "Factor",
                "RatioTypeCE",
                "RatioCompCE",
                //"RatioCompType",
                //"WeightedSum",
                "Time(ms)"  //we will output time with unit second
            };

            List<int> intIndexLt = new List<int>(strFieldLt.Count);
            foreach (var strField in strFieldLt)
            {
                int intCount = 0;
                foreach (var objHead in objHeadEb)
                {
                    if (strField == objHead.ToString())
                    {
                        intIndexLt.Add(intCount);
                    }
                    intCount++;
                }
            }

            //{index[,length][:formatString]}; 
            //length: If positive, the parameter is right-aligned; if negative, it is left-aligned.
            //const string format = "{0,6}";
            string strData = "";

            foreach (var objDataLt in objDataLtEb)
            {
                strData += string.Format("{0,3}", objDataLt[intIndexLt[0]]);  //for ID
                for (int i = 1; i < intIndexLt.Count -1; i++)
                {
                    int intIndex = intIndexLt[i];

                    if (i == 1 || i ==2) // for n and m
                    {
                        strData += (" & " + string.Format("{0,2}", objDataLt[intIndex].ToString()));
                    }
                    else if (i == 3)  //for overestimation facotr
                    {
                        strData += (" & " + string.Format("{0,3}", objDataLt[intIndex].ToString()));
                    }
                    else if (i == 4)
                    {
                        strData += (" & " + string.Format("{0,5}", Convert.ToDouble(objDataLt[intIndex]).ToString("0.000")));
                    }
                    else if (i == 5)
                    {
                        strData += (" & " + string.Format("{0,7}", Convert.ToDouble(objDataLt[intIndex]).ToString("0.000")));
                    }
                    else  //for time
                    {
                        //strData += (" & " + string.Format(format, Math.Round(Convert.ToDouble(objDataLt[intIndex]), _intDigits).ToString("0.000")));
                        strData += (" & " + string.Format("{0,5}", Convert.ToDouble(objDataLt[intIndex]).ToString("0.000")));                    
                    }
                }
                strData += (" & " + string.Format("{0,5}", (Convert.ToDouble(objDataLt[intIndexLt.GetLast_T()]) / 1000).ToString("0.0"))); 
                strData += ("\\" + "\\" + "\n");
            }

            using (var writer = new StreamWriter(strSavePath + "\\" + CHelperFunction.GetTimeStamp() + "_" + "DetailsForLatex" + ".txt", true))
            {
                writer.Write(strData);
            }
        }

        private static void ExportIDOverEstimation(IEnumerable<IList<object>> objDataLtEb, string strSavePath)
        {
            string strData = "";
            foreach (var objDataLt in objDataLtEb)
            {
                if (Convert.ToInt32(objDataLt[3]) > 1)
                {
                    strData += "intSpecifiedIDLt.Add(" + objDataLt[0] + ");\n";
                }
                else
                {
                    break;
                }
            }
            using (var writer = new StreamWriter(strSavePath + "\\" + CHelperFunction.GetTimeStamp() + "_" + "FormalizedOverEstimationID" + ".txt", true))
            {
                writer.Write(strData);
            }
        }
        #endregion


    }
}
