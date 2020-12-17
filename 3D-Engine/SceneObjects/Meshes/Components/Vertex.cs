using _3D_Engine.Maths.Vectors;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of an <see cref="Vertex"/>.
    /// </summary>
    public sealed class Vertex
    {
        #region Fields and Constructors

        public Vector3D Normal;
        public Vector4D Point;

        #endregion

        #region Constructors

        public Vertex(Vector4D point) => Point = point;

        public Vertex(Vector4D point, Vector3D normal)
        {
            Point = point;
            Normal = normal;
        }

        #endregion
    }
}