using System.Diagnostics;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Rotations

        public void Set_Camera_Direction_1(Vector3D new_world_direction, Vector3D new_world_direction_up)
        {
            //if (new_world_direction * new_world_direction_up != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction = new_world_direction.Normalise(); new_world_direction_up = new_world_direction_up.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction.Cross_Product(new_world_direction_up);
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }
        public void Set_Camera_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            //if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction_up = new_world_direction_up.Normalise(); new_world_direction_right = new_world_direction_right.Normalise();
            World_Direction = new_world_direction_up.Cross_Product(new_world_direction_right);
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction_right;
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }
        public void Set_Camera_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction)
        {
            //if (new_world_direction_right * new_world_direction != 0) throw new Exception("Camera direction vectors are not orthogonal.");
            new_world_direction_right = new_world_direction_right.Normalise(); new_world_direction = new_world_direction.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_right.Cross_Product(new_world_direction);
            World_Direction_Right = new_world_direction_right;
            Calculate_Clipping_Planes();
            Output_Camera_Direction();
        }
        
        private void Output_Camera_Direction() =>
            Debug.WriteLine("Camera direction changed to:\n" +
                $"Forward: {World_Direction}\n" +
                $"Up: {World_Direction_Up}\n" +
                $"Right: {World_Direction_Right}"
            );

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
    }
}