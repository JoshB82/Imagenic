using System.Collections.Generic;

namespace Imagenic.Core.Maths;

public interface IApproximateEqualityComparer<T> : IEqualityComparer<T>
{
    public float Epsilon { get; set; }
}