using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

public class MeshStructure : Entity
{
    #region Fields and Properties

    private EventList<Vertex> vertices;
    private EventList<Edge>? edges;
    private EventList<Triangle>? triangles;
    private EventList<Face>? faces;

    public EventList<Vertex> Vertices
    {
        get => vertices;
        set
        {
            ThrowIfNull(vertices);
            vertices = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }
    public EventList<Edge>? Edges
    {
        get => edges;
        set
        {
            ThrowIfNull(edges);
            edges = value!;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    public EventList<Triangle>? Triangles
    {
        get => triangles;
        set
        {
            ThrowIfNull(triangles);
            triangles = value!;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    public EventList<Face>? Faces
    {
        get => faces;
        set
        {
            ThrowIfNull(faces);
            faces = value!;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    public IList<Texture>? Textures { get; }

    public Dimension DimensionCount { get; }

    #endregion

    #region Constructors

    public MeshStructure(Dimension dimensionCount,
                         EventList<Vertex> vertices,
                         EventList<Edge>? edges = null,
                         EventList<Triangle>? triangles = null,
                         EventList<Face>? faces = null,
                         IList<Texture>? textures = null)
    {
        DimensionCount = dimensionCount;
        Vertices = vertices;
        Edges = edges;
        Triangles = triangles;
        Faces = faces;
        Textures = textures;
    }

    public MeshStructure(Dimension dimensionCount,
                         IList<Vertex> vertices,
                         IList<Edge>? edges = null,
                         IList<Triangle>? triangles = null,
                         IList<Face>? faces = null,
                         IList<Texture>? textures = null)
        : this(dimensionCount,
               new EventList<Vertex>(vertices),
               edges?.ToEventList(),
               triangles?.ToEventList(),
               faces?.ToEventList(),
               textures)
    { }

    public MeshStructure(EventList<Vertex> vertices,
                         EventList<Edge>? edges = null,
                         EventList<Triangle>? triangles = null,
                         EventList<Face>? faces = null,
                         IList<Texture>? textures = null)
        : this(DimensionHelper.DetermineDimension(vertices), vertices, edges, triangles, faces, textures)
    { }

    public MeshStructure(IList<Vertex> vertices,
                         IList<Edge>? edges = null,
                         IList<Triangle>? triangles = null,
                         IList<Face>? faces = null,
                         IList<Texture>? textures = null)
        : this(DimensionHelper.DetermineDimension(vertices), vertices, edges, triangles, faces, textures)
    { }

    #endregion
}