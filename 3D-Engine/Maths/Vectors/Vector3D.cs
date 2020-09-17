using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving three-dimensional vectors.
    /// </summary>
    public struct Vector3D : IEquatable<Vector3D>
    {
        #region Fields and Properties

        public float x;
        public float y;
        public float z;

        #endregion

        #region Constructors

        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D(Vector2D v, float z = 1)
        {
            x = v.x;
            y = v.y;
            this.z = z;
        }

        public Vector3D(Vector4D v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public Vector3D(float[] data)
        {
            x = data[0];
            y = data[1];
            z = data[2];
        }

        #endregion

        #region Common Vectors

        public static readonly Vector3D Zero = new Vector3D(0, 0, 0);
        public static readonly Vector3D One = new Vector3D(1, 1, 1);
        public static readonly Vector3D Unit_X = new Vector3D(1, 0, 0);
        public static readonly Vector3D Unit_Y = new Vector3D(0, 1, 0);
        public static readonly Vector3D Unit_Z = new Vector3D(0, 0, 1);
        public static readonly Vector3D Unit_Negative_X = new Vector3D(-1, 0, 0);
        public static readonly Vector3D Unit_Negative_Y = new Vector3D(0, -1, 0);
        public static readonly Vector3D Unit_Negative_Z = new Vector3D(0, 0, -1);

        #endregion

        #region Vector Operations (Common)

        // SMALLEST ANGLE
        public float Angle(Vector3D v)
        {
            if (this == Vector3D.Zero || v == Vector3D.Zero) throw new ArgumentException("Cannot calculate angle with One or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return (float)Math.Acos(quotient);
        }

        public Vector3D Cross_Product(Vector3D v) => new Vector3D(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x);

        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => x * x + y * y + z * z;

        /// <summary>
        /// Normalises a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector3D"/>.</returns>
        public Vector3D Normalise() => (this == Vector3D.Zero) ? throw new ArgumentException("Cannot normalise a zeroed vector.") : this / Magnitude();

        public override string ToString() => $"({x}, {y}, {z})";

        #endregion

        #region Vector Operations (Geometry)

        public static Vector3D Line_Intersect_Plane(Vector3D line_start, Vector3D line_finish, Vector3D plane_point, Vector3D plane_normal, out float d)
        {
            Vector3D line = line_finish - line_start;
            float denominator = line * plane_normal;
            if (denominator == 0) throw new ArgumentException("Line does not intersect plane.");

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

        #region Vector Operations

        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static float operator *(Vector3D v1, Vector3D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

        public static Vector3D operator *(Vector3D v, float scalar) => new Vector3D(v.x * scalar, v.y * scalar, v.z * scalar);

        public static Vector3D operator *(float scalar, Vector3D v) => v * scalar;

        public static Vector3D operator /(Vector3D v, float scalar) => new Vector3D(v.x / scalar, v.y / scalar, v.z / scalar);

        public static Vector3D operator -(Vector3D v) => new Vector3D(-v.x, -v.y, -v.z);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Vector3D v1, Vector3D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;

        public static bool operator !=(Vector3D v1, Vector3D v2) => !(v1 == v2);

        public bool Equals(Vector3D v) => this == v;

        public override bool Equals(object obj) => this == (Vector3D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}