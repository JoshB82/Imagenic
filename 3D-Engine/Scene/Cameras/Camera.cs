using System;
using System.Diagnostics;

namespace _3D_Graphics
{
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and properties
        
        // ID
        /// <summary>
        /// Unique identification number for this camera.
        /// </summary>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Origins
        /// <summary>
        /// The position of the camera in model space.
        /// </summary>
        public Vector4D Origin { get; }
        /// <summary>
        /// The position of the camera in world space.
        /// </summary>
        public Vector3D World_Origin { get; set; }

        // Directions
        /// <summary>
        /// The direction the camera faces in model space.
        /// </summary>
        public Vector3D Model_Direction { get; } = Vector3D.Unit_Negative_Z;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_X;

        /// <summary>
        /// The direction the camera faces in world space.
        /// </summary>
        public Vector3D World_Direction { get; private set; }
        public Vector3D World_Direction_Up { get; private set; }
        public Vector3D World_Direction_Right { get; private set; }

        // Matrices
        public Matrix4x4 Model_to_World { get; protected set; }
        public Matrix4x4 World_to_View { get; protected set; }
        public Matrix4x4 View_to_Screen { get; protected set; }

        // View volume parameters
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double Z_Near { get; set; }
        public abstract double Z_Far { get; set; }

        // Appearance
        public bool Draw_Camera_Model { get; set; } = false;
        public bool Draw_Entire_View { get; set; } = false;
        public bool Draw_Near_View { get; set; } = false;

        // Clipping planes
        public Clipping_Plane[] View_Clipping_Planes { get; protected set; }
        public abstract void Calculate_Clipping_Planes();

        #endregion

        #region Matrix calculations
        
        public void Calculate_Model_to_World_Matrix()
        {
            Matrix4x4 direction_rotation = Transform.Rotate_Between_Vectors(Model_Direction, World_Direction);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            Model_to_World = translation * direction_up_rotation * direction_rotation;
        }
        public void Calculate_World_to_View_Matrix()
        {
            Matrix4x4 translation = Transform.Translate(-World_Origin);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_up_rotation * new Vector4D(World_Direction)), Model_Direction);

            World_to_View = direction_rotation * direction_up_rotation * translation;
        }

        #endregion

        #region Constructors

        public Camera(Vector3D origin, Vector3D direction, Vector3D direction_up)
        {
            ID = ++next_id;

            World_Origin = origin;
            Set_Camera_Direction_1(direction, direction_up);
        }

        #endregion
    }

    public class Orthogonal_Camera : Camera
    {
        #region Fields and properties

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

        public Orthogonal_Camera(Vector3D origin, Vector3D direction, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(origin, direction, direction_up)
        {
            View_to_Screen = Matrix4x4.Identity_Matrix();

            Width = width;
            Height = height;
            Z_Near = z_near;
            Z_Far = z_far;
            Calculate_Clipping_Planes();

            Debug.WriteLine($"Orthogonal camera created at {origin}");
        }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Vector3D direction, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(origin, direction, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Orthogonal_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(origin, pointed_at.World_Origin, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

        public override void Calculate_Clipping_Planes()
        {
            double semi_width = Width / 2, semi_height = Height / 2;

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, -Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, -Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, -Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, -Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width, semi_height, -Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width, -semi_height, -Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, near_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_top_right_point, far_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, near_bottom_right_point, far_bottom_right_point);

            View_Clipping_Planes = new Clipping_Plane[]
            {
                    new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Negative_Z), // Near z
                    new Clipping_Plane(far_top_left_point, Vector3D.Unit_Z), // Far z
                    new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                    new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                    new Clipping_Plane(near_top_right_point, top_normal), // Top
                    new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }
    }

    public class Perspective_Camera : Camera
    {
        #region Fields and properties

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
                View_to_Screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }
        public override double Z_Far
        {
            get => z_far;
            set
            {
                z_far = value;
                View_to_Screen.Data[2][2] = -(z_far + z_near) / (z_far - z_near);
                View_to_Screen.Data[2][3] = -(2 * z_far * z_near) / (z_far - z_near);
            }
        }

        #endregion

        #region Constructors

        public Perspective_Camera(Vector3D origin, Vector3D direction, Vector3D direction_up, double width, double height, double z_near, double z_far) : base(origin, direction, direction_up)
        {
            View_to_Screen = Matrix4x4.Zeroed_Matrix();
            View_to_Screen.Data[3][2] = -1;

            Z_Near = z_near;
            Z_Far = z_far;
            Width = width;
            Height = height;
            Calculate_Clipping_Planes();

            Debug.WriteLine($"Perspective camera created at {origin}");
        }

        public Perspective_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, double width, double height, double z_near, double z_far) : this(origin, pointed_at.World_Origin - origin, direction_up, width, height, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Vector3D direction, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(origin, direction, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        public Perspective_Camera(Vector3D origin, Mesh pointed_at, Vector3D direction_up, string ignore, double fov_x, double fov_y, double z_near, double z_far) : this(origin, pointed_at.World_Origin, direction_up, Math.Tan(fov_x / 2) * z_near * 2, Math.Tan(fov_y / 2) * z_near * 2, z_near, z_far) { }

        #endregion

        public override void Calculate_Clipping_Planes()
        {
            double semi_width = Width / 2, semi_height = Height / 2, z_ratio = Z_Far / Z_Near;

            Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, -Z_Near);
            Vector3D near_bottom_right_point = new Vector3D(semi_width, -semi_height, -Z_Near);
            Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, -Z_Near);
            Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, -Z_Near);

            Vector3D far_top_left_point = new Vector3D(-semi_width * z_ratio, semi_height * z_ratio, -Z_Far);
            Vector3D far_bottom_right_point = new Vector3D(semi_width * z_ratio, -semi_height * z_ratio, -Z_Far);

            Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, near_bottom_right_point);
            Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, near_top_left_point, far_top_left_point);
            Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, near_top_right_point, far_top_left_point);
            Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, near_bottom_right_point, far_bottom_right_point);

            View_Clipping_Planes = new Clipping_Plane[]
            {
                    new Clipping_Plane(near_bottom_left_point, Vector3D.Unit_Negative_Z), // Near z
                    new Clipping_Plane(far_top_left_point, Vector3D.Unit_Z), // Far z
                    new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
                    new Clipping_Plane(near_bottom_left_point, left_normal), // Left
                    new Clipping_Plane(near_top_right_point, top_normal), // Top
                    new Clipping_Plane(near_top_right_point, right_normal) // Right
            };
        }
    }
}