using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.Enums;

public enum Dimension
{
    Zero,
    One,
    Two,
    Three
}

public static class DimensionHelper
{
    public static Dimension DetermineDimension(IEnumerable<Vertex> vertices)
    {
        if (vertices is null)
        {
            // throw exception
        }

        if (vertices.Count() == 1)
        {
            return Dimension.Zero;
        }
        if (vertices.AreCollinear())
        {
            return Dimension.One;
        }
        if (vertices.AreCoplanar())
        {
            return Dimension.Two;
        }
        return Dimension.Three;
    }
}