namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Torus"/> mesh.
    /// </summary>
    public sealed class Torus : Mesh
    {
        #region Fields and Properties

        public float Inner_Radius { get; set; }
        public float Outer_Radius { get; set; }

        #endregion

        #region Constructors

        public Torus(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float radius, float inner_radius, float outer_radius) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;


        }

        #endregion
    }
}