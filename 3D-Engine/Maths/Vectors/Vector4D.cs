﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a four-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector4D has a size of 16 bytes, so, where possible, a Vector4D should be passed by reference to reduce unnecessary copying.
 */

using System;
using static System.MathF;

namespace _3D_Engine.Maths.Vectors
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Vector4D']/*"/>
    public struct Vector4D : IEquatable<Vector4D>
    {
        #region Fields and Properties

        // Common vectors
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 0, 0, 0).
        /// </summary>
        public static readonly Vector4D Zero = new Vector4D();
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (1, 1, 1, 1).
        /// </summary>
        public static readonly Vector4D One = new Vector4D(1, 1, 1, 1);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (1, 0, 0, 0).
        /// </summary>
        public static readonly Vector4D Unit_X = new Vector4D(1, 0, 0, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 1, 0, 0).
        /// </summary>
        public static readonly Vector4D Unit_Y = new Vector4D(0, 1, 0, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 0, 1, 0).
        /// </summary>
        public static readonly Vector4D Unit_Z = new Vector4D(0, 0, 1, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 0, 0, 1).
        /// </summary>
        public static readonly Vector4D Unit_W = new Vector4D(0, 0, 0, 1);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (-1, 0, 0, 0).
        /// </summary>
        public static readonly Vector4D Unit_Negative_X = new Vector4D(-1, 0, 0, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, -1, 0, 0).
        /// </summary>
        public static readonly Vector4D Unit_Negative_Y = new Vector4D(0, -1, 0, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 0, -1, 0).
        /// </summary>
        public static readonly Vector4D Unit_Negative_Z = new Vector4D(0, 0, -1, 0);
        /// <summary>
        /// A <see cref="Vector4D"/> equal to (0, 0, 0, -1).
        /// </summary>
        public static readonly Vector4D Unit_Negative_W = new Vector4D(0, 0, 0, -1);

        // Vector contents
        /// <summary>
        /// First component of the <see cref="Vector4D"/>, equivalent to the q1 component of a <see cref="Quaternion"/>.
        /// </summary>
        public float x;
        /// <summary>
        /// Second component of the <see cref="Vector4D"/>, equivalent to the q2 component of a <see cref="Quaternion"/>.
        /// </summary>
        public float y;
        /// <summary>
        /// Third component of the <see cref="Vector4D"/>, equivalent to the q3 component of a <see cref="Quaternion"/>.
        /// </summary>
        public float z;
        /// <summary>
        /// Fourth component of the <see cref="Vector4D"/>, equivalent to the q4 component of a <see cref="Quaternion"/>.
        /// </summary>
        public float w;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Vector4D"/> from four values.
        /// </summary>
        /// <param name="x">The value to be put at the x component of the <see cref="Vector4D"/>.</param>
        /// <param name="y">The value to be put at the y component of the <see cref="Vector4D"/>.</param>
        /// <param name="z">The value to be put at the z component of the <see cref="Vector4D"/>.</param>
        /// <param name="w">The value to be put at the w component of the <see cref="Vector4D"/>.</param>
        public Vector4D(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Creates a <see cref="Vector4D"/> from the values in a <see cref="Vector3D"/> and a further value.
        /// </summary>
        /// <param name="v">
        /// The <see cref="Vector3D"/> containing values to be put in the <see cref="Vector4D"/> at the x, y and z components.
        /// </param>
        /// <param name="w">
        /// The value to be put in the <see cref="Vector4D"/> at the w component.
        /// </param>
        public Vector4D(Vector3D v, float w = 0)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            this.w = w;
        }

        /// <summary>
        /// Creates a <see cref="Vector4D"/> from the values in a <see cref="Vector2D"/> and two further values.
        /// </summary>
        /// <param name="v">
        /// The <see cref="Vector2D"/> containing values to be put in the <see cref="Vector4D"/> at the x and y components.
        /// </param>
        /// <param name="z">
        /// The value to be put in the <see cref="Vector4D"/> at the z component.
        /// </param>
        /// <param name="w">
        /// The value to be put in the <see cref="Vector4D"/> at the w component.
        /// </param>
        public Vector4D(Vector2D v, float z = 0, float w = 0)
        {
            x = v.x;
            y = v.y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Creates a <see cref="Vector4D"/> from an array of elements.
        /// </summary>
        /// <param name="elements">The array containing elements to be put in the <see cref="Vector4D"/>.</param>
        public Vector4D(float[] elements)
        {
            if (elements.Length < 4) throw new ArgumentException("Parameter \"elements\" must at least be of length 4.", nameof(elements));
            x = elements[0];
            y = elements[1];
            z = elements[2];
            w = elements[3];
        }

        #endregion

        #region Vector Operations

        // Common
        public readonly float Angle(Vector4D v)
        {
            if (this == Vector4D.Zero || v == Vector4D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zero vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Acos(quotient);
        }

        /// <summary>
        /// Finds the magnitude of a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>The magnitude of a <see cref="Vector4D"/>.</returns>
        public readonly float Magnitude() => Sqrt(Squared_Magnitude());

        /// <summary>
        /// Finds the squared magnitude of a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>The squared magnitude of a <see cref="Vector4D"/>.</returns>
        public readonly float Squared_Magnitude() => x * x + y * y + z * z + w * w;

        /// <summary>
        /// Normalises a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector4D"/>.</returns>
        public readonly Vector4D Normalise() =>
            this.Approx_Equals(Vector4D.Zero, 1E-6f)
                ? throw new ArgumentException("Cannot normalise a zero vector.")
                : this / Magnitude();

        // Equality and miscellaneous
        public static bool operator ==(Vector4D v1, Vector4D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;

        public static bool operator !=(Vector4D v1, Vector4D v2) => !(v1 == v2);

        public readonly bool Equals(Vector4D v) => this == v;

        public readonly bool Approx_Equals(Vector4D v, float epsilon = float.Epsilon) =>
            this.x.Approx_Equals(v.x, epsilon) &&
            this.y.Approx_Equals(v.y, epsilon) &&
            this.z.Approx_Equals(v.z, epsilon) &&
            this.w.Approx_Equals(v.w, epsilon);

        public override readonly bool Equals(object obj) => this == (Vector4D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        public override readonly string ToString() => $"({x}, {y}, {z}, {w})";

        #endregion

        #region Operator Overloading

        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

        public static Vector4D operator +(Vector4D v1, Vector3D v2) => new Vector4D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w);

        public static float operator *(Vector4D v1, Vector4D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;

        public static Vector4D operator *(Vector4D v, float scalar) => new Vector4D(v.x * scalar, v.y * scalar, v.z * scalar, v.w * scalar);

        public static Vector4D operator *(float scalar, Vector4D v) => v * scalar;

        public static Vector4D operator /(Vector4D v, float scalar) => new Vector4D(v.x / scalar, v.y / scalar, v.z / scalar, v.w / scalar);

        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.x, -v.y, -v.z, -v.w);

        #endregion

        #region Casting

        public static explicit operator Quaternion(Vector4D v) => new Quaternion(v.x, v.y, v.z, v.w);

        public static explicit operator Vector3D(Vector4D v) => new Vector3D(v.x, v.y, v.z);

        #endregion
    }
}