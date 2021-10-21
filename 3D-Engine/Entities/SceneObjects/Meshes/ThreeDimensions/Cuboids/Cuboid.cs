/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a Cuboid Mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Cuboids
{
    /// <summary>
    /// A mesh of a cuboid.
    /// </summary>
    /// <remarks>
    /// Composition:<br/>
    /// Six square <see cref="Face">faces</see>, each consisting of two <see cref="Triangle">triangles</see>, 12 <see cref="Edge">edges</see> and eight <see cref="Vertex">vertices</see>.
    /// </remarks>
    public sealed class Cuboid : Mesh
    {
        #region Fields and Properties

        private float length, width, height;

        /// <summary>
        /// The length of the <see cref="Cuboid"/>.
        /// </summary>
        public float Length
        {
            get => length;
            set
            {
                length = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        /// <summary>
        /// The width of the <see cref="Cuboid"/>.
        /// </summary>
        public float Width
        {
            get => width;
            set
            {
                width = value;
                Scaling = new Vector3D(length, width, height);
            }
        }
        /// <summary>
        /// The height of the <see cref="Cuboid"/>.
        /// </summary>
        public float Height
        {
            get => height;
            set
            {
                height = value;
                Scaling = new Vector3D(length, width, height);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cuboid"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      float length,
                      float width,
                      float height) : base(worldOrigin, worldOrientation, 3)
        {
            Length = length;
            Width = width;
            Height = height;

            Vertices = MeshData.CuboidVertices;
            Edges = MeshData.CuboidEdges;
            Faces = MeshData.CuboidFaces;
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      float length,
                      float width,
                      float height,
                      Texture texture) : base(worldOrigin, worldOrientation, 3)
        {
            Length = length;
            Width = width;
            Height = height;

            Vertices = MeshData.CuboidVertices;
            Edges = MeshData.CuboidEdges;
            Faces = MeshData.GenerateTextureFaces(new Texture[] { texture, texture, texture, texture, texture, texture });

            Textures = new Texture[1] { texture };
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a <see cref="Texture"/> for each side.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Cuboid"/> in world space.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cuboid"/>.</param>
        /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cuboid"/>.</param>
        /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cuboid"/>.</param>
        /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cuboid"/>.</param>
        /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cuboid"/>.</param>
        /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      float length,
                      float width,
                      float height,
                      Texture front,
                      Texture right,
                      Texture back,
                      Texture left,
                      Texture top,
                      Texture bottom) : base(worldOrigin, worldOrientation, 3)
        {
            Length = length;
            Width = width;
            Height = height;

            Vertices = MeshData.CuboidVertices;
            Edges = MeshData.CuboidEdges;
            Faces = MeshData.GenerateTextureFaces(new Texture[] { back, right, front, left, top, bottom });

            Textures = new Texture[6] { front, right, back, left, top, bottom };
        }

        #endregion

        #region Casting



        #endregion
    }
}