using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Transformations;

namespace _3D_Engine.SceneObjects.Cameras
{
    public abstract partial class Camera : SceneObject
    {
        #region Common

        /// <summary>
        /// Pans the camera in the forward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanForward(float distance) => Translate(WorldDirectionForward * distance);
        /// <summary>
        /// Pans the camera in the left direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanLeft(float distance) => Translate(WorldDirectionRight * -distance);
        /// <summary>
        /// Pans the camera in the right direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanRight(float distance) => Translate(WorldDirectionRight * distance);
        /// <summary>
        /// Pans the camera in the backward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanBackward(float distance) => Translate(WorldDirectionForward * -distance);
        /// <summary>
        /// Pans the camera in the up direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanUp(float distance) => Translate(WorldDirectionUp * distance);
        /// <summary>
        /// Pans the camera in the down direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanDown(float distance) => Translate(WorldDirectionUp * -distance);

        public void RotateUp(float angle)
        {
            Matrix4x4 transformation_up = Transform.Rotate(WorldDirectionRight, -angle);
            SetDirection1((Vector3D)(transformation_up * WorldDirectionForward), (Vector3D)(transformation_up * WorldDirectionUp));
        }
        public void RotateLeft(float angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(WorldDirectionUp, -angle);
            SetDirection3((Vector3D)(transformation_left * WorldDirectionRight), (Vector3D)(transformation_left * WorldDirectionForward));
        }
        public void RotateRight(float angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(WorldDirectionUp, angle);
            SetDirection3((Vector3D)(transformation_right * WorldDirectionRight), (Vector3D)(transformation_right * WorldDirectionForward));
        }
        public void RotateDown(float angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(WorldDirectionRight, angle);
            SetDirection1((Vector3D)(transformation_down * WorldDirectionForward), (Vector3D)(transformation_down * WorldDirectionUp));
        }
        public void RollLeft(float angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(WorldDirectionForward, angle);
            SetDirection2((Vector3D)(transformation_roll_left * WorldDirectionUp), (Vector3D)(transformation_roll_left * WorldDirectionRight));
        }
        public void RollRight(float angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(WorldDirectionForward, -angle);
            SetDirection2((Vector3D)(transformation_roll_right * WorldDirectionUp), (Vector3D)(transformation_roll_right * WorldDirectionRight));
        }

        #endregion
    }
}