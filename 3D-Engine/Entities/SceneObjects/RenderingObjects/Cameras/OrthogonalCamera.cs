/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an orthogonal camera.
 */

using _3D_Engine.Entities.Groups;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Maths;
using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="OrthogonalCamera"/>.
    /// </summary>
    public sealed class OrthogonalCamera : Camera
    {
        #region Constructors

        public OrthogonalCamera(Vector3D worldOrigin, Orientation worldOrientation) : this(worldOrigin, worldOrientation, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar, Default.CameraRenderWidth, Default.CameraRenderHeight) { }

        public OrthogonalCamera(Vector3D worldOrigin, Orientation worldOrientation, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D worldOrigin, Orientation worldOrientation, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => new OrthogonalCamera(worldOrigin, worldOrientation, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar, renderWidth, renderHeight);

        public OrthogonalCamera(Vector3D worldOrigin, SceneObject pointedAt, Vector3D directionUp) : this(worldOrigin, Orientation.CreateOrientationForwardUp(pointedAt.WorldOrigin - worldOrigin, directionUp)) { }

        public OrthogonalCamera(Vector3D worldOrigin, SceneObject pointedAt, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : this(worldOrigin, Orientation.CreateOrientationForwardUp(pointedAt.WorldOrigin - worldOrigin, directionUp), viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D worldOrigin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => OrthogonalCameraAngle(worldOrigin, Orientation.CreateOrientationForwardUp(pointedAt.WorldOrigin - worldOrigin, directionUp), fovX, fovY, zNear, zFar, renderWidth, renderHeight);

        #endregion

        #region Methods

        internal override void ProcessLighting(Group sceneToRender)
        {
            Matrix4x4 windowToWorld = ViewToWorld * ScreenToView * WindowToScreen;

            for (int x = 0; x < RenderWidth; x++)
            {
                for (int y = 0; y < RenderHeight; y++)
                {
                    if (zBuffer.Values[x][y] != outOfBoundsValue)
                    {
                        // Move the point from window space to world space
                        Vector4D worldSpacePoint = windowToWorld * new Vector4D(x, y, zBuffer.Values[x][y], 1);

                        // Apply lighting
                        ApplyLighting(worldSpacePoint, ref colourBuffer.Values[x][y], x, y, sceneToRender);
                    }
                }
            }
        }

        #endregion
    }
}