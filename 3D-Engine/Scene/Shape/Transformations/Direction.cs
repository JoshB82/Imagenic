namespace _3D_Engine
{
    public static partial class Transform
    {
        /// <summary>
        /// Calculates the forward direction given the up direction and the right direction.
        /// </summary>
        /// <param name="direction_up">The up direction.</param>
        /// <param name="direction_right">The right direction.</param>
        /// <returns>The forward direction.</returns>
        public static Vector3D Calculate_Direction_Forward(Vector3D direction_up, Vector3D direction_right) => direction_right.Cross_Product(direction_up);
        /// <summary>
        /// Calculates the up direction given the right direction and the forward direction.
        /// </summary>
        /// <param name="direction_right">The right direction.</param>
        /// <param name="direction_forward">The forward direction.</param>
        /// <returns>The up direction.</returns>
        public static Vector3D Calculate_Direction_Up(Vector3D direction_right, Vector3D direction_forward) => direction_forward.Cross_Product(direction_right);
        /// <summary>
        /// Calculates the right direction given the forward direction and the up direction.
        /// </summary>
        /// <param name="direction_forward">The forward direction.</param>
        /// <param name="direction_up">The up direction.</param>
        /// <returns>The right direction.</returns>
        public static Vector3D Calculate_Direction_Right(Vector3D direction_forward, Vector3D direction_up) => direction_up.Cross_Product(direction_forward);
    }
}