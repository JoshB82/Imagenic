using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine
{
    public sealed class Spotlight : Light
    {
        #region Fields and Properties

        private int shadow_map_width = Default.Shadow_Map_Width;
        private int shadow_map_height = Default.Shadow_Map_Height;
        private float shadow_map_z_near = Default.Shadow_Map_Z_Near;
        private float shadow_map_z_far = Default.Shadow_Map_Z_Far;

        public override int Shadow_Map_Width
        {
            get => shadow_map_width;
            set
            {
                shadow_map_width = value;

                // Update shadow map
                Set_Shadow_Map();

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m00 = 2 * shadow_map_z_near / shadow_map_width;
            }
        }

        public override int Shadow_Map_Height
        {
            get => shadow_map_height;
            set
            {
                shadow_map_height = value;
                
                // Update shadow map
                Set_Shadow_Map();

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m11 = 2 * shadow_map_z_near / shadow_map_height;
            }
        }

        public override float Shadow_Map_Z_Near
        {
            get => shadow_map_z_near;
            set
            {
                shadow_map_z_near = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m22 = (shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.m23 = -(2 * shadow_map_z_far * shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
            }
        }

        public override float Shadow_Map_Z_Far
        {
            get => shadow_map_z_far;
            set
            {
                shadow_map_z_far = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m22 = (shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.m23 = -(2 * shadow_map_z_far * shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);
            }
        }

        #endregion

        #region Constructors

        public Spotlight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : base(origin, direction_forward, direction_up)
        {
            Light_View_to_Light_Screen = Matrix4x4.Zero;
            Light_View_to_Light_Screen.m32 = 1;

            Shadow_Map_Width = Default.Shadow_Map_Width;
            Shadow_Map_Height = Default.Shadow_Map_Height;
            Shadow_Map_Z_Near = Default.Shadow_Map_Z_Near;
            Shadow_Map_Z_Far = Default.Shadow_Map_Z_Far;

            Strength = strength;
        }

        #endregion

        /*
        internal override void Calculate_Light_View_Clipping_Planes()
        {
            float semi_width = (float)shadow_map_width / 2, semi_height = (float)shadow_map_height / 2, z_ratio = shadow_map_z_far / shadow_map_z_near;

            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, shadow_map_z_near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, shadow_map_z_near);
            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, shadow_map_z_near);

            Vector3D far_top_right_point = new Vector3D(semi_width * z_ratio, semi_height * z_ratio, shadow_map_z_far);
            Vector3D far_bottom_left_point = new Vector3D(-semi_width * z_ratio, -semi_height * z_ratio, shadow_map_z_far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width * z_ratio, -semi_height * z_ratio, shadow_map_z_far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, far_bottom_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_right_point, near_top_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_left_point, near_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, far_top_right_point, far_bottom_right_point);

            Light_View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_top_left_point, top_normal), // Top
                new Clipping_Plane(near_top_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, right_normal), // Right
                new Clipping_Plane(near_top_left_point, Vector3D.Unit_Z), // Near
                new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Z) // Far
            };
        }
        */
    }
}