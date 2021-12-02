using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Enums;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.Components;

public class MeshStructure
{
    #region Fields and Properties

    private IList<Vertex> vertices;
    public IList<Vertex> Vertices
    {
        get => vertices;
        set
        {
            vertices = value ?? throw new ParameterCannotBeNullException();
        }
    }
    public IList<Edge> Edges { get; set; }
    public IList<Face> Faces { get; set; }

    public IEnumerable<Texture> Textures { get; set; }

    public Dimension DimensionCount { get; }

    #endregion

    #region Constructors

    public MeshStructure(Dimension dimensionCount,
                         IList<Vertex> vertices,
                         IList<Edge> edges = null,
                         IList<Face> faces = null)
    {
        DimensionCount = dimensionCount;    
        
        Vertices = vertices;
        Edges = edges;
        Faces = faces;
    }

    #endregion
}