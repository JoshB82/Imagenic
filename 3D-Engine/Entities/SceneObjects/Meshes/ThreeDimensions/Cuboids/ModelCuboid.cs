using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Edges;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions.Cuboids
{
    internal static class MeshData
    {
        #region Fields and Properties

        // Cuboid
        internal static readonly IList<Vertex> CuboidVertices = GenerateCuboidVertices();

        internal static readonly IList<Vector3D> TextureVertices = new List<Vector3D>
        {
            new Vector3D(0, 0, 1), // 0
            new Vector3D(1, 0, 1), // 1
            new Vector3D(1, 1, 1), // 2
            new Vector3D(0, 1, 1) // 3
        };

        internal static readonly IList<Edge> CuboidEdges = new List<Edge>
        {
            new SolidEdge(CuboidVertices[0], CuboidVertices[1]), // 0
            new SolidEdge(CuboidVertices[1], CuboidVertices[2]), // 1
            new SolidEdge(CuboidVertices[2], CuboidVertices[3]), // 2
            new SolidEdge(CuboidVertices[3], CuboidVertices[0]), // 3
            new SolidEdge(CuboidVertices[4], CuboidVertices[5]), // 4
            new SolidEdge(CuboidVertices[5], CuboidVertices[6]), // 5
            new SolidEdge(CuboidVertices[6], CuboidVertices[7]), // 6
            new SolidEdge(CuboidVertices[7], CuboidVertices[4]), // 7
            new SolidEdge(CuboidVertices[0], CuboidVertices[4]), // 8
            new SolidEdge(CuboidVertices[1], CuboidVertices[5]), // 9
            new SolidEdge(CuboidVertices[2], CuboidVertices[6]), // 10
            new SolidEdge(CuboidVertices[3], CuboidVertices[7]) // 11
        };

        internal static readonly IList<Face> CuboidFaces = new List<Face>
        {
            new Face(new List<Triangle> // 0 []
            {
                new SolidTriangle(CuboidVertices[0], CuboidVertices[1], CuboidVertices[2]),
                new SolidTriangle(CuboidVertices[0], CuboidVertices[2], CuboidVertices[3])
            }),
            new Face(new List<Triangle> // 1 []
            {
                new SolidTriangle(CuboidVertices[1], CuboidVertices[5], CuboidVertices[6]),
                new SolidTriangle(CuboidVertices[1], CuboidVertices[6], CuboidVertices[2])
            }),
            new Face(new List<Triangle> // 2 []
            {
                new SolidTriangle(CuboidVertices[5], CuboidVertices[4], CuboidVertices[7]),
                new SolidTriangle(CuboidVertices[5], CuboidVertices[7], CuboidVertices[6])
            }),
            new Face(new List<Triangle> // 3 []
            {
                new SolidTriangle(CuboidVertices[4], CuboidVertices[0], CuboidVertices[3]),
                new SolidTriangle(CuboidVertices[4], CuboidVertices[3], CuboidVertices[7])
            }),
            new Face(new List<Triangle> // 4 []
            {
                new SolidTriangle(CuboidVertices[3], CuboidVertices[2], CuboidVertices[6]),
                new SolidTriangle(CuboidVertices[3], CuboidVertices[6], CuboidVertices[7])
            }),
            new Face(new List<Triangle> // 5 []
            {
                new SolidTriangle(CuboidVertices[1], CuboidVertices[0], CuboidVertices[4]),
                new SolidTriangle(CuboidVertices[1], CuboidVertices[4], CuboidVertices[5])
            })
        };

        #endregion

        #region Methods

        private static List<Vertex> GenerateCuboidVertices()
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

        internal static IList<Face> GenerateTextureFaces(Texture[] textures)
        {
            return new List<Face>
            {
                new Face(new List<Triangle> // 0 []
                {
                    new TextureTriangle(CuboidVertices[0], CuboidVertices[1], CuboidVertices[2], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[0]),
                    new TextureTriangle(CuboidVertices[0], CuboidVertices[2], CuboidVertices[3], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[0])
                }),
                new Face(new List<Triangle> // 1 []
                {
                    new TextureTriangle(CuboidVertices[1], CuboidVertices[5], CuboidVertices[6], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[1]),
                    new TextureTriangle(CuboidVertices[1], CuboidVertices[6], CuboidVertices[2], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[1])
                }),
                new Face(new List<Triangle> // 2 []
                {
                    new TextureTriangle(CuboidVertices[5], CuboidVertices[4], CuboidVertices[7], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[2]),
                    new TextureTriangle(CuboidVertices[5], CuboidVertices[7], CuboidVertices[6], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[2])
                }),
                new Face(new List<Triangle> // 3 []
                {
                    new TextureTriangle(CuboidVertices[4], CuboidVertices[0], CuboidVertices[3], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[3]),
                    new TextureTriangle(CuboidVertices[4], CuboidVertices[3], CuboidVertices[7], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[3])
                }),
                new Face(new List<Triangle> // 4 []
                {
                    new TextureTriangle(CuboidVertices[3], CuboidVertices[2], CuboidVertices[6], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[4]),
                    new TextureTriangle(CuboidVertices[3], CuboidVertices[6], CuboidVertices[7], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[4])
                }),
                new Face(new List<Triangle> // 5 []
                {
                    new TextureTriangle(CuboidVertices[1], CuboidVertices[0], CuboidVertices[4], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[5]),
                    new TextureTriangle(CuboidVertices[1], CuboidVertices[4], CuboidVertices[5], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[5])
                })
            };
        }

        #endregion
    }
}