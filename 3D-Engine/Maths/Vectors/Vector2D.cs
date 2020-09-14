using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles constructors and operations involving two-dimensional vectors.
    /// </summary>
    public struct Vector2D
    {
        #region Fields and Properties

        public float X;
        public float Y;

        #endregion

        #region Constructors

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector3D v)
        {
            X = v.X;
            Y = v.Y;
        }

        public Vector2D(float[] data)
        {
            X = data[0];
            Y = data[1];
        }

        #endregion

        #region Common Vectors

        public static Vector2D Zero { get; } = new Vector2D(0, 0);
        public static Vector2D One { get; } = new Vector2D(1, 1);
        public static Vector2D Unit_X { get; } = new Vector2D(1, 0);
        public static Vector2D Unit_Y { get; } = new Vector2D(0, 1);
        public static Vector2D Unit_Negative_X { get; } = new Vector2D(-1, 0);
        public static Vector2D Unit_Negative_Y { get; } = new Vector2D(0, -1);

        #endregion

        #region Vector Operations (Common)

        public float Angle(Vector2D v)
        {
            if (this == Vector2D.Zero || v == Vector2D.Zero) throw new ArgumentException("Cannot calculate angle with one or more zeroed vectors."); //?
            float quotient = this * v / (this.Magnitude() * v.Magnitude());
            if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
            return (float)Math.Acos(quotient);
        }

        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        public float Squared_Magnitude() => X * X + Y * Y;

        /// <summary>
        /// Normalises a <see cref="Vector2D"/>.
        /// </summary>
        /// <returns>A normalised <see cref="Vector2D"/>.</returns>
        public Vector2D Normalise() => (this == Vector2D.Zero) ? throw new ArgumentException("Cannot normalise a zeroed vector.") : this / Magnitude();

        public override string ToString() => $"({X}, {Y})";

        #endregion

        #region Vector Operations

        public static Vector2D operator +(Vector2D v1, Vector2D v2) => new Vector2D(v1.X + v2.X, v1.Y + v2.Y);

        public static Vector2D operator -(Vector2D v1, Vector2D v2) => new Vector2D(v1.X - v2.X, v1.Y - v2.Y);

        public static float operator *(Vector2D v1, Vector2D v2) => v1.X * v2.X + v1.Y * v2.Y;

        public static Vector2D operator *(Vector2D v, float scalar) => new Vector2D(v.X * scalar, v.Y * scalar);

        public static Vector2D operator *(float scalar, Vector2D v) => v * scalar;

        public static Vector2D operator /(Vector2D v, float scalar) => new Vector2D(v.X / scalar, v.Y / scalar);

        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);

        #endregion
        
        #region Equality and Miscellaneous

        public static bool operator ==(Vector2D v1, Vector2D v2) => v1.X == v2.X && v1.Y == v2.Y;

        public static bool operator !=(Vector2D v1, Vector2D v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Vector2D)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion

    }
}