using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myOpenGL
{
    public class Matrix4
    {
        private float[,] elements;

        public Matrix4()
        {
            elements = new float[4, 4];
        }

        public static Matrix4 LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            Vector3 zaxis = (eye - target).Normalize();  // The "forward" vector
            Vector3 xaxis = Vector3.Cross(up, zaxis).Normalize(); // The "right" vector
            Vector3 yaxis = Vector3.Cross(zaxis, xaxis); // The "up" vector

            Matrix4 result = new Matrix4();

            result[0, 0] = xaxis.X;
            result[1, 0] = xaxis.Y;
            result[2, 0] = xaxis.Z;
            result[3, 0] = -Vector3.Dot(xaxis, eye);

            result[0, 1] = yaxis.X;
            result[1, 1] = yaxis.Y;
            result[2, 1] = yaxis.Z;
            result[3, 1] = -Vector3.Dot(yaxis, eye);

            result[0, 2] = zaxis.X;
            result[1, 2] = zaxis.Y;
            result[2, 2] = zaxis.Z;
            result[3, 2] = -Vector3.Dot(zaxis, eye);

            result[0, 3] = 0;
            result[1, 3] = 0;
            result[2, 3] = 0;
            result[3, 3] = 1;

            return result;
        }

        public float this[int row, int col]
        {
            get { return elements[row, col]; }
            set { elements[row, col] = value; }
        }

        public float[] ToArray()
        {
            float[] array = new float[16];
            int index = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    array[index++] = elements[row, col];
                }
            }
            return array;
        }
    }
}
