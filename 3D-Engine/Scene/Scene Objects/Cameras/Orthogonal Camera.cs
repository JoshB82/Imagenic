using System;
using static System.IO.Directory; // ?

namespace _3D_Engine
{
    public class Orthogonal_Camera : Camera
    {
        #region Fields and Properties

        private double width, height, z_near, z_far;

        public override double Width
        {
            get => width;
            set
            {
                width = value;
                Camera_View_to_Screen.Data[0][0] = 2 / width;
            }
        }
        public override double Height
        {
            get => height;
            set
            {
                height = value;
                Camera_View_to_Screen.Data[1][1] = 2 / height;
            }
        }
        public override double Z_Near
        {
            get => z_near;
            set
            {
                z_near = value;
                Camera_View_to_Screen.Data[2][2] = 2 / (z_far - z_near);
                Camera_View_to_Screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }
        public override double Z_Far
        {
            get => z_far;
            set
            {
                z_far = value;
                Camera_View_to_Screen.Data[2][2] = 2 / (z_far - z_near);
                Camera_View_to_Screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }

        #endregion

        #region Constructors

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(origin, direction_forward, direction_up)
        {
            string icon_path = GetParent(GetCurrentDirectory()).FullName + "\\Meshes\\Default\\Orthogonal Camera.obj";
            Icon = new Custom(origin, direction_forward, direction_up, icon_path);

            Camera_View_to_Screen = Matrix4x4.Identity_Matrix();

            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;

            Calculate_Camera_View_Clipping_Planes();
        }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, direction_forward, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, pointed_at.World_Origin, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

        internal override void Calculate_Camera_View_Clipping_Planes()
        {
            double semi_width = Width / 2, semi_height = Height / 2;

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width, semi_height, Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width, -semi_height, Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_bottom_right_point, far_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_bottom_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_left_point, near_top_right_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_bottom_right_point, near_top_right_point, far_bottom_right_point); //make order look nice

            Camera_View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near z
                new Clipping_Plane(far_top_left_point, Vector3D.Unit_Negative_Z), // Far z
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, top_normal), // Top
                new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }
    }
}