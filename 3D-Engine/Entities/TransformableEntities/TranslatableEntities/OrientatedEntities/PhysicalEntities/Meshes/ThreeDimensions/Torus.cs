/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a torus mesh.
 */

using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// A sealed class representing a three-dimensional torus mesh. It inherits from the abstract <see cref="Mesh"/> class.
/// </summary>
/// <remarks>
/// Composition:<br/>
/// <list type="bullet">
/// <item><description>A number of vertices equal to the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>;</description></item>
/// <item><description>A number of edges equal to twice the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>;</description></item>
/// <item><description>A number of triangles equal to twice the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>;</description></item>
/// <item><description>A number of faces equal to the product of the <see cref="InnerResolution">inner resolution</see> and the <see cref="OuterResolution">outer resolution</see>.</description></item>
/// </list>
/// </remarks>
public sealed class Torus : Mesh
{
    #region Fields and Properties

    private float innerRadius, outerRadius;
    private int innerResolution, outerResolution;
    private EdgeStyle edgeStyle;
    private FaceStyle faceStyle;

    /// <summary>
    /// The radius of the empty inner circle.
    /// </summary>
    public float InnerRadius
    {
        get => innerRadius;
        set
        {
            if (value == innerRadius) return;
            innerRadius = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        }
    }

    /// <summary>
    /// The distance from the centre point to the outermost point.
    /// </summary>
    public float OuterRadius
    {
        get => outerRadius;
        set
        {
            if (value == outerRadius) return;
            outerRadius = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        }
    }

    /// <summary>
    /// The number of vertices on the perimeter of the circle revolved around the central axis.
    /// </summary>
    public int InnerResolution
    {
        get => innerResolution;
        set
        {
            if (value == innerResolution) return;
            innerResolution = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
            Structure.Edges = GenerateEdges(Structure.Vertices, innerResolution, outerResolution, edgeStyle);
            Structure.Triangles = GenerateTriangles(Structure.Vertices, innerResolution, outerResolution, faceStyle);
            Structure.Faces = GenerateFaces(innerResolution, outerResolution, Structure.Triangles, faceStyle);
        }
    }

    /// <summary>
    /// The number of vertices on the perimeter of the circle with radius OuterRadius.
    /// </summary>
    public int OuterResolution
    {
        get => outerResolution;
        set
        {
            if (value == outerResolution) return;
            outerResolution = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);

            Structure.Vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
            Structure.Edges = GenerateEdges(Structure.Vertices, innerResolution, outerResolution, edgeStyle);
            Structure.Triangles = GenerateTriangles(Structure.Vertices, innerResolution, outerResolution, faceStyle);
            Structure.Faces = GenerateFaces(innerResolution, outerResolution, Structure.Triangles, faceStyle);
        }
    }

    /// <summary>
    /// The <see cref="EdgeStyle"/> applied to all edges.
    /// </summary>
    public EdgeStyle EdgeStyle
    {
        get => edgeStyle;
        set
        {
            if (value == edgeStyle) return;
            edgeStyle = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    /// <summary>
    /// The <see cref="FaceStyle"/> applied to all triangles and faces.
    /// </summary>
    public FaceStyle FaceStyle
    {
        get => faceStyle;
        set
        {
            if (value == faceStyle) return;
            faceStyle = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a torus.
    /// </summary>
    /// <param name="worldOrigin"></param>
    /// <param name="worldOrientation"></param>
    /// <param name="innerRadius"></param>
    /// <param name="outerRadius"></param>
    /// <param name="innerResolution"></param>
    /// <param name="outerResolution"></param>
    /// <param name="edgeStyle"></param>
    /// <param name="faceStyle"></param>
    public Torus(Vector3D worldOrigin,
                 [DisallowNull] Orientation worldOrientation,
                 float innerRadius,
                 float outerRadius,
                 int innerResolution,
                 int outerResolution,
                 [DisallowNull] EdgeStyle edgeStyle,
                 [DisallowNull] FaceStyle faceStyle)
        : base(worldOrigin, worldOrientation, GenerateStructure(innerResolution, outerResolution, innerRadius, outerRadius, edgeStyle, faceStyle)
              #if DEBUG
              , MessageBuilder<TorusCreatedMessage>.Instance()
              #endif
              )
    {
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        this.innerResolution = innerResolution;
        this.outerResolution = outerResolution;
        this.edgeStyle = edgeStyle;
        this.faceStyle = faceStyle;
    }

    #endregion

    #region Methods

    private static MeshStructure GenerateStructure(int innerResolution, int outerResolution, float innerRadius, float outerRadius, EdgeStyle edgeStyle, FaceStyle faceStyle)
    {
        IList<Vertex> vertices = GenerateVertices(innerResolution, outerResolution, innerRadius, outerRadius);
        IList<Edge> edges = GenerateEdges(vertices, innerResolution, outerResolution, edgeStyle);
        IList<Triangle> triangles = GenerateTriangles(vertices, innerResolution, outerResolution, faceStyle);
        IList<Face> faces = GenerateFaces(innerResolution, outerResolution, triangles, faceStyle);
        IList<Texture>? textures = faceStyle is TextureStyle textureStyle
                                 ? new Texture[1] { textureStyle.DisplayTexture }
                                 : null;

        return new MeshStructure(Dimension.Three, vertices, edges, triangles, faces, textures);
    }

    private static EventList<Vertex> GenerateVertices(int innerResolution, int outerResolution, float innerRadius, float outerRadius)
    {
        IList<Vertex> vertices = new Vertex[innerResolution * outerResolution + 1];

        vertices[0] = new Vertex(Vector3D.Zero);

        float interiorRadius = (outerRadius - innerRadius) / 2, exteriorRadius = innerRadius + interiorRadius;
        float innerAngle = Tau / innerResolution, outerAngle = Tau / outerResolution;
        for (int i = 0; i < outerResolution; i++)
        {
            for (int j = 0; j < innerResolution; j++)
            {
                vertices[innerResolution * i + j + 1] = new Vertex(new Vector3D(Cos(innerAngle * i) * interiorRadius * Cos(outerAngle * i) * exteriorRadius,
                                                                                Sin(innerAngle * i) * interiorRadius,
                                                                                Sin(outerAngle * i) * exteriorRadius));
            }
        }

        return new EventList<Vertex>(vertices);
    }

    private static EventList<Edge> GenerateEdges(IList<Vertex> vertices, int innerResolution, int outerResolution, EdgeStyle style)
    {
        IList<Edge> edges = new Edge[innerResolution * outerResolution * 2];

        for (int j = 0; j < outerResolution - 1; j++)
        {
            for (int i = 0; i < innerResolution - 1; i++)
            {
                edges[j * innerResolution + i] = new Edge(style, vertices[j * innerResolution + i + 1], vertices[j * innerResolution + i + 2]);
                edges[innerResolution * outerResolution + j * innerResolution + i] = new Edge(style, vertices[j * innerResolution + i + 1], vertices[(j + 1) * innerResolution + i + 1]);
            }
            edges[innerResolution * (j + 1)] = new Edge(style, vertices[innerResolution * (j + 1)], vertices[j * innerResolution + 1]);
        }
        for (int i = 0; i < innerResolution - 1; i++)
        {
            edges[(outerResolution - 1) * innerResolution + i] = new Edge(style, vertices[(outerResolution - 1) * innerResolution + i + 1], vertices[i + 2]);
            edges[innerResolution * outerResolution + (outerResolution - 1) * innerResolution + i] = new Edge(style, vertices[(outerResolution - 1) * innerResolution + i + 1], vertices[innerResolution + i + 1]);
        }

        return new EventList<Edge>(edges);
    }

    private static EventList<Triangle> GenerateTriangles(IList<Vertex> vertices, int innerResolution, int outerResolution, FaceStyle style)
    {
        IList<Triangle> triangles = new Triangle[innerResolution * outerResolution * 2];

        for (int j = 0; j < outerResolution - 1; j++)
        {
            for (int i = 0; i < innerResolution - 1; i++)
            {
                triangles[j * innerResolution + i] = new Triangle(style, style, vertices[j * innerResolution + i + 1],
                                                                                vertices[j * innerResolution + i + 2],
                                                                                vertices[(j + 1) * innerResolution + i + 2]);
                triangles[j * innerResolution + i + innerResolution * outerResolution] = new Triangle(style, style, vertices[j * innerResolution + i + 1],
                                                                                                                    vertices[(j + 1) * innerResolution + i + 2],
                                                                                                                    vertices[(j + 1) * innerResolution + i + 1]);
            }
            triangles[(j + 1) * innerResolution - 1] = new Triangle(style, style, vertices[(j + 1) * innerResolution], vertices[j * innerResolution + 1], vertices[(j + 1) * innerResolution + 2]);
            triangles[(j + 1) * innerResolution - 1 + innerResolution * outerResolution] = new Triangle(style, style, vertices[(j + 1) * innerResolution], vertices[(j + 1) * innerResolution + 2], vertices[(j + 1) * innerResolution + 1]);
        }
        for (int i = 0; i < innerResolution - 1; i++)
        {
            triangles[(outerResolution - 1) * innerResolution + i] = new Triangle(style, style, vertices[innerResolution * (outerResolution - 1) + i + 1],
                                                                                                vertices[innerResolution * (outerResolution - 1) + i + 2],
                                                                                                vertices[i + 2]);
            triangles[(outerResolution - 1) * innerResolution + i + innerResolution * outerResolution] = new Triangle(style, style, vertices[innerResolution * (outerResolution - 1) + i + 1],
                                                                                                                                    vertices[i + 2],
                                                                                                                                    vertices[i + 1]);
        }
        triangles[(outerResolution - 1) * innerResolution + innerResolution - 1] = new Triangle(style, style, vertices[innerResolution * (outerResolution - 1) + innerResolution],
                                                                                                              vertices[innerResolution * (outerResolution - 1) + 1],
                                                                                                              vertices[1]);
        triangles[(outerResolution - 1) * innerResolution + innerResolution - 1 + innerResolution * outerResolution] = new Triangle(style, style, vertices[innerResolution * (outerResolution - 1) + innerResolution],
                                                                                                                                                  vertices[1],
                                                                                                                                                  vertices[innerResolution]);

        return new EventList<Triangle>(triangles);
    }

    private static EventList<Face> GenerateFaces(int innerResolution, int outerResolution, IList<Triangle> triangles, FaceStyle style)
    {
        IList<Face> faces = new Face[innerResolution * outerResolution];

        for (int i = 0; i < innerResolution * outerResolution; i++)
        {
            faces[i] = new Face(style, style, triangles[i], triangles[i + innerResolution * outerResolution]);
        }

        return new EventList<Face>(faces);
    }

    #endregion
}