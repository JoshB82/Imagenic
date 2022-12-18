/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an object that represents a three-dimensional orientation consisting of three directions: forward, up and right.
 */

using _3D_Engine.Constants;
using Imagenic.Core.Maths.Transformations;
using System;

namespace Imagenic.Core.Maths;

public class Orientation : IEquatable<Orientation>
{
    #region Fields and Properties

    // Directions
    internal static readonly Vector3D ModelDirectionForward = Vector3D.UnitZ;
    internal static readonly Vector3D ModelDirectionUp = Vector3D.UnitY;
    internal static readonly Vector3D ModelDirectionRight = Vector3D.UnitX;

    // Orientations
    internal static readonly Orientation ModelOrientation = CreateOrientationForwardUp(ModelDirectionForward, ModelDirectionUp);
    public static readonly Orientation OrientationXY = new(Vector3D.UnitX, Vector3D.UnitY, Vector3D.UnitNegativeZ);
    public static readonly Orientation OrientationXZ = new(Vector3D.UnitX, Vector3D.UnitZ, Vector3D.UnitNegativeY);
    public static readonly Orientation OrientationYX = new(Vector3D.UnitY, Vector3D.UnitX, Vector3D.UnitZ);
    public static readonly Orientation OrientationYZ = new(Vector3D.UnitY, Vector3D.UnitZ, Vector3D.UnitNegativeX);
    public static readonly Orientation OrientationZX = new(Vector3D.UnitZ, Vector3D.UnitX, Vector3D.UnitNegativeY);
    public static readonly Orientation OrientationZY = new(Vector3D.UnitZ, Vector3D.UnitY, Vector3D.UnitX);
    public static readonly Orientation OrientationXNegativeY = new(Vector3D.UnitX, Vector3D.UnitNegativeY, Vector3D.UnitZ);
    public static readonly Orientation OrientationXNegativeZ = new(Vector3D.UnitX, Vector3D.UnitNegativeZ, Vector3D.UnitY);
    public static readonly Orientation OrientationYNegativeX = new(Vector3D.UnitY, Vector3D.UnitNegativeX, Vector3D.UnitNegativeZ);
    public static readonly Orientation OrientationYNegativeZ = new(Vector3D.UnitY, Vector3D.UnitNegativeZ, Vector3D.UnitX);
    public static readonly Orientation OrientationZNegativeX = new(Vector3D.UnitZ, Vector3D.UnitNegativeX, Vector3D.UnitY);
    public static readonly Orientation OrientationZNegativeY = new(Vector3D.UnitZ, Vector3D.UnitNegativeY, Vector3D.UnitNegativeX);
    public static readonly Orientation OrientationNegativeXY = new(Vector3D.UnitNegativeX, Vector3D.UnitY, Vector3D.UnitZ);
    public static readonly Orientation OrientationNegativeXZ = new(Vector3D.UnitNegativeX, Vector3D.UnitZ, Vector3D.UnitNegativeY);
    public static readonly Orientation OrientationNegativeYX = new(Vector3D.UnitNegativeY, Vector3D.UnitX, Vector3D.UnitNegativeZ);
    public static readonly Orientation OrientationNegativeYZ = new(Vector3D.UnitNegativeY, Vector3D.UnitZ, Vector3D.UnitX);
    public static readonly Orientation OrientationNegativeZX = new(Vector3D.UnitNegativeZ, Vector3D.UnitX, Vector3D.UnitY);
    public static readonly Orientation OrientationNegativeZY = new(Vector3D.UnitNegativeZ, Vector3D.UnitY, Vector3D.UnitNegativeX);
    public static readonly Orientation OrientationNegativeXNegativeY = new(Vector3D.UnitNegativeX, Vector3D.UnitNegativeY, Vector3D.UnitNegativeZ);
    public static readonly Orientation OrientationNegativeXNegativeZ = new(Vector3D.UnitNegativeX, Vector3D.UnitNegativeZ, Vector3D.UnitY);
    public static readonly Orientation OrientationNegativeYNegativeX = new(Vector3D.UnitNegativeY, Vector3D.UnitNegativeX, Vector3D.UnitZ);
    public static readonly Orientation OrientationNegativeYNegativeZ = new(Vector3D.UnitNegativeY, Vector3D.UnitNegativeZ, Vector3D.UnitNegativeX);
    public static readonly Orientation OrientationNegativeZNegativeX = new(Vector3D.UnitNegativeZ, Vector3D.UnitNegativeX, Vector3D.UnitNegativeY);
    public static readonly Orientation OrientationNegativeZNegativeY = new(Vector3D.UnitNegativeZ, Vector3D.UnitNegativeY, Vector3D.UnitX);

    public Vector3D DirectionForward { get; private set; }
    public Vector3D DirectionUp { get; private set; }
    public Vector3D DirectionRight { get; private set; }

    // Miscellaneous
    private const float epsilon = 1E-6f;

    #endregion

    #region Constructors

    private Orientation() { }

    private Orientation(Vector3D directionForward, Vector3D directionUp, Vector3D directionRight)
    {
        DirectionForward = directionForward;
        DirectionUp = directionUp;
        DirectionRight = directionRight;
    }

    public static Orientation CreateOrientationForwardUp(Vector3D directionForward, Vector3D directionUp)
    {
        Orientation newOrientation = new();
        newOrientation.SetDirectionForwardUp(directionForward, directionUp);
        return newOrientation;
    }

    public static Orientation CreateOrientationUpRight(Vector3D directionUp, Vector3D directionRight)
    {
        Orientation newOrientation = new();
        newOrientation.SetDirectionUpRight(directionUp, directionRight);
        return newOrientation;
    }

    public static Orientation CreateOrientationRightForward(Vector3D directionRight, Vector3D directionForward)
    {
        Orientation newOrientation = new();
        newOrientation.SetDirectionRightForward(directionRight, directionForward);
        return newOrientation;
    }

    #endregion

    #region Methods

    public void SetDirectionForwardUp(Vector3D directionForward, Vector3D directionUp)
    {
        if (directionForward.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
        }
        if (directionUp.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
        }
        if (!(directionForward * directionUp).ApproxEquals(0, epsilon))
        {
            throw new MessageBuilder<VectorsAreNotOrthogonalMessage>()
                  .AddParameter(directionForward)
                  .AddParameter(directionUp)
                  .BuildIntoException<ArgumentException>();

            //throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionForward), nameof(directionUp));
        }

        DirectionForward = directionForward.Normalise();
        DirectionUp = directionUp.Normalise();
        DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp).Normalise();
    }

    public void SetDirectionUpRight(Vector3D directionUp, Vector3D directionRight)
    {
        if (directionUp.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
        }
        if (directionRight.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
        }
        if (!(directionUp * directionRight).ApproxEquals(0, epsilon))
        {
            throw new MessageBuilder<VectorsAreNotOrthogonalMessage>()
                  .AddParameter(directionUp)
                  .AddParameter(directionRight)
                  .BuildIntoException<ArgumentException>();

            //throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionUp), nameof(directionRight));
        }

        DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight).Normalise();
        DirectionUp = directionUp.Normalise();
        DirectionRight = directionRight.Normalise();
    }

    public void SetDirectionRightForward(Vector3D directionRight, Vector3D directionForward)
    {
        if (directionRight.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
        }
        if (directionForward.ApproxEquals(Vector3D.Zero, epsilon))
        {
            throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
        }
        if (!(directionRight * directionForward).ApproxEquals(0, epsilon))
        {
            throw new MessageBuilder<VectorsAreNotOrthogonalMessage>()
                  .AddParameter(directionRight)
                  .AddParameter(directionForward)
                  .BuildIntoException<ArgumentException>();

            //throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionRight), nameof(directionForward));
        }

        DirectionForward = directionForward.Normalise();
        DirectionUp = Transform.CalculateDirectionUp(directionRight, directionForward).Normalise();
        DirectionRight = directionRight.Normalise();
    }

    private void DirectionCheck(string firstDirection, string secondDirection)
    {

    }

    public bool Equals(Orientation other) => (DirectionForward, DirectionUp, DirectionRight) == (other.DirectionForward, other.DirectionUp, other.DirectionRight);

    public override int GetHashCode() => (DirectionForward, DirectionUp, DirectionRight).GetHashCode();

    public static bool operator ==(Orientation lhs, Orientation rhs) => lhs.Equals(rhs);

    public static bool operator !=(Orientation lhs, Orientation rhs) => !(lhs == rhs);

    public override bool Equals(object? obj)
    {
        return this == (Orientation)obj;
    }

    #endregion
}