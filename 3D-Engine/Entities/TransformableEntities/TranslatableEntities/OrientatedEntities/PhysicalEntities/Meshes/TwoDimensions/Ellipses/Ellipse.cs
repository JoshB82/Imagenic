/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an ellipse mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A two-dimensional mesh representing an ellipse.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description>A number of vertices equal to the <strong><see cref="Resolution">resolution</see></strong> of the ellipse;</description></item>
/// <item><description>A number of edges equal to the <strong><see cref="Resolution">resolution</see></strong> of the ellipse;</description></item>
/// <item><description><strong>One</strong> face (made of a number of triangles equal to the <strong><see cref="Resolution">resolution</see></strong> of the ellipse).</description></item>
/// </list>
/// </remarks>
public sealed class Ellipse : Mesh
{
    #region Fields and Properties

    private float majorAxis, minorAxis;
    private int resolution;

    /// <summary>
    /// The length of the major axis.
    /// </summary>
    public float MajorAxis
    {
        get => majorAxis;
        set
        {
            if (value == majorAxis) return;
            majorAxis = value;
            Scaling = new Vector3D(majorAxis / 2, 1, minorAxis / 2);
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The length of the minor axis.
    /// </summary>
    public float MinorAxis
    {
        get => minorAxis;
        set
        {
            if (value == minorAxis) return;
            minorAxis = value;
            Scaling = new Vector3D(majorAxis / 2, 1, minorAxis / 2);
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The number of <see cref="Vertex">vertices</see> on the perimeter of the ellipse.
    /// </summary>
    public int Resolution
    {
        get => resolution;
        set
        {
            if (value == resolution) return;
            resolution = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(resolution);
            Structure.Edges = GenerateEdges(Structure.Vertices, resolution);
            Structure.Triangles = GenerateTriangles(Structure.Vertices, resolution);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates an ellipse.
    /// </summary>
    /// <param name="worldOrigin">The initial location of the <see cref="Ellipse"/>.</param>
    /// <param name="worldOrientation">The initial orientation of the <see cref="Ellipse"/>.</param>
    /// <param name="majorAxis">The length of the major axis.</param>
    /// <param name="minorAxis">The length of the minor axis.</param>
    /// <param name="resolution">The number of <see cref="Vertex">vertices</see> on the perimeter of the <see cref="Ellipse"/>.</param>
    public Ellipse(Vector3D worldOrigin,
                   Orientation worldOrientation,
                   float majorAxis,
                   float minorAxis,
                   int resolution)
        : base(worldOrigin, worldOrientation, GenerateStructure(resolution)
               #if DEBUG
               , MessageBuilder<EllipseCreatedMessage>.Instance()
               #endif
              )
    {
        MajorAxis = majorAxis;
        MinorAxis = minorAxis;
        this.resolution = resolution;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int resolution)
    {
        EventList<Vertex> vertices = GenerateVertices(resolution);
        EventList<Edge> edges = GenerateEdges(vertices, resolution);
        EventList<Triangle> triangles = GenerateTriangles(vertices, resolution);
        EventList<Face> faces = GenerateFaces(triangles);

        return new MeshStructure(Dimension.Two, vertices, edges, triangles, faces, null);
    }

    private static EventList<Vertex> GenerateVertices(int resolution)
    {
        IList<Vertex> vertices = new Vertex[resolution + 1];

        vertices[0] = new Vertex(Vector3D.Zero);
        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++) vertices[i + 1] = new Vertex(new Vector3D(Cos(angle * i), 0, Sin(angle * i)));

        return new EventList<Vertex>(vertices);
    }

    private static EventList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution)
    {
        IList<Edge> edges = new Edge[resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new SolidEdge(vertices[i + 1], vertices[i + 2]);
        }
        edges[resolution - 1] = new SolidEdge(vertices[resolution], vertices[1]);

        return new EventList<Edge>(edges);
    }

    private static EventList<Triangle> GenerateTriangles(IList<Vertex> vertices, int resolution)
    {
        IList<Triangle> triangles = new Triangle[resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            triangles.Add(new SolidTriangle(vertices[0], vertices[i + 2], vertices[i + 1]));
        }
        triangles.Add(new SolidTriangle(vertices[0], vertices[1], vertices[resolution]));

        return new EventList<Triangle>(triangles);
    }

    private static EventList<Face> GenerateFaces(IList<Triangle> triangles)
    {
        IList<Face> faces = new Face[1];

        faces[0] = new SolidFace(triangles);

        return new EventList<Face>(faces);
    }

    #endregion

    #region Casting

    public static implicit operator Ellipse(Circle circle) => new Ellipse(circle.WorldOrigin, circle.WorldOrientation, circle.Radius, circle.Radius, circle.Resolution);

    public static explicit operator Circle(Ellipse ellipse) => new Circle(ellipse.WorldOrigin, ellipse.WorldOrientation, Min(ellipse.MajorAxis, ellipse.MinorAxis), ellipse.Resolution);

    #endregion
}