using System.Diagnostics;

namespace _3D_Engine
{
    public sealed class Text2D : Mesh
    {
        #region Fields and Properties

        public string[] Fonts { get; set; }
        public double Size { get; set; }
        public char Style { get; set; }

        #endregion

        #region Constructors

        public Text2D(Vector3D origin, Vector3D direction, Vector3D direction_up, string[] fonts, double size, char style)
        {

        }

        #endregion
    }
}
