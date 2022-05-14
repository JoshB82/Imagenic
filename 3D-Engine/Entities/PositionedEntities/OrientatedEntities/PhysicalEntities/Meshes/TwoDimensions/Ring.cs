/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a ring mesh.
 */

using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Edges;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities.Faces;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions.Ellipses;
using System.Collections.Generic;
using static System.MathF;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.TwoDimensions
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
                if (value == innerRadius) return;
                innerRadius = value;
                RequestNewRenders();

                Structure.Vertices = GenerateVertices(resolution, innerRadius, outerRadius);
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
                if (value == outerRadius) return;
                outerRadius = value;
                RequestNewRenders();

                Structure.Vertices = GenerateVertices(resolution, innerRadius, outerRadius);
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
                if (value == resolution) return;
                resolution = value;
                RequestNewRenders();

                Structure = GenerateStructure(resolution, innerRadius, outerRadius);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a <see cref="Ring"/> mesh.
        /// </summary>
        /// <param name="worldOrigin">The position of the <see cref="Ring"/>.</param>
        /// <param name="worldOrientation"></param>
        /// <param name="innerRadius">The radius of the inner <see cref="Circle"/>.</param>
        /// <param name="outerRadius">The radius of the outer <see cref="Circle"/>.</param>
        /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Ring"/>.</param>
        public Ring(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float innerRadius,
                    float outerRadius,
                    int resolution) : base(worldOrigin, worldOrientation, GenerateStructure(resolution, innerRadius, outerRadius))
        {
            this.innerRadius = innerRadius;
            this.outerRadius = outerRadius;
            this.resolution = resolution;
        }

        #endregion

        #region Methods

        private static MeshStructure GenerateStructure(int resolution, float innerRadius, float outerRadius)
        {
            IList<Vertex> vertices = GenerateVertices(resolution, innerRadius, outerRadius);
            IList<Edge> edges = GenerateEdges(vertices, resolution);
            IList<Face> faces = GenerateFaces(vertices, resolution);

            return new MeshStructure(Dimension.Two, vertices, edges, faces);
        }

        private static IList<Vertex> GenerateVertices(int resolution, float innerRadius, float outerRadius)
        {
            // Vertices are defined in anti-clockwise order.
            IList<Vertex> vertices = new Vertex[2 * resolution + 1];

            float angle = Tau / resolution;
            for (int i = 0; i < resolution; i++)
            {
                vertices[i + 1] = new Vertex(new Vector4D(Cos(angle * i) * innerRadius, 0, Sin(angle * i) * innerRadius, 1));
                vertices[i + resolution + 1] = new Vertex(new Vector4D(Cos(angle * i) * outerRadius, 0, Sin(angle * i) * outerRadius, 1));
            }

            return vertices;
        }

        private static IList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
        {
            IList<Edge> edges = new Edge[2 * resolution];

            for (int i = 0; i < resolution - 1; i++)
            {
                edges[i] = new SolidEdge(vertices[i + 1], vertices[i + 2]);
                edges[i + resolution] = new SolidEdge(vertices[i + resolution + 1], vertices[i + resolution + 2]);
            }
            edges[resolution - 1] = new SolidEdge(vertices[resolution], vertices[1]);
            edges[2 * resolution - 1] = new SolidEdge(vertices[2 * resolution], vertices[resolution + 1]);

            return edges;
        }

        private static IList<Face> GenerateFaces(IList<Vertex> vertices, int resolution)
        {
            IList<Face> faces = new Face[1];

            faces[0].Triangles = new SolidTriangle[2 * resolution];
            for (int i = 0; i < resolution - 1; i++)
            {
                faces[0].Triangles[i] = new SolidTriangle(vertices[i + 1], vertices[i + resolution + 2], vertices[i + resolution + 1]);
                faces[0].Triangles[i + resolution] = new SolidTriangle(vertices[i + 1], vertices[i + 2], vertices[i + resolution + 2]);
            }
            faces[0].Triangles[resolution - 1] = new SolidTriangle(vertices[resolution], vertices[resolution + 1], vertices[2 * resolution]);
            faces[0].Triangles[2 * resolution - 1] = new SolidTriangle(vertices[resolution], vertices[1], vertices[resolution + 1]);

            return faces;
        }

        #endregion
    }
}