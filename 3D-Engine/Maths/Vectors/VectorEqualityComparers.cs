using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Maths.Vectors;

public class ApproximateEqualityComparer<T> : EqualityComparer<T>, IApproximateEqualityComparer<T> where T : IApproximatelyEquatable<T>
{
    public static new ApproximateEqualityComparer<T> Default => new();

    public float Epsilon { get; set; }

    public override bool Equals(T? x, T? y)
    {
        if (x is null && y is null)
        {
            return true;
        }
        else if (x is null && y is not null)
        {
            return false;
        }
        else
        {
            return x!.ApproxEquals(y, Epsilon);
        }
    }

    public bool ApproxEquals(T? x, T? y, float epsilon = float.Epsilon)
    {
        if (x is null && y is null)
        {
            return true;
        }
        else if (x is null && y is not null)
        {
            return false;
        }
        else
        {
            return x!.ApproxEquals(y, epsilon);
        }
    }

    public int GetHashCode([DisallowNull] T obj)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(T? x, T? y) where T : default
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode([DisallowNull] T obj)
    {
        throw new NotImplementedException();
    }
}

public sealed class FloatApproximateEqualityComparer : IApproximateEqualityComparer<float>
{
    public float Epsilon { get; set; }

    public bool ApproxEquals(float x, float y, float epsilon = float.Epsilon) => x.ApproxEquals(y, epsilon);

    public bool Equals(float x, float y) => x.ApproxEquals(y, Epsilon);

    public int GetHashCode([DisallowNull] float obj)
    {
        throw new NotImplementedException();
    }
}

public class Vector2DApproximateEqualityComparer : IApproximateEqualityComparer<Vector2D>
{
    #region Fields and Properties

    public float Epsilon { get; set; }

    #endregion

    #region Constructors

    public Vector2DApproximateEqualityComparer(float epsilon = float.Epsilon)
    {
        Epsilon = epsilon;
    }

    #endregion

    #region Methods

    public bool Equals(Vector2D x, Vector2D y)
    {
        return x.ApproxEquals(y, Epsilon);
    }

    public int GetHashCode([DisallowNull] Vector2D obj)
    {
        throw new NotImplementedException();
    }

    #endregion
}

public class Vector3DApproximateEqualityComparer : IApproximateEqualityComparer<Vector3D>
{
    #region Fields and Properties

    public float Epsilon { get; set; }

    #endregion

    #region Constructors

    public Vector3DApproximateEqualityComparer(float epsilon = float.Epsilon)
    {
        Epsilon = epsilon;
    }

    #endregion

    #region Methods

    public bool Equals(Vector3D x, Vector3D y)
    {
        return x.ApproxEquals(y, Epsilon);
    }

    public int GetHashCode([DisallowNull] Vector3D obj)
    {
        throw new NotImplementedException();
    }

    #endregion
}

public class Vector4DApproximateEqualityComparer : IApproximateEqualityComparer<Vector4D>
{
    #region Fields and Properties

    public float Epsilon { get; set; }

    #endregion

    #region Constructors

    public Vector4DApproximateEqualityComparer(float epsilon = float.Epsilon)
    {
        Epsilon = epsilon;
    }

    #endregion

    #region Methods

    public bool Equals(Vector4D x, Vector4D y)
    {
        return x.ApproxEquals(y);
    }

    public int GetHashCode([DisallowNull] Vector4D obj)
    {
        throw new NotImplementedException();
    }

    #endregion
}