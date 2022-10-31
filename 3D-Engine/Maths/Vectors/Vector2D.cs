/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a two-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector2D has a size of 8 bytes, so, where possible, a Vector2D should be passed by reference to reduce unnecessary copying (depending on the architecture of the machine).
 */

using _3D_Engine.Constants;
using Imagenic.Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace Imagenic.Core.Maths.Vectors;

/// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Vector2D']/*"/>
[Serializable]
public struct Vector2D : IApproximatelyEquatable<Vector2D>,
    IAdditionOperators<Vector2D, Vector2D, Vector2D>,
    IEqualityOperators<Vector2D, Vector2D>,
    IVector<Vector2D>
{
    #region Fields and Properties

    // Common Vectors
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (0, 0).
    /// </summary>
    public static readonly Vector2D Zero = new(0, 0);
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (1, 1).
    /// </summary>
    public static readonly Vector2D One = new(1, 1);
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (1, 0).
    /// </summary>
    public static readonly Vector2D UnitX = new(1, 0);
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (0, 1).
    /// </summary>
    public static readonly Vector2D UnitY = new(0, 1);
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (-1, 0).
    /// </summary>
    public static readonly Vector2D UnitNegativeX = new(-1, 0);
    /// <summary>
    /// A <see cref="Vector2D"/> equal to (0, -1).
    /// </summary>
    public static readonly Vector2D UnitNegativeY = new(0, -1);

    // Vector Contents
    public float x;
    public float y;

    // Variations
    public Vector2D YX => new(y, x);

    Vector2D IVector<Vector2D>.Zero => throw new NotImplementedException();

    Vector2D IVector<Vector2D>.One => throw new NotImplementedException();

    public static Vector2D NegativeOne => throw new NotImplementedException();

    public int Radix => throw new NotImplementedException();

    public static Vector2D AdditiveIdentity => throw new NotImplementedException();

    public static Vector2D MultiplicativeIdentity => throw new NotImplementedException();

    Vector2D IVector<Vector2D>.Zero => throw new NotImplementedException();

    Vector2D IVector<Vector2D>.One => throw new NotImplementedException();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Vector2D"/> from two values.
    /// </summary>
    /// <param name="x">The value to be put at the x component of the <see cref="Vector2D"/>.</param>
    /// <param name="y">The value to be put at the y component of the <see cref="Vector2D"/>.</param>
    public Vector2D(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Creates a <see cref="Vector2D"/> from an array of elements.
    /// </summary>
    /// <param name="elements">The array containing elements to be put in the <see cref="Vector2D"/>.</param>
    public Vector2D(float[] elements)
    {
        if (elements.Length < 2) throw new ArgumentException(Exceptions.Vector2DParameterLength, nameof(elements));
        x = elements[0];
        y = elements[1];
    }

    public Vector2D(IEnumerable<float> elements)
    {
        ExceptionHelper.ThrowIfParameterIsNull(elements, nameof(elements));
        var elementsArray = elements.ToArray();
        if (elementsArray.Length < 2)
        {
            // throw exception
        }
        x = elementsArray[0];
        y = elementsArray[1];
    }

    #endregion

    #region Methods

    public readonly bool IsZero(float epsilon = float.Epsilon) => ApproxEquals(Zero, epsilon);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public readonly float Angle(Vector2D v, float epsilon = float.Epsilon)
    {
        if (IsZero(epsilon))
        {
            throw MessageBuilder<CannotCalculateAngleBetweenTwoVectorsMessage>.Instance()
                .AddParameter(this)
                .BuildIntoException<InvalidOperationException>();
        }
        if (v.IsZero(epsilon))
        {
            throw MessageBuilder<CannotCalculateAngleBetweenTwoVectorsMessage>.Instance()
                .AddParameter(v)
                .BuildIntoException<InvalidOperationException>();
        }
        float quotient = this * v / Sqrt(SquaredMagnitude() * v.SquaredMagnitude());
        return Acos(quotient.Clamp(-1, 1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <param name="angle"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public readonly bool TryGetAngle(Vector2D v, out float angle, float epsilon = float.Epsilon)
    {
        angle = 0;
        if (IsZero(epsilon) || v.IsZero(epsilon))
        {
            return false;
        }
        float quotient = this * v / Sqrt(SquaredMagnitude() * v.SquaredMagnitude());
        angle = Acos(quotient.Clamp(-1, 1));
        return true;
    }

    /// <summary>
    /// Normalises this <see cref="Vector2D"/>.
    /// </summary>
    /// <param name="epsilon"></param>
    /// <returns>A normalised <see cref="Vector2D"/>.</returns>
    public readonly Vector2D Normalise(float epsilon = float.Epsilon)
    {
        if (IsZero(epsilon))
        {
            throw MessageBuilder<VectorCannotBeNormalisedMessage>.Instance()
                .AddParameter(this)
                .BuildIntoException<InvalidOperationException>();
        }
        return this / Magnitude();
    }    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public readonly bool TryNormalise(out Vector2D v, float epsilon = float.Epsilon)
    {
        v = Zero;
        if (IsZero(epsilon))
        {
            return false;
        }
        v = this / Magnitude();
        return true;
    }

    /// <summary>
    /// Calculates the magnitude of this <see cref="Vector2D"/>.
    /// </summary>
    /// <returns>The magnitude of this <see cref="Vector2D"/>.</returns>
    public readonly float Magnitude() => Sqrt(SquaredMagnitude());

    /// <summary>
    /// Calculates the squared magnitude of this <see cref="Vector2D"/>.
    /// </summary>
    /// <returns>The squared magnitude of this <see cref="Vector2D"/>.</returns>
    public readonly float SquaredMagnitude() => x * x + y * y;

    public readonly float Gradient(Vector2D other) => (other.y - y) / (other.x - x);

    public void Deconstruct(out float x, out float y)
    {
        x = this.x;
        y = this.y;
    }
    public readonly override string ToString() => $"({x}, {y})";
    public string ToString(string? format, IFormatProvider? formatProvider) => $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    // Operators
    public static Vector2D operator checked *(Vector2D v, float scalar) => checked(new(v.x * scalar, v.y * scalar));
    public static Vector2D operator *(Vector2D v, float scalar) => new(v.x * scalar, v.y * scalar);

    public static Vector2D operator checked *(float scalar, Vector2D v) => checked(v * scalar);
    public static Vector2D operator *(float scalar, Vector2D v) => v * scalar;

    public static Vector2D operator checked +(Vector2D v1, Vector2D v2) => checked(new(v1.x + v2.x, v1.y + v2.y));
    public static Vector2D operator +(Vector2D v1, Vector2D v2) => new(v1.x + v2.x, v1.y + v2.y);

    public static Vector2D operator checked -(Vector2D v1, Vector2D v2) => checked(new(v1.x - v2.x, v1.y - v2.y));
    public static Vector2D operator -(Vector2D v1, Vector2D v2) => new(v1.x - v2.x, v1.y - v2.y);

    public static float operator checked *(Vector2D v1, Vector2D v2) => checked(v1.x * v2.x + v1.y * v2.y);
    public static float operator *(Vector2D v1, Vector2D v2) => v1.x * v2.x + v1.y * v2.y;

    public static Vector2D operator checked /(Vector2D v, float scalar) => checked(new(v.x / scalar, v.y / scalar));
    public static Vector2D operator /(Vector2D v, float scalar) => new(v.x / scalar, v.y / scalar);

    public static Vector2D operator checked -(Vector2D v) => checked(new(-v.x, -v.y));
    public static Vector2D operator -(Vector2D v) => new(-v.x, -v.y);

    #endregion

    #region Vector Operations

    // Common






    // Equality and miscellaneous
    public static bool operator ==(Vector2D v1, Vector2D v2) => v1.x == v2.x && v1.y == v2.y;

    public static bool operator !=(Vector2D v1, Vector2D v2) => !(v1 == v2);

    public readonly bool Equals(Vector2D v) => this == v;

    public readonly bool ApproxEquals(Vector2D v, float epsilon = float.Epsilon) => x.ApproxEquals(v.x, epsilon) && y.ApproxEquals(v.y, epsilon);

    public override readonly bool Equals(object obj) => this == (Vector2D)obj;

    public override int GetHashCode() => (x, y).GetHashCode();

    public static Vector2D Parse(string s, IFormatProvider? provider) => new(s.Split(',').Select(e => float.Parse(e, provider)));
    
        
    

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Vector2D result)
    {
        throw new NotImplementedException();
    }













    #endregion

    #region Casting

    public static implicit operator Vector3D(Vector2D v) => new(v);

    #endregion
}