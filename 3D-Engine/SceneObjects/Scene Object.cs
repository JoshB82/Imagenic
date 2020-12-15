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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine.SceneObjects
{
    /// <summary>
    /// Encapsulates creation of a <see cref="SceneObject"/>.
    /// </summary>
    public abstract partial class SceneObject
    {
        #region Fields and Properties

        // Appearance
        /// <summary>
        /// Determines whether the <see cref="Scene_Object"/> is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        // Direction Arrows
        internal Group DirectionArrows { get; }
        internal bool HasDirectionArrows;

        /// <summary>
        /// Determines whether the <see cref="SceneObject"/> direction arrows are shown or not.
        /// </summary>
        public bool DisplayDirectionArrows { get; set; } = false;

        // Directions
        internal static readonly Vector3D ModelDirectionForward = Vector3D.UnitZ;
        internal static readonly Vector3D ModelDirectionUp = Vector3D.UnitY;
        internal static readonly Vector3D ModelDirectionRight = Vector3D.UnitX;

        /// <summary>
        /// The forward direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public Vector3D WorldDirectionForward { get; private set; }
        /// <summary>
        /// The up direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public Vector3D WorldDirectionUp { get; private set; }
        /// <summary>
        /// The right direction of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public Vector3D WorldDirectionRight { get; private set; }

        // ID
        /// <summary>
        /// The identification number.
        /// </summary>
        public int Id { get; private set; }
        private static int nextId = -1;

        // Matrices
        internal Matrix4x4 ModelToWorld;

        internal virtual void CalculateMatrices()
        {
            Matrix4x4 directionForwardRotation = Transform.Rotate_Between_Vectors(ModelDirectionForward, WorldDirectionForward);
            Matrix4x4 directionUpRotation = Transform.Rotate_Between_Vectors((Vector3D)(directionForwardRotation * ModelDirectionUp), WorldDirectionUp);
            Matrix4x4 translation = Transform.Translate(WorldOrigin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            ModelToWorld = translation * directionUpRotation * directionForwardRotation;
        }

        // Origins
        internal static readonly Vector4D ModelOrigin = Vector4D.UnitW;

        /// <summary>
        /// The position of the <see cref="SceneObject"/> in world space.
        /// </summary>
        public virtual Vector3D WorldOrigin { get; set; }
        internal void CalculateWorldOrigin() => WorldOrigin = (Vector3D)(ModelToWorld * ModelOrigin);

        #endregion

        #region Constructors

        internal SceneObject(Vector3D origin, Vector3D directionForward, Vector3D directionUp, bool hasDirectionArrows = true)
        {
            Id = ++nextId;

            WorldOrigin = origin;
            Set_Direction_1(directionForward, directionUp);

            if (HasDirectionArrows = hasDirectionArrows)
            {
                const int resolution = 30, body_radius = 10, tip_radius = 20, body_length = 10, tip_length = 5;

                List<SceneObject> direction_arrows = new List<SceneObject>
                {
                    new Arrow(origin, WorldDirectionForward, WorldDirectionUp, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Blue }, // Z-axis
                    new Arrow(origin, WorldDirectionUp, -WorldDirectionForward, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Green }, // Y-axis
                    new Arrow(origin, WorldDirectionRight, -WorldDirectionUp, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Red } // X-axis
                };
                DirectionArrows = new Group(origin, directionForward, directionUp, direction_arrows, false);
            }

            Trace.WriteLine($"{GetType().Name} created at {origin}");
        }

        #endregion
    }

    /// <include file="Help_8.xml" path="doc/members/member[@name='T:_3D_Engine.Volume_Outline']/*"/>
    [Flags]
    public enum Volume_Outline : byte
    {
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Volume_Outline.None']/*"/>
        None = 0,
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Volume_Outline.Near']/*"/>
        Near = 1,
        /// <include file="Help_8.xml" path="doc/members/member[@name='F:_3D_Engine.Volume_Outline.Far']/*"/>
        Far = 2
    }
}