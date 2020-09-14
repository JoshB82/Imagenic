using System;

namespace _3D_Engine
{
    /// <include file="Help_5.xml" path="doc/members/member[@name='T:_3D_Engine.Quaternion']/*"/>
    public struct Quaternion
    {
        #region Fields and Parameters

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q1']/*"/>
        public float Q1;

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q2']/*"/>
        public float Q2;

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q3']/*"/>
        public float Q3;

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q4']/*"/>
        public float Q4;

        #endregion

        #region Constructors

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float,System.Float,System.Float,System.Float)']/*"/>
        public Quaternion(float q1, float q2, float q3, float q4)
        {
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
            Q4 = q4;
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float[])']/*"/>
        public Quaternion(float[] elements)
        {
            Q1 = elements[0];
            Q2 = elements[1];
            Q3 = elements[2];
            Q4 = elements[3];
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(_3D_Engine.Vector4D)']/*"/>
        public Quaternion(Vector4D v)
        {
            Q1 = v.X;
            Q2 = v.Y;
            Q3 = v.Z;
            Q4 = v.W;
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float,_3D_Engine.Vector3D)']/*"/>
        public Quaternion(float scalar, Vector3D v)
        {
            Q1 = scalar;
            Q2 = v.X;
            Q3 = v.Y;
            Q4 = v.Z;
        }

        #endregion

        #region Common Quaternions

        public static Quaternion Zero { get; } = new Quaternion(0, 0, 0, 0);
        public static Quaternion Identity { get; } = new Quaternion(1, 0, 0, 0);

        #endregion

        #region Quaternion Operations (Common)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Magnitude']/*"/>
        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Squared_Magnitude']/*"/>
        public float Squared_Magnitude() => Q1 * Q1 + Q2 * Q2 + Q3 * Q3 + Q4 * Q4;

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Normalise']/*"/>
        public Quaternion Normalise() => (this == Quaternion.Zero) ? throw new ArgumentException("Cannot normalise a zeroed quaternion.") : this / Magnitude();

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.ToString']/*"/>
        public override string ToString() => $"({Q1}, {Q2}, {Q3}, {Q4})";

        #endregion

        #region Quaternion Operations (Operator Overloading)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Addition(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 + q2.Q1, q1.Q2 + q2.Q2, q1.Q3 + q2.Q3, q1.Q4 + q2.Q4);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Subtraction(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 - q2.Q1, q1.Q2 - q2.Q2, q1.Q3 - q2.Q3, q1.Q4 - q2.Q4);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 * q2.Q1 - q1.Q2 * q2.Q2 - q1.Q3 * q2.Q3 - q1.Q4 * q2.Q4, q1.Q1 * q2.Q2 - q1.Q2 * q2.Q1 - q1.Q3 * q2.Q4 - q1.Q4 * q2.Q3, q1.Q1 * q2.Q3 - q1.Q2 * q2.Q4 - q1.Q3 * q2.Q1 - q1.Q4 * q2.Q2, q1.Q1 * q2.Q4 - q1.Q2 * q2.Q3 - q1.Q3 * q2.Q2 - q1.Q4 * q2.Q1);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,System.Float)']/*"/>
        public static Quaternion operator *(Quaternion q, float scalar) => new Quaternion(q.Q1 * scalar, q.Q2 * scalar, q.Q3 * scalar, q.Q4 * scalar);

        public static Quaternion operator *(float scalar, Quaternion q) => q * scalar;

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Division(_3D_Engine.Quaternion,System.Float)']/*"/>
        public static Quaternion operator /(Quaternion q, float scalar) => new Quaternion(q.Q1 / scalar, q.Q2 / scalar, q.Q3 / scalar, q.Q4 / scalar);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Quaternion v1, Quaternion v2) => v1.Q1 == v2.Q1 && v1.Q2 == v2.Q2 && v1.Q3 == v2.Q3 && v1.Q4 == v2.Q4;

        public static bool operator !=(Quaternion v1, Quaternion v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Quaternion)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}