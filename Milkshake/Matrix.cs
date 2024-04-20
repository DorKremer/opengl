using System;

namespace MathEX
{
    public class Matrix
    {
        private float[] mat;

        public Matrix()
        {
            LoadIdentity();
        }

        public Matrix(float[] initalVals)
        {
            mat = new float[16];
            for (int i = 0; i < 16; i++)
                mat[i] = initalVals[i];
        }

        public Matrix(Matrix initalVals)
        {
            mat = new float[16];
            for (int i = 0; i < 16; i++)
                mat[i] = initalVals[i];
        }

        public float this[int index]
        {
            get { return mat[index]; }
            set { mat[index] = value; }
        }

        public int Length
        {
            get { return mat.Length; }
        }

        public void LoadIdentity()
        {
            mat = new float[16];
            mat[0] = mat[5] = mat[10] = mat[15] = 1.0f;
        }

        public void Set(Matrix pMatrix)
        {
            for (int i = 0; i < 16; i++)
                mat[i] = pMatrix[i];
        }

        public void InverseRotateVect(ref float[] vec)
        {
            float[] tmpvec = new float[9];

            tmpvec[0] = vec[0] * mat[0] + vec[1] * mat[1] + vec[2] * mat[2];
            tmpvec[1] = vec[0] * mat[4] + vec[1] * mat[5] + vec[2] * mat[6];
            tmpvec[2] = vec[0] * mat[8] + vec[1] * mat[9] + vec[2] * mat[10];

            vec = tmpvec;
        }

        public void TranslateVect(ref float[] vec)
        {
            vec[0] += mat[12];
            vec[1] += mat[13];
            vec[2] += mat[14];
        }

        public void InverseTranslateVect(ref float[] vec)
        {
            vec[0] -= mat[12];
            vec[1] -= mat[13];
            vec[2] -= mat[14];
        }

        public void SetRotationRadians(float[] angles)
        {
            double cr = Math.Cos(angles[0]);
            double sr = Math.Sin(angles[0]);
            double cp = Math.Cos(angles[1]);
            double sp = Math.Sin(angles[1]);
            double cy = Math.Cos(angles[2]);
            double sy = Math.Sin(angles[2]);

            mat[0] = (float)(cp * cy);
            mat[1] = (float)(cp * sy);
            mat[2] = (float)(-sp);
            mat[3] = (float)(0.0f);

            double srsp = sr * sp;
            double crsp = cr * sp;

            mat[4] = (float)(srsp * cy - cr * sy);
            mat[5] = (float)(srsp * sy + cr * cy);
            mat[6] = (float)(sr * cp);

            mat[8] = (float)(crsp * cy + sr * sy);
            mat[9] = (float)(crsp * sy - sr * cy);
            mat[10] = (float)(cr * cp);
        }

        public void RotateVect(ref float[] vec)
        {
            float[] tmpvec = new float[3];

            tmpvec[0] = vec[0] * mat[0] + vec[1] * mat[4] + vec[2] * mat[8];
            tmpvec[1] = vec[0] * mat[1] + vec[1] * mat[5] + vec[2] * mat[9];
            tmpvec[2] = vec[0] * mat[2] + vec[1] * mat[6] + vec[2] * mat[10];

            vec = tmpvec;
        }

        public void PostMultiply(Matrix matrix)
        {
            float[] newMatrix = new float[16];

            newMatrix[0] = mat[0] * matrix[0] + mat[4] * matrix[1] + mat[8] * matrix[2];
            newMatrix[1] = mat[1] * matrix[0] + mat[5] * matrix[1] + mat[9] * matrix[2];
            newMatrix[2] = mat[2] * matrix[0] + mat[6] * matrix[1] + mat[10] * matrix[2];
            newMatrix[3] = 0;

            newMatrix[4] = mat[0] * matrix[4] + mat[4] * matrix[5] + mat[8] * matrix[6];
            newMatrix[5] = mat[1] * matrix[4] + mat[5] * matrix[5] + mat[9] * matrix[6];
            newMatrix[6] = mat[2] * matrix[4] + mat[6] * matrix[5] + mat[10] * matrix[6];
            newMatrix[7] = 0;

            newMatrix[8] = mat[0] * matrix[8] + mat[4] * matrix[9] + mat[8] * matrix[10];
            newMatrix[9] = mat[1] * matrix[8] + mat[5] * matrix[9] + mat[9] * matrix[10];
            newMatrix[10] = mat[2] * matrix[8] + mat[6] * matrix[9] + mat[10] * matrix[10];
            newMatrix[11] = 0;

            newMatrix[12] = mat[0] * matrix[12] + mat[4] * matrix[13] + mat[8] * matrix[14] + mat[12];
            newMatrix[13] = mat[1] * matrix[12] + mat[5] * matrix[13] + mat[9] * matrix[14] + mat[13];
            newMatrix[14] = mat[2] * matrix[12] + mat[6] * matrix[13] + mat[10] * matrix[14] + mat[14];
            newMatrix[15] = 1;

            mat = newMatrix;
        }

        public void SetTranslation(float[] translation)
        {
            mat[12] = translation[0];
            mat[13] = translation[1];
            mat[14] = translation[2];
        }

        public void SetInverseTranslation(float[] translation)
        {
            mat[12] = -translation[0];
            mat[13] = -translation[1];
            mat[14] = -translation[2];
        }

        public void SetRotationDegrees(float[] angles)
        {
            float[] vec = new float[3];
            vec[0] = (float)(angles[0] * 180.0 / Math.PI);
            vec[1] = (float)(angles[1] * 180.0 / Math.PI);
            vec[2] = (float)(angles[2] * 180.0 / Math.PI);
            SetRotationRadians(vec);
        }

        public void SetInverseRotationRadians(float[] angles)
        {
            double cr = Math.Cos(angles[0]);
            double sr = Math.Sin(angles[0]);
            double cp = Math.Cos(angles[1]);
            double sp = Math.Sin(angles[1]);
            double cy = Math.Cos(angles[2]);
            double sy = Math.Sin(angles[2]);

            mat[0] = (float)(cp * cy);
            mat[4] = (float)(cp * sy);
            mat[8] = (float)(-sp);

            double srsp = sr * sp;
            double crsp = cr * sp;

            mat[1] = (float)(srsp * cy - cr * sy);
            mat[5] = (float)(srsp * sy + cr * cy);
            mat[9] = (float)(sr * cp);

            mat[2] = (float)(crsp * cy + sr * sy);
            mat[6] = (float)(crsp * sy - sr * cy);
            mat[10] = (float)(cr * cp);
        }

        public void SetInverseRotationDegrees(float[] angles)
        {
            float[] vec = new float[3];
            vec[0] = (float)(angles[0] * 180.0 / Math.PI);
            vec[1] = (float)(angles[1] * 180.0 / Math.PI);
            vec[2] = (float)(angles[2] * 180.0 / Math.PI);
            SetInverseRotationRadians(vec);
        }
    }
}