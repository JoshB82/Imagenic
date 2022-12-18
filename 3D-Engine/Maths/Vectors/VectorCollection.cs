namespace Imagenic.Core.Maths.Vectors;

internal struct VectorTriple<T> where T : IVector<T>
{
    internal T p1, p2, p3;

	internal VectorTriple(T p1, T p2, T p3)
	{
		this.p1 = p1;
		this.p2 = p2;
		this.p3 = p3;
	}
}

internal static class VectorTripleExtensions
{
	internal static VectorTriple<Vector4D> ApplyMatrix(this VectorTriple<Vector4D> vt, Matrix4x4 matrix)
	{
		vt.p1 = matrix * vt.p1;
        vt.p2 = matrix * vt.p2;
        vt.p3 = matrix * vt.p3;
		return vt;
    }
}