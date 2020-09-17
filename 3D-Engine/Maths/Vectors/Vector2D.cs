using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving two-dimensional vectors.
    /// </summary>
    public struct Vector2D : IEquatable<Vector2D>
    {
        #region Fields and Properties

        public float x;
        public float y;

        #endregion

        #region Constructors

        public Vector2D(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2D(Vector3D v)
        {
            x = v.x;
            y = v.y;
        }

        public Vector2D(float[] data)
        {
            x = data[0];
            y = data[1];
        }

        #endregion

        #region Common Vectors

        public static readonly Vector2D Zero  = new Vector2D(0, 0);
        public static readonly Vector2D One  = new Vector2D(1, 1);
        public static readonly Vector2D Unit_X  = new Vector2D(1, 0);
        public static readonly Vector2D Unit_Y  = new Vector2D(0, 1);
        public static readonly Vector2D Unit_Negative_X  = new Vector2D(-1, 0);
        public static readonly Vector2D Unit_Negative_Y  = new Vector2D(0, -1);

        #endregion

        #region Vector Operations (Common)

        public float Angle(Vector2D v)
        {
            if (this == Vector2D.Zero || v == Vector2D.Zero) throw new ArgumentException("Cannot calculate angle with One or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return (float)Math.Acos(quotient);
        }

        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => x * x + y * y;

        /// <summary>
        /// Normalises a <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector2D"/>.</returns>
        public Vector2D Normalise() => (this == Vector2D.Zero) ? throw new ArgumentException("Cannot normalise a zeroed vector.") : this / Magnitude();

        public override string ToString() => $"({x}, {y})";

        #endregion

        #region Vector Operations

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.x + v2.x, v1.y + v2.y);

        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.x - v2.x, v1.y - v2.y);

        public static float operator *(Vector2D v1, Vector2D v2) => v1.x * v2.x + v1.y * v2.y;

        public static Vector2D operator *(Vector2D v, float scalar) => new Vector2D(v.x * scalar, v.y * scalar);

        public static Vector2D operator *(float scalar, Vector2D v) => v * scalar;

        public static Vector2D operator /(Vector2D v, float scalar) => new Vector2D(v.x / scalar, v.y / scalar);

        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.x, -v.y);

        #endregion
        
        #region Equality and Miscellaneous

        public static bool operator ==(Vector2D v1, Vector2D v2) => v1.x == v2.x && v1.y == v2.y;

        public static bool operator !=(Vector2D v1, Vector2D v2) => !(v1 == v2);

        public bool Equals(Vector2D v) => this == v;

        public override bool Equals(object obj) => this == (Vector2D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion

    }
}