/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of an orthogonal camera.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="OrthogonalCamera"/>.
    /// </summary>
    public sealed class OrthogonalCamera : Camera
    {
        #region Constructors

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar, Default.CameraRenderWidth, Default.CameraRenderHeight) { }

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => new OrthogonalCamera(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar, renderWidth, renderHeight);

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp) { }

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float width, float height, float zNear, float zFar, int renderWidth, int renderHeight) : this(origin, pointedAt.WorldOrigin - origin, directionUp, width, height, zNear, zFar, renderWidth, renderHeight) { }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => OrthogonalCameraAngle(origin, pointedAt.WorldOrigin - origin, directionUp, fovX, fovY, zNear, zFar, renderWidth, renderHeight);

        #endregion

        #region Methods

        internal override void ProcessLighting()
        {
            Matrix4x4 windowToWorld = this.ModelToWorld * this.ViewToScreen.Inverse() * cameraScreenToWindowInverse;

            for (int x = 0; x < RenderWidth; x++)
            {
                for (int y = 0; y < RenderHeight; y++)
                {
                    if (zBuffer.Values[x][y] != outOfBoundsValue)
                    {
                        // Move the point from window space to world space and apply lighting
                        ApplyLighting(windowToWorld * new Vector4D(x, y, zBuffer.Values[x][y], 1), ref colourBuffer.Values[x][y], x, y);
                    }
                }
            }
        }

        #endregion
    }
}