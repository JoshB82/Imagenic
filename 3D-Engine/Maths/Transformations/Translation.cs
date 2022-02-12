/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines static methods for calculating matrices representing translations.
 */

using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Maths;

namespace _3D_Engine.Maths.Transformations
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in the x-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 TranslateX(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distance;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in the y-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 TranslateY(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m13 = distance;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in the z-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 TranslateZ(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m23 = distance;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in all directions.
        /// </summary>
        /// <param name="distanceX">Distance to move by in x-direction.</param>
        /// <param name="distanceY">Distance to move by in y-direction.</param>
        /// <param name="distanceZ">Distance to move by in z-direction.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Translate(float distanceX, float distanceY, float distanceZ)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distanceX;
            translation.m13 = distanceY;
            translation.m23 = distanceZ;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in all directions.
        /// </summary>
        /// <param name="displacement">Vector to translate by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Translate(Vector3D displacement)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = displacement.x;
            translation.m13 = displacement.y;
            translation.m23 = displacement.z;
            return translation;
        }
    }
}