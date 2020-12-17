/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for calculating matrices representing scaling.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Transformations
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the x-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale_X(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m00 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the y-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale_Y(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m11 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the z-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale_Z(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m22 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in all directions.
        /// </summary>
        /// <param name="scale_factor_x">Factor to scale by in the x-direction.</param>
        /// <param name="scale_factor_y">Factor to scale by in the y-direction.</param>
        /// <param name="scale_factor_z">Factor to scale by in the z-direction.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(float scale_factor_x, float scale_factor_y, float scale_factor_z)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m00 = scale_factor_x;
            scaling.m11 = scale_factor_y;
            scaling.m22 = scale_factor_z;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in all directions by the same scale factor.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(float scale_factor) => Scale(scale_factor, scale_factor, scale_factor);

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the x, y and z directions by the respective scale factors in the <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="scale_factor"><see cref="Vector3D"/> containing factors to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(Vector3D scale_factor) => Scale(scale_factor.x, scale_factor.y, scale_factor.z);
    }
}