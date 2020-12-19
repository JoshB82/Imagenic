using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.Meshes
{
    public abstract partial class Mesh : SceneObject
    {
        #region Scaling

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleX(float scaleFactor) => Scaling = new Vector3D(Scaling.x * scaleFactor, 0, 0);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in the y-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleY(float scaleFactor) => Scaling = new Vector3D(0, Scaling.y * scaleFactor, 0);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in the z-direction.
        /// </summary>
        /// <param name="scaleFactor">Factor to scale by.</param>
        public void ScaleZ(float scaleFactor) => Scaling = new Vector3D(0, 0, Scaling.z * scaleFactor);
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

        #endregion
    }
}