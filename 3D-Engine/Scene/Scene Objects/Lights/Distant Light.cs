using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine
{
    public sealed class Distant_Light : Light
    {
        #region Fields and Properties
        
        private int shadow_map_width = Default.Shadow_Map_Width;
        private int shadow_map_height = Default.Shadow_Map_Height;
        private double shadow_map_z_near = Default.Shadow_Map_Z_Near;
        private double shadow_map_z_far = Default.Shadow_Map_Z_Far;

        public override int Shadow_Map_Width
        {
            get => shadow_map_width;
            set
            {
                shadow_map_width = value;

                // Update shadow map
                Set_Shadow_Map();

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.Data[0][0] = (double)2 / shadow_map_width;

                // Update left and right clipping planes
                Light_View_Clipping_Planes[0].Point = new Vector3D((double)-shadow_map_width / 2,
                    (double)shadow_map_height / 2, shadow_map_z_near);
                Light_View_Clipping_Planes[3].Point = new Vector3D((double)shadow_map_width / 2,
                    (double)-shadow_map_height / 2, shadow_map_z_far);
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
                Light_View_to_Light_Screen.Data[1][1] = (double)2 / shadow_map_height;

                // Update top and bottom clipping planes
                Light_View_Clipping_Planes[1].Point = new Vector3D((double)shadow_map_width / 2,
                    (double)-shadow_map_height / 2, shadow_map_z_far);
                Light_View_Clipping_Planes[4].Point = new Vector3D((double)-shadow_map_width / 2,
                    (double)shadow_map_height / 2, shadow_map_z_near);
            }
        }
        public override double Shadow_Map_Z_Near
        {
            get => shadow_map_z_near;
            set
            {
                shadow_map_z_near = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.Data[2][2] = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.Data[2][3] = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);

                // Update near clipping plane
                Light_View_Clipping_Planes[2].Point = new Vector3D((double) -shadow_map_width / 2,
                    (double) shadow_map_height / 2, shadow_map_z_near);
            }
        }
        public override double Shadow_Map_Z_Far
        {
            get => shadow_map_z_far;
            set
            {
                shadow_map_z_far = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.Data[2][2] = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.Data[2][3] = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);

                // Update far clipping plane
                Light_View_Clipping_Planes[5].Point = new Vector3D((double) shadow_map_width / 2,
                    (double) -shadow_map_height / 2, shadow_map_z_far);
            }
        }

        #endregion

        #region Constructors

        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double strength) : base(origin, direction_forward, direction_up)
        {
            Light_View_to_Light_Screen = Matrix4x4.Identity_Matrix();

            Light_View_Clipping_Planes = new[]
            {
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_X), // Left
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Y), // Bottom
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Z), // Near
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_X), // Right
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_Y), // Top
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_Z) // Far
            };

            Shadow_Map_Width = Default.Shadow_Map_Width;
            Shadow_Map_Height = Default.Shadow_Map_Height;
            Shadow_Map_Z_Near = Default.Shadow_Map_Z_Near;
            Shadow_Map_Z_Far = Default.Shadow_Map_Z_Far;

            Strength = strength;

            string[] icon_obj_data = Properties.Resources.Distant_Light.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, icon_obj_data) { Face_Colour = Color.DarkCyan };
            Icon.Scale(5);
        }

        public Distant_Light(Vector3D origin, Scene_Object pointed_at, Vector3D direction_up, double strength) : this(origin, pointed_at.World_Origin - origin, direction_up, strength) { }

        #endregion
    }
}