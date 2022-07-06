using System;
using System.Numerics;

namespace Imagenic.Core.Maths.Vectors;

public interface IVector<TVector> :
    IApproximatelyEquatable<TVector>,
    IEqualityOperators<TVector, TVector>,
    IAdditionOperators<TVector, TVector, TVector>, // Element-wise addition
    IAdditiveIdentity<TVector, TVector>,
    ISubtractionOperators<TVector, TVector, TVector>, // Element-wise subtraction
    IMultiplyOperators<TVector, TVector, float>, // Dot product
    IMultiplicativeIdentity<TVector, TVector>,
    IDivisionOperators<TVector, float, TVector>, // Scalar division
    IUnaryNegationOperators<TVector, TVector>,
    ISpanFormattable
    where TVector : IVector<TVector>
{
    public static abstract TVector operator *(TVector v, float f); // Scalar multiplication
    public static abstract TVector operator *(float f, TVector v); // Scalar multiplication

    public static abstract TVector Zero { get; }
    public static abstract TVector One { get; }
    public static abstract TVector NegativeOne { get; }
    public int Radix { get; }

    public bool IsZero(float epsilon);

    public float Magnitude();
    public float SquaredMagnitude();
    public TVector Normalise(float epsilon);
    public bool TryNormalise(out TVector v, float epsilon);
    public float Angle(TVector v, float epsilon);
    public bool TryGetAngle(TVector v, out float angle, float epsilon);
}