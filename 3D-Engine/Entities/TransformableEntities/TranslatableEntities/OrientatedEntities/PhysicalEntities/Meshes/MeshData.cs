using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

internal static class MeshData
{
    #region Fields and Properties

    private static float radical = MathF.Sqrt(3) / 3;

    internal static readonly IList<Vector3D> TextureVertices = new Vector3D[]
    {
        new Vector3D(0, 0, 1), // 0 [Bottom-left]
        new Vector3D(1, 0, 1), // 1 [Bottom-right]
        new Vector3D(1, 1, 1), // 2 [Top-right]
        new Vector3D(0, 1, 1) // 3 [Top-left]
    };

    // Cuboid
    internal static readonly IList<Vertex> CuboidVertices = new List<Vertex>
    {
        new Vertex(Vector3D.Zero, new Vector3D(-radical, -radical, -radical)), // 0 [Back-bottom-left]
        new Vertex(Vector3D.UnitX, new Vector3D(radical, -radical, -radical)), // 1 [Back-bottom-right]
        new Vertex(new Vector3D(1, 1, 0), new Vector3D(radical, radical, -radical)), // 2 [Back-top-right]
        new Vertex(Vector3D.UnitY, new Vector3D(-radical, radical, -radical)), // 3 [Back-top-left]
        new Vertex(Vector3D.UnitZ, new Vector3D(-radical, -radical, radical)), // 4 [Front-bottom-left]
        new Vertex(new Vector3D(1, 0, 1), new Vector3D(radical, -radical, radical)), // 5 [Front-bottom-right]
        new Vertex(Vector3D.One, new Vector3D(radical, radical, radical)), // 6 [Front-top-right]
        new Vertex(new Vector3D(0, 1, 1), new Vector3D(-radical, radical, radical)) // 7 [Front-top-left]
    };

    // Line
    internal static readonly IList<Vertex> LineVertices = new Vertex[2]
    {
        new Vertex(Vector3D.Zero), // 0
        new Vertex(Vector3D.One) // 1
    };

    // Plane
    internal static readonly IList<Vertex> PlaneVertices = new Vertex[4]
    {
        new(new Vector3D(0, 0, 0)), // 0 [Bottom-left]
        new(new Vector3D(1, 0, 0)), // 1 [Bottom-right]
        new(new Vector3D(1, 0, 1)), // 2 [Top-right]
        new(new Vector3D(0, 0, 1)) // 3 [Top-left]
    };

    #endregion

    #region Methods

    internal static IList<Edge> GenerateCuboidEdges(EdgeStyle style)
    {
        return new List<Edge>
        {
            new Edge(style, CuboidVertices[0], CuboidVertices[1]), // 0 [Back-bottom]
            new Edge(style, CuboidVertices[1], CuboidVertices[2]), // 1 [Back-right]
            new Edge(style, CuboidVertices[2], CuboidVertices[3]), // 2 [Back-top]
            new Edge(style, CuboidVertices[3], CuboidVertices[0]), // 3 [Back-left]
            new Edge(style, CuboidVertices[4], CuboidVertices[5]), // 4 [Front-bottom]
            new Edge(style, CuboidVertices[5], CuboidVertices[6]), // 5 [Front-right]
            new Edge(style, CuboidVertices[6], CuboidVertices[7]), // 6 [Front-top]
            new Edge(style, CuboidVertices[7], CuboidVertices[4]), // 7 [Front-left]
            new Edge(style, CuboidVertices[0], CuboidVertices[4]), // 8 [Middle-bottom-left]
            new Edge(style, CuboidVertices[1], CuboidVertices[5]), // 9 [Middle-bottom-right]
            new Edge(style, CuboidVertices[2], CuboidVertices[6]), // 10 [Middle-top-right]
            new Edge(style, CuboidVertices[3], CuboidVertices[7]) // 11 [Middle-top-left]
        };
    }

    internal static IList<Triangle> GenerateCuboidTriangles(FaceStyle[] frontStyles, FaceStyle[] backStyles)
    {
        return new List<Triangle>
        {
            new Triangle(frontStyles[2], backStyles[2], CuboidVertices[0], CuboidVertices[1], CuboidVertices[2]), // 0 [Back-1]
            new Triangle(frontStyles[2], backStyles[2], CuboidVertices[0], CuboidVertices[2], CuboidVertices[3]), // 1 [Back-2]
            new Triangle(frontStyles[1], backStyles[1], CuboidVertices[1], CuboidVertices[5], CuboidVertices[6]), // 2 [Right-1]
            new Triangle(frontStyles[1], backStyles[1], CuboidVertices[1], CuboidVertices[6], CuboidVertices[2]), // 3 [Right-2]
            new Triangle(frontStyles[0], backStyles[0], CuboidVertices[5], CuboidVertices[4], CuboidVertices[7]), // 4 [Front-1]
            new Triangle(frontStyles[0], backStyles[0], CuboidVertices[5], CuboidVertices[7], CuboidVertices[6]), // 5 [Front-2]
            new Triangle(frontStyles[3], backStyles[3], CuboidVertices[4], CuboidVertices[0], CuboidVertices[3]), // 6 [Left-1]
            new Triangle(frontStyles[3], backStyles[3], CuboidVertices[4], CuboidVertices[3], CuboidVertices[7]), // 7 [Left-2]
            new Triangle(frontStyles[4], backStyles[4], CuboidVertices[3], CuboidVertices[2], CuboidVertices[6]), // 8 [Top-1]
            new Triangle(frontStyles[4], backStyles[4], CuboidVertices[3], CuboidVertices[6], CuboidVertices[7]), // 9 [Top-2]
            new Triangle(frontStyles[5], backStyles[5], CuboidVertices[1], CuboidVertices[0], CuboidVertices[4]), // 10 [Bottom-1]
            new Triangle(frontStyles[5], backStyles[5], CuboidVertices[1], CuboidVertices[4], CuboidVertices[5])  // 11 [Bottom-2]
        };
    }

    internal static IList<Triangle> GenerateCuboidTriangles(FaceStyle frontStyle, FaceStyle[] backStyles)
    {
        return new List<Triangle>
        {
            new Triangle(frontStyle, backStyles[2], CuboidVertices[0], CuboidVertices[1], CuboidVertices[2]), // 0 [Back-1]
            new Triangle(frontStyle, backStyles[2], CuboidVertices[0], CuboidVertices[2], CuboidVertices[3]), // 1 [Back-2]
            new Triangle(frontStyle, backStyles[1], CuboidVertices[1], CuboidVertices[5], CuboidVertices[6]), // 2 [Right-1]
            new Triangle(frontStyle, backStyles[1], CuboidVertices[1], CuboidVertices[6], CuboidVertices[2]), // 3 [Right-2]
            new Triangle(frontStyle, backStyles[0], CuboidVertices[5], CuboidVertices[4], CuboidVertices[7]), // 4 [Front-1]
            new Triangle(frontStyle, backStyles[0], CuboidVertices[5], CuboidVertices[7], CuboidVertices[6]), // 5 [Front-2]
            new Triangle(frontStyle, backStyles[3], CuboidVertices[4], CuboidVertices[0], CuboidVertices[3]), // 6 [Left-1]
            new Triangle(frontStyle, backStyles[3], CuboidVertices[4], CuboidVertices[3], CuboidVertices[7]), // 7 [Left-2]
            new Triangle(frontStyle, backStyles[4], CuboidVertices[3], CuboidVertices[2], CuboidVertices[6]), // 8 [Top-1]
            new Triangle(frontStyle, backStyles[4], CuboidVertices[3], CuboidVertices[6], CuboidVertices[7]), // 9 [Top-2]
            new Triangle(frontStyle, backStyles[5], CuboidVertices[1], CuboidVertices[0], CuboidVertices[4]), // 10 [Bottom-1]
            new Triangle(frontStyle, backStyles[5], CuboidVertices[1], CuboidVertices[4], CuboidVertices[5])  // 11 [Bottom-2]
        };
    }

    internal static IList<Face> GenerateCuboidFaces(FaceStyle[] frontStyles, FaceStyle[] backStyles)
    {
        IList<Triangle> triangles = GenerateCuboidTriangles(frontStyles, backStyles);
        return new List<Face>
        {
            new Face(frontStyles[2], backStyles[2], triangles[0], triangles[1]),// 0 [Back]
            new Face(frontStyles[1], backStyles[1], triangles[2], triangles[3]), // 1 [Right]
            new Face(frontStyles[0], backStyles[0], triangles[4], triangles[5]), // 2 [Front]
            new Face(frontStyles[3], backStyles[3], triangles[6], triangles[7]), // 3 [Left]
            new Face(frontStyles[4], backStyles[4], triangles[8], triangles[9]), // 4 [Top]
            new Face(frontStyles[5], backStyles[5], triangles[10], triangles[11]) // 5 [Bottom]
        };
    }

    internal static IList<Face> GenerateCuboidFaces(IList<Triangle> triangles)
    {
        return new List<Face>
        {
            new Face(triangles[0], triangles[1]),// 0 [Back]
            new Face(triangles[2], triangles[3]), // 1 [Right]
            new Face(triangles[4], triangles[5]), // 2 [Front]
            new Face(triangles[6], triangles[7]), // 3 [Left]
            new Face(triangles[8], triangles[9]), // 4 [Top]
            new Face(triangles[10], triangles[11]) // 5 [Bottom]
        };
    }

    internal static IList<Edge> GenerateLineEdges(EdgeStyle style)
    {
        return new Edge[1]
        {
            new Edge(style, LineVertices[0], LineVertices[1]) // 0
        };
    }

    // Plane


    internal static IList<Edge> GeneratePlaneEdges(EdgeStyle style)
    {
        return new Edge[4]
        {
            new Edge(style, PlaneVertices[0], PlaneVertices[1]), // 0 []
            new Edge(style, PlaneVertices[1], PlaneVertices[2]), // 1 []
            new Edge(style, PlaneVertices[2], PlaneVertices[3]), // 2 []
            new Edge(style, PlaneVertices[0], PlaneVertices[3]) // 3 []
        };
    }

    internal static IList<Triangle> GeneratePlaneTriangles(FaceStyle frontStyle, FaceStyle backStyle)
    {
        return new Triangle[]
        {
            new Triangle(frontStyle, backStyle, PlaneVertices[0], PlaneVertices[1], PlaneVertices[2]), // 0 []
            new Triangle(frontStyle, backStyle, PlaneVertices[0], PlaneVertices[2], PlaneVertices[3]) // 1 []
        };
    }

    internal static IList<Face> GeneratePlaneFaces(FaceStyle frontStyle, FaceStyle backStyle)
    {
        IList<Triangle> triangles = GeneratePlaneTriangles(frontStyle, backStyle);
        return new Face[1]
        {
            new Face(frontStyle, backStyle, triangles[0], triangles[1])
        };
    }

    #endregion
}