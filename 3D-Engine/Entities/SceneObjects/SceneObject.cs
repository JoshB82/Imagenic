﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * An abstract base class that defines objects of type SceneObject. Any object which inherits from this class can be part of a Group.
 */

using _3D_Engine.Entities.Groups;
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
    /// An abstract base class that defines objects of type <see cref="SceneObject"/>. Any object which inherits from this class can be part of a <see cref="Group"/>.
    /// </summary>
    public abstract class SceneObject : IEnumerable<SceneObject>
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



        /// <summary>
        /// The forward direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public virtual Vector3D WorldDirectionForward { get; protected set; }
        /// <summary>
        /// The up direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public Vector3D WorldDirectionUp { get; private set; }
        /// <summary>
        /// The right direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public Vector3D WorldDirectionRight { get; private set; }

        internal Group DirectionArrows { get; set; }

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
            Matrix4x4 directionForwardRotation = Transform.RotateBetweenVectors(Orientation.ModelDirectionForward, WorldDirectionForward);
            Matrix4x4 directionUpRotation = Transform.RotateBetweenVectors((Vector3D)(directionForwardRotation * Orientation.ModelDirectionUp), WorldDirectionUp);
            Matrix4x4 translation = Transform.Translate(WorldOrigin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            ModelToWorld = translation * directionUpRotation * directionForwardRotation;
        }

        // Orientation
        public Orientation WorldOrientation { get; set; }

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

        public void AddChildren(params SceneObject[] children)
        {
            foreach (SceneObject child in children)
            {
                Children.Add(child);
                child.Parent = this;
            }
        }
        public void AddChildren(IEnumerable<SceneObject> children) => AddChildren(children.ToArray());

        public void RemoveChildren(params SceneObject[] children)
        {
            foreach (SceneObject child in children)
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
        public void RemoveChildren(IEnumerable<SceneObject> children) => RemoveChildren(children.ToArray());

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

        public IEnumerable<SceneObject> GetAllChildrenAndSelf(Predicate<SceneObject> predicate = null)
        {
            List<SceneObject> sceneObjects = this.GetAllChildren(predicate).ToList();
            if ((predicate is not null && predicate(this)) || predicate is null)
            {
                sceneObjects.Add(this);
            }
            return sceneObjects;
        }

        public IEnumerator<SceneObject> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

            SetDirection1(worldOrientation.DirectionForward, worldOrientation.DirectionUp);
            WorldOrigin = worldOrigin;

            #if DEBUG

            ConsoleOutput.DisplayMessageFromObject(this, $"Created at {worldOrigin}.");

            #endif
        }

        #endregion
    }
}