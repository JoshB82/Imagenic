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

using _3D_Engine.Groups;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using static _3D_Engine.Properties.Settings;
using static System.MathF;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.SceneObjects;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="PerspectiveCamera"/>.
    /// </summary>
    public sealed class PerspectiveCamera : Camera
    {
        #region Constructors

        public PerspectiveCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar, Default.CameraRenderWidth, Default.CameraRenderHeight) { }

        public PerspectiveCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(origin, directionForward, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight)
        {
            ScreenToView = Matrix4x4.Zero;
            ScreenToView.m23 = 1;
        }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => new(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar, renderWidth, renderHeight);

        public PerspectiveCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp) { }

        public PerspectiveCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : this(origin, pointedAt.WorldOrigin - origin, directionUp, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        public static PerspectiveCamera PerspectiveCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => PerspectiveCameraAngle(origin, pointedAt.WorldOrigin - origin, directionUp, fovX, fovY, zNear, zFar, renderWidth, renderHeight);

        #endregion

        #region Methods

        internal override void ProcessLighting(Group sceneToRender)
        {
            for (int x = 0; x < RenderWidth; x++)
            {
                for (int y = 0; y < RenderHeight; y++)
                {
                    if (zBuffer.Values[x][y] != outOfBoundsValue)
                    {
                        // Move the point from window space to screen space
                        Vector4D screenSpacePoint = WindowToScreen * new Vector4D(x, y, zBuffer.Values[x][y], 1);

                        // Move the point from screen space to world space and apply lighting
                        screenSpacePoint *= 2 * ZNear * ZFar / (ZNear + ZFar - screenSpacePoint.z * (ZFar - ZNear));
                        ApplyLighting(ScreenToWorld * screenSpacePoint, ref colourBuffer.Values[x][y], x, y, sceneToRender);
                    }
                }
            }
        }

        #endregion
    }
}