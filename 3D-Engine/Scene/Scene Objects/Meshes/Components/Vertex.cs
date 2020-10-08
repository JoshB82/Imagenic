namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of an <see cref="Vertex"/>.
    /// </summary>
    public sealed class Vertex
    {
        #region Fields and Constructors

        public Vector3D Normal { get; set; }
        public Vector4D Point { get; set; }

        #endregion

        #region Constructors

        public Vertex(Vector4D point)
        {
            Point = point;
        }

        public Vertex(Vector4D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }

        #endregion
    }
}