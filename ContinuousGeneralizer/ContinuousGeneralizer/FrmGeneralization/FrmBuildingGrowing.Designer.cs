﻿namespace ContinuousGeneralizer.FrmGeneralization
{
    partial class FrmBuildingGrowing
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtEvaluation = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBufferRadius = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBufferStyle = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboSSRoad = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboLSRoad = new System.Windows.Forms.ComboBox();
            this.btnResultFolder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboSmallerScaleLayer = new System.Windows.Forms.ComboBox();
            this.lblLayer = new System.Windows.Forms.Label();
            this.cboLargerScaleLayer = new System.Windows.Forms.ComboBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnReduce = new System.Windows.Forms.Button();
            this.pbScale = new System.Windows.Forms.ProgressBar();
            this.btnSaveInterpolation = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnMultiResults = new System.Windows.Forms.Button();
            this.txtProportion = new System.Windows.Forms.TextBox();
            this.btnInputedScale = new System.Windows.Forms.Button();
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
            this.txtMiterLimit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtEvaluation);
            this.groupBox3.Location = new System.Drawing.Point(12, 651);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(288, 105);
            this.groupBox3.TabIndex = 80;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Evaluation";
            // 
            // txtEvaluation
            // 
            this.txtEvaluation.Location = new System.Drawing.Point(9, 45);
            this.txtEvaluation.Name = "txtEvaluation";
            this.txtEvaluation.ReadOnly = true;
            this.txtEvaluation.Size = new System.Drawing.Size(114, 20);
            this.txtEvaluation.TabIndex = 59;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMiterLimit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtBufferRadius);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboBufferStyle);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cboSSRoad);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cboLSRoad);
            this.groupBox1.Controls.Add(this.btnResultFolder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboSmallerScaleLayer);
            this.groupBox1.Controls.Add(this.lblLayer);
            this.groupBox1.Controls.Add(this.cboLargerScaleLayer);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 239);
            this.groupBox1.TabIndex = 78;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // txtBufferRadius
            // 
            this.txtBufferRadius.Location = new System.Drawing.Point(122, 131);
            this.txtBufferRadius.Name = "txtBufferRadius";
            this.txtBufferRadius.Size = new System.Drawing.Size(158, 20);
            this.txtBufferRadius.TabIndex = 105;
            this.txtBufferRadius.Text = "10";
            this.txtBufferRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 104;
            this.label3.Text = "Buffer Radius:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 102;
            this.label1.Text = "Buffer Style:";
            // 
            // cboBufferStyle
            // 
            this.cboBufferStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBufferStyle.FormattingEnabled = true;
            this.cboBufferStyle.Items.AddRange(new object[] {
            "Miter",
            "Round",
            "Square"});
            this.cboBufferStyle.Location = new System.Drawing.Point(120, 181);
            this.cboBufferStyle.Name = "cboBufferStyle";
            this.cboBufferStyle.Size = new System.Drawing.Size(160, 21);
            this.cboBufferStyle.TabIndex = 103;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 107);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 100;
            this.label8.Text = "SS Road:";
            // 
            // cboSSRoad
            // 
            this.cboSSRoad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSSRoad.FormattingEnabled = true;
            this.cboSSRoad.Location = new System.Drawing.Point(122, 104);
            this.cboSSRoad.Name = "cboSSRoad";
            this.cboSSRoad.Size = new System.Drawing.Size(160, 21);
            this.cboSSRoad.TabIndex = 101;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 98;
            this.label9.Text = "LS Road:";
            // 
            // cboLSRoad
            // 
            this.cboLSRoad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLSRoad.FormattingEnabled = true;
            this.cboLSRoad.Location = new System.Drawing.Point(122, 77);
            this.cboLSRoad.Name = "cboLSRoad";
            this.cboLSRoad.Size = new System.Drawing.Size(160, 21);
            this.cboLSRoad.TabIndex = 99;
            // 
            // btnResultFolder
            // 
            this.btnResultFolder.Location = new System.Drawing.Point(188, 208);
            this.btnResultFolder.Name = "btnResultFolder";
            this.btnResultFolder.Size = new System.Drawing.Size(92, 25);
            this.btnResultFolder.TabIndex = 97;
            this.btnResultFolder.Text = "Result Folder";
            this.btnResultFolder.UseVisualStyleBackColor = true;
            this.btnResultFolder.Click += new System.EventHandler(this.btnResultFolder_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 75;
            this.label2.Text = "Less detailed layer:";
            // 
            // cboSmallerScaleLayer
            // 
            this.cboSmallerScaleLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSmallerScaleLayer.FormattingEnabled = true;
            this.cboSmallerScaleLayer.Location = new System.Drawing.Point(122, 50);
            this.cboSmallerScaleLayer.Name = "cboSmallerScaleLayer";
            this.cboSmallerScaleLayer.Size = new System.Drawing.Size(160, 21);
            this.cboSmallerScaleLayer.TabIndex = 76;
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(6, 25);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(99, 13);
            this.lblLayer.TabIndex = 18;
            this.lblLayer.Text = "More detailed layer:";
            // 
            // cboLargerScaleLayer
            // 
            this.cboLargerScaleLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLargerScaleLayer.FormattingEnabled = true;
            this.cboLargerScaleLayer.Location = new System.Drawing.Point(122, 22);
            this.cboLargerScaleLayer.Name = "cboLargerScaleLayer";
            this.cboLargerScaleLayer.Size = new System.Drawing.Size(160, 21);
            this.cboLargerScaleLayer.TabIndex = 19;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 257);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(64, 25);
            this.btnRun.TabIndex = 71;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(245, 179);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 29);
            this.btnAdd.TabIndex = 80;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnReduce
            // 
            this.btnReduce.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReduce.Location = new System.Drawing.Point(6, 179);
            this.btnReduce.Name = "btnReduce";
            this.btnReduce.Size = new System.Drawing.Size(35, 29);
            this.btnReduce.TabIndex = 79;
            this.btnReduce.Text = "-";
            this.btnReduce.UseVisualStyleBackColor = true;
            // 
            // pbScale
            // 
            this.pbScale.Location = new System.Drawing.Point(62, 179);
            this.pbScale.Name = "pbScale";
            this.pbScale.Size = new System.Drawing.Size(162, 29);
            this.pbScale.TabIndex = 71;
            // 
            // btnSaveInterpolation
            // 
            this.btnSaveInterpolation.Location = new System.Drawing.Point(8, 147);
            this.btnSaveInterpolation.Name = "btnSaveInterpolation";
            this.btnSaveInterpolation.Size = new System.Drawing.Size(162, 25);
            this.btnSaveInterpolation.TabIndex = 70;
            this.btnSaveInterpolation.Text = "Save the interpolated result";
            this.btnSaveInterpolation.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnMultiResults);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnReduce);
            this.groupBox2.Controls.Add(this.pbScale);
            this.groupBox2.Controls.Add(this.btnSaveInterpolation);
            this.groupBox2.Controls.Add(this.txtProportion);
            this.groupBox2.Controls.Add(this.btnInputedScale);
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
            this.groupBox2.Location = new System.Drawing.Point(12, 424);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 221);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display";
            // 
            // btnMultiResults
            // 
            this.btnMultiResults.Location = new System.Drawing.Point(174, 85);
            this.btnMultiResults.Name = "btnMultiResults";
            this.btnMultiResults.Size = new System.Drawing.Size(106, 25);
            this.btnMultiResults.TabIndex = 81;
            this.btnMultiResults.Text = "MultiResults";
            this.btnMultiResults.UseVisualStyleBackColor = true;
            // 
            // txtProportion
            // 
            this.txtProportion.Location = new System.Drawing.Point(116, 116);
            this.txtProportion.Name = "txtProportion";
            this.txtProportion.Size = new System.Drawing.Size(108, 20);
            this.txtProportion.TabIndex = 68;
            this.txtProportion.Text = "0.55";
            this.txtProportion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnInputedScale
            // 
            this.btnInputedScale.Location = new System.Drawing.Point(6, 116);
            this.btnInputedScale.Name = "btnInputedScale";
            this.btnInputedScale.Size = new System.Drawing.Size(104, 25);
            this.btnInputedScale.TabIndex = 67;
            this.btnInputedScale.Text = "Output with t";
            this.btnInputedScale.UseVisualStyleBackColor = true;
            // 
            // btn050
            // 
            this.btn050.Location = new System.Drawing.Point(230, 22);
            this.btn050.Name = "btn050";
            this.btn050.Size = new System.Drawing.Size(50, 25);
            this.btn050.TabIndex = 63;
            this.btn050.Text = "t=0.5";
            this.btn050.UseVisualStyleBackColor = true;
            // 
            // btn025
            // 
            this.btn025.Location = new System.Drawing.Point(62, 85);
            this.btn025.Name = "btn025";
            this.btn025.Size = new System.Drawing.Size(50, 25);
            this.btn025.TabIndex = 65;
            this.btn025.Text = "t=0.25";
            this.btn025.UseVisualStyleBackColor = true;
            // 
            // btn010
            // 
            this.btn010.Location = new System.Drawing.Point(6, 22);
            this.btn010.Name = "btn010";
            this.btn010.Size = new System.Drawing.Size(50, 25);
            this.btn010.TabIndex = 51;
            this.btn010.Text = "t=0.1";
            this.btn010.UseVisualStyleBackColor = true;
            // 
            // btn020
            // 
            this.btn020.Location = new System.Drawing.Point(62, 22);
            this.btn020.Name = "btn020";
            this.btn020.Size = new System.Drawing.Size(50, 25);
            this.btn020.TabIndex = 52;
            this.btn020.Text = "t=0.2";
            this.btn020.UseVisualStyleBackColor = true;
            // 
            // btn100
            // 
            this.btn100.Location = new System.Drawing.Point(230, 53);
            this.btn100.Name = "btn100";
            this.btn100.Size = new System.Drawing.Size(50, 25);
            this.btn100.TabIndex = 62;
            this.btn100.Text = "t=1";
            this.btn100.UseVisualStyleBackColor = true;
            // 
            // btn030
            // 
            this.btn030.Location = new System.Drawing.Point(118, 22);
            this.btn030.Name = "btn030";
            this.btn030.Size = new System.Drawing.Size(50, 25);
            this.btn030.TabIndex = 53;
            this.btn030.Text = "t=0.3";
            this.btn030.UseVisualStyleBackColor = true;
            // 
            // btn075
            // 
            this.btn075.Location = new System.Drawing.Point(118, 85);
            this.btn075.Name = "btn075";
            this.btn075.Size = new System.Drawing.Size(50, 25);
            this.btn075.TabIndex = 61;
            this.btn075.Text = "t=0.75";
            this.btn075.UseVisualStyleBackColor = true;
            // 
            // btn040
            // 
            this.btn040.Location = new System.Drawing.Point(174, 22);
            this.btn040.Name = "btn040";
            this.btn040.Size = new System.Drawing.Size(50, 25);
            this.btn040.TabIndex = 54;
            this.btn040.Text = "t=0.4";
            this.btn040.UseVisualStyleBackColor = true;
            // 
            // btn000
            // 
            this.btn000.Location = new System.Drawing.Point(6, 85);
            this.btn000.Name = "btn000";
            this.btn000.Size = new System.Drawing.Size(50, 25);
            this.btn000.TabIndex = 60;
            this.btn000.Text = "t=0.0";
            this.btn000.UseVisualStyleBackColor = true;
            // 
            // btn060
            // 
            this.btn060.Location = new System.Drawing.Point(6, 53);
            this.btn060.Name = "btn060";
            this.btn060.Size = new System.Drawing.Size(50, 25);
            this.btn060.TabIndex = 56;
            this.btn060.Text = "t=0.6";
            this.btn060.UseVisualStyleBackColor = true;
            // 
            // btn080
            // 
            this.btn080.Location = new System.Drawing.Point(118, 53);
            this.btn080.Name = "btn080";
            this.btn080.Size = new System.Drawing.Size(50, 25);
            this.btn080.TabIndex = 59;
            this.btn080.Text = "t=0.8";
            this.btn080.UseVisualStyleBackColor = true;
            // 
            // btn070
            // 
            this.btn070.Location = new System.Drawing.Point(62, 53);
            this.btn070.Name = "btn070";
            this.btn070.Size = new System.Drawing.Size(50, 25);
            this.btn070.TabIndex = 57;
            this.btn070.Text = "t=0.7";
            this.btn070.UseVisualStyleBackColor = true;
            // 
            // btn090
            // 
            this.btn090.Location = new System.Drawing.Point(174, 53);
            this.btn090.Name = "btn090";
            this.btn090.Size = new System.Drawing.Size(50, 25);
            this.btn090.TabIndex = 58;
            this.btn090.Text = "t=0.9";
            this.btn090.UseVisualStyleBackColor = true;
            // 
            // txtMiterLimit
            // 
            this.txtMiterLimit.Location = new System.Drawing.Point(122, 155);
            this.txtMiterLimit.Name = "txtMiterLimit";
            this.txtMiterLimit.Size = new System.Drawing.Size(158, 20);
            this.txtMiterLimit.TabIndex = 107;
            this.txtMiterLimit.Text = "2";
            this.txtMiterLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 106;
            this.label4.Text = "Miter Limit:";
            // 
            // FrmBuildingGrowing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 770);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnRun);
            this.Name = "FrmBuildingGrowing";
            this.Text = "FrmBuildingGrowing";
            this.Load += new System.EventHandler(this.FrmBuildingGrowing_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtEvaluation;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnResultFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboSmallerScaleLayer;
        private System.Windows.Forms.Label lblLayer;
        private System.Windows.Forms.ComboBox cboLargerScaleLayer;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnReduce;
        private System.Windows.Forms.ProgressBar pbScale;
        private System.Windows.Forms.Button btnSaveInterpolation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnMultiResults;
        private System.Windows.Forms.TextBox txtProportion;
        private System.Windows.Forms.Button btnInputedScale;
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
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboSSRoad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboLSRoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBufferStyle;
        private System.Windows.Forms.TextBox txtBufferRadius;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMiterLimit;
        private System.Windows.Forms.Label label4;
    }
}