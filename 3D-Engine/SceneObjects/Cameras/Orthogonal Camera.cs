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

namespace _3D_Engine.SceneObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="OrthogonalCamera"/>.
    /// </summary>
    public sealed class OrthogonalCamera : Camera
    {
        #region Fields and Properties

        private float width, height, zNear, zFar;

        public override float Width
        {
            get => width;
            set
            {
                width = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m00 = 2 / width;

                // Update left and right clipping planes
                CameraViewClippingPlanes[0].Point.x = -width / 2;
                CameraViewClippingPlanes[3].Point.x = width / 2;
            }
        }
        public override float Height
        {
            get => height;
            set
            {
                height = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m11 = 2 / height;

                // Update top and bottom clipping planes
                CameraViewClippingPlanes[1].Point.y = -height / 2;
                CameraViewClippingPlanes[4].Point.y = height / 2;
            }
        }
        public override float ZNear
        {
            get => zNear;
            set
            {
                zNear = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m22 = 2 / (zFar - zNear);
                CameraViewToCameraScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                // Update near clipping plane
                CameraViewClippingPlanes[2].Point.z = zNear;
            }
        }
        public override float ZFar
        {
            get => zFar;
            set
            {
                zFar = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m22 = 2 / (zFar - zNear);
                CameraViewToCameraScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                // Update far clipping plane
                CameraViewClippingPlanes[5].Point.z = zFar;
            }
        }

        #endregion

        #region Constructors

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar) { }

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float width, float height, float zNear, float zFar) : base(origin, directionForward, directionUp)
        {
            CameraViewToCameraScreen = Matrix4x4.Identity;

            CameraViewClippingPlanes = new[]
            {
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitX), // Left
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitY), // Bottom
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitZ), // Near
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeX), // Right
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeY), // Top
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
            };

            Width = width;
            Height = height;
            ZNear = zNear;
            ZFar = zFar;
        }

        public OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp, Default.CameraWidth, Default.CameraHeight, Default.CameraZNear, Default.CameraZFar) { }

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float width, float height, float zNear, float zFar) : this(origin, pointedAt.WorldOrigin - origin, directionUp, width, height, zNear, zFar) { }

        public OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, pointedAt, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        #endregion
    }
}