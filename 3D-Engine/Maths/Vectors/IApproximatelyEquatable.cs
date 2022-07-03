using System;

namespace Imagenic.Core.Maths.Vectors;

public interface IApproximatelyEquatable<T> : IEquatable<T>
{
    public bool ApproxEquals(T? other, float epsilon);
}