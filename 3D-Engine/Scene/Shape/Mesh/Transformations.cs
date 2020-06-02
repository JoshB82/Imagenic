using System;

namespace _3D_Graphics
{
    public abstract partial class Mesh
    {
        #region Rotations

        public void Set_Shape_Direction_1(Vector3D new_world_direction, Vector3D new_world_direction_up)
        {
            if (new_world_direction * new_world_direction_up != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction = new_world_direction.Normalise(); new_world_direction_up = new_world_direction_up.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction.Cross_Product(new_world_direction_up);
        }
        public void Set_Shape_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_up = new_world_direction_up.Normalise(); new_world_direction_right = new_world_direction_right.Normalise();
            World_Direction = new_world_direction_up.Cross_Product(new_world_direction_right); ;
            World_Direction_Up = new_world_direction_up;
            World_Direction_Right = new_world_direction_right;
        }
        public void Set_Shape_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction)
        {
            if (new_world_direction_right * new_world_direction != 0) throw new Exception("Shape direction vectors are not orthogonal.");
            new_world_direction_right = new_world_direction_right.Normalise(); new_world_direction = new_world_direction.Normalise();
            World_Direction = new_world_direction;
            World_Direction_Up = new_world_direction_right.Cross_Product(new_world_direction);
            World_Direction_Right = new_world_direction_right;
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