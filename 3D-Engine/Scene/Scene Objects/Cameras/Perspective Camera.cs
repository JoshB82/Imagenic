using static System.MathF;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Perspective_Camera"/>.
    /// </summary>
    public sealed class Perspective_Camera : Camera
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
                Camera_View_to_Camera_Screen.m00 = 2 * z_near / width;
                
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
                Camera_View_to_Camera_Screen.m11 = 2 * z_near / height;

                // Update top and bottom clipping planes
                float semi_width = width / 2, semi_height = height / 2;
                Camera_View_Clipping_Planes[4].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(-semi_width, semi_height, z_near), new Vector3D(semi_width, semi_height, z_near));
                Camera_View_Clipping_Planes[1].Normal = Vector3D.Normal_From_Plane(Vector3D.Zero, new Vector3D(semi_width, -semi_height, z_near), new Vector3D(-semi_width, -semi_height, z_near));
            }
        }
        public override float Z_Near
        {
            get => z_near;
            set
            {
                z_near = value;

                // Update camera-view-to-camera-screen matrix
                Camera_View_to_Camera_Screen.m22 = (z_far + z_near) / (z_far - z_near);
                Camera_View_to_Camera_Screen.m23 = -(2 * z_far * z_near) / (z_far - z_near);

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
                Camera_View_to_Camera_Screen.m22 = (z_far + z_near) / (z_far - z_near);
                Camera_View_to_Camera_Screen.m23 = -(2 * z_far * z_near) / (z_far - z_near);
                
                // Update far clipping plane
                Camera_View_Clipping_Planes[5].Point.z = z_far;
            }
        }

        #endregion

        #region Constructors

        public Perspective_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float width, float height, float z_near, float z_far) : base(origin, direction_forward, direction_up)
        {
            Camera_View_to_Camera_Screen = Matrix4x4.Zero;
            Camera_View_to_Camera_Screen.m32 = 1;

            Camera_View_Clipping_Planes = new[]
            {
                new Clipping_Plane(Vector3D.Zero, new Vector3D()), // Left
                new Clipping_Plane(Vector3D.Zero, new Vector3D()), // Bottom
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Z), // Near
                new Clipping_Plane(Vector3D.Zero, new Vector3D()), // Right
                new Clipping_Plane(Vector3D.Zero, new Vector3D()), // Top
                new Clipping_Plane(new Vector3D(), Vector3D.Unit_Negative_Z) // Far
            };

            Z_Near = z_near;
            Z_Far = z_far;
            this.height = height;
            Width = width;
            Height = height;
        }

        public Perspective_Camera(Vector3D origin, Scene_Object pointed_at, Vector3D direction_up, float width, float height, float z_near, float z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far, string ignore) : this(origin, direction_forward, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Scene_Object pointed_at, Vector3D direction_up, float fov_x, float fov_y, float z_near, float z_far, string ignore) : this(origin, pointed_at.World_Origin, direction_up, Tan(fov_x / 2) * z_near * 2, Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion
    }
}