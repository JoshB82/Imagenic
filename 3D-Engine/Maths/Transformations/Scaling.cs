namespace _3D_Engine
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a matrix for scaling in the x-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>Scaling matrix.</returns>
        public static Matrix4x4 Scale_X(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.M00 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a matrix for scaling in the y-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>Scaling matrix.</returns>
        public static Matrix4x4 Scale_Y(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.M11 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a matrix for scaling in the z-direction.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>Scaling matrix.</returns>
        public static Matrix4x4 Scale_Z(float scale_factor)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.M22 = scale_factor;
            return scaling;
        }

        /// <summary>
        /// Creates a matrix for scaling in all directions.
        /// </summary>
        /// <param name="scale_factor_x">Factor to scale by in x-direction.</param>
        /// <param name="scale_factor_y">Factor to scale by in y-direction.</param>
        /// <param name="scale_factor_z">Factor to scale by in z-direction.</param>
        /// <returns>Scaling matrix.</returns>
        public static Matrix4x4 Scale(float scale_factor_x, float scale_factor_y, float scale_factor_z)
        {
            Matrix4x4 scaling = Matrix4x4.Identity;
            scaling.M00 = scale_factor_x;
            scaling.M11 = scale_factor_y;
            scaling.M22 = scale_factor_z;
            return scaling;
        }

        /// <summary>
        /// Creates a matrix for scaling in all directions by the same scale factor.
        /// </summary>
        /// <param name="scale_factor">Factor to scale by.</param>
        /// <returns>Scaling matrix.</returns>
        public static Matrix4x4 Scale(float scale_factor) => Scale(scale_factor, scale_factor, scale_factor);
    }
}