using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Transformations;

namespace _3D_Engine.SceneObjects.Groups
{
    public partial class Group
    {
        #region Rotations

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

        #endregion

        #region Translations

        public void TranslateX(float distance)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.WorldOrigin += new Vector3D(distance, 0, 0);
            }
        }

        public void TranslateY(float distance)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.WorldOrigin += new Vector3D(0, distance, 0);
            }
        }

        public void TranslateZ(float distance)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.WorldOrigin += new Vector3D(0, 0, distance);
            }
        }

        public void Translate(Vector3D displacement)
        {
            foreach (SceneObject sceneObject in SceneObjects)
            {
                sceneObject.WorldOrigin += displacement;
            }
        }



        /*
        public override void TranslateX(float distance)
        {
            base.TranslateX(distance);
            foreach (SceneObject sceneObject in SceneObjects) sceneObject.TranslateX(distance);
        }
        public override void TranslateY(float distance)
        {
            base.TranslateY(distance);
            foreach (SceneObject sceneObject in SceneObjects) sceneObject.TranslateY(distance);
        }
        public override void TranslateZ(float distance)
        {
            base.TranslateZ(distance);
            foreach (SceneObject sceneObject in SceneObjects) sceneObject.TranslateZ(distance);
        }
        public override void Translate(Vector3D displacement)
        {
            base.Translate(displacement);
            foreach (SceneObject sceneObject in SceneObjects) sceneObject.Translate(displacement);
        }
        */
        #endregion
    }
}