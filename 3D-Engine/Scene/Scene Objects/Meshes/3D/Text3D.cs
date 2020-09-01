namespace _3D_Engine
{
    public sealed class Text3D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public double Size { get; set; }
        public char Style { get; set; }
        public double Depth { get; set; }

        #endregion

        #region Constructors

        public Text3D(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string[] fonts, double size, char style, double depth) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;


        }

        #endregion
    }
}