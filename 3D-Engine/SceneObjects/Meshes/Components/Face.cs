using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Face"/>.
    /// </summary>
    public sealed class Face
    {
        #region Fields and Properties

        // Vertices
        public Vertex P1, P2, P3;
        public Vector3D T1, T2, T3;

        internal Vector4D p1, p2, p3;
        internal Vector3D t1, t2, t3;
        internal void Reset_Vertices()
        {
            p1 = P1.Point; p2 = P2.Point; p3 = P3.Point;
            t1 = T1; t2 = T2; t3 = T3;
        }

        public bool Has_Texture { get; internal set; } = false;

        // Appearance
        public Color Colour { get; set; } = Properties.Settings.Default.Face_Colour;
        public Texture Texture_Object { get; set; }
        public bool Draw_Outline { get; set; } = false;
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        internal Face(Vector4D p1, Vector4D p2, Vector4D p3)
        {
            this.p1 = p1; this.p2 = p2; this.p3 = p3;
        }

        internal Face(Vector4D p1, Vector4D p2, Vector4D p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object)
        {
            this.p1 = p1; this.p2 = p2; this.p3 = p3;
            this.t1 = t1; this.t2 = t2; this.t3 = t3;
            Texture_Object = texture_object;
        }

        public Face(Vertex model_p1, Vertex model_p2, Vertex model_p3)
        {
            P1 = model_p1; P2 = model_p2; P3 = model_p3;
        }

        public Face(Vertex model_p1, Vertex model_p2, Vertex model_p3, Vector3D t1, Vector3D t2, Vector3D t3, Texture texture_object)
        {
            P1 = model_p1; P2 = model_p2; P3 = model_p3;
            T1 = t1; T2 = t2; T3 = t3;
            Has_Texture = true; Texture_Object = texture_object;
        }

        #endregion

        #region Methods

        internal void Apply_Matrix(Matrix4x4 matrix)
        {
            p1 = matrix * p1;
            p2 = matrix * p2;
            p3 = matrix * p3;
        }

        #endregion
    }
}