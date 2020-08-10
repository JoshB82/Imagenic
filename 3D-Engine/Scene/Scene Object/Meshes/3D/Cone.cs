using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Cone"/> mesh.
    /// </summary>
    public sealed class Cone : Mesh
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
        public int Resolution { get; set; }

        #endregion

        #region Constructors

        public Cone(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double height, double radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Height = height;
            Radius = radius;

            // Vertices are defined in anti-clockwise order, looking from above and then downwards.
            Vertices = new Vertex[resolution + 2];
            Vertices[0] = new Vertex(Vector4D.Zero);
            Vertices[1] = new Vertex(new Vector4D(0, 1, 0));
            
            double angle = 2 * Math.PI / resolution;
            for (int i = 0; i < resolution; i++) Vertices[i + 2] = new Vertex(new Vector4D(Math.Cos(angle * i), 0, Math.Sin(angle * i)));
            
            Edges = new Edge[2 * resolution];

            for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 2], Vertices[i + 3]);
            Edges[resolution - 1] = new Edge(Vertices[resolution + 1], Vertices[2]);

            Faces = new Face[2 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                Faces[i] = new Face(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                Faces[i + resolution] = new Face(Vertices[i + 2], Vertices[1], Vertices[i + 3]);
            }
            Faces[resolution - 1] = new Face(Vertices[resolution - 1], Vertices[0], Vertices[2]);
            Faces[2 * resolution - 1] = new Face(Vertices[resolution - 1], Vertices[1], Vertices[2]);
        }

        #endregion
    }
}