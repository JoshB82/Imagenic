using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using static System.MathF;
using _3D_Engine.SceneObjects.Meshes;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Ring"/> mesh.
    /// </summary>
    public sealed class Ring : Mesh
    {
        #region Fields and Properties

        private float innerRadius, outerRadius;
        private int resolution;

        /// <summary>
        /// The radius of the inner <see cref="Circle"/>.
        /// </summary>
        public float InnerRadius
        {
            get => innerRadius;
            set
            {
                innerRadius = value;
                if (resolution == 0) return;

                // Vertices are defined in anti-clockwise order.
                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i) * innerRadius, 0, Sin(angle * i) * innerRadius, 1));
            }
        }
        /// <summary>
        /// The radius of the outer <see cref="Circle"/>.
        /// </summary>
        public float OuterRadius
        {
            get => outerRadius;
            set
            {
                outerRadius = value;
                if (resolution == 0) return;

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + resolution + 1] = new Vertex(new Vector4D(Cos(angle * i) * outerRadius, 0, Sin(angle * i) * outerRadius, 1));
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
                Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    float sin = Sin(angle * i), cos = Cos(angle * i);
                    Vertices[i + 1] = new Vertex(new Vector4D(cos * innerRadius, 0, sin * innerRadius, 1));
                    Vertices[i + resolution + 1] = new Vertex(new Vector4D(cos * outerRadius, 0, sin * outerRadius, 1));
                }

                Edges = new Edge[2 * resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    Edges[i] = new Edge(Vertices[i + 1], Vertices[i + 2]);
                    Edges[i + resolution] = new Edge(Vertices[i + resolution + 1], Vertices[i + resolution + 2]);
                }
                Edges[resolution - 1] = new Edge(Vertices[resolution], Vertices[1]);
                Edges[2 * resolution - 1] = new Edge(Vertices[2 * resolution], Vertices[resolution + 1]);

                Faces = new SolidFace[2 * resolution];
                for (int i = 0; i < resolution - 1; i++)
                {
                    Faces[i] = new SolidFace(Vertices[i + 1], Vertices[i + resolution + 2], Vertices[i + resolution + 1]);
                    Faces[i + resolution] = new SolidFace(Vertices[i + 1], Vertices[i + 2], Vertices[i + resolution + 2]);
                }
                Faces[resolution - 1] = new SolidFace(Vertices[resolution], Vertices[resolution + 1], Vertices[2 * resolution]);
                Faces[2 * resolution - 1] = new SolidFace(Vertices[resolution], Vertices[1], Vertices[resolution + 1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Ring"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Ring"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Ring"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Ring"/>.</param>
        /// <param name="innerRadius">The radius of the inner <see cref="Circle"/>.</param>
        /// <param name="outerRadius">The radius of the outer <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Ring"/>.</param>
        public Ring(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float innerRadius, float outerRadius, int resolution) : base(origin, directionForward, directionUp)
        {
            Dimension = 2;

            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
            Resolution = resolution;
        }

        #endregion
    }
}