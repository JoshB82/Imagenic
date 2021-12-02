using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components;

public class Face
{
    #region Fields and Properties

    // Appearance
    public float Opacity { get; set; } = 1f;

    private bool visible = true;
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
        }
    }

    // Structure
    public IList<Vertex> Vertices { get; set; }
    public IList<Edge> Edges { get; set; }
    public IList<Triangle> Triangles { get; set; }

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