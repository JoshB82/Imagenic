/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a distant light.
 */

using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.RenderingObjects.Lights
{
    /// <summary>
    /// Encapsulates creation of a <see cref="DistantLight"/>.
    /// </summary>
    public sealed class DistantLight : Light
    {
        #region Constructors

        public DistantLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.LightStrength, Default.ShadowMapWidth, Default.ShadowMapHeight, Default.ShadowMapZNear, Default.ShadowMapZFar) { }

        public DistantLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength) : this(origin, directionForward, directionUp, strength, Default.ShadowMapWidth, Default.ShadowMapHeight, Default.ShadowMapZNear, Default.ShadowMapZFar) { }

        public DistantLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength, int viewWidth, int viewHeight, float zNear, float zFar) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar)
        {
            Strength = strength;   

            string[] iconObjData = Properties.Resources.DistantLight.Split("\n");
            Icon = new Custom(origin, directionForward, directionUp, iconObjData)
            {
                Dimension = 3,
                FaceColour = Color.DarkCyan
            };
            Icon.Scale(5);
        }

        public static DistantLight DistantLightAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength, float fovX, float fovY, float zNear, float zFar) => new DistantLight(origin, directionForward, directionUp, strength, (Tan(fovX / 2) * zNear * 2).RoundToInt(), (Tan(fovY / 2) * zNear * 2).RoundToInt(), zNear, zFar);

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up) : this(origin, pointed_at.WorldOrigin - origin, direction_up) { }

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength) : this(origin, pointed_at.WorldOrigin - origin, direction_up, strength) { }

        public DistantLight(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, int shadow_map_width, int shadow_map_height, float shadow_map_z_near, float shadow_map_z_far) : this(origin, pointed_at.WorldOrigin - origin, direction_up, strength, shadow_map_width, shadow_map_height, shadow_map_z_near, shadow_map_z_far) { }

        public static DistantLight DistantLightAngle(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float strength, float fov_x, float fov_y, float z_near, float z_far) => new DistantLight(origin, pointed_at, direction_up, strength, (Tan(fov_x / 2) * z_near * 2).RoundToInt(), (Tan(fov_y / 2) * z_near * 2).RoundToInt(), z_near, z_far);

        #endregion
    }
}