using System;

namespace Imagenic.Core.Maths;

public interface IApproximatelyEquatable<T> : IEquatable<T>
{
    public bool ApproxEquals(T? other, float epsilon);
}