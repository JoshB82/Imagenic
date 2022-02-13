using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Utilities;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;

public sealed class Face : Entity
{
    #region Fields and Properties

    // Appearance
    private float opacity = 1f;
    public float Opacity
    {
        get => opacity;
        set
        {
            if (value == opacity) return;
            opacity = value;
            InvokeRenderingEvents();
        }
    }

    private bool visible = true;
    public bool Visible
    {
        get => visible;
        set
        {
            if (value == visible) return;
            visible = value;
            InvokeRenderingEvents();
        }
    }

    // Structure
    private EventList<Vertex> vertices;
    private EventList<Edge> edges;
    private EventList<Triangle> triangles;

    public EventList<Vertex> Vertices
    {
        get => vertices;
        set
        {
            if (value == vertices) return;
            vertices = value;
            InvokeRenderingEvents();
        }
    }
    public EventList<Edge> Edges
    {
        get => edges;
        set
        {
            if (value == edges) return;
            edges = value;
            InvokeRenderingEvents();
        }
    }
    public EventList<Triangle> Triangles
    {
        get => triangles;
        set
        {
            if (value == triangles) return;
            triangles = value;
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    public Face(IList<Vertex> vertices)
    {
        Vertices = vertices;

        for (int i = 0; i < vertices.Count - 1; i++)
        {
            Edges.Add(new SolidEdge(vertices[i], vertices[i + 1]));
        }
        Edges.Add(new SolidEdge(vertices[vertices.Count - 1], vertices[0]));

        for (int i = 0; i < vertices.Count; i++)
        {
            //Triangles.Add(new SolidFace())
        }
    }

    public Face(IList<Edge> edges)
    {
        Edges = edges;
    }

    public Face(params Triangle[] triangles)
    {
        Triangles = triangles;
    }

    public Face(IList<Triangle> triangles)
    {
        Triangles = triangles;
    }

    #endregion

    #region Methods

    public Mesh Extrude(Face face, Vector3D displacement)
    {
        return null;
    }

    public Face Join(Face face1, Face face2)
    {
        return null;
    }

    internal bool DoesOverlap(Triangle t1, Triangle t2)
    {
        return true; // o.o
    }

    internal Triangle[] Decompose(Vertex[] vertices)
    {
        return null;
    }

    #endregion
}