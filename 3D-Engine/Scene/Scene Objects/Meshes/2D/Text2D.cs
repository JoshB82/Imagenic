namespace _3D_Engine
{
    public sealed class Text2D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public float Size { get; set; }
        public char Style { get; set; }

        #endregion

        #region Constructors

        public Text2D(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, string[] fonts, float size, char style) : base(origin, direction_forward, direction_up)
        {
            Dimension = 2;


        }

        #endregion
    }
}