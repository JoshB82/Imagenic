using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Light : Scene_Object
    {
        #region Fields and Properties

        // Matrices
        internal Matrix4x4 World_to_Light_View { get; set; } // use fields instead?

        internal void Calculate_World_to_Light_View_Matrix()
        {
            // Calculate required transformations
            Matrix4x4 translation = Transform.Translate(-World_Origin);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(World_Direction_Up, Model_Direction_Up);
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_up_rotation * new Vector4D(World_Direction_Forward)), Model_Direction_Forward);

            // String the transformations together in the following order:
            // 1) Translation to final position in view space
            // 2) Rotation around direction up vector
            // 3) Rotation around direction forward vector
            World_to_Light_View = direction_forward_rotation * direction_up_rotation * translation;
        }

        // Clipping planes
        internal Clipping_Plane[] Light_View_Clipping_Planes { get; set; }
        internal abstract void Calculate_Light_View_Clipping_Planes(Camera camera);

        // Appearance
        public Color Colour { get; set; } = Color.White;
        public Mesh Icon { get; protected set; }
        public double Strength { get; set; }

        public bool Draw_Camera_Model { get; set; } = false;
        public bool Draw_View { get; set; } = false;

        // Buffers
        internal double[][] z_buffer;

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up) { }

        #endregion
    }
}