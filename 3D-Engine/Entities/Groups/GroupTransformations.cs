using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Maths.Vectors;

namespace _3D_Engine.Entities.Groups
{
    public partial class Group
    {
        #region Rotations

        public Group SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection1(newWorldDirectionForward, newWorldDirectionUp);
            }

            return this;
        }

        public Group SetDirection2(Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection2(newWorldDirectionUp, newWorldDirectionRight);
            }

            return this;
        }

        public Group SetDirection3(Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection3(newWorldDirectionRight, newWorldDirectionForward);
            }

            return this;
        }

        public Group Rotate(Vector3D axis, float angle)
        {
            this.Rotate<Group>(axis, angle);

            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.Rotate(axis, angle);
            }

            return this;
        }

        public Group RotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {
            //base.RotateBetweenVectors(v1, v2, axis);

            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.RotateBetweenVectors(v1, v2, axis);
            }

            return this;
        }

        /*
        public override void SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            base.SetDirection1(newWorldDirectionForward, newWorldDirectionUp);

            if (SceneObjects is null || SceneObjects.Count == 0) return;

            // Calculate rotation matrices
            Matrix4x4 directionForwardRotation = Transform.Rotate_Between_Vectors(WorldDirectionForward, newWorldDirectionForward);
            Matrix4x4 directionUpRotation = Transform.Rotate_Between_Vectors((Vector3D) (directionForwardRotation * new Vector4D(WorldDirectionUp, 1)), newWorldDirectionUp);
            Matrix4x4 resultant = directionUpRotation * directionForwardRotation;

            // Apply rotation matrices to children of group
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection1
                (
                    (Vector3D)(directionForwardRotation * sceneObject.WorldDirectionForward),
                    (Vector3D)(resultant * new Vector4D(sceneObject.WorldDirectionUp, 1))
                );
            }
        }
        public override void SetDirection2(Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight)
        {
            base.SetDirection2(newWorldDirectionUp, newWorldDirectionRight);

            if (SceneObjects.Count == 0) return;

            // Calculate rotation matrices
            Matrix4x4 directionUpRotation = Transform.Rotate_Between_Vectors(WorldDirectionUp, newWorldDirectionUp);
            Matrix4x4 directionRightRotation = Transform.Rotate_Between_Vectors((Vector3D) (directionUpRotation * new Vector4D(WorldDirectionRight, 1)), newWorldDirectionRight);
            Matrix4x4 resultant = directionRightRotation * directionUpRotation;

            // Apply rotation matrices to children of group
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection2
                (
                    (Vector3D)(directionUpRotation * sceneObject.WorldDirectionUp),
                    (Vector3D)(resultant * new Vector4D(sceneObject.WorldDirectionRight, 1))
                );
            }
        }
        public override void SetDirection3(Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward)
        {
            base.SetDirection3(newWorldDirectionRight, newWorldDirectionForward);

            if (SceneObjects.Count == 0) return;

            // Calculate rotation matrices
            Matrix4x4 directionRightRotation = Transform.Rotate_Between_Vectors(WorldDirectionRight, newWorldDirectionRight);
            Matrix4x4 directionForwardRotation = Transform.Rotate_Between_Vectors((Vector3D) (directionRightRotation * new Vector4D(WorldDirectionForward, 1)),
                newWorldDirectionForward);
            Matrix4x4 resultant = directionForwardRotation * directionRightRotation;

            // Apply rotation matrices to children of group
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.SetDirection3
                (
                    (Vector3D)(directionRightRotation * sceneObject.WorldDirectionRight),
                    (Vector3D)(resultant * new Vector4D(sceneObject.WorldDirectionForward, 1))
                );
            }
        }
        */
        #endregion
    }
}