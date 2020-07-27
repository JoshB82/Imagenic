namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="World_Point"/> mesh.
    /// </summary>
    public sealed class World_Point : Mesh
    {
        public World_Point(Vector3D origin) : base(origin, Vector3D.Unit_Z, Vector3D.Unit_Y)
        {
            Vertices = new Vector4D[1]
            {
                new Vector4D(0, 0, 0) // 0
            };

            Spots = new Spot[1]
            {
                new Spot(Vertices[0])
            };

            Draw_Edges = false;
            
            Draw_Faces = false;
        }
    }
}