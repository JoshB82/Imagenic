/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a three-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector3D has a size of 12 bytes, so, where possible, a Vector3D should be passed by reference to reduce unnecessary copying.
 */

using _3D_Engine.Constants;
using Imagenic.Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Maths.Vectors;

/// <include file="Help_8.xml" path="doc/members/member[@name='TImage:_3D_Engine.Vector3D']/*"/>
[Serializable]
public struct Vector3D : IVector<Vector3D>, IEquatable<Vector3D>
{
    #region Fields and Properties
    
    // Common Vectors
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (0, 0, 0).
    /// </summary>
    public static readonly Vector3D Zero = new(0, 0, 0);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (1, 1, 1).
    /// </summary>
    public static readonly Vector3D One = new(1, 1, 1);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (-1, -1, -1).
    /// </summary>
    public static readonly Vector3D NegativeOne = new(-1, -1, -1);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (1, 0, 0).
    /// </summary>
    public static readonly Vector3D UnitX = new(1, 0, 0);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (0, 1, 0).
    /// </summary>
    public static readonly Vector3D UnitY = new(0, 1, 0);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (0, 0, 1).
    /// </summary>
    public static readonly Vector3D UnitZ = new(0, 0, 1);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (-1, 0, 0).
    /// </summary>
    public static readonly Vector3D UnitNegativeX = new(-1, 0, 0);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (0, -1, 0).
    /// </summary>
    public static readonly Vector3D UnitNegativeY = new(0, -1, 0);
    /// <summary>
    /// A <see cref="Vector3D"/> equal to (0, 0, -1).
    /// </summary>
    public static readonly Vector3D UnitNegativeZ = new(0, 0, -1);

    // Vector Contents
    /// <summary>
    /// The x-component of this <see cref="Vector3D"/>.
    /// </summary>
    public float x;
    /// <summary>
    /// The y-component of this <see cref="Vector3D"/>.
    /// </summary>
    public float y;
    /// <summary>
    /// The z-component of this <see cref="Vector3D"/>.
    /// </summary>
    public float z;

    // Variations
    public Vector2D XY => new(x, y);
    public Vector2D YX => new(y, x);
    public Vector2D XZ => new(x, z);
    public Vector2D ZX => new(z, x);
    public Vector2D YZ => new(y, z);
    public Vector2D ZY => new(z, y);
    public Vector3D XZY => new(x, z, y);
    public Vector3D YXZ => new(y, x, z);
    public Vector3D YZX => new(y, z, x);
    public Vector3D ZXY => new(z, x, y);
    public Vector3D ZYX => new(z, y, x);

    Vector3D IVector<Vector3D>.Zero => throw new NotImplementedException();

    Vector3D IVector<Vector3D>.One => throw new NotImplementedException();

    public static Vector3D NegativeOne => throw new NotImplementedException();

    public int Radix => throw new NotImplementedException();

    public static Vector3D AdditiveIdentity => Zero;

    public static Vector3D MultiplicativeIdentity => throw new NotImplementedException();

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Vector3D"/> from three values.
    /// </summary>
    /// <param name="x">The value to be put at the x component of the <see cref="Vector3D"/>.</param>
    /// <param name="y">The value to be put at the y component of the <see cref="Vector3D"/>.</param>
    /// <param name="z">The value to be put at the z component of the <see cref="Vector3D"/>.</param>
    public Vector3D(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Creates a <see cref="Vector3D"/> from the values in a <see cref="Vector2D"/> and a further value.
    /// </summary>
    /// <param name="v">
    /// The <see cref="Vector2D"/> containing values to be put in the <see cref="Vector3D"/> at the x and y components.
    /// </param>
    /// <param name="z">
    /// The value to be put in the <see cref="Vector3D"/> at the z component.
    /// </param>
    public Vector3D(Vector2D v, float z = 0)
    {
        x = v.x;
        y = v.y;
        this.z = z;
    }

    /// <summary>
    /// Creates a <see cref="Vector3D"/> from an array of elements.
    /// </summary>
    /// <param name="elements">The array containing elements to be put in the <see cref="Vector3D"/>.</param>
    public Vector3D(float[] elements)
    {
        if (elements.Length < 3) throw new ArgumentException(Exceptions.Vector3DParameterLength, nameof(elements));
        x = elements[0];
        y = elements[1];
        z = elements[2];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elements"></param>
    public Vector3D([DisallowNull] IEnumerable<float> elements)
    {
        ThrowIfNull(elements);

        using var enumerator = elements.GetEnumerator();
        enumerator.Reset();

        if (!enumerator.MoveNext()) ThrowTooSmallError();
        x = enumerator.Current;

        if (!enumerator.MoveNext()) ThrowTooSmallError();
        y = enumerator.Current;

        if (!enumerator.MoveNext()) ThrowTooSmallError();
        z = enumerator.Current;

        void ThrowTooSmallError()
        {
            throw MessageBuilder<ElementInputTooSmallMessage>.Instance()
                .AddParameter(elements)
                .BuildIntoException<InvalidOperationException>();
        }

        //-----

        /*
        var elementsArray = elements.ToArray();
        if (elementsArray.Length < 3)
        {
            // throw exception
        }
        x = elementsArray[0];
        y = elementsArray[1];
        z = elementsArray[2];

        */
    }

    #endregion

    #region Methods

    /// <summary>
    /// Indicates whether or not this <see cref="Vector3D"/> is equal to (0, 0, 0) (<see cref="Vector3D.Zero"/>).
    /// </summary>
    /// <param name="epsilon"></param>
    /// <returns></returns>
    public readonly bool IsZero(float epsilon = float.Epsilon) => ApproxEquals(Zero, epsilon);

    /// <summary>
    /// Calculates the smallest angle between two <see cref="Vector3D">Vector3Ds</see>.
    /// </summary>
    /// <param name="v">A <see cref="Vector3D"/> creating an angle from the current <see cref="Vector3D"/> instance.</param>
    /// <param name="epsilon"></param>
    /// <returns>The smallest angle between two <see cref="Vector3D">Vector3Ds</see>.</returns>
    public readonly float Angle(Vector3D v, float epsilon = float.Epsilon)
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

    public readonly bool TryGetAngle(Vector3D v, out float angle, float epsilon = float.Epsilon)
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
    /// Normalises this <see cref="Vector3D"/>.
    /// </summary>
    /// <param name="epsilon">The acceptable magnitude of error in any equality comparison.</param>
    /// <returns>A normalised <see cref="Vector3D"/>.</returns>
    public readonly Vector3D Normalise(float epsilon = float.Epsilon)
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
    public bool TryNormalise(out Vector3D v, float epsilon = float.Epsilon)
    {
        v = Zero;
        if (IsZero(epsilon))
        {
            return false;
        }
        v = this / Magnitude();
        return true;
    }

    public readonly float DistanceFrom(Vector3D point) => (this - point).Magnitude();

    /// <summary>
    /// Calculates the magnitude of this <see cref="Vector3D"/>.
    /// </summary>
    /// <returns>The magnitude of this <see cref="Vector3D"/>.</returns>
    public readonly float Magnitude() => Sqrt(SquaredMagnitude());

    /// <summary>
    /// Calculates the squared magnitude of this <see cref="Vector3D"/>.
    /// </summary>
    /// <returns>The squared magnitude of this <see cref="Vector3D"/>.</returns>
    public readonly float SquaredMagnitude() => x * x + y * y + z * z;

    public void Deconstruct(out float x, out float y, out float z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }
    public override readonly string ToString() => $"({x}, {y}, {z})";
    public readonly string ToString(string? format, IFormatProvider? formatProvider) => $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)}, {z.ToString(format, formatProvider)})";
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    // Operators
    public static Vector3D operator checked +(Vector3D v1, Vector3D v2) => checked(new(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z));
    public static Vector3D operator +(Vector3D v1, Vector3D v2) => new(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

    public static Vector3D operator checked *(Vector3D v, float scalar) => checked(new(v.x * scalar, v.y * scalar, v.z * scalar));
    public static Vector3D operator *(Vector3D v, float scalar) => new(v.x * scalar, v.y * scalar, v.z * scalar);

    public static Vector3D operator checked *(float scalar, Vector3D v) => checked(v * scalar);
    public static Vector3D operator *(float scalar, Vector3D v) => v * scalar;

    public static Vector3D operator checked -(Vector3D v1, Vector3D v2) => checked(new(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z));
    public static Vector3D operator -(Vector3D v1, Vector3D v2) => new(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

    public static float operator checked *(Vector3D v1, Vector3D v2) => checked(v1.x * v2.x + v1.y * v2.y + v1.z * v2.z);
    public static float operator *(Vector3D v1, Vector3D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

    public static Vector3D operator checked /(Vector3D v, float scalar) => checked(new(v.x / scalar, v.y / scalar, v.z / scalar));
    public static Vector3D operator /(Vector3D v, float scalar) => new(v.x / scalar, v.y / scalar, v.z / scalar);

    public static Vector3D operator checked -(Vector3D v) => checked(new(-v.x, -v.y, -v.z));
    public static Vector3D operator -(Vector3D v) => new(-v.x, -v.y, -v.z);

    #endregion

    #region Vector Operations

    // Common
    

    /// <summary>
    /// Finds the cross product of two <see cref="Vector3D">Vector3Ds</see>.
    /// </summary>
    /// <param name="v">A <see cref="Vector3D"/> used in calculating the cross product with the current <see cref="Vector3D"/> instance.</param>
    /// <returns>The cross product of two <see cref="Vector3D">Vector3Ds</see>.</returns>
    public readonly Vector3D CrossProduct(Vector3D v) => new Vector3D(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);

    

    

    // Equality and miscellaneous
    public static bool operator ==(Vector3D v1, Vector3D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;

    public static bool operator !=(Vector3D v1, Vector3D v2) => !(v1 == v2);

    public readonly bool Equals(Vector3D v) => this == v;

    public readonly bool ApproxEquals(Vector3D v, float epsilon = float.Epsilon) => x.ApproxEquals(v.x, epsilon) && y.ApproxEquals(v.y, epsilon) && z.ApproxEquals(v.z, epsilon);

    public override readonly bool Equals(object obj) => this == (Vector3D)obj;

    public override int GetHashCode() => (x, y, z).GetHashCode();

    

    #endregion

    #region Vector Operations (Geometry)

    public static Vector3D LineIntersectPlane(Vector3D lineStart, Vector3D lineFinish, Vector3D planePoint, Vector3D planeNormal, out float d)
    {
        Vector3D line = lineFinish - lineStart;
        float denominator = line * planeNormal;
        //if (denominator == 0) throw new ArgumentException("Line does not intersect plane or exists entirely on plane.");

        // d = new length / old length
        d = (planePoint - lineStart) * planeNormal / denominator;
        // Round in direction of normal!?
        return line * d + lineStart;
    }

    /// <summary>
    /// Calculates a normal vector to a plane defined by three points. Looking in the direction of the desired normal vector and at the points, the points are arranged in an clockwise order.
    /// </summary>
    /// <param name="p1">First point on the plane.</param>
    /// <param name="p2">Second point on the plane.</param>
    /// <param name="p3">Third point on the plane.</param>
    /// <returns>A normal vector.</returns>
    public static Vector3D NormalFromPlane(Vector3D p1, Vector3D p2, Vector3D p3) => (p3 - p1).CrossProduct(p2 - p1).Normalise();

    public static float PointDistanceFromPlane(Vector3D point, Vector3D planePoint, Vector3D planeNormal) => point * planeNormal - planePoint * planeNormal;

    

    

    

    

    

    

    

    

    #endregion

    #region Casting

    public static explicit operator Vector2D(Vector3D v) => new(v.x, v.y);

    public static implicit operator Vector4D(Vector3D v) => new(v);

    #endregion
}