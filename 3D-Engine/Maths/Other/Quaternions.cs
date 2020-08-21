using System;

namespace _3D_Engine
{
    /// <include file="Help_3.xml" path="doc/members/member[@name='T:_3D_Engine.Quaternion']/*"/>
    public struct Quaternion
    {
        #region Fields and Parameters

        /// <include file="Help_3.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q1']/*"/>
        public double Q1 { get; set; }
        /// <include file="Help_3.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q2']/*"/>
        public double Q2 { get; set; }
        /// <include file="Help_3.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q3']/*"/>
        public double Q3 { get; set; }
        /// <include file="Help_3.xml" path="doc/members/member[@name='P:_3D_Engine.Quaternion.Q4']/*"/>
        public double Q4 { get; set; }

        #endregion

        #region Constructors

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Double,System.Double,System.Double,System.Double)']/*"/>
        public Quaternion(double q1, double q2, double q3, double q4)
        {
            Q1 = q1;
            Q2 = q2;
            Q3 = q3;
            Q4 = q4;
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Double[])']/*"/>
        public Quaternion(double[] data)
        {
            Q1 = data[0];
            Q2 = data[1];
            Q3 = data[2];
            Q4 = data[3];
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(_3D_Engine.Vector4D)']/*"/>
        public Quaternion(Vector4D v)
        {
            Q1 = v.X;
            Q2 = v.Y;
            Q3 = v.Z;
            Q4 = v.W;
        }

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.#ctor(System.Double,_3D_Engine.Vector3D)']/*"/>
        public Quaternion(double scalar, Vector3D v)
        {
            Q1 = scalar;
            Q2 = v.X;
            Q3 = v.Y;
            Q4 = v.Z;
        }

        #endregion

        #region Quaternion Operations (Common)

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Magnitude']/*"/>
        public double Magnitude() => Math.Sqrt(Squared_Magnitude());

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Squared_Magnitude']/*"/>
        public double Squared_Magnitude() => Math.Pow(Q1, 2) + Math.Pow(Q2, 2) + Math.Pow(Q3, 2) + Math.Pow(Q4, 2);

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.Normalise']/*"/>
        public Quaternion Normalise() => this / Magnitude();

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.ToString']/*"/>
        public override string ToString() => $"({Q1}, {Q2}, {Q3}, {Q4})";

        #endregion

        #region Quaternion Operations (Operator Overloading)

        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Addition(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator +(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 + q2.Q1, q1.Q2 + q2.Q2, q1.Q3 + q2.Q3, q1.Q4 + q2.Q4);
        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Subtraction(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator -(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 - q2.Q1, q1.Q2 - q2.Q2, q1.Q3 - q2.Q3, q1.Q4 - q2.Q4);
        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,_3D_Engine.Quaternion)']/*"/>
        public static Quaternion operator *(Quaternion q1, Quaternion q2) => new Quaternion(q1.Q1 * q2.Q1 - q1.Q2 * q2.Q2 - q1.Q3 * q2.Q3 - q1.Q4 * q2.Q4, q1.Q1 * q2.Q2 - q1.Q2 * q2.Q1 - q1.Q3 * q2.Q4 - q1.Q4 * q2.Q3, q1.Q1 * q2.Q3 - q1.Q2 * q2.Q4 - q1.Q3 * q2.Q1 - q1.Q4 * q2.Q2, q1.Q1 * q2.Q4 - q1.Q2 * q2.Q3 - q1.Q3 * q2.Q2 - q1.Q4 * q2.Q1);
        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Multiply(_3D_Engine.Quaternion,System.Double)']/*"/>
        public static Quaternion operator *(Quaternion q, double scalar) => new Quaternion(q.Q1 * scalar, q.Q2 * scalar, q.Q3 * scalar, q.Q4 * scalar);
        /// <include file="Help_3.xml" path="doc/members/member[@name='M:_3D_Engine.Quaternion.op_Division(_3D_Engine.Quaternion,System.Double)']/*"/>
        public static Quaternion operator /(Quaternion q, double scalar) => new Quaternion(q.Q1 / scalar, q.Q2 / scalar, q.Q3 / scalar, q.Q4 / scalar);

        #endregion
    }
}