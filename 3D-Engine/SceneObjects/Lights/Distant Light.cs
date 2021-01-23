using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Meshes;
using System.Drawing;

using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.Lights
{
    /// <summary>
    /// Encapsulates creation of a <see cref="DistantLight"/>.
    /// </summary>
    public sealed class DistantLight : Light
    {
        #region Fields and Properties

        private int shadowMapWidth, shadowMapHeight;
        private float shadowMapZNear, shadowMapZFar;

        public override int ShadowMapWidth
        {
            get => shadowMapWidth;
            set
            {
                shadowMapWidth = value;

                // Update shadow map
                SetShadowMap();

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m00 = 2f / shadowMapWidth;

                // Update left and right clipping planes
                LightViewClippingPlanes[0].Point.x = -shadowMapWidth / 2f;
                LightViewClippingPlanes[3].Point.x = shadowMapWidth / 2f;
            }
        }
        public override int ShadowMapHeight
        {
            get => shadowMapHeight;
            set
            {
                shadowMapHeight = value;

                // Update shadow map
                SetShadowMap();

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m11 = 2f / shadowMapHeight;

                // Update top and bottom clipping planes
                LightViewClippingPlanes[1].Point.y = -shadowMapHeight / 2f;
                LightViewClippingPlanes[4].Point.y = shadowMapHeight / 2f;
            }
        }
        public override float ShadowMapZNear
        {
            get => shadowMapZNear;
            set
            {
                shadowMapZNear = value;

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m22 = 2 / (shadowMapZFar - shadowMapZNear);
                LightViewToLightScreen.m23 = -(shadowMapZFar + shadowMapZNear) / (shadowMapZFar - shadowMapZNear);

                // Update near clipping plane
                LightViewClippingPlanes[2].Point.z = shadowMapZNear;
            }
        }
        public override float ShadowMapZFar
        {
            get => shadowMapZFar;
            set
            {
                shadowMapZFar = value;

                // Update light-view-to-light-screen matrix
                LightViewToLightScreen.m22 = 2 / (shadowMapZFar - shadowMapZNear);
                LightViewToLightScreen.m23 = -(shadowMapZFar + shadowMapZNear) / (shadowMapZFar - shadowMapZNear);

                // Update far clipping plane
                LightViewClippingPlanes[5].Point.z = shadowMapZFar;
            }
        }

        #endregion

        #region Constructors

        public DistantLight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : this(origin, direction_forward, direction_up, Default.LightStrength, Default.ShadowMapWidth, Default.ShadowMapHeight, Default.ShadowMapZNear, Default.ShadowMapZFar) { }

        public DistantLight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength) : this(origin, direction_forward, direction_up, strength, Default.ShadowMapWidth, Default.ShadowMapHeight, Default.ShadowMapZNear, Default.ShadowMapZFar) { }

        public DistantLight(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength, int shadow_map_width, int shadow_map_height, float shadow_map_z_near, float shadow_map_z_far) : base(origin, direction_forward, direction_up)
        {
            LightViewToLightScreen = Matrix4x4.Identity;

            LightViewClippingPlanes = new[]
            {
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitX), // Left
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitY), // Bottom
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitZ), // Near
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeX), // Right
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeY), // Top
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
            };

            Strength = strength;

            ShadowMapWidth = shadow_map_width;
            ShadowMapHeight = shadow_map_height;
            ShadowMapZNear = shadow_map_z_near;
            ShadowMapZFar = shadow_map_z_far;

            string[] iconObjData = Properties.Resources.Distant_Light.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, iconObjData)
            {
                Dimension = 3,
                FaceColour = Color.DarkCyan
            };
            Icon.Scale(5);
        }

        public DistantLight DistantLightAngle(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float strength, float fov_x, float fov_y, float z_near, float z_far) => new DistantLight(origin, direction_forward, direction_up, strength, (Tan(fov_x / 2) * z_near * 2).RoundToInt(), (Tan(fov_y / 2) * z_near * 2).RoundToInt(), z_near, z_far);

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up) : this(origin, pointed_at.WorldOrigin - origin, direction_up) { }

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength) : this(origin, pointed_at.WorldOrigin - origin, direction_up, strength) { }

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, int shadow_map_width, int shadow_map_height, float shadow_map_z_near, float shadow_map_z_far) : this(origin, pointed_at.WorldOrigin - origin, direction_up, strength, shadow_map_width, shadow_map_height, shadow_map_z_near, shadow_map_z_far) { }

        public DistantLight DistantLightAngle(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, float fov_x, float fov_y, float z_near, float z_far) => new DistantLight(origin, pointed_at, direction_up, strength, (Tan(fov_x / 2) * z_near * 2).RoundToInt(), (Tan(fov_y / 2) * z_near * 2).RoundToInt(), z_near, z_far);

        #endregion
    }
}