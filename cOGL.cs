using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Milkshape;
using MathEX;

namespace OpenGL
{
    class cOGL
    {
        Control p;
        GLUquadric obj;
        int Width;
        int Height;

        Milkshape.Character model;

        public cOGL(Control pb)
        {
            p = pb;
            Width = p.Width;
            Height = p.Height;
            ground1[0, 0] = 100f;
            ground1[0, 1] = -14.8f;
            ground1[0, 2] = trueMax;

            ground1[1, 0] = 100.0f;
            ground1[1, 1] = -14.8f;
            ground1[1, 2] = trueMin;

            ground1[2, 0] = -100.0f;
            ground1[2, 1] = -14.8f;
            ground1[2, 2] = trueMin;



            InitializeGL();
            obj = GLU.gluNewQuadric();

            model = new Milkshape.Character("f360.ms3d");
        }

        ~cOGL()
        {
            WGL.wglDeleteContext(m_uint_RC);
        }

        uint m_uint_HWND = 0;

        public uint HWND
        {
            get { return m_uint_HWND; }
        }

        uint m_uint_DC = 0;

        public uint DC
        {
            get { return m_uint_DC; }
        }

        uint m_uint_RC = 0;

        public uint RC
        {
            get { return m_uint_RC; }
        }
        // cam
        public bool TranIn = false;
        public bool TranOut = false;
        public bool StartPos = false;
        public bool RotLeft = false;
        public bool RotRight = false;
        public bool RotUp = false;
        public bool RotDown = false;

        public bool showAxes = false;
        public bool shadows = false;
        public bool animateRoad = false;
        public bool showRef = false;
        public bool showCubemap = true;
        public int intOptionC = 0;
        float[,] ground1 = new float[3, 3];
        public float[] ScrollValue = new float[14];
        public float carX = 0f, carY = 0f, carZ = 0f;
        public float horizontalAngle, verticalAngle;
        public uint[] Textures = new uint[6];
        public float speed = 12f;
        const int x = 0;
        const int y = 1;
        const int z = 2;
        float tileSize = 200;

        float[] cubeXform = new float[16];
        //cam
        float DeltaX = 0f;
        float AngleX = 0f;
        float AngleY = 0f;
        //light
        float[] pos = new float[4];

        double[] AccumulatedRotationsTraslations = new double[16];
        //cross
        float angle = 0.0f;
        //animate
        public float trueMin = -1000f;
        public float min = -1000f, max = 1000f, size = 100f;
        public float trueMax = 1000;

        void TranslateView()
        {
            if (TranIn)
                if (DeltaX >= -167)
                    DeltaX -= 3;
            if (TranOut)
                if (DeltaX <= 15)
                    DeltaX += 3;
            if (StartPos)
                DeltaX = 0;
            GL.glTranslatef(0, DeltaX, 0);
        }


        void RotateView()
        {
            if (RotLeft)
                AngleX += 3;
            if (RotRight)
                AngleX -= 3;
            if (RotUp)
                AngleY += 3;
            if (RotDown)
                AngleY -= 3;
            if (StartPos)
            {
                AngleX = 0;
                AngleY = 0;
            }
            GL.glRotatef(AngleX, 0, 0, 1);
            GL.glRotatef(AngleY, 1, 0, 0);
        }


        public void Draw()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;

            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT | GL.GL_STENCIL_BUFFER_BIT);
            GL.glLoadIdentity();

            double[] ModelVievMatrixBeforeSpecificTransforms = new double[16];
            double[] CurrentRotationTraslation = new double[16];
            GLU.gluLookAt(ScrollValue[0], ScrollValue[1], ScrollValue[2],
                ScrollValue[3], ScrollValue[4], ScrollValue[5],
                ScrollValue[6], ScrollValue[7], ScrollValue[8]);

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, ModelVievMatrixBeforeSpecificTransforms);
            GL.glLoadIdentity();

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, CurrentRotationTraslation);

            GL.glLoadMatrixd(AccumulatedRotationsTraslations);

            GL.glMultMatrixd(CurrentRotationTraslation);

            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);

            GL.glLoadMatrixd(ModelVievMatrixBeforeSpecificTransforms);

            GL.glMultMatrixd(AccumulatedRotationsTraslations);
            if (showRef)
            {
                GL.glEnable(GL.GL_BLEND);
                GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

                GL.glEnable(GL.GL_STENCIL_TEST);
                GL.glStencilOp(GL.GL_REPLACE, GL.GL_REPLACE, GL.GL_REPLACE);
                GL.glStencilFunc(GL.GL_ALWAYS, 1, 0xFFFFFFFF);
                GL.glColorMask((byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE, (byte)GL.GL_FALSE);
                GL.glDisable(GL.GL_DEPTH_TEST);

                DrawRoad(false);

                GL.glColorMask((byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE, (byte)GL.GL_TRUE);
                GL.glEnable(GL.GL_DEPTH_TEST);

                GL.glStencilFunc(GL.GL_EQUAL, 1, 0xFFFFFFFF);
                GL.glStencilOp(GL.GL_KEEP, GL.GL_KEEP, GL.GL_KEEP);

                GL.glEnable(GL.GL_STENCIL_TEST);

                GL.glPushMatrix();
                GL.glTranslatef(0, -15, 0);
                GL.glScalef(1, -1, 1);
                GL.glTranslatef(0, 15, 0);
                GL.glEnable(GL.GL_CULL_FACE);
                GL.glCullFace(GL.GL_BACK);
                DrawScene();
                GL.glCullFace(GL.GL_FRONT);
                DrawScene();
                GL.glDisable(GL.GL_CULL_FACE);
                GL.glPopMatrix();

                GL.glDepthMask((byte)GL.GL_FALSE);
                DrawRoad(false);
                GL.glDepthMask((byte)GL.GL_TRUE);
                GL.glDisable(GL.GL_STENCIL_TEST);
                DrawScene();
                DrawRoad(true);

            }
            else
            {
                DrawScene();
                DrawRoad(true);
            }
            GL.glFlush();
            WGL.wglSwapBuffers(m_uint_DC);
        }
        void DrawScene()
        {

            GL.glTranslatef(pos[0], pos[1], pos[2]);
            GL.glColor3f(1, 1, 0);
            GLUT.glutSolidSphere(10, 20, 20);
            GL.glTranslatef(-pos[0], -pos[1], -pos[2]);
            GL.glColor3f(1, 1, 1);

            TranslateView();
            RotateView();
            if (showAxes)
            {
                DrawAxes();
            }
            pos[0] = 10 * ScrollValue[10];
            pos[1] = 10 * ScrollValue[11];
            pos[2] = 10 * ScrollValue[12];
            pos[3] = 1.0f;
            GL.glLightfv(GL.GL_LIGHT0, GL.GL_POSITION, pos);

            float[] ambeintColor = { 0.75f, 0.75f, 0.75f, 1f };
            float[] posAmbient1 = { 200f, 200f, 200f, 1f };
            float[] posAmbient2 = { 200f, 200f, -200f, 1f };
            float[] posAmbient3 = { -200f, 200f, 200f, 1f };
            float[] posAmbient4 = { -200f, 200f, -200f, 1f };

            GL.glLightfv(GL.GL_LIGHT1, GL.GL_AMBIENT, ambeintColor);
            GL.glLightfv(GL.GL_LIGHT1, GL.GL_POSITION, posAmbient1);
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_AMBIENT, ambeintColor);
            GL.glLightfv(GL.GL_LIGHT2, GL.GL_POSITION, posAmbient2);
            GL.glLightfv(GL.GL_LIGHT3, GL.GL_AMBIENT, ambeintColor);
            GL.glLightfv(GL.GL_LIGHT3, GL.GL_POSITION, posAmbient3);
            GL.glLightfv(GL.GL_LIGHT4, GL.GL_AMBIENT, ambeintColor);
            GL.glLightfv(GL.GL_LIGHT4, GL.GL_POSITION, posAmbient4);

            if (showCubemap)
            {
                GL.glEnable(GL.GL_TEXTURE_2D);
            }
            DrawTexturedCube();
            GL.glDisable(GL.GL_TEXTURE_2D);

            if (verticalAngle != 0 || horizontalAngle != 0)
            {
                GL.glRotatef(horizontalAngle, 1f, 1f, 1f);
                GL.glRotatef(verticalAngle, 1f, 1f, 1f);
                horizontalAngle = 0f;
                verticalAngle = 0f;
            }

            GL.glEnable(GL.GL_LIGHTING);
            GL.glColor3d(1, 0, 0);
            DrawCar(carX, carY, carZ, false);
            if (shadows)
            {

                GL.glPopMatrix();

                GL.glDisable(GL.GL_LIGHTING);

                GL.glPushMatrix();

                MakeShadowMatrix(ground1);
                GL.glMultMatrixf(cubeXform);
                DrawCar(carX, carY, carZ, true);
                GL.glPopMatrix();
               
            }
        }
        protected virtual void InitializeGL()
        {
            m_uint_HWND = (uint)p.Handle.ToInt32();
            m_uint_DC = WGL.GetDC(m_uint_HWND);

            WGL.wglSwapBuffers(m_uint_DC);

            WGL.PIXELFORMATDESCRIPTOR pfd = new WGL.PIXELFORMATDESCRIPTOR();
            WGL.ZeroPixelDescriptor(ref pfd);
            pfd.nVersion = 1;
            pfd.dwFlags = (WGL.PFD_DRAW_TO_WINDOW | WGL.PFD_SUPPORT_OPENGL | WGL.PFD_DOUBLEBUFFER);
            pfd.iPixelType = (byte)(WGL.PFD_TYPE_RGBA);
            pfd.cColorBits = 32;
            pfd.cStencilBits = 32;
            pfd.cDepthBits = 32;
            pfd.iLayerType = (byte)(WGL.PFD_MAIN_PLANE);

            int pixelFormatIndex = 0;
            pixelFormatIndex = WGL.ChoosePixelFormat(m_uint_DC, ref pfd);
            if (pixelFormatIndex == 0)
            {
                MessageBox.Show("Unable to retrieve pixel format");
                return;
            }

            if (WGL.SetPixelFormat(m_uint_DC, pixelFormatIndex, ref pfd) == 0)
            {
                MessageBox.Show("Unable to set pixel format");
                return;
            }
            //Create rendering context
            m_uint_RC = WGL.wglCreateContext(m_uint_DC);
            if (m_uint_RC == 0)
            {
                MessageBox.Show("Unable to get rendering context");
                return;
            }
            if (WGL.wglMakeCurrent(m_uint_DC, m_uint_RC) == 0)
            {
                MessageBox.Show("Unable to make rendering context current");
                return;
            }
            initRenderingGL();
        }

        protected virtual void initRenderingGL()
        {
            if (m_uint_DC == 0 || m_uint_RC == 0)
                return;
            if (this.Width == 0 || this.Height == 0)
                return;

            GL.glShadeModel(GL.GL_SMOOTH);
            GL.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.glClearDepth(1.0f);

            GL.glEnable(GL.GL_LIGHT0);
            GL.glEnable(GL.GL_LIGHTING);
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            GL.glColorMaterial(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT_AND_DIFFUSE);

            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glDepthFunc(GL.GL_LEQUAL);
            GL.glHint(GL.GL_PERSPECTIVE_CORRECTION_Hint, GL.GL_NICEST);

            GL.glViewport(0, 0, this.Width, this.Height);
            GL.glMatrixMode(GL.GL_PROJECTION);
            GL.glLoadIdentity();

            GLU.gluPerspective(90.0, ((double)Width) / Height, 1.0, 3000.0);

            GL.glMatrixMode(GL.GL_MODELVIEW);
            GL.glLoadIdentity();
            GL.glGetDoublev(GL.GL_MODELVIEW_MATRIX, AccumulatedRotationsTraslations);
            GenerateTextures();
        }


        void GenerateTextures()
        {
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
            GL.glGenTextures(6, Textures);
            string[] imagesName ={ "front.jpg","back.jpg",
                                    "left.jpg","right.jpg","top.jpg","bottom.jpg",};
            for (int i = 0; i < 6; i++)
            {
                Bitmap image = new Bitmap(imagesName[i]);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY); //Y axis in Windows is directed downwards, while in OpenGL-upwards
                System.Drawing.Imaging.BitmapData bitmapdata;
                Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

                bitmapdata = image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[i]);
                //2D for XYZ
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, image.Width, image.Height,
                                                              0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapdata.Scan0);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

                image.UnlockBits(bitmapdata);
                image.Dispose();
            }
        }

        void DrawCar(float x, float y, float z, bool shadow)
        {
            GL.glTranslatef(x, y, z);

            GL.glDisable(GL.GL_LIGHT1);
            GL.glDisable(GL.GL_LIGHT2);
            GL.glDisable(GL.GL_LIGHT3);
            GL.glDisable(GL.GL_LIGHT4);

            if (shadow)
                model.DrawModelShadow();
            else
                model.DrawModel();

            GL.glEnable(GL.GL_LIGHT1);
            GL.glEnable(GL.GL_LIGHT2);
            GL.glEnable(GL.GL_LIGHT3);
            GL.glEnable(GL.GL_LIGHT4);

            DrawWheels(x, y, z, shadow);

            GL.glTranslatef(-x, -y, -z);
        }

        void DrawWheels(float x, float y, float z, bool shadow)
        {
            if (shadow)
                GL.glColor4f(0.4f, 0.4f, 0.4f, 1);
            else
                GL.glColor3f(0.01f, 0.01f, 0.01f);
            DrawCircle(8f, 28, -7f, -21f);
            DrawCircle(8f, 28, -7f, 22);
            DrawCircle(8f, -32, -7f, 22);
            DrawCircle(8f, -32, -7f, -21);

            if (shadow)
                GL.glColor4f(0.4f, 0.4f, 0.4f, 1);
            else
                GL.glColor3f(0.1f, 0.1f, 0.1f);
            DrawCircle(6f, 28, -7f, -21.1f);
            DrawCircle(6f, 28, -7f, 22.1f);
            DrawCircle(6f, -32f, -7f, 22.1f);
            DrawCircle(6f, -32f, -7f, -21.1f);
            if (shadow)
                GL.glColor4f(0.4f, 0.4f, 0.4f, 1);
            else
                GL.glColor3f(0.2f, 0.2f, 0.2f);
            drawCross(28f, -7f, -21.1f);
            drawCross(28f, -7f, 22.1f);
            drawCross(-32f, -7f, 22.1f);
            drawCross(-32f, -7f, -21.1f);

            if (animateRoad)
            {
                angle += speed;
                if (angle >= 360)
                    angle = 0;
            }
            GL.glColor3f(1f, 1f, 1f);
        }


        void drawCross(float x, float y, float z)
        {
            GL.glPushMatrix();
            GL.glTranslatef(z, y, x);
            GL.glRotatef(angle, 1.0f, 0.0f, 0.0f);

            GL.glBegin(GL.GL_QUADS);

            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, 5, 0.5f);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, 5, -0.5f);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, -5, -0.5f);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, -5, 0.5f);

            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, 0.5f, 5);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, 0.5f, -5);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, -0.5f, -5);
            GL.glVertex3f(z < 0 ? -0.1f : 0.1f, -0.5f, 5);


            GL.glEnd();

            GL.glRotatef(-angle, 1.0f, 0.0f, 0.0f);
            GL.glTranslatef(-x, -y, -z);
            GL.glPopMatrix();
        }

        void DrawCircle(float radius, float centerX, float centerY, float centerZ)
        {
            int numSegments = 100;
            GL.glBegin(GL.GL_TRIANGLE_FAN);
            for (int i = 0; i < numSegments; i++)
            {
                float theta = 2.0f * 3.1415926f * (float)i / (float)numSegments; // Get the current angle
                float x = (float)(radius * Math.Cos(theta)); // Calculate the x-coordinate
                float y = (float)(radius * Math.Sin(theta)); // Calculate the y-coordinate
                GL.glVertex3f(centerZ, y + centerY, x + centerX); // Output vertex
            }
            GL.glEnd();
        }


        void DrawRoad(bool updateSpeed)
        {
            GL.glEnable(GL.GL_LIGHTING);
            if (showRef)
                GL.glColor4f(0.04f, 0.04f, 0.04f, 0.6f);
            else
                GL.glColor3f(0.04f, 0.04f, 0.04f);
            GL.glBegin(GL.GL_QUADS);
            GL.glVertex3f(100f, -15.0f, trueMax);
            GL.glVertex3f(100.0f, -15.0f, trueMin);
            GL.glVertex3f(-100.0f, -15.0f, trueMin);
            GL.glVertex3f(-100.0f, -15.0f, trueMax);
            GL.glEnd();

            DrawLines(updateSpeed);
            DrawSidewalk(updateSpeed);
        }

        void DrawLines(bool updateSpeed)
        {

            float posMin = Math.Abs(min);
            float posMax = Math.Abs(max);
            int range = (int)((posMin + posMax) / size);
            GL.glColor3f(0.18f, 0.18f, 0f);
            if (speed >= 0)
            {
                max = trueMax;
                for (int i = 0; i < range; i++)
                {
                    if (i % 2 != 0)
                    {
                        GL.glBegin(GL.GL_QUADS);
                        float start = min + i * size, end = min + (i + 1) * size;
                        GL.glVertex3f(10f, -14.9f, end);
                        GL.glVertex3f(10.0f, -14.9f, start);
                        GL.glVertex3f(-10.0f, -14.9f, start);
                        GL.glVertex3f(-10.0f, -14.9f, end);
                        GL.glEnd();
                    }
                }
                if (animateRoad && updateSpeed)
                {
                    if (min < trueMin - size * 4)
                        min = trueMin;
                    else
                        min -= speed;
                }
            }
            else
            {
                min = trueMin;
                for (int i = 0; i < range; i++)
                {
                    if (i % 2 == 0)
                    {
                        GL.glBegin(GL.GL_QUADS);
                        float end = max - i * size, start = max - (i + 1) * size;
                        GL.glVertex3f(10f, -14.9f, end);
                        GL.glVertex3f(10.0f, -14.9f, start);
                        GL.glVertex3f(-10.0f, -14.9f, start);
                        GL.glVertex3f(-10.0f, -14.9f, end);
                        GL.glEnd();
                    }
                }
                if (animateRoad && updateSpeed)
                {
                    if (max < trueMin - size * 4)
                        max = trueMax;
                    else
                        max -= speed;
                }
            }
            GL.glColor3f(1, 1, 1);
        }

        void DrawSidewalk(bool updateSpeed)
        {
            float posMin = Math.Abs(min);
            float posMax = Math.Abs(max);
            int range = (int)((posMin + posMax) / tileSize);
            if (speed >= 0)
            {
                max = trueMax;
                for (int i = 0; i < range; i++)
                {
                    if (i % 2 == 0) 
                        GL.glColor3f(0.2f, 0.2f, 0.2f);
                    else
                        GL.glColor3f(0.22f, 0.22f, 0.22f);

                    float start = min + i * tileSize, end = min + (i + 1) * tileSize;
                    if(i%2==0)
                        DrawPole(start, end);
                    GL.glBegin(GL.GL_QUADS);
                    GL.glVertex3f(100f, -15f, end);
                    GL.glVertex3f(100f, -15f, start);
                    GL.glVertex3f(100f, -10f, start);
                    GL.glVertex3f(100f, -10f, end);

                    GL.glVertex3f(100f + tileSize, -10f, end);
                    GL.glVertex3f(100f + tileSize, -10f, start);
                    GL.glVertex3f(100f, -10f, start);
                    GL.glVertex3f(100f, -10f, end);

                    if (i % 2 == 0)
                        GL.glColor3f(0.22f, 0.22f, 0.22f);
                    else
                        GL.glColor3f(0.2f, 0.2f, 0.2f);

                    GL.glVertex3f(-100f, -15f, end);
                    GL.glVertex3f(-100f, -15f, start);
                    GL.glVertex3f(-100f, -10f, start);
                    GL.glVertex3f(-100f, -10f, end);

                    GL.glVertex3f(-100f - tileSize, -10f, end);
                    GL.glVertex3f(-100f - tileSize, -10f, start);
                    GL.glVertex3f(-100f, -10f, start);
                    GL.glVertex3f(-100f, -10f, end);

                    GL.glEnd();
                }
                if (animateRoad && updateSpeed)
                {
                    if (min < trueMin - tileSize * 4)
                        min = trueMin;
                    else
                        min -= speed;
                }
            }
            else
            {
                min = trueMin;
                for (int i = 0; i < range; i++)
                {
                    if (i % 2 == 0)
                        GL.glColor3f(0.2f, 0.2f, 0.2f);
                    else
                        GL.glColor3f(0.22f, 0.22f, 0.22f);

                    float start = max - i * tileSize, end = max - (i + 1) * tileSize;
                    if (i % 2 != 0)
                        DrawPole(start, end);
                    GL.glBegin(GL.GL_QUADS);
                    GL.glVertex3f(100f, -15f, end);
                    GL.glVertex3f(100f, -15f, start);
                    GL.glVertex3f(100f, -10f, start);
                    GL.glVertex3f(100f, -10f, end);

                    GL.glVertex3f(100f + tileSize, -10f, end);
                    GL.glVertex3f(100f + tileSize, -10f, start);
                    GL.glVertex3f(100f, -10f, start);
                    GL.glVertex3f(100f, -10f, end);

                    if (i % 2 == 0)
                        GL.glColor3f(0.22f, 0.22f, 0.22f);
                    else
                        GL.glColor3f(0.2f, 0.2f, 0.2f);

                    GL.glVertex3f(-100f, -15f, end);
                    GL.glVertex3f(-100f, -15f, start);
                    GL.glVertex3f(-100f, -10f, start);
                    GL.glVertex3f(-100f, -10f, end);

                    GL.glVertex3f(-100f - tileSize, -10f, end);
                    GL.glVertex3f(-100f - tileSize, -10f, start);
                    GL.glVertex3f(-100f, -10f, start);
                    GL.glVertex3f(-100f, -10f, end);

                    GL.glEnd();
                }
                if (animateRoad && updateSpeed)
                {
                    if (max < trueMin - tileSize * 4)
                        max = trueMax;
                    else
                        max -= speed;
                }
            }
            GL.glColor3f(1, 1, 1);
        }

        void DrawPole(float start, float end)
        {
            GL.glColor3f(0, 0.1f, 0);

            GL.glBegin(GL.GL_QUADS);

            GL.glVertex3f(-125f, -10f, (end + start) / 2 + 5);
            GL.glVertex3f(-125f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(-135f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(-135f, -10f, (end + start) / 2 + 5);

            GL.glVertex3f(-125f, -10f, (end + start) / 2 + 15);
            GL.glVertex3f(-125f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(-135f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(-135f, -10f, (end + start) / 2 + 15);

            GL.glVertex3f(-135f, -10f, (end + start) / 2 + 5);
            GL.glVertex3f(-135f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(-135f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(-135f, -10f, (end + start) / 2 + 15);

            GL.glVertex3f(-125f, -10f, (end + start) / 2 + 15);
            GL.glVertex3f(-125f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(-125f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(-125f, -10f, (end + start) / 2 + 5);

            GL.glVertex3f(125f, -10f, (end + start) / 2 + 5);
            GL.glVertex3f(125f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(135f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(135f, -10f, (end + start) / 2 + 5);

            GL.glVertex3f(125f, -10f, (end + start) / 2 + 15);
            GL.glVertex3f(125f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(135f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(135f, -10f, (end + start) / 2 + 15);

            GL.glVertex3f(135f, -10f, (end + start) / 2 + 5);
            GL.glVertex3f(135f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(135f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(135f, -10f, (end + start) / 2 + 15);

            GL.glVertex3f(125f, -10f, (end + start) / 2 + 15);
            GL.glVertex3f(125f, 30f, (end + start) / 2 + 15);
            GL.glVertex3f(125f, 30f, (end + start) / 2 + 5);
            GL.glVertex3f(125f, -10f, (end + start) / 2 + 5);

            GL.glEnd();
            GL.glColor3f(1, 1, 1);
        }

        void DrawTexturedCube()
        {
            GL.glEnable(GL.GL_CULL_FACE);
            GL.glCullFace(GL.GL_BACK);
            GL.glCullFace(GL.GL_FRONT);

            // front
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[0]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1000.0f, -1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(1000.0f, -1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(1000.0f, 1000.0f, 1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1000.0f, 1000.0f, 1000.0f);
            GL.glEnd();
            // back
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[1]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-1000.0f, 1000.0f, -1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(1000.0f, 1000.0f, -1000.0f);
            GL.glEnd();
            // left
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[2]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(-1000.0f, -1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(-1000.0f, 1000.0f, 1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1000.0f, 1000.0f, -1000.0f);
            GL.glEnd();
            // right
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[3]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(1000.0f, -1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(1000.0f, 1000.0f, -1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(1000.0f, 1000.0f, 1000.0f);
            GL.glEnd();
            // top
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[4]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1000.0f, 1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(1000.0f, 1000.0f, 1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(1000.0f, 1000.0f, -1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1000.0f, 1000.0f, -1000.0f);
            GL.glEnd();
            // bottom
            GL.glBindTexture(GL.GL_TEXTURE_2D, Textures[5]);
            GL.glBegin(GL.GL_QUADS);
            GL.glTexCoord2f(0.0f, 0.0f); GL.glVertex3f(-1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 0.0f); GL.glVertex3f(1000.0f, -1000.0f, -1000.0f);
            GL.glTexCoord2f(1.0f, 1.0f); GL.glVertex3f(1000.0f, -1000.0f, 1000.0f);
            GL.glTexCoord2f(0.0f, 1.0f); GL.glVertex3f(-1000.0f, -1000.0f, 1000.0f);
            GL.glEnd();
            GL.glDisable(GL.GL_CULL_FACE);
        }

        void DrawAxes()
        {
            GL.glBegin(GL.GL_LINES);
            //x  RED
            GL.glColor3f(1.0f, 0.0f, 0.0f);
            GL.glVertex3f(-300.0f, 0.0f, 0.0f);
            GL.glVertex3f(300.0f, 0.0f, 0.0f);
            //y  GREEN 
            GL.glColor3f(0.0f, 1.0f, 0.0f);
            GL.glVertex3f(0.0f, -300.0f, 0.0f);
            GL.glVertex3f(0.0f, 300.0f, 0.0f);
            //z  BLUE
            GL.glColor3f(0.0f, 0.0f, 1.0f);
            GL.glVertex3f(0.0f, 0.0f, -300.0f);
            GL.glVertex3f(0.0f, 0.0f, 300.0f);
            GL.glEnd();
            GL.glColor3f(1.0f, 1.0f, 1.0f);
        }

        void ReduceToUnit(float[] vector)
        {
            float length;

            length = (float)Math.Sqrt((vector[0] * vector[0]) +
                                (vector[1] * vector[1]) +
                                (vector[2] * vector[2]));

            if (length == 0.0f)
                length = 1.0f;

            vector[0] /= length;
            vector[1] /= length;
            vector[2] /= length;
        }

        void calcNormal(float[,] v, float[] outp)
        {
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            v1[x] = v[0, x] - v[1, x];
            v1[y] = v[0, y] - v[1, y];
            v1[z] = v[0, z] - v[1, z];

            v2[x] = v[1, x] - v[2, x];
            v2[y] = v[1, y] - v[2, y];
            v2[z] = v[1, z] - v[2, z];

            outp[x] = v1[y] * v2[z] - v1[z] * v2[y];
            outp[y] = v1[z] * v2[x] - v1[x] * v2[z];
            outp[z] = v1[x] * v2[y] - v1[y] * v2[x];

            ReduceToUnit(outp);
        }

        void MakeShadowMatrix(float[,] points)
        {
            float[] planeCoeff = new float[4];
            float dot;

            calcNormal(points, planeCoeff);
            planeCoeff[3] = -(
                (planeCoeff[0] * points[2, 0]) + (planeCoeff[1] * points[2, 1]) +
                (planeCoeff[2] * points[2, 2]));


            dot = planeCoeff[0] * pos[0] +
                    planeCoeff[1] * pos[1] +
                    planeCoeff[2] * pos[2] +
                    planeCoeff[3];

            cubeXform[0] = dot - pos[0] * planeCoeff[0];
            cubeXform[4] = 0.0f - pos[0] * planeCoeff[1];
            cubeXform[8] = 0.0f - pos[0] * planeCoeff[2];
            cubeXform[12] = 0.0f - pos[0] * planeCoeff[3];

            cubeXform[1] = 0.0f - pos[1] * planeCoeff[0];
            cubeXform[5] = dot - pos[1] * planeCoeff[1];
            cubeXform[9] = 0.0f - pos[1] * planeCoeff[2];
            cubeXform[13] = 0.0f - pos[1] * planeCoeff[3];

            cubeXform[2] = 0.0f - pos[2] * planeCoeff[0];
            cubeXform[6] = 0.0f - pos[2] * planeCoeff[1];
            cubeXform[10] = dot - pos[2] * planeCoeff[2];
            cubeXform[14] = 0.0f - pos[2] * planeCoeff[3];

            cubeXform[3] = 0.0f - pos[3] * planeCoeff[0];
            cubeXform[7] = 0.0f - pos[3] * planeCoeff[1];
            cubeXform[11] = 0.0f - pos[3] * planeCoeff[2];
            cubeXform[15] = dot - pos[3] * planeCoeff[3];
        }

    }
}