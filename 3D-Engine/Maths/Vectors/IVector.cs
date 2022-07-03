using System.Numerics;

namespace Imagenic.Core.Maths.Vectors;

public interface IVector<TVector> :
    IApproximatelyEquatable<TVector>,
    IAdditionOperators<TVector, TVector, TVector>, // Element-wise addition
    IAdditiveIdentity<TVector, TVector>,
    ISubtractionOperators<TVector, TVector, TVector>, // Element-wise subtraction
    IMultiplyOperators<TVector, TVector, float>, // Dot product
    //IMultiplyOperators<TVector, float, TVector>,
    //IMultiplyOperators<float, TVector, TVector>,
    IMultiplicativeIdentity<TVector, TVector>,
    IDivisionOperators<TVector, float, TVector>, // Scalar division
    IUnaryNegationOperators<TVector, TVector>
    where TVector : IVector<TVector>
{
    public static abstract TVector operator *(TVector v, float f); // Scalar multiplication
    public static abstract TVector operator *(float f, TVector v); // Scalar multiplication
    // Zero

    public float Magnitude() => Sqrt(SquaredMagnitude());
    public float SquaredMagnitude();
    public TVector Normalise(float epsilon)
    {
        if (ApproxEquals(Zero, epsilon))
        {

        }
        else
        {
            return this / Magnitude();
        }
    }
}