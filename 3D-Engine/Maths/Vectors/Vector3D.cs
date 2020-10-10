/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a three-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector3D has a size of 12 bytes, so, where possible, a Vector3D should be passed by reference to reduce unnecessary copying.
 */

using System;
using static System.MathF;

namespace _3D_Engine
{
    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Vector3D']/*"/>
    public struct Vector3D : IEquatable<Vector3D>
    {
        #region Fields and Properties

        // Common vectors
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (0, 0, 0).
        /// </summary>
        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (1, 1, 1).
        /// </summary>
        public static readonly Vector3D One = new Vector3D(1, 1, 1);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (1, 0, 0).
        /// </summary>
        public static readonly Vector3D Unit_X = new Vector3D(1, 0, 0);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (0, 1, 0).
        /// </summary>
        public static readonly Vector3D Unit_Y = new Vector3D(0, 1, 0);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (0, 0, 1).
        /// </summary>
        public static readonly Vector3D Unit_Z = new Vector3D(0, 0, 1);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (-1, 0, 0).
        /// </summary>
        public static readonly Vector3D Unit_Negative_X = new Vector3D(-1, 0, 0);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (0, -1, 0).
        /// </summary>
        public static readonly Vector3D Unit_Negative_Y = new Vector3D(0, -1, 0);
        /// <summary>
        /// A <see cref="Vector3D"/> equal to (0, 0, -1).
        /// </summary>
        public static readonly Vector3D Unit_Negative_Z = new Vector3D(0, 0, -1);

        // Vector contents
        public float x;
        public float y;
        public float z;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Vector3D"/> from three values.
        /// </summary>
        /// <param name="x">The value to be put at the x component of the <see cref="Vector3D"/>.</param>
        /// <param name="y">The value to be put at the y component of the <see cref="Vector3D"/>.</param>
        /// <param name="z">The value to be put at the z component of the <see cref="Vector3D"/>.</param>
        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Creates a <see cref="Vector3D"/> from the values in a <see cref="Vector2D"/> and a further value.
        /// </summary>
        /// <param name="v">
        /// The <see cref="Vector2D"/> containing values to be put in the <see cref="Vector3D"/> at the x and y components.
        /// </param>
        /// <param name="z">
        /// The value to be put in the <see cref="Vector3D"/> at the z component.
        /// </param>
        public Vector3D(Vector2D v, float z = 0)
        {
            x = v.x;
            y = v.y;
            this.z = z;
        }

        /// <summary>
        /// Creates a <see cref="Vector3D"/> from an array of elements.
        /// </summary>
        /// <param name="elements">The array containing elements to be put in the <see cref="Vector3D"/>.</param>
        public Vector3D(float[] elements)
        {
            if (elements.Length < 3) throw new ArgumentException("Parameter \"elements\" must at least be of length 3.", nameof(elements));
            x = elements[0];
            y = elements[1];
            z = elements[2];
        }

        #endregion

        #region Vector Operations

        // Common
        /// <summary>
        /// Finds the smallest angle between two <see cref="Vector3D">Vector3Ds</see>.
        /// </summary>
        /// <param name="v">A <see cref="Vector3D"/> creating an angle from the current <see cref="Vector3D"/> instance.</param>
        /// <returns>The angle between two <see cref="Vector3D">Vector3Ds</see>.</returns>
        public readonly float Angle(Vector3D v)
        {
            if (this == Vector3D.Zero || v == Vector3D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zero vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Acos(quotient);
        }

        /// <summary>
        /// Finds the cross product of two <see cref="Vector3D">Vector3Ds</see>.
        /// </summary>
        /// <param name="v">A <see cref="Vector3D"/> used in calculating the cross product with the current <see cref="Vector3D"/> instance.</param>
        /// <returns>The cross product of two <see cref="Vector3D">Vector3Ds</see>.</returns>
        public readonly Vector3D Cross_Product(Vector3D v) => new Vector3D(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x);

        /// <summary>
        /// Finds the magnitude of a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>The magnitude of a <see cref="Vector3D"/>.</returns>
        public readonly float Magnitude() => Sqrt(Squared_Magnitude());

        /// <summary>
        /// Finds the squared magnitude of a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>The squared magnitude of a <see cref="Vector3D"/>.</returns>
        public readonly float Squared_Magnitude() => x * x + y * y + z * z;

        /// <summary>
        /// Normalises a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector3D"/>.</returns>
        public readonly Vector3D Normalise() =>
            this.Approx_Equals(Vector3D.Zero, 1E-6f)
                ? throw new ArgumentException("Cannot normalise a zero vector.")
                : this / Magnitude();

        // Equality and miscellaneous
        public static bool operator ==(Vector3D v1, Vector3D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;

        public static bool operator !=(Vector3D v1, Vector3D v2) => !(v1 == v2);

        public readonly bool Equals(Vector3D v) => this == v;

        public readonly bool Approx_Equals(Vector3D v, float epsilon = float.Epsilon) => this.x.Approx_Equals(v.x, epsilon) && this.y.Approx_Equals(v.y, epsilon) && this.z.Approx_Equals(v.z, epsilon);

        public override readonly bool Equals(object obj) => this == (Vector3D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        public override readonly string ToString() => $"({x}, {y}, {z})";

        #endregion

        #region Vector Operations (Geometry)

        public static Vector3D Line_Intersect_Plane(Vector3D line_start, Vector3D line_finish, Vector3D plane_point, Vector3D plane_normal, out float d)
        {
            Vector3D line = line_finish - line_start;
            float denominator = line * plane_normal;
            //if (denominator == 0) throw new ArgumentException("Line does not intersect plane or exists entirely on plane.");

            // d = new length / old length
            d = (plane_point - line_start) * plane_normal / (denominator);
            // Round in direction of normal!?
            return line * d + line_start;
        }

        /// <summary>
        /// Calculates a normal vector to a plane defined by three points. Looking in the direction of the desired normal vector and at the points, the points are arranged in an clockwise order.
        /// </summary>
        /// <param name="p1">First point on the plane.</param>
        /// <param name="p2">Second point on the plane.</param>
        /// <param name="p3">Third point on the plane.</param>
        /// <returns>A normal vector.</returns>
        public static Vector3D Normal_From_Plane(Vector3D p1, Vector3D p2, Vector3D p3) => (p3 - p1).Cross_Product(p2 - p1).Normalise();

        public static float Point_Distance_From_Plane(Vector3D point, Vector3D plane_point, Vector3D plane_normal) => point * plane_normal - plane_point * plane_normal;

        #endregion

        #region Operator Overloading

        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static float operator *(Vector3D v1, Vector3D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

        public static Vector3D operator *(Vector3D v, float scalar) => new Vector3D(v.x * scalar, v.y * scalar, v.z * scalar);

        public static Vector3D operator *(float scalar, Vector3D v) => v * scalar;

        public static Vector3D operator /(Vector3D v, float scalar) => new Vector3D(v.x / scalar, v.y / scalar, v.z / scalar);

        public static Vector3D operator -(Vector3D v) => new Vector3D(-v.x, -v.y, -v.z);

        #endregion

        #region Casting

        public static explicit operator Vector2D(Vector3D v) => new Vector2D(v.x, v.y);

        public static implicit operator Vector4D(Vector3D v) => new Vector4D(v);

        #endregion
    }
}