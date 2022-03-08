using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Maths.Vectors;

public class Vector2DApproximateEqualityComparer : IEqualityComparer<Vector2D>
{
    public bool Equals(Vector2D x, Vector2D y)
    {
        return x.ApproxEquals(y);
    }

    public int GetHashCode([DisallowNull] Vector2D obj)
    {
        throw new NotImplementedException();
    }
}

public class Vector3DApproximateEqualityComparer : IEqualityComparer<Vector3D>
{
    public bool Equals(Vector3D x, Vector3D y)
    {
        return x.ApproxEquals(y);
    }

    public int GetHashCode([DisallowNull] Vector3D obj)
    {
        throw new NotImplementedException();
    }
}

public class Vector4DApproximateEqualityComparer : IEqualityComparer<Vector4D>
{
    public bool Equals(Vector4D x, Vector4D y)
    {
        return x.ApproxEquals(y);
    }

    public int GetHashCode([DisallowNull] Vector4D obj)
    {
        throw new NotImplementedException();
    }
}