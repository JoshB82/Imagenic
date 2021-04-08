/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a scene object.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Miscellaneous;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Transformations;
using System.Drawing;
using static _3D_Engine.Properties.Settings;

namespace _3D_Engine.SceneObjects
{
    /// <summary>
    /// Encapsulates creation of a <see cref="SceneObject"/>.
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
                visible = value;
                UpdateRenderCamera();
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
                displayDirectionArrows = value;
                UpdateRenderCamera();
            }
        }
        internal bool HasDirectionArrows { get; set; }

        // Id
        /// <summary>
        /// The identification number.
        /// </summary>
        public int Id { get; private set; }
        private static int nextId = -1;

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
                worldOrigin = value;

                CalculateMatrices();

                UpdateRenderCamera();
            }
        }

        // Render Camera
        internal Camera RenderCamera { get; set; }
        internal void UpdateRenderCamera()
        {
            if (RenderCamera is not null) RenderCamera.NewRenderNeeded = true;
        }

        #endregion

        #region Constructors

        internal SceneObject(Vector3D origin, Vector3D directionForward, Vector3D directionUp, bool hasDirectionArrows = true)
        {
            Id = ++nextId;

            if (HasDirectionArrows = hasDirectionArrows)
            {
                Arrow DirectionForwardArrow = new(origin, directionForward, directionUp, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false) { FaceColour = Color.Blue };
                Arrow DirectionUpArrow = new(origin, directionUp, -directionForward, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false) { FaceColour = Color.Green };
                Arrow DirectionRightArrow = new(origin, Transform.CalculateDirectionRight(directionForward, directionUp), directionUp, Default.DirectionArrowBodyLength, Default.DirectionArrowTipLength, Default.DirectionArrowBodyRadius, Default.DirectionArrowTipRadius, Default.DirectionArrowResolution, false) { FaceColour = Color.Red };

                DirectionArrows = new(DirectionForwardArrow, DirectionUpArrow, DirectionRightArrow);
            }

            SetDirection1(directionForward, directionUp);
            WorldOrigin = origin;

            ConsoleOutput.DisplayMessageFromObject(this, $"Created at {origin}.");
        }

        #endregion
    }
}