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

        internal SceneObject Subject { get; set; }

        private bool displayDirectionArrows = false;
        /// <summary>
        /// Determines whether the <see cref="SceneObject"/> direction arrows are shown or not.
        /// </summary>
        public bool DisplayDirectionArrows
        {
            get => displayDirectionArrows;
            set
            {
                if (value == displayDirectionArrows) return;
                displayDirectionArrows = value;
                Subject.RequestNewRenders();
            }
        }
        internal bool HasDirectionArrows { get; set; }

        // Directions
        internal static readonly Vector3D ModelDirectionForward = Vector3D.UnitZ;
        internal static readonly Vector3D ModelDirectionUp = Vector3D.UnitY;
        internal static readonly Vector3D ModelDirectionRight = Vector3D.UnitX;

        public Vector3D DirectionForward { get; private set; }
        public Vector3D DirectionUp { get; private set; }
        public Vector3D DirectionRight { get; private set; }

        #endregion

        #region Constructors

        public static Orientation CreateOrientationForwardUp(Vector3D directionForward, Vector3D directionUp)
        {
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
            }
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
            }

            return new Orientation
            {
                DirectionForward = directionForward.Normalise(),
                DirectionUp = directionUp.Normalise(),
                DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp).Normalise()
            };
        }

        public static Orientation CreateOrientationUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
            }
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
            }

            return new Orientation
            {
                DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight).Normalise(),
                DirectionUp = directionUp.Normalise(),
                DirectionRight = directionRight.Normalise()
            };
        }

        public static Orientation CreateOrientationRightForward(Vector3D directionRight, Vector3D directionForward)
        {
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
            }
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
            }

            return new Orientation
            {
                DirectionForward = directionForward.Normalise(),
                DirectionUp = Transform.CalculateDirectionUp(directionRight, directionForward).Normalise(),
                DirectionRight = directionRight.Normalise()
            };
        }

        #endregion

        #region Methods

        public void SetDirectionForwardUp(Vector3D directionForward, Vector3D directionUp)
        {
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
            }
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
            }

            DirectionForward = directionForward.Normalise();
            DirectionUp = directionUp.Normalise();
            DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp).Normalise();
        }

        public void SetDirectionUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionUp));
            }
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
            }

            DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight).Normalise();
            DirectionUp = directionUp.Normalise();
            DirectionRight = directionRight.Normalise();
        }

        public void SetDirectionRightForward(Vector3D directionRight, Vector3D directionForward)
        {
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionRight));
            }
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                throw GenerateException<VectorCannotBeZeroException>.WithParameters(nameof(directionForward));
            }

            DirectionForward = directionForward.Normalise();
            DirectionUp = Transform.CalculateDirectionUp(directionRight, directionForward).Normalise();
            DirectionRight = directionRight.Normalise();
        }

        public bool Equals(Orientation other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}