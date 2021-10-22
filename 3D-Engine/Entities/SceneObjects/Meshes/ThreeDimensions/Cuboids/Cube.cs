/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines creation of a Cube Mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Cuboids
{
    /// <summary>
    /// A mesh of a cube.
    /// </summary>
    /// <remarks>
    /// Composition<br/>
    /// It has six square <see cref="Face">faces</see>, each consisting of two <see cref="Triangle">triangles</see>, 12 <see cref="Edge">edges</see> and eight <see cref="Vertex">vertices</see>.
    /// </remarks>
    public sealed class Cube : Mesh
    {
        #region Fields and Properties

        private float sideLength;

        /// <summary>
        /// The length of each side.
        /// </summary>
        public float SideLength
        {
            get => sideLength;
            set
            {
                sideLength = value;
                Scaling = new Vector3D(sideLength, sideLength, sideLength);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Cube"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Cube"/> in world space.</param>
        /// <param name="sideLength">The length of each side.</param>
        public Cube(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float sideLength) : base(worldOrigin, worldOrientation, 3)
        {
            Vertices = MeshData.CuboidVertices;
            Edges = MeshData.CuboidEdges;
            Faces = MeshData.CuboidFaces;

            SideLength = sideLength;
        }

        /// <summary>
        /// Creates a textured <see cref="Cube"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
        /// <param name="worldOrientation"></param>
        /// <param name="sideLength">The length of each side.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on each surface of the <see cref="Cube"/>.</param>
        public Cube(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float sideLength,
                    Texture texture) : base(worldOrigin, worldOrientation, 3)
        {
            Vertices = MeshData.CuboidVertices;
            Edges = MeshData.CuboidEdges;
            Faces = MeshData.GenerateTextureFaces(new Texture[] { texture, texture, texture, texture, texture, texture });

            SideLength = sideLength;
            Textures = new Texture[1] { texture };
        }

        /// <summary>
        /// Creates a textured <see cref="Cube"/> mesh, specifying a <see cref="Texture"/> for each side.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cube"/> in world space.</param>
        /// <param name="worldOrientation"></param>
        /// <param name="sideLength">The length of each side of the <see cref="Cube"/>.</param>
        /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cube"/>.</param>
        /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cube"/>.</param>
        /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cube"/>.</param>
        /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cube"/>.</param>
        /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cube"/>.</param>
        /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cube"/>.</param>
        public Cube(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float sideLength,
                    Texture front,
                    Texture right,
                    Texture back,
                    Texture left,
                    Texture top,
                    Texture bottom) : base(worldOrigin, worldOrientation, 3)
        {
            SideLength = sideLength;
            Textures = new Texture[6] { front, right, back, left, top, bottom };
        }

        private void SetStructure(float sideLength)
        {
            SideLength = sideLength;

            Vertices = new Vertex[8]
            {
                new(new Vector4D(0, 0, 0, 1)), // 0
                new(new Vector4D(1, 0, 0, 1)), // 1
                new(new Vector4D(1, 1, 0, 1)), // 2
                new(new Vector4D(0, 1, 0, 1)), // 3
                new(new Vector4D(0, 0, 1, 1)), // 4
                new(new Vector4D(1, 0, 1, 1)), // 5
                new(new Vector4D(1, 1, 1, 1)), // 6
                new(new Vector4D(0, 1, 1, 1)) // 7
            }; // need to be oriented to front side

            Edges = new Edge[12]
            {
                new(Vertices[0], Vertices[1]), // 0
                new(Vertices[1], Vertices[2]), // 1
                new(Vertices[2], Vertices[3]), // 2
                new(Vertices[0], Vertices[3]), // 3
                new(Vertices[1], Vertices[5]), // 4
                new(Vertices[5], Vertices[6]), // 5
                new(Vertices[2], Vertices[6]), // 6
                new(Vertices[4], Vertices[5]), // 7
                new(Vertices[4], Vertices[7]), // 8
                new(Vertices[6], Vertices[7]), // 9
                new(Vertices[0], Vertices[4]), // 10
                new(Vertices[3], Vertices[7]) // 11
            };
        }

        #endregion

        #region Casting

        /// <summary>
        /// Casts a <see cref="Cube"/> into a <see cref="Cuboid"/>.
        /// </summary>
        /// <param name="cube"><see cref="Cube"/> to cast.</param>
        public static explicit operator Cuboid(Cube cube) =>
            new(cube.WorldOrigin, cube.WorldDirectionForward, cube.WorldDirectionUp, cube.sideLength, cube.sideLength, cube.sideLength)
            {
                Textures = cube.Textures,
                Triangles = cube.Triangles
            };

        #endregion
    }
}