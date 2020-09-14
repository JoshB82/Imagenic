using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving three-dimensional vectors.
    /// </summary>
    public struct Vector3D
    {
        #region Fields and Properties

        public float X;
        public float Y;
        public float Z;

        #endregion

        #region Constructors

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(Vector2D v, float z = 1)
        {
            X = v.X;
            Y = v.Y;
            Z = z;
        }

        public Vector3D(Vector4D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public Vector3D(float[] data)
        {
            X = data[0];
            Y = data[1];
            Z = data[2];
        }

        #endregion

        #region Common Vectors

        public static Vector3D Zero { get; } = new Vector3D(0, 0, 0);
        public static Vector3D One { get; } = new Vector3D(1, 1, 1);
        public static Vector3D Unit_X { get; } = new Vector3D(1, 0, 0);
        public static Vector3D Unit_Y { get; } = new Vector3D(0, 1, 0);
        public static Vector3D Unit_Z { get; } = new Vector3D(0, 0, 1);
        public static Vector3D Unit_Negative_X { get; } = new Vector3D(-1, 0, 0);
        public static Vector3D Unit_Negative_Y { get; } = new Vector3D(0, -1, 0);
        public static Vector3D Unit_Negative_Z { get; } = new Vector3D(0, 0, -1);

        #endregion

        #region Vector Operations (Common)

        // SMALLEST ANGLE
        public float Angle(Vector3D v)
        {
            if (this == Vector3D.Zero || v == Vector3D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return (float)Math.Acos(quotient);
        }

        public Vector3D Cross_Product(Vector3D v) => new Vector3D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);

        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => X * X + Y * Y + Z * Z;

        /// <summary>
        /// Normalises a <see cref="Vector3D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector3D"/>.</returns>
        public Vector3D Normalise() => (this == Vector3D.Zero) ? throw new ArgumentException("Cannot normalise a zeroed vector.") : this / Magnitude();

        public override string ToString() => $"({X}, {Y}, {Z})";

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
            // Y-AXES WRONG (upside down)?
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

        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);

        public static float operator *(Vector3D v1, Vector3D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        public static Vector3D operator *(Vector3D v, float scalar) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);

        public static Vector3D operator *(float scalar, Vector3D v) => v * scalar;

        public static Vector3D operator /(Vector3D v, float scalar) => new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);

        public static Vector3D operator -(Vector3D v) => new Vector3D(-v.X, -v.Y, -v.Z);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Vector3D v1, Vector3D v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;

        public static bool operator !=(Vector3D v1, Vector3D v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Vector3D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}