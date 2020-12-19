using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Cube"/> mesh.
    /// </summary>
    public sealed class Cube : Mesh
    {
        #region Fields and Properties

        private float side_length;

        /// <summary>
        /// The length of each side of the <see cref="Cube"/>.
        /// </summary>
        public float Side_Length
        {
            get => side_length;
            set
            {
                side_length = value;
                Scaling = new Vector3D(side_length, side_length, side_length);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cube"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cube"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        public Cube(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float side_length) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(side_length);
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
        /// Creates a textured <see cref="Cube"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cube"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cube"/>.</param>
        public Cube(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float side_length, Texture texture) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(side_length);
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
        /// Creates a textured <see cref="Cube"/> mesh, specifying a <see cref="Texture"/> for each side.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cube"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cube"/>.</param>
        /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cube"/>.</param>
        /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cube"/>.</param>
        /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cube"/>.</param>
        /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cube"/>.</param>
        /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cube"/>.</param>
        public Cube(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float side_length, Texture front, Texture right, Texture back, Texture left, Texture top, Texture bottom) : base(origin, direction_forward, direction_up)
        {
            Set_Structure(side_length);
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

        private void Set_Structure(float side_length)
        {
            Dimension = 3;
            Side_Length = side_length;

            Vertices = new Vertex[8]
            {
                new Vertex(new Vector4D(0, 0, 0, 1)), // 0
                new Vertex(new Vector4D(1, 0, 0, 1)), // 1
                new Vertex(new Vector4D(1, 1, 0, 1)), // 2
                new Vertex(new Vector4D(0, 1, 0, 1)), // 3
                new Vertex(new Vector4D(0, 0, 1, 1)), // 4
                new Vertex(new Vector4D(1, 0, 1, 1)), // 5
                new Vertex(new Vector4D(1, 1, 1, 1)), // 6
                new Vertex(new Vector4D(0, 1, 1, 1)) // 7
            }; // need to be oriented to front side

            Edges = new Edge[12]
            {
                new Edge(Vertices[0], Vertices[1]), // 0
                new Edge(Vertices[1], Vertices[2]), // 1
                new Edge(Vertices[2], Vertices[3]), // 2
                new Edge(Vertices[0], Vertices[3]), // 3
                new Edge(Vertices[1], Vertices[5]), // 4
                new Edge(Vertices[5], Vertices[6]), // 5
                new Edge(Vertices[2], Vertices[6]), // 6
                new Edge(Vertices[4], Vertices[5]), // 7
                new Edge(Vertices[4], Vertices[7]), // 8
                new Edge(Vertices[6], Vertices[7]), // 9
                new Edge(Vertices[0], Vertices[4]), // 10
                new Edge(Vertices[3], Vertices[7]), // 11
            };
        }

        #endregion

        #region Casting

        /// <summary>
        /// Casts a <see cref="Cube"/> into a <see cref="Cuboid"/>.
        /// </summary>
        /// <param name="cube"><see cref="Cube"/> to cast.</param>
        public static explicit operator Cuboid(Cube cube) =>
            new Cuboid(cube.WorldOrigin, cube.WorldDirectionForward, cube.WorldDirectionUp, cube.side_length, cube.side_length, cube.side_length)
            {
                Textures = cube.Textures,
                Faces = cube.Faces
            };

        #endregion
    }
}