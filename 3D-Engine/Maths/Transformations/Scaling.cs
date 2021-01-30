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
        /// <param name="scaleFactor">The factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 ScaleX(float scaleFactor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m00 = scaleFactor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the y-direction.
        /// </summary>
        /// <param name="scaleFactor">The factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 ScaleY(float scaleFactor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m11 = scaleFactor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the z-direction.
        /// </summary>
        /// <param name="scaleFactor">The factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 ScaleZ(float scaleFactor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m22 = scaleFactor;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in all directions.
        /// </summary>
        /// <param name="scaleFactorX">The factor to scale by in the x-direction.</param>
        /// <param name="scaleFactorY">The factor to scale by in the y-direction.</param>
        /// <param name="scaleFactorZ">The factor to scale by in the z-direction.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(float scaleFactorX, float scaleFactorY, float scaleFactorZ)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.m00 = scaleFactorX;
            scaling.m11 = scaleFactorY;
            scaling.m22 = scaleFactorZ;
            return scaling;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in all directions by the same scale factor.
        /// </summary>
        /// <param name="scaleFactor">The factor to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(float scaleFactor) => Scale(scaleFactor, scaleFactor, scaleFactor);

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for scaling in the x, y and z directions by the respective scale factors in the <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="scaleFactor">The <see cref="Vector3D"/> containing factors to scale by.</param>
        /// <returns>A scaling <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Scale(Vector3D scaleFactor) => Scale(scaleFactor.x, scaleFactor.y, scaleFactor.z);
    }
}