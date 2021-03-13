/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a quaternion and provides methods to extract common information and for operator overloading. Each instance of a Quaternion has a size of 16 bytes, so, where possible, a Quaternion should be passed by reference to reduce unnecessary copying.
 */

using _3D_Engine.Constants;
using _3D_Engine.Maths.Vectors;
using System;

using static System.MathF;

namespace _3D_Engine.Maths
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Quaternion']/*"/>
    public struct Quaternion : IEquatable<Quaternion>
    {
        #region Fields and Parameters

        // Common quaternions
        /// <summary>
        /// A <see cref="Quaternion"/> equal to (0, 0, 0, 0).
        /// </summary>
        public static readonly Quaternion Zero = new Quaternion();
        /// <summary>
        /// A <see cref="Quaternion"/> equal to (1, 0, 0, 0).
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(1, 0, 0, 0);

        // Quaternion contents
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q1']/*"/>
        public float q1;
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q2']/*"/>
        public float q2;
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q3']/*"/>
        public float q3;
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q4']/*"/>
        public float q4;

        #endregion

        #region Constructors

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Single,System.Single,System.Single,System.Single)']/*"/>
        public Quaternion(float q1, float q2, float q3, float q4)
        {
            this.q1 = q1;
            this.q2 = q2;
            this.q3 = q3;
            this.q4 = q4;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Single,_3D_Engine.Vector3D)']/*"/>
        public Quaternion(float scalar, Vector3D v)
        {
            q1 = scalar;
            q2 = v.x;
            q3 = v.y;
            q4 = v.z;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Single[])']/*"/>
        public Quaternion(float[] elements)
        {
            if (elements.Length < 4) throw new ArgumentException(Exceptions.FourParameterLength, nameof(elements));
            q1 = elements[0];
            q2 = elements[1];
            q3 = elements[2];
            q4 = elements[3];
        }

        #endregion

        #region Quaternion Operations

        // Common
        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Magnitude']/*"/>
        public readonly float Magnitude() => Sqrt(SquaredMagnitude());

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Squared_Magnitude']/*"/>
        public readonly float SquaredMagnitude() => q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4;

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Normalise']/*"/>
        public readonly Quaternion Normalise() =>
            this.Approx_Equals(Zero, 1E-6f)
                ? throw Exceptions.QuaternionNormalise
                : this / Magnitude();

        // Equality and miscellaneous
        public static bool operator ==(Quaternion v1, Quaternion v2) => v1.q1 == v2.q1 && v1.q2 == v2.q2 && v1.q3 == v2.q3 && v1.q4 == v2.q4;

        public static bool operator !=(Quaternion v1, Quaternion v2) => !(v1 == v2);

        public readonly bool Equals(Quaternion q) => this == q;

        public readonly bool Approx_Equals(Quaternion q, float epsilon = float.Epsilon) =>
            this.q1.ApproxEquals(q.q1, epsilon) &&
            this.q2.ApproxEquals(q.q2, epsilon) &&
            this.q3.ApproxEquals(q.q3, epsilon) &&
            this.q4.ApproxEquals(q.q4, epsilon);

        public override readonly bool Equals(object obj) => this == (Quaternion)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.ToString']/*"/>
        public override readonly string ToString() => $"({q1}, {q2}, {q3}, {q4})";

        #endregion

        #region Operator Overloading

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Addition(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new(q1.q1 + q2.q1, q1.q2 + q2.q2, q1.q3 + q2.q3, q1.q4 + q2.q4);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Subtraction(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new(q1.q1 - q2.q1, q1.q2 - q2.q2, q1.q3 - q2.q3, q1.q4 - q2.q4);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator *(Quaternion q1, Quaternion q2) =>
            new
            (
                q1.q1 * q2.q1 - q1.q2 * q2.q2 - q1.q3 * q2.q3 - q1.q4 * q2.q4,
                q1.q1 * q2.q2 - q1.q2 * q2.q1 - q1.q3 * q2.q4 - q1.q4 * q2.q3,
                q1.q1 * q2.q3 - q1.q2 * q2.q4 - q1.q3 * q2.q1 - q1.q4 * q2.q2,
                q1.q1 * q2.q4 - q1.q2 * q2.q3 - q1.q3 * q2.q2 - q1.q4 * q2.q1
            );

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,System.Single)']/*"/>
        public static Quaternion operator *(Quaternion q, float scalar) => new(q.q1 * scalar, q.q2 * scalar, q.q3 * scalar, q.q4 * scalar);

        public static Quaternion operator *(float scalar, Quaternion q) => q * scalar;

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Division(_3D_Engine.Quaternion,System.Single)']/*"/>
        public static Quaternion operator /(Quaternion q, float scalar) => new(q.q1 / scalar, q.q2 / scalar, q.q3 / scalar, q.q4 / scalar);

        #endregion
    }
}