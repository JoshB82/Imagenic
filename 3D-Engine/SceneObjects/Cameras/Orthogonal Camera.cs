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

        private float width, height, z_near, z_far;

        public override float Width
        {
            get => width;
            set
            {
                width = value;

                // Update camera-view-to-camera-screen matrix
                Camera_View_to_Camera_Screen.m00 = 2 / width;

                // Update left and right clipping planes
                Camera_View_Clipping_Planes[0].Point.x = -width / 2;
                Camera_View_Clipping_Planes[3].Point.x = width / 2;
            }
        }
        public override float Height
        {
            get => height;
            set
            {
                height = value;

                // Update camera-view-to-camera-screen matrix
                Camera_View_to_Camera_Screen.m11 = 2 / height;

                // Update top and bottom clipping planes
                Camera_View_Clipping_Planes[1].Point.y = -height / 2;
                Camera_View_Clipping_Planes[4].Point.y = height / 2;
            }
        }
        public override float Z_Near
        {
            get => z_near;
            set
            {
                z_near = value;

                // Update camera-view-to-camera-screen matrix
                Camera_View_to_Camera_Screen.m22 = 2 / (z_far - z_near);
                Camera_View_to_Camera_Screen.m23 = -(z_far + z_near) / (z_far - z_near);

                // Update near clipping plane
                Camera_View_Clipping_Planes[2].Point.z = z_near;
            }
        }
        public override float Z_Far
        {
            get => z_far;
            set
            {
                z_far = value;

                // Update camera-view-to-camera-screen matrix
                Camera_View_to_Camera_Screen.m22 = 2 / (z_far - z_near);
                Camera_View_to_Camera_Screen.m23 = -(z_far + z_near) / (z_far - z_near);

                // Update far clipping plane
                Camera_View_Clipping_Planes[5].Point.z = z_far;
            }
        }

        #endregion

        #region Constructors

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.Camera_Width, Default.Camera_Height, Default.Camera_Z_Near, Default.Camera_Z_Far) { }

        public OrthogonalCamera(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float width, float height, float zNear, float zFar) : base(origin, directionForward, directionUp)
        {
            Camera_View_to_Camera_Screen = Matrix4x4.Identity;

            Camera_View_Clipping_Planes = new[]
            {
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_X), // Left
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Y), // Bottom
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Z), // Near
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Negative_X), // Right
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Negative_Y), // Top
                new Clipping_Plane(Vector3D.Zero, Vector3D.Unit_Negative_Z) // Far
            };

            Width = width;
            Height = height;
            Z_Near = zNear;
            Z_Far = zFar;
        }

        public OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, directionForward, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp) : this(origin, pointedAt.World_Origin - origin, directionUp, Default.Camera_Width, Default.Camera_Height, Default.Camera_Z_Near, Default.Camera_Z_Far) { }

        public OrthogonalCamera(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float width, float height, float zNear, float zFar) : this(origin, pointedAt.World_Origin - origin, directionUp, width, height, zNear, zFar) { }

        public OrthogonalCamera OrthogonalCameraAngle(Vector3D origin, SceneObject pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(origin, pointedAt, directionUp, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

        #endregion
    }
}