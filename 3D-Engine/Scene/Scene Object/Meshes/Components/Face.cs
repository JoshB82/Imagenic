using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Face"/>.
    /// </summary>
    public sealed class Face
    {
        #region Fields and Properties

        // Positions
        internal Vertex P1 { get; set; }
        internal Vertex P2 { get; set; }
        internal Vertex P3 { get; set; }
        public Vector3D World_P1 { get; internal set; }
        public Vector3D World_P2 { get; internal set; }
        public Vector3D World_P3 { get; internal set; }
        public Vector3D T1 { get; internal set; }
        public Vector3D T2 { get; internal set; }
        public Vector3D T3 { get; internal set; }

        // Appearance
        public Color Colour { get; set; } = Settings.Default_Face_Colour;
        public Texture Texture_Object { get; set; }
        public bool Draw_Outline { get; set; } = false;
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        public Face(Vertex p1, Vertex p2, Vertex p3)
        {
            P1 = p1; P2 = p2; P3 = p3;
        }

        public Face(Vertex p1, Vertex p2, Vertex p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object)
        {
            P1 = p1; P2 = p2; P3 = p3;
            T1 = t1; T2 = t2; T3 = t3;
            Texture_Object = texture_object;
        }

        #endregion
    }
}