using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public sealed class Cone : Mesh
    {
        private double height, radius;
        public double Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }

        public Cone(Vector3D origin, Vector3D direction, Vector3D direction_up, double height, double radius, int resolution)
        {
            Height = height;
            Radius = radius;

            World_Origin = new Vector4D(origin);
            Set_Shape_Direction_1(direction, direction_up);

            Circle cone_base = new Circle(origin, direction, direction_up, radius, resolution);

            Model_Vertices = new Vector4D[resolution + 2];
            Model_Vertices[resolution + 1] = new Vector4D(0, 1, 0);
            for (int i = 0; i <= resolution; i++) Model_Vertices[i] = cone_base.Model_Vertices[i];

            Spots = new Spot[2]
            {
                cone_base.Spots[0],
                new Spot()
            };

            Edges = new Edge[2 * resolution];
            for (int i = 0; i < resolution; i++)
            {
                Edges[i] = cone_base.Edges[i];
                Edges[i + resolution] = new Edge(Model_Vertices[i + 1], Model_Vertices[resolution + 1]);
            }

            Faces = new Face[2 * resolution];
            for (int i = 0; i < resolution; i++) Faces[i] = cone_base.Faces[i];
            for (int i = resolution; i < 2 * resolution - 1; i++) Faces[i] = new Face(Model_Vertices[i - resolution + 1], Model_Vertices[resolution + 1], Model_Vertices[i - resolution + 2]);
            Faces[2 * resolution - 1] = new Face(Model_Vertices[resolution], Model_Vertices[resolution + 1], Model_Vertices[1]);

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;
            Face_Colour = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Debug.WriteLine($"Cone created at {origin}");
        }

        public Cone(Vector3D origin, Vector3D direction, Vector3D direction_up, double height, double radius, int resolution, Bitmap texture)
        {

        }
    }
}