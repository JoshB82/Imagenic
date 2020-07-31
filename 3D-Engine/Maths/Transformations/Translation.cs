namespace _3D_Engine
{
    public static partial class Transform
    {
        /// <summary>
        /// Creates a matrix for translation in the x-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation matrix.</returns>
        public static Matrix4x4 Translate_X(double distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity_Matrix();
            translation.Data[0][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in the y-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation matrix.</returns>
        public static Matrix4x4 Translate_Y(double distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity_Matrix();
            translation.Data[1][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in the z-direction.
        /// </summary>
        /// <param name="distance">Distance to move by.</param>
        /// <returns>A translation matrix.</returns>
        public static Matrix4x4 Translate_Z(double distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity_Matrix();
            translation.Data[2][3] = distance;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in all directions.
        /// </summary>
        /// <param name="distance_x">Distance to move by in x-direction.</param>
        /// <param name="distance_y">Distance to move by in y-direction.</param>
        /// <param name="distance_z">Distance to move by in z-direction.</param>
        /// <returns>A translation matrix.</returns>
        public static Matrix4x4 Translate(double distance_x, double distance_y, double distance_z)
        {
            Matrix4x4 translation = Matrix4x4.Identity_Matrix();
            translation.Data[0][3] = distance_x;
            translation.Data[1][3] = distance_y;
            translation.Data[2][3] = distance_z;
            return translation;
        }

        /// <summary>
        /// Creates a matrix for translation in all directions.
        /// </summary>
        /// <param name="distance">Vector to translate by.</param>
        /// <returns>A translation matrix.</returns>
        public static Matrix4x4 Translate(Vector3D distance)
        {
            Matrix4x4 translation = Matrix4x4.Identity_Matrix();
            translation.Data[0][3] = distance.X;
            translation.Data[1][3] = distance.Y;
            translation.Data[2][3] = distance.Z;
            return translation;
        }
    }
}