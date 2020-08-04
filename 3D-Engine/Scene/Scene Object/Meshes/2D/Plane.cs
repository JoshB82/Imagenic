namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Plane"/> mesh.
    /// </summary>
    public sealed class Plane : Mesh
    {
        #region Fields and Properties

        private double length, width;

        /// <summary>
        /// The length of the <see cref="Plane"/>.
        /// </summary>
        public double Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }
        /// <summary>
        /// The width of the <see cref="Plane"/>.
        /// </summary>
        public double Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Plane"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Plane"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Plane"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Plane"/>. This is also a normal to the surface of the <see cref="Plane"/>.</param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        public Plane(Vector3D origin, Vector3D direction_forward, Vector3D normal, double length, double width) : base(origin, direction_forward, normal)
        {
            Set_Structure(length, width);
            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2]), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3]) // 1
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Plane"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Plane"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Plane"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Plane"/>. This is also a normal to the surface of the <see cref="Plane"/>.</param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Plane"/>.</param>
        public Plane(Vector3D origin, Vector3D direction_forward, Vector3D normal, double length, double width, Texture texture) : base(origin, direction_forward, normal)
        {
            Set_Structure(length, width);
            Textures = new Texture[1] { texture };
            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], texture.Vertices[0], texture.Vertices[1], texture.Vertices[2], texture), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3], texture.Vertices[0], texture.Vertices[2], texture.Vertices[3], texture) // 1
            };
        }

        private void Set_Structure(double length, double width)
        {
            Length = length;
            Width = width;

            Vertices = new Vertex[4]
            {
                new Vertex(new Vector4D(0, 0, 0)), // 0
                new Vertex(new Vector4D(1, 0, 0)), // 1
                new Vertex(new Vector4D(1, 0, 1)), // 2
                new Vertex(new Vector4D(0, 0, 1)) // 3
            };

            Edges = new Edge[5]
            {
                new Edge(Vertices[0], Vertices[1]), // 0
                new Edge(Vertices[1], Vertices[2]), // 1
                new Edge(Vertices[0], Vertices[2]) { Visible = false }, // 2
                new Edge(Vertices[2], Vertices[3]), // 3
                new Edge(Vertices[0], Vertices[3]) // 4
            };
        }

        #endregion
    }
}