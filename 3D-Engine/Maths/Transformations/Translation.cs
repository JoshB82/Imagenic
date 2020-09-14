namespace _3D_Engine
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in the x-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Translate_X(float distance)
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
        public static Matrix4x4 Translate_Y(float distance)
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
        public static Matrix4x4 Translate_Z(float distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m23 = distance;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in all directions.
        /// </summary>
        /// <param name="distance_x">Distance to move by in x-direction.</param>
        /// <param name="distance_y">Distance to move by in y-direction.</param>
        /// <param name="distance_z">Distance to move by in z-direction.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Translate(float distance_x, float distance_y, float distance_z)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distance_x;
            translation.m13 = distance_y;
            translation.m23 = distance_z;
            return translation;
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> for translation in all directions.
        /// </summary>
        /// <param name="distance">Vector to translate by.</param>
        /// <returns>A translation <see cref="Matrix4x4"/>.</returns>
        public static Matrix4x4 Translate(Vector3D distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity;
            translation.m03 = distance.x;
            translation.m13 = distance.y;
            translation.m23 = distance.z;
            return translation;
        }
    }
}