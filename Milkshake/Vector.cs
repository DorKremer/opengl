using System;

namespace MathEX
{
	public class Vector
	{
		float[] vec;
		public Vector()
		{
			vec = new float[4];
		}

		public float[] GetVector
		{
            get { return vec; }
		}

		public Vector(float[] values )
		{
			vec = new float[4];
			Set( values );
			vec[3] = 1;
		}

		void Set(float[] values )
		{
			vec[0] = values[0];
			vec[1] = values[1];
			vec[2] = values[2];
		}

		public void Normalize()
		{
			float len = Length;

			vec[0] /= len;
			vec[1] /= len;
			vec[2] /= len;
		}

		public float Length
		{
			get
			{
				return ( float )Math.Sqrt( vec[0] * vec[0] + vec[1] * vec[1] + vec[2] * vec[2] );
			}
		}

		public void Transform(Matrix mat )
		{
			double[] vector = new double[4];

			vector[0] = vec[0] * mat[0] + vec[1] * mat[4] + vec[2] * mat[8] + mat[12];
			vector[1] = vec[0] * mat[1] + vec[1] * mat[5] + vec[2] * mat[9] + mat[13];
			vector[2] = vec[0] * mat[2] + vec[1] * mat[6] + vec[2] * mat[10] + mat[14];
			vector[3] = vec[0] * mat[3] + vec[1] * mat[7] + vec[2] * mat[11] + mat[15];

			vec[0] = ( float )( vector[0] );
			vec[1] = ( float )( vector[1] );
			vec[2] = ( float )( vector[2] );
			vec[3] = ( float )( vector[3] );
		}
		
		public void Transform3( Matrix mat )
		{
			double[] vector = new double[3];

			vector[0] = vec[0] * mat[0] + vec[1] * mat[4] + vec[2] * mat[8];
			vector[1] = vec[0] * mat[1] + vec[1] * mat[5] + vec[2] * mat[9];
			vector[2] = vec[0] * mat[2] + vec[1] * mat[6] + vec[2] * mat[10];

			vec[0] = ( float )( vector[0] );
			vec[1] = ( float )( vector[1] );
			vec[2] = ( float )( vector[2] );
			vec[3] = 1.0f;
		}
	}
}
