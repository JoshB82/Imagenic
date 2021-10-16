﻿/*
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
    /// Composition:
    /// It has six square <see cref="Face">faces</see>, each consisting of two <see cref="Triangle">triangles</see>, 12 <see cref="Edge">edges</see> and eight <see cref="Vertex">vertices</see>.</remarks>
    public sealed partial class Cuboid : Mesh
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
        /// <param name="worldOrigin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cuboid"/>.</param>
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

            Vertices = ModelVertices;
            Edges = MeshEdges;
            Faces = MeshFaces;
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a single <see cref="Texture"/> for all sides.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cuboid"/>.</param>
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

            Vertices = ModelVertices;
            Edges = MeshEdges;
            Faces = MeshFaces;

            Textures = new Texture[1] { texture };
        }

        /// <summary>
        /// Creates a textured <see cref="Cuboid"/> mesh, specifying a <see cref="Texture"/> for each side.
        /// </summary>
        /// <param name="origin">The position of the <see cref="Cuboid"/>.</param>
        /// <param name="directionForward">The direction the <see cref="Cuboid"/> faces.</param>
        /// <param name="directionUp">The upward orientation of the <see cref="Cuboid"/>.</param>
        /// <param name="length">The length of the <see cref="Cuboid"/>.</param>
        /// <param name="width">The width of the <see cref="Cuboid"/>.</param>
        /// <param name="height">The height of the <see cref="Cuboid"/>.</param>
        /// <param name="front">The <see cref="Texture"/> for the front face of the <see cref="Cuboid"/>.</param>
        /// <param name="right">The <see cref="Texture"/> for the right face of the <see cref="Cuboid"/>.</param>
        /// <param name="back">The <see cref="Texture"/> for the back face of the <see cref="Cuboid"/>.</param>
        /// <param name="left">The <see cref="Texture"/> for the left face of the <see cref="Cuboid"/>.</param>
        /// <param name="top">The <see cref="Texture"/> for the top face of the <see cref="Cuboid"/>.</param>
        /// <param name="bottom">The <see cref="Texture"/> for the bottom face of the <see cref="Cuboid"/>.</param>
        public Cuboid(Vector3D origin,
                      Vector3D directionForward,
                      Vector3D directionUp,
                      float length,
                      float width,
                      float height,
                      Texture front,
                      Texture right,
                      Texture back,
                      Texture left,
                      Texture top,
                      Texture bottom) : base(origin, directionForward, directionUp, 3)
        {
            SetStructure(length, width, height);
            Textures = new Texture[6] { front, right, back, left, top, bottom };
            Triangles = new TextureTriangle[12]
            {
                new(Vertices[1], Vertices[6], Vertices[2], front.Vertices[1], front.Vertices[3], front.Vertices[2], front), // 0
                new(Vertices[1], Vertices[5], Vertices[6], front.Vertices[1], front.Vertices[0], front.Vertices[3], front), // 1
                new(Vertices[4], Vertices[7], Vertices[5], right.Vertices[0], right.Vertices[3], right.Vertices[1], right), // 2
                new(Vertices[5], Vertices[7], Vertices[6], right.Vertices[1], right.Vertices[3], right.Vertices[2], right), // 3
                new(Vertices[0], Vertices[3], Vertices[4], back.Vertices[0], back.Vertices[3], back.Vertices[1], back), // 4
                new(Vertices[4], Vertices[3], Vertices[7], back.Vertices[1], back.Vertices[3], back.Vertices[2], back), // 5
                new(Vertices[0], Vertices[1], Vertices[2], left.Vertices[1], left.Vertices[0], left.Vertices[3], left), // 6
                new(Vertices[0], Vertices[2], Vertices[3], left.Vertices[1], left.Vertices[3], left.Vertices[2], left), // 7
                new(Vertices[7], Vertices[3], Vertices[6], top.Vertices[0], top.Vertices[3], top.Vertices[1], top), // 8
                new(Vertices[6], Vertices[3], Vertices[2], top.Vertices[1], top.Vertices[3], top.Vertices[2], top), // 9
                new(Vertices[4], Vertices[5], Vertices[1], bottom.Vertices[3], bottom.Vertices[2], bottom.Vertices[1], bottom), // 10
                new(Vertices[4], Vertices[1], Vertices[0], bottom.Vertices[3], bottom.Vertices[1], bottom.Vertices[0], bottom) // 11
            };
        }

        private void SetStructure(float length, float width, float height)
        {
            Dimension = 3;

            Length = length;
            Width = width;
            Height = height;

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
    }
}