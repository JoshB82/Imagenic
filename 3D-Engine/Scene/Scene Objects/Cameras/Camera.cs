using System;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 World_to_Camera_View { get; private set; }
        internal Matrix4x4 Camera_View_to_Camera_Screen { get; set; }
        internal Matrix4x4 Camera_Screen_to_Camera_Window { get; private set; }
        internal void Calculate_World_To_Camera_View() => World_to_Camera_View = Model_to_World.Inverse();

        // Clipping planes
        internal Clipping_Plane[] Camera_View_Clipping_Planes { get; set; }
        internal abstract void Calculate_Camera_View_Clipping_Planes();

        // View volume parameters
        public abstract double Width { get; set; }
        public abstract double Height { get; set; }
        public abstract double Z_Near { get; set; }
        public abstract double Z_Far { get; set; }

        // Appearance
        /// <summary>
        /// Determines if the <see cref="Camera"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool Draw_Camera_Icon { get; set; } = false;
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