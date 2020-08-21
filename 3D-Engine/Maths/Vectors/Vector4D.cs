using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving four-dimensional vectors for use in 3D graphics.
    /// </summary>
    public struct Vector4D
    {
        #region Fields and Properties

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        #endregion

        #region Constructors

        public Vector4D(double x, double y, double z, double w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4D(Vector3D v, double w = 1)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        public Vector4D(double[] data)
        {
            X = data[0];
            Y = data[1];
            Z = data[2];
            W = data[3];
        }

        #endregion

        #region Common Vectors

        public static readonly Vector4D Zero = new Vector4D(0, 0, 0);
        public static readonly Vector4D One = new Vector4D(1, 1, 1);
        public static readonly Vector4D Unit_X = new Vector4D(1, 0, 0);
        public static readonly Vector4D Unit_Y = new Vector4D(0, 1, 0);
        public static readonly Vector4D Unit_Z = new Vector4D(0, 0, 1);
        public static readonly Vector4D Unit_Negative_X = new Vector4D(-1, 0, 0);
        public static readonly Vector4D Unit_Negative_Y = new Vector4D(0, -1, 0);
        public static readonly Vector4D Unit_Negative_Z = new Vector4D(0, 0, -1);

        #endregion

        #region Vector Operations (Common)

        public double Angle(Vector4D v)
        {
            double quotient = (this * v) / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Math.Acos(quotient);
        }

        public Vector4D Cross_Product(Vector4D v) => new Vector4D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X, this.W);

        public double Magnitude() => Math.Sqrt(Squared_Magnitude());

        public double Squared_Magnitude() => Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2) + Math.Pow(W, 2);

        public Vector4D Normalise() => this / Magnitude();

        public override string ToString() => $"({X}, {Y}, {Z}, {W})";

        #endregion

        #region Vector Operations

        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W);
        public static Vector4D operator +(Vector4D v1, Vector3D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W);
        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W);
        public static double operator *(Vector4D v1, Vector4D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;
        public static Vector4D operator *(Vector4D v, double scalar) => new Vector4D(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);
        public static Vector4D operator /(Vector4D v, double scalar) => new Vector4D(v.X / scalar, v.Y / scalar, v.Z / scalar, v.W / scalar);
        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.X, -v.Y, -v.Z, -v.W);

        #endregion

        #region Equality

        public static bool operator ==(Vector4D v1, Vector4D v2) => (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W);
        public static bool operator !=(Vector4D v1, Vector4D v2) => !(v1 == v2);
        public override bool Equals(object obj) => this == (Vector4D)obj;
        // Get hash code

        #endregion
    }
}