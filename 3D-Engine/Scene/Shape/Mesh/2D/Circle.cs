using System;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Graphics
{
    public sealed class Circle : Mesh
    {
        private double radius;
        public double Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, 1, radius);
            }
        }
        public int Resolution { get; set; }

        public Circle(Vector3D origin, Vector3D direction, Vector3D normal, double radius, int resolution)
        {
            Radius = radius;
            Resolution = resolution;

            World_Origin = new Vector4D(origin);
            Set_Shape_Direction_1(direction, normal);

            Model_Vertices = new Vector4D[resolution + 1] { new Vector4D(0, 0, 0) };

            double angle = 2 * Math.PI / resolution;
            double sin_angle = Math.Sin(angle), cos_angle = Math.Cos(angle);

            for (int i = 0; i < resolution; i++) Model_Vertices[i + 1] = new Vector4D(cos_angle * i, 0, sin_angle * i);

            Spots = new Spot[1]
            {
                new Spot(Model_Vertices[0])
            };

            Edges = new Edge[resolution];
            for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Model_Vertices[i + 1], Model_Vertices[i + 2]);
            Edges[resolution - 1] = new Edge(Model_Vertices[resolution], Model_Vertices[1]);

            Faces = new Face[resolution];
            for (int i = 0; i < resolution - 1; i++) Faces[i] = new Face(Model_Vertices[i + 1], Model_Vertices[0], Model_Vertices[i + 2]);
            Faces[resolution - 1] = new Face(Model_Vertices[resolution], Model_Vertices[0], Model_Vertices[1]);

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;
            Face_Colour = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Debug.WriteLine($"Circle created at {origin}");
        }

        public Circle(Vector3D origin, Vector3D direction, Vector3D normal, double radius, int resolution, Bitmap texture)
        {
            Radius = radius;
            Resolution = resolution;

            World_Origin = new Vector4D(origin);
            Set_Shape_Direction_1(direction, normal);

            Model_Vertices = new Vector4D[resolution + 1] { new Vector4D(0, 0, 0) };

            double angle = 2 * Math.PI / resolution;
            double sin_angle = Math.Sin(angle), cos_angle = Math.Cos(angle);

            for (int i = 0; i < resolution; i++) Model_Vertices[i + 1] = new Vector4D(cos_angle * i, 0, sin_angle * i);

            //Texture_Vertices = new Vector3D[resolution + 1]-
            //{ new Vector3D(0.5, 0.5, 1) };

            for (int i = 0; i < resolution; i++) Model_Vertices[i + 1] = new Vector4D(cos_angle * i, 0, sin_angle * i);

            Spots = new Spot[1]
            {
                new Spot(Model_Vertices[0])
            };

            Edges = new Edge[resolution];
            for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Model_Vertices[i + 1], Model_Vertices[i + 2]);
            Edges[resolution - 1] = new Edge(Model_Vertices[resolution], Model_Vertices[1]);

            Faces = new Face[resolution];
            for (int i = 0; i < resolution - 1; i++) Faces[i] = new Face(Model_Vertices[i + 1], Model_Vertices[0], Model_Vertices[i + 2]);
            Faces[resolution - 1] = new Face(Model_Vertices[resolution], Model_Vertices[0], Model_Vertices[1]);

            Spot_Colour = Color.Blue;
            Edge_Colour = Color.Black;
            Face_Colour = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00); // Green

            Debug.WriteLine($"Circle created at {origin}");
        }
    }
}