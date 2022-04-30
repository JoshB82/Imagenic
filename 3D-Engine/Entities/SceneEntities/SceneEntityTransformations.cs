﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines methods for transforming SceneObjects, specifically rotation and translation.
 */

using _3D_Engine.Constants;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Utilities;
using Imagenic.Core.Entities.SceneObjects.Meshes;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components;
using Imagenic.Core.Entities.SceneObjects.Meshes.ThreeDimensions;
using Imagenic.Core.Maths;
using Imagenic.Core.Utilities;
using Imagenic.Core.Utilities.Tree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.Entities.SceneObjects
{
    public static class SceneEntityTransformations
    {
        #region Fields and Properties

        private const float epsilon = 1E-6f;

        #endregion

        #region Methods

        // Rotations

        #region SetOrientation method

        public static T SetOrientation<T>(this T sceneEntity, Orientation orientation) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            sceneEntity.WorldOrientation = orientation;

            return sceneEntity;
        }

        public static IEnumerable<T> SetOrientation<T>(this IEnumerable<T> sceneEntities, Orientation orientation) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (T sceneEntity in sceneEntities)
            {
                sceneEntity.WorldOrientation = orientation;
                yield return sceneEntity;
            }
        }

        public static IEnumerable<T> SetOrientation<T>(this IEnumerable<T> sceneEntities, Orientation orientation, Func<T, bool> predicate) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (T sceneEntity in sceneEntities.Where(predicate))
            {
                sceneEntity.WorldOrientation = orientation;
                yield return sceneEntity;
            }
        }

        public static Node<T> SetOrientation<T>(this Node<T> sceneEntityNode, Orientation orientation) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (Node<SceneEntity> node in sceneEntityNode.GetDescendantsAndSelfOfType<SceneEntity>())
            {
                node.Content.WorldOrientation = orientation;
            }

            return sceneEntityNode;
        }

        public static Node<T> SetOrientation<T>(this Node<T> sceneEntityNode, Orientation orientation, Func<T, bool> predicate) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (Node<SceneEntity> node in sceneEntityNode.GetDescendantsAndSelfOfType<SceneEntity>(predicate))
            {
                node.Content.WorldOrientation = orientation;
            }

            return sceneEntityNode;
        }

        public static IEnumerable<Node<T>> SetOrientation<T>(this IEnumerable<Node<T>> sceneEntityNodes, Orientation orientation) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (Node<SceneEntity> node in sceneEntityNodes.GetDescendantsOfType<SceneEntity>())
            {
                node.Content.WorldOrientation = orientation;
            }

            foreach (Node<T> node in sceneEntityNodes)
            {
                node.Content.WorldOrientation = orientation;
                yield return node;
            }
        }

        public static IEnumerable<Node<T>> SetOrientation<T>(this IEnumerable<Node<T>> sceneEntityNodes, Orientation orientation, Func<Node<T>, bool> predicate) where T : SceneEntity
        {
            ThrowIfParameterIsNull(orientation);

            foreach (Node<SceneEntity> node in sceneEntityNodes.GetDescendantsOfType<SceneEntity>(predicate))
            {
                node.Content.WorldOrientation = orientation;
            }

            foreach (Node<T> node in sceneEntityNodes.Where(predicate))
            {
                node.Content.WorldOrientation = orientation;
                yield return node;
            }
        }

        #endregion

        #region TranslateX method

        public static T TranslateX<T>(this T sceneEntity, float distance) where T : SceneEntity
        {
            sceneEntity.WorldOrigin += new Vector3D(distance, 0, 0);

            return sceneEntity;
        }

        public static IEnumerable<T> TranslateX<T>(this IEnumerable<T> sceneEntities, float distance) where T : SceneEntity
        {
            var displacement = new Vector3D(distance, 0, 0);
            foreach (T sceneEntity in sceneEntities)
            {
                sceneEntity.WorldOrigin += displacement;
                yield return sceneEntity;
            }
        }

        #endregion

        #region TranslateY method

        public static T TranslateY<T>(this T sceneEntity, float distance) where T : SceneEntity
        {
            sceneEntity.WorldOrigin += new Vector3D(0, distance, 0);

            return sceneEntity;
        }

        public static IEnumerable<T> TranslateY<T>(this IEnumerable<T> sceneEntities, float distance) where T : SceneEntity
        {
            var displacement = new Vector3D(0, distance, 0);
            foreach (T sceneEntity in sceneEntities)
            {
                sceneEntity.WorldOrigin += displacement;
                yield return sceneEntity;
            }
        }

        #endregion

        #region TranslateZ method

        public static T TranslateZ<T>(this T sceneEntity, float distance) where T : SceneEntity
        {
            sceneEntity.WorldOrigin += new Vector3D(0, 0, distance);

            return sceneEntity;
        }

        public static IEnumerable<T> TranslateZ<T>(this IEnumerable<T> sceneEntities, float distance) where T : SceneEntity
        {
            var displacement = new Vector3D(0, 0, distance);
            foreach (T sceneEntity in sceneEntities)
            {
                sceneEntity.WorldOrigin += displacement;
                yield return sceneEntity;
            }
        }

        #endregion

        #region Translate method

        public static T Translate<T>(this T sceneEntity, Vector3D displacement) where T : SceneEntity
        {
            sceneEntity.WorldOrigin += displacement;

            return sceneEntity;
        }

        public static IEnumerable<T> Translate<T>(this IEnumerable<T> sceneEntities, Vector3D displacement) where T : SceneEntity
        {
            foreach (T sceneEntity in sceneEntities)
            {
                sceneEntity.WorldOrigin += displacement;
                yield return sceneEntity;
            }
        }

        #endregion





        /// <summary>
        /// Sets the <see cref="Orientation"/> to the passed argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public static T SetOrientation<T>(this T sceneObject, Orientation orientation, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            if (orientation is null)
            {
                throw GenerateException<ParameterCannotBeNullException>.WithParameters(nameof(orientation));
            }

            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrientation = orientation;
            }

            return sceneObject;
        }

        public static T SetDirection1<T>(this T sceneObject, Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp) where T : SceneEntity
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
        public static T SetDirection2<T>(this T sceneObject, Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight) where T : SceneEntity
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
        public static T SetDirection3<T>(this T sceneObject, Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward) where T : SceneEntity
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
        public static T Rotate<T>(this T sceneObject, Vector3D axis, float angle, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            if (angle.ApproxMoreThan(0, epsilon))
            {
                Matrix4x4 rotation = Transform.Rotate(axis, angle);

                foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
                {
                    child.WorldOrigin = (Vector3D)(rotation * new Vector4D(child.WorldOrigin, 1));
                    child.WorldOrientation = Orientation.CreateOrientationForwardUp(
                        (Vector3D)(rotation * new Vector4D(child.WorldDirectionForward, 1)),
                        (Vector3D)(rotation * new Vector4D(child.WorldDirectionUp, 1))
                    );

                    switch (child)
                    {
                        case Mesh mesh:
                            foreach (Vertex vertex in mesh.Vertices)
                            {
                                if (vertex.Normal.HasValue)
                                {
                                    vertex.Normal = (Vector3D)(rotation * new Vector4D(vertex.Normal.Value, 1));
                                }
                            }
                            break;
                    }
                }
            }

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
        public static T RotateBetweenVectors<T>(this T sceneObject, Vector3D v1, Vector3D v2, Vector3D? axis = null, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            Matrix4x4 rotation = Transform.RotateBetweenVectors(v1, v2, axis);

            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin = (Vector3D)(rotation * new Vector4D(child.WorldOrigin, 1));
                child.WorldOrientation.SetDirectionForwardUp(
                    (Vector3D)(rotation * new Vector4D(child.WorldDirectionForward, 1)),
                    (Vector3D)(rotation * new Vector4D(child.WorldDirectionUp, 1))
                );

                switch (child)
                {
                    case Mesh mesh:
                        foreach (Vertex vertex in mesh.Vertices)
                        {
                            if (vertex.Normal.HasValue)
                            {
                                vertex.Normal = (Vector3D)(rotation * new Vector4D(vertex.Normal.Value, 1));
                            }
                        }
                        break;
                }
            }

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
        public static T TranslateX<T>(this T sceneObject, float distance, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin += new Vector3D(distance, 0, 0);
            }
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static T TranslateY<T>(this T sceneObject, float distance, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin += new Vector3D(0, distance, 0);
            }
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static T TranslateZ<T>(this T sceneObject, float distance, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin += new Vector3D(0, 0, distance);
            }
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static T Translate<T>(this T sceneObject, Vector3D displacement, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin += displacement;
            }
            return sceneObject;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sceneObject"></param>
        /// <param name="destination"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T TranslateTo<T>(this T sceneObject, Vector3D destination, Predicate<SceneEntity> predicate = null) where T : SceneEntity
        {
            foreach (SceneEntity child in sceneObject.GetAllChildrenAndSelf(predicate))
            {
                child.WorldOrigin = destination;
            }
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
        /// Translates the <see cref="SceneEntity"/> in the x-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateX(float distance) => WorldOrigin += new Vector3D(distance, 0, 0);

        /// <summary>
        /// Translates the <see cref="SceneEntity"/> in the y-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateY(float distance) => WorldOrigin += new Vector3D(0, distance, 0);

        /// <summary>
        /// Translates the <see cref="SceneEntity"/> in the z-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateZ(float distance) => WorldOrigin += new Vector3D(0, 0, distance);

        /// <summary>
        /// Translates the <see cref="SceneEntity"/> by the given vector.
        /// </summary>
        /// <param name="displacement">Vector to translate by.</param>
        public virtual void Translate(Vector3D displacement) => WorldOrigin += displacement;

        #endregion
    }
}