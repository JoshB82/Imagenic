namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Prism"/> mesh.
    /// </summary>
    public sealed class Prism : Mesh
    {
        #region Fields and Properties

        private double height, radius;
        private int resolution;

        /// <summary>
        /// The height of the <see cref="Prism"/>.
        /// </summary>
        public double Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Prism"/>.
        /// </summary>
        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The number of points that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Prism"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                
                Circle Prism_top_and_bottom = new Circle(World_Origin, World_Direction_Forward, World_Direction_Up, 1, resolution);
                
                Vertices = new Vector4D[2 * resolution + 2];
                Vertices[0] = Prism_top_and_bottom.Vertices[0];
                for (int i = 1; i <= resolution; i++) Vertices[i] = Prism_top_and_bottom.Vertices[i];
                Vertices[resolution + 1] = Prism_top_and_bottom.Vertices[0] + new Vector4D(0, 1, 0);
                for (int i = resolution + 2; i <= 2 * resolution + 1; i++) Vertices[i] = Prism_top_and_bottom.Vertices[i] + new Vector4D(0, 1, 0);

                Spots = new Spot[2]
                {
                    new Spot(Vertices[0]),
                    new Spot(Vertices[resolution + 1])
                };

                Edges = new Edge[4 * resolution];
                for (int i = 0; i < resolution; i++)
                {
                    Edges[i] = Prism_top_and_bottom.Edges[i];
                    Edges[i + resolution] = Prism_top_and_bottom.Edges[i];
                }
                for (int i = 1; i <= resolution; i++) Edges[i + 2 * resolution - 1] = new Edge(Vertices[i], Vertices[i + resolution + 1]);
                for (int i = 1; i < resolution; i++) Edges[i + 3 * resolution - 1] = new Edge(Vertices[i], Vertices[i + resolution + 2]);
                Edges[4 * resolution - 1] = new Edge(Vertices[resolution], Vertices[resolution + 2]);

                Faces = new Face[4 * resolution];
                for (int i = 0; i < resolution; i++)
                {
                    Faces[i] = Prism_top_and_bottom.Faces[i];
                    Faces[i + resolution] = Prism_top_and_bottom.Faces[i];
                }
                for (int i = 1; i < resolution; i++)
                {
                    Faces[i + 2 * resolution - 1] = new Face(Vertices[i], Vertices[i + resolution + 1], Vertices[i + resolution + 2]);
                    Faces[i + 3 * resolution - 2] = new Face(Vertices[i], Vertices[i + resolution + 2], Vertices[i + 1]);
                }
                Faces[4 * resolution - 2] = new Face(Vertices[resolution], Vertices[2 * resolution + 1], Vertices[resolution + 2]);
                Faces[4 * resolution - 1] = new Face(Vertices[resolution], Vertices[resolution + 2], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Prism"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Prism"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Prism"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Prism"/>.</param>
        /// <param name="height">The height of the <see cref="Prism"/>.</param>
        /// <param name="radius">The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Prism"/>.</param>
        /// <param name="resolution">The number of points that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Prism"/>.</param>
        public Prism(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double height, double radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Height = height;
            Radius = radius;
            Resolution = resolution;
        }

        #endregion
    }
}