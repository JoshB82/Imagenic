using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using _3D_Engine.Utilities;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components;

public class MeshStructure : Entity
{
    #region Fields and Properties

    private EventList<Vertex> vertices;
    private EventList<Edge> edges;
    private EventList<Face> faces;

    public EventList<Vertex> Vertices
    {
        get => vertices;
        set
        {
            vertices = value ?? throw new ParameterCannotBeNullException();
            InvokeRenderingEvents();
        }
    }
    public EventList<Edge> Edges
    {
        get => edges;
        set
        {
            edges = value;
            InvokeRenderingEvents(true, false);
        }
    }
    public EventList<Face> Faces
    {
        get => faces;
        set
        {
            faces = value;
            InvokeRenderingEvents();
        }
    }

    public IList<Texture> Textures { get; }

    public Dimension DimensionCount { get; }

    #endregion

    #region Constructors

    public MeshStructure(Dimension dimensionCount,
                         IList<Vertex> vertices,
                         IList<Edge> edges = null,
                         IList<Face> faces = null)
    {
        DimensionCount = dimensionCount;    
        
        Vertices = new EventList<Vertex>(vertices);
        Edges = new EventList<Edge>(edges);
        Faces = new EventList<Face>(faces);
    }

    public MeshStructure(IList<Vertex> vertices,
                         IList<Edge> edges = null,
                         IList<Face> faces = null)
        : this(DimensionHelper.DetermineDimension(vertices), vertices, edges, faces)
    { }

    #endregion
}