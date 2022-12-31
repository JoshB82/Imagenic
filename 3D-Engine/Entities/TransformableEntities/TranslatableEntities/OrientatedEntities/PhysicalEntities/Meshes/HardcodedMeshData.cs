/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides hardcoded mesh data.
 */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Imagenic.Core.Entities;

internal static class HardcodedMeshData
{
    #region Fields and Properties

    // Cuboid
    internal static readonly IList<Vertex> CuboidVertices = GenerateCuboidVertices();

    internal static readonly IList<Vector3D> TextureVertices = new Vector3D[]
    {
        new Vector3D(0, 0, 1), // 0 [Bottom-left]
        new Vector3D(1, 0, 1), // 1 [Bottom-right]
        new Vector3D(1, 1, 1), // 2 [Top-right]
        new Vector3D(0, 1, 1) // 3 [Top-left]
    };

    internal static readonly IList<Edge> CuboidEdges = new List<Edge>
    {
        new SolidEdge(CuboidVertices[0], CuboidVertices[1]), // 0 [Back-bottom]
        new SolidEdge(CuboidVertices[1], CuboidVertices[2]), // 1 [Back-right]
        new SolidEdge(CuboidVertices[2], CuboidVertices[3]), // 2 [Back-top]
        new SolidEdge(CuboidVertices[3], CuboidVertices[0]), // 3 [Back-left]
        new SolidEdge(CuboidVertices[4], CuboidVertices[5]), // 4 [Front-bottom]
        new SolidEdge(CuboidVertices[5], CuboidVertices[6]), // 5 [Front-right]
        new SolidEdge(CuboidVertices[6], CuboidVertices[7]), // 6 [Front-top]
        new SolidEdge(CuboidVertices[7], CuboidVertices[4]), // 7 [Front-left]
        new SolidEdge(CuboidVertices[0], CuboidVertices[4]), // 8 [Middle-bottom-left]
        new SolidEdge(CuboidVertices[1], CuboidVertices[5]), // 9 [Middle-bottom-right]
        new SolidEdge(CuboidVertices[2], CuboidVertices[6]), // 10 [Middle-top-right]
        new SolidEdge(CuboidVertices[3], CuboidVertices[7]) // 11 [Middle-top-left]
    };

    internal static IList<Triangle> CuboidSolidTriangles = new List<Triangle>
    {
        new SolidTriangle(CuboidVertices[0], CuboidVertices[1], CuboidVertices[2]), // 0
        new SolidTriangle(CuboidVertices[0], CuboidVertices[2], CuboidVertices[3]), // 1
        new SolidTriangle(CuboidVertices[1], CuboidVertices[5], CuboidVertices[6]), // 2
        new SolidTriangle(CuboidVertices[1], CuboidVertices[6], CuboidVertices[2]), // 3
        new SolidTriangle(CuboidVertices[5], CuboidVertices[4], CuboidVertices[7]), // 4
        new SolidTriangle(CuboidVertices[5], CuboidVertices[7], CuboidVertices[6]), // 5
        new SolidTriangle(CuboidVertices[4], CuboidVertices[0], CuboidVertices[3]), // 6
        new SolidTriangle(CuboidVertices[4], CuboidVertices[3], CuboidVertices[7]), // 7
        new SolidTriangle(CuboidVertices[3], CuboidVertices[2], CuboidVertices[6]), // 8
        new SolidTriangle(CuboidVertices[3], CuboidVertices[6], CuboidVertices[7]), // 9
        new SolidTriangle(CuboidVertices[1], CuboidVertices[0], CuboidVertices[4]), // 10
        new SolidTriangle(CuboidVertices[1], CuboidVertices[4], CuboidVertices[5])  // 11
    };

    internal static readonly IList<Face> CuboidSolidFaces = new List<Face>
    {
        new SolidFace(new List<Triangle> // 0 [Back]
        {
            CuboidSolidTriangles[0],
            CuboidSolidTriangles[1]
        }),
        new SolidFace(new List<Triangle> // 1 [Right]
        {
            CuboidSolidTriangles[2],
            CuboidSolidTriangles[3]
        }),
        new SolidFace(new List<Triangle> // 2 [Front]
        {
            CuboidSolidTriangles[4],
            CuboidSolidTriangles[5]
        }),
        new SolidFace(new List<Triangle> // 3 [Left]
        {
            CuboidSolidTriangles[6],
            CuboidSolidTriangles[7]
        }),
        new SolidFace(new List<Triangle> // 4 [Top]
        {
            CuboidSolidTriangles[8],
            CuboidSolidTriangles[9]
        }),
        new SolidFace(new List<Triangle> // 5 [Bottom]
        {
            CuboidSolidTriangles[10],
            CuboidSolidTriangles[11]
        })
    };

    // Line
    internal static readonly IList<Vertex> LineVertices = new Vertex[2]
    {
        new Vertex(Vector3D.Zero), // 0
        new Vertex(Vector3D.One) // 1
    };

    internal static readonly IList<Edge> LineEdges = new Edge[1]
    {
        new SolidEdge(LineVertices[0], LineVertices[1]) // 0
    };

    // Plane
    internal static readonly IList<Vertex> PlaneVertices = new Vertex[4]
    {
        new(new Vector3D(0, 0, 0)), // 0 [Bottom-left]
        new(new Vector3D(1, 0, 0)), // 1 [Bottom-right]
        new(new Vector3D(1, 0, 1)), // 2 [Top-right]
        new(new Vector3D(0, 0, 1)) // 3 [Top-left]
    };

    internal static readonly IList<Edge> PlaneEdges = new Edge[4]
    {
        new SolidEdge(PlaneVertices[0], PlaneVertices[1]), // 0 []
        new SolidEdge(PlaneVertices[1], PlaneVertices[2]), // 1 []
        new SolidEdge(PlaneVertices[2], PlaneVertices[3]), // 2 []
        new SolidEdge(PlaneVertices[0], PlaneVertices[3]) // 3 []
    };

    internal static IList<Triangle> PlaneSolidTriangles = new Triangle[]
    {
        new SolidTriangle(PlaneVertices[0], PlaneVertices[1], PlaneVertices[2]), // 0 []
        new SolidTriangle(PlaneVertices[0], PlaneVertices[2], PlaneVertices[3]) // 1 []
    };

    internal static readonly IList<Face> PlaneSolidFaces = new Face[1]
    {
        new SolidFace(PlaneSolidTriangles[0], PlaneSolidTriangles[1])
    };

    #endregion

    #region Methods

    

    private static List<Vertex> GenerateCuboidVertices()
    {
        float radical = MathF.Sqrt(3) / 3;
        return new List<Vertex>
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
    }

    internal static IList<Face> GenerateCuboidTextureFaces(IList<Triangle> triangles)
    {
        return new List<Face>
        {
            new SolidFace(new List<Triangle> // 0 [Back]
            {
                triangles[0],
                triangles[1]
            }),
            new SolidFace(new List<Triangle> // 1 [Right]
            {
                triangles[2],
                triangles[3]
            }),
            new SolidFace(new List<Triangle> // 2 [Front]
            {
                triangles[4],
                triangles[5]
            }),
            new SolidFace(new List<Triangle> // 3 [Left]
            {
                triangles[6],
                triangles[7]
            }),
            new SolidFace(new List<Triangle> // 4 [Top]
            {
                triangles[8],
                triangles[9]
            }),
            new SolidFace(new List<Triangle> // 5 [Bottom]
            {
                triangles[10],
                triangles[11]
            })
        };
    }

    internal static IList<Triangle> GenerateCuboidTextureTriangles(Texture[] textures)
    {
        return new List<Triangle>
        {
            new TextureTriangle(CuboidVertices[0], CuboidVertices[1], CuboidVertices[2], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[0]), // 0
            new TextureTriangle(CuboidVertices[0], CuboidVertices[2], CuboidVertices[3], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[0]), // 1
            new TextureTriangle(CuboidVertices[1], CuboidVertices[5], CuboidVertices[6], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[1]), // 2
            new TextureTriangle(CuboidVertices[1], CuboidVertices[6], CuboidVertices[2], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[1]), // 3
            new TextureTriangle(CuboidVertices[5], CuboidVertices[4], CuboidVertices[7], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[2]), // 4
            new TextureTriangle(CuboidVertices[5], CuboidVertices[7], CuboidVertices[6], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[2]), // 5
            new TextureTriangle(CuboidVertices[4], CuboidVertices[0], CuboidVertices[3], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[3]), // 6
            new TextureTriangle(CuboidVertices[4], CuboidVertices[3], CuboidVertices[7], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[3]), // 7
            new TextureTriangle(CuboidVertices[3], CuboidVertices[2], CuboidVertices[6], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[4]), // 8
            new TextureTriangle(CuboidVertices[3], CuboidVertices[6], CuboidVertices[7], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[4]), // 9
            new TextureTriangle(CuboidVertices[1], CuboidVertices[0], CuboidVertices[4], TextureVertices[0], TextureVertices[1], TextureVertices[2], textures[5]), // 10
            new TextureTriangle(CuboidVertices[1], CuboidVertices[4], CuboidVertices[5], TextureVertices[0], TextureVertices[2], TextureVertices[3], textures[5]) // 11
        };
    }

    internal static IList<Triangle> GeneratePlaneSolidTriangles(Color frontColour, Color backColour)
    {
        return new Triangle[2]
        {
            new SolidTriangle(PlaneVertices[0], PlaneVertices[1], PlaneVertices[2], frontColour, backColour),
            new SolidTriangle(PlaneVertices[0], PlaneVertices[2], PlaneVertices[3], frontColour, backColour)
        };
    }

    internal static IList<Triangle> GeneratePlaneGradientTriangles(Gradient frontGradient, Gradient backGradient)
    {
        return new Triangle[2]
        {
            new GradientTriangle(PlaneVertices[0], PlaneVertices[1], PlaneVertices[2], frontGradient, backGradient),
            new GradientTriangle(PlaneVertices[0], PlaneVertices[2], PlaneVertices[3], frontGradient, backGradient)
        };
    }

    internal static IList<Triangle> GeneratePlaneTextureTriangles(Texture texture)
    {
        return new Triangle[2]
        {
            new TextureTriangle(PlaneVertices[0], PlaneVertices[1], PlaneVertices[2], TextureVertices[0], TextureVertices[1], TextureVertices[2], texture), // 0 []
            new TextureTriangle(PlaneVertices[0], PlaneVertices[2], PlaneVertices[3], TextureVertices[0], TextureVertices[2], TextureVertices[3], texture) // 1 []
        };
    }

    internal static IList<Face> GeneratePlaneSolidFace(IList<SolidTriangle> triangles)
    {
        return new Face[1]
        {
            new SolidFace(triangles[0], triangles[1])
        };
    }

    internal static IList<Face> GeneratePlaneGradientFace(IList<Triangle> triangles)
    {
        return new Face[1]
        {
            new GradientFace(triangles[0], triangles[1])
        };
    }

    internal static IList<Face> GeneratePlaneTextureFace(IList<Triangle> triangles)
    {
        return new Face[1]
        {
            new SolidFace(triangles[0], triangles[1])
        };
    }

    #endregion
}