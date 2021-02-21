/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a perspective camera.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Drawing;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="PerspectiveCamera"/>.
    /// </summary>
    public sealed class PerspectiveCamera : Camera
    {
        #region Constructors

        public PerspectiveCamera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : this(origin, direction_forward, direction_up, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar) { }

        public PerspectiveCamera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float width, float height, float z_near, float z_far) : base(origin, direction_forward, direction_up)
        {
            

            ZNear = z_near;
            ZFar = z_far;
            this.height = height;
            ViewWidth = width;
            ViewHeight = height;
        }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far) => new PerspectiveCamera(origin, direction_forward, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far);

        public PerspectiveCamera(Vector3D origin, SceneObject pointed_at, Vector3D direction_up) : this(origin, pointed_at.WorldOrigin - origin, direction_up) { }

        public PerspectiveCamera(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float width, float height, float z_near, float z_far) : this(origin, pointed_at.WorldOrigin - origin, direction_up, width, height, z_near, z_far) { }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far) => new PerspectiveCamera(origin, pointed_at, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far);

        #endregion

        #region Methods

        internal override void ProcessLighting()
        {
            for (int x = 0; x < renderWidth; x++)
            {
                for (int y = 0; y < renderHeight; y++)
                {
                    // check all floats and ints
                    if (zBuffer.Values[x][y] != outOfBoundsValue)
                    {
                        ShadowMapCheck
                        (
                            x, y, zBuffer.Values[x][y],
                            ref colourBuffer.Values[x][y],
                            ref cameraScreenToWindowInverse,
                            ref this.CameraScreenToWorld
                        );
                    }
                }
            }
        }

        // Shadow Map Check (SMC)
        private void ShadowMapCheck(
            int x, int y, float z,
            ref Color pointColour,
            ref Matrix4x4 windowToCameraScreen,
            ref Matrix4x4 cameraScreenToWorld)
        {
            // Move the point from window space to camera-screen space
            Vector4D cameraScreenSpacePoint = windowToCameraScreen * new Vector4D(x, y, z, 1);

            // Move the point from camera-screen space to world space
            cameraScreenSpacePoint *= 2 * this.ZNear * this.ZFar / (this.ZNear + this.ZFar - cameraScreenSpacePoint.z * (this.ZFar - this.ZNear));

            // Apply lighting
            ApplyLighting(cameraScreenToWorld * cameraScreenSpacePoint, ref pointColour, x, y);
        }

        #endregion
    }
}