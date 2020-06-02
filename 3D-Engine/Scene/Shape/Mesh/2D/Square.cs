using System.Diagnostics;
using System.Drawing;

namespace _3D_Graphics
{
    /// <summary>
    /// Handles creation of a square mesh.
    /// </summary>
    public sealed class Square : Mesh
    {
        #region Fields and Properties

        private double side_length;
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

        public Square(Vector3D origin, Vector3D direction, Vector3D normal, double side_length)
        {
            Side_Length = side_length;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, normal);

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

            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2]), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3]) // 1
            };

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;
            Face_Colour = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Debug.WriteLine($"Square created at {origin}");
        }

        public Square(Vector3D origin, Vector3D direction, Vector3D normal, double side_length, Texture texture)
        {
            Side_Length = side_length;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, normal);

            Vertices = new Vector4D[4]
            {
                new Vector4D(0, 0, 0), // 0
                new Vector4D(1, 0, 0), // 1
                new Vector4D(1, 0, 1), // 2
                new Vector4D(0, 0, 1) // 3
            };

            Texture_Vertices = new Vector3D[4]
            {
                // WHY Z=1?
                new Vector3D(0, 0, 1), // 0
                new Vector3D(1, 0, 1), // 1
                new Vector3D(1, 1, 1), // 2
                new Vector3D(0, 1, 1) // 3
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

            Faces = new Face[2]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], Texture_Vertices[0], Texture_Vertices[1], Texture_Vertices[2], texture), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3], Texture_Vertices[0], Texture_Vertices[2], Texture_Vertices[3], texture) // 1
            };

            Textures = new Texture[1]
            {
                texture // 0
            };

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;

            Debug.WriteLine($"Square created at {origin}");
        }

        #endregion
    }
}