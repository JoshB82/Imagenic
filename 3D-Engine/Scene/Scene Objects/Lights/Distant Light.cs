using System.Drawing;

namespace _3D_Engine
{
    public sealed class Distant_Light : Light
    {
        #region Fields and Properties

        private int shadow_map_width, shadow_map_height;
        private double shadow_map_z_near, shadow_map_z_far;

        public override int Shadow_Map_Width
        {
            get => shadow_map_width;
            set
            {
                shadow_map_width = value;
                Light_View_to_Light_Screen.Data[0][0] = (double)2 / shadow_map_width;
                Set_Shadow_Map();
            }
        }
        public override int Shadow_Map_Height
        {
            get => shadow_map_height;
            set
            {
                shadow_map_height = value;
                Light_View_to_Light_Screen.Data[1][1] = (double)2 / shadow_map_height;
                Set_Shadow_Map();
            }
        }
        public override double Shadow_Map_Z_Near
        {
            get => shadow_map_z_near;
            set
            {
                shadow_map_z_near = value;
                Light_View_to_Light_Screen.Data[2][2] = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.Data[2][3] = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
            }
        }
        public override double Shadow_Map_Z_Far
        {
            get => shadow_map_z_far;
            set
            {
                shadow_map_z_far = value;
                Light_View_to_Light_Screen.Data[2][2] = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.Data[2][3] = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
            }
        }

        #endregion

        #region Constructors

        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double strength) : base(origin, direction_forward, direction_up)
        {
            string[] icon_obj_data = Properties.Resources.Distant_Light.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, icon_obj_data);
            Icon.Scale(5);
            Icon.Face_Colour = Color.DarkGoldenrod;

            Light_View_to_Light_Screen = Matrix4x4.Identity_Matrix();

            Shadow_Map_Width = 810;
            Shadow_Map_Height = 640;
            Shadow_Map_Z_Near = 10;
            Shadow_Map_Z_Far = 1000;

            Strength = strength;

            Calculate_Light_View_Clipping_Planes();
        }

        public Distant_Light(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double strength) : this(origin, pointed_at.World_Origin - origin, direction_up, strength) { }

        #endregion

        #region Methods

        internal override void Calculate_Light_View_Clipping_Planes()
        {
            double semi_width = shadow_map_width / 2, semi_height = shadow_map_height / 2;

            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, shadow_map_z_near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, shadow_map_z_near);
            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, shadow_map_z_near);

            Vector3D far_top_right_point = new Vector3D(semi_width, semi_height, shadow_map_z_far);
            Vector3D far_bottom_left_point = new Vector3D(-semi_width, -semi_height, shadow_map_z_far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width, -semi_height, shadow_map_z_far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, far_bottom_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_right_point, near_top_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_left_point, near_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, far_top_right_point, far_bottom_right_point);
            Vector3D near_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, near_top_right_point);
            Vector3D far_normal = Vector3D.Normal_From_Plane(far_bottom_right_point, far_top_right_point, far_bottom_left_point); // make order look nice! and below

            Light_View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_top_left_point, top_normal), // Top
                new Clipping_Plane(near_top_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, right_normal), // Right
                new Clipping_Plane(near_top_left_point, near_normal), // Near
                new Clipping_Plane(far_top_right_point, far_normal), // Far
            };
        }

        #endregion
    }
}