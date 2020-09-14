using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving four-dimensional vectors for use in 3D graphics.
    /// </summary>
    public struct Vector4D
    {
        #region Fields and Properties

        public float X;
        public float Y;
        public float Z;
        public float W;

        #endregion

        #region Constructors

        public Vector4D(float x, float y, float z, float w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4D(Vector3D v, float w = 1)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        public Vector4D(float[] data)
        {
            X = data[0];
            Y = data[1];
            Z = data[2];
            W = data[3];
        }

        #endregion

        #region Common Vectors

        public static Vector4D Zero { get; } = new Vector4D(0, 0, 0);
        public static Vector4D One { get; } = new Vector4D(1, 1, 1);
        public static Vector4D Unit_X { get; } = new Vector4D(1, 0, 0);
        public static Vector4D Unit_Y { get; } = new Vector4D(0, 1, 0);
        public static Vector4D Unit_Z { get; } = new Vector4D(0, 0, 1);
        public static Vector4D Unit_Negative_X { get; } = new Vector4D(-1, 0, 0);
        public static Vector4D Unit_Negative_Y { get; } = new Vector4D(0, -1, 0);
        public static Vector4D Unit_Negative_Z { get; } = new Vector4D(0, 0, -1);

        #endregion
        
        #region Vector Operations (Common)

        public float Angle(Vector4D v)
        {
            if (this == Vector4D.Zero || v == Vector4D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return (float)Math.Acos(quotient);
        }

        public Vector4D Cross_Product(Vector4D v) => new Vector4D(this.Y * v.Z - this.Z * v.Y, this.Z * v.X - this.X * v.Z, this.X * v.Y - this.Y * v.X, this.W);

        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => X * X + Y * Y + Z * Z + W * W;

        /// <summary>
        /// Normalises a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector4D"/>.</returns>
        public Vector4D Normalise() => (this == Vector4D.Zero) ? throw new ArgumentException("Cannot normalise a zeroed vector.") : this / Magnitude();

        public override string ToString() => $"({X}, {Y}, {Z}, {W})";

        #endregion

        #region Vector Operations

        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W);

        public static Vector4D operator +(Vector4D v1, Vector3D v2) => new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W);

        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W);

        public static float operator *(Vector4D v1, Vector4D v2) => v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W;

        public static Vector4D operator *(Vector4D v, float scalar) => new Vector4D(v.X * scalar, v.Y * scalar, v.Z * scalar, v.W * scalar);

        public static Vector4D operator *(float scalar, Vector4D v) => v * scalar;

        public static Vector4D operator /(Vector4D v, float scalar) => new Vector4D(v.X / scalar, v.Y / scalar, v.Z / scalar, v.W / scalar);

        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.X, -v.Y, -v.Z, -v.W);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Vector4D v1, Vector4D v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z && v1.W == v2.W;

        public static bool operator !=(Vector4D v1, Vector4D v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Vector4D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}