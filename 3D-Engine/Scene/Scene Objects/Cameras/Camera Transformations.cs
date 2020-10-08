namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Common

        /// <summary>
        /// Pans the camera in the forward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Forward(float distance) => Translate(World_Direction_Forward * distance);
        /// <summary>
        /// Pans the camera in the left direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Left(float distance) => Translate(World_Direction_Right * -distance);
        /// <summary>
        /// Pans the camera in the right direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Right(float distance) => Translate(World_Direction_Right * distance);
        /// <summary>
        /// Pans the camera in the backward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Backward(float distance) => Translate(World_Direction_Forward * -distance);
        /// <summary>
        /// Pans the camera in the up direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Up(float distance) => Translate(World_Direction_Up * distance);
        /// <summary>
        /// Pans the camera in the down direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Down(float distance) => Translate(World_Direction_Up * -distance);

        public void Rotate_Up(float angle)
        {
            Matrix4x4 transformation_up = Transform.Rotate(World_Direction_Right, -angle);
            Set_Direction_1((Vector3D)(transformation_up * World_Direction_Forward), (Vector3D)(transformation_up * World_Direction_Up));
        }
        public void Rotate_Left(float angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(World_Direction_Up, -angle);
            Set_Direction_3((Vector3D)(transformation_left * World_Direction_Right), (Vector3D)(transformation_left * World_Direction_Forward));
        }
        public void Rotate_Right(float angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(World_Direction_Up, angle);
            Set_Direction_3((Vector3D)(transformation_right * World_Direction_Right), (Vector3D)(transformation_right * World_Direction_Forward));
        }
        public void Rotate_Down(float angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(World_Direction_Right, angle);
            Set_Direction_1((Vector3D)(transformation_down * World_Direction_Forward), (Vector3D)(transformation_down * World_Direction_Up));
        }
        public void Roll_Left(float angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(World_Direction_Forward, angle);
            Set_Direction_2((Vector3D)(transformation_roll_left * World_Direction_Up), (Vector3D)(transformation_roll_left * World_Direction_Right));
        }
        public void Roll_Right(float angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(World_Direction_Forward, -angle);
            Set_Direction_2((Vector3D)(transformation_roll_right * World_Direction_Up), (Vector3D)(transformation_roll_right * World_Direction_Right));
        }

        #endregion
    }
}