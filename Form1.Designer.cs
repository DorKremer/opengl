namespace myOpenGL
{
    partial class Form1
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
            this.timerRUN = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.hScrollBar9 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar8 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar7 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar12 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar11 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar13 = new System.Windows.Forms.HScrollBar();
            this.roadAnimation = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.showAxes = new System.Windows.Forms.CheckBox();
            this.speedBar = new System.Windows.Forms.HScrollBar();
            this.label4 = new System.Windows.Forms.Label();
            this.cubemapTex = new System.Windows.Forms.CheckBox();
            this.showRef = new System.Windows.Forms.CheckBox();
            this.shadows = new System.Windows.Forms.CheckBox();
            this.hScrollBar4 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar5 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar6 = new System.Windows.Forms.HScrollBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerRUN
            // 
            this.timerRUN.Enabled = true;
            this.timerRUN.Interval = 10;
            this.timerRUN.Tick += new System.EventHandler(this.timerRUN_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(981, 511);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // hScrollBar9
            // 
            this.hScrollBar9.Location = new System.Drawing.Point(1308, 245);
            this.hScrollBar9.Maximum = 1000;
            this.hScrollBar9.Minimum = -1000;
            this.hScrollBar9.Name = "hScrollBar9";
            this.hScrollBar9.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar9.SmallChange = 10;
            this.hScrollBar9.TabIndex = 41;
            this.hScrollBar9.Value = 100;
            this.hScrollBar9.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar8
            // 
            this.hScrollBar8.Location = new System.Drawing.Point(1308, 228);
            this.hScrollBar8.Maximum = 1000;
            this.hScrollBar8.Minimum = -1000;
            this.hScrollBar8.Name = "hScrollBar8";
            this.hScrollBar8.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar8.SmallChange = 10;
            this.hScrollBar8.TabIndex = 39;
            this.hScrollBar8.Value = 110;
            this.hScrollBar8.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar7
            // 
            this.hScrollBar7.Location = new System.Drawing.Point(1308, 211);
            this.hScrollBar7.Maximum = 1000;
            this.hScrollBar7.Minimum = -1000;
            this.hScrollBar7.Name = "hScrollBar7";
            this.hScrollBar7.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar7.SmallChange = 10;
            this.hScrollBar7.TabIndex = 40;
            this.hScrollBar7.Value = 100;
            this.hScrollBar7.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Location = new System.Drawing.Point(1304, 50);
            this.hScrollBar3.Maximum = 1000;
            this.hScrollBar3.Minimum = -1000;
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar3.SmallChange = 10;
            this.hScrollBar3.TabIndex = 36;
            this.hScrollBar3.Value = 800;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Location = new System.Drawing.Point(1303, 32);
            this.hScrollBar2.Maximum = 1000;
            this.hScrollBar2.Minimum = -1000;
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar2.SmallChange = 10;
            this.hScrollBar2.TabIndex = 35;
            this.hScrollBar2.Value = 1000;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(1303, 11);
            this.hScrollBar1.Maximum = 1000;
            this.hScrollBar1.Minimum = -1000;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(119, 19);
            this.hScrollBar1.SmallChange = 10;
            this.hScrollBar1.TabIndex = 33;
            this.hScrollBar1.Value = -800;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar12
            // 
            this.hScrollBar12.Location = new System.Drawing.Point(1308, 306);
            this.hScrollBar12.Maximum = 1000;
            this.hScrollBar12.Minimum = -1000;
            this.hScrollBar12.Name = "hScrollBar12";
            this.hScrollBar12.Size = new System.Drawing.Size(125, 17);
            this.hScrollBar12.TabIndex = 45;
            this.hScrollBar12.Value = 200;
            this.hScrollBar12.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar11
            // 
            this.hScrollBar11.Location = new System.Drawing.Point(1308, 289);
            this.hScrollBar11.Maximum = 200;
            this.hScrollBar11.Minimum = -200;
            this.hScrollBar11.Name = "hScrollBar11";
            this.hScrollBar11.Size = new System.Drawing.Size(125, 17);
            this.hScrollBar11.TabIndex = 44;
            this.hScrollBar11.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar13
            // 
            this.hScrollBar13.Location = new System.Drawing.Point(1308, 323);
            this.hScrollBar13.Maximum = 200;
            this.hScrollBar13.Minimum = -200;
            this.hScrollBar13.Name = "hScrollBar13";
            this.hScrollBar13.Size = new System.Drawing.Size(125, 17);
            this.hScrollBar13.TabIndex = 43;
            this.hScrollBar13.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // roadAnimation
            // 
            this.roadAnimation.AutoSize = true;
            this.roadAnimation.Location = new System.Drawing.Point(1075, 9);
            this.roadAnimation.Name = "roadAnimation";
            this.roadAnimation.Size = new System.Drawing.Size(93, 17);
            this.roadAnimation.TabIndex = 46;
            this.roadAnimation.Text = "Animate Road";
            this.roadAnimation.UseVisualStyleBackColor = true;
            this.roadAnimation.CheckedChanged += new System.EventHandler(this.roadAnimation_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1207, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Camera Position X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1207, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Camera Position Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1207, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Camera Position Z";
            // 
            // showAxes
            // 
            this.showAxes.AutoSize = true;
            this.showAxes.Location = new System.Drawing.Point(1075, 32);
            this.showAxes.Name = "showAxes";
            this.showAxes.Size = new System.Drawing.Size(79, 17);
            this.showAxes.TabIndex = 53;
            this.showAxes.Text = "Show Axes";
            this.showAxes.UseVisualStyleBackColor = true;
            this.showAxes.CheckedChanged += new System.EventHandler(this.showAxes_CheckedChanged);
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(1303, 67);
            this.speedBar.Minimum = -100;
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(119, 17);
            this.speedBar.SmallChange = 10;
            this.speedBar.TabIndex = 54;
            this.speedBar.Value = 10;
            this.speedBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.speedBar_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1225, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Car Speed";
            // 
            // cubemapTex
            // 
            this.cubemapTex.AutoSize = true;
            this.cubemapTex.Checked = true;
            this.cubemapTex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cubemapTex.Location = new System.Drawing.Point(1075, 55);
            this.cubemapTex.Name = "cubemapTex";
            this.cubemapTex.Size = new System.Drawing.Size(110, 17);
            this.cubemapTex.TabIndex = 57;
            this.cubemapTex.Text = "Cubemap Texture";
            this.cubemapTex.UseVisualStyleBackColor = true;
            this.cubemapTex.CheckedChanged += new System.EventHandler(this.cubemapTex_CheckedChanged);
            // 
            // showRef
            // 
            this.showRef.AutoSize = true;
            this.showRef.Location = new System.Drawing.Point(1075, 78);
            this.showRef.Name = "showRef";
            this.showRef.Size = new System.Drawing.Size(103, 17);
            this.showRef.TabIndex = 58;
            this.showRef.Text = "Reflective Road";
            this.showRef.UseVisualStyleBackColor = true;
            this.showRef.CheckedChanged += new System.EventHandler(this.showRef_CheckedChanged);
            // 
            // shadows
            // 
            this.shadows.AutoSize = true;
            this.shadows.Location = new System.Drawing.Point(1075, 101);
            this.shadows.Name = "shadows";
            this.shadows.Size = new System.Drawing.Size(70, 17);
            this.shadows.TabIndex = 59;
            this.shadows.Text = "Shadows";
            this.shadows.UseVisualStyleBackColor = true;
            this.shadows.CheckedChanged += new System.EventHandler(this.shadows_CheckedChanged);
            // 
            // hScrollBar4
            // 
            this.hScrollBar4.Location = new System.Drawing.Point(1303, 118);
            this.hScrollBar4.Maximum = 1000;
            this.hScrollBar4.Minimum = -1000;
            this.hScrollBar4.Name = "hScrollBar4";
            this.hScrollBar4.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar4.SmallChange = 10;
            this.hScrollBar4.TabIndex = 62;
            this.hScrollBar4.Value = 100;
            this.hScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar5
            // 
            this.hScrollBar5.Location = new System.Drawing.Point(1304, 135);
            this.hScrollBar5.Maximum = 1000;
            this.hScrollBar5.Minimum = -1000;
            this.hScrollBar5.Name = "hScrollBar5";
            this.hScrollBar5.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar5.SmallChange = 10;
            this.hScrollBar5.TabIndex = 60;
            this.hScrollBar5.Value = 110;
            this.hScrollBar5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar6
            // 
            this.hScrollBar6.Location = new System.Drawing.Point(1304, 152);
            this.hScrollBar6.Maximum = 1000;
            this.hScrollBar6.Minimum = -1000;
            this.hScrollBar6.Name = "hScrollBar6";
            this.hScrollBar6.Size = new System.Drawing.Size(119, 17);
            this.hScrollBar6.SmallChange = 10;
            this.hScrollBar6.TabIndex = 61;
            this.hScrollBar6.Value = 100;
            this.hScrollBar6.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1213, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Camera Target Z";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1213, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 64;
            this.label6.Text = "Camera Target Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1213, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 63;
            this.label7.Text = "Camera Target X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1218, 323);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 68;
            this.label8.Text = "Light Source Z";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1218, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 67;
            this.label9.Text = "Light Source Y";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1218, 289);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 66;
            this.label10.Text = "Light Source X";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1269, 245);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 71;
            this.label11.Text = "Up Z";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1269, 228);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 13);
            this.label12.TabIndex = 70;
            this.label12.Text = "Up Y";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1269, 211);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(31, 13);
            this.label13.TabIndex = 69;
            this.label13.Text = "Up X";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.hScrollBar4);
            this.Controls.Add(this.hScrollBar5);
            this.Controls.Add(this.hScrollBar6);
            this.Controls.Add(this.shadows);
            this.Controls.Add(this.showRef);
            this.Controls.Add(this.cubemapTex);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.speedBar);
            this.Controls.Add(this.showAxes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.roadAnimation);
            this.Controls.Add(this.hScrollBar12);
            this.Controls.Add(this.hScrollBar11);
            this.Controls.Add(this.hScrollBar13);
            this.Controls.Add(this.hScrollBar9);
            this.Controls.Add(this.hScrollBar8);
            this.Controls.Add(this.hScrollBar7);
            this.Controls.Add(this.hScrollBar3);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ModelCar";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerRUN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hScrollBar9;
        private System.Windows.Forms.HScrollBar hScrollBar8;
        private System.Windows.Forms.HScrollBar hScrollBar7;
        private System.Windows.Forms.HScrollBar hScrollBar3;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar12;
        private System.Windows.Forms.HScrollBar hScrollBar11;
        private System.Windows.Forms.HScrollBar hScrollBar13;
        private System.Windows.Forms.CheckBox roadAnimation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox showAxes;
        private System.Windows.Forms.HScrollBar speedBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cubemapTex;
        private System.Windows.Forms.CheckBox showRef;
        private System.Windows.Forms.CheckBox shadows;
        private System.Windows.Forms.HScrollBar hScrollBar4;
        private System.Windows.Forms.HScrollBar hScrollBar5;
        private System.Windows.Forms.HScrollBar hScrollBar6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}

