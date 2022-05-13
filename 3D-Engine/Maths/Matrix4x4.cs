/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a 4x4 square matrix and provides methods to extract common information and for operator overloading. Each instance of a Matrix4x4 has a size of 64 bytes, so, where possible, a Matrix4x4 should be passed by reference to reduce unnecessary copying.
 */

using _3D_Engine.Constants;
using System;

namespace Imagenic.Core.Maths;

/// <summary>
/// Encapsulates creation of a <see cref="Matrix4x4"/> (square matrix with four rows and four columns).
/// </summary>
public struct Matrix4x4 : IEquatable<Matrix4x4>
{
    #region Fields and Properties

    // Common matrices
    /// <summary>
    /// A <see cref="Matrix4x4"/> with all values set to zero.
    /// </summary>
    public static readonly Matrix4x4 Zero = new();
    /// <summary>
    /// A <see cref="Matrix4x4"/> equal to the 4x4 identity matrix.
    /// </summary>
    public static readonly Matrix4x4 Identity = new() { m00 = 1, m11 = 1, m22 = 1, m33 = 1 };

    // Matrix contents
    /// <summary>
    /// From the top left, the value in the first row and first column.
    /// </summary>
    public float m00;
    /// <summary>
    /// From the top left, the value in the first row and second column.
    /// </summary>
    public float m01;
    /// <summary>
    /// From the top left, the value in the first row and third column.
    /// </summary>
    public float m02;
    /// <summary>
    /// From the top left, the value in the first row and fourth column.
    /// </summary>
    public float m03;
    /// <summary>
    /// From the top left, the value in the second row and first column.
    /// </summary>
    public float m10;
    /// <summary>
    /// From the top left, the value in the second row and second column.
    /// </summary>
    public float m11;
    /// <summary>
    /// From the top left, the value in the second row and third column.
    /// </summary>
    public float m12;
    /// <summary>
    /// From the top left, the value in the second row and fourth column.
    /// </summary>
    public float m13;
    /// <summary>
    /// From the top left, the value in the third row and first column.
    /// </summary>
    public float m20;
    /// <summary>
    /// From the top left, the value in the third row and second column.
    /// </summary>
    public float m21;
    /// <summary>
    /// From the top left, the value in the third row and third column.
    /// </summary>
    public float m22;
    /// <summary>
    /// From the top left, the value in the third row and fourth column.
    ///</summary>
    public float m23;
    /// <summary>
    /// From the top left, the value in the fourth row and first column.
    /// </summary>
    public float m30;
    /// <summary>
    /// From the top left, the value in the fourth row and second column.
    /// </summary>
    public float m31;
    /// <summary>
    /// From the top left, the value in the fourth row and third column.
    /// </summary>
    public float m32;
    /// <summary>
    /// From the top left, the value in the fourth row and fourth column.
    /// </summary>
    public float m33;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref= "Matrix4x4"/> from 16 values.
    /// </summary>
    /// <param name="m00">From the top left, the value to be put at the first row and first column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m01">From the top left, the value to be put at the first row and second column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m02">From the top left, the value to be put at the first row and third column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m03">From the top left, the value to be put at the first row and fourth column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m10">From the top left, the value to be put at the second row and first column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m11">From the top left, the value to be put at the second row and second column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m12">From the top left, the value to be put at the second row and third column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m13">From the top left, the value to be put at the second row and fourth column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m20">From the top left, the value to be put at the third row and first column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m21">From the top left, the value to be put at the third row and second column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m22">From the top left, the value to be put at the third row and third column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m23">From the top left, the value to be put at the third row and fourth column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m30">From the top left, the value to be put at the fourth row and first column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m31">From the top left, the value to be put at the fourth row and second column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m32">From the top left, the value to be put at the fourth row and third column of the <see cref="Matrix4x4"/>.</param>
    /// <param name="m33">From the top left, the value to be put at the fourth row and fourth column of the <see cref="Matrix4x4"/>.</param>
    public Matrix4x4(
        float m00, float m01, float m02, float m03,
        float m10, float m11, float m12, float m13,
        float m20, float m21, float m22, float m23,
        float m30, float m31, float m32, float m33)
    {
        (this.m00, this.m01, this.m02, this.m03) = (m00, m01, m02, m03);
        (this.m10, this.m11, this.m12, this.m13) = (m10, m11, m12, m13);
        (this.m20, this.m21, this.m22, this.m23) = (m20, m21, m22, m23);
        (this.m30, this.m31, this.m32, this.m33) = (m30, m31, m32, m33);
    }

    /// <summary>
    /// Creates a <see cref="Matrix4x4"/> from a two-dimensional array of elements.
    /// </summary>
    /// <param name="elements">The array containing elements to be entered into the <see cref= "Matrix4x4"/>.</param>
    public Matrix4x4(float[,] elements)
    {
        if (elements is null)
        {
            throw GenerateException<ParameterCannotBeNullException>.WithParameters(nameof(elements));
        }
        if (elements.GetLength(0) != 4 || elements.GetLength(1) != 4)
        {
            throw GenerateException<InvalidArraySizeException>.WithParameters(nameof(elements));
        }

        (m00, m01, m02, m03) = (elements[0, 0], elements[0, 1], elements[0, 2], elements[0, 3]);
        (m10, m11, m12, m13) = (elements[1, 0], elements[1, 1], elements[1, 2], elements[1, 3]);
        (m20, m21, m22, m23) = (elements[2, 0], elements[2, 1], elements[2, 2], elements[2, 3]);
        (m30, m31, m32, m33) = (elements[3, 0], elements[3, 1], elements[3, 2], elements[3, 3]);
    }

    /// <summary>
    /// Creates a <see cref="Matrix4x4"/> from a jagged array of elements.
    /// </summary>
    /// <param name="elements">The array containing elements to be entered into the <see cref="Matrix4x4"/>.</param>
    public Matrix4x4(float[][] elements)
    {
        if (elements is null)
        {
            throw GenerateException<ParameterCannotBeNullException>.WithParameters(nameof(elements));
        }
        if (elements.Length != 4 || elements[0].Length != 4 || elements[1].Length != 4 || elements[2].Length != 4 || elements[3].Length != 4)
        {
            throw GenerateException<InvalidArraySizeException>.WithParameters(nameof(elements));
        }

        (m00, m01, m02, m03) = (elements[0][0], elements[0][1], elements[0][2], elements[0][3]);
        (m10, m11, m12, m13) = (elements[1][0], elements[1][1], elements[1][2], elements[1][3]);
        (m20, m21, m22, m23) = (elements[2][0], elements[2][1], elements[2][2], elements[2][3]);
        (m30, m31, m32, m33) = (elements[3][0], elements[3][1], elements[3][2], elements[3][3]);
    }

    #endregion

    #region Matrix Operations

    // Common
    /// <summary>
    /// Finds the determinant of a <see cref="Matrix4x4"/>.
    /// </summary>
    /// <returns>Determinant of a <see cref="Matrix4x4"/>.</returns> // source!
    public readonly float Determinant()
    {
        float d1 = m20 * m31 - m21 * m30, d2 = m20 * m32 - m22 * m30, d3 = m20 * m33 - m23 * m30;
        float d4 = m21 * m32 - m22 * m31, d5 = m21 * m33 - m23 * m31, d6 = m22 * m33 - m23 * m32;

        return
          m00 * (m11 * d6 - m12 * d5 + m13 * d4)
        - m01 * (m10 * d6 - m12 * d3 + m13 * d2)
        + m02 * (m10 * d5 - m11 * d3 + m13 * d1)
        - m03 * (m10 * d4 - m11 * d2 + m12 * d1);
    }

    /// <summary>
    /// Finds the inverse of a <see cref="Matrix4x4"/>.
    /// </summary>
    /// <returns>Inverse of a <see cref="Matrix4x4"/>.</returns>
    public readonly Matrix4x4 Inverse()
    {
        float d1 = m10 * m21 - m11 * m20, d2 = m10 * m22 - m12 * m20, d3 = m10 * m23 - m13 * m20;
        float d4 = m10 * m31 - m11 * m30, d5 = m10 * m32 - m12 * m30, d6 = m10 * m33 - m13 * m30;
        float d7 = m11 * m22 - m12 * m21, d8 = m11 * m23 - m13 * m21, d9 = m11 * m32 - m12 * m31;
        float d10 = m11 * m33 - m13 * m31, d11 = m12 * m23 - m13 * m22, d12 = m12 * m33 - m13 * m32;
        float d13 = m20 * m31 - m21 * m30, d14 = m20 * m32 - m22 * m30, d15 = m20 * m33 - m23 * m30;
        float d16 = m21 * m32 - m22 * m31, d17 = m21 * m33 - m23 * m31, d18 = m22 * m33 - m23 * m32;

        float det = m00 * (m11 * d18 - m12 * d17 + m13 * d16)
                    - m01 * (m10 * d18 - m12 * d15 + m13 * d14)
                    + m02 * (m10 * d17 - m11 * d15 + m13 * d13)
                    - m03 * (m10 * d16 - m11 * d14 + m12 * d13);
        if (det == 0)
        {
            throw GenerateException<Matrix4x4DoesNotHaveAnInverseException>.WithParameters();
        }

        return new Matrix4x4
        (
              m11 * d18 - m12 * d17 + m13 * d16,
            -(m01 * d18 - m02 * d17 + m03 * d16),
              m01 * d12 - m02 * d10 + m03 * d9,
            -(m01 * d11 - m02 * d8 + m03 * d7),
            -(m10 * d18 - m12 * d15 + m13 * d14),
              m00 * d18 - m02 * d15 + m03 * d14,
            -(m00 * d12 - m02 * d6 + m03 * d5),
              m00 * d11 - m02 * d3 + m03 * d2,
              m10 * d17 - m11 * d15 + m13 * d13,
            -(m00 * d17 - m01 * d15 + m03 * d13),
              m00 * d10 - m01 * d6 + m03 * d4,
            -(m00 * d8 - m01 * d3 + m03 * d1),
            -(m10 * d16 - m11 * d14 + m12 * d13),
              m00 * d16 - m01 * d14 + m02 * d13,
            -(m00 * d9 - m01 * d5 + m02 * d4),
              m00 * d7 - m01 * d2 + m02 * d1
        ) / det;
    }

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.Transpose']/*"/>
    public readonly Matrix4x4 Transpose() =>
        new
        (
            m00, m10, m20, m30,
            m01, m11, m21, m31,
            m02, m12, m22, m32,
            m03, m13, m23, m33
        );

    // Equality and miscellaneous
    public static bool operator ==(Matrix4x4 v1, Matrix4x4 v2) =>
        (v1.m00, v1.m01, v1.m02, v1.m03) == (v2.m00, v2.m01, v2.m02, v2.m03) &&
        (v1.m10, v1.m11, v1.m12, v1.m13) == (v2.m10, v2.m11, v2.m12, v2.m13) &&
        (v1.m20, v1.m21, v1.m22, v1.m23) == (v2.m20, v2.m21, v2.m22, v2.m23) &&
        (v1.m30, v1.m31, v1.m32, v1.m33) == (v2.m30, v2.m31, v2.m32, v2.m33);

    public static bool operator !=(Matrix4x4 v1, Matrix4x4 v2) => !(v1 == v2);

    public readonly bool Equals(Matrix4x4 m) => this == m;

    public readonly bool ApproxEquals(Matrix4x4 m, float epsilon = float.Epsilon) =>
        m00.ApproxEquals(m.m00, epsilon) &&
        m01.ApproxEquals(m.m01, epsilon) &&
        m02.ApproxEquals(m.m02, epsilon) &&
        m03.ApproxEquals(m.m03, epsilon) &&
        m10.ApproxEquals(m.m10, epsilon) &&
        m11.ApproxEquals(m.m11, epsilon) &&
        m12.ApproxEquals(m.m12, epsilon) &&
        m13.ApproxEquals(m.m13, epsilon) &&
        m20.ApproxEquals(m.m20, epsilon) &&
        m21.ApproxEquals(m.m21, epsilon) &&
        m22.ApproxEquals(m.m22, epsilon) &&
        m23.ApproxEquals(m.m23, epsilon) &&
        m30.ApproxEquals(m.m30, epsilon) &&
        m31.ApproxEquals(m.m31, epsilon) &&
        m32.ApproxEquals(m.m32, epsilon) &&
        m33.ApproxEquals(m.m33, epsilon);

    public override readonly bool Equals(object obj) => this == (Matrix4x4)obj;

    public override int GetHashCode() => (m00, m01, m02, m03, m10, m11, m12, m13, m20, m21, m22, m23, m30, m31, m32, m33).GetHashCode();

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.ToString']/*"/>
    public override readonly string ToString() =>
        $"({m00}, {m01}, {m02}, {m03}, \n" +
        $"{m10}, {m11}, {m12}, {m13}, \n" +
        $"{m20}, {m21}, {m22}, {m23}, \n" +
        $"{m30}, {m31}, {m32}, {m33})";

    #endregion

    #region Operator Overloading

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Addition(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
    public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2) =>
        new()
        {
            m00 = m1.m00 + m2.m00,
            m01 = m1.m01 + m2.m01,
            m02 = m1.m02 + m2.m02,
            m03 = m1.m03 + m2.m03,
            m10 = m1.m10 + m2.m10,
            m11 = m1.m11 + m2.m11,
            m12 = m1.m12 + m2.m12,
            m13 = m1.m13 + m2.m13,
            m20 = m1.m20 + m2.m20,
            m21 = m1.m21 + m2.m21,
            m22 = m1.m22 + m2.m22,
            m23 = m1.m23 + m2.m23,
            m30 = m1.m30 + m2.m30,
            m31 = m1.m31 + m2.m31,
            m32 = m1.m32 + m2.m32,
            m33 = m1.m33 + m2.m33
        };

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Subtraction(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
    public static Matrix4x4 operator -(Matrix4x4 m1, Matrix4x4 m2) =>
        new()
        {
            m00 = m1.m00 - m2.m00,
            m01 = m1.m01 - m2.m01,
            m02 = m1.m02 - m2.m02,
            m03 = m1.m03 - m2.m03,
            m10 = m1.m10 - m2.m10,
            m11 = m1.m11 - m2.m11,
            m12 = m1.m12 - m2.m12,
            m13 = m1.m13 - m2.m13,
            m20 = m1.m20 - m2.m20,
            m21 = m1.m21 - m2.m21,
            m22 = m1.m22 - m2.m22,
            m23 = m1.m23 - m2.m23,
            m30 = m1.m30 - m2.m30,
            m31 = m1.m31 - m2.m31,
            m32 = m1.m32 - m2.m32,
            m33 = m1.m33 - m2.m33
        };

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Matrix4x4)']/*"/>
    public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2) =>
        new
        (
            m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30,
            m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31,
            m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32,
            m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33,
            m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30,
            m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31,
            m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32,
            m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33,
            m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30,
            m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31,
            m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32,
            m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33,
            m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30,
            m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31,
            m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32,
            m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33
        );

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,_3D_Engine.Vector4D)']/*"/>
    public static Vector4D operator *(Matrix4x4 m, Vector4D v) =>
        new(
            m.m00 * v.x + m.m01 * v.y + m.m02 * v.z + m.m03 * v.w,
            m.m10 * v.x + m.m11 * v.y + m.m12 * v.z + m.m13 * v.w,
            m.m20 * v.x + m.m21 * v.y + m.m22 * v.z + m.m23 * v.w,
            m.m30 * v.x + m.m31 * v.y + m.m32 * v.z + m.m33 * v.w
        );

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Multiply(_3D_Engine.Matrix4x4,System.Single)']/*"/>
    public static Matrix4x4 operator *(Matrix4x4 m, float scalar) =>
        new()
        {
            m00 = m.m00 * scalar,
            m01 = m.m01 * scalar,
            m02 = m.m02 * scalar,
            m03 = m.m03 * scalar,
            m10 = m.m10 * scalar,
            m11 = m.m11 * scalar,
            m12 = m.m12 * scalar,
            m13 = m.m13 * scalar,
            m20 = m.m20 * scalar,
            m21 = m.m21 * scalar,
            m22 = m.m22 * scalar,
            m23 = m.m23 * scalar,
            m30 = m.m30 * scalar,
            m31 = m.m31 * scalar,
            m32 = m.m32 * scalar,
            m33 = m.m33 * scalar
        };

    /// <summary>
    /// Finds the product of a scalar and a <see cref="Matrix4x4"/>.
    /// </summary>
    /// <param name="scalar">The scalar to be multiplied.</param>
    /// <param name="m">The <see cref="Matrix4x4"/> to be multiplied.</param>
    /// <returns>The product of a scalar and a <see cref="Matrix4x4"/>.</returns>
    public static Matrix4x4 operator *(float scalar, Matrix4x4 m) => m * scalar;

    /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Matrix4x4.op_Division(_3D_Engine.Matrix4x4,System.Single)']/*"/>
    public static Matrix4x4 operator /(Matrix4x4 m, float scalar) =>
        new()
        {
            m00 = m.m00 / scalar,
            m01 = m.m01 / scalar,
            m02 = m.m02 / scalar,
            m03 = m.m03 / scalar,
            m10 = m.m10 / scalar,
            m11 = m.m11 / scalar,
            m12 = m.m12 / scalar,
            m13 = m.m13 / scalar,
            m20 = m.m20 / scalar,
            m21 = m.m21 / scalar,
            m22 = m.m22 / scalar,
            m23 = m.m23 / scalar,
            m30 = m.m30 / scalar,
            m31 = m.m31 / scalar,
            m32 = m.m32 / scalar,
            m33 = m.m33 / scalar
        };

    #endregion
}