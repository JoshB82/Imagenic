using System.Diagnostics;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Rotations

        public void Set_Camera_Direction_1(Vector3D new_world_direction_forward, Vector3D new_world_direction_up)
        {
            //if (new_world_direction * new_world_direction_up != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction_forward = new_world_direction_forward.Normalise(); new_world_direction_up = new_world_direction_up.Normalise();
            World_Direction_Forward = new_world_direction_forward;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = Transform.Calculate_Direction_Right(new_world_direction_forward, new_world_direction_up);
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }
        public void Set_Camera_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            //if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction_up = new_world_direction_up.Normalise(); new_world_direction_right = new_world_direction_right.Normalise();
            World_Direction_Forward = Transform.Calculate_Direction_Forward(new_world_direction_up, new_world_direction_right);
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction_right;
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }
        public void Set_Camera_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction_forward)
        {
            //if (new_world_direction_right * new_world_direction != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction_right = new_world_direction_right.Normalise(); new_world_direction_forward = new_world_direction_forward.Normalise();
            World_Direction_Forward = new_world_direction_forward;
            World_Direction_Up = Transform.Calculate_Direction_Up(new_world_direction_right, new_world_direction_forward);
            World_Direction_Right = new_world_direction_right;
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }

        private void Output_Camera_Direction()
        {
            if (Settings.Debug_Output_Verbosity == Verbosity.All || Settings.Camera_Debug_Output_Verbosity == Verbosity.All)
                Debug.WriteLine("<=========\n" +
                    GetType().Name + " direction changed to:\n" +
                    $"Forward: {World_Direction_Forward}\n" +
                    $"Up: {World_Direction_Up}\n" +
                    $"Right: {World_Direction_Right}\n" +
                    "=========>"
                );
        }

        #endregion

        #region Translations

        public void Translate_X(double distance)
        {
            World_Origin += new Vector3D(distance, 0, 0);
            Calculate_Clipping_Planes();
        }
        public void Translate_Y(double distance)
        {
            World_Origin += new Vector3D(0, distance, 0);
            Calculate_Clipping_Planes();
        }
        public void Translate_Z(double distance)
        {
            World_Origin += new Vector3D(0, 0, distance);
            Calculate_Clipping_Planes();
        }
        public void Translate(Vector3D distance)
        {
            World_Origin += distance;
            Calculate_Clipping_Planes();
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
            Set_Camera_Direction_1(new Vector3D(transformation_up * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_up * new Vector4D(World_Direction_Up)));
        }
        public void Rotate_Left(double angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(World_Direction_Up, -angle);
            Set_Camera_Direction_3(new Vector3D(transformation_left * new Vector4D(World_Direction_Right)), new Vector3D(transformation_left * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Right(double angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(World_Direction_Up, angle);
            Set_Camera_Direction_3(new Vector3D(transformation_right * new Vector4D(World_Direction_Right)), new Vector3D(transformation_right * new Vector4D(World_Direction_Forward)));
        }
        public void Rotate_Down(double angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(World_Direction_Right, angle);
            Set_Camera_Direction_1(new Vector3D(transformation_down * new Vector4D(World_Direction_Forward)), new Vector3D(transformation_down * new Vector4D(World_Direction_Up)));
        }
        public void Roll_Left(double angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(World_Direction_Forward, angle);
            Set_Camera_Direction_2(new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_left * new Vector4D(World_Direction_Right)));
        }
        public void Roll_Right(double angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(World_Direction_Forward, -angle);
            Set_Camera_Direction_2(new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Up)), new Vector3D(transformation_roll_right * new Vector4D(World_Direction_Right)));
        }

        #endregion
    }
}