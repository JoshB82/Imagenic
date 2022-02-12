/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines static methods for calculating matrices and quaternions representing rotations.
 */

using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Maths;
using static System.MathF;

namespace _3D_Engine.Maths.Transformations
{
    public static partial class Transform
    {
        #region Matrix Rotations

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
            return new
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

        /// <summary>
        /// Creates a rotation <see cref="Matrix4x4"/> that rotates one <see cref="Vector3D" /> onto another. A rotation axis must be supplied if <see cref="Vector3D" > Vector3Ds </ see > are antiparallel.
        /// </summary>
        /// <param name="v1">The first <see cref="Vector3D"/>.</param>
        /// <param name="v2">The second <see cref="Vector3D"/>.</param>
        /// <param name="axis">Axis that will be rotated around if <see cref="Vector3D">Vector3Ds</see> are antiparallel.</param>
        /// <returns>A rotation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 RotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.ApproxEquals(v2, 1E-6f)) return Matrix4x4.Identity;
            axis ??= Vector3D.UnitY;
            Vector3D rotationAxis = v1.ApproxEquals(-v2, 1E-6F) ? (Vector3D)axis : v1.CrossProduct(v2).Normalise();
            float angle = v1.Angle(v2);
            return Rotate(rotationAxis, angle);
        }

        #endregion

        #region Quaternion Rotations

        /// <summary>
        /// Creates a <see cref="Quaternion"/> for rotation about the x-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate by.</param>
        /// <returns>A rotation <see cref="Quaternion"/>.</returns>
        public static Quaternion QuaternionRotateX(float angle) => QuaternionRotate(Vector3D.UnitX, angle);

        /// <summary>
        /// Creates a<see cref="Quaternion"/> for rotation about the y-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate by.</param>
        /// <returns>A rotation <see cref="Quaternion"/>.</returns>
        public static Quaternion QuaternionRotateY(float angle) => QuaternionRotate(Vector3D.UnitY, angle);

        /// <summary>
        /// Creates a <see cref="Quaternion"/> for rotation about the z-axis.
        /// </summary>
        /// <param name="angle">Angle to rotate by.</param>
        /// <returns>A rotation <see cref="Quaternion"/>.</returns>
        public static Quaternion QuaternionRotateZ(float angle) => QuaternionRotate(Vector3D.UnitZ, angle);

        /// <summary>
        /// Creates a <see cref= "Quaternion"/> that represents a rotation around any axis.
        /// </summary>
        /// <param name="axis">Axis that will be rotated around.</param>
        /// <param name="angle">Angle to rotate by.</param>
        /// <returns>A rotation <see cref="Quaternion"/>.</returns>
        public static Quaternion QuaternionRotate(Vector3D axis, float angle) => angle.ApproxEquals(0, 1E-6f) ? Quaternion.Identity : new Quaternion(Cos(angle / 2), axis.Normalise() * Sin(angle / 2)).Normalise();

        /// <summary>
        /// Creates a <see cref="Quaternion"/> that rotates one <see cref="Vector3D"/> onto another. A rotation axis must be supplied if <see cref="Vector3D"> Vector3Ds </see> are antiparallel.
        /// </summary>
        /// <param name="v1">The first <see cref="Vector3D"/>.</param>
        /// <param name="v2">The second <see cref="Vector3D"/>.</param>
        /// <param name="axis">Axis that will be rotated around if <see cref="Vector3D">Vector3Ds</see> are antiparallel.</param>
        /// <returns>A rotation <see cref="Quaternion"/>.</returns>
        public static Quaternion QuaternionRotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            if (v1.ApproxEquals(v2, 1E-6f)) return Quaternion.Identity;
            axis ??= Vector3D.UnitY;
            Vector3D rotationAxis = v1.ApproxEquals(-v2, 1E-6F) ? (Vector3D)axis : v1.CrossProduct(v2).Normalise();
            float angle = v1.Angle(v2);
            return QuaternionRotate(rotationAxis, angle);
        }

        #endregion

        #region Quaternion to Matrix Conversion

        /// <summary>
        /// Creates the corresponding rotation <see cref="T:_3D_Engine.Matrix4x4" /> for the specified <see cref="T:_3D_Engine.Quaternion" />.
        /// </summary>
        /// <param name="q">The <see cref="T:_3D_Engine.Quaternion" /> to convert.</param>
        /// <returns>A rotation <see cref="T:_3D_Engine.Matrix4x4" />.</returns>
        /// <include file="Help_8.xml" path="doc/members/member[@name='']/*"/>cast!
        public static Matrix4x4 QuaternionToMatrix(Quaternion q) =>
            // RIGHT HANDED ROTATION
            // (ANTI CLOCKWISE WHEN LOOKING AT ORIGIN FROM ARROW TIP TO BEGINNING)
            new
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