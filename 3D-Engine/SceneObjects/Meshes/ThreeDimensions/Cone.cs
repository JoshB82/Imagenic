﻿using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.SceneObjects.Meshes.ThreeDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Cone"/> mesh.
    /// </summary>
    public sealed class Cone : Mesh
    {
        #region Fields and Properties

        private float height, radius;
        private int resolution;

        /// <summary>
        /// The height of the <see cref="Cone"/>.
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
        /// The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.
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
        /// The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                resolution = value;

                // Vertices
                // They are defined in anti-clockwise order, looking from above and then downwards.
                Vertices = new Vertex[resolution + 2];
                Vertices[0] = new Vertex(Vector4D.Zero);
                Vertices[1] = new Vertex(Vector4D.UnitY);

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));

                // Edges
                Edges = new Edge[resolution];

                for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 2], Vertices[i + 3]);
                Edges[resolution - 1] = new Edge(Vertices[resolution + 1], Vertices[2]);

                // Faces
                Faces = new Face[2 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i] = new Face(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                    Faces[i + resolution] = new Face(Vertices[i + 2], Vertices[i + 3], Vertices[1]);
                }
                Faces[resolution - 1] = new Face(Vertices[resolution - 1], Vertices[0], Vertices[2]);
                Faces[2 * resolution - 1] = new Face(Vertices[resolution - 1], Vertices[2], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cone"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cone"/>.</param>
        /// <param name="direction_forward">The direction the <see cref="Cone"/> faces.</param>
        /// <param name="direction_up">The upward orientation of the <see cref="Cone"/>.</param>
        /// <param name="height">The height of the <see cref="Cone"/>.</param>
        /// <param name="radius">The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        /// <param name="resolution">The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        public Cone(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, float height, float radius, int resolution) : base(origin, direction_forward, direction_up)
        {
            Dimension = 3;

            Height = height;
            Radius = radius;
            Resolution = resolution;
        }
        
        #endregion
    }
}