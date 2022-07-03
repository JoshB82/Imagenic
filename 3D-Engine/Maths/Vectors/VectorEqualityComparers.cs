using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Maths.Vectors;

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