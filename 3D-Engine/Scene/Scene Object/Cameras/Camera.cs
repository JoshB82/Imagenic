using System;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 Model_to_World { get; set; }
        internal Matrix4x4 World_to_View { get; set; }
        internal Matrix4x4 View_to_Screen { get; set; }

        // Clipping planes
        internal Clipping_Plane[] View_Clipping_Planes { get; set; }
        internal abstract void Calculate_Clipping_Planes();

        // View volume parameters
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double Z_Near { get; set; }
        public abstract double Z_Far { get; set; }

        // Appearance
        /// <summary>
        /// Determines if the <see cref="Camera"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool Draw_Camera_Model { get; set; } = false;
        /// <summary>
        /// Determines if the outline of the <see cref="Camera"/>'s view is drawn.
        /// </summary>
        public bool Draw_Entire_View { get; set; } = false;
        /// <summary>
        /// Determines if the outline of the <see cref="Camera"/>'s view is drawn, up to the near plane.
        /// </summary>
        public bool Draw_Near_View { get; set; } = false;

        public string Icon { get; protected set; }

        #endregion

        #region Matrix calculations
        
        internal void Calculate_Model_to_World_Matrix()
        {
            // Calculate required transformations
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(Model_Direction_Forward, World_Direction_Forward);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_forward_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            Model_to_World = translation * direction_up_rotation * direction_forward_rotation;
        }
        internal void Calculate_World_to_View_Matrix()
        {
            // Calculate required transformations
            Matrix4x4 translation = Transform.Translate(-World_Origin);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_up_rotation * new Vector4D(World_Direction_Forward)), Model_Direction_Forward);

            // String the transformations together in the following order:
            // 1) Translation to final position in view space
            // 2) Rotation around direction up vector
            // 3) Rotation around direction forward vector
            World_to_View = direction_forward_rotation * direction_up_rotation * translation;
        }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        #endregion
    }

    public class Orthogonal_Camera : Camera
    {
        #region Fields and Properties

        private double width, height, z_near, z_far;
        public override double Width
        {
            get => width;
            set
            {
                width = value;
                View_to_Screen.Data[0][0] = 2 / width;
            }
        }
        public override double Height
        {
            get => height;
            set
            {
                height = value;
                View_to_Screen.Data[1][1] = 2 / height;
            }
        }
        public override double Z_Near
        {
            get => z_near;
            set
            {
                z_near = value;
                View_to_Screen.Data[2][2] = -2 / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }
        public override double Z_Far
        {
            get => z_far;
            set
            {
                z_far = value;
                View_to_Screen.Data[2][2] = -2 / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(z_far + z_near) / (z_far - z_near);
            }
        }

        #endregion

        #region Constructors

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(origin, direction_forward, direction_up)
        {
            View_to_Screen = Matrix4x4.Identity_Matrix();

            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;
            Calculate_Clipping_Planes();
        }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, direction_forward, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, pointed_at.World_Origin, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

        internal override void Calculate_Clipping_Planes()
        {
            double semi_width = Width / 2, semi_height = Height / 2;

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width, semi_height, Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width, -semi_height, Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, near_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_top_right_point, far_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, near_bottom_right_point, far_bottom_right_point); //?

            View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near z
                new Clipping_Plane(far_top_left_point, Vector3D.Unit_Negative_Z), // Far z
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, top_normal), // Top
                new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }
    }

    public class Perspective_Camera : Camera
    {
        #region Fields and Properties

        private double width, height, z_near, z_far;
        public override double Width
        {
            get => width;
            set
            {
                width = value;
                View_to_Screen.Data[0][0] = 2 * z_near / width;
            }
        }
        public override double Height
        {
            get => height;
            set
            {
                height = value;
                View_to_Screen.Data[1][1] = 2 * z_near / height;
            }
        }
        public override double Z_Near
        {
            get => z_near;
            set
            {
                z_near = value;
                View_to_Screen.Data[2][2] = (z_far + z_near) / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }
        public override double Z_Far
        {
            get => z_far;
            set
            {
                z_far = value;
                View_to_Screen.Data[2][2] = (z_far + z_near) / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }

        #endregion

        #region Constructors

        public Perspective_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(origin, direction_forward, direction_up)
        {
            View_to_Screen = Matrix4x4.Zeroed_Matrix();
            View_to_Screen.Data[3][2] = 1;

            Z_Near = z_near;
            Z_Far = z_far;
            Width = width;
            Height = height;
            Calculate_Clipping_Planes();
        }

        public Perspective_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, direction_forward, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double fov_x, double fov_y, double z_near, double z_far, string ignore) : this(origin, pointed_at.World_Origin, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

        internal override void Calculate_Clipping_Planes()
        {
            double semi_width = Width / 2, semi_height = Height / 2, z_ratio = Z_Far / Z_Near; // put in get

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width * z_ratio, semi_height * z_ratio, Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width * z_ratio, -semi_height * z_ratio, Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_bottom_right_point, far_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_bottom_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_left_point, near_top_right_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_bottom_right_point, near_top_right_point, far_bottom_right_point); //make order look nice

            View_Clipping_Planes = new Clipping_Plane[]
            {
                new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Z), // Near z
                new Clipping_Plane(far_top_left_point, Vector3D.Unit_Negative_Z), // Far z
                new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                new Clipping_Plane(near_top_right_point, top_normal), // Top
                new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }
    }
}