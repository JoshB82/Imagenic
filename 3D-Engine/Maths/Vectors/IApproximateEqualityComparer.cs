using System.Collections.Generic;

namespace Imagenic.Core.Maths.Vectors;

public interface IApproximateEqualityComparer<T> : IEqualityComparer<T>
{
    public float Epsilon { get; set; }
}