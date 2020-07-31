using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Matrix4x4"/> (square matrix with four rows and four columns).
    /// </summary>
    public struct Matrix4x4
    {
        #region Fields and Properties

        /// <summary>
        /// The values held within the <see cref="Matrix4x4"/>; [rows, columns].
        /// </summary>
        public double[][] Data { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a zeroed <see cref="Matrix4x4"/>.
        /// </summary>
        public static Matrix4x4 Zeroed_Matrix()
        {
            double[][] data = new double[4][];
            for (int i = 0; i < 4; i++) data[i] = new double[4];
            return new Matrix4x4(data);
        }

        /// <summary>
        /// Creates an identity <see cref="Matrix4x4"/>.
        /// </summary>
        public static Matrix4x4 Identity_Matrix()
        {
            double[][] data = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                data[i] = new double[4];
                data[i][i] = 1;
            }
            return new Matrix4x4(data);
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> from 16 values.
        /// </summary>
        /// <param name="i1">[0, 0] value.</param>
        /// <param name="i2">[0, 1] value.</param>
        /// <param name="i3">[0, 2] value.</param>
        /// <param name="i4">[0, 3] value.</param>
        /// <param name="i5">[1, 0] value.</param>
        /// <param name="i6">[1, 1] value.</param>
        /// <param name="i7">[1, 2] value.</param>
        /// <param name="i8">[1, 3] value.</param>
        /// <param name="i9">[2, 0] value.</param>
        /// <param name="i10">[2, 1] value.</param>
        /// <param name="i11">[2, 2] value.</param>
        /// <param name="i12">[2, 3] value.</param>
        /// <param name="i13">[3, 0] value.</param>
        /// <param name="i14">[3, 1] value.</param>
        /// <param name="i15">[3, 2] value.</param>
        /// <param name="i16">[3, 3] value.</param>
        public Matrix4x4(double i1, double i2, double i3, double i4, double i5, double i6, double i7, double i8, double i9, double i10, double i11, double i12, double i13, double i14, double i15, double i16)
        {
            Data = new double[4][];
            Data[0] = new double[4] { i1, i2, i3, i4 };
            Data[1] = new double[4] { i5, i6, i7, i8 };
            Data[2] = new double[4] { i9, i10, i11, i12 };
            Data[3] = new double[4] { i13, i14, i15, i16 };
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> from an array of data.
        /// </summary>
        /// <param name="data">Array containing data to be entered into the <see cref="Matrix4x4"/>.</param>
        public Matrix4x4(double[][] data)
        {
            if (data.Length != 4 || data[0].Length != 4 || data[1].Length != 4 || data[2].Length != 4 || data[3].Length != 4) throw new Exception("Array must be of size 4x4.");
            Data = data;
        }

        #endregion

        public override string ToString() =>
            $"({Data[0][0]}, {Data[0][1]}, {Data[0][2]}, {Data[0][3]}\n" +
            $"{Data[1][0]}, {Data[1][1]}, {Data[1][2]}, {Data[1][3]}\n" +
            $"{Data[2][0]}, {Data[2][1]}, {Data[2][2]}, {Data[2][3]}\n" +
            $"{Data[3][0]}, {Data[3][1]}, {Data[3][2]}, {Data[3][3]})";

        #region Matrix Operations (Operator Overloading)

        /// <summary>
        /// Finds the element-wise sum of two <see cref="Matrix4x4"/>.
        /// </summary>
        /// <param name="m1">First addend.</param>
        /// <param name="m2">Second addend.</param>
        /// <returns>Sum of two <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
        {
            double[][] result = new double[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] + m2.Data[i][j];
            return new Matrix4x4(result);
        }

        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2)
        {
            double[][] result = new double[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] - m2.Data[i][j];
            return new Matrix4x4(result);
        }

        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            return new Matrix4x4(
                m1.Data[0][0] * m2.Data[0][0] + m1.Data[0][1] * m2.Data[1][0] + m1.Data[0][2] * m2.Data[2][0] + m1.Data[0][3] * m2.Data[3][0],
                m1.Data[0][0] * m2.Data[0][1] + m1.Data[0][1] * m2.Data[1][1] + m1.Data[0][2] * m2.Data[2][1] + m1.Data[0][3] * m2.Data[3][1],
                m1.Data[0][0] * m2.Data[0][2] + m1.Data[0][1] * m2.Data[1][2] + m1.Data[0][2] * m2.Data[2][2] + m1.Data[0][3] * m2.Data[3][2],
                m1.Data[0][0] * m2.Data[0][3] + m1.Data[0][1] * m2.Data[1][3] + m1.Data[0][2] * m2.Data[2][3] + m1.Data[0][3] * m2.Data[3][3],
                m1.Data[1][0] * m2.Data[0][0] + m1.Data[1][1] * m2.Data[1][0] + m1.Data[1][2] * m2.Data[2][0] + m1.Data[1][3] * m2.Data[3][0],
                m1.Data[1][0] * m2.Data[0][1] + m1.Data[1][1] * m2.Data[1][1] + m1.Data[1][2] * m2.Data[2][1] + m1.Data[1][3] * m2.Data[3][1],
                m1.Data[1][0] * m2.Data[0][2] + m1.Data[1][1] * m2.Data[1][2] + m1.Data[1][2] * m2.Data[2][2] + m1.Data[1][3] * m2.Data[3][2],
                m1.Data[1][0] * m2.Data[0][3] + m1.Data[1][1] * m2.Data[1][3] + m1.Data[1][2] * m2.Data[2][3] + m1.Data[1][3] * m2.Data[3][3],
                m1.Data[2][0] * m2.Data[0][0] + m1.Data[2][1] * m2.Data[1][0] + m1.Data[2][2] * m2.Data[2][0] + m1.Data[2][3] * m2.Data[3][0],
                m1.Data[2][0] * m2.Data[0][1] + m1.Data[2][1] * m2.Data[1][1] + m1.Data[2][2] * m2.Data[2][1] + m1.Data[2][3] * m2.Data[3][1],
                m1.Data[2][0] * m2.Data[0][2] + m1.Data[2][1] * m2.Data[1][2] + m1.Data[2][2] * m2.Data[2][2] + m1.Data[2][3] * m2.Data[3][2],
                m1.Data[2][0] * m2.Data[0][3] + m1.Data[2][1] * m2.Data[1][3] + m1.Data[2][2] * m2.Data[2][3] + m1.Data[2][3] * m2.Data[3][3],
                m1.Data[3][0] * m2.Data[0][0] + m1.Data[3][1] * m2.Data[1][0] + m1.Data[3][2] * m2.Data[2][0] + m1.Data[3][3] * m2.Data[3][0],
                m1.Data[3][0] * m2.Data[0][1] + m1.Data[3][1] * m2.Data[1][1] + m1.Data[3][2] * m2.Data[2][1] + m1.Data[3][3] * m2.Data[3][1],
                m1.Data[3][0] * m2.Data[0][2] + m1.Data[3][1] * m2.Data[1][2] + m1.Data[3][2] * m2.Data[2][2] + m1.Data[3][3] * m2.Data[3][2],
                m1.Data[3][0] * m2.Data[0][3] + m1.Data[3][1] * m2.Data[1][3] + m1.Data[3][2] * m2.Data[2][3] + m1.Data[3][3] * m2.Data[3][3]
            );
        }

        public static Vector4D operator *(Matrix4x4 m, Vector4D v)
        {
            return new Vector4D(
                m.Data[0][0] * v.X + m.Data[0][1] * v.Y + m.Data[0][2] * v.Z + m.Data[0][3] * v.W,
                m.Data[1][0] * v.X + m.Data[1][1] * v.Y + m.Data[1][2] * v.Z + m.Data[1][3] * v.W,
                m.Data[2][0] * v.X + m.Data[2][1] * v.Y + m.Data[2][2] * v.Z + m.Data[2][3] * v.W,
                m.Data[3][0] * v.X + m.Data[3][1] * v.Y + m.Data[3][2] * v.Z + m.Data[3][3] * v.W
            );
        }

        /*
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
        {
            double[][] result = new double[4][];
            double[] row = new double[4];
            double[] col = new double[4];

            for (int i = 0; i < 4; i++)
            {
                result[i] = new double[4];
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++) row[k] = m1.Data[i][k];
                    for (int l = 0; l < 4; l++) col[l] = m2.Data[l][j];
                    result[i][j] = new Vector4D(row) * new Vector4D(col);
                }
            }
            return new Matrix4x4(result);
        }
        */

        /*
        public static Vector4D operator *(Matrix4x4 m, Vector4D v)
        {
            double[] result = new double[4];
            double[] row = new double[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) row[j] = m.Data[i][j];
                result[i] = new Vector4D(row) * v;
            }
            return new Vector4D(result);
        }
        */

        /*
        public static Vertex operator *(Matrix4x4 m, Vertex v)
        {
            double[] result = new double[4];
            double[] row = new double[4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++) row[j] = m.Data[i][j];
                result[i] = new Vector4D(row) * v;
            }
            return new Vertex(result[0], result[1], result[2], result[3], v.Colour, v.Visible, v.Diameter);
        }
        */

        public static Matrix4x4 operator *(double scalar, Matrix4x4 m)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] *= scalar;
            return m;
        }

        public static Matrix4x4 operator /(double scalar, Matrix4x4 m)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] /= scalar;
            return m;
        }
        
        #endregion
    }
}