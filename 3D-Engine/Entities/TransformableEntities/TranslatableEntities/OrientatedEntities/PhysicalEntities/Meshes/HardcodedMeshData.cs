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

    

    

    

    

    

    

    

    

    #endregion

    #region Methods

    

    

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