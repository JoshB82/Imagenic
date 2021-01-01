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
        public void Pan_Forward(float distance) => Translate(WorldDirectionForward * distance);
        /// <summary>
        /// Pans the camera in the left direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Left(float distance) => Translate(WorldDirectionRight * -distance);
        /// <summary>
        /// Pans the camera in the right direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Right(float distance) => Translate(WorldDirectionRight * distance);
        /// <summary>
        /// Pans the camera in the backward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Backward(float distance) => Translate(WorldDirectionForward * -distance);
        /// <summary>
        /// Pans the camera in the up direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Up(float distance) => Translate(WorldDirectionUp * distance);
        /// <summary>
        /// Pans the camera in the down direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void Pan_Down(float distance) => Translate(WorldDirectionUp * -distance);

        public void Rotate_Up(float angle)
        {
            Matrix4x4 transformation_up = Transform.Rotate(WorldDirectionRight, -angle);
            SetDirection1((Vector3D)(transformation_up * WorldDirectionForward), (Vector3D)(transformation_up * WorldDirectionUp));
        }
        public void Rotate_Left(float angle)
        {
            Matrix4x4 transformation_left = Transform.Rotate(WorldDirectionUp, -angle);
            SetDirection3((Vector3D)(transformation_left * WorldDirectionRight), (Vector3D)(transformation_left * WorldDirectionForward));
        }
        public void Rotate_Right(float angle)
        {
            Matrix4x4 transformation_right = Transform.Rotate(WorldDirectionUp, angle);
            SetDirection3((Vector3D)(transformation_right * WorldDirectionRight), (Vector3D)(transformation_right * WorldDirectionForward));
        }
        public void Rotate_Down(float angle)
        {
            Matrix4x4 transformation_down = Transform.Rotate(WorldDirectionRight, angle);
            SetDirection1((Vector3D)(transformation_down * WorldDirectionForward), (Vector3D)(transformation_down * WorldDirectionUp));
        }
        public void Roll_Left(float angle)
        {
            Matrix4x4 transformation_roll_left = Transform.Rotate(WorldDirectionForward, angle);
            SetDirection2((Vector3D)(transformation_roll_left * WorldDirectionUp), (Vector3D)(transformation_roll_left * WorldDirectionRight));
        }
        public void Roll_Right(float angle)
        {
            Matrix4x4 transformation_roll_right = Transform.Rotate(WorldDirectionForward, -angle);
            SetDirection2((Vector3D)(transformation_roll_right * WorldDirectionUp), (Vector3D)(transformation_roll_right * WorldDirectionRight));
        }

        #endregion
    }
}