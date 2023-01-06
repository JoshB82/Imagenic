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

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// Defines a <see cref="Ring"/> mesh.
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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

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
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(resolution, innerRadius, outerRadius);
            Structure.Edges = GenerateEdges(Structure.Vertices, resolution, edgeStyle);
            Structure.Triangles = GenerateTriangles(Structure.Vertices, resolution, frontStyle, backStyle);
            Structure.Faces = GenerateFaces(Structure.Triangles, frontStyle, backStyle);
        }
    }

    private EdgeStyle edgeStyle;
    private FaceStyle frontStyle, backStyle;

    public EdgeStyle EdgeStyle
    {
        get => edgeStyle;
        set
        {
            edgeStyle = value;
        }
    }

    public FaceStyle FrontStyle
    {
        get => frontStyle;
        set
        {
            frontStyle = value;
        }
    }
    public FaceStyle BackStyle
    {
        get => backStyle;
        set
        {
            backStyle = value;
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
                [DisallowNull] Orientation worldOrientation,
                float innerRadius,
                float outerRadius,
                int resolution,
                [DisallowNull] EdgeStyle edgeStyle,
                [DisallowNull] FaceStyle frontStyle,
                [DisallowNull] FaceStyle backStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(resolution, innerRadius, outerRadius, edgeStyle, frontStyle, backStyle)
            #if DEBUG
              , MessageBuilder<RingCreatedMessage>.Instance()
            #endif
              )
    {
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        this.resolution = resolution;
        this.edgeStyle = edgeStyle;
        this.frontStyle = frontStyle;
        this.backStyle = backStyle;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int resolution, float innerRadius, float outerRadius, EdgeStyle edgeStyle, FaceStyle frontStyle, FaceStyle backStyle)
    {
        IList<Vertex> vertices = GenerateVertices(resolution, innerRadius, outerRadius);
        IList<Edge> edges = GenerateEdges(vertices, resolution, edgeStyle);
        IList<Triangle> triangles = GenerateTriangles(vertices, resolution, frontStyle, backStyle);
        IList<Face> faces = GenerateFaces(triangles, frontStyle, backStyle);
        IList<Texture>? textures = null;
        if (frontStyle is TextureStyle textureFrontStyle)
        {
            textures = new List<Texture> { textureFrontStyle.DisplayTexture };
        }
        if (backStyle is TextureStyle textureBackStyle)
        {
            textures ??= new List<Texture>();
            textures.Add(textureBackStyle.DisplayTexture);
        }

        return new MeshStructure(Dimension.Two, vertices, edges, triangles, faces, textures);
    }

    private static EventList<Vertex> GenerateVertices(int resolution, float innerRadius, float outerRadius)
    {
        // Vertices are defined in anti-clockwise order.
        IList<Vertex> vertices = new Vertex[2 * resolution + 1];

        float angle = Tau / resolution;
        for (int i = 0; i < resolution; i++)
        {
            vertices[i + 1] = new Vertex(new Vector3D(Cos(angle * i) * innerRadius, 0, Sin(angle * i) * innerRadius));
            vertices[i + resolution + 1] = new Vertex(new Vector3D(Cos(angle * i) * outerRadius, 0, Sin(angle * i) * outerRadius));
        }

        return new EventList<Vertex>(vertices);
    }

    private static EventList<Edge> GenerateEdges(IList<Vertex> vertices, int resolution, EdgeStyle style)
    {
        IList<Edge> edges = new Edge[2 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            edges[i] = new Edge(style, vertices[i + 1], vertices[i + 2]);
            edges[i + resolution] = new Edge(style, vertices[i + resolution + 1], vertices[i + resolution + 2]);
        }
        edges[resolution - 1] = new Edge(style, vertices[resolution], vertices[1]);
        edges[2 * resolution - 1] = new Edge(style, vertices[2 * resolution], vertices[resolution + 1]);

        return new EventList<Edge>(edges);
    }

    private static EventList<Triangle> GenerateTriangles(IList<Vertex> vertices, int resolution, FaceStyle frontStyle, FaceStyle backStyle)
    {
        IList<Triangle> triangles = new Triangle[2 * resolution];

        for (int i = 0; i < resolution - 1; i++)
        {
            triangles[i] = new Triangle(frontStyle, backStyle, vertices[i + 1], vertices[i + resolution + 2], vertices[i + resolution + 1]);
            triangles[i + resolution] = new Triangle(frontStyle, backStyle, vertices[i + 1], vertices[i + 2], vertices[i + resolution + 2]);
        }
        triangles[resolution - 1] = new Triangle(frontStyle, backStyle, vertices[resolution], vertices[resolution + 1], vertices[2 * resolution]);
        triangles[2 * resolution - 1] = new Triangle(frontStyle, backStyle, vertices[resolution], vertices[1], vertices[resolution + 1]);

        return new EventList<Triangle>(triangles);
    }

    private static EventList<Face> GenerateFaces(IList<Triangle> triangles, FaceStyle frontStyle, FaceStyle backStyle)
    {
        IList<Face> faces = new Face[1];

        faces[0] = new Face(frontStyle, backStyle, triangles);

        return new EventList<Face>(faces);
    }

    #endregion
}