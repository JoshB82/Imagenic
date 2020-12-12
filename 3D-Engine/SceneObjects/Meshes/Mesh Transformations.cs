namespace _3D_Engine
{
    public abstract partial class Mesh : SceneObject
    {
        #region Scaling

        /// <summary>
        /// Scales a <see cref="Mesh"/> in the x-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        public void Scale_X(float scale_factor) => Scaling = new Vector3D(Scaling.x * scale_factor, 0, 0);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in the y-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        public void Scale_Y(float scale_factor) => Scaling = new Vector3D(0, Scaling.y * scale_factor, 0);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in the z-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        public void Scale_Z(float scale_factor) => Scaling = new Vector3D(0, 0, Scaling.z * scale_factor);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions.
        /// </summary>
        /// <param name="scale_factor_x">Factor to scale by in the x-direction.</param>
        /// <param name="scale_factor_y">Factor to scale by in the y-direction.</param>
        /// <param name="scale_factor_z">Factor to scale by in the z-direction.</param>
        public void Scale(float scale_factor_x, float scale_factor_y, float scale_factor_z) => Scaling = new Vector3D(Scaling.x * scale_factor_x, Scaling.y * scale_factor_y, Scaling.z * scale_factor_z);
        /// <summary>
        /// Scales a <see cref="Mesh"/> in all directions by the same scale factor.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        public void Scale(float scale_factor) => Scaling = new Vector3D(Scaling.x * scale_factor, Scaling.y * scale_factor, Scaling.z * scale_factor);

        #endregion
    }
}