namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Ring"/> mesh.
    /// </summary>
    public sealed class Ring : Mesh
    {
        #region Fields and Properties

        private double inner_radius, outer_radius;
        private int resolution;

        public double Inner_Radius
        {
            get => inner_radius;
            set
            {
                inner_radius = value;
                Circle inner = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution);
                for (int i = 1; i <= resolution; i++) Vertices[i] = inner.Vertices[i];
            }
        }
        public double Outer_Radius
        {
            get => outer_radius;
            set
            {
                outer_radius = value;
                Circle outer = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution);
                for (int i = 1; i <= resolution; i++) Vertices[i + resolution] = outer.Vertices[i];
            }
        }
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                Circle inner = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution);
                Circle outer = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution);
                for (int i = 1; i <= resolution; i++)
                {
                    Vertices[i] = inner.Vertices[i];
                    Vertices[i + resolution] = outer.Vertices[i];
                }
            }
        }

        #endregion

        #region Constructors

        public Ring(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double inner_radius, double outer_radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Inner_Radius = inner_radius;
            Outer_Radius = outer_radius;
            Resolution = resolution;

            Circle inner = new Circle(origin, direction_forward, direction_up, inner_radius, resolution);
            Circle outer = new Circle(origin, direction_forward, direction_up, outer_radius, resolution);

            Vertices = new Vector4D[2 * resolution + 1];
            Vertices[0] = Vector4D.Zero;

            Spots = new Spot[1] { new Spot(Vertices[0]) };

            Edges = new Edge[2 * resolution];
            for (int i = 0; i < resolution; i++)
            {
                Edges[i] = inner.Edges[i];
                Edges[i + resolution] = outer.Edges[i];
            }

            Faces = new Face[2 * resolution];
            for (int i = 0; i < 2 * resolution - 2; i += 2)
            {
                Faces[i] = new Face(inner.Vertices[i + 1], outer.Vertices[i + 2], outer.Vertices[i + 1]);
                Faces[i + 1] = new Face(inner.Vertices[i + 1], inner.Vertices[i + 2], outer.Vertices[i + 2]);
            }
            Faces[2 * resolution - 2] = new Face(inner.Vertices[resolution], outer.Vertices[1], outer.Vertices[resolution]);
            Faces[2 * resolution - 1] = new Face(inner.Vertices[resolution], inner.Vertices[1], outer.Vertices[1]);
        }

        #endregion
    }
}