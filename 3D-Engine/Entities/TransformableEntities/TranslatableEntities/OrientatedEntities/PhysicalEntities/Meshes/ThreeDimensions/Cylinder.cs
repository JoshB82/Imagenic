/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a cylinder.
 */

using Imagenic.Core.Enums;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A cylinder mesh.
/// </summary>
/// <remarks>
/// Composition:<br/>
///
/// </remarks>
public sealed class Cylinder : Mesh
{
    #region Fields and Properties

    private float height, radius;
    private int resolution;

    /// <summary>
    /// The height of the <see cref="Cylinder"/>.
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
    /// The radius of the top and bottom <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
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
    /// The number of vertices that are on the perimeter of each of the <see cref="Circle"/> that make up the <see cref="Cylinder"/>.
    /// </summary>
    public int Resolution
    {
        get => resolution;
        set
        {
            if (value == resolution) return;
            resolution = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure = GenerateStructure(resolution);
        }
    }

    #endregion

    #region Constructors

    #if DEBUG

    /// <summary>
    /// Creates a <see cref="Cylinder"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cylinder"/>.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cylinder"/> in world space.</param>
    /// <param name="height">The height of the <see cref="Cylinder"/>.</param>
    /// <param name="radius">The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
    /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
    public Cylinder(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float height,
                    float radius,
                    int resolution)
        : base(worldOrigin,
               worldOrientation,
               GenerateStructure(resolution), MessageBuilder<CylinderCreatedMessage>.Instance())
    {
        NonDebugConstructorBody(height, radius, resolution);
    }

    #else

    /// <summary>
    /// Creates a <see cref="Cylinder"/> mesh.
    /// </summary>
    /// <param name="worldOrigin">The position of the <see cref="Cylinder"/>.</param>
    /// <param name="worldOrientation">The orientation of the <see cref="Cylinder"/> in world space.</param>
    /// <param name="height">The height of the <see cref="Cylinder"/>.</param>
    /// <param name="radius">The radius of the top and bottom <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
    /// <param name="resolution">The number of vertices that are on the perimeter of each of the <see cref="Circle"/>s that make up the <see cref="Cylinder"/>.</param>
    public Cylinder(Vector3D worldOrigin,
                    Orientation worldOrientation,
                    float height,
                    float radius,
                    int resolution)
        : base(worldOrigin,
               worldOrientation,
               GenerateStructure(resolution))
    {
        NonDebugConstructorBody(height, radius, resolution);
    }

    #endif

    private void NonDebugConstructorBody(float height, float radius, int resolution)
    {
        Height = height;
        Radius = radius;
        Resolution = resolution;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int resolution)
    {
        IList<Vertex> vertices = GenerateVertices(resolution);
        IList<Edge> edges = GenerateEdges(vertices, resolution);
        IList<Triangle> triangles = GenerateTriangles(vertices, resolution);
        IList<Face> faces = GenerateFaces(triangles, resolution);

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces);
    }

    private static IList<Vertex> GenerateVertices(int resolution)
    {
        IList<Vertex> vertices = new Vertex[2 * resolution + 2];
        vertices[0] = new Vertex(Vector3D.Zero);
        vertices[1] = new Vertex(Vector3D.UnitY);

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            vertices[i + 2] = new Vertex(new Vector3D(Cos(angle * i), 0, Sin(angle * i)));
            vertices[i + resolution + 2] = new Vertex(new Vector3D(Cos(angle * i), 1, Sin(angle * i)));
        }

        return vertices;
    }

    private static IList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
    {
        IList<Edge> edges = new Edge[3 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new SolidEdge(vertices[i + 2], vertices[i + 3]);
            edges[i + resolution] = new SolidEdge(vertices[i + resolution + 2], vertices[i + resolution + 3]);
            edges[i + 2 * resolution] = new SolidEdge(vertices[i + 2], vertices[i + resolution + 2]);
        }
        edges[resolution - 1] = new SolidEdge(vertices[resolution + 1], vertices[2]);
        edges[2 * resolution - 1] = new SolidEdge(vertices[2 * resolution + 1], vertices[resolution + 2]);
        edges[3 * resolution - 1] = new SolidEdge(vertices[resolution + 1], vertices[2 * resolution + 1]);

        return edges;
    }

    private static IList<Triangle> GenerateTriangles(IList<Vertex> vertices, int resolution)
    {
        IList<Triangle> triangles = new Triangle[4 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            triangles[i] = new SolidTriangle(vertices[i + 2], vertices[0], vertices[i + 3]);
            triangles[i + resolution - 1] = new SolidTriangle(vertices[i + resolution + 2], vertices[i + resolution + 3], vertices[1]);
            triangles[i + 2 * resolution - 2] = new SolidTriangle(vertices[i + 2], vertices[i + resolution + 3], vertices[i + resolution + 2]);
            triangles[i + 3 * resolution - 3] = new SolidTriangle(vertices[i + 2], vertices[i + 3], vertices[i + resolution + 3]);
        }

        triangles[4 * resolution - 1] = new SolidTriangle(vertices[resolution + 1], vertices[0], vertices[2]);
        triangles[4 * resolution - 2] = new SolidTriangle(vertices[2 * resolution + 1], vertices[resolution + 2], vertices[1]);
        triangles[4 * resolution - 3] = new SolidTriangle(vertices[resolution + 1], vertices[resolution + 2], vertices[2 * resolution + 1]);
        triangles[4 * resolution - 4] = new SolidTriangle(vertices[resolution + 1], vertices[2], vertices[resolution + 2]);

        return triangles;
    }

    private static IList<Face> GenerateFaces(IList<Triangle> triangles, int resolution)
    {
        IList<Face> faces = new Face[resolution + 2];

        for (int i = 0; i < resolution - 1; i++)
        {
            faces[0].Triangles.Add(triangles[i]);
            faces[1].Triangles.Add(triangles[i + resolution - 1]);

            faces[i + 2] = new SolidFace(
                triangles[i + 2 * resolution - 2],
                triangles[i + 3 * resolution - 3]
            );
        }

        faces[0].Triangles.Add(triangles[4 * resolution - 1]);
        faces[1].Triangles.Add(triangles[4 * resolution - 2]);

        faces[resolution] = new SolidFace(
            triangles[4 * resolution - 3],
            triangles[4 * resolution - 4]
        );

        return faces;
    }

#endregion
}