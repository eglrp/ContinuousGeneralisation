﻿namespace ContinuousGeneralizer.FrmMorphingExtend
{
    public partial class FrmBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbScale = new System.Windows.Forms.ProgressBar();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnReduce = new System.Windows.Forms.Button();
            this.txtProportion = new System.Windows.Forms.TextBox();
            this.btnInputedScale = new System.Windows.Forms.Button();
            this.btnSaveInterpolation = new System.Windows.Forms.Button();
            this.timerAdd = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn050 = new System.Windows.Forms.Button();
            this.btn025 = new System.Windows.Forms.Button();
            this.btn010 = new System.Windows.Forms.Button();
            this.btn020 = new System.Windows.Forms.Button();
            this.btn100 = new System.Windows.Forms.Button();
            this.btn030 = new System.Windows.Forms.Button();
            this.btn075 = new System.Windows.Forms.Button();
            this.btn040 = new System.Windows.Forms.Button();
            this.btn000 = new System.Windows.Forms.Button();
            this.btn060 = new System.Windows.Forms.Button();
            this.btn080 = new System.Windows.Forms.Button();
            this.btn070 = new System.Windows.Forms.Button();
            this.btn090 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReadData = new System.Windows.Forms.Button();
            this.timerReduce = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRunShp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSmallerScaleLayer = new System.Windows.Forms.ComboBox();
            this.lblLayer = new System.Windows.Forms.Label();
            this.cboLargerScaleLayer = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbScale
            // 
            this.pbScale.Location = new System.Drawing.Point(62, 155);
            this.pbScale.Name = "pbScale";
            this.pbScale.Size = new System.Drawing.Size(162, 29);
            this.pbScale.TabIndex = 85;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(6, 91);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(64, 25);
            this.btnRun.TabIndex = 50;
            this.btnRun.Text = "确  定";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(245, 155);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 29);
            this.btnAdd.TabIndex = 84;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnReduce
            // 
            this.btnReduce.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReduce.Location = new System.Drawing.Point(6, 155);
            this.btnReduce.Name = "btnReduce";
            this.btnReduce.Size = new System.Drawing.Size(35, 29);
            this.btnReduce.TabIndex = 83;
            this.btnReduce.Text = "-";
            this.btnReduce.UseVisualStyleBackColor = true;
            this.btnReduce.Click += new System.EventHandler(this.btnReduce_Click);
            // 
            // txtProportion
            // 
            this.txtProportion.Location = new System.Drawing.Point(116, 116);
            this.txtProportion.Name = "txtProportion";
            this.txtProportion.Size = new System.Drawing.Size(103, 20);
            this.txtProportion.TabIndex = 68;
            this.txtProportion.Text = "1.5";
            this.txtProportion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnInputedScale
            // 
            this.btnInputedScale.Location = new System.Drawing.Point(6, 116);
            this.btnInputedScale.Name = "btnInputedScale";
            this.btnInputedScale.Size = new System.Drawing.Size(104, 25);
            this.btnInputedScale.TabIndex = 67;
            this.btnInputedScale.Text = "按输入比例";
            this.btnInputedScale.UseVisualStyleBackColor = true;
            this.btnInputedScale.Click += new System.EventHandler(this.btnInputedScale_Click);
            // 
            // btnSaveInterpolation
            // 
            this.btnSaveInterpolation.Location = new System.Drawing.Point(6, 190);
            this.btnSaveInterpolation.Name = "btnSaveInterpolation";
            this.btnSaveInterpolation.Size = new System.Drawing.Size(162, 25);
            this.btnSaveInterpolation.TabIndex = 66;
            this.btnSaveInterpolation.Text = "保存当前内插曲线";
            this.btnSaveInterpolation.UseVisualStyleBackColor = true;
            this.btnSaveInterpolation.Click += new System.EventHandler(this.btnSaveInterpolation_Click);
            // 
            // timerAdd
            // 
            this.timerAdd.Interval = 10;
            this.timerAdd.Tick += new System.EventHandler(this.timerAdd_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbScale);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnReduce);
            this.groupBox2.Controls.Add(this.txtProportion);
            this.groupBox2.Controls.Add(this.btnInputedScale);
            this.groupBox2.Controls.Add(this.btnSaveInterpolation);
            this.groupBox2.Controls.Add(this.btn050);
            this.groupBox2.Controls.Add(this.btn025);
            this.groupBox2.Controls.Add(this.btn010);
            this.groupBox2.Controls.Add(this.btn020);
            this.groupBox2.Controls.Add(this.btn100);
            this.groupBox2.Controls.Add(this.btn030);
            this.groupBox2.Controls.Add(this.btn075);
            this.groupBox2.Controls.Add(this.btn040);
            this.groupBox2.Controls.Add(this.btn000);
            this.groupBox2.Controls.Add(this.btn060);
            this.groupBox2.Controls.Add(this.btn080);
            this.groupBox2.Controls.Add(this.btn070);
            this.groupBox2.Controls.Add(this.btn090);
            this.groupBox2.Location = new System.Drawing.Point(12, 272);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 221);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "显示";
            // 
            // btn050
            // 
            this.btn050.Location = new System.Drawing.Point(230, 22);
            this.btn050.Name = "btn050";
            this.btn050.Size = new System.Drawing.Size(50, 25);
            this.btn050.TabIndex = 63;
            this.btn050.Text = "t=0.5";
            this.btn050.UseVisualStyleBackColor = true;
            this.btn050.Click += new System.EventHandler(this.btn050_Click);
            // 
            // btn025
            // 
            this.btn025.Location = new System.Drawing.Point(62, 85);
            this.btn025.Name = "btn025";
            this.btn025.Size = new System.Drawing.Size(50, 25);
            this.btn025.TabIndex = 65;
            this.btn025.Text = "t=0.25";
            this.btn025.UseVisualStyleBackColor = true;
            this.btn025.Click += new System.EventHandler(this.btn025_Click);
            // 
            // btn010
            // 
            this.btn010.Location = new System.Drawing.Point(6, 22);
            this.btn010.Name = "btn010";
            this.btn010.Size = new System.Drawing.Size(50, 25);
            this.btn010.TabIndex = 51;
            this.btn010.Text = "t=0.1";
            this.btn010.UseVisualStyleBackColor = true;
            this.btn010.Click += new System.EventHandler(this.btn010_Click);
            // 
            // btn020
            // 
            this.btn020.Location = new System.Drawing.Point(62, 22);
            this.btn020.Name = "btn020";
            this.btn020.Size = new System.Drawing.Size(50, 25);
            this.btn020.TabIndex = 52;
            this.btn020.Text = "t=0.2";
            this.btn020.UseVisualStyleBackColor = true;
            this.btn020.Click += new System.EventHandler(this.btn020_Click);
            // 
            // btn100
            // 
            this.btn100.Location = new System.Drawing.Point(230, 53);
            this.btn100.Name = "btn100";
            this.btn100.Size = new System.Drawing.Size(50, 25);
            this.btn100.TabIndex = 62;
            this.btn100.Text = "t=1";
            this.btn100.UseVisualStyleBackColor = true;
            this.btn100.Click += new System.EventHandler(this.btn100_Click);
            // 
            // btn030
            // 
            this.btn030.Location = new System.Drawing.Point(118, 22);
            this.btn030.Name = "btn030";
            this.btn030.Size = new System.Drawing.Size(50, 25);
            this.btn030.TabIndex = 53;
            this.btn030.Text = "t=0.3";
            this.btn030.UseVisualStyleBackColor = true;
            this.btn030.Click += new System.EventHandler(this.btn030_Click);
            // 
            // btn075
            // 
            this.btn075.Location = new System.Drawing.Point(118, 85);
            this.btn075.Name = "btn075";
            this.btn075.Size = new System.Drawing.Size(50, 25);
            this.btn075.TabIndex = 61;
            this.btn075.Text = "t=0.75";
            this.btn075.UseVisualStyleBackColor = true;
            this.btn075.Click += new System.EventHandler(this.btn075_Click);
            // 
            // btn040
            // 
            this.btn040.Location = new System.Drawing.Point(174, 22);
            this.btn040.Name = "btn040";
            this.btn040.Size = new System.Drawing.Size(50, 25);
            this.btn040.TabIndex = 54;
            this.btn040.Text = "t=0.4";
            this.btn040.UseVisualStyleBackColor = true;
            this.btn040.Click += new System.EventHandler(this.btn040_Click);
            // 
            // btn000
            // 
            this.btn000.Location = new System.Drawing.Point(6, 85);
            this.btn000.Name = "btn000";
            this.btn000.Size = new System.Drawing.Size(50, 25);
            this.btn000.TabIndex = 60;
            this.btn000.Text = "t=0.0";
            this.btn000.UseVisualStyleBackColor = true;
            this.btn000.Click += new System.EventHandler(this.btn000_Click);
            // 
            // btn060
            // 
            this.btn060.Location = new System.Drawing.Point(6, 53);
            this.btn060.Name = "btn060";
            this.btn060.Size = new System.Drawing.Size(50, 25);
            this.btn060.TabIndex = 56;
            this.btn060.Text = "t=0.6";
            this.btn060.UseVisualStyleBackColor = true;
            this.btn060.Click += new System.EventHandler(this.btn060_Click);
            // 
            // btn080
            // 
            this.btn080.Location = new System.Drawing.Point(118, 53);
            this.btn080.Name = "btn080";
            this.btn080.Size = new System.Drawing.Size(50, 25);
            this.btn080.TabIndex = 59;
            this.btn080.Text = "t=0.8";
            this.btn080.UseVisualStyleBackColor = true;
            this.btn080.Click += new System.EventHandler(this.btn080_Click);
            // 
            // btn070
            // 
            this.btn070.Location = new System.Drawing.Point(62, 53);
            this.btn070.Name = "btn070";
            this.btn070.Size = new System.Drawing.Size(50, 25);
            this.btn070.TabIndex = 57;
            this.btn070.Text = "t=0.7";
            this.btn070.UseVisualStyleBackColor = true;
            this.btn070.Click += new System.EventHandler(this.btn070_Click);
            // 
            // btn090
            // 
            this.btn090.Location = new System.Drawing.Point(174, 53);
            this.btn090.Name = "btn090";
            this.btn090.Size = new System.Drawing.Size(50, 25);
            this.btn090.TabIndex = 58;
            this.btn090.Text = "t=0.9";
            this.btn090.UseVisualStyleBackColor = true;
            this.btn090.Click += new System.EventHandler(this.btn090_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnReadData);
            this.groupBox1.Controls.Add(this.btnRun);
            this.groupBox1.Location = new System.Drawing.Point(12, 143);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 125);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "根据Excel数据";
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(6, 19);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(78, 31);
            this.btnReadData.TabIndex = 51;
            this.btnReadData.Text = "ReadData";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // timerReduce
            // 
            this.timerReduce.Interval = 10;
            this.timerReduce.Tick += new System.EventHandler(this.timerReduce_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cboSmallerScaleLayer);
            this.groupBox3.Controls.Add(this.lblLayer);
            this.groupBox3.Controls.Add(this.cboLargerScaleLayer);
            this.groupBox3.Controls.Add(this.btnRunShp);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(288, 125);
            this.groupBox3.TabIndex = 63;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "根据Shp数据";
            // 
            // btnRunShp
            // 
            this.btnRunShp.Location = new System.Drawing.Point(6, 94);
            this.btnRunShp.Name = "btnRunShp";
            this.btnRunShp.Size = new System.Drawing.Size(64, 25);
            this.btnRunShp.TabIndex = 63;
            this.btnRunShp.Text = "确  定";
            this.btnRunShp.UseVisualStyleBackColor = true;
            this.btnRunShp.Click += new System.EventHandler(this.btnRunShp_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 87;
            this.label2.Text = "小比例尺图层：";
            // 
            // cboSmallerScaleLayer
            // 
            this.cboSmallerScaleLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSmallerScaleLayer.FormattingEnabled = true;
            this.cboSmallerScaleLayer.Location = new System.Drawing.Point(122, 47);
            this.cboSmallerScaleLayer.Name = "cboSmallerScaleLayer";
            this.cboSmallerScaleLayer.Size = new System.Drawing.Size(160, 21);
            this.cboSmallerScaleLayer.TabIndex = 88;
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(6, 22);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(91, 13);
            this.lblLayer.TabIndex = 85;
            this.lblLayer.Text = "大比例尺图层：";
            // 
            // cboLargerScaleLayer
            // 
            this.cboLargerScaleLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLargerScaleLayer.FormattingEnabled = true;
            this.cboLargerScaleLayer.Location = new System.Drawing.Point(122, 19);
            this.cboLargerScaleLayer.Name = "cboLargerScaleLayer";
            this.cboLargerScaleLayer.Size = new System.Drawing.Size(160, 21);
            this.cboLargerScaleLayer.TabIndex = 86;
            // 
            // FrmBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 505);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmBase";
            this.Text = "FrmBase";
            this.Load += new System.EventHandler(this.FrmBase_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnReduce;
        private System.Windows.Forms.Button btnInputedScale;
        private System.Windows.Forms.Button btnSaveInterpolation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn050;
        private System.Windows.Forms.Button btn025;
        private System.Windows.Forms.Button btn010;
        private System.Windows.Forms.Button btn020;
        private System.Windows.Forms.Button btn100;
        private System.Windows.Forms.Button btn030;
        private System.Windows.Forms.Button btn075;
        private System.Windows.Forms.Button btn040;
        private System.Windows.Forms.Button btn000;
        private System.Windows.Forms.Button btn060;
        private System.Windows.Forms.Button btn080;
        private System.Windows.Forms.Button btn070;
        private System.Windows.Forms.Button btn090;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnReadData;
        protected System.Windows.Forms.ProgressBar pbScale;
        protected System.Windows.Forms.Timer timerAdd;
        protected System.Windows.Forms.Timer timerReduce;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.Button btnRunShp;
        public System.Windows.Forms.TextBox txtProportion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSmallerScaleLayer;
        private System.Windows.Forms.Label lblLayer;
        private System.Windows.Forms.ComboBox cboLargerScaleLayer;
    }
}