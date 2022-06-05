/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a four-dimensional vector and provides methods to extract common information and for operator overloading. Each instance of a Vector4D has a size of 16 bytes, so, where possible, a Vector4D should be passed by reference to reduce unnecessary copying.
 */

using _3D_Engine.Constants;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.MathF;

namespace Imagenic.Core.Maths.Vectors;

/// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Vector4D']/*"/>
public struct Vector4D : IEquatable<Vector4D>
{
    #region Fields and Properties

    // Common Vectors
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 0, 0, 0).
    /// </summary>
    public static readonly Vector4D Zero = new();
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (1, 1, 1, 1).
    /// </summary>
    public static readonly Vector4D One = new(1, 1, 1, 1);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (1, 0, 0, 0).
    /// </summary>
    public static readonly Vector4D UnitX = new(1, 0, 0, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 1, 0, 0).
    /// </summary>
    public static readonly Vector4D UnitY = new(0, 1, 0, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 0, 1, 0).
    /// </summary>
    public static readonly Vector4D UnitZ = new(0, 0, 1, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 0, 0, 1).
    /// </summary>
    public static readonly Vector4D UnitW = new(0, 0, 0, 1);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (-1, 0, 0, 0).
    /// </summary>
    public static readonly Vector4D UnitNegativeX = new(-1, 0, 0, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, -1, 0, 0).
    /// </summary>
    public static readonly Vector4D UnitNegativeY = new(0, -1, 0, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 0, -1, 0).
    /// </summary>
    public static readonly Vector4D UnitNegativeZ = new(0, 0, -1, 0);
    /// <summary>
    /// A <see cref="Vector4D"/> equal to (0, 0, 0, -1).
    /// </summary>
    public static readonly Vector4D UnitNegativeW = new(0, 0, 0, -1);

    // Vector Contents
    /// <summary>
    /// First component of the <see cref="Vector4D"/>, equivalent to the q1 component of a <see cref="Quaternion"/>.
    /// </summary>
    public float x;
    /// <summary>
    /// Second component of the <see cref="Vector4D"/>, equivalent to the q2 component of a <see cref="Quaternion"/>.
    /// </summary>
    public float y;
    /// <summary>
    /// Third component of the <see cref="Vector4D"/>, equivalent to the q3 component of a <see cref="Quaternion"/>.
    /// </summary>
    public float z;
    /// <summary>
    /// Fourth component of the <see cref="Vector4D"/>, equivalent to the q4 component of a <see cref="Quaternion"/>.
    /// </summary>
    public float w;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="Vector4D"/> from four values.
    /// </summary>
    /// <param name="x">The value to be put at the x component of the <see cref="Vector4D"/>.</param>
    /// <param name="y">The value to be put at the y component of the <see cref="Vector4D"/>.</param>
    /// <param name="z">The value to be put at the z component of the <see cref="Vector4D"/>.</param>
    /// <param name="w">The value to be put at the w component of the <see cref="Vector4D"/>.</param>
    public Vector4D(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Creates a <see cref="Vector4D"/> from the values in a <see cref="Vector3D"/> and a further value.
    /// </summary>
    /// <param name="v">
    /// The <see cref="Vector3D"/> containing values to be put in the <see cref="Vector4D"/> at the x, y and z components.
    /// </param>
    /// <param name="w">
    /// The value to be put in the <see cref="Vector4D"/> at the w component.
    /// </param>
    public Vector4D(Vector3D v, float w = 0)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        this.w = w;
    }

    /// <summary>
    /// Creates a <see cref="Vector4D"/> from the values in a <see cref="Vector2D"/> and two further values.
    /// </summary>
    /// <param name="v">
    /// The <see cref="Vector2D"/> containing values to be put in the <see cref="Vector4D"/> at the x and y components.
    /// </param>
    /// <param name="z">
    /// The value to be put in the <see cref="Vector4D"/> at the z component.
    /// </param>
    /// <param name="w">
    /// The value to be put in the <see cref="Vector4D"/> at the w component.
    /// </param>
    public Vector4D(Vector2D v, float z = 0, float w = 0)
    {
        x = v.x;
        y = v.y;
        this.z = z;
        this.w = w;
    }

    /// <summary>
    /// Creates a <see cref="Vector4D"/> from an array of elements.
    /// </summary>
    /// <param name="elements">The array containing elements to be put in the <see cref="Vector4D"/>.</param>
    public Vector4D(float[] elements)
    {
        if (elements.Length < 4) throw new ArgumentException(Exceptions.FourParameterLength, nameof(elements));
        x = elements[0];
        y = elements[1];
        z = elements[2];
        w = elements[3];
    }

    public Vector4D(IEnumerable<float> elements)
    {
        ExceptionHelper.ThrowIfParameterIsNull(elements, nameof(elements));
        var elementsArray = elements.ToArray();
        if (elementsArray.Length < 4)
        {
            // throw exception
        }
        x = elementsArray[0];
        y = elementsArray[1];
        z = elementsArray[2];
        w = elementsArray[3];
    }

    #endregion

    public void Deconstruct(out float x, out float y, out float z, out float w)
    {
        x = this.x;
        y = this.y;
        z = this.z;
        w = this.w;
    }

    #region Vector Operations

    // Common
    public readonly float Angle(Vector4D v)
    {
        if (this == Zero || v == Zero) throw Exceptions.Angle;
        float quotient = this * v / (Magnitude() * v.Magnitude());
        if (quotient < -1) quotient = -1; if (quotient > 1) quotient = 1;
        return Acos(quotient);
    }

    /// <summary>
    /// Finds the magnitude of a <see cref="Vector4D"/>.
    /// </summary>
    /// <returns>The magnitude of a <see cref="Vector4D"/>.</returns>
    public readonly float Magnitude() => Sqrt(SquaredMagnitude());

    /// <summary>
    /// Finds the squared magnitude of a <see cref="Vector4D"/>.
    /// </summary>
    /// <returns>The squared magnitude of a <see cref="Vector4D"/>.</returns>
    public readonly float SquaredMagnitude() => x * x + y * y + z * z + w * w;

    /// <summary>
    /// Normalises a <see cref="Vector4D"/>.
    /// </summary>
    /// <returns>A normalised <see cref="Vector4D"/>.</returns>
    public readonly Vector4D Normalise() =>
        ApproxEquals(Zero, 1E-6f)
        ? throw Exceptions.Normalise
        : this / Magnitude();

    // Equality and miscellaneous
    public static bool operator ==(Vector4D v1, Vector4D v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;

    public static bool operator !=(Vector4D v1, Vector4D v2) => !(v1 == v2);

    public readonly bool Equals(Vector4D v) => this == v;

    public readonly bool ApproxEquals(Vector4D v, float epsilon = float.Epsilon) =>
        x.ApproxEquals(v.x, epsilon) &&
        y.ApproxEquals(v.y, epsilon) &&
        z.ApproxEquals(v.z, epsilon) &&
        w.ApproxEquals(v.w, epsilon);

    public override readonly bool Equals(object obj) => this == (Vector4D)obj;

    public override int GetHashCode() => (x, y, z, w).GetHashCode();

    public override readonly string ToString() => $"({x}, {y}, {z}, {w})";

    #endregion

    #region Operator Overloading

    public static Vector4D operator +(Vector4D v1, Vector4D v2) => new(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

    public static Vector4D operator +(Vector4D v1, Vector3D v2) => new(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w);

    public static Vector4D operator -(Vector4D v1, Vector4D v2) => new(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w);

    public static float operator *(Vector4D v1, Vector4D v2) => v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;

    public static Vector4D operator *(Vector4D v, float scalar) => new(v.x * scalar, v.y * scalar, v.z * scalar, v.w * scalar);

    public static Vector4D operator *(float scalar, Vector4D v) => v * scalar;

    public static Vector4D operator /(Vector4D v, float scalar) => new(v.x / scalar, v.y / scalar, v.z / scalar, v.w / scalar);

    public static Vector4D operator -(Vector4D v) => new(-v.x, -v.y, -v.z, -v.w);

    #endregion

    #region Casting

    public static explicit operator Quaternion(Vector4D v) => new(v.x, v.y, v.z, v.w);

    public static explicit operator Vector3D(Vector4D v) => new(v.x, v.y, v.z);

    #endregion
}