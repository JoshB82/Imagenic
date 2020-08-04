namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="World_Point"/> mesh.
    /// </summary>
    public sealed class World_Point : Mesh
    {
        #region Constructors

        /// <summary>
        /// Creates a <see cref="World_Point"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="World_Point"/>.</param>
        public World_Point(Vector3D origin) : base(origin, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Vertices = new Vertex[1] { new Vertex(new Vector4D(0, 0, 0)) };

            Draw_Edges = false;
            
            Draw_Faces = false;
        }

        #endregion
    }
}