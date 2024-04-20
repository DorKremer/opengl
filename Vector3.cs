using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myOpenGL
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 UnitX { get { return new Vector3(1, 0, 0); } }
        public static Vector3 UnitY { get { return new Vector3(0, 1, 0); } }
        public static Vector3 UnitZ { get { return new Vector3(0, 0, 1); } }

        public Vector3 Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            return new Vector3(X / length, Y / length, Z / length);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            float x = v1.Y * v2.Z - v1.Z * v2.Y;
            float y = v1.Z * v2.X - v1.X * v2.Z;
            float z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector3(x, y, z);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float scalar)
        {
            return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }
    }
}
