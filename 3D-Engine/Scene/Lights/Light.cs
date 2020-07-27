using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Light : Scene_Object
    {
        #region Fields and Properties

        // Transformations
        public Vector3D Translation { get; protected set; }

        // Appearance
        public Color Colour { get; set; } = Color.White;
        public double Strength { get; set; }

        /*private double intensity; // should it be set to something initially?
        public double Intensity
        {
            get => intensity;
            set { intensity = (value < 0) ? 0 : (value > 1) ? 1 : intensity; }
        }
        */
        public string Icon { get; protected set; }

        internal double[][] z_buffer;

        #endregion

        #region Constructors

        internal Light(Vector3D origin, Vector3D direction_forward, Vector3D direction_up) : base(origin, direction_forward, direction_up)
        {
        }

        #endregion
    }
}