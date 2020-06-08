using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a pyramid mesh.
    /// </summary>
    public sealed class Pyramid : Mesh
    {
        #region Fields and Properties

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
        public int Resolution { get; set; };

        #endregion

        #region Constructors

        public Pyramid(Vector3D origin, Vector3D direction, Vector3D direction_up, double height, double radius, int resolution)
        {
            Height = height;
            Radius = radius;

            World_Origin = origin;
            Set_Shape_Direction_1(direction, direction_up);

            Circle pyramid_base = new Circle(origin, direction, direction_up, radius, resolution);

            Vertices = new Vector4D[resolution + 2];
            Vertices[resolution + 1] = new Vector4D(0, 1, 0);
            for (int i = 0; i <= resolution; i++) Vertices[i] = pyramid_base.Vertices[i];

            Spots = new Spot[2]
            {
                pyramid_base.Spots[0],
                new Spot()
            };

            Edges = new Edge[2 * resolution];
            for (int i = 0; i < resolution; i++)
            {
                Edges[i] = pyramid_base.Edges[i];
                Edges[i + resolution] = new Edge(Vertices[i + 1], Vertices[resolution + 1]);
            }

            Faces = new Face[2 * resolution];
            for (int i = 0; i < resolution; i++) Faces[i] = pyramid_base.Faces[i];
            for (int i = resolution; i < 2 * resolution - 1; i++) Faces[i] = new Face(Vertices[i - resolution + 1], Vertices[resolution + 1], Vertices[i - resolution + 2]);
            Faces[2 * resolution - 1] = new Face(Vertices[resolution], Vertices[resolution + 1], Vertices[1]);

            Debug.WriteLine($"Pyramid created at {origin}");
        }

        /*
        public Pyramid(Vector3D origin, Vector3D direction, Vector3D direction_up, double height, double radius, int resolution, Bitmap texture)
        {

        }
        */

        #endregion
    }
}