using System;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Ring"/> mesh.
    /// </summary>
    public sealed class Ring : Mesh
    {
        #region Fields and Properties

        private float inner_radius, outer_radius;
        private int resolution;

        /// <summary>
        /// The radius of the inner <see cref="Circle"/>.
        /// </summary>
        public float Inner_Radius
        {
            get => inner_radius;
            set
            {
                inner_radius = value;
                if (resolution == 0) return;

                // Vertices are defined in anti-clockwise order.
                float angle = 2 * (float)Math.PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D((float)Math.Cos(angle * i) * inner_radius, 0, (float)Math.Sin(angle * i) * inner_radius));
            }
        }
        /// <summary>
        /// The radius of the outer <see cref="Circle"/>.
        /// </summary>
        public float Outer_Radius
        {
            get => outer_radius;
            set
            {
                outer_radius = value;
                if (resolution == 0) return;

                float angle = 2 * (float)Math.PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + resolution + 1] = new Vertex(new Vector4D((float)Math.Cos(angle * i) * outer_radius, 0, (float)Math.Sin(angle * i) * outer_radius));
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

                float angle = 2 * (float)Math.PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    float sin = (float)Math.Sin(angle * i), cos = (float)Math.Cos(angle * i);
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
                    Faces[i] = new Face(Vertices[i + 1], Vertices[i + resolution + 2], Vertices[i + resolution + 1]);
                    Faces[i + resolution] = new Face(Vertices[i + 1], Vertices[i + 2], Vertices[i + resolution + 2]);
                }
                Faces[resolution - 1] = new Face(Vertices[resolution], Vertices[resolution + 1], Vertices[2 * resolution]);
                Faces[2 * resolution - 1] = new Face(Vertices[resolution], Vertices[1], Vertices[resolution + 1]);
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
        public Ring(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float inner_radius, float outer_radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Dimension = 2;

            Inner_Radius = inner_radius;
            Outer_Radius = outer_radius;
            Resolution = resolution;
        }

        #endregion
    }
}