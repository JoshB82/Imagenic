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
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionForward));
            }
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionUp));
            }

            return new Orientation
            {
                DirectionForward = directionForward,
                DirectionUp = directionUp,
                DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp)
            };
        }

        public static Orientation CreateOrientationUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionUp));
            }
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionRight));
            }

            return new Orientation
            {
                DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight),
                DirectionUp = directionUp,
                DirectionRight = directionRight
            };
        }

        public static Orientation CreateOrientationRightForward(Vector3D directionRight, Vector3D directionForward)
        {
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionRight));
            }
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionForward));
            }

            return new Orientation
            {
                DirectionForward = directionForward,
                DirectionUp = Transform.CalculateDirectionUp(directionRight, directionForward),
                DirectionRight = directionRight
            };
        }

        #endregion

        #region Methods

        public void SetDirectionForwardUp(Vector3D directionForward, Vector3D directionUp)
        {
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionForward));
            }
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionUp));
            }

            DirectionForward = directionForward;
            DirectionUp = directionUp;
            DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp);
        }

        public void SetDirectionUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            if (directionUp.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionUp));
            }
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionRight));
            }

            DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight);
            DirectionUp = directionUp;
            DirectionRight = directionRight;
        }

        public void SetDirectionRightForward(Vector3D directionRight, Vector3D directionForward)
        {
            if (directionRight.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionRight));
            }
            if (directionForward.ApproxEquals(Vector3D.Zero))
            {
                GenerateException.WithParameters<VectorCannotBeZeroException>(nameof(directionForward));
            }

            DirectionForward = directionForward;
            DirectionUp = Transform.CalculateDirectionUp(directionRight, directionForward);
            DirectionRight = directionRight;
        }

        public bool Equals(Orientation other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}