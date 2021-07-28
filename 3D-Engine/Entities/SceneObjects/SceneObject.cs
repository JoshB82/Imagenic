/*
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
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Transformations;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System.Collections.Generic;
using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.Entities.SceneObjects
{
    /// <summary>
    /// An abstract base class that defines objects of type <see cref="SceneObject"/>. Any object which inherits from this class can be part of a <see cref="Group"/>.
    /// </summary>
    public abstract partial class SceneObject
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

        // Directions
        internal static readonly Vector3D ModelDirectionForward = Vector3D.UnitZ;
        internal static readonly Vector3D ModelDirectionUp = Vector3D.UnitY;
        internal static readonly Vector3D ModelDirectionRight = Vector3D.UnitX;

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
        internal virtual void CalculateMatrices()
        {
            Matrix4x4 directionForwardRotation = Transform.RotateBetweenVectors(ModelDirectionForward, WorldDirectionForward);
            Matrix4x4 directionUpRotation = Transform.RotateBetweenVectors((Vector3D)(directionForwardRotation * ModelDirectionUp), WorldDirectionUp);
            Matrix4x4 translation = Transform.Translate(WorldOrigin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            ModelToWorld = translation * directionUpRotation * directionForwardRotation;
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
                parent = value;
                parent.Children.Add(this);
            }
        }
        public IList<SceneObject> Children { get; private set; }

        public void AddChild(SceneObject child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        #endregion

        #region Constructors

        internal SceneObject(Vector3D origin,
                             Vector3D directionForward,
                             Vector3D directionUp,
                             bool hasDirectionArrows = true)
        {
            if (HasDirectionArrows = hasDirectionArrows)
            {
                Arrow DirectionForwardArrow = new(origin, directionForward, directionUp, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false);
                Arrow DirectionUpArrow = new(origin, directionUp, -directionForward, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false);
                Arrow DirectionRightArrow = new(origin, Transform.CalculateDirectionRight(directionForward, directionUp), directionUp, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false);

                DirectionForwardArrow.ColourAllSolidFaces(Color.Blue);
                DirectionUpArrow.ColourAllSolidFaces(Color.Green);
                DirectionRightArrow.ColourAllSolidFaces(Color.Red);

                DirectionArrows = new(DirectionForwardArrow, DirectionUpArrow, DirectionRightArrow);
            }

            SetDirection1(directionForward, directionUp);
            WorldOrigin = origin;

            #if DEBUG

            ConsoleOutput.DisplayMessageFromObject(this, $"Created at {origin}.");

            #endif
        }

        #endregion
    }
}