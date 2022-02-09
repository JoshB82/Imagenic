using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Faces;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions;

public sealed class Path : Mesh
{
    #region Fields and Properties

    #endregion

    #region Constructors

    public Path(Vector3D worldOrigin, Orientation worldOrientation, IEnumerable<Vector3D> points) : base(worldOrigin, worldOrientation, 2, new PathVertexData { Points = points })
    {

    }

    public Path(Vector3D worldOrigin, Orientation worldOrientation, IEnumerable<Vertex> vertices) : base(worldOrigin, worldOrientation, 2, new PathVertexData { Points = vertices.Select(x => (Vector3D)x.Point) })
    {

    }

    public Path(Vector3D worldOrigin, Orientation worldOrientation, IEnumerable<Line> lines)
    {

    }

    #endregion

    #region Methods

    protected override IList<Vertex> GenerateVertices(MeshData<Vertex> vertexData)
    {
        PathVertexData pathVertexData = vertexData as PathVertexData;

        IList<Vertex> vertices = new List<Vertex>();
        foreach (Vector3D point in pathVertexData.Points)
        {
            vertices.Add(new Vertex(point));
        }

        return vertices;
    }

    protected override IList<Edge> GenerateEdges(MeshData<Edge> edgeData)
    {

    }

    protected override IList<Face> GenerateFaces(MeshData<Face> faceData)
    {
        return null;
    }

    #endregion

    #region Classes

    private class PathVertexData : MeshData<Vertex>
    {
        internal IEnumerable<Vector3D> Points { get; set; }
    }

    private class PathEdgeData : MeshData<Edge>
    {

    }

    private class PathFaceData : MeshData<Face>
    {

    }

    #endregion
}