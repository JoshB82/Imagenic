using System.Diagnostics;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Cube"/> mesh.
    /// </summary>
    public sealed class Cube : Mesh
    {
        #region Fields and Properties

        private double side_length;

        /// <summary>
        /// The length of each side of the <see cref="Cube"/>.
        /// </summary>
        public double Side_Length
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
        /// <param name="direction">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        public Cube(Vector3D origin, Vector3D direction, Vector3D direction_up, double side_length)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

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

            Debug.WriteLine($"Cube created at {origin}");
        }

        /// <summary>
        /// Creates a textured <see cref="Cube"/> mesh, specifying a single texture for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cube"/>.</param>
        /// <param name="direction">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on the surface of the <see cref="Cube"/>.</param>
        public Cube(Vector3D origin, Vector3D direction, Vector3D direction_up, double side_length, Texture texture)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

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

            Debug.WriteLine($"Cube created at {origin}");
        }

        /// <summary>
        /// Creates a textured <see cref="Cube"/> mesh, specifying a texture for each side.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cube"/>.</param>
        /// <param name="direction">The direction the <see cref="Cube"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cube"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Cube"/>.</param>
        /// <param name="textures">The <see cref="Texture"/>s that define what to draw on the surface of the <see cref="Cube"/>. <br/><br/>The order of textures in this array is:
        /// <list type="table">
        /// <item><term>textures[0]</term>
        /// <description>Front</description></item>
        /// <item><term>textures[1]</term>
        /// <description>Right</description></item>
        /// <item><term>textures[2]</term>
        /// <description>Back</description></item>
        /// <item><term>textures[3]</term>
        /// <description>Left</description></item>
        /// <item><term>textures[4]</term>
        /// <description>Top</description></item>
        /// <item><term>textures[5]</term>
        /// <description>Bottom</description></item>
        /// </list></param>
        public Cube(Vector3D origin, Vector3D direction, Vector3D direction_up, double side_length, Texture[] textures)
        {
            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Set_Structure(side_length);
            Textures = textures;
            Faces = new Face[12]
            {
                new Face(Vertices[1], Vertices[6], Vertices[2], textures[0].Vertices[1], textures[0].Vertices[3], textures[0].Vertices[2], textures[0]), // 0
                new Face(Vertices[1], Vertices[5], Vertices[6], textures[0].Vertices[1], textures[0].Vertices[0], textures[0].Vertices[3], textures[0]), // 1
                new Face(Vertices[4], Vertices[7], Vertices[5], textures[1].Vertices[0], textures[1].Vertices[3], textures[1].Vertices[1], textures[1]), // 2
                new Face(Vertices[5], Vertices[7], Vertices[6], textures[1].Vertices[1], textures[1].Vertices[3], textures[1].Vertices[2], textures[1]), // 3
                new Face(Vertices[0], Vertices[3], Vertices[4], textures[2].Vertices[0], textures[2].Vertices[3], textures[2].Vertices[1], textures[2]), // 4
                new Face(Vertices[4], Vertices[3], Vertices[7], textures[2].Vertices[1], textures[2].Vertices[3], textures[2].Vertices[2], textures[2]), // 5
                new Face(Vertices[0], Vertices[1], Vertices[2], textures[3].Vertices[1], textures[3].Vertices[0], textures[3].Vertices[3], textures[3]), // 6
                new Face(Vertices[0], Vertices[2], Vertices[3], textures[3].Vertices[1], textures[3].Vertices[3], textures[3].Vertices[2], textures[3]), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6], textures[4].Vertices[0], textures[4].Vertices[3], textures[4].Vertices[1], textures[4]), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2], textures[4].Vertices[1], textures[4].Vertices[3], textures[4].Vertices[2], textures[4]), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1], textures[5].Vertices[3], textures[5].Vertices[2], textures[5].Vertices[1], textures[5]), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0], textures[5].Vertices[3], textures[5].Vertices[1], textures[5].Vertices[0], textures[5]) // 11
            };

            Debug.WriteLine($"Cube created at {origin}");
        }

        private void Set_Structure(double side_length)
        {
            Side_Length = side_length;

            Vertices = new Vector4D[8]
            {
                new Vector4D(0, 0, 0), // 0
                new Vector4D(1, 0, 0), // 1
                new Vector4D(1, 1, 0), // 2
                new Vector4D(0, 1, 0), // 3
                new Vector4D(0, 0, 1), // 4
                new Vector4D(1, 0, 1), // 5
                new Vector4D(1, 1, 1), // 6
                new Vector4D(0, 1, 1) // 7
            }; // need to be oriented to front side
            Spots = new Spot[8]
            {
                new Spot(Vertices[0]), // 0
                new Spot(Vertices[1]), // 1
                new Spot(Vertices[2]), // 2
                new Spot(Vertices[3]), // 3
                new Spot(Vertices[4]), // 4
                new Spot(Vertices[5]), // 5
                new Spot(Vertices[6]), // 6
                new Spot(Vertices[7]) // 7
            };
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