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
        #region Fields and Properties

        private float viewWidth, viewHeight, zNear, zFar;

        public override float ViewWidth
        {
            get => viewWidth;
            set
            {
                viewWidth = value;

                // Update view-to-screen matrix
                ViewToScreen.m00 = 2 / viewWidth;

                // Update left and right clipping planes
                ViewClippingPlanes[0].Point.x = -viewWidth / 2;
                ViewClippingPlanes[3].Point.x = viewWidth / 2;

                NewRenderNeeded = true;
            }
        }
        public override float ViewHeight
        {
            get => viewHeight;
            set
            {
                viewHeight = value;

                // Update view-to-screen matrix
                ViewToScreen.m11 = 2 / viewHeight;

                // Update top and bottom clipping planes
                ViewClippingPlanes[1].Point.y = -viewHeight / 2;
                ViewClippingPlanes[4].Point.y = viewHeight / 2;

                NewRenderNeeded = true;
            }
        }
        public override float ZNear
        {
            get => zNear;
            set
            {
                zNear = value;

                // Update view-to-screen matrix
                ViewToScreen.m22 = 2 / (zFar - zNear);
                ViewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                // Update near clipping plane
                ViewClippingPlanes[2].Point.z = zNear;

                NewRenderNeeded = true;
            }
        }
        public override float ZFar
        {
            get => zFar;
            set
            {
                zFar = value;

                // Update view-to-screen matrix
                ViewToScreen.m22 = 2 / (zFar - zNear);
                ViewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                // Update far clipping plane
                ViewClippingPlanes[5].Point.z = zFar;

                NewRenderNeeded = true;
            }
        }

        #endregion

        #region Constructors

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar) { }

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float width, float height, float zNear, float zFar) : base(origin, directionForward, directionUp)
        {
            ViewToScreen = Matrix4x4.Identity;

            ViewClippingPlanes = new ClippingPlane[]
            {
                new(Vector3D.Zero, Vector3D.UnitX), // Left
                new(Vector3D.Zero, Vector3D.UnitY), // Bottom
                new(Vector3D.Zero, Vector3D.UnitZ), // Near
                new(Vector3D.Zero, Vector3D.UnitNegativeX), // Right
                new(Vector3D.Zero, Vector3D.UnitNegativeY), // Top
                new(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
            };

            ViewWidth = width;
            ViewHeight = height;
            ZNear = zNear;
            ZFar = zFar;
        }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar) { }

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float width, float height, float zNear, float zFar) : this(origin, pointedAt.WorldOrigin - origin, directionUp, width, height, zNear, zFar) { }

        public static OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, pointedAt, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        #endregion

        #region Methods

        internal override void ProcessLighting()
        {
            Matrix4x4 windowToWorld = this.ModelToWorld * this.ViewToScreen.Inverse() * cameraScreenToWindowInverse;

            for (int x = 0; x < renderWidth; x++)
            {
                for (int y = 0; y < renderHeight; y++)
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