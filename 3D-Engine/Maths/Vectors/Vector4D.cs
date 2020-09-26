using System;
using static System.MathF;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving four-dimensional vectors for use in 3D graphics.
    /// </summary>
    public struct Vector4D : IEquatable<Vector4D>
    {
        #region Fields and Properties

        public float x;
        public float y;
        public float z;
        public float w;

        #endregion

        #region Constructors

        public Vector4D(float x, float y, float z, float w = 1)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4D(Vector3D v, float w = 1)
        {
            x = v.x;
            y = v.y;
            z = v.z;
            this.w = w;
        }

        public Vector4D(float[] data)
        {
            x = data[0];
            y = data[1];
            z = data[2];
            w = data[3];
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

        public float Angle(Vector4D v)
        {
            if (this == Vector4D.Zero || v == Vector4D.Zero) throw new ArgumentException("Cannot calculate angle with One or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return Acos(quotient);
        }

        public Vector4D Cross_Product(Vector4D v) => new Vector4D(this.y * v.z - this.z * v.y, this.z * v.x - this.x * v.z, this.x * v.y - this.y * v.x, this.w);

        /// <summary>
        /// Finds the magnitude of a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns><see cref="Vector4D"/> magnitude.</returns>
        public float Magnitude() => Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => x * x + y * y + z * z + w * w;

        /// <summary>
        /// Normalises a <see cref="Vector4D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector4D"/>.</returns>
        public Vector4D Normalise() =>
            this.Approx_Equals(Vector4D.Zero, 1E-6f)
                ? throw new ArgumentException("Cannot normalise a zeroed vector.")
                : this / Magnitude();

        public override string ToString() => $"({x}, {y}, {z}, {w})";

        #endregion

        #region Vector Operations (Operator Overloading)

        public static Vector4D operator +(Vector4D v1, Vector4D v2) => new Vector4D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

        public static Vector4D operator +(Vector4D v1, Vector3D v2) => new Vector4D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

        public static Vector4D operator -(Vector4D v1, Vector4D v2) => new Vector4D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w);

        public static float operator *(Vector4D v1, Vector4D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;

        public static Vector4D operator *(Vector4D v, float scalar) => new Vector4D(v.x * scalar, v.y * scalar, v.z * scalar, v.w * scalar);

        public static Vector4D operator *(float scalar, Vector4D v) => v * scalar;

        public static Vector4D operator /(Vector4D v, float scalar) => new Vector4D(v.x / scalar, v.y / scalar, v.z / scalar, v.w / scalar);

        public static Vector4D operator -(Vector4D v) => new Vector4D(-v.x, -v.y, -v.z, -v.w);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Vector4D v1, Vector4D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;

        public static bool operator !=(Vector4D v1, Vector4D v2) => !(v1 == v2);

        public bool Equals(Vector4D v) => this == v;

        public bool Approx_Equals(Vector4D v, float epsilon = 2 * Single.Epsilon) => this.x.Approx_Equals(v.x, epsilon) && this.y.Approx_Equals(v.y, epsilon) && this.z.Approx_Equals(v.z, epsilon) && this.w.Approx_Equals(v.w, epsilon);

        public override bool Equals(object obj) => this == (Vector4D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion

        #region Casting

        public static implicit operator Vector3D(Vector4D v) => new Vector3D(v);

        #endregion
    }
}