/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a circle mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Enums;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System;
using System.Collections.Generic;
using static System.MathF;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions
{
    /// <summary>
    /// A sealed class representing a two-dimensional circle mesh. It inherits from<br/>
    /// the abstract <see cref="Mesh"/> class.
    /// </summary>
    /// <remarks>
    /// Composition:<br/>
    /// <list type="bullet">
    /// <item><description>A number of vertices equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.</description></item>
    /// <item><description>A number of edges equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.</description></item>
    /// <item><description><strong>1</strong> face (made of a number of triangles equal to the <strong><see cref="Resolution">resolution</see></strong> of the circle.)</description></item>
    /// </list>
    /// </remarks>
    public sealed class Circle : Mesh
    {
        #region Fields and Properties

        private float radius;
        private int resolution;

        /// <summary>
        /// The radius of the <see cref="Circle"/>.
        /// </summary>
        public float Radius
        {
            get => radius;
            set
            {
                radius = value;
                Scaling = new Vector3D(radius, 1, radius);
            }
        }

        /// <summary>
        /// The number of <see cref="Vertex">vertices</see> that are on the perimeter of the <see cref="Circle"/>.
        /// </summary>
        public int Resolution
        {
            get => resolution;
            set
            {
                if (value < 3) throw new ArgumentException("Resolution cannot be less than 3.");
                resolution = value;
                RequestNewRenders();

                GenerateVertices();
                GenerateEdges();
                GenerateFaces();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Circle"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Circle"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Circle"/> in world space.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
        public Circle(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      float radius,
                      int resolution) : base(worldOrigin, worldOrientation, GenerateStructure(resolution))
        {
            Radius = radius;
            Resolution = resolution;
        }

        /// <summary>
        /// Creates a textured <see cref="Circle"/> mesh, specifying a single <see cref= "Texture" /> for all sides.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Circle"/> in world space.</param>
        /// <param name="worldOrientation">The orientation of the <see cref="Circle"/> in world space.</param>
        /// <param name="radius">The radius of the <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of the <see cref="Circle"/>.</param>
        /// <param name="texture">The <see cref="Texture"/> that defines what to draw on the surface of the <see cref="Circle"/>.</param>
        public Circle(Vector3D worldOrigin,
                      Orientation worldOrientation,
                      float radius,
                      int resolution,
                      Texture texture) : base(worldOrigin, worldOrientation, GenerateStructure(resolution))
        {
            Radius = radius;
            Textures = new Texture[1] { texture };
            Resolution = resolution;
        }

        #endregion

        #region Methods

        private static MeshStructure GenerateStructure(int resolution)
        {
            IList<Vertex> vertices = GenerateVertices(resolution);
            IList<Edge> edges = GenerateEdges(vertices, resolution);
            IList<Face> faces = GenerateFaces(vertices, resolution);

            return new MeshStructure(Dimension.Two, vertices, edges, faces);
        }

        private static IList<Vertex> GenerateVertices(int resolution)
        {
            // Vertices are defined in anti-clockwise order.
            IList<Vertex> vertices = new Vertex[resolution + 1];
            vertices[0] = new Vertex(new Vector4D(0, 0, 0, 1));

            float angle = Tau / resolution;
            for (int i = 0; i < resolution; i++)
            {
                vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i), 0, Sin(angle * i), 1));
            }

            if (Textures is not null)
            {
                Textures[0].Vertices = new Vector3D[resolution + 1];
                Textures[0].Vertices[0] = new Vector3D(0.5f, 0.5f, 1);

                for (int i = 0; i < resolution; i++)
                {
                    Textures[0].Vertices[i + 1] = new Vector3D(Cos(angle * i) * 0.5f, Sin(angle * i) * 0.5f, 1);
                }
            }

            return vertices;
        }

        private static IList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
        {
            IList<Edge> edges = new Edge[resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                edges[i] = new SolidEdge(vertices[i + 1], vertices[i + 2]);
            }
            edges[resolution - 1] = new SolidEdge(vertices[resolution], vertices[1]);

            return edges;
        }

        private static IList<Face> GenerateFaces(IList<Vertex> vertices, int resolution)
        {
            IList<Face> faces = new Face[1];

            if (Textures is null)
            {
                for (int i = 0; i < resolution - 1; i++)
                {
                    faces[0].Triangles.Add(new SolidTriangle(vertices[i + 1], vertices[0], vertices[i + 2]));
                }
                faces[0].Triangles.Add(new SolidTriangle(vertices[resolution], vertices[0], vertices[1]));
            }
            else
            {
                for (int i = 0; i < resolution - 1; i++)
                {
                    faces[0].Triangles.Add(new TextureTriangle(vertices[i + 1], vertices[0], vertices[i + 2], Textures[0].Vertices[i + 1], Textures[0].Vertices[0], Textures[0].Vertices[i + 2], Textures[0]));
                }
                faces[0].Triangles.Add(new TextureTriangle(vertices[resolution], vertices[0], vertices[1], Textures[0].Vertices[resolution], Textures[0].Vertices[0], Textures[0].Vertices[1], Textures[0]));
            }

            return faces;
        }

        #endregion

        #region Casting



        #endregion
    }
}