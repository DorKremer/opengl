using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGL;
using System.Runtime.InteropServices;
using System.Media;
using MathEX;
using System.Threading;

namespace myOpenGL
{

    public partial class Form1 : Form
    {
        cOGL cGL;
        public int i=0;
        public int j=0;
        public int k=0;
        public int s = 0;
        
        public Form1()
        {
            InitializeComponent();
            cGL = new cOGL(panel1);
            //apply the bars values as cGL.ScrollValue[..] properties 
            //!!!
            hScrollBarScroll(hScrollBar1, null);
            hScrollBarScroll(hScrollBar2, null);
            hScrollBarScroll(hScrollBar3, null);

            hScrollBarScroll(hScrollBar4, null);
            hScrollBarScroll(hScrollBar5, null);
            hScrollBarScroll(hScrollBar6, null);

            hScrollBarScroll(hScrollBar7, null);
            hScrollBarScroll(hScrollBar8, null);
            hScrollBarScroll(hScrollBar9, null);

            hScrollBarScroll(hScrollBar11, null);
            hScrollBarScroll(hScrollBar12, null);
            hScrollBarScroll(hScrollBar13, null);

        }
        
        private void timerRUN_Tick(object sender, EventArgs e)
        {
           cGL.Draw();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            cGL.Draw();
        }
        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 87:   //W
                    if (cGL.carZ + cGL.speed+20 >= cGL.trueMax)
                        return;
                    cGL.carZ += cGL.speed;
                    cGL.Draw();
                    break;
                case 65:   //A
                    if (cGL.carX + cGL.speed + 20 >= 100)
                        return;
                    cGL.carX += cGL.speed;
                    cGL.Draw();
                    break;
                case 83:   //S
                    if (cGL.carZ - cGL.speed <= cGL.trueMin)
                        return;
                    cGL.carZ -= cGL.speed;
                    cGL.Draw();
                    break;             
                case 68:   //D
                    if (cGL.carX - cGL.speed -20 <= -100)
                        return;
                    cGL.carX -= cGL.speed;
                    cGL.Draw();
                    break;
                //case 33:   //PageUp +
                //    cGL.TranIn = true;
                //    cGL.Draw();
                //    break;
                //case 34:   //PageDown -
                //    cGL.TranOut = true;
                //    cGL.Draw();
                //    break;               
                //case 97:    //num 1
                //    cGL.RotDown = true;
                //    cGL.RotLeft = true;
                //    cGL.Draw();
                //    break;
                //case 98:    //num 2
                //    cGL.RotDown = true;
                //    cGL.Draw();
                //    break;
                //case 99:    //num 3
                //    cGL.RotDown = true;
                //    cGL.RotRight = true;
                //    cGL.Draw();
                //    break;
                //case 100:   //num 4
                //    cGL.RotLeft = true;
                //    cGL.Draw();
                //    break;
                //case 101:   //num 5
                //    cGL.StartPos = true;
                //    cGL.Draw();
                //    break;
                //case 102:   //num 6
                //    cGL.RotRight = true;
                //    cGL.Draw();
                //    break;
                //case 103:   //num 7
                //    cGL.RotUp = true;
                //    cGL.RotLeft = true;
                //    cGL.Draw();
                //    break;
                //case 104:   //num 8
                //    cGL.RotUp = true;
                //    cGL.Draw();
                //    break;
                //case 105:   //num 9
                //    cGL.RotUp = true;
                //    cGL.RotRight = true;
                //    cGL.Draw();
                //    break;
                //case 107:   //num +
                //    cGL.TranIn = true;
                //    cGL.Draw();
                //    break;
                //case 109:   //num -
                //    cGL.TranOut = true;
                //    cGL.Draw();
                //    break;            
                default:    //Any other key
                    break;
            }

            cGL.RotLeft = false;
            cGL.RotRight = false;
            cGL.RotUp = false;
            cGL.RotDown = false;
            cGL.StartPos = false;
            cGL.TranIn = false;
            cGL.TranOut = false;
        }

        private void hScrollBarScroll(object sender, ScrollEventArgs e)
        {
            cGL.intOptionC = 0;
            HScrollBar hb = (HScrollBar)sender;
            int n = int.Parse(hb.Name.Substring(10));
            cGL.ScrollValue[n - 1] = (hb.Value - 100) / 10.0f;
            if (e != null)
                cGL.Draw();
        }

        private void roadAnimation_CheckedChanged(object sender, EventArgs e)
        {
            cGL.animateRoad = roadAnimation.Checked;
        }

        private void showAxes_CheckedChanged(object sender, EventArgs e)
        {
            cGL.showAxes = showAxes.Checked;
        }

        private void speedBar_Scroll(object sender, ScrollEventArgs e)
        {
            cGL.speed=speedBar.Value;
        }

        private void cubemapTex_CheckedChanged(object sender, EventArgs e)
        {
            cGL.showCubemap=cubemapTex.Checked;
        }

        private void showRef_CheckedChanged(object sender, EventArgs e)
        {
            cGL.showRef = showRef.Checked;
        }

        private void shadows_CheckedChanged(object sender, EventArgs e)
        {
            cGL.shadows = shadows.Checked;
        }
    }
}