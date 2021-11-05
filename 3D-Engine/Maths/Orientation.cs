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
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using System;


namespace _3D_Engine.Maths
{
    public class Orientation : IEquatable<Orientation>
    {
        #region Fields and Properties

        internal SceneObject LinkedSceneObject { get; set; }

        // Directions
        internal static readonly Vector3D ModelDirectionForward = Vector3D.UnitZ;
        internal static readonly Vector3D ModelDirectionUp = Vector3D.UnitY;
        internal static readonly Vector3D ModelDirectionRight = Vector3D.UnitX;
        internal static readonly Orientation ModelOrientation = Orientation.CreateOrientationForwardUp(ModelDirectionForward, ModelDirectionUp);

        public Vector3D DirectionForward { get; private set; }
        public Vector3D DirectionUp { get; private set; }
        public Vector3D DirectionRight { get; private set; }

        // Miscellaneous
        private const float epsilon = 1E-6f;

        #endregion

        #region Constructors

        private Orientation() { }

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
                throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionForward), nameof(directionUp));
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
                throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionUp), nameof(directionRight));
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
                throw GenerateException<VectorsAreNotOrthogonalException>.WithParameters(nameof(directionRight), nameof(directionForward));
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

        #endregion
    }
}