using System;
using System.Linq;

namespace Imagenic.Core.Entities;

public static class FaceTransformations
{
    public static TFace Join<TFace>(this TFace f1, TFace f2) where TFace : Face
    {
        f1.Triangles.AddRange(f2.Triangles);
        return f1;
    }
}