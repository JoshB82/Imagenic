using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving three-dimensional vectors.
    /// </summary>
    public struct Vector3D
    {
        #region Fields and Properties

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        #endregion

        #region Constructors

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        internal Vector3D(Vector2D v, double z)
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

        public Vector3D(double[] data)
        {
            X = data[0];
            Y = data[1];
            Z = data[2];
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
        public double Angle(Vector3D v)
        {
            double quotient = (this * v) / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Math.Acos(quotient);
        }

        public Vector3D Cross_Product(Vector3D v) => new Vector3D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X);

        public double Magnitude() => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));

        public Vector3D Normalise() => this / Magnitude();

        public override string ToString() => $"({X}, {Y}, {Z})";

        #endregion

        #region Vector Operations (Geometry)

        public static Vector3D Line_Intersect_Plane(Vector3D line_start, Vector3D line_finish, Vector3D plane_point, Vector3D plane_normal, out double d)
        {
            Vector3D line = (line_finish - line_start);
            // d = new length / old length
            d = ((plane_point - line_start) * plane_normal) / (line * plane_normal);
            // Round in direction of normal!?
            // Y-AXES WRONG (upside down)?
            return line * d + line_start;
        }

        /// <summary>
        /// Calculates a normal vector to a plane defined by three points. Looking in the direction of the vector, the points are arranged in an anticlockwise order.
        /// </summary>
        /// <param name="p1">First point on the plane.</param>
        /// <param name="p2">Second point on the plane.</param>
        /// <param name="p3">Third point on the plane.</param>
        /// <returns>A normal vector.</returns>
        public static Vector3D Normal_From_Plane(Vector3D p1, Vector3D p2, Vector3D p3) => (p3 - p1).Cross_Product(p2 - p1).Normalise();

        public static double Point_Distance_From_Plane(Vector3D point, Vector3D plane_point, Vector3D plane_normal) => point * plane_normal - plane_point * plane_normal;

        #endregion

        #region Vector Operations (Operator Overloading)

        public static Vector3D operator +(Vector3D v1, Vector3D v2) => new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Vector3D operator -(Vector3D v1, Vector3D v2) => new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static double operator *(Vector3D v1, Vector3D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        public static Vector3D operator *(Vector3D v, double scalar) => new Vector3D(v.X * scalar, v.Y * scalar, v.Z * scalar);
        public static Vector3D operator /(Vector3D v, double scalar) => new Vector3D(v.X / scalar, v.Y / scalar, v.Z / scalar);
        public static Vector3D operator -(Vector3D v) => new Vector3D(-v.X, -v.Y, -v.Z);
        public static bool operator ==(Vector3D v1, Vector3D v2) => (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z);
        public static bool operator !=(Vector3D v1, Vector3D v2) => !(v1 == v2);

        #endregion
    }
}