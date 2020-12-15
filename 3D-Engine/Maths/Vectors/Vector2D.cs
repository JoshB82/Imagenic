/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a two-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector2D has a size of 8 bytes, so, where possible, a Vector2D should be passed by reference to reduce unnecessary copying (depending on the architecture of the machine).
 */

using System;
using static System.MathF;

namespace _3D_Engine.Maths.Vectors
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Vector2D']/*"/>
    public struct Vector2D : IEquatable<Vector2D>
    {
        #region Fields and Properties

        // Common vectors
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (0, 0).
        /// </summary>
        public static readonly Vector2D Zero = new Vector2D(0, 0);
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (1, 1).
        /// </summary>
        public static readonly Vector2D One = new Vector2D(1, 1);
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (1, 0).
        /// </summary>
        public static readonly Vector2D UnitX = new Vector2D(1, 0);
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (0, 1).
        /// </summary>
        public static readonly Vector2D UnitY = new Vector2D(0, 1);
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (-1, 0).
        /// </summary>
        public static readonly Vector2D UnitNegativeX = new Vector2D(-1, 0);
        /// <summary>
        /// A <see cref="Vector2D"/> equal to (0, -1).
        /// </summary>
        public static readonly Vector2D UnitNegativeY = new Vector2D(0, -1);

        // Vector contents
        public float x;
        public float y;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Vector2D"/> from two values.
        /// </summary>
        /// <param name="x">The value to be put at the x component of the <see cref="Vector2D"/>.</param>
        /// <param name="y">The value to be put at the y component of the <see cref="Vector2D"/>.</param>
        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Creates a <see cref="Vector2D"/> from an array of elements.
        /// </summary>
        /// <param name="elements">The array containing elements to be put in the <see cref="Vector2D"/>.</param>
        public Vector2D(float[] elements)
        {
            if (elements.Length < 2) throw new ArgumentException("Parameter \"elements\" must at least be of length 2.", nameof(elements));
            x = elements[0];
            y = elements[1];
        }

        #endregion

        #region Vector Operations

        // Common
        public readonly float Angle(Vector2D v)
        {
            if (this == Vector2D.Zero || v == Vector2D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zero vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Acos(quotient);
        }

        /// <summary>
        /// Finds the magnitude of a <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>The magnitude of a <see cref="Vector2D"/>.</returns>
        public readonly float Magnitude() => Sqrt(Squared_Magnitude());

        /// <summary>
        /// Finds the squared magnitude of a <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>The squared magnitude of a <see cref="Vector2D"/>.</returns>
        public readonly float Squared_Magnitude() => x * x + y * y;

        /// <summary>
        /// Normalises a <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector2D"/>.</returns>
        public readonly Vector2D Normalise() =>
            this.Approx_Equals(Vector2D.Zero, 1E-6f)
            ? throw new ArgumentException("Cannot normalise a zero vector.")
            : this / Magnitude();

        // Equality and miscellaneous
        public static bool operator ==(Vector2D v1, Vector2D v2) => v1.x == v2.x && v1.y == v2.y;

        public static bool operator !=(Vector2D v1, Vector2D v2) => !(v1 == v2);

        public readonly bool Equals(Vector2D v) => this == v;

        public readonly bool Approx_Equals(Vector2D v, float epsilon = float.Epsilon) => this.x.Approx_Equals(v.x, epsilon) && this.y.Approx_Equals(v.y, epsilon);

        public override readonly bool Equals(object obj) => this == (Vector2D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        public override readonly string ToString() => $"({x}, {y})";

        #endregion

        #region Operator Overloading

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.x + v2.x, v1.y + v2.y);

        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.x - v2.x, v1.y - v2.y);

        public static float operator *(Vector2D v1, Vector2D v2) => v1.x * v2.x + v1.y * v2.y;

        public static Vector2D operator *(Vector2D v, float scalar) => new Vector2D(v.x * scalar, v.y * scalar);

        public static Vector2D operator *(float scalar, Vector2D v) => v * scalar;

        public static Vector2D operator /(Vector2D v, float scalar) => new Vector2D(v.x / scalar, v.y / scalar);

        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.x, -v.y);

        #endregion

        #region Casting

        public static implicit operator Vector3D(Vector2D v) => new Vector3D(v);

        #endregion
    }
}