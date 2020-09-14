using System;

namespace _3D_Engine
{
    public static partial class Transform
    {
        #region Matrix rotations

        /// <summary>
        /// Creates a matrix for rotation about the x-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation matrix.</returns>
        public static Matrix4x4 Rotate_X(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = (float)Math.Sin(angle), cos_angle = (float)Math.Cos(angle);
            rotation.m11 = cos_angle;
            rotation.m12 = -sin_angle;
            rotation.m21 = sin_angle;
            rotation.m22 = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a matrix for rotation about the y-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation matrix.</returns>
        public static Matrix4x4 Rotate_Y(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = (float)Math.Sin(angle), cos_angle = (float)Math.Cos(angle);
            rotation.m00 = cos_angle;
            rotation.m02 = sin_angle;
            rotation.m20 = -sin_angle;
            rotation.m22 = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a matrix for rotation about the z-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation matrix.</returns>
        public static Matrix4x4 Rotate_Z(float angle)
        {
            Matrix4x4 rotation = Matrix4x4.Identity;
            if (angle == 0) return rotation;
            float sin_angle = (float)Math.Sin(angle), cos_angle = (float)Math.Cos(angle);
            rotation.m00 = cos_angle;
            rotation.m01 = -sin_angle;
            rotation.m10 = sin_angle;
            rotation.m11 = cos_angle;
            return rotation;
        }

        /// <summary>
        /// Creates a matrix for rotation about any axis.
        /// </summary>
        /// <param name="axis">Axis that will be rotated around.</param>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation matrix.</returns>
        public static Matrix4x4 Rotate(Vector3D axis, float angle)
        {
            if (angle == 0) return Matrix4x4.Identity;
            float sin_angle = (float)Math.Sin(angle), cos_angle = (float)Math.Cos(angle);
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

        /// <summary>
        /// Creates a rotation matrix that rotates one vector onto another. A rotation axis must be supplied if vectors are antiparallel.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <param name="axis">Axis that will be rotated around if vectors are antiparallel.</param>
        /// <returns>A rotation matrix.</returns>
        public static Matrix4x4 Rotate_Between_Vectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1 == v2) return Matrix4x4.Identity;
            axis ??= Vector3D.Unit_Y;
            Vector3D rotation_axis = (v1 == -v2) ? (Vector3D)axis : v1.Cross_Product(v2).Normalise();
            float angle = v1.Angle(v2);
            return Rotate(rotation_axis, angle);
        }

        #endregion

        #region Quaternion rotations

        /// <summary>
        /// Creates a quaternion for rotation about the x-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation quaternion.</returns>
        public static Quaternion Quaternion_Rotate_X(float angle) => Quaternion_Rotate(Vector3D.Unit_X, angle);

        /// <summary>
        /// Creates a quaternion for rotation about the y-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation quaternion.</returns>
        public static Quaternion Quaternion_Rotate_Y(float angle) => Quaternion_Rotate(Vector3D.Unit_Y, angle);

        /// <summary>
        /// Creates a quaternion for rotation about the z-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation quaternion.</returns>
        public static Quaternion Quaternion_Rotate_Z(float angle) => Quaternion_Rotate(Vector3D.Unit_Z, angle);

        /// <summary>
        /// Create a quaternion that represents a rotation around any axis.
        /// </summary>
        /// <param name="axis">Axis that will be rotated around.</param>
        /// <param name="angle">Angle to rotate.</param>
        /// <returns>A rotation quaternion.</returns>
        public static Quaternion Quaternion_Rotate(Vector3D axis, float angle) => (angle == 0) ? Quaternion.Identity : new Quaternion((float)Math.Cos(angle / 2), axis.Normalise() * (float)Math.Sin(angle / 2)).Normalise();

        /// <summary>
        /// Creates a quaternion that rotates one vector onto another. A rotation axis must be supplied if vectors are antiparallel.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <param name="axis"></param>
        /// <returns>A rotation quaternion.</returns>
        public static Quaternion Quaternion_Rotate_Between_Vectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1 == v2) return Quaternion.Identity;
            axis ??= Vector3D.Unit_Y;
            Vector3D rotation_axis = (v1 == -v2) ? (Vector3D)axis : v1.Cross_Product(v2).Normalise();
            float angle = v1.Angle(v2);
            return Quaternion_Rotate(rotation_axis, angle);
        }

        #endregion

        #region Quaternion to matrix conversion

        /// <summary>
        /// Creates the corresponding rotation matrix for the specified quaternion.
        /// </summary>
        /// <param name="q">The quaternion to convert.</param>
        /// <returns>A rotation matrix.</returns>
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