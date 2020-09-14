namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Common

        public void Pan_Forward(float distance) => Translate(World_Direction_Forward * distance);
        public void Pan_Left(float distance) => Translate(World_Direction_Right * -distance);
        public void Pan_Right(float distance) => Translate(World_Direction_Right * distance);
        public void Pan_Back(float distance) => Translate(World_Direction_Forward * -distance);
        public void Pan_Up(float distance) => Translate(World_Direction_Up * distance);
        public void Pan_Down(float distance) => Translate(World_Direction_Up * -distance);

        public void Rotate_Up(float angle)
        {
            Matrix4x4 transformation_up = Transform.Rotate(World_Direction_Right, -angle);
            Set_Direction_1(new Vector3D(transformation_up * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_up * new Vector4D(World_Direction_Up)));
        }
        public void Rotate_Left(float angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(World_Direction_Up, -angle);
            Set_Direction_3(new Vector3D(transformation_left * new Vector4D(World_Direction_Right)), new Vector3D(transformation_left * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Right(float angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(World_Direction_Up, angle);
            Set_Direction_3(new Vector3D(transformation_right * new Vector4D(World_Direction_Right)), new Vector3D(transformation_right * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Down(float angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(World_Direction_Right, angle);
            Set_Direction_1(new Vector3D(transformation_down * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_down * new Vector4D(World_Direction_Up)));
        }
        public void Roll_Left(float angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(World_Direction_Forward, angle);
            Set_Direction_2(new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Right)));
        }
        public void Roll_Right(float angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(World_Direction_Forward, -angle);
            Set_Direction_2(new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Right)));
        }

        #endregion
    }
}