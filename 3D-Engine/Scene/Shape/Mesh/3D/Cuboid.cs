using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a cuboid mesh.
    /// </summary>
    public sealed class Cuboid : Mesh
    {
        #region Fields and Properties

        private double length, width, height;
        public double Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        public double Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
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

        public Cuboid(Vector3D origin, Vector3D direction, Vector3D direction_up, double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

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
            };

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
                new Edge(Vertices[1], Vertices[4]) { Visible = false }// 17
            };

            Faces = new Face[12]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2]), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3]), // 1
                new Face(Vertices[1], Vertices[6], Vertices[2]), // 2
                new Face(Vertices[1], Vertices[5], Vertices[6]), // 3
                new Face(Vertices[4], Vertices[7], Vertices[5]), // 4
                new Face(Vertices[5], Vertices[7], Vertices[6]), // 5
                new Face(Vertices[0], Vertices[3], Vertices[4]), // 6
                new Face(Vertices[4], Vertices[3], Vertices[7]), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6]), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2]), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1]), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0]) // 11
            };

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;
            Face_Colour = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Debug.WriteLine($"Cuboid created at {origin}");
        }

        public Cuboid(Vector3D origin, Vector3D direction, Vector3D direction_up, double length, double width, double height, Texture texture)
        {
            Length = length;
            Width = width;
            Height = height;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

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
            };

            Texture_Vertices = new Vector3D[4]
            {
                new Vector3D(0, 0, 1), // 0
                new Vector3D(1, 0, 1), // 1
                new Vector3D(1, 1, 1), // 2
                new Vector3D(0, 1, 1) // 3
            };

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
                new Edge(Vertices[1], Vertices[4]) { Visible = false }// 17
            };

            Faces = new Face[12]
            {
                new Face(Vertices[0], Vertices[1], Vertices[2], Texture_Vertices[1], Texture_Vertices[0], Texture_Vertices[3], texture), // 0
                new Face(Vertices[0], Vertices[2], Vertices[3], Texture_Vertices[1], Texture_Vertices[3], Texture_Vertices[2], texture), // 1
                new Face(Vertices[1], Vertices[6], Vertices[2], Texture_Vertices[1], Texture_Vertices[3], Texture_Vertices[2], texture), // 2
                new Face(Vertices[1], Vertices[5], Vertices[6], Texture_Vertices[1], Texture_Vertices[0], Texture_Vertices[3], texture), // 3
                new Face(Vertices[4], Vertices[7], Vertices[5], Texture_Vertices[0], Texture_Vertices[3], Texture_Vertices[1], texture), // 4
                new Face(Vertices[5], Vertices[7], Vertices[6], Texture_Vertices[1], Texture_Vertices[3], Texture_Vertices[2], texture), // 5
                new Face(Vertices[0], Vertices[3], Vertices[4], Texture_Vertices[0], Texture_Vertices[3], Texture_Vertices[1], texture), // 6
                new Face(Vertices[4], Vertices[3], Vertices[7], Texture_Vertices[1], Texture_Vertices[3], Texture_Vertices[2], texture), // 7
                new Face(Vertices[7], Vertices[3], Vertices[6], Texture_Vertices[0], Texture_Vertices[3], Texture_Vertices[1], texture), // 8
                new Face(Vertices[6], Vertices[3], Vertices[2], Texture_Vertices[1], Texture_Vertices[3], Texture_Vertices[2], texture), // 9
                new Face(Vertices[4], Vertices[5], Vertices[1], Texture_Vertices[3], Texture_Vertices[2], Texture_Vertices[1], texture), // 10
                new Face(Vertices[4], Vertices[1], Vertices[0], Texture_Vertices[3], Texture_Vertices[1], Texture_Vertices[0], texture) // 11
            };

            Textures = new Texture[1]
            {
                texture // 0
            };

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;

            Debug.WriteLine($"Cuboid created at {origin}");
        }

        #endregion
    }
}