using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Ring"/> mesh.
    /// </summary>
    public sealed class Ring : Mesh
    {
        #region Fields and Properties

        private double inner_radius, outer_radius;
        private int resolution;

        /// <summary>
        /// The radius of the inner <see cref="Circle"/>.
        /// </summary>
        public double Inner_Radius
        {
            get => inner_radius;
            set
            {
                inner_radius = value;
                if (resolution == 0) return;

                // Vertices are defined in anti-clockwise order.
                double angle = 2 * Math.PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Math.Cos(angle * i) * inner_radius, 0, Math.Sin(angle * i) * inner_radius));
            }
        }
        /// <summary>
        /// The radius of the outer <see cref="Circle"/>.
        /// </summary>
        public double Outer_Radius
        {
            get => outer_radius;
            set
            {
                outer_radius = value;
                if (resolution == 0) return;

                double angle = 2 * Math.PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + resolution + 1] = new Vertex(new Vector4D(Math.Cos(angle * i) * outer_radius, 0, Math.Sin(angle * i) * outer_radius));
            }
        }
        /// <summary>
        /// The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Ring"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                Vertices = new Vertex[2 * resolution + 1];
                Vertices[0] = new Vertex(Vector4D.Zero);

                double angle = 2 * Math.PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    double sin = Math.Sin(angle * i), cos = Math.Cos(angle * i);
                    Vertices[i + 1] = new Vertex(new Vector4D(cos * inner_radius, 0, sin * inner_radius));
                    Vertices[i + resolution + 1] = new Vertex(new Vector4D(cos * outer_radius, 0, sin * outer_radius));
                }

                Edges = new Edge[2 * resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    Edges[i] = new Edge(Vertices[i + 1], Vertices[i + 2]);
                    Edges[i + resolution] = new Edge(Vertices[i + resolution + 1], Vertices[i + resolution + 2]);
                }
                Edges[resolution - 1] = new Edge(Vertices[resolution], Vertices[1]);
                Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution], Vertices[resolution + 1]);

                Faces = new Face[2 * resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i] = new Face(Vertices[i], Vertices[resolution + 1], Vertices[resolution]);
                    Faces[i + resolution] = new Face(Vertices[i], Vertices[i + 1], Vertices[resolution + 1]);
                }
                Faces[resolution - 1] = new Face(Vertices[resolution - 1], Vertices[resolution], Vertices[2 * resolution - 1]);
                Faces[2 * resolution - 1] = new Face(Vertices[resolution - 1], Vertices[0], Vertices[resolution]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Ring"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Ring"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Ring"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Ring"/>.</param>
        /// <param name="inner_radius">The radius of the inner <see cref="Circle"/>.</param>
        /// <param name="outer_radius">The radius of the outer <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Ring"/>.</param>
        public Ring(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, double inner_radius, double outer_radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Inner_Radius = inner_radius;
            Outer_Radius = outer_radius;
            Resolution = resolution;
        }

        #endregion
    }
}