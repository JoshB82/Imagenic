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

        private Circle inner_circle, outer_circle;

        public double Inner_Radius
        {
            get => inner_radius;
            set
            {
                inner_radius = value;
                inner_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution);

                for (int i = 1; i <= resolution; i++) Vertices[i] = inner_circle.Vertices[i];
                
                for (int i = 0; i < resolution; i++) Edges[i] = inner_circle.Edges[i];

                Set_Faces();
            }
        }
        public double Outer_Radius
        {
            get => outer_radius;
            set
            {
                outer_radius = value;
                outer_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution);

                for (int i = 1; i <= resolution; i++) Vertices[i + resolution] = outer_circle.Vertices[i];

                for (int i = 0; i < resolution; i++) Edges[i + resolution] = outer_circle.Edges[i];

                Set_Faces();
            }
        }
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                inner_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, inner_radius, resolution);
                outer_circle = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, outer_radius, resolution);

                Vertices = new Vector4D[2 * resolution + 1];
                Vertices[0] = Vector4D.Zero;
                for (int i = 1; i <= resolution; i++)
                {
                    Vertices[i] = inner_circle.Vertices[i];
                    Vertices[i + resolution] = outer_circle.Vertices[i];
                }

                Edges = new Edge[2 * resolution];
                for (int i = 0; i < resolution; i++)
                {
                    Edges[i] = inner_circle.Edges[i];
                    Edges[i + resolution] = outer_circle.Edges[i];
                }

                Faces = new Face[2 * resolution];
                Set_Faces();
            }
        }

        private void Set_Faces()
        {
            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new Face(inner_circle.Vertices[i], outer_circle.Vertices[i + 1], outer_circle.Vertices[i]);
                Faces[i + resolution] = new Face(inner_circle.Vertices[i], inner_circle.Vertices[i + 1], outer_circle.Vertices[i + 1]);
            }
            Faces[resolution - 1] = new Face(inner_circle.Vertices[resolution - 1], outer_circle.Vertices[0], outer_circle.Vertices[resolution - 1]);
            Faces[2 * resolution - 1] = new Face(inner_circle.Vertices[resolution - 1], inner_circle.Vertices[0], outer_circle.Vertices[0]);
        }

        #endregion

        #region Constructors

        public Ring(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double inner_radius, double outer_radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Inner_Radius = inner_radius;
            Outer_Radius = outer_radius;
            Resolution = resolution;
        }

        #endregion
    }
}