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

        public PerspectiveCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar, Default.CameraRenderWidth, Default.CameraRenderHeight) { }

        public PerspectiveCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => new PerspectiveCamera(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar, renderWidth, renderHeight);

        public PerspectiveCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp) { }

        public PerspectiveCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float width, float height, float zNear, float zFar, int renderWidth, int renderHeight) : this(origin, pointedAt.WorldOrigin - origin, directionUp, width, height, zNear, zFar, renderWidth, renderHeight) { }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => PerspectiveCameraAngle(origin, pointedAt.WorldOrigin - origin, directionUp, fovX, fovY, zNear, zFar, renderWidth, renderHeight);

        #endregion

        #region Methods

        internal override void ProcessLighting()
        {
            for (int x = 0; x < RenderWidth; x++)
            {
                for (int y = 0; y < RenderHeight; y++)
                {
                    // check all floats and ints
                    if (zBuffer.Values[x][y] != outOfBoundsValue)
                    {
                        ShadowMapCheck
                        (
                            x, y, zBuffer.Values[x][y],
                            ref colourBuffer.Values[x][y],
                            ref cameraScreenToWindowInverse,
                            ref this.ScreenToWorld
                        );
                    }
                }
            }
        }

        // Shadow Map Check
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