namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Cuboid"/> mesh.
    /// </summary>
    public sealed class Cuboid : Mesh
    {
        #region Fields and Properties

        private double length, width, height;

        /// <summary>
        /// The length of the <see cref="Cuboid"/>.
        /// </summary>
        public double Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        /// <summary>
        /// The width of the <see cref="Cuboid"/>.
        /// </summary>
        public double Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        /// <summary>
        /// The height of the <see cref="Cuboid"/>.
        /// </summary>
        public double Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(length, width, height);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cuboid"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cuboid"/>.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double length, double width, double height) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(length, width, height);
            Faces = new Face[12]
            {
                new Face(Vertices[1], Vertices[6], Vertices[2]), // 0
                new Face(Vertices[1], Vertices[5], Vertices[6]), // 1
                new Face(Vertices[4], Vertices[7], Vertices[5]), // 2
                new Face(Vertices[5], Vertices[7], Vertices[6]), // 3
                new Face(Vertices[0], Vertices[3], Vertices[4]), // 4
                new Face(Vertices[4], Vertices[3], Vertices[7]), // 5
                new Face(Vertices[0], Vertices[1], Vertices[2]), // 6
                new Face(Vertices[0], Vertices[2], Vertices[3]), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6]), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2]), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1]), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0]) // 11
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cuboid"/>.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double length, double width, double height, Texture texture) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(length, width, height);
            Textures = new Texture[1] { texture };
            Faces = new Face[12]
            {
                new Face(Vertices[1], Vertices[6], Vertices[2], texture.Vertices[1], texture.Vertices[3], texture.Vertices[2], texture), // 0
                new Face(Vertices[1], Vertices[5], Vertices[6], texture.Vertices[1], texture.Vertices[0], texture.Vertices[3], texture), // 1
                new Face(Vertices[4], Vertices[7], Vertices[5], texture.Vertices[0], texture.Vertices[3], texture.Vertices[1], texture), // 2
                new Face(Vertices[5], Vertices[7], Vertices[6], texture.Vertices[1], texture.Vertices[3], texture.Vertices[2], texture), // 3
                new Face(Vertices[0], Vertices[3], Vertices[4], texture.Vertices[0], texture.Vertices[3], texture.Vertices[1], texture), // 4
                new Face(Vertices[4], Vertices[3], Vertices[7], texture.Vertices[1], texture.Vertices[3], texture.Vertices[2], texture), // 5
                new Face(Vertices[0], Vertices[1], Vertices[2], texture.Vertices[1], texture.Vertices[0], texture.Vertices[3], texture), // 6
                new Face(Vertices[0], Vertices[2], Vertices[3], texture.Vertices[1], texture.Vertices[3], texture.Vertices[2], texture), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6], texture.Vertices[0], texture.Vertices[3], texture.Vertices[1], texture), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2], texture.Vertices[1], texture.Vertices[3], texture.Vertices[2], texture), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1], texture.Vertices[3], texture.Vertices[2], texture.Vertices[1], texture), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0], texture.Vertices[3], texture.Vertices[1], texture.Vertices[0], texture) // 11
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a <see cref="Texture"/> for each side.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cuboid"/>.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cuboid"/>.</param>
        /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cuboid"/>.</param>
        /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cuboid"/>.</param>
        /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cuboid"/>.</param>
        /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cuboid"/>.</param>
        /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double length, double width, double height, Texture front, Texture right, Texture back, Texture left, Texture top, Texture bottom) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(length, width, height);
            Textures = new Texture[6]
            {
                front,
                right,
                back,
                left,
                top,
                bottom
            };
            Faces = new Face[12]
            {
                new Face(Vertices[1], Vertices[6], Vertices[2], front.Vertices[1], front.Vertices[3], front.Vertices[2], front), // 0
                new Face(Vertices[1], Vertices[5], Vertices[6], front.Vertices[1], front.Vertices[0], front.Vertices[3], front), // 1
                new Face(Vertices[4], Vertices[7], Vertices[5], right.Vertices[0], right.Vertices[3], right.Vertices[1], right), // 2
                new Face(Vertices[5], Vertices[7], Vertices[6], right.Vertices[1], right.Vertices[3], right.Vertices[2], right), // 3
                new Face(Vertices[0], Vertices[3], Vertices[4], back.Vertices[0], back.Vertices[3], back.Vertices[1], back), // 4
                new Face(Vertices[4], Vertices[3], Vertices[7], back.Vertices[1], back.Vertices[3], back.Vertices[2], back), // 5
                new Face(Vertices[0], Vertices[1], Vertices[2], left.Vertices[1], left.Vertices[0], left.Vertices[3], left), // 6
                new Face(Vertices[0], Vertices[2], Vertices[3], left.Vertices[1], left.Vertices[3], left.Vertices[2], left), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6], top.Vertices[0], top.Vertices[3], top.Vertices[1], top), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2], top.Vertices[1], top.Vertices[3], top.Vertices[2], top), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1], bottom.Vertices[3], bottom.Vertices[2], bottom.Vertices[1], bottom), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0], bottom.Vertices[3], bottom.Vertices[1], bottom.Vertices[0], bottom) // 11
            };
        }

        private void Set_Structure(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;

            Vertices = new Vertex[8]
            {
                new Vertex(new Vector4D(0, 0, 0)), // 0
                new Vertex(new Vector4D(1, 0, 0)), // 1
                new Vertex(new Vector4D(1, 1, 0)), // 2
                new Vertex(new Vector4D(0, 1, 0)), // 3
                new Vertex(new Vector4D(0, 0, 1)), // 4
                new Vertex(new Vector4D(1, 0, 1)), // 5
                new Vertex(new Vector4D(1, 1, 1)), // 6
                new Vertex(new Vector4D(0, 1, 1)) // 7
            }; // need to be oriented to front side

            Edges = new Edge[18]
            {
                new Edge(Vertices[0], Vertices[1]), // 0
                new Edge(Vertices[1], Vertices[2]), // 1
                new Edge(Vertices[0], Vertices[2]) { Visible = false }, // 2
                new Edge(Vertices[2], Vertices[3]), // 3
                new Edge(Vertices[0], Vertices[3]), // 4
                new Edge(Vertices[1], Vertices[5]), // 5
                new Edge(Vertices[5], Vertices[6]), // 6
                new Edge(Vertices[1], Vertices[6]) { Visible = false }, // 7
                new Edge(Vertices[2], Vertices[6]), // 8
                new Edge(Vertices[4], Vertices[5]), // 9
                new Edge(Vertices[4], Vertices[7]), // 10
                new Edge(Vertices[5], Vertices[7]) { Visible = false }, // 11
                new Edge(Vertices[6], Vertices[7]), // 12
                new Edge(Vertices[0], Vertices[4]), // 13
                new Edge(Vertices[3], Vertices[4]) { Visible = false }, // 14
                new Edge(Vertices[3], Vertices[7]), // 15
                new Edge(Vertices[3], Vertices[6]) { Visible = false }, // 16
                new Edge(Vertices[1], Vertices[4]) { Visible = false } // 17
            };
        }

        #endregion
    }
}