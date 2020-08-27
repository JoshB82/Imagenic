using System;

namespace _3D_Engine
{
    /// <include file="Help_3.xml" path="doc/members/member[@name='T:_3D_Engine.Matrix4x4']/*"/>
    public struct Matrix4x4
    {
        #region Fields and Properties

        /// <include file="Help_3.xml" path="doc/members/member[@name='P:_3D_Engine.Matrix4x4.Data']/*"/>
        public double[][] Data { get; set; }

        #endregion

        #region Constructors

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Zeroed_Matrix']/*"/>
        public static Matrix4x4 Zeroed_Matrix()
        {
            double[][] data = new double[4][];
            for (int i = 0; i < 4; i++) data[i] = new double[4];
            return new Matrix4x4(data);
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Identity_Matrix']/*"/>
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

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.#ctor(System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double,System.Double)']/*"/>
        public Matrix4x4(double i1, double i2, double i3, double i4, double i5, double i6, double i7, double i8, double i9, double i10, double i11, double i12, double i13, double i14, double i15, double i16)
        {
            Data = new double[4][];
            Data[0] = new double[4] { i1, i2, i3, i4 };
            Data[1] = new double[4] { i5, i6, i7, i8 };
            Data[2] = new double[4] { i9, i10, i11, i12 };
            Data[3] = new double[4] { i13, i14, i15, i16 };
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.#ctor(System.Double[][])']/*"/>
        public Matrix4x4(double[][] data)
        {
            if (data.Length != 4 || data[0].Length != 4 || data[1].Length != 4 || data[2].Length != 4 || data[3].Length != 4) throw new Exception("Array must be of size 4x4.");
            Data = data;
        }

        #endregion

        #region Matrix Operations (Common)

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Determinant']/*"/> // source!
        public double Determinant()
        {
            double d1 = Data[2][0] * Data[3][1] - Data[2][1] * Data[3][0]; //13
            double d2 = Data[2][0] * Data[3][2] - Data[2][2] * Data[3][0]; //14
            double d3 = Data[2][0] * Data[3][3] - Data[2][3] * Data[3][0]; //15
            double d4 = Data[2][1] * Data[3][2] - Data[2][2] * Data[3][1]; //16
            double d5 = Data[2][1] * Data[3][3] - Data[2][3] * Data[3][1]; //17
            double d6 = Data[2][2] * Data[3][3] - Data[2][3] * Data[3][2]; //18

            return
                  Data[0][0] * (Data[1][1] * d6 - Data[1][2] * d5 + Data[1][3] * d4)
                - Data[0][1] * (Data[1][0] * d6 - Data[1][2] * d3 + Data[1][3] * d2)
                + Data[0][2] * (Data[1][0] * d5 - Data[1][1] * d3 + Data[1][3] * d1)
                - Data[0][3] * (Data[1][0] * d4 - Data[1][1] * d2 + Data[1][2] * d1);
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Inverse']/*"/>
        public Matrix4x4 Inverse()
        {
            double d1 = Data[1][0] * Data[2][1] - Data[1][1] * Data[2][0];
            double d2 = Data[1][0] * Data[2][2] - Data[1][2] * Data[2][0];
            double d3 = Data[1][0] * Data[2][3] - Data[1][3] * Data[2][0];
            double d4 = Data[1][0] * Data[3][1] - Data[1][1] * Data[3][0];
            double d5 = Data[1][0] * Data[3][2] - Data[1][2] * Data[3][0];
            double d6 = Data[1][0] * Data[3][3] - Data[1][3] * Data[3][0];
            double d7 = Data[1][1] * Data[2][2] - Data[1][2] * Data[2][1];
            double d8 = Data[1][1] * Data[2][3] - Data[1][3] * Data[2][1];
            double d9 = Data[1][1] * Data[3][2] - Data[1][2] * Data[3][1];
            double d10 = Data[1][1] * Data[3][3] - Data[1][3] * Data[3][1];
            double d11 = Data[1][2] * Data[2][3] - Data[1][3] * Data[2][2];
            double d12 = Data[1][2] * Data[3][3] - Data[1][3] * Data[3][2];
            double d13 = Data[2][0] * Data[3][1] - Data[2][1] * Data[3][0];
            double d14 = Data[2][0] * Data[3][2] - Data[2][2] * Data[3][0];
            double d15 = Data[2][0] * Data[3][3] - Data[2][3] * Data[3][0];
            double d16 = Data[2][1] * Data[3][2] - Data[2][2] * Data[3][1];
            double d17 = Data[2][1] * Data[3][3] - Data[2][3] * Data[3][1];
            double d18 = Data[2][2] * Data[3][3] - Data[2][3] * Data[3][2];

            double det =   Data[0][0] * (Data[1][1] * d18 - Data[1][2] * d17 + Data[1][3] * d16)
                         - Data[0][1] * (Data[1][0] * d18 - Data[1][2] * d15 + Data[1][3] * d14)
                         + Data[0][2] * (Data[1][0] * d17 - Data[1][1] * d15 + Data[1][3] * d13)
                         - Data[0][3] * (Data[1][0] * d16 - Data[1][1] * d14 + Data[1][2] * d13);
            if (det == 0) throw new Exception("Matrix does not have an inverse.");
            double inv_det = 1 / det;

            Matrix4x4 m = new Matrix4x4
                (
                    Data[1][1] * d18 - Data[1][2] * d17 + Data[1][3] * d16,
                    -(Data[0][1] * d18 - Data[0][2] * d17 + Data[0][3] * d16),
                    Data[0][1] * d12 - Data[0][2] * d10 + Data[0][3] * d9,
                    -(Data[0][1] * d11 - Data[0][2] * d8 + Data[0][3] * d7),
                    -(Data[1][0] * d18 - Data[1][2] * d15 + Data[1][3] * d14),
                    Data[0][0] * d18 - Data[0][2] * d15 + Data[0][3] * d14,
                    -(Data[0][0] * d12 - Data[0][2] * d6 + Data[0][3] * d5),
                    Data[0][0] * d11 - Data[0][2] * d3 + Data[0][3] * d2,
                    Data[1][0] * d17 - Data[1][1] * d15 + Data[1][3] * d13,
                    -(Data[0][0] * d17 - Data[0][1] * d15 + Data[0][3] * d13),
                    Data[0][0] * d10 - Data[0][1] * d6 + Data[0][3] * d4,
                    -(Data[0][0] * d8 - Data[0][1] * d3 + Data[0][3] * d1),
                    -(Data[1][0] * d16 - Data[1][1] * d14 + Data[1][2] * d13),
                    Data[0][0] * d16 - Data[0][1] * d14 + Data[0][2] * d13,
                    -(Data[0][0] * d9 - Data[0][1] * d5 + Data[0][2] * d4),
                    Data[0][0] * d7 - Data[0][1] * d2 + Data[0][2] * d1
                );

            return m * inv_det;
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Transpose']/*"/>
        public Matrix4x4 Transpose() =>
            new Matrix4x4(
                Data[0][0], Data[1][0], Data[2][0], Data[3][0],
                Data[0][1], Data[1][1], Data[2][1], Data[3][1],
                Data[0][2], Data[1][2], Data[2][2], Data[3][2],
                Data[0][3], Data[1][3], Data[2][3], Data[3][3]
            );

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.ToString']/*"/>
        public override string ToString() =>
            $"({Data[0][0]}, {Data[0][1]}, {Data[0][2]}, {Data[0][3]}, \n" +
            $"{Data[1][0]}, {Data[1][1]}, {Data[1][2]}, {Data[1][3]}, \n" +
            $"{Data[2][0]}, {Data[2][1]}, {Data[2][2]}, {Data[2][3]}, \n" +
            $"{Data[3][0]}, {Data[3][1]}, {Data[3][2]}, {Data[3][3]})";

        #endregion

        #region Matrix Operations (Operator Overloading)

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Addition(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
        {
            double[][] result = new double[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] + m2.Data[i][j];
            return new Matrix4x4(result);
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Subtraction(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2)
        {
            double[][] result = new double[4][];
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) result[i][j] = m1.Data[i][j] - m2.Data[i][j];
            return new Matrix4x4(result);
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2) =>
            new Matrix4x4(
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

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Vector4D)']/*"/>
        public static Vector4D operator *(Matrix4x4 m, Vector4D v) => 
            new Vector4D(
                m.Data[0][0] * v.X + m.Data[0][1] * v.Y + m.Data[0][2] * v.Z + m.Data[0][3] * v.W,
                m.Data[1][0] * v.X + m.Data[1][1] * v.Y + m.Data[1][2] * v.Z + m.Data[1][3] * v.W,
                m.Data[2][0] * v.X + m.Data[2][1] * v.Y + m.Data[2][2] * v.Z + m.Data[2][3] * v.W,
                m.Data[3][0] * v.X + m.Data[3][1] * v.Y + m.Data[3][2] * v.Z + m.Data[3][3] * v.W
            );

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,System.Double)']/*"/>
        public static Matrix4x4 operator *(Matrix4x4 m, double scalar)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] *= scalar;
            return m;
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Division(_3D_Engine.Matrix4x4,System.Double)']/*"/>
        public static Matrix4x4 operator /(Matrix4x4 m, double scalar)
        {
            for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) m.Data[i][j] /= scalar;
            return m;
        }
        
        #endregion
    }
}