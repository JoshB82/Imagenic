using static System.MathF;

namespace _3D_Engine
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Circle"/> mesh.
    /// </summary>
    public sealed class Circle : Mesh
    {
        #region Fields and Properties

        private float radius;
        private int resolution;

        /// <summary>
        /// The radius of the <see cref="Circle"/>.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, 1, radius);
            }
        }
        /// <summary>
        /// The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the <see cref="Circle"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                // Vertices are defined in anti-clockwise order.
                Vertices = new Vertex[resolution + 1]; // ?
                Vertices[0] = new Vertex(Vector4D.Zero); // ?

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));

                if (Textures is not null)
                {
                    Textures[0].Vertices = new Vector3D[resolution + 1];
                    Textures[0].Vertices[0] = new Vector3D(0.5f, 0.5f, 1);

                    for (int i = 0; i < resolution; i++) Textures[0].Vertices[i + 1] = new Vector3D(Cos(angle * i) * 0.5f, Sin(angle * i) * 0.5f, 1);
                }

                Edges = new Edge[resolution];
                for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 1], Vertices[i + 2]);
                Edges[resolution - 1] = new Edge(Vertices[resolution], Vertices[1]);

                Faces = new Face[resolution];
                for (int i = 0; i < resolution - 1; i++) Faces[i] = new Face(Vertices[i + 1], Vertices[0], Vertices[i + 2]);
                Faces[resolution - 1] = new Face(Vertices[resolution], Vertices[0], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Circle"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Circle"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Circle"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Circle"/>. This is also a normal to the surface of the <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
        public Circle(Vector3D origin, Vector3D direction_forward, Vector3D normal, float radius, int resolution) : base(origin, direction_forward, normal)
        {
            Dimension = 2;

            Radius = radius;
            Resolution = resolution;
        }

        /// <summary>
        /// Creates a textured <see cref="Circle"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Circle"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Circle"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Circle"/>. This is also a normal to the surface of the <see cref="Circle"/>.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on the surface of the <see cref="Circle"/>.</param>
        public Circle(Vector3D origin, Vector3D direction_forward, Vector3D normal, float radius, int resolution, Texture texture) : base(origin, direction_forward, normal)
        {
            Dimension = 2;

            Radius = radius;
            Textures = new Texture[1] { texture };
            Resolution = resolution;
        }

        #endregion
    }
}