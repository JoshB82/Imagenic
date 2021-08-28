using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Cuboids
{
    public sealed partial class ModelCuboid
    {
        #region Fields and Properties

        // Structure
        internal static readonly IList<Vertex> ModelVertices = GenerateModelVertices();

        internal static List<Vertex> GenerateModelVertices()
        {
            float radical = MathF.Sqrt(3) / 3;
            return new List<Vertex>
            {
                new Vertex(new Vector4D(0, 0, 0, 1), new Vector3D(,,)), // 0
                new Vertex(new Vector4D(1, 0, 0, 1), new Vector3D(,,)), // 1
                new Vertex(new Vector4D(1, 1, 0, 1), new Vector3D(,,)), // 2
                new Vertex(new Vector4D(0, 1, 0, 1), new Vector3D(,,)), // 3
                new Vertex(new Vector4D(0, 0, 1, 1), new Vector3D(,,)), // 4
                new Vertex(new Vector4D(1, 0, 1, 1), new Vector3D(,,)), // 5
                new Vertex(new Vector4D(1, 1, 1, 1), new Vector3D(,,)), // 6
                new Vertex(new Vector4D(0, 1, 1, 1), new Vector3D(,,)) // 7
            };
        }

        internal static readonly IList<Edge> MeshEdges = new List<Edge>
        {

        };

        internal static readonly IList<Face> MeshFaces = new List<Face>
        {

        };

        #endregion
    }
}