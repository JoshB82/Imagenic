using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using System;

namespace _3D_Engine.Maths
{
    public class Orientation : IEquatable<Orientation>
    {
        #region Fields and Properties

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
                RequestNewRenders();
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

        /*
        private Orientation()
        {
            DirectionForward = Vector3D.Zero;
            DirectionUp = Vector3D.Zero;
            DirectionRight = Vector3D.Zero;
        }
        */

        public static Orientation CreateOrientationForwardUp(Vector3D directionForward, Vector3D directionUp)
        {
            return new Orientation
            {
                DirectionForward = directionForward,
                DirectionUp = directionUp,
                DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp)
            };
        }

        public static Orientation CreateOrientationUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            return new Orientation
            {
                DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight),
                DirectionUp = directionUp,
                DirectionRight = directionRight
            };
        }

        public static Orientation CreateOrientationRightForward(Vector3D directionRight, Vector3D directionForward)
        {
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
            DirectionForward = directionForward;
            DirectionUp = directionUp;
            DirectionRight = Transform.CalculateDirectionRight(directionForward, directionUp);
        }

        public void SetDirectionUpRight(Vector3D directionUp, Vector3D directionRight)
        {
            DirectionForward = Transform.CalculateDirectionForward(directionUp, directionRight);
            DirectionUp = directionUp;
            DirectionRight = directionRight;
        }

        public void SetDirectionRightForward(Vector3D directionRight, Vector3D directionForward)
        {
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