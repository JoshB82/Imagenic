using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    /// <summary>
    /// Encapsulates creation of a <see cref="Plane"/> mesh.
    /// </summary>
    public sealed class Plane : Mesh
    {
        #region Fields and Properties

        private float length, width;

        /// <summary>
        /// The length of the <see cref="Plane"/>.
        /// </summary>
        public float Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }
        /// <summary>
        /// The width of the <see cref="Plane"/>.
        /// </summary>
        public float Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, 1, width);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Plane"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Plane"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Plane"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Plane"/>. This is also a normal to the surface of the <see cref="Plane"/>.</param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        public Plane(Vector3D origin, Vector3D directionForward, Vector3D normal, float length, float width) : base(origin, directionForward, normal)
        {
            SetStructure(length, width);
            Faces = new SolidFace[2]
            {
                new(Vertices[0], Vertices[1], Vertices[2]), // 0
                new(Vertices[0], Vertices[2], Vertices[3]) // 1
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Plane"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Plane"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Plane"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Plane"/>. This is also a normal to the surface of the <see cref="Plane"/>.</param>
        /// <param name="length">The length of the <see cref="Plane"/>.</param>
        /// <param name="width">The width of the <see cref="Plane"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Plane"/>.</param>
        public Plane(Vector3D origin, Vector3D directionForward, Vector3D normal, float length, float width, Texture texture) : base(origin, directionForward, normal)
        {
            SetStructure(length, width);
            Textures = new Texture[1] { texture };
            Faces = new TextureFace[2]
            {
                new(Vertices[0], Vertices[1], Vertices[2], texture.Vertices[0], texture.Vertices[1], texture.Vertices[2], texture), // 0
                new(Vertices[0], Vertices[2], Vertices[3], texture.Vertices[0], texture.Vertices[2], texture.Vertices[3], texture) // 1
            };
        }

        private void SetStructure(float length, float width)
        {
            Dimension = 2;

            Length = length;
            Width = width;

            Vertices = new Vertex[4]
            {
                new(new Vector4D(0, 0, 0, 1)), // 0
                new(new Vector4D(1, 0, 0, 1)), // 1
                new(new Vector4D(1, 0, 1, 1)), // 2
                new(new Vector4D(0, 0, 1, 1)) // 3
            };

            Edges = new Edge[4]
            {
                new(Vertices[0], Vertices[1]), // 0
                new(Vertices[1], Vertices[2]), // 1
                new(Vertices[2], Vertices[3]), // 2
                new(Vertices[0], Vertices[3]) // 3
            };
        }

        #endregion
    }
}