﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;

using ContinuousGeneralizer;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeneralizationMethods;
using MorphingClass.CGeometry;

using ILOG.Concert;
using ILOG.CPLEX;

namespace ContinuousGeneralizer.FrmMorphing
{
    /// <summary>
    /// Continuous Aggregation of Maps based on Dijkstra: CAMDijkstra
    /// </summary>
    public partial class FrmCAMDijkstra : Form
    {
        private CDataRecords _DataRecords;                    //数据记录
        private CFrmOperation _FrmOperation;


        private CCAMDijkstra _pCAMDijkstra;


        private double _dblProportion = 0.5;

        /// <summary>属性：数据记录</summary>
        public CDataRecords DataRecords
        {
            get { return _DataRecords; }
            set { _DataRecords = value; }
        }


        public FrmCAMDijkstra()
        {
            InitializeComponent();
        }

        public FrmCAMDijkstra(CDataRecords pDataRecords)
        {
            InitializeComponent();
            _DataRecords = pDataRecords;
            pDataRecords.ParameterInitialize.frmCurrentForm = this;
        }

        private void FrmCAMDijkstra_Load(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            ParameterInitialize.cboLayerLt = new List<ComboBox>(2);
            ParameterInitialize.cboLayerLt.Add(this.cboLargerScaleLayer);
            ParameterInitialize.cboLayerLt.Add(this.cboSmallerScaleLayer);
            ParameterInitialize.strMorphingMethod = "CAMDijkstra";
            ParameterInitialize.cboShapeConstraint = this.cboShapeConstraint;
            ParameterInitialize.chkSmallest = this.chkSmallest;

            this.cboShapeConstraint.SelectedIndex = 2;

            //进行Load操作，初始化变量
            _FrmOperation = new CFrmOperation(ref ParameterInitialize);
        }



        public void btnRun_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            //var objDataLtLt = new List<List<object>>();
            var objResultSD = new SortedDictionary<string, List<object>>();
            var StrObjLtSD= new CStrObjLtSD(CCAMDijkstra.strKeyLt);
            //读取数据
            _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize);
            _pCAMDijkstra.CAMDijkstra(Convert.ToInt32(txtNodes.Text), "AStar");

            this.txtEvaluation.Text = _pCAMDijkstra.dblCost.ToString();
            StrObjLtSD.Merge(_pCAMDijkstra.StrObjLtSD);
            CCAMDijkstra.SaveData(StrObjLtSD, ParameterInitialize, "AStar", Convert.ToInt32(txtNodes.Text));

            MessageBox.Show("Done!");
        }

        private void btnRunSpecified_Click(object sender, EventArgs e)
        {
            List<int> intSpecifiedIDLt = new List<int>();

            intSpecifiedIDLt.Add(543);
            intSpecifiedIDLt.Add(94);
            intSpecifiedIDLt.Add(190);
            intSpecifiedIDLt.Add(301);
            intSpecifiedIDLt.Add(436);
            intSpecifiedIDLt.Add(523);
            intSpecifiedIDLt.Add(553);
            intSpecifiedIDLt.Add(397);
            intSpecifiedIDLt.Add(586);
            intSpecifiedIDLt.Add(551);
            intSpecifiedIDLt.Add(492);
            intSpecifiedIDLt.Add(194);
            intSpecifiedIDLt.Add(177);
            intSpecifiedIDLt.Add(506);
            intSpecifiedIDLt.Add(165);
            intSpecifiedIDLt.Add(215);
            intSpecifiedIDLt.Add(590);
            intSpecifiedIDLt.Add(386);
            intSpecifiedIDLt.Add(112);
            intSpecifiedIDLt.Add(452);
            intSpecifiedIDLt.Add(531);
            intSpecifiedIDLt.Add(298);
            intSpecifiedIDLt.Add(477);
            intSpecifiedIDLt.Add(53);
            intSpecifiedIDLt.Add(252);
            intSpecifiedIDLt.Add(503);
            intSpecifiedIDLt.Add(343);
            intSpecifiedIDLt.Add(462);
            intSpecifiedIDLt.Add(516);
            intSpecifiedIDLt.Add(400);
            intSpecifiedIDLt.Add(179);
            intSpecifiedIDLt.Add(424);
            intSpecifiedIDLt.Add(410);
            intSpecifiedIDLt.Add(537);
            intSpecifiedIDLt.Add(272);
            intSpecifiedIDLt.Add(1);
            intSpecifiedIDLt.Add(463);
            intSpecifiedIDLt.Add(418);
            intSpecifiedIDLt.Add(155);




            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            var StrObjLtSD = new CStrObjLtSD(CCAMDijkstra.strKeyLt);
            for (int i = 0; i < intSpecifiedIDLt.Count; i++)
            { 
                _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize, "RegionNum", intSpecifiedIDLt[i].ToString());
                _pCAMDijkstra.CAMDijkstra(Convert.ToInt32(txtNodes.Text), "AStar");

                if (chkOutput.Checked==true)
                {
                    btnMultiResults_Click(sender, e);
                }                

                StrObjLtSD.Merge(_pCAMDijkstra.StrObjLtSD);                
            }

            CCAMDijkstra.SaveData(StrObjLtSD, ParameterInitialize, "AStar", Convert.ToInt32(txtNodes.Text));
            MessageBox.Show("Done!");
        }



        private void btnRunILP_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            var StrObjLtSD = new CStrObjLtSD(CCAMDijkstra.strKeyLt);
            Cplex cplex = new Cplex();

            //读取数据
            _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize);
            _pCAMDijkstra.CAMDijkstra(Convert.ToInt32(txtNodes.Text), "ILP");

            this.txtEvaluation.Text = _pCAMDijkstra.dblCost.ToString();
            StrObjLtSD.Merge(_pCAMDijkstra.StrObjLtSD);
            CCAMDijkstra.SaveData(StrObjLtSD, ParameterInitialize, "ILP", Convert.ToInt32(txtNodes.Text));

            MessageBox.Show("Done!");
        }






        private void btnRunILPSpecified_Click(object sender, EventArgs e)
        {
            List<int> intSpecifiedIDLt = new List<int>();

            intSpecifiedIDLt.Add(94);
            intSpecifiedIDLt.Add(301);
            intSpecifiedIDLt.Add(543);
            intSpecifiedIDLt.Add(215);
            intSpecifiedIDLt.Add(590);
            intSpecifiedIDLt.Add(436);
            intSpecifiedIDLt.Add(386);
            intSpecifiedIDLt.Add(112);
            intSpecifiedIDLt.Add(523);
            intSpecifiedIDLt.Add(452);
            intSpecifiedIDLt.Add(553);
            intSpecifiedIDLt.Add(586);
            intSpecifiedIDLt.Add(179);
            intSpecifiedIDLt.Add(194);
            intSpecifiedIDLt.Add(177);
            intSpecifiedIDLt.Add(252);
            intSpecifiedIDLt.Add(506);
            intSpecifiedIDLt.Add(165);
            intSpecifiedIDLt.Add(462);
            intSpecifiedIDLt.Add(272);
            intSpecifiedIDLt.Add(531);
            intSpecifiedIDLt.Add(397);
            intSpecifiedIDLt.Add(190);
            intSpecifiedIDLt.Add(298);
            intSpecifiedIDLt.Add(551);
            intSpecifiedIDLt.Add(492);
            intSpecifiedIDLt.Add(477);
            intSpecifiedIDLt.Add(53);
            intSpecifiedIDLt.Add(424);
            intSpecifiedIDLt.Add(410);
            intSpecifiedIDLt.Add(97);
            intSpecifiedIDLt.Add(537);
            intSpecifiedIDLt.Add(503);
            intSpecifiedIDLt.Add(429);
            intSpecifiedIDLt.Add(67);
            intSpecifiedIDLt.Add(463);
            intSpecifiedIDLt.Add(391);
            intSpecifiedIDLt.Add(516);
            intSpecifiedIDLt.Add(186);
            intSpecifiedIDLt.Add(420);
            intSpecifiedIDLt.Add(14);
            intSpecifiedIDLt.Add(545);
            intSpecifiedIDLt.Add(400);
            intSpecifiedIDLt.Add(155);
            intSpecifiedIDLt.Add(566);
            intSpecifiedIDLt.Add(358);
            intSpecifiedIDLt.Add(257);
            intSpecifiedIDLt.Add(154);
            intSpecifiedIDLt.Add(248);
            intSpecifiedIDLt.Add(343);
            intSpecifiedIDLt.Add(1);
            intSpecifiedIDLt.Add(525);
            intSpecifiedIDLt.Add(418);
            intSpecifiedIDLt.Add(371);
            intSpecifiedIDLt.Add(77);
            intSpecifiedIDLt.Add(216);
            intSpecifiedIDLt.Add(647);
            intSpecifiedIDLt.Add(441);
            intSpecifiedIDLt.Add(542);
            intSpecifiedIDLt.Add(569);
            intSpecifiedIDLt.Add(153);
            intSpecifiedIDLt.Add(521);
            intSpecifiedIDLt.Add(500);
            intSpecifiedIDLt.Add(123);
            intSpecifiedIDLt.Add(245);
            intSpecifiedIDLt.Add(560);



            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            var StrObjLtSD = new CStrObjLtSD(CCAMDijkstra.strKeyLt);

            for (int i = 0; i < intSpecifiedIDLt.Count; i++)
            {
                _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize, "RegionNum", intSpecifiedIDLt[i].ToString());
                _pCAMDijkstra.CAMDijkstra(Convert.ToInt32(txtNodes.Text), "ILP");
                StrObjLtSD.Merge(_pCAMDijkstra.StrObjLtSD);
            }

            CCAMDijkstra.SaveData(StrObjLtSD, ParameterInitialize, "ILP", Convert.ToInt32(txtNodes.Text));
            MessageBox.Show("Done!");
        }



        private void btnRunILP_Extend_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            var StrObjLtSD = new CStrObjLtSD(CCAMDijkstra.strKeyLt);

            //读取数据
            _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize);
            _pCAMDijkstra.CAMDijkstra(Convert.ToInt32(txtNodes.Text), "ILP_Extend");

            this.txtEvaluation.Text = _pCAMDijkstra.dblCost.ToString();
            StrObjLtSD.Merge(_pCAMDijkstra.StrObjLtSD);
            //CCAMDijkstra.SaveData(StrObjLtSD, ParameterInitialize, Convert.ToInt32(txtNodes.Text));

            



        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            int intQuitCount = Convert.ToInt32(txtNodes.Text);
            int intTwiceCount = intQuitCount *2;
            int intUnlimited = 2000000000;

            //1: MinimizeInteriorBoundaries
            //2: MaximizeMinimumCompactness
            //3: MaximizeMinimumCompactness_Combine
            //true: involving smallest
            //false: all
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 3, false);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 3, true);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 1, true);
            RunBasic(ParameterInitialize, "AStar", intQuitCount, 2, true);

            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 3, false);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 3, true);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 1, true);
            RunBasic(ParameterInitialize, "AStar", intTwiceCount, 2, true);

            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 3, false);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 3, true);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 1, true);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 2, true);



            //RunBasic(ParameterInitialize, "ILP", intUnlimited, 1, false);
            //RunBasic(ParameterInitialize, "ILP", intUnlimited, 1, true);


            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 1, true);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 2, false);
            //RunBasic(ParameterInitialize, "AStar", intQuitCount, 2, true);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 1, true);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 2, false);
            //RunBasic(ParameterInitialize, "AStar", intTwiceCount, 2, true);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 1, false);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 1, true);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 2, false);
            //RunBasic(ParameterInitialize, "AStar", intUnlimited, 2, true);

            //RunBasic(ParameterInitialize, "ILP", intUnlimited, 1, false);
            //RunBasic(ParameterInitialize, "ILP", intUnlimited, 1, true);

            MessageBox.Show("Done!");
        }

        private void RunBasic(CParameterInitialize ParameterInitialize,string strMethod, int intQuitCount, int cboShapeConstraintSelectedIndex, bool chkSmallestChecked)
        {
            ParameterInitialize.cboShapeConstraint.SelectedIndex = cboShapeConstraintSelectedIndex;
            ParameterInitialize.chkSmallest.Checked = chkSmallestChecked;

            _pCAMDijkstra = new CCAMDijkstra(ParameterInitialize);
            _pCAMDijkstra.CAMDijkstra(intQuitCount, strMethod);
            CCAMDijkstra.SaveData(_pCAMDijkstra.StrObjLtSD, ParameterInitialize, strMethod, intQuitCount);

            //ParameterInitialize.chkSmallest = this.chkSmallest;
            //this.cboShapeConstraint.SelectedIndex = 1;
        }

        private void btnResultFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@_DataRecords.ParameterInitialize.strSavePath);
        }



        private void btn010_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.1;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn020_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.2;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn030_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.3;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn040_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.4;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn050_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.5;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn060_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.6;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn070_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.7;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn080_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.8;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn090_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.9;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn000_Click(object sender, EventArgs e)
        {
            _dblProportion = 0;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn025_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.25;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn075_Click(object sender, EventArgs e)
        {
            _dblProportion = 0.75;
            _pCAMDijkstra.Output(_dblProportion);
        }
        private void btn100_Click(object sender, EventArgs e)
        {
            _dblProportion = 1;
            _pCAMDijkstra.Output(_dblProportion);
        }

        private void btnInputedScale_Click(object sender, EventArgs e)
        {
            _dblProportion = Convert.ToDouble(this.txtProportion.Text);
            _pCAMDijkstra.Output(_dblProportion);

        }

        private void btnReduce_Click(object sender, EventArgs e)
        {
            try
            {
                _dblProportion = _dblProportion - 0.02;
                pbScale.Value = Convert.ToInt16(100 * _dblProportion);
                _pCAMDijkstra.Output(_dblProportion);
            }
            catch (System.Exception)
            {
                MessageBox.Show("不能再减小了！");
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _dblProportion = _dblProportion + 0.02;
            pbScale.Value = Convert.ToInt16(100 * _dblProportion);
            _pCAMDijkstra.Output(_dblProportion);
        }

        private void btnSaveInterpolation_Click(object sender, EventArgs e)
        {
            //CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            //CParameterResult ParameterResult = _DataRecords.ParameterResult;
            //string strFileName = _dblProportion.ToString();
            //CHelperFunction.SaveCPlLt(ParameterResult.DisplayCPlLt, strFileName+"_morphing", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //CHelperFunction.SaveCPlLt(ParameterResult.FadedDisplayCPlLt, strFileName + "_DPFade", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);





            //CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            //List<CPolyline> cpllt = new List<CPolyline>();
            //for (int i = 0; i < _RelativeInterpolationCplLt.Count; i++)
            //{
            //    cpllt.Add(_RelativeInterpolationCplLt[i]);
            //}
            //string strFileName = _dblProportion.ToString();
            //CHelperFunction.SaveCPlLt(cpllt, strFileName, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
        }

        private void btnMultiResults_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                _dblProportion = i * 0.1;
                i++;
                _pCAMDijkstra.Output(_dblProportion);
            }
        }



    }
}