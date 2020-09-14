using System;

namespace _3D_Engine
{
    /// <include file="Help_5.xml" path="doc/members/member[@name='T:_3D_Engine.Quaternion']/*"/>
    public struct Quaternion
    {
        #region Fields and Parameters

        /// <include file="Help_5.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q1']/*"/>
        public float q1;

        /// <include file="Help_5.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q2']/*"/>
        public float q2;

        /// <include file="Help_5.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q3']/*"/>
        public float q3;

        /// <include file="Help_5.xml" path="doc/members/member[@name='F:_3D_Engine.Quaternion.q4']/*"/>
        public float q4;

        #endregion

        #region Constructors

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float,System.Float,System.Float,System.Float)']/*"/>
        public Quaternion(float q1, float q2, float q3, float q4)
        {
            this.q1 = q1;
            this.q2 = q2;
            this.q3 = q3;
            this.q4 = q4;
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float[])']/*"/>
        public Quaternion(float[] elements)
        {
            q1 = elements[0];
            q2 = elements[1];
            q3 = elements[2];
            q4 = elements[3];
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(_3D_Engine.Vector4D)']/*"/>
        public Quaternion(Vector4D v)
        {
            q1 = v.x;
            q2 = v.y;
            q3 = v.z;
            q4 = v.w;
        }

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Float,_3D_Engine.Vector3D)']/*"/>
        public Quaternion(float scalar, Vector3D v)
        {
            q1 = scalar;
            q2 = v.x;
            q3 = v.y;
            q4 = v.z;
        }

        #endregion

        #region Common Quaternions

        public static Quaternion Zero = new Quaternion(0, 0, 0, 0);
        public static Quaternion Identity = new Quaternion(1, 0, 0, 0);

        #endregion

        #region Quaternion Operations (Common)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Magnitude']/*"/>
        public float Magnitude() => (float)Math.Sqrt(Squared_Magnitude());

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Squared_Magnitude']/*"/>
        public float Squared_Magnitude() => q1 * q1 + q2 * q2 + q3 * q3 + q4 * q4;

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Normalise']/*"/>
        public Quaternion Normalise() => (this == Quaternion.Zero) ? throw new ArgumentException("Cannot normalise a zeroed quaternion.") : this / Magnitude();

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.ToString']/*"/>
        public override string ToString() => $"({q1}, {q2}, {q3}, {q4})";

        #endregion

        #region Quaternion Operations (Operator Overloading)

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Addition(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new Quaternion(q1.q1 + q2.q1, q1.q2 + q2.q2, q1.q3 + q2.q3, q1.q4 + q2.q4);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Subtraction(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new Quaternion(q1.q1 - q2.q1, q1.q2 - q2.q2, q1.q3 - q2.q3, q1.q4 - q2.q4);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(q1.q1 * q2.q1 - q1.q2 * q2.q2 - q1.q3 * q2.q3 - q1.q4 * q2.q4, q1.q1 * q2.q2 - q1.q2 * q2.q1 - q1.q3 * q2.q4 - q1.q4 * q2.q3, q1.q1 * q2.q3 - q1.q2 * q2.q4 - q1.q3 * q2.q1 - q1.q4 * q2.q2, q1.q1 * q2.q4 - q1.q2 * q2.q3 - q1.q3 * q2.q2 - q1.q4 * q2.q1);

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,System.Float)']/*"/>
        public static Quaternion operator *(Quaternion q, float scalar) => new Quaternion(q.q1 * scalar, q.q2 * scalar, q.q3 * scalar, q.q4 * scalar);

        public static Quaternion operator *(float scalar, Quaternion q) => q * scalar;

        /// <include file="Help_5.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Division(_3D_Engine.Quaternion,System.Float)']/*"/>
        public static Quaternion operator /(Quaternion q, float scalar) => new Quaternion(q.q1 / scalar, q.q2 / scalar, q.q3 / scalar, q.q4 / scalar);

        #endregion

        #region Equality and Miscellaneous

        public static bool operator ==(Quaternion v1, Quaternion v2) => v1.q1 == v2.q1 && v1.q2 == v2.q2 && v1.q3 == v2.q3 && v1.q4 == v2.q4;

        public static bool operator !=(Quaternion v1, Quaternion v2) => !(v1 == v2);

        public override bool Equals(object obj) => this == (Quaternion)obj;

        public override int GetHashCode() => throw new NotImplementedException();

        #endregion
    }
}