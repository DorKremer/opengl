using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using MathEX;
using OpenGL;

namespace Milkshape
{
	/// MS3D Model Class
    public class MilkshapeModel
    {
        #region MS3D Structures for Deserialization
        /// MS3D File header
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DHeader
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] ID;
            public int version;
        }

        /// MS3D Vertex information
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DVertex
        {
            public byte flags;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] vertex;
            public char boneID;
            public byte refCount;
        }

        /// MS3D Triangle information
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DTriangle
        {
            public short flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] vertexIndices;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 9)]
            public float[] vertexNormals; //[3],[3]
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] s;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] t;
            public byte smoothingGroup;
            public byte groupIndex;
        }

        /// MS3D Material information
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DMaterial
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] name;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
            public float[] ambient;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
            public float[] diffuse;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
            public float[] specular;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
            public float[] emissive;
            [MarshalAs(UnmanagedType.R4)]
            public float shininess;	// 0.0f - 128.0f
            [MarshalAs(UnmanagedType.R4)]
            public float transparency;	// 0.0f - 1.0f
            public char mode;	// 0, 1, 2 is unused now
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] texture;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] alphamap;
        }

        /// MS3D Joint information
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DJoint
        {
            public byte flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] parentName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] rotation;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] translation;
            public short numRotationKeyframes;
            public short numTranslationKeyframes;
        }

        /// MS3D Keyframe data
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MS3DKeyframe
        {
            [MarshalAs(UnmanagedType.R4)]
            public float time;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] parameter;
        }
        #endregion

        #region Variables
        internal Model model_data;					// Generic model object holds loaded data
        private static uint texCount = 6;			// OpenGL texture ID counter
        protected DateTime lastTime = DateTime.Now;	// Used to calculate animation time
        protected Animation currentAnimation;		// Index of the current animation
        public string fileFolder;
        internal bool Animate = false;				// When true, ms3d file should animate
        protected bool looping = true;              // When true, ms3d file should loop the animation
        #endregion

        #region LoadData
        /// Loads the MS3D file
        public virtual void LoadModelData(string filename)
        {
            model_data = new Model();
            looping = true;
            System.IO.FileInfo fInfo = new System.IO.FileInfo(filename);
            if (!fInfo.Exists)
            {
                // Display an error message and don't load anything if no file was found
                MessageBox.Show("Unable to find the file: " + filename, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fileFolder = fInfo.DirectoryName;

            System.IO.Stream fs = new System.IO.FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

            long totalLength = fs.Length;

            // Read the header data and store it in our m_Header member variable
            byte[] b = new byte[Marshal.SizeOf(typeof(MS3DHeader))];

            fs.Read(b, 0, b.Length);
            MS3DHeader m_Header = (MS3DHeader)RawDeserializeEx(b, typeof(MS3DHeader));
            string id = new string(m_Header.ID);
            // Only Milkshape3D Version 1.3 and 1.4 is supported.

            #region Vertices
            b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, Marshal.SizeOf(typeof(short)));
            short nVertices = (short)RawDeserializeEx(b, typeof(short));
            model_data.Vertices = new Vertex[nVertices];
            model_data.numVertices = nVertices;

            for (int i = 0; i < nVertices; i++)
            {
                b = new byte[Marshal.SizeOf(typeof(MS3DVertex))];
                fs.Read(b, 0, b.Length);
                MS3DVertex Vertex = (MS3DVertex)RawDeserializeEx(b, typeof(MS3DVertex));
                model_data.Vertices[i] = new Vertex();
                model_data.Vertices[i].boneID = Vertex.boneID;
                model_data.Vertices[i].location = Vertex.vertex;
            }
            #endregion

            #region Triangles
            b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, Marshal.SizeOf(typeof(short)));
            short nTriangles = (short)RawDeserializeEx(b, typeof(short));
            model_data.Triangles = new Triangle[nTriangles];
            model_data.numTriangles = nTriangles;

            for (int i = 0; i < nTriangles; i++)
            {
                int[] vertexIndices = new int[3] { 0, 0, 0 };
                float[] t = new float[3] { 0, 0, 0 };

                b = new byte[Marshal.SizeOf(typeof(MS3DTriangle))];
                fs.Read(b, 0, b.Length);
                MS3DTriangle pTriangle = (MS3DTriangle)RawDeserializeEx(b, typeof(MS3DTriangle));

                model_data.Triangles[i] = new Triangle();

                vertexIndices[0] = (int)pTriangle.vertexIndices[0];
                vertexIndices[1] = (int)pTriangle.vertexIndices[1];
                vertexIndices[2] = (int)pTriangle.vertexIndices[2];
                t[0] = 1.0f - pTriangle.t[0];
                t[1] = 1.0f - pTriangle.t[1];
                t[2] = 1.0f - pTriangle.t[2];
                model_data.Triangles[i].vertexNormals = pTriangle.vertexNormals;
                model_data.Triangles[i].s = pTriangle.s;
                model_data.Triangles[i].t = t;
                model_data.Triangles[i].vertexIndices = vertexIndices;
            }
            #endregion

            #region Groups
            b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, Marshal.SizeOf(typeof(short)));
            short nGroups = (short)RawDeserializeEx(b, typeof(short));
            model_data.Meshes = new Mesh[nGroups];
            model_data.numMeshes = nGroups;

            for (int i = 0; i < nGroups; i++)
            {
                b = new byte[Marshal.SizeOf(typeof(byte))];
                fs.Read(b, 0, b.Length); // Flags

                b = new byte[32];
                fs.Read(b, 0, b.Length); // name
                string name = System.Text.Encoding.UTF8.GetString(b);

                b = new byte[Marshal.SizeOf(typeof(short))];
                fs.Read(b, 0, b.Length); // number of Triangle Indices
                nTriangles = (short)RawDeserializeEx(b, typeof(short));
                int[] pTriangleIndices = new int[nTriangles];

                for (int j = 0; j < nTriangles; j++)
                {
                    b = new byte[Marshal.SizeOf(typeof(short))];
                    fs.Read(b, 0, b.Length); // read indices value
                    pTriangleIndices[j] = (short)RawDeserializeEx(b, typeof(short));
                }

                b = new byte[Marshal.SizeOf(typeof(char))];
                fs.Read(b, 0, b.Length); // read material index
                char materialIndex = (char)RawDeserializeEx(b, typeof(char));

                model_data.Meshes[i] = new Mesh();
                model_data.Meshes[i].materialIndex = (int)materialIndex;
                model_data.Meshes[i].numTriangles = nTriangles;
                model_data.Meshes[i].TriangleIndices = pTriangleIndices;
            }
            #endregion

            #region Materials
            b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, Marshal.SizeOf(typeof(short)));
            short nMaterials = (short)RawDeserializeEx(b, typeof(short));
            model_data.Materials = new Material[nMaterials];
            model_data.numMaterials = nMaterials;

            for (int i = 0; i < nMaterials; i++)
            {
                b = new byte[Marshal.SizeOf(typeof(MS3DMaterial))];
                fs.Read(b, 0, b.Length); // read material
                MS3DMaterial pMaterial = (MS3DMaterial)RawDeserializeEx(b, typeof(MS3DMaterial));
                model_data.Materials[i] = new Material();
                model_data.Materials[i].name = pMaterial.name;
                model_data.Materials[i].ambient = pMaterial.ambient;
                model_data.Materials[i].diffuse = pMaterial.diffuse;
                model_data.Materials[i].specular = pMaterial.specular;
                model_data.Materials[i].emissive = pMaterial.emissive;
                model_data.Materials[i].shininess = pMaterial.shininess;
				model_data.Materials[i].transparency = pMaterial.transparency;
                model_data.Materials[i].mode = pMaterial.mode;
                model_data.Materials[i].TextureFilename = CropNull(new string(pMaterial.texture));
                model_data.Materials[i].alpha = pMaterial.alphamap;

                // set alpha
                model_data.Materials[i].ambient[3] = model_data.Materials[i].transparency;
                model_data.Materials[i].diffuse[3] = model_data.Materials[i].transparency;
                model_data.Materials[i].specular[3] = model_data.Materials[i].transparency;
                model_data.Materials[i].emissive[3] = model_data.Materials[i].transparency;
            }
            #endregion

            ReloadTextures();
			
            #region Animation
            b = new byte[Marshal.SizeOf(typeof(float))];
            fs.Read(b, 0, b.Length);
            float animFPS = (float)RawDeserializeEx(b, typeof(float));
            if (animFPS < 1.0f)
                animFPS = 1.0f;

            // Skip the currentTime value
            fs.Read(b, 0, b.Length);

            b = new byte[Marshal.SizeOf(typeof(int))];
            fs.Read(b, 0, b.Length);
            int totalFrames = (int)RawDeserializeEx(b, typeof(int));
            
			model_data.totalTime = totalFrames * 1000f / animFPS;
            #endregion

            #region Joints
            b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, b.Length);
            model_data.numJoints = (short)RawDeserializeEx(b, typeof(short));
            model_data.Joints = new Joint[model_data.numJoints];

            JointNameListRec[] pNameList = new JointNameListRec[model_data.numJoints];

            #region Building JointNameListRec
            long tempPos = fs.Position;

            for (int i = 0; i < model_data.numJoints; i++)
            {
                b = new byte[Marshal.SizeOf(typeof(MS3DJoint))];
                fs.Read(b, 0, b.Length);
                MS3DJoint pJoint = (MS3DJoint)RawDeserializeEx(b, typeof(MS3DJoint));

                pNameList[i] = new JointNameListRec();
                pNameList[i].jointIndex = i;
                pNameList[i].Name = CropNull(new string(pJoint.name));

                // skip forward for the next joint
                b = new byte[Marshal.SizeOf(typeof(MS3DKeyframe)) * (pJoint.numRotationKeyframes + pJoint.numTranslationKeyframes)];
                fs.Read(b, 0, b.Length);
            }

            fs.Seek(tempPos, System.IO.SeekOrigin.Begin);
            #endregion

            #region Load Joints
            for (int i = 0; i < model_data.numJoints; i++)
            {
                b = new byte[Marshal.SizeOf(typeof(MS3DJoint))];
                fs.Read(b, 0, b.Length);
                MS3DJoint pJoint = (MS3DJoint)RawDeserializeEx(b, typeof(MS3DJoint));

                int parentIndex = -1;
                string parentName = CropNull(new string(pJoint.parentName));
                if (parentName.Length > 0)
                {
                    for (int j = 0; j < model_data.numJoints; j++)
                    {
                        if (pNameList[j].Name == parentName)
                        {
                            parentIndex = pNameList[j].jointIndex;
                            break;
                        }
                    }

                    if (parentIndex == -1)
                        return;
                }

                model_data.Joints[i] = new Joint();
                model_data.Joints[i].localRotation = pJoint.rotation;
                model_data.Joints[i].localTranslation = pJoint.translation;
                model_data.Joints[i].parent = parentIndex;
                model_data.Joints[i].numRotationKeyframes = pJoint.numRotationKeyframes;
                model_data.Joints[i].RotationKeyframes = new Keyframe[pJoint.numRotationKeyframes];
                model_data.Joints[i].numTranslationKeyframes = pJoint.numTranslationKeyframes;
                model_data.Joints[i].TranslationKeyframes = new Keyframe[pJoint.numTranslationKeyframes];

                model_data.Joints[i].mat_relative = new Matrix();
                model_data.Joints[i].mat_final = new Matrix();
                model_data.Joints[i].mat_absolute = new Matrix();

                // the frame time is in seconds, so multiply it by the animation fps, to get the frames
                // rotation channel
                for (int j = 0; j < pJoint.numRotationKeyframes; j++)
                {
                    b = new byte[Marshal.SizeOf(typeof(MS3DKeyframe))];
                    fs.Read(b, 0, b.Length);
                    MS3DKeyframe pKeyframe = (MS3DKeyframe)RawDeserializeEx(b, typeof(MS3DKeyframe));

                    SetJointKeyframe(i, j, pKeyframe.time * 1000.0f, ref pKeyframe.parameter, true);
                }

                // translation channel
                for (int j = 0; j < pJoint.numTranslationKeyframes; j++)
                {
                    b = new byte[Marshal.SizeOf(typeof(MS3DKeyframe))];
                    fs.Read(b, 0, b.Length);
                    MS3DKeyframe pKeyframe = (MS3DKeyframe)RawDeserializeEx(b, typeof(MS3DKeyframe));

                    SetJointKeyframe(i, j, pKeyframe.time * 1000.0f, ref pKeyframe.parameter, false);
                }
            }
            #endregion

            pNameList = null;
            #endregion

            #region Comments
            if (fs.Position < totalLength)
            {
                int subVersion = 0;
                b = new byte[Marshal.SizeOf(typeof(int))];
                fs.Read(b, 0, b.Length);
                subVersion = (int)RawDeserializeEx(b, typeof(int));

                if (subVersion == 1)
                {
                    int numComments = 0;
                    uint commentSize = 0;

                    #region Group Comments
                    b = new byte[Marshal.SizeOf(typeof(int))];
                    fs.Read(b, 0, b.Length);
                    numComments = (int)RawDeserializeEx(b, typeof(int));

                    for (int i = 0; i < numComments; i++)
                    {
                        int index;
                        string comment = "";

                        fs.Read(b, 0, b.Length);
                        index = (int)RawDeserializeEx(b, typeof(int));

                        b = new byte[Marshal.SizeOf(typeof(uint))];
                        fs.Read(b, 0, b.Length);
                        commentSize = (uint)RawDeserializeEx(b, typeof(uint));

                        if (commentSize > 0)
                        {
                            b = new byte[Marshal.SizeOf(sizeof(char) * commentSize)];
                            fs.Read(b, 0, b.Length);
                            comment = System.Text.Encoding.ASCII.GetString(b);
                        }
                        if (index >= 0 && index < model_data.numMeshes)
                            model_data.Meshes[index].comment = comment;
                    }
                    #endregion

                    #region Material Comments
                    b = new byte[Marshal.SizeOf(typeof(int))];
                    fs.Read(b, 0, b.Length);
                    numComments = (int)RawDeserializeEx(b, typeof(int));

                    for (int i = 0; i < numComments; i++)
                    {
                        int index;
                        string comment = "";

                        fs.Read(b, 0, b.Length);
                        index = (int)RawDeserializeEx(b, typeof(int));

                        b = new byte[Marshal.SizeOf(typeof(uint))];
                        fs.Read(b, 0, b.Length);
                        commentSize = (uint)RawDeserializeEx(b, typeof(uint));

                        if (commentSize > 0)
                        {
                            b = new byte[Marshal.SizeOf(sizeof(char) * commentSize)];
                            fs.Read(b, 0, b.Length);
                            comment = System.Text.Encoding.ASCII.GetString(b);
                        }
                        if (index >= 0 && index < model_data.numMaterials)
                            model_data.Materials[index].comment = comment;
                    }
                    #endregion

                    #region Joint Comments
                    b = new byte[Marshal.SizeOf(typeof(int))];
                    fs.Read(b, 0, b.Length);
                    numComments = (int)RawDeserializeEx(b, typeof(int));

                    for (int i = 0; i < numComments; i++)
                    {
                        int index;
                        string comment = "";

                        fs.Read(b, 0, b.Length);
                        index = (int)RawDeserializeEx(b, typeof(int));

                        b = new byte[Marshal.SizeOf(typeof(uint))];
                        fs.Read(b, 0, b.Length);
                        commentSize = (uint)RawDeserializeEx(b, typeof(uint));

                        if (commentSize > 0)
                        {
                            b = new byte[Marshal.SizeOf(sizeof(char) * commentSize)];
                            fs.Read(b, 0, b.Length);
                            comment = System.Text.Encoding.ASCII.GetString(b);
                        }
                        if (index >= 0 && index < model_data.numJoints)
                        model_data.Joints[index].comment = comment;
                    }
                    #endregion

                    #region Model Comments
                    b = new byte[Marshal.SizeOf(typeof(int))];
                    fs.Read(b, 0, b.Length);
                    numComments = (int)RawDeserializeEx(b, typeof(int));

                    if (numComments == 1)
                    {
                        string comment = "";
                        b = new byte[Marshal.SizeOf(typeof(uint))];
                        fs.Read(b, 0, b.Length);
                        commentSize = (uint)RawDeserializeEx(b, typeof(uint));
                        if (commentSize > 0)
                        {
                            b = new byte[Marshal.SizeOf(sizeof(char) * commentSize)];
                            fs.Read(b, 0, b.Length);
                            comment = System.Text.Encoding.ASCII.GetString(b);
                        }
                        model_data.comment = comment;
                    }
                    #endregion
                }
            }
            #endregion

            #region Vertex Extra
            if (fs.Position < totalLength)
            {
                int subVersion = 0;
                b = new byte[Marshal.SizeOf(typeof(int))];
                fs.Read(b, 0, b.Length);
                subVersion = (int)RawDeserializeEx(b, typeof(int));
                if ((subVersion == 1) || (subVersion == 2))
                {
                    for (int i = 0; i < model_data.numVertices; i++)
                    {
                        model_data.Vertices[i].boneIds = new char[3];
                        b = new byte[Marshal.SizeOf(typeof(char))];
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].boneIds[0] = (char)RawDeserializeEx(b, typeof(char));
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].boneIds[1] = (char)RawDeserializeEx(b, typeof(char));
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].boneIds[2] = (char)RawDeserializeEx(b, typeof(char));

                        model_data.Vertices[i].weights = new char[3];
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].weights[0] = (char)RawDeserializeEx(b, typeof(char));
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].weights[1] = (char)RawDeserializeEx(b, typeof(char));
                        fs.Read(b, 0, b.Length);
                        model_data.Vertices[i].weights[2] = (char)RawDeserializeEx(b, typeof(char));

                        if (subVersion == 2)
                        {
                            b = new byte[Marshal.SizeOf(typeof(uint))];
                            fs.Read(b, 0, b.Length);
                            model_data.Vertices[i].extra = (uint)RawDeserializeEx(b, typeof(uint));
                        }
                    }
                }
            }
            #endregion

            #region Joint Extra
            if (fs.Position < totalLength)
            {
                int subVersion = 0;
                b = new byte[Marshal.SizeOf(typeof(int))];
                fs.Read(b, 0, b.Length);
                subVersion = (int)RawDeserializeEx(b, typeof(int));

                if (subVersion == 1)
                {
                    for (int i = 0; i < model_data.numJoints; i++)
                    {
                        model_data.Joints[i].color = new float[3];
                        b = new byte[Marshal.SizeOf(typeof(float))];
                        fs.Read(b, 0, b.Length);
                        model_data.Joints[i].color[0] = (float)RawDeserializeEx(b, typeof(float));
                        fs.Read(b, 0, b.Length);
                        model_data.Joints[i].color[1] = (float)RawDeserializeEx(b, typeof(float));
                        fs.Read(b, 0, b.Length);
                        model_data.Joints[i].color[2] = (float)RawDeserializeEx(b, typeof(float));
                    }
                }
            }
            #endregion

            #region Model Extra
            if (fs.Position < totalLength)
            {
                int subVersion = 0;
                b = new byte[Marshal.SizeOf(typeof(int))];
                fs.Read(b, 0, b.Length);
                subVersion = (int)RawDeserializeEx(b, typeof(int));

                if (subVersion == 1)
                {
                    b = new byte[Marshal.SizeOf(typeof(float))];
                    fs.Read(b, 0, b.Length);
                    model_data.jointSize = (float)RawDeserializeEx(b, typeof(float));

                    b = new byte[Marshal.SizeOf(typeof(int))];
                    fs.Read(b, 0, b.Length);
                    model_data.transparencyMode = (int)RawDeserializeEx(b, typeof(int));

                    b = new byte[Marshal.SizeOf(typeof(float))];
                    fs.Read(b, 0, b.Length);
                    model_data.alphaRef = (float)RawDeserializeEx(b, typeof(float));
                }
            }
            #endregion

            fs.Close();

            try { SetupJoints(); }
            catch { }
        }

        private void SetJointKeyframe(int jointIndex, int keyframeIndex, float time, ref float[] parameter, bool isRotation)
        {
            if (isRotation)
            {
                model_data.Joints[jointIndex].RotationKeyframes[keyframeIndex] = new Keyframe();
                model_data.Joints[jointIndex].RotationKeyframes[keyframeIndex].jointIndex = jointIndex;
                model_data.Joints[jointIndex].RotationKeyframes[keyframeIndex].time = time;
                model_data.Joints[jointIndex].RotationKeyframes[keyframeIndex].parameter = parameter;
            }
            else
            {
                model_data.Joints[jointIndex].TranslationKeyframes[keyframeIndex] = new Keyframe();
                model_data.Joints[jointIndex].TranslationKeyframes[keyframeIndex].jointIndex = jointIndex;
                model_data.Joints[jointIndex].TranslationKeyframes[keyframeIndex].time = time;
                model_data.Joints[jointIndex].TranslationKeyframes[keyframeIndex].parameter = parameter;
            }
        }

        private void SetupJoints()
        {
            for (int i = 0; i < model_data.numJoints; i++)
            {
                Joint joint = model_data.Joints[i];

                joint.mat_relative.SetRotationRadians(joint.localRotation);
                joint.mat_relative.SetTranslation(joint.localTranslation);

                if (joint.parent != -1)
                {
                    joint.mat_absolute.Set(model_data.Joints[joint.parent].mat_absolute);
                    joint.mat_absolute.PostMultiply(joint.mat_relative);
                }
                else
                    joint.mat_absolute.Set(joint.mat_relative);

                model_data.Joints[i] = joint;
            }

            for (int i = 0; i < model_data.numVertices; i++)
            {
                Vertex vertex = model_data.Vertices[i];

                if ((int)vertex.boneID != 255)
                {
                    Matrix matrix = model_data.Joints[vertex.boneID].mat_absolute;

                    matrix.InverseTranslateVect(ref vertex.location);
                    matrix.InverseRotateVect(ref vertex.location);
                }
                model_data.Vertices[i] = vertex;
            }

            for (int i = 0; i < model_data.numTriangles; i++)
            {
                Triangle triangle = model_data.Triangles[i];

                for (int j = 0; j < 3; j++)
                {

                    Vertex vertex = model_data.Vertices[triangle.vertexIndices[j]];
                    if ((int)vertex.boneID != 255)
                    {
                        Matrix matrix = model_data.Joints[vertex.boneID].mat_absolute;

                        int scalar = (j * 3);
                        float[] norm = new float[3] { triangle.vertexNormals[0 + scalar], triangle.vertexNormals[1 + scalar], triangle.vertexNormals[2 + scalar] };

                        matrix.InverseRotateVect(ref norm);

                        triangle.vertexNormals[0 + scalar] = norm[0];
                        triangle.vertexNormals[1 + scalar] = norm[1];
                        triangle.vertexNormals[2 + scalar] = norm[2];
                    }
                }
                model_data.Triangles[i] = triangle;
            }
        }
        #endregion

        #region Textures
        public void ReloadTextures()
        {
            for (int i = 0; i < model_data.numMaterials; i++)
                if (model_data.Materials[i].TextureFilename.Length > 0)
                    model_data.Materials[i].texture = CreateTexture(fileFolder + "/" + model_data.Materials[i].TextureFilename);
                else
                    model_data.Materials[i].texture = 0;
        }

        private uint CreateTexture(string fileName)
        {
            Bitmap bitmap = null;													// The Bitmap Image For Our Texture
            Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
            BitmapData bitmapData = null;											// The Bitmap's Pixel Data

            texCount++;

            bitmap = new Bitmap(fileName);										// Load The File As A Bitmap

            if (bitmap != null)
            {
                bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);					// Flip The Bitmap Along The Y-Axis
                rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);	// Select The Whole Bitmap

                // Get The Pixel Data From The Locked Bitmap
                bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                GL.glBindTexture(GL.GL_TEXTURE_2D, texCount);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, GL.GL_LINEAR);
                GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, GL.GL_LINEAR);
                GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)GL.GL_RGB8, bitmap.Width, bitmap.Height, 0, GL.GL_BGR_EXT, GL.GL_UNSIGNED_byte, bitmapData.Scan0);
            
                bitmap.UnlockBits(bitmapData);							// Unlock The Pixel Data From Memory
                bitmap.Dispose();												// Clean Up The Bitmap
            }

            return texCount;
        }
        #endregion

        public virtual void BindMaterial(int materialIndex)
        {
            GL.glEnable(GL.GL_COLOR_MATERIAL);
            if (materialIndex < 0 || materialIndex >= model_data.numMaterials)
            {
                GL.glDepthMask(GL.GL_TRUE);
                GL.glDisable(GL.GL_ALPHA_TEST);
                GL.glColor4f(1, 1, 1, 1);
                GL.glDisable(GL.GL_TEXTURE_2D);
                GL.glDisable(GL.GL_BLEND);
                GL.glBindTexture(GL.GL_TEXTURE_2D, 0);
                float[] ma = { 0.2f, 0.2f, 0.2f, 1.0f };
                float[] md = { 0.8f, 0.8f, 0.8f, 1.0f };
                float[] ms = { 0.0f, 0.0f, 0.0f, 1.0f };
                float[] me = { 0.0f, 0.0f, 0.0f, 1.0f };
                float mss = 0.0f;
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, ma);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, md);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, ms);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_EMISSION, me);
                GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, mss);
            }
            else
            {
                Material material = model_data.Materials[materialIndex];

                if ((material.transparency < 1.0f) || (material.mode == Model.HASALPHA))
                {
                    GL.glEnable(GL.GL_BLEND);
                    GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);
                    GL.glColor4f(1.0f, 1.0f, 1.0f, material.transparency);
                    GL.glLightModeli(GL.GL_LIGHT_MODEL_TWO_SIDE, 1);

                    if (model_data.transparencyMode == Model.TRANSPARENCY_MODE_SIMPLE)
                    {
                        GL.glDepthMask(GL.GL_FALSE);
                        GL.glEnable(GL.GL_ALPHA_TEST);
                        GL.glAlphaFunc(GL.GL_GREATER, 0.0f);
                    }
                    else
                        if (model_data.transparencyMode == Model.TRANSPARENCY_MODE_ALPHAREF)
                        {
                            GL.glDepthMask(GL.GL_TRUE);
                            GL.glEnable(GL.GL_ALPHA_TEST);
                            GL.glAlphaFunc(GL.GL_GREATER, model_data.alphaRef);
                        }
                }
                else
                {
                    GL.glDisable(GL.GL_BLEND);
                    GL.glColor4f(1.0f, 1.0f, 1.0f, 1.0f);
                    GL.glLightModeli(GL.GL_LIGHT_MODEL_TWO_SIDE, 0);
                }

                if (material.mode == Model.SPHEREMAP)
                {
                    GL.glEnable(GL.GL_TEXTURE_GEN_S);
                    GL.glEnable(GL.GL_TEXTURE_GEN_T);
                    GL.glTexGeni(GL.GL_S, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                    GL.glTexGeni(GL.GL_T, GL.GL_TEXTURE_GEN_MODE, (int)GL.GL_SPHERE_MAP);
                }
                else
                {
                    GL.glDisable(GL.GL_TEXTURE_GEN_S);
                    GL.glDisable(GL.GL_TEXTURE_GEN_T);
                }

                GL.glBindTexture(GL.GL_TEXTURE_2D, material.texture);
                GL.glEnable(GL.GL_TEXTURE_2D);

                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_AMBIENT, material.ambient);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_DIFFUSE, material.diffuse);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_SPECULAR, material.specular);
                GL.glMaterialfv(GL.GL_FRONT_AND_BACK, GL.GL_EMISSION, material.emissive);
                GL.glMaterialf(GL.GL_FRONT_AND_BACK, GL.GL_SHININESS, material.shininess);
            }
        }

        /// Renders model into current OpenGL context
        public virtual void DrawModel()
        {
            // Draw by group
            for (int i = 0; i < model_data.numMeshes; i++)
            {
                BindMaterial(model_data.Meshes[i].materialIndex);

                GL.glBegin(GL.GL_TRIANGLES);
                for (int j = 0; j < model_data.Meshes[i].numTriangles; j++)
                {
                    int triangleIndex = model_data.Meshes[i].TriangleIndices[j];
                    Triangle pTri = model_data.Triangles[triangleIndex];
                    for (int k = 0; k < 3; k++)
                    {
                        int index = pTri.vertexIndices[k];

                        int scalar = (k * 3);
                        float[] norm = new float[3] { pTri.vertexNormals[0 + scalar], pTri.vertexNormals[1 + scalar], pTri.vertexNormals[2 + scalar] };

                        GL.glNormal3fv(norm);
                        GL.glTexCoord2f(pTri.s[k], pTri.t[k]);
                        GL.glVertex3fv(model_data.Vertices[index].location);
                    }
                }
                GL.glEnd();
            }
        }

        public virtual void DrawModelShadow()
        {
            // Draw by group
            for (int i = 0; i < model_data.numMeshes; i++)
            {

                GL.glBegin(GL.GL_TRIANGLES);
                GL.glColor4f(0.4f, 0.4f, 0.4f, 1);
                for (int j = 0; j < model_data.Meshes[i].numTriangles; j++)
                {
                    int triangleIndex = model_data.Meshes[i].TriangleIndices[j];
                    Triangle pTri = model_data.Triangles[triangleIndex];
                    for (int k = 0; k < 3; k++)
                    {
                        int index = pTri.vertexIndices[k];

                        int scalar = (k * 3);
                        float[] norm = new float[3] { pTri.vertexNormals[0 + scalar], pTri.vertexNormals[1 + scalar], pTri.vertexNormals[2 + scalar] };

                        GL.glNormal3fv(norm);
                        GL.glTexCoord2f(pTri.s[k], pTri.t[k]);
                        GL.glVertex3fv(model_data.Vertices[index].location);
                    }
                }
                GL.glEnd();
            }
        }

        /// Converts a fixed char[] into a string by removing data after the first "\0" character
        private string CropNull(string input)
        {
            input = input.Trim();

            if (input.EndsWith(((char)13).ToString())
                && input.IndexOf('\0') == -1)
            {
                return input.Substring(0, input.Length - 1);
            }

            if (input.IndexOf('\0') == -1)
                return input;

            return input.Substring(0, input.IndexOf('\0'));
        }

        private static object RawDeserializeEx(byte[] rawdatas, Type anytype)
        {
            int rawsize = Marshal.SizeOf(anytype);
            if (rawsize > rawdatas.Length)
                return null;
            GCHandle handle = GCHandle.Alloc(rawdatas, GCHandleType.Pinned);
            IntPtr buffer = handle.AddrOfPinnedObject();
            object retobj = Marshal.PtrToStructure(buffer, anytype);
            handle.Free();
            return retobj;
        }
    }
}