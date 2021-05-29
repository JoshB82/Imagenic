/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a class for square meshes which are composed of two identical triangular faces.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    /// <summary>
    /// A sealed class that defines objects of type <see cref="Square"/> and represents a square mesh.
    /// </summary>
    /// <remarks>This class inherits from the <see cref="Mesh"/> class.</remarks>
    public sealed class Square : Mesh
    {
        #region Fields and Properties

        private float sideLength;

        /// <summary>
        /// The length of each side of the <see cref="Square"/>.
        /// </summary>
        public float SideLength
        {
            get => sideLength;
            set
            {
                sideLength = value;
                Scaling = new Vector3D(sideLength, 1, sideLength);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Square"/> mesh.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Square"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Square"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Square"/>. This is also a normal to the surface of the <see cref="Square"/>.</param>
        /// <param name="sideLength">The length of each side of the <see cref="Square"/>.</param>
        public Square(Vector3D origin, Vector3D directionForward, Vector3D normal, float sideLength) : base(origin, directionForward, normal)
        {
            SetStructure(sideLength);
            Faces = new SolidFace[2]
            {
                new(Vertices[0], Vertices[1], Vertices[2]), // 0
                new(Vertices[0], Vertices[2], Vertices[3]) // 1
            };
        }

        /// <summary>
        /// Creates a textured <see cref="Square"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Square"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Square"/> faces.</param>
        /// <param name="normal">The upward orientation of the <see cref="Square"/>. This is also a normal to the surface of the <see cref="Square"/>.</param>
        /// <param name="sideLength">The length of each side of the <see cref="Square"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Square"/>.</param>
        public Square(Vector3D origin, Vector3D directionForward, Vector3D normal, float sideLength, Texture texture) : base(origin, directionForward, normal)
        {
            SetStructure(sideLength);
            Textures = new Texture[1] { texture };
            Faces = new TextureFace[2]
            {
                new(Vertices[0], Vertices[1], Vertices[2], texture.Vertices[0], texture.Vertices[1], texture.Vertices[2], texture), // 0
                new(Vertices[0], Vertices[2], Vertices[3], texture.Vertices[0], texture.Vertices[2], texture.Vertices[3], texture) // 1
            };
        }

        private void SetStructure(float sideLength)
        {
            Dimension = 2;

            SideLength = sideLength;

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

        #region Casting

        /// <summary>
        /// Casts a <see cref="Square"/> into a <see cref="Plane"/>.
        /// </summary>
        /// <param name="square"><see cref="Square"/> to cast.</param>
        public static explicit operator Plane(Square square) =>
            new(square.WorldOrigin, square.WorldDirectionForward, square.WorldDirectionUp, square.sideLength, square.sideLength)
            {
                Textures = square.Textures,
                Faces = square.Faces
            };

        #endregion
    }
}