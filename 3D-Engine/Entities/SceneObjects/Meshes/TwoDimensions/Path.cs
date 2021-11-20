using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.TwoDimensions;

public sealed class Path : Mesh
{
    #region Fields and Properties

    #endregion

    #region Constructors

    public Path(Vector3D worldOrigin, Orientation worldOrientation, IEnumerable<Vector3D> points)
    {

    }

    public Path(Vector3D worldOrigin, Orientation worldOrientation, IEnumerable<Line> lines)
    {

    }

    #endregion

    #region Methods

    protected override IList<Vertex> GenerateVertices()
    {

    }

    protected override IList<Edge> GenerateEdges()
    {

    }

    protected override IList<Face> GenerateFaces()
    {
        return null;
    }

    #endregion
}