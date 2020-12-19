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

using static _3D_Engine.Properties.Settings;
using static System.MathF;

namespace _3D_Engine.SceneObjects.Cameras
{
    /// <summary>
    /// Encapsulates creation of a <see cref="PerspectiveCamera"/>.
    /// </summary>
    public sealed class PerspectiveCamera : Camera
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
                CameraViewToCameraScreen.m00 = 2 * z_near / width;
                
                // Update left and right clipping planes
                float semi_width = width / 2, semi_height = height / 2;
                Camera_View_Clipping_Planes[0].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(-semi_width, -semi_height, z_near), new Vector3D(-semi_width, semi_height, z_near));
                Camera_View_Clipping_Planes[3].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(semi_width, semi_height, z_near), new Vector3D(semi_width, -semi_height, z_near));
            }
        }
        public override float Height
        {
            get => height;
            set
            {
                height = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m11 = 2 * z_near / height;

                // Update top and bottom clipping planes
                float semi_width = width / 2, semi_height = height / 2;
                Camera_View_Clipping_Planes[4].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(-semi_width, semi_height, z_near), new Vector3D(semi_width, semi_height, z_near));
                Camera_View_Clipping_Planes[1].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(semi_width, -semi_height, z_near), new Vector3D(-semi_width, -semi_height, z_near));
            }
        }
        public override float ZNear
        {
            get => z_near;
            set
            {
                z_near = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m22 = (z_far + z_near) / (z_far - z_near);
                CameraViewToCameraScreen.m23 = -(2 * z_far * z_near) / (z_far - z_near);

                // Update near clipping plane
                Camera_View_Clipping_Planes[2].Point.z = z_near;
            }
        }
        public override float ZFar
        {
            get => z_far;
            set
            {
                z_far = value;

                // Update camera-view-to-camera-screen matrix
                CameraViewToCameraScreen.m22 = (z_far + z_near) / (z_far - z_near);
                CameraViewToCameraScreen.m23 = -(2 * z_far * z_near) / (z_far - z_near);
                
                // Update far clipping plane
                Camera_View_Clipping_Planes[5].Point.z = z_far;
            }
        }

        #endregion

        #region Constructors

        public PerspectiveCamera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : this(origin, direction_forward, direction_up, Default.Camera_Width, Default.Camera_Height, Default.Camera_Z_Near, Default.Camera_Z_Far) { }

        public PerspectiveCamera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float width, float height, float z_near, float z_far) : base(origin, direction_forward, direction_up)
        {
            CameraViewToCameraScreen = Matrix4x4.Zero;
            CameraViewToCameraScreen.m32 = 1;

            Camera_View_Clipping_Planes = new[]
            {
                new ClippingPlane(Vector3D.Zero, Vector3D.Zero), // Left
                new ClippingPlane(Vector3D.Zero, Vector3D.Zero), // Bottom
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitZ), // Near
                new ClippingPlane(Vector3D.Zero, Vector3D.Zero), // Right
                new ClippingPlane(Vector3D.Zero, Vector3D.Zero), // Top
                new ClippingPlane(Vector3D.Zero, Vector3D.UnitNegativeZ) // Far
            };

            ZNear = z_near;
            ZFar = z_far;
            this.height = height;
            Width = width;
            Height = height;
        }

        public PerspectiveCamera Perspective_Camera_Angle(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far) => new PerspectiveCamera(origin, direction_forward, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far);

        public PerspectiveCamera(Vector3D origin, SceneObject pointed_at, Vector3D direction_up) : this(origin, pointed_at.WorldOrigin - origin, direction_up) { }

        public PerspectiveCamera(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float width, float height, float z_near, float z_far) : this(origin, pointed_at.WorldOrigin - origin, direction_up, width, height, z_near, z_far) { }

        public PerspectiveCamera Perspective_Camera_Angle(Vector3D origin, SceneObject pointed_at, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far) => new PerspectiveCamera(origin, pointed_at, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far);

        #endregion
    }
}