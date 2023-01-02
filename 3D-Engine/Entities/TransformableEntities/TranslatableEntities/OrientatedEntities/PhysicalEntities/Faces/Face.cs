using Imagenic.Core.Attributes;
using Imagenic.Core.Enums;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

public class Face : Entity
{
    #region Fields and Properties

    // Appearance
    private FaceStyle frontStyle, backStyle;
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


    private float opacity = 1f;
    /*public float Opacity
    {
        get => opacity;
        set
        {
            if (value == opacity) return;
            opacity = value;
            InvokeRenderingEvents();
        }
    }*/

    private bool visible = true;
    /*public bool Visible
    {
        get => visible;
        set
        {
            if (value == visible) return;
            visible = value;
            InvokeRenderingEvents();
        }
    }*/

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
            ThrowIfNull(value);
            ThrowIfEmpty(value);
            triangles = value;

            vertices.Clear();
            foreach (Triangle triangle in triangles)
            {
                vertices.Add(triangle.P1);
                vertices.Add(triangle.P2);
                vertices.Add(triangle.P3);
            }
            vertices = vertices.Distinct().ToEventList();

            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    #endregion

    #region Constructors

    public Face([DisallowNull, ThrowIfNull] IList<Vertex> vertices) : base(vertices[0].WorldOrigin, Orientation.OrientationXY)
    {
        if (vertices.Count < 3)
        {
            // Throw exception.
        }

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

    public Face([DisallowNull][ThrowIfNull] IList<Edge> edges) : base(edges[0].P1.WorldOrigin, Orientation.OrientationXY)
    {
        if (edges.Count < 3)
        {
            // Throw exception.
        }

        Edges = edges;
    }

    public Face([DisallowNull] FaceStyle frontStyle,
                [DisallowNull] FaceStyle backStyle,
                params Triangle[] triangles)
        : this(frontStyle, backStyle, triangles)
    { }

    public Face([DisallowNull] FaceStyle frontStyle,
                [DisallowNull] FaceStyle backStyle,
                IEnumerable<Triangle> triangles)
        : base(MessageBuilder<FaceCreatedMessage>.Instance())
    {
        ThrowIfNull(frontStyle, backStyle);
        FrontStyle = frontStyle;
        BackStyle = backStyle;
        Triangles = new EventList<Triangle>(triangles.ToList());
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