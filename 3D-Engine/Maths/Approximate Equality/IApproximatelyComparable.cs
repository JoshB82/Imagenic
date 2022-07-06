using System;

namespace Imagenic.Core.Maths;

public interface IApproximatelyComparable<in T> : IComparable<T>
{
    public int ApproximatelyCompareTo(T? other, float epsilon);
}