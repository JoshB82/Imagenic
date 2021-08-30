using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Cuboids
{
    public sealed partial class Cuboid : Mesh
    {
        #region Fields and Properties

        // Structure
        internal static readonly IList<Vertex> ModelVertices = GenerateModelVertices();

        internal static List<Vertex> GenerateModelVertices()
        {
            float radical = MathF.Sqrt(3) / 3;
            return new List<Vertex>
            {
                new Vertex(new Vector4D(0, 0, 0, 1), new Vector3D(-radical, -radical, -radical)), // 0
                new Vertex(new Vector4D(1, 0, 0, 1), new Vector3D(radical, -radical, -radical)), // 1
                new Vertex(new Vector4D(1, 1, 0, 1), new Vector3D(radical, radical, -radical)), // 2
                new Vertex(new Vector4D(0, 1, 0, 1), new Vector3D(-radical, radical, -radical)), // 3
                new Vertex(new Vector4D(0, 0, 1, 1), new Vector3D(-radical, -radical, radical)), // 4
                new Vertex(new Vector4D(1, 0, 1, 1), new Vector3D(radical, -radical, radical)), // 5
                new Vertex(new Vector4D(1, 1, 1, 1), new Vector3D(radical, radical, radical)), // 6
                new Vertex(new Vector4D(0, 1, 1, 1), new Vector3D(-radical, radical, radical)) // 7
            };
        }

        internal static readonly IList<Edge> MeshEdges = new List<Edge>
        {
            new Edge(ModelVertices[0], ModelVertices[1]), // 0
            new Edge(ModelVertices[1], ModelVertices[2]), // 1
            new Edge(ModelVertices[2], ModelVertices[3]), // 2
            new Edge(ModelVertices[3], ModelVertices[0]), // 3
            new Edge(ModelVertices[4], ModelVertices[5]), // 4
            new Edge(ModelVertices[5], ModelVertices[6]), // 5
            new Edge(ModelVertices[6], ModelVertices[7]), // 6
            new Edge(ModelVertices[7], ModelVertices[4]), // 7
            new Edge(ModelVertices[0], ModelVertices[4]), // 8
            new Edge(ModelVertices[1], ModelVertices[5]), // 9
            new Edge(ModelVertices[2], ModelVertices[6]), // 10
            new Edge(ModelVertices[3], ModelVertices[7]) // 11
        };

        internal static readonly IList<Face> MeshFaces = new List<Face>
        {
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
                new SolidTriangle(Vertices[],Vertices[],Vertices[]),
            })
        };

        internal static readonly IList<Face> TextureFaces = new List<Face>
        {
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            }),
            new Face(new List<Triangle>
            {
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
                new TextureTriangle(Vertices[],Vertices[],Vertices[]),
            })
        };

        #endregion
    }
}