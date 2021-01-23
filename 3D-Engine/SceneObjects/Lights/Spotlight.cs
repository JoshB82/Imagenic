using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.SceneObjects.Lights
{
    public sealed class Spotlight : Light
    {
        #region Fields and Properties

        private int shadow_map_width, shadow_map_height;
        private float shadow_map_z_near, shadowMapZFar;

        public override int ShadowMapWidth
        {
            get => shadow_map_width;
            set
            {
                shadow_map_width = value;

                // Update shadow map
                SetShadowMap();

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m00 = 2 * shadow_map_z_near / shadow_map_width;
            }
        }

        public override int ShadowMapHeight
        {
            get => shadow_map_height;
            set
            {
                shadow_map_height = value;
                
                // Update shadow map
                SetShadowMap();

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m11 = 2 * shadow_map_z_near / shadow_map_height;
            }
        }

        public override float ShadowMapZNear
        {
            get => shadow_map_z_near;
            set
            {
                shadow_map_z_near = value;

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m22 = (shadowMapZFar + shadow_map_z_near) / (shadowMapZFar - shadow_map_z_near);
                LightViewToLightScreen.m23 = -(2 * shadowMapZFar * shadow_map_z_near) / (shadowMapZFar - shadow_map_z_near);
            }
        }

        public override float ShadowMapZFar
        {
            get => shadowMapZFar;
            set
            {
                shadowMapZFar = value;

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m22 = (shadowMapZFar + shadow_map_z_near) / (shadowMapZFar - shadow_map_z_near);
                LightViewToLightScreen.m23 = -(2 * shadowMapZFar * shadow_map_z_near) / (shadowMapZFar - shadow_map_z_near);
            }
        }

        #endregion

        #region Constructors
        
        public Spotlight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : base(origin, direction_forward, direction_up)
        {
            LightViewToLightScreen = Matrix4x4.Zero;
            LightViewToLightScreen.m32 = 1;

            ShadowMapWidth = Default.ShadowMapWidth;
            ShadowMapHeight = Default.ShadowMapHeight;
            ShadowMapZNear = Default.ShadowMapZNear;
            ShadowMapZFar = Default.ShadowMapZFar;

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