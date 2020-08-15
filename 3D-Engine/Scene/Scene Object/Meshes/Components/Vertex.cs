namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of an <see cref="Vertex"/>.
    /// </summary>
    public sealed class Vertex
    {
        #region Fields and Constructors

        public Vector4D Point { get; set; }
        public Vector3D Normal { get; set; }

        #endregion

        #region Constructors

        public Vertex(Vector4D point) => Point = point;

        #endregion
    }
}