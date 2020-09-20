using System;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine
{
    public class Orthogonal_Camera : Camera
    {
        #region Fields and Properties

        private float width = Default.Camera_Width;
        private float height = Default.Camera_Height;
        private float z_near = Default.Camera_Z_Near;
        private float z_far = Default.Camera_Z_Far;

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

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float width, float height, float z_near, float z_far) : base(origin, direction_forward, direction_up)
        {
            Camera_View_to_Camera_Screen = Matrix4x4.Identity;

            Camera_View_Clipping_Planes = new[]
            {
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_X), // Left
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Y), // Bottom
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Z), // Near
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_X), // Right
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_Y), // Top
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_Z) // Far
            };

            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;
        }

        public Orthogonal_Camera(Vector3D origin, Scene_Object pointed_at, Vector3D direction_up, float width, float height, float z_near, float z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far, string ignore) : this(origin, direction_forward, direction_up, (float)Math.Tan(fov_x / 2) * z_near * 2, (float)Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Scene_Object pointed_at, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far, string ignore) : this(origin, pointed_at.World_Origin, direction_up, (float)Math.Tan(fov_x / 2) * z_near * 2, (float)Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

    }
}