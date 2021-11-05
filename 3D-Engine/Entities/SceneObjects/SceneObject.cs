/*
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
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.Entities.SceneObjects
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="SceneObject"/>.
    /// </summary>
    public abstract partial class SceneObject : IList<SceneObject>
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

        private bool displayDirectionArrows = false;
        /// <summary>
        /// Determines whether the <see cref="SceneObject"/> direction arrows are shown or not.
        /// </summary>
        public bool DisplayDirectionArrows
        {
            get => displayDirectionArrows;
            set
            {
                if (value == displayDirectionArrows) return;
                displayDirectionArrows = value;
                RequestNewRenders();
            }
        }
        internal bool HasDirectionArrows { get; set; }

        // Id
        private static int nextId;
        /// <summary>
        /// The identification number.
        /// </summary>
        public int Id { get; } = nextId++;

        // Matrices
        public Matrix4x4 ModelToWorld { get; internal set; }

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
                CalculateModelToWorldMatrix();
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
                CalculateModelToWorldMatrix();
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
                this.AddChildren(
                    new Arrow(worldOrigin, worldOrientation, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Blue),
                    new Arrow(worldOrigin, Orientation.CreateOrientationForwardUp(worldOrientation.DirectionUp, -worldOrientation.DirectionForward), Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Green),
                    new Arrow(worldOrigin, Orientation.CreateOrientationForwardUp(Transform.CalculateDirectionRight(worldOrientation.DirectionForward, worldOrientation.DirectionUp), worldOrientation.DirectionUp), Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false).ColourAllSolidFaces(Color.Red)
                );
            }

            this.SetOrientation(worldOrientation);
            WorldOrigin = worldOrigin;

            #if DEBUG

            DisplayMessage<EntityCreatedMessage>.WithTypeAndParameters<SceneObject>(worldOrigin.ToString());

            #endif
        }

        #endregion

        #region Methods

        internal virtual void CalculateModelToWorldMatrix()
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

        #endregion
    }
}