using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions
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
                Vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));
                Vertices[1] = new Vertex(new Vector4D(0, 1, 0, 1));

                float angle = 2 * PI / resolution;
                for (int i = 0; i < resolution; i++) Vertices[i + 2] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));

                // Edges
                Edges = new Edge[resolution];

                for (int i = 0; i < resolution - 1; i++) Edges[i] = new Edge(Vertices[i + 2], Vertices[i + 3]);
                Edges[resolution - 1] = new Edge(Vertices[resolution + 1], Vertices[2]);

                // Faces
                Triangles = new SolidFace[2 * resolution];

                for (int i = 0; i < resolution - 1; i++)
                {
                    Triangles[i] = new SolidFace(Vertices[i + 2], Vertices[0], Vertices[i + 3]);
                    Triangles[i + resolution] = new SolidFace(Vertices[i + 2], Vertices[i + 3], Vertices[1]);
                }
                Triangles[resolution - 1] = new SolidFace(Vertices[resolution - 1], Vertices[0], Vertices[2]);
                Triangles[2 * resolution - 1] = new SolidFace(Vertices[resolution - 1], Vertices[2], Vertices[1]);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cone"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cone"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cone"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cone"/>.</param>
        /// <param name="height">The height of the <see cref="Cone"/>.</param>
        /// <param name="radius">The radius of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        /// <param name="resolution">The number of <see cref="Vertex">Vertices</see> that are on the perimeter of the base <see cref="Circle"/> of the <see cref="Cone"/>.</param>
        public Cone(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float height, float radius, int resolution) : base(origin, directionForward, directionUp)
        {
            Dimension = 3;

            Height = height;
            Radius = radius;
            Resolution = resolution;
        }

        #endregion
    }
}