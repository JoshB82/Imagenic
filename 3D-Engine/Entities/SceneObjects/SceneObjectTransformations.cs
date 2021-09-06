/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines methods for rotating and translating SceneObjects.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;

namespace _3D_Engine.Entities.SceneObjects
{
    public static class SceneObjectExtensions
    {
        #region Fields and Properties

        private const float epsilon = 1E-6f;

        #endregion

        #region Methods

        // Rotations

        public static T SetDirection<T>(this T sceneObject, Orientation orientation) where T : SceneObject
        {
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="newWorldDirectionForward"></param>
        /// <param name="newWorldDirectionUp"></param>
        /// <returns></returns>
        public static T SetDirection1<T>(this T sceneObject, Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp) where T : SceneObject
        {
            if (newWorldDirectionForward.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionForward));
            }
            if (newWorldDirectionUp.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionUp));
            }

            newWorldDirectionForward = newWorldDirectionForward.Normalise();
            newWorldDirectionUp = newWorldDirectionUp.Normalise();

            AdjustVectors(newWorldDirectionForward, newWorldDirectionUp,Transform.CalculateDirectionRight(newWorldDirectionForward, newWorldDirectionUp)
            );

            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="newWorldDirectionUp"></param>
        /// <param name="newWorldDirectionRight"></param>
        /// <returns></returns>
        public static T SetDirection2<T>(this T sceneObject, Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight) where T : SceneObject
        {
            if (newWorldDirectionUp.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionUp));
            }
            if (newWorldDirectionRight.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionRight));
            }

            newWorldDirectionUp = newWorldDirectionUp.Normalise();
            newWorldDirectionRight = newWorldDirectionRight.Normalise();

            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="newWorldDirectionRight"></param>
        /// <param name="newWorldDirectionForward"></param>
        /// <returns></returns>
        public static T SetDirection3<T>(this T sceneObject, Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward) where T : SceneObject
        {
            if (newWorldDirectionRight.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionRight));
            }
            if (newWorldDirectionForward.ApproxEquals(Vector3D.Zero, epsilon))
            {
                throw GenerateException.GenerateException<VectorCannotBeZeroException>(nameof(newWorldDirectionForward));
            }

            newWorldDirectionForward = newWorldDirectionForward.Normalise();
            newWorldDirectionRight = newWorldDirectionRight.Normalise();

            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static T Rotate<T>(this T sceneObject, Vector3D axis, float angle) where T : SceneObject
        {
            Matrix4x4 rotation = Transform.Rotate(axis, angle);

            sceneObject.WorldOrigin = (Vector3D)(rotation * new Vector4D(sceneObject.WorldOrigin, 1));
            sceneObject.WorldDirectionForward = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionForward, 1));
            sceneObject.WorldDirectionUp = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionUp, 1));
            sceneObject.WorldDirectionRight = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionRight, 1));

            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        public static T RotateBetweenVectors<T>(this T sceneObject, Vector3D v1, Vector3D v2, Vector3D? axis = null) where T : SceneObject
        {
            Matrix4x4 rotation = Transform.RotateBetweenVectors(v1, v2, axis);

            sceneObject.WorldOrigin = (Vector3D)(rotation * new Vector4D(sceneObject.WorldOrigin, 1));
            sceneObject.WorldDirectionForward = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionForward, 1));
            sceneObject.WorldDirectionUp = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionUp, 1));
            sceneObject.WorldDirectionRight = (Vector3D)(rotation * new Vector4D(sceneObject.WorldDirectionRight, 1));

            return sceneObject;
        }

        // Translations
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static T TranslateX<T>(this T sceneObject, float distance) where T : SceneObject
        {
            sceneObject.WorldOrigin += new Vector3D(distance, 0, 0);
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static T TranslateY<T>(this T sceneObject, float distance) where T : SceneObject
        {
            sceneObject.WorldOrigin += new Vector3D(0, distance, 0);
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static T TranslateZ<T>(this T sceneObject, float distance) where T : SceneObject
        {
            sceneObject.WorldOrigin += new Vector3D(0, 0, distance);
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static T Translate<T>(this T sceneObject, Vector3D displacement) where T : SceneObject
        {
            sceneObject.WorldOrigin += displacement;
            return sceneObject;
        }

        #endregion
    }

    public abstract partial class SceneObject2
    {
        #region Rotations

        /// <summary>
        /// Sets the forward, up and right directions given the forward and up directions.
        /// </summary>
        /// <param name="newWorldDirectionForward">The forward direction.</param>
        /// <param name="newWorldDirectionUp">The up direction.</param>
        public virtual void SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            VectorCheck(newWorldDirectionForward, newWorldDirectionUp);

            // if (new_world_direction_forward * new_world_direction_up != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            AdjustVectors(
                newWorldDirectionForward,
                newWorldDirectionUp,
                Transform.CalculateDirectionRight(newWorldDirectionForward, newWorldDirectionUp)
            );
        }

        /// <summary>
        /// Sets the forward, up and right directions given the up and right directions.
        /// </summary>
        /// <param name="newWorldDirectionUp">The up direction.</param>
        /// <param name="newWorldDirectionRight">The right direction.</param>
        public virtual void SetDirection2(Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight)
        {
            VectorCheck(newWorldDirectionUp, newWorldDirectionRight);

            // if (new_world_direction_up * new_world_direction_right != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            AdjustVectors(
                Transform.CalculateDirectionForward(newWorldDirectionUp, newWorldDirectionRight),
                newWorldDirectionUp,
                newWorldDirectionRight
            );
        }

        /// <summary>
        /// Sets the forward, up and right directions given the right and forward directions.
        /// </summary>
        /// <param name="newWorldDirectionRight">The right direction.</param>
        /// <param name="newWorldDirectionForward">The forward direction.</param>
        public virtual void SetDirection3(Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward)
        {
            VectorCheck(newWorldDirectionRight, newWorldDirectionForward);

            // if (new_world_direction_right * new_world_direction_forward != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            AdjustVectors(
                newWorldDirectionForward,
                Transform.CalculateDirectionUp(newWorldDirectionRight, newWorldDirectionForward),
                newWorldDirectionRight
            );
        }

        internal virtual void AdjustVectors(Vector3D directionForward, Vector3D directionUp, Vector3D directionRight)
        {
            WorldDirectionForward = directionForward;
            WorldDirectionUp = directionUp;
            WorldDirectionRight = directionRight;

            if (HasDirectionArrows)
            {
                ((Arrow)DirectionArrows.SceneObjects[0]).SetDirection1(directionForward, directionUp);
                ((Arrow)DirectionArrows.SceneObjects[1]).SetDirection1(directionForward, directionUp);
                ((Arrow)DirectionArrows.SceneObjects[2]).SetDirection1(directionForward, directionUp);
            }

            ConsoleOutput.DisplayOutputDirectionMessage(this, Properties.Settings.Default.Verbosity);

            CalculateMatrices();
            RequestNewRenders();
        }

        private static void VectorCheck(Vector3D firstVector, Vector3D secondVector)
        {
            if (firstVector.ApproxEquals(Vector3D.Zero, 1E-6f) || secondVector.ApproxEquals(Vector3D.Zero, 1E-6f))
            {
                throw new ArgumentException("New direction vector(s) cannot be set to zero vector.");
            }
        }

        //public virtual void Rotate(Vector3D axis, float angle)
        //{

        //}

        public virtual void RotateBetweenVectors(Vector3D v1, Vector3D v2, Vector3D? axis = null)
        {

        }

        #endregion

        #region Translations

        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the x-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateX(float distance) => WorldOrigin += new Vector3D(distance, 0, 0);

        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the y-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateY(float distance) => WorldOrigin += new Vector3D(0, distance, 0);

        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the z-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateZ(float distance) => WorldOrigin += new Vector3D(0, 0, distance);

        /// <summary>
        /// Translates the <see cref="SceneObject"/> by the given vector.
        /// </summary>
        /// <param name="displacement">Vector to translate by.</param>
        public virtual void Translate(Vector3D displacement) => WorldOrigin += displacement;

        #endregion
    }
}