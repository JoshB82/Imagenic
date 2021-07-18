/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines methods for scaling Meshes.
 */

using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes
{
    public abstract partial class Mesh : SceneObject
    {
        #region Scaling

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleX(float scaleFactor) => Scaling = new Vector3D(Scaling.x * scaleFactor, Scaling.y, Scaling.z);

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the y-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleY(float scaleFactor) => Scaling = new Vector3D(Scaling.x, Scaling.y * scaleFactor, Scaling.z);

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the z-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleZ(float scaleFactor) => Scaling = new Vector3D(Scaling.x, Scaling.y, Scaling.z * scaleFactor);

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <param name="scaleFactorX">Factor to scale by in the x-direction.</param>
        /// <param name="scaleFactorY">Factor to scale by in the y-direction.</param>
        /// <param name="scaleFactorZ">Factor to scale by in the z-direction.</param>
        public void Scale(float scaleFactorX, float scaleFactorY, float scaleFactorZ) => Scaling = new Vector3D(Scaling.x * scaleFactorX, Scaling.y * scaleFactorY, Scaling.z * scaleFactorZ);

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions by the same scale factor.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void Scale(float scaleFactor) => Scaling = new Vector3D(Scaling.x * scaleFactor, Scaling.y * scaleFactor, Scaling.z * scaleFactor);

        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <param name="scaleFactor">Vector representing factors to scale by.</param>
        public void Scale(Vector3D scaleFactor) => Scaling = scaleFactor;

        #endregion
    }
}