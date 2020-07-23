using System;
using System.Diagnostics;

namespace _3D_Engine
{
    public abstract partial class Mesh
    {
        #region Rotations

        public void Set_Shape_Direction_1(Vector3D new_world_direction_forward, Vector3D new_world_direction_up)
        {
            if (new_world_direction_forward * new_world_direction_up != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_forward = new_world_direction_forward.Normalise(); new_world_direction_up = new_world_direction_up.Normalise();
            World_Direction_Forward = new_world_direction_forward;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = Transform.Calculate_Direction_Right(new_world_direction_forward, new_world_direction_up);
            Output_Mesh_Direction();
        }
        public void Set_Shape_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_up = new_world_direction_up.Normalise(); new_world_direction_right = new_world_direction_right.Normalise();
            World_Direction_Forward = Transform.Calculate_Direction_Forward(new_world_direction_up, new_world_direction_right);
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction_right;
            Output_Mesh_Direction();
        }
        public void Set_Shape_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction_forward)
        {
            if (new_world_direction_right * new_world_direction_forward != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_right = new_world_direction_right.Normalise(); new_world_direction_forward = new_world_direction_forward.Normalise();
            World_Direction_Forward = new_world_direction_forward;
            World_Direction_Up = Transform.Calculate_Direction_Up(new_world_direction_right, new_world_direction_forward);
            World_Direction_Right = new_world_direction_right;
            Output_Mesh_Direction();
        }

        private void Output_Mesh_Direction()
        {
            if (Settings.Debug_Output_Verbosity == Verbosity.All || Settings.Mesh_Debug_Output_Verbosity == Verbosity.All)
                Debug.WriteLine("<=========\n" +
                    GetType().Name +" direction changed to:\n" +
                    $"Forward: {World_Direction_Forward}\n" +
                    $"Up: {World_Direction_Up}\n" +
                    $"Right: {World_Direction_Right}\n" +
                    "=========>"
                );
        }

        #endregion

        #region Scaling

        public void Scale_X(double scale_factor) => Scaling = new Vector3D(Scaling.X * scale_factor, 0, 0);
        public void Scale_Y(double scale_factor) => Scaling = new Vector3D(0, Scaling.Y * scale_factor, 0);
        public void Scale_Z(double scale_factor) => Scaling = new Vector3D(0, 0, Scaling.Z * scale_factor);
        public void Scale(double scale_factor_x, double scale_factor_y, double scale_factor_z) => Scaling = new Vector3D(Scaling.X * scale_factor_x, Scaling.Y * scale_factor_y, Scaling.Z * scale_factor_z);
        public void Scale(double scale_factor) => Scaling = new Vector3D(Scaling.X * scale_factor, Scaling.Y * scale_factor, Scaling.Z * scale_factor);

        #endregion

        #region Translations

        public void Translate_X(double distance) => World_Origin += new Vector3D(distance, 0, 0);
        public void Translate_Y(double distance) => World_Origin += new Vector3D(0, distance, 0);
        public void Translate_Z(double distance) => World_Origin += new Vector3D(0, 0, distance);
        public void Translate(Vector3D distance) => World_Origin += distance;

        #endregion
    }
}