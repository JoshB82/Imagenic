using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using System;

namespace _3D_Engine.Maths
{
    public struct Orientation : IEquatable<Orientation>
    {
        #region Fields and Properties

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