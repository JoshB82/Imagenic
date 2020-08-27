namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Rotations

        public override void Set_Direction_1(Vector3D new_world_direction_forward, Vector3D new_world_direction_up)
        {
            base.Set_Direction_1(new_world_direction_forward, new_world_direction_up);
            Calculate_Camera_View_Clipping_Planes();
        }
        public override void Set_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            base.Set_Direction_2(new_world_direction_up, new_world_direction_right);
            Calculate_Camera_View_Clipping_Planes();
        }
        public override void Set_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction_forward)
        {
            base.Set_Direction_3(new_world_direction_right, new_world_direction_forward);
            Calculate_Camera_View_Clipping_Planes();
        }

        #endregion

        #region Translations

        public override void Translate_X(double distance)
        {
            base.Translate_X(distance);
            Calculate_Camera_View_Clipping_Planes();
        }
        public override void Translate_Y(double distance)
        {
            base.Translate_Y(distance);
            Calculate_Camera_View_Clipping_Planes();
        }
        public override void Translate_Z(double distance)
        {
            base.Translate_Z(distance);
            Calculate_Camera_View_Clipping_Planes();
        }
        public override void Translate(Vector3D displacement)
        {
            base.Translate(displacement);
            Calculate_Camera_View_Clipping_Planes();
        }

        #endregion

        #region Common

        public void Pan_Forward(double distance) => Translate(World_Direction_Forward * distance);
        public void Pan_Left(double distance) => Translate(World_Direction_Right * -distance);
        public void Pan_Right(double distance) => Translate(World_Direction_Right * distance);
        public void Pan_Back(double distance) => Translate(World_Direction_Forward * -distance);
        public void Pan_Up(double distance) => Translate(World_Direction_Up * distance);
        public void Pan_Down(double distance) => Translate(World_Direction_Up * -distance);

        public void Rotate_Up(double angle)
        {
            Matrix4x4 transformation_up = Transform.Rotate(World_Direction_Right, -angle);
            Set_Direction_1(new Vector3D(transformation_up * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_up * new Vector4D(World_Direction_Up)));
        }
        public void Rotate_Left(double angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(World_Direction_Up, -angle);
            Set_Direction_3(new Vector3D(transformation_left * new Vector4D(World_Direction_Right)), new Vector3D(transformation_left * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Right(double angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(World_Direction_Up, angle);
            Set_Direction_3(new Vector3D(transformation_right * new Vector4D(World_Direction_Right)), new Vector3D(transformation_right * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Down(double angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(World_Direction_Right, angle);
            Set_Direction_1(new Vector3D(transformation_down * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_down * new Vector4D(World_Direction_Up)));
        }
        public void Roll_Left(double angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(World_Direction_Forward, angle);
            Set_Direction_2(new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Right)));
        }
        public void Roll_Right(double angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(World_Direction_Forward, -angle);
            Set_Direction_2(new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Right)));
        }

        #endregion
    }
}