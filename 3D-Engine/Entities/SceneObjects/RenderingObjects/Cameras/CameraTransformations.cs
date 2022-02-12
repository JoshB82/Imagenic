using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using Imagenic.Core.Maths;
using System;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    public static class CameraTransformations
    {
        public static T Zoom<T>(this T camera, float multiplier, Predicate<Camera> predicate = null) where T : Camera
        {
            foreach (Camera child in camera.GetAllChildrenAndSelf(x => x is Camera camera && predicate(camera)))
            {
                child.ZNear *= multiplier;
                child.ZFar *= multiplier;
            }
            return camera;
        }
    }

    public abstract partial class Camera : RenderingObject
    {
        #region Common

        /// <summary>
        /// Pans the camera in the forward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanForward(float distance)
        {
            Translate(WorldDirectionForward * distance);
            NewRenderNeeded = true;
        }
        /// <summary>
        /// Pans the camera in the left direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanLeft(float distance)
        {
            Translate(WorldDirectionRight * -distance);
            NewRenderNeeded = true;
        }
        /// <summary>
        /// Pans the camera in the right direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanRight(float distance)
        {
            Translate(WorldDirectionRight * distance);
            NewRenderNeeded = true;
        }
        /// <summary>
        /// Pans the camera in the backward direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanBackward(float distance)
        {
            Translate(WorldDirectionForward * -distance);
            NewRenderNeeded = true;
        }
        /// <summary>
        /// Pans the camera in the up direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanUp(float distance)
        {
            Translate(WorldDirectionUp * distance);
            NewRenderNeeded = true;
        }
        /// <summary>
        /// Pans the camera in the down direction.
        /// </summary>
        /// <param name="distance">Distance to pan by.</param>
        public void PanDown(float distance)
        {
            Translate(WorldDirectionUp * -distance);
            NewRenderNeeded = true;
        }

        public void RotateUp(float angle)
        {
            Matrix4x4 transformationUp = Transform.Rotate(WorldDirectionRight, -angle);
            SetDirection1((Vector3D)(transformationUp * WorldDirectionForward), (Vector3D)(transformationUp * WorldDirectionUp));
            NewRenderNeeded = true;
        }
        public void RotateLeft(float angle)
        {
            Matrix4x4 transformationLeft = Transform.Rotate(WorldDirectionUp, -angle);
            SetDirection3((Vector3D)(transformationLeft * WorldDirectionRight), (Vector3D)(transformationLeft * WorldDirectionForward));
            NewRenderNeeded = true;
        }
        public void RotateRight(float angle)
        {
            Matrix4x4 transformationRight = Transform.Rotate(WorldDirectionUp, angle);
            SetDirection3((Vector3D)(transformationRight * WorldDirectionRight), (Vector3D)(transformationRight * WorldDirectionForward));
            NewRenderNeeded = true;
        }
        public void RotateDown(float angle)
        {
            Matrix4x4 transformationDown = Transform.Rotate(WorldDirectionRight, angle);
            SetDirection1((Vector3D)(transformationDown * WorldDirectionForward), (Vector3D)(transformationDown * WorldDirectionUp));
            NewRenderNeeded = true;
        }
        public void RollLeft(float angle)
        {
            Matrix4x4 transformationRollLeft = Transform.Rotate(WorldDirectionForward, angle);
            SetDirection2((Vector3D)(transformationRollLeft * WorldDirectionUp), (Vector3D)(transformationRollLeft * WorldDirectionRight));
            NewRenderNeeded = true;
        }
        public void RollRight(float angle)
        {
            Matrix4x4 transformationRollRight = Transform.Rotate(WorldDirectionForward, -angle);
            SetDirection2((Vector3D)(transformationRollRight * WorldDirectionUp), (Vector3D)(transformationRollRight * WorldDirectionRight));
            NewRenderNeeded = true;
        }

        #endregion
    }
}