using System;

namespace _3D_Engine
{
    /// <include file="Help_5.xml" path="doc/members/member[@name='T:_3D_Engine.Matrix4x4']/*"/>
    public struct Matrix4x4
    {
        #region Fields and Properties
        
        // Matrix contents
        public float M00, M01, M02, M03;
        public float M10, M11, M12, M13;
        public float M20, M21, M22, M23;
        public float M30, M31, M32, M33;

        #endregion

        #region Constructors

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.#ctor(System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float,System.Float)']/*"/>
        public Matrix4x4(
            float m00, float m01, float m02, float m03, 
            float m10, float m11, float m12, float m13, 
            float m20, float m21, float m22, float m23, 
            float m30, float m31, float m32, float m33)
        {
            (M00, M01, M02, M03) = (m00, m01, m02, m03);
            (M10, M11, M12, M13) = (m10, m11, m12, m13);
            (M20, M21, M22, M23) = (m20, m21, m22, m23);
            (M30, M31, M32, M33) = (m30, m31, m32, m33);
        }

        public Matrix4x4(float[,] elements)
        {
            if (elements.GetLength(0) != 4
                || elements.GetLength(1) != 4)
                throw new ArgumentException("\"elements\" must be of size 4x4.", nameof(elements));

            (M00, M01, M02, M03) = (elements[0, 0], elements[0, 1], elements[0, 2], elements[0, 3]);
            (M10, M11, M12, M13) = (elements[1, 0], elements[1, 1], elements[1, 2], elements[1, 3]);
            (M20, M21, M22, M23) = (elements[2, 0], elements[2, 1], elements[2, 2], elements[2, 3]);
            (M30, M31, M32, M33) = (elements[3, 0], elements[3, 1], elements[3, 2], elements[3, 3]);
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.#ctor(System.Float[][])']/*"/>
        public Matrix4x4(float[][] elements)
        {
            if (elements.Length != 4 ||
                elements[0].Length != 4 ||
                elements[1].Length != 4 ||
                elements[2].Length != 4 ||
                elements[3].Length != 4)
                throw new ArgumentException("\"elements\" must be of size 4x4.");

            (M00, M01, M02, M03) = (elements[0][0], elements[0][1], elements[0][2], elements[0][3]);
            (M10, M11, M12, M13) = (elements[1][0], elements[1][1], elements[1][2], elements[1][3]);
            (M20, M21, M22, M23) = (elements[2][0], elements[2][1], elements[2][2], elements[2][3]);
            (M30, M31, M32, M33) = (elements[3][0], elements[3][1], elements[3][2], elements[3][3]);
        }

        #endregion

        #region Common Matrices

        public static Matrix4x4 Zero => new Matrix4x4();
        public static Matrix4x4 Identity =>
            new Matrix4x4 { M00 = 1, M11 = 1, M22 = 1, M33 = 1 };

        #endregion

        #region Matrix Operations (Common)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Determinant']/*"/> // source!
        public float Determinant()
        {
            float d1 = M20 * M31 - M21 * M30;
            float d2 = M20 * M32 - M22 * M30;
            float d3 = M20 * M33 - M23 * M30;
            float d4 = M21 * M32 - M22 * M31;
            float d5 = M21 * M33 - M23 * M31;
            float d6 = M22 * M33 - M23 * M32;

            return
              M00 * (M11 * d6 - M12 * d5 + M13 * d4)
            - M01 * (M10 * d6 - M12 * d3 + M13 * d2)
            + M02 * (M10 * d5 - M11 * d3 + M13 * d1)
            - M03 * (M10 * d4 - M11 * d2 + M12 * d1);
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Inverse']/*"/>
        public Matrix4x4 Inverse()
        {
            float d1 = M10 * M21 - M11 * M20;
            float d2 = M10 * M22 - M12 * M20;
            float d3 = M10 * M23 - M13 * M20;
            float d4 = M10 * M31 - M11 * M30;
            float d5 = M10 * M32 - M12 * M30;
            float d6 = M10 * M33 - M13 * M30;
            float d7 = M11 * M22 - M12 * M21;
            float d8 = M11 * M23 - M13 * M21;
            float d9 = M11 * M32 - M12 * M31;
            float d10 = M11 * M33 - M13 * M31;
            float d11 = M12 * M23 - M13 * M22;
            float d12 = M12 * M33 - M13 * M32;
            float d13 = M20 * M31 - M21 * M30;
            float d14 = M20 * M32 - M22 * M30;
            float d15 = M20 * M33 - M23 * M30;
            float d16 = M21 * M32 - M22 * M31;
            float d17 = M21 * M33 - M23 * M31;
            float d18 = M22 * M33 - M23 * M32;

            float det =   M00 * (M11 * d18 - M12 * d17 + M13 * d16)
                         - M01 * (M10 * d18 - M12 * d15 + M13 * d14)
                         + M02 * (M10 * d17 - M11 * d15 + M13 * d13)
                         - M03 * (M10 * d16 - M11 * d14 + M12 * d13);
            if (det == 0) throw new InvalidOperationException("Matrix does not have an inverse.");

            return new Matrix4x4
            (
                M11 * d18 - M12 * d17 + M13 * d16,
                -(M01 * d18 - M02 * d17 + M03 * d16),
                M01 * d12 - M02 * d10 + M03 * d9,
                -(M01 * d11 - M02 * d8 + M03 * d7),
                -(M10 * d18 - M12 * d15 + M13 * d14),
                M00 * d18 - M02 * d15 + M03 * d14,
                -(M00 * d12 - M02 * d6 + M03 * d5),
                M00 * d11 - M02 * d3 + M03 * d2,
                M10 * d17 - M11 * d15 + M13 * d13,
                -(M00 * d17 - M01 * d15 + M03 * d13),
                M00 * d10 - M01 * d6 + M03 * d4,
                -(M00 * d8 - M01 * d3 + M03 * d1),
                -(M10 * d16 - M11 * d14 + M12 * d13),
                M00 * d16 - M01 * d14 + M02 * d13,
                -(M00 * d9 - M01 * d5 + M02 * d4),
                M00 * d7 - M01 * d2 + M02 * d1
            ) / det;
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Transpose']/*"/>
        public Matrix4x4 Transpose() =>
            new Matrix4x4
            (
                M00, M10, M20, M30,
                M01, M11, M21, M31,
                M02, M12, M22, M32,
                M03, M13, M23, M33
            );

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.ToString']/*"/>
        public override string ToString() =>
            $"({M00}, {M01}, {M02}, {M03}, \n" +
            $"{M10}, {M11}, {M12}, {M13}, \n" +
            $"{M20}, {M21}, {M22}, {M23}, \n" +
            $"{M30}, {M31}, {M32}, {M33})";

        #endregion

        #region Matrix Operations (Operator Overloading)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Addition(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2) =>
            new Matrix4x4()
            {
                M00 = m1.M00 + m2.M00, M01 = m1.M01 + m2.M01, M02 = m1.M02 + m2.M02, M03 = m1.M03 + m2.M03,
                M10 = m1.M10 + m2.M10, M11 = m1.M11 + m2.M11, M12 = m1.M12 + m2.M12, M13 = m1.M13 + m2.M13,
                M20 = m1.M20 + m2.M20, M21 = m1.M21 + m2.M21, M22 = m1.M22 + m2.M22, M23 = m1.M23 + m2.M23,
                M30 = m1.M30 + m2.M30, M31 = m1.M31 + m2.M31, M32 = m1.M32 + m2.M32, M33 = m1.M33 + m2.M33
            };

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Subtraction(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2) =>
            new Matrix4x4()
            {
                M00 = m1.M00 - m2.M00, M01 = m1.M01 - m2.M01, M02 = m1.M02 - m2.M02, M03 = m1.M03 - m2.M03,
                M10 = m1.M10 - m2.M10, M11 = m1.M11 - m2.M11, M12 = m1.M12 - m2.M12, M13 = m1.M13 - m2.M13,
                M20 = m1.M20 - m2.M20, M21 = m1.M21 - m2.M21, M22 = m1.M22 - m2.M22, M23 = m1.M23 - m2.M23,
                M30 = m1.M30 - m2.M30, M31 = m1.M31 - m2.M31, M32 = m1.M32 - m2.M32, M33 = m1.M33 - m2.M33
            };

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
        public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2) =>
            new Matrix4x4
            (
                m1.M00 * m2.M00 + m1.M01 * m2.M10 + m1.M02 * m2.M20 + m1.M03 * m2.M30,
                m1.M00 * m2.M01 + m1.M01 * m2.M11 + m1.M02 * m2.M21 + m1.M03 * m2.M31,
                m1.M00 * m2.M02 + m1.M01 * m2.M12 + m1.M02 * m2.M22 + m1.M03 * m2.M32,
                m1.M00 * m2.M03 + m1.M01 * m2.M13 + m1.M02 * m2.M23 + m1.M03 * m2.M33,
                m1.M10 * m2.M00 + m1.M11 * m2.M10 + m1.M12 * m2.M20 + m1.M13 * m2.M30,
                m1.M10 * m2.M01 + m1.M11 * m2.M11 + m1.M12 * m2.M21 + m1.M13 * m2.M31,
                m1.M10 * m2.M02 + m1.M11 * m2.M12 + m1.M12 * m2.M22 + m1.M13 * m2.M32,
                m1.M10 * m2.M03 + m1.M11 * m2.M13 + m1.M12 * m2.M23 + m1.M13 * m2.M33,
                m1.M20 * m2.M00 + m1.M21 * m2.M10 + m1.M22 * m2.M20 + m1.M23 * m2.M30,
                m1.M20 * m2.M01 + m1.M21 * m2.M11 + m1.M22 * m2.M21 + m1.M23 * m2.M31,
                m1.M20 * m2.M02 + m1.M21 * m2.M12 + m1.M22 * m2.M22 + m1.M23 * m2.M32,
                m1.M20 * m2.M03 + m1.M21 * m2.M13 + m1.M22 * m2.M23 + m1.M23 * m2.M33,
                m1.M30 * m2.M00 + m1.M31 * m2.M10 + m1.M32 * m2.M20 + m1.M33 * m2.M30,
                m1.M30 * m2.M01 + m1.M31 * m2.M11 + m1.M32 * m2.M21 + m1.M33 * m2.M31,
                m1.M30 * m2.M02 + m1.M31 * m2.M12 + m1.M32 * m2.M22 + m1.M33 * m2.M32,
                m1.M30 * m2.M03 + m1.M31 * m2.M13 + m1.M32 * m2.M23 + m1.M33 * m2.M33
            );

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Vector4D)']/*"/>
        public static Vector4D operator *(Matrix4x4 m, Vector4D v) => 
            new Vector4D(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03 * v.W,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13 * v.W,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23 * v.W,
                m.M30 * v.X + m.M31 * v.Y + m.M32 * v.Z + m.M33 * v.W
            );

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,System.Float)']/*"/>
        public static Matrix4x4 operator *(Matrix4x4 m, float scalar) =>
            new Matrix4x4()
            {
                M00 = m.M00 * scalar, M01 = m.M01 * scalar, M02 = m.M02 * scalar, M03 = m.M03 * scalar,
                M10 = m.M10 * scalar, M11 = m.M11 * scalar, M12 = m.M12 * scalar, M13 = m.M13 * scalar,
                M20 = m.M20 * scalar, M21 = m.M21 * scalar, M22 = m.M22 * scalar, M23 = m.M23 * scalar,
                M30 = m.M30 * scalar, M31 = m.M31 * scalar, M32 = m.M32 * scalar, M33 = m.M33 * scalar
            };

        public static Matrix4x4 operator *(float scalar, Matrix4x4 m) => m * scalar;

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Division(_3D_Engine.Matrix4x4,System.Float)']/*"/>
        public static Matrix4x4 operator /(Matrix4x4 m, float scalar) =>
            new Matrix4x4()
            {
                M00 = m.M00 / scalar, M01 = m.M01 / scalar, M02 = m.M02 / scalar, M03 = m.M03 / scalar,
                M10 = m.M10 / scalar, M11 = m.M11 / scalar, M12 = m.M12 / scalar, M13 = m.M13 / scalar,
                M20 = m.M20 / scalar, M21 = m.M21 / scalar, M22 = m.M22 / scalar, M23 = m.M23 / scalar,
                M30 = m.M30 / scalar, M31 = m.M31 / scalar, M32 = m.M32 / scalar, M33 = m.M33 / scalar
            };

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Matrix4x4 v1, Matrix4x4 v2) => 
            v1.M00 == v2.M00 && v1.M01 == v2.M01 && v1.M02 == v2.M02 && v1.M03 == v2.M03 &&
            v1.M10 == v2.M10 && v1.M11 == v2.M11 && v1.M12 == v2.M12 && v1.M13 == v2.M13 &&
            v1.M20 == v2.M20 && v1.M21 == v2.M21 && v1.M22 == v2.M22 && v1.M23 == v2.M23 &&
            v1.M30 == v2.M30 && v1.M31 == v2.M31 && v1.M32 == v2.M32 && v1.M33 == v2.M33;

        public static bool operator !=(Matrix4x4 v1, Matrix4x4 v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Matrix4x4)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}