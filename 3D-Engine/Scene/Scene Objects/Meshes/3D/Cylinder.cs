using System;
using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Cylinder"/> mesh.
    /// </summary>
    public sealed class Cylinder : Mesh
    {
        #region Fields and Properties

        private float height, radius;
        private int resolution;

        /// <summary>
        /// The height of the <see cref="Cylinder"/>.
        /// </summary>
        public float Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The radius of the top and bottom <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, height, radius);
            }
        }
        /// <summary>
        /// The number of vertices that are on the perimeter of each of the <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                Vertices = new Vertex[2 * resolution + 2];
                Vertices[0] = new Vertex(Vector4D.Zero);
                Vertices[1] = new Vertex(Vector4D.Unit_Y);

                float angle = 2 * (float)Math.PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    Vertices[i + 2] = new Vertex(new Vector4D((float)Math.Cos(angle * i), 0, (float)Math.Sin(angle * i)));
                    Vertices[i + resolution + 2] = new Vertex(new Vector4D((float)Math.Cos(angle * i), 1, (float)Math.Sin(angle * i)));
                }

                Edges = new Edge[2 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Edges[i] = new Edge(Vertices[i + 2], Vertices[i + 3]);
                    Edges[i + resolution] = new Edge(Vertices[i + resolution + 2], Vertices[i + resolution + 3]);
                }
                Edges[resolution - 1] = new Edge(Vertices[resolution + 1], Vertices[2]);
                Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution + 1], Vertices[resolution + 2]);

                Faces = new Face[4 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i] = new Face(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                    Faces[i + resolution] = new Face(Vertices[i + 2], Vertices[i + resolution + 3], Vertices[i + resolution + 2]);
                    Faces[i + 2 * resolution] = new Face(Vertices[i + 2], Vertices[i + 3], Vertices[i + resolution + 3]);
                    Faces[i + 3 * resolution] = new Face(Vertices[i + resolution + 2], Vertices[i + resolution + 3], Vertices[1]);
                }
                Faces[resolution - 1] = new Face(Vertices[resolution + 1], Vertices[0], Vertices[2]);
                Faces[2 * resolution - 1] = new Face(Vertices[resolution + 1], Vertices[resolution + 2], Vertices[2 * resolution + 1]);
                Faces[3 * resolution - 1] = new Face(Vertices[resolution + 1], Vertices[2], Vertices[resolution + 2]);
                Faces[4 * resolution - 1] = new Face(Vertices[2 * resolution + 1], Vertices[resolution + 2], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cylinder"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cylinder"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cylinder"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cylinder"/>.</param>
        /// <param name="height">The height of the <see cref="Cylinder"/>.</param>
        /// <param name="radius">The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
        public Cylinder(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float height, float radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;

            Height = height;
            Radius = radius;
            Resolution = resolution;
        }

        #endregion
    }
}