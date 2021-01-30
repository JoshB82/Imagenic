/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for calculating matrices and quaternions representing rotations.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

using static System.MathF;

namespace _3D_Engine.Transformations
{
    public static partial class Transform
    {
        #region Matrix rotations

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for rotation about the x-axis.
        /// </summary>
        /// <param name="angle">The angle to rotate by.</param>
        /// <returns>A rotation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 RotateX(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sinAngle = Sin(angle), cosAngle = Cos(angle);
            rotation.m11 = cosAngle;
            rotation.m12 = -sinAngle;
            rotation.m21 = sinAngle;
            rotation.m22 = cosAngle;
            return rotation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for rotation about the y-axis.
        /// </summary>
        /// <param name="angle">The angle to rotate by.</param>
        /// <returns>A rotation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 RotateY(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sinAngle = Sin(angle), cosAngle = Cos(angle);
            rotation.m00 = cosAngle;
            rotation.m02 = sinAngle;
            rotation.m20 = -sinAngle;
            rotation.m22 = cosAngle;
            return rotation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for rotation about the z-axis.
        /// </summary>
        /// <param name="angle">The angle to rotate by.</param>
        /// <returns>A rotation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 RotateZ(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sinAngle = Sin(angle), cosAngle = Cos(angle);
            rotation.m00 = cosAngle;
            rotation.m01 = -sinAngle;
            rotation.m10 = sinAngle;
            rotation.m11 = cosAngle;
            return rotation;
        }

        /// <summary>
        /// Creates a<see cref= "Matrix4x4" /> for rotation about any axis.
        /// </summary>
        /// <param name="axis">Axis that will be rotated around.</param>
        /// <param name="angle">The angle to rotate by.</param>
        /// <returns>A rotation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Rotate(Vector3D axis, float angle)
        {
            if (angle == 0) return Matrix4x4.Identity;
            float sinAngle = Sin(angle), cosAngle = Cos(angle);
            return new Matrix4x4
                (
                    cosAngle + axis.x * axis.x * (1 - cosAngle),
                    axis.x * axis.y * (1 - cosAngle) - axis.z * sinAngle,
                    axis.x * axis.z * (1 - cosAngle) + axis.y * sinAngle,
                    0,
                    axis.y * axis.x * (1 - cosAngle) + axis.z * sinAngle,
                    cosAngle + axis.y * axis.y * (1 - cosAngle),
                    axis.y * axis.z * (1 - cosAngle) - axis.x * sinAngle,
                    0,
                    axis.z * axis.x * (1 - cosAngle) - axis.y * sinAngle,
                    axis.z * axis.y * (1 - cosAngle) + axis.x * sinAngle,
                    cosAngle + axis.z * axis.z * (1 - cosAngle),
                    0,
                    0,
                    0,
                    0,
                    1
                );
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate_Between_Vectors(_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Nullable{_3D_Engine.Vector3D})']/*"/>
        public static Matrix4x4 RotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.ApproxEquals(v2, 1E-6f)) return Matrix4x4.Identity;
            axis ??= Vector3D.UnitY;
            Vector3D rotationAxis = v1.ApproxEquals(-v2, 1E-6F) ? (Vector3D)axis : v1.CrossProduct(v2).Normalise();
            float angle = v1.Angle(v2);
            return Rotate(rotationAxis, angle);
        }

        #endregion

        #region Quaternion rotations

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_X(System.Single)']/*"/>
        public static Quaternion QuaternionRotateX(float angle) => QuaternionRotate(Vector3D.UnitX, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Y(System.Single)']/*"/>
        public static Quaternion QuaternionRotateY(float angle) => QuaternionRotate(Vector3D.UnitY, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Z(System.Single)']/*"/>
        public static Quaternion QuaternionRotateZ(float angle) => QuaternionRotate(Vector3D.UnitZ, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate(_3D_Engine.Vector3D,System.Single)']/*"/>
        public static Quaternion QuaternionRotate(Vector3D axis, float angle) => angle.ApproxEquals(0, 1E-6f) ? Quaternion.Identity : new Quaternion(Cos(angle / 2), axis.Normalise() * Sin(angle / 2)).Normalise();

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Between_Vectors(_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Nullable{_3D_Engine.Vector3D})']/*"/>
        public static Quaternion QuaternionRotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.ApproxEquals(v2, 1E-6f)) return Quaternion.Identity;
            axis ??= Vector3D.UnitY;
            Vector3D rotationAxis = v1.ApproxEquals(-v2, 1E-6F) ? (Vector3D)axis : v1.CrossProduct(v2).Normalise();
            float angle = v1.Angle(v2);
            return QuaternionRotate(rotationAxis, angle);
        }

        #endregion

        #region Quaternion to matrix conversion

        /// <summary>
        /// Creates the corresponding rotation <see cref="T:_3D_Engine.Matrix4x4" /> for the specified <see cref="T:_3D_Engine.Quaternion" />.
        /// </summary>
        /// <param name="q">The <see cref="T:_3D_Engine.Quaternion" /> to convert.</param>
        /// <returns>A rotation <see cref="T:_3D_Engine.Matrix4x4" />.</returns>
        /// <include file="Help_8.xml" path="doc/members/member[@name='']/*"/>cast!
        public static Matrix4x4 QuaternionToMatrix(Quaternion q) =>
            // RIGHT HANDED ROTATION
            // (ANTI CLOCKWISE WHEN LOOKING AT ORIGIN FROM ARROW TIP TO BEGINNING)
            new Matrix4x4
                (
                    1 - 2 * (q.q3 * q.q3 + q.q4 * q.q4),
                    2 * (q.q2 * q.q3 - q.q4 * q.q1),
                    2 * (q.q2 * q.q4 + q.q3 * q.q1),
                    0,
                    2 * (q.q2 * q.q3 + q.q4 * q.q1),
                    1 - 2 * (q.q2 * q.q2 + q.q4 * q.q4),
                    2 * (q.q3 * q.q4 - q.q2 * q.q1),
                    0,
                    2 * (q.q2 * q.q4 - q.q3 * q.q1),
                    2 * (q.q3 * q.q4 + q.q2 * q.q1),
                    1 - 2 * (q.q2 * q.q2 + q.q3 * q.q3),
                    0,
                    0,
                    0,
                    0,
                    1
                );

        #endregion
    }
}