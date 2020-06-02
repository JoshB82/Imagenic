using System;

namespace _3D_Engine
{
    public struct Quaternion
    {
        public double Q1 { get; set; }
        public double Q2 { get; set; }
        public double Q3 { get; set; }
        public double Q4 { get; set; }

        public Quaternion(double q1, double q2, double q3, double q4)
        {
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
            Q4 = q4;
        }

        public Quaternion(double[] data)
        {
            Q1 = data[0];
            Q2 = data[1];
            Q3 = data[2];
            Q4 = data[3];
        }

        public Quaternion(Vector4D v)
        {
            Q1 = v.X;
            Q2 = v.Y;
            Q3 = v.Z;
            Q4 = v.W;
        }

        public Quaternion(double scalar, Vector3D v)
        {
            Q1 = scalar;
            Q2 = v.X;
            Q3 = v.Y;
            Q4 = v.Z;
        }

        #region Quaternion Operations (Common)
        public double Magnitude() => Math.Sqrt(Math.Pow(Q1, 2) + Math.Pow(Q2, 2) + Math.Pow(Q3, 2) + Math.Pow(Q4, 2));

        public Quaternion Normalise() => this / Magnitude();

        public override string ToString() => $"({Q1}, {Q2}, {Q3}, {Q4})";
        #endregion

        #region Quaternion Operations (Operator Overloading)
        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 + q2.Q1, q1.Q2 + q2.Q2, q1.Q3 + q2.Q3, q1.Q4 + q2.Q4);
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 - q2.Q1, q1.Q2 - q2.Q2, q1.Q3 - q2.Q3, q1.Q4 - q2.Q4);
        public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 * q2.Q1 - q1.Q2 * q2.Q2 - q1.Q3 * q2.Q3 - q1.Q4 * q2.Q4, q1.Q1 * q2.Q2 - q1.Q2 * q2.Q1 - q1.Q3 * q2.Q4 - q1.Q4 * q2.Q3, q1.Q1 * q2.Q3 - q1.Q2 * q2.Q4 - q1.Q3 * q2.Q1 - q1.Q4 * q2.Q2, q1.Q1 * q2.Q4 - q1.Q2 * q2.Q3 - q1.Q3 * q2.Q2 - q1.Q4 * q2.Q1);
        public static Quaternion operator *(Quaternion q, double scalar) => new Quaternion(q.Q1 * scalar, q.Q2 * scalar, q.Q3 * scalar, q.Q4 * scalar);
        public static Quaternion operator /(Quaternion q, double scalar) => new Quaternion(q.Q1 / scalar, q.Q2 / scalar, q.Q3 / scalar, q.Q4 / scalar);
        #endregion
    }
}