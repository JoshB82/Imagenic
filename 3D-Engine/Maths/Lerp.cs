using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Maths;

public class Lerp<T> : INumber<T> where T : INumber<T>
{
    public static T One => throw new NotImplementedException();

    public static T Zero => throw new NotImplementedException();

    public static T AdditiveIdentity => throw new NotImplementedException();

    public static T MultiplicativeIdentity => throw new NotImplementedException();

    public static T Abs(T value)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<float> Between(float start, float end, int numSteps)
    {
        float stepSize = (end - start) / numSteps;
        for (int i = 1; i <= numSteps; i++)
        {
            yield return start + i * stepSize;
        }
    }

    public static IEnumerable<Vector3D> Between(Vector3D start, Vector3D end, int numSteps)
    {
        Vector3D stepSize = (end - start) / numSteps;
        for (int i = 1; i <= numSteps; i++)
        {
            yield return start + i * stepSize;
        }
    }

    public static IEnumerable<T> Between<T>(T start, T end, int numSteps) where T : INumber<T>
    {
        T stepSize = (end - start) / numSteps;
        for (int i = 1; i <= numSteps; i++)
        {
            yield return start + i * stepSize;
        }
    }

    public static T Clamp(T value, T min, T max)
    {
        throw new NotImplementedException();
    }

    public static T Create<TOther>(TOther value) where TOther : INumber<TOther>
    {
        throw new NotImplementedException();
    }

    public static T CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>
    {
        throw new NotImplementedException();
    }

    public static T CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>
    {
        throw new NotImplementedException();
    }

    public static (T Quotient, T Remainder) DivRem(T left, T right)
    {
        throw new NotImplementedException();
    }

    public static T Max(T x, T y)
    {
        throw new NotImplementedException();
    }

    public static T Min(T x, T y)
    {
        throw new NotImplementedException();
    }

    public static T Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static T Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static T Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static T Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static T Sign(T value)
    {
        throw new NotImplementedException();
    }

    public static bool TryCreate<TOther>(TOther value, out T result) where TOther : INumber<TOther>
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out T result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out T result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out T result)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out T result)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(T? other)
    {
        throw new NotImplementedException();
    }

    public bool Equals(T? other)
    {
        throw new NotImplementedException();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }
}