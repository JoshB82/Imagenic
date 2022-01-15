using _3D_Engine.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Maths.Vectors;

internal static class VectorExtensions
{
    public static bool AreCollinear(this IEnumerable<Vector3D> points)
    {
        return points.Zip(points.Skip(1), (a, b) => (b - a).Normalise())
            .Distinct().Count() == 1;
    }

    public static float CalculateGradient(Vector2D point1, Vector2D point2)
    {
        return (point2.y - point1.y) / (point2.x - point1.x);
    }
}