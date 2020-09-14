using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Camera : Scene_Object
    {
        #region Fields and Properties

        // Appearance
        public Mesh Icon { get; protected set; }

        /// <summary>
        /// Determines if the <see cref="Camera"/> is drawn in the <see cref="Scene"/>.
        /// </summary>
        public bool Draw_Icon { get; set; } = false;

        public View_Outline View_Style = View_Outline.Entire;

        /// <summary>
        /// Determines if the outline of the <see cref="Camera">Camera's</see> view is drawn.
        /// </summary>
        
        /// <summary>
        /// Determines if the outline of the <see cref="Camera">Camera's</see> view is drawn, up to the near plane.
        /// </summary>

        // Matrices
        internal Matrix4x4 World_to_Camera_View { get; private set; }

        internal Matrix4x4 Camera_View_to_Camera_Screen;

        internal void Calculate_World_To_Camera_View() => World_to_Camera_View = Model_to_World.Inverse();

        // Clipping planes
        internal Clipping_Plane[] Camera_View_Clipping_Planes;
        internal static Clipping_Plane[] Camera_Screen_Clipping_Planes { get; } = new[]
        {
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_X), // Left
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_Y), // Bottom
            new Clipping_Plane(-Vector3D.One, Vector3D.Unit_Z), // Near
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_X), // Right
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_Y), // Top
            new Clipping_Plane(Vector3D.One, Vector3D.Unit_Negative_Z) // Far
        };

        // View volume
        public abstract float Width { get; set; }
        public abstract float Height { get; set; }
        public abstract float Z_Near { get; set; }
        public abstract float Z_Far { get; set; }

        #endregion

        #region Constructors

        internal Camera(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up)
        {
            string[] icon_obj_data = Properties.Resources.Camera.Split("\n");
            Icon = new Custom(origin, direction_forward, direction_up, icon_obj_data) { Face_Colour = Color.DarkCyan };
            Icon.Scale(5);
        }

        #endregion
    }
}