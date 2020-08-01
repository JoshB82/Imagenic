using System.Diagnostics;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Square"/> mesh.
    /// </summary>
    public sealed class Square : Mesh
    {
        #region Fields and Properties

        private double side_length;

        /// <summary>
        /// The length of each side of the <see cref="Square"/>.
        /// </summary>
        public double Side_Length
        {
            get => side_length;
            set
            {
                side_length = value;
                Scaling = new Vector3D(side_length, 1, side_length);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Square"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Square"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Square"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Square"/>. This is also a normal to the surface of the <see cref="Square"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Square"/>.</param>
        public Square(Vector3D origin, Vector3D direction_forward, Vector3D normal, double side_length) : base(origin, direction_forward, normal)
        {
            Set_Structure(side_length);
            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2]), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3]) // 1
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Square"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Square"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Square"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Square"/>. This is also a normal to the surface of the <see cref="Square"/>.</param>
        /// <param name="side_length">The length of each side of the <see cref="Square"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Square"/>.</param>
        public Square(Vector3D origin, Vector3D direction_forward, Vector3D normal, double side_length, Texture texture) : base(origin, direction_forward, normal)
        {
            Set_Structure(side_length);
            Textures = new Texture[1] { texture };
            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], texture.Vertices[0], texture.Vertices[1], texture.Vertices[2], texture), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3], texture.Vertices[0], texture.Vertices[2], texture.Vertices[3], texture) // 1
            };
        }

        private void Set_Structure(double side_length)
        {
            Side_Length = side_length;

            Vertices = new Vector4D[4]
            {
                new Vector4D(0, 0, 0), // 0
                new Vector4D(1, 0, 0), // 1
                new Vector4D(1, 0, 1), // 2
                new Vector4D(0, 0, 1) // 3
            };
            Spots = new Spot[4]
            {
                new Spot(Vertices[0]), // 0
                new Spot(Vertices[1]), // 1
                new Spot(Vertices[2]), // 2
                new Spot(Vertices[3]) // 3
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

        #region Casting

        /// <summary>
        /// Casts the <see cref="Square"/> into a <see cref="Plane"/>.
        /// </summary>
        /// <param name="square"><see cref="Square"/> to cast.</param>
        public static explicit operator Plane(Square square)
        {
            Plane plane_cast = new Plane(square.World_Origin, square.World_Direction_Forward, square.World_Direction_Up, square.side_length, square.side_length);
            plane_cast.Textures = square.Textures;
            plane_cast.Faces = square.Faces;
            return plane_cast;
        }

        #endregion
    }
}