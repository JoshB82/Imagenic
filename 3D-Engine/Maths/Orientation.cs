using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Maths
{
    public struct Orientation
    {
        #region Fields and Properties

        public Vector3D DirectionForward { get; set; }
        public Vector3D DirectionUp { get; set; }
        public Vector3D DirectionRight { get; set; }

        #endregion

        #region Constructors

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
    }
}