using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using System;
using System.Drawing;

namespace Imagenic.Core.Renderers.Rasterising;

internal static class Interpolator
{
    // Interpolation (source!)
    internal static void InterpolateSolidStyle(DrawableTriangle triangle, Buffer2D<Color> colourBuffer,
        Action<DrawableTriangle, Buffer2D<Color>, int, int, float> action)
    {
        
    }

    internal static void InterpolateTextureStyle(Buffer2D<Color> colourBuffer, Bitmap texture,
                                                 int x1, int y1, float tx1, float ty1, float tz1,
                                                 int x2, int y2, float tx2, float ty2, float tz2,
                                                 int x3, int y3, float tx3, float ty3, float tz3)
    {
        
    }

    internal static void InterpolateGradientStyle(
        int x1, int y1, float z1,
        int x2, int y2, float z2,
        int x3, int y3, float z3,
        GradientStyle style)
    {

    }
}