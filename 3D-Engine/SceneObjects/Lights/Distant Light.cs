using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects;
using _3D_Engine.SceneObjects.Cameras;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Distant_Light"/>.
    /// </summary>
    public sealed class Distant_Light : Light
    {
        #region Fields and Properties

        private int shadow_map_width, shadow_map_height;
        private float shadow_map_z_near, shadow_map_z_far;

        public override int Shadow_Map_Width
        {
            get => shadow_map_width;
            set
            {
                shadow_map_width = value;

                // Update shadow map
                Set_Shadow_Map();

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m00 = 2f / shadow_map_width;

                // Update left and right clipping planes
                Light_View_Clipping_Planes[0].Point.x = -shadow_map_width / 2f;
                Light_View_Clipping_Planes[3].Point.x = shadow_map_width / 2f;
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
                Light_View_to_Light_Screen.m11 = 2f / shadow_map_height;

                // Update top and bottom clipping planes
                Light_View_Clipping_Planes[1].Point.y = -shadow_map_height / 2f;
                Light_View_Clipping_Planes[4].Point.y = shadow_map_height / 2f;
            }
        }
        public override float Shadow_Map_Z_Near
        {
            get => shadow_map_z_near;
            set
            {
                shadow_map_z_near = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m22 = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.m23 = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);

                // Update near clipping plane
                Light_View_Clipping_Planes[2].Point.z = shadow_map_z_near;
            }
        }
        public override float Shadow_Map_Z_Far
        {
            get => shadow_map_z_far;
            set
            {
                shadow_map_z_far = value;

                // Update light-view-to-light-screen matrix
                Light_View_to_Light_Screen.m22 = 2 / (shadow_map_z_far - shadow_map_z_near);
                Light_View_to_Light_Screen.m23 = -(shadow_map_z_far + shadow_map_z_near) / (shadow_map_z_far - shadow_map_z_near);

                // Update far clipping plane
                Light_View_Clipping_Planes[5].Point.z = shadow_map_z_far;
            }
        }

        #endregion

        #region Constructors

        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : this(origin, direction_forward, direction_up, Default.Light_Strength, Default.Shadow_Map_Width, Default.Shadow_Map_Height, Default.Shadow_Map_Z_Near, Default.Shadow_Map_Z_Far) { }

        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : this(origin, direction_forward, direction_up, strength, Default.Shadow_Map_Width, Default.Shadow_Map_Height, Default.Shadow_Map_Z_Near, Default.Shadow_Map_Z_Far) { }

        public Distant_Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength, int shadow_map_width, int shadow_map_height, float shadow_map_z_near, float shadow_map_z_far) : base(origin, direction_forward, direction_up)
        {
            Light_View_to_Light_Screen = Matrix4x4.Identity;

            Light_View_Clipping_Planes = new[]
            {
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitX), // Left
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitY), // Bottom
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitZ), // Near
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitNegativeX), // Right
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitNegativeY), // Top
                new Clipping_Plane(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
            };

            Strength = strength;

            Shadow_Map_Width = shadow_map_width;
            Shadow_Map_Height = shadow_map_height;
            Shadow_Map_Z_Near = shadow_map_z_near;
            Shadow_Map_Z_Far = shadow_map_z_far;

            string[] icon_obj_data = Properties.Resources.Distant_Light.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, icon_obj_data)
            {
                Dimension = 3,
                Face_Colour = Color.DarkCyan
            };
            Icon.Scale(5);
        }

        public Distant_Light Distant_Light_Angle(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength, float fov_x, float fov_y, float z_near, float z_far) => new Distant_Light(origin, direction_forward, direction_up, strength, (Tan(fov_x / 2) * z_near * 2).Round_to_Int(), (Tan(fov_y / 2) * z_near * 2).Round_to_Int(), z_near, z_far);

        public Distant_Light(Vector3D origin, SceneObject pointed_at, Vector3D direction_up) : this(origin, pointed_at.World_Origin - origin, direction_up) { }

        public Distant_Light(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength) : this(origin, pointed_at.World_Origin - origin, direction_up, strength) { }

        public Distant_Light(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, int shadow_map_width, int shadow_map_height, float shadow_map_z_near, float shadow_map_z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, strength, shadow_map_width, shadow_map_height, shadow_map_z_near, shadow_map_z_far) { }

        public Distant_Light Distant_Light_Angle(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, float fov_x, float fov_y, float z_near, float z_far) => new Distant_Light(origin, pointed_at, direction_up, strength, (Tan(fov_x / 2) * z_near * 2).Round_to_Int(), (Tan(fov_y / 2) * z_near * 2).Round_to_Int(), z_near, z_far);

        #endregion
    }
}