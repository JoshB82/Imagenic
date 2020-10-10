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

using static System.MathF;

namespace _3D_Engine
{
    public static partial class Transform
    {
        #region Matrix rotations

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate_X(System.Single)']/*"/>
        public static Matrix4x4 Rotate_X(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = Sin(angle), cos_angle = Cos(angle);
            rotation.m11 = cos_angle;
            rotation.m12 = -sin_angle;
            rotation.m21 = sin_angle;
            rotation.m22 = cos_angle;
            return rotation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate_Y(System.Single)']/*"/>
        public static Matrix4x4 Rotate_Y(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = Sin(angle), cos_angle = Cos(angle);
            rotation.m00 = cos_angle;
            rotation.m02 = sin_angle;
            rotation.m20 = -sin_angle;
            rotation.m22 = cos_angle;
            return rotation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate_Z(System.Single)']/*"/>
        public static Matrix4x4 Rotate_Z(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = Sin(angle), cos_angle = Cos(angle);
            rotation.m00 = cos_angle;
            rotation.m01 = -sin_angle;
            rotation.m10 = sin_angle;
            rotation.m11 = cos_angle;
            return rotation;
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate(_3D_Engine.Vector3D,System.Single)']/*"/>
        public static Matrix4x4 Rotate(Vector3D axis, float angle)
        {
            if (angle == 0) return Matrix4x4.Identity;
            float sin_angle = Sin(angle), cos_angle = Cos(angle);
            return new Matrix4x4
                (
                    cos_angle + axis.x * axis.x * (1 - cos_angle),
                    axis.x * axis.y * (1 - cos_angle) - axis.z * sin_angle,
                    axis.x * axis.z * (1 - cos_angle) + axis.y * sin_angle,
                    0,
                    axis.y * axis.x * (1 - cos_angle) + axis.z * sin_angle,
                    cos_angle + axis.y * axis.y * (1 - cos_angle),
                    axis.y * axis.z * (1 - cos_angle) - axis.x * sin_angle,
                    0,
                    axis.z * axis.x * (1 - cos_angle) - axis.y * sin_angle,
                    axis.z * axis.y * (1 - cos_angle) + axis.x * sin_angle,
                    cos_angle + axis.z * axis.z * (1 - cos_angle),
                    0,
                    0,
                    0,
                    0,
                    1
                );
        }

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Rotate_Between_Vectors(_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Nullable{_3D_Engine.Vector3D})']/*"/>
        public static Matrix4x4 Rotate_Between_Vectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.Approx_Equals(v2, 1E-6f)) return Matrix4x4.Identity;
            axis ??= Vector3D.Unit_Y;
            Vector3D rotation_axis = (v1 == -v2) ? (Vector3D)axis : v1.Cross_Product(v2).Normalise();
            float angle = v1.Angle(v2);
            return Rotate(rotation_axis, angle);
        }

        #endregion

        #region Quaternion rotations

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_X(System.Single)']/*"/>
        public static Quaternion Quaternion_Rotate_X(float angle) => Quaternion_Rotate(Vector3D.Unit_X, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Y(System.Single)']/*"/>
        public static Quaternion Quaternion_Rotate_Y(float angle) => Quaternion_Rotate(Vector3D.Unit_Y, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Z(System.Single)']/*"/>
        public static Quaternion Quaternion_Rotate_Z(float angle) => Quaternion_Rotate(Vector3D.Unit_Z, angle);

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate(_3D_Engine.Vector3D,System.Single)']/*"/>
        public static Quaternion Quaternion_Rotate(Vector3D axis, float angle) => angle.Approx_Equals(0, 1E-6f) ? Quaternion.Identity : new Quaternion(Cos(angle / 2), axis.Normalise() * Sin(angle / 2)).Normalise();

        /// <include file="Help_8.xml" path="doc/members/member[@name='M:_3D_Engine.Transform.Quaternion_Rotate_Between_Vectors(_3D_Engine.Vector3D,_3D_Engine.Vector3D,System.Nullable{_3D_Engine.Vector3D})']/*"/>
        public static Quaternion Quaternion_Rotate_Between_Vectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.Approx_Equals(v2, 1E-6f)) return Quaternion.Identity;
            axis ??= Vector3D.Unit_Y;
            Vector3D rotation_axis = (v1 == -v2) ? (Vector3D)axis : v1.Cross_Product(v2).Normalise();
            float angle = v1.Angle(v2);
            return Quaternion_Rotate(rotation_axis, angle);
        }

        #endregion

        #region Quaternion to matrix conversion

        /// <summary>
        /// Creates the corresponding rotation <see cref="T:_3D_Engine.Matrix4x4" /> for the specified <see cref="T:_3D_Engine.Quaternion" />.
        /// </summary>
        /// <param name="q">The <see cref="T:_3D_Engine.Quaternion" /> to convert.</param>
        /// <returns>A rotation <see cref="T:_3D_Engine.Matrix4x4" />.</returns>
        /// <include file="Help_8.xml" path="doc/members/member[@name='']/*"/>cast!
        public static Matrix4x4 Quaternion_to_Matrix(Quaternion q) =>
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