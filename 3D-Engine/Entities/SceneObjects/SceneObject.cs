﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * An abstract base class that defines objects of type SceneObject.
 */

using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _3D_Engine.Entities.SceneObjects
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="SceneObject"/>.
    /// </summary>
    public abstract class SceneObject : IList<SceneObject>
    {
        #region Fields and Properties

        // Appearance
        private bool visible = true;
        /// <summary>
        /// Determines whether the <see cref="SceneObject"/> is visible or not.
        /// </summary>
        public bool Visible
        {
            get => visible;
            set
            {
                if (value == visible) return;
                visible = value;
                RequestNewRenders();
            }
        }

        // Id
        private static int nextId;
        /// <summary>
        /// The identification number.
        /// </summary>
        public int Id { get; } = nextId++;

        // Matrices
        public Matrix4x4 ModelToWorld { get; internal set; }
        internal virtual void CalculateMatrices()
        {
            Matrix4x4 directionForwardRotation = Transform.RotateBetweenVectors(Orientation.ModelDirectionForward, worldOrientation.DirectionForward);
            Matrix4x4 directionUpRotation = Transform.RotateBetweenVectors((Vector3D)(directionForwardRotation * Orientation.ModelDirectionUp), worldOrientation.DirectionUp);
            Matrix4x4 translation = Transform.Translate(WorldOrigin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            ModelToWorld = translation * directionUpRotation * directionForwardRotation;
        }

        // Orientation
        private Orientation worldOrientation;
        public Orientation WorldOrientation
        {
            get => worldOrientation;
            set
            {
                if (value == worldOrientation) return;
                if (value is null)
                {
                    throw GenerateException<ParameterCannotBeNullException>.WithParameters(nameof(value));
                }
                worldOrientation = value;
                CalculateMatrices();
                RequestNewRenders();
            }
        }

        // Origins
        internal static readonly Vector4D ModelOrigin = Vector4D.UnitW;
        private Vector3D worldOrigin;
        /// <summary>
        /// The position of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public virtual Vector3D WorldOrigin
        {
            get => worldOrigin;
            set
            {
                if (value == worldOrigin) return;
                worldOrigin = value;
                CalculateMatrices();
                RequestNewRenders();
            }
        }

        // Render Camera
        internal List<Camera> RenderCameras { get; set; } = new();
        internal virtual void RequestNewRenders()
        {
            foreach (Camera camera in RenderCameras)
            {
                camera.NewRenderNeeded = true;
            }
        }

        // Tree
        private SceneObject parent;
        public SceneObject Parent
        {
            get => parent;
            set
            {
                parent.RemoveChildren(this);
                parent = value;
                parent.AddChildren(this);
            }
        }
        private IList<SceneObject> children = new List<SceneObject>();
        public IList<SceneObject> Children
        {
            get => children;
            set
            {
                foreach (SceneObject child in children)
                {
                    child.Parent = null;
                }
                children = value;
                foreach (SceneObject child in children)
                {
                    child.Parent = this;
                }
            }
        }

        public int Count => Children.Count;

        public bool IsReadOnly => Children.IsReadOnly;

        public SceneObject this[int index] { get => Children[index]; set => Children[index] = value; }

        public void AddChildren(IEnumerable<SceneObject> children)
        {
            foreach (SceneObject child in children)
            {
                Children.Add(child);
                child.Parent = this;
            }
        }
        public void AddChildren(params SceneObject[] children) => AddChildren(children);

        public void RemoveChildren(IEnumerable<SceneObject> children)
        {
            foreach (SceneObject child in children)
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
        public void RemoveChildren(params SceneObject[] children) => RemoveChildren(children);

        public void RemoveChildren(Predicate<SceneObject> predicate)
        {
            foreach (SceneObject child in Children)
            {
                if (predicate(child))
                {
                    Children.Remove(child);
                }
            }
        }

        public void RemoveChildren<T>(Predicate<T> predicate = null)
        {
            foreach (SceneObject child in Children)
            {
                if (child is T t && ((predicate is not null && predicate(t)) || predicate is null))
                {
                    Children.Remove(child);
                }
            }
        }

        public IEnumerable<SceneObject> GetAllParents(Predicate<SceneObject> predicate = null)
        {
            List<SceneObject> parents = new();
            if (Parent is not null && ((predicate is not null && predicate(Parent)) || predicate is null))
            {
                parents.Add(Parent);
                parents.AddRange(Parent.GetAllParents(predicate));
            }
            return parents;
        }

        public IEnumerable<T> GetAllParents<T>(Predicate<T> predicate = null) where T : SceneObject
        {
            return this.GetAllParents(x => x is T t && predicate(t)) as IEnumerable<T>;
        }

        public IEnumerable<SceneObject> GetAllParentsAndSelf(Predicate<SceneObject> predicate = null)
        {
            List<SceneObject> sceneObjects = this.GetAllParents(predicate).ToList();
            if ((predicate is not null && predicate(this)) || predicate is null)
            {
                sceneObjects.Add(this);
            }
            return sceneObjects;
        }

        public IEnumerable<T> GetAllParentsAndSelf<T>(Predicate<T> predicate = null) where T : SceneObject
        {
            return this.GetAllParentsAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
        }

        public IEnumerable<SceneObject> GetAllChildren(Predicate<SceneObject> predicate = null)
        {
            List<SceneObject> children = new();
            foreach (SceneObject child in Children)
            {
                if (predicate is not null && !predicate(child))
                {
                    continue;
                }

                children.Add(child);
                children.AddRange(child.GetAllChildren(predicate));
            }
            return children;
        }

        public IEnumerable<T> GetAllChildren<T>(Predicate<T> predicate = null) where T : SceneObject
        {
            return this.GetAllChildren(x => x is T t && predicate(t)) as IEnumerable<T>;
        }

        public IEnumerable<SceneObject> GetAllChildrenAndSelf(Predicate<SceneObject> predicate = null)
        {
            List<SceneObject> sceneObjects = this.GetAllChildren(predicate).ToList();
            if ((predicate is not null && predicate(this)) || predicate is null)
            {
                sceneObjects.Add(this);
            }
            return sceneObjects;
        }

        /// <summary>
        /// Gets all children and this that are of type <typeparamref name="T"/> and an optional predicate.
        /// </summary>
        /// <typeparam name="T">The type of all the returned <see cref="SceneObject">SceneObjects</see>.</typeparam>
        /// <param name="predicate">A <see cref="Predicate{T}"/> that all returned <see cref="SceneObject">SceneObjects</see> must satisfy.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAllChildrenAndSelf<T>(Predicate<T> predicate = null) where T : SceneObject
        {
            return this.GetAllChildrenAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
        }

        public IEnumerator<SceneObject> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(SceneObject item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, SceneObject item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void Add(SceneObject item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(SceneObject item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(SceneObject[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(SceneObject item)
        {
            return Children.Remove(item);
        }

        #endregion

        #region Constructors

        internal SceneObject(Vector3D worldOrigin,
                             Orientation worldOrientation,
                             bool hasDirectionArrows = true)
        {
            if (HasDirectionArrows = hasDirectionArrows)
            {


                DirectionArrows = new(DirectionForwardArrow, DirectionUpArrow, DirectionRightArrow);
            }

            this.SetOrientation(worldOrientation);
            this.worldOrientation.LinkedSceneObject = this;
            WorldOrigin = worldOrigin;

            #if DEBUG

            DisplayMessage<EntityCreatedMessage>.WithTypeAndParameters<SceneObject>(worldOrigin.ToString());

            #endif
        }

        #endregion
    }
}