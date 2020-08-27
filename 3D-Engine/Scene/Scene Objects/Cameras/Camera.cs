using System;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 World_to_Camera_View { get; set; }
        internal Matrix4x4 Camera_View_to_Screen { get; set; }

        internal void Calculate_World_to_Camera_View_Matrix()
        {
            Matrix4x4 translation = Transform.Translate(-World_Origin);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_up_rotation * new Vector4D(World_Direction_Forward)), Model_Direction_Forward);

            // String the transformations together in the following order:
            // 1) Translation to final position in view space
            // 2) Rotation around direction up vector
            // 3) Rotation around direction forward vector
            World_to_Camera_View = direction_forward_rotation * direction_up_rotation * translation;
        }

        // Clipping planes
        internal Clipping_Plane[] Camera_View_Clipping_Planes { get; set; }
        internal abstract void Calculate_Camera_View_Clipping_Planes(); // when's the best time to calculate this?

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
        /// Determines if the outline of the <see cref="Camera">Camera's</see> view is drawn.
        /// </summary>
        public bool Draw_Entire_View { get; set; } = false;
        /// <summary>
        /// Determines if the outline of the <see cref="Camera">Camera's</see> view is drawn, up to the near plane.
        /// </summary>
        public bool Draw_Near_View { get; set; } = false;

        public Mesh Icon { get; protected set; }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        #endregion
    }
}