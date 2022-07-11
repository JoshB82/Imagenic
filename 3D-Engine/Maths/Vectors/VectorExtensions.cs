using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Maths.Vectors;

/// <summary>
/// 
/// </summary>
public static class VectorExtensions
{
    private static readonly FloatApproximateEqualityComparer floatComparer = new();
    private static readonly ApproximateEqualityComparer<Vector3D> _3DVectorComparer = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static bool AreCollinear([DisallowNull] this IEnumerable<Vector2D> points, float epsilon = float.Epsilon)
    {
        ThrowIfNull(points);
        floatComparer.Epsilon = epsilon;
        return points.Zip(points.Skip(1), (a, b) => a.Gradient(b)).Distinct(floatComparer).Count() == 1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public static bool AreCollinear([DisallowNull] this IEnumerable<Vector3D> points, float epsilon = float.Epsilon)
    {
        ThrowIfNull(points);
        _3DVectorComparer.Epsilon = epsilon;
        return points.Zip(points.Skip(1), (a, b) => (b - a).Normalise()).Distinct(_3DVectorComparer).Count() == 1;
    }
}