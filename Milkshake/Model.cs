using System;
using MathEX;

namespace Milkshape
{
	/// Triangle Indicies collection
    public struct Mesh
	{
		public int materialIndex;
		public int numTriangles;
		public int[] TriangleIndices;
        public string comment;
	}

	/// Material properties
	public struct Material
    {
        public char[] name;
		public float[]  ambient;
		public float[] diffuse;
		public float[] specular;
		public float[] emissive;
        public float shininess;
        public float transparency;	// 0.0f - 1.0f
        public char mode;	// 0, 1, 2 is unused now
		public uint texture;
		public string TextureFilename;
        public char[] alpha;
        public string comment;
	}

	/// Triangle structure
	public struct Triangle
	{
		public float[] vertexNormals; //[3][3]
		public float[] s;
		public float[] t;
        public int[] vertexIndices;
	}

	/// Vertex structure
	public struct Vertex
	{
		public char boneID;	// for skeletal animation
        public float[] location;
        public char[] boneIds;
        public char[] weights;
        public uint extra;
	}

	/// Animation keyframe information
	public struct Keyframe
	{
		public int jointIndex;
		public float time;	// in milliseconds
		public float[] parameter;
	}

	/// Join name collection
	public class JointNameListRec
	{
		public int jointIndex;
		public string Name;
	}

	/// Skeleton bone joint
	public class Joint
	{
		public float[] localRotation;
		public float[] localTranslation;
		public Matrix mat_absolute;
		public Matrix mat_relative;

		public int numRotationKeyframes;
		public int numTranslationKeyframes;
		public Keyframe[] TranslationKeyframes;
		public Keyframe[] RotationKeyframes;

		public int currentTranslationKeyframe;
		public int currentRotationKeyframe;
		public Matrix mat_final;

        public int parent;
        public string comment;
        public float[] color;
    }

    /// Animation information structure
    public class Animation
    {
        public string Name;
        public int StartKeyframe;
        public int EndKeyFrame;
        public double StartTime;
        public double EndTime;
        public double Delay;
    }

	/// Generic Model Class for OpenGL
	public class Model
    {
        #region Model Texture Parameters
        public const int SPHEREMAP = 128;
        public const int HASALPHA = 64;
        public const int COMBINEALPHA = 32;

        public const int TRANSPARENCY_MODE_SIMPLE = 0;
        public const int TRANSPARENCY_MODE_DEPTHSORTEDTRIANGLES = 1;
        public const int TRANSPARENCY_MODE_ALPHAREF = 2;
        #endregion

        public Model()
        {
            numVertices = 0;
            Vertices = null;
            numMeshes = 0;
            Meshes = null;
            numTriangles = 0;
            Triangles = null;
            numMaterials = 0;
            Materials = null;
            numJoints = 0;
            Joints = null;

            totalTime = 0;
            Animations = null;

            comment = "";
            jointSize = 1.0f;
            transparencyMode = 0;
            alphaRef = 0.5f;
        }

		public int numMeshes;
		public Mesh[] Meshes;

		public int numMaterials;
		public Material[] Materials;

		public int numTriangles;
		public Triangle[] Triangles;

		public int numVertices;
		public Vertex[] Vertices;

		public int numJoints;
		public Joint[] Joints;

		public double totalTime;
        public Animation[] Animations;

        public string comment;
        public float jointSize;
        public int transparencyMode;
        public float alphaRef;
	}
}
