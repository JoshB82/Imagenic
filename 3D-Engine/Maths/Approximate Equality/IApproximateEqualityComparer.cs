using System.Collections.Generic;

namespace Imagenic.Core.Maths;

public interface IApproximateEqualityComparer<in T> : IEqualityComparer<T>
{
    public bool ApproxEquals(T? x, T? y, float epsilon);
    public float Epsilon { get; set; }
}