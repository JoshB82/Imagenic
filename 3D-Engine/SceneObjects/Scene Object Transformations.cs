using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.Transformations;
using System;
using System.Diagnostics;

namespace _3D_Engine.SceneObjects
{
    public abstract partial class SceneObject
    {
        #region Rotations

        /// <summary>
        /// Sets the forward, up and right directions given the forward and up directions.
        /// </summary>
        /// <param name="newWorldDirectionForward">The forward direction.</param>
        /// <param name="newWorldDirectionUp">The up direction.</param>
        public virtual void SetDirection1(Vector3D newWorldDirectionForward, Vector3D newWorldDirectionUp)
        {
            if (newWorldDirectionForward.Approx_Equals(Vector3D.Zero, 1E-6f) ||
                newWorldDirectionUp.Approx_Equals(Vector3D.Zero, 1E-6f))
                throw new ArgumentException("New direction vector(s) cannot be set to zero vector.");

            // if (new_world_direction_forward * new_world_direction_up != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            newWorldDirectionForward = newWorldDirectionForward.Normalise();
            newWorldDirectionUp = newWorldDirectionUp.Normalise();

            AdjustVectors(
                newWorldDirectionForward,
                newWorldDirectionUp,
                Transform.CalculateDirectionRight(newWorldDirectionForward, newWorldDirectionUp)
            );
            Output_Direction();
        }
        
        /// <summary>
        /// Sets the forward, up and right directions given the up and right directions.
        /// </summary>
        /// <param name="newWorldDirectionUp">The up direction.</param>
        /// <param name="newWorldDirectionRight">The right direction.</param>
        public virtual void SetDirection2(Vector3D newWorldDirectionUp, Vector3D newWorldDirectionRight)
        {
            if (newWorldDirectionUp.Approx_Equals(Vector3D.Zero, 1E-6f) ||
                newWorldDirectionRight.Approx_Equals(Vector3D.Zero, 1E-6f))
                throw new ArgumentException("New direction vector(s) cannot be set to zero vector.");

            // if (new_world_direction_up * new_world_direction_right != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            newWorldDirectionUp = newWorldDirectionUp.Normalise();
            newWorldDirectionRight = newWorldDirectionRight.Normalise();

            AdjustVectors(
                Transform.CalculateDirectionForward(newWorldDirectionUp, newWorldDirectionRight),
                newWorldDirectionUp,
                newWorldDirectionRight
            );
            Output_Direction();
        }
        /// <summary>
        /// Sets the forward, up and right directions given the right and forward directions.
        /// </summary>
        /// <param name="newWorldDirectionRight">The right direction.</param>
        /// <param name="newWorldDirectionForward">The forward direction.</param>
        public virtual void SetDirection3(Vector3D newWorldDirectionRight, Vector3D newWorldDirectionForward)
        {
            if (newWorldDirectionRight.Approx_Equals(Vector3D.Zero, 1E-6f) ||
                newWorldDirectionForward.Approx_Equals(Vector3D.Zero, 1E-6f))
                throw new ArgumentException("New direction vector(s) cannot be set to zero vector.");

            // if (new_world_direction_right * new_world_direction_forward != 0) throw new ArgumentException("Direction vectors are not orthogonal.");

            newWorldDirectionForward = newWorldDirectionForward.Normalise();
            newWorldDirectionRight = newWorldDirectionRight.Normalise();

            AdjustVectors(
                newWorldDirectionForward,
                Transform.CalculateDirectionUp(newWorldDirectionRight, newWorldDirectionForward),
                newWorldDirectionRight
            );
            Output_Direction();
        }

        private void AdjustVectors(Vector3D direction_forward, Vector3D direction_up, Vector3D direction_right)
        {
            WorldDirectionForward = direction_forward;
            WorldDirectionUp = direction_up;
            WorldDirectionRight = direction_right;

            if (HasDirectionArrows && DirectionArrows is not null)
            {
                ((Arrow)DirectionArrows.SceneObjects[0]).Unit_Vector = direction_forward;
                ((Arrow)DirectionArrows.SceneObjects[1]).Unit_Vector = direction_up;
                ((Arrow)DirectionArrows.SceneObjects[2]).Unit_Vector = direction_right;
            }
        }
        private void Output_Direction()
        {
            if (!Settings.Trace_Output) return;

            Verbosity Trace_Output_Verbosity = Verbosity.None;
            switch (this) // ??????????????????????????
            {
                case Camera:
                    Trace_Output_Verbosity = Settings.Camera_Trace_Output_Verbosity;
                    break;
                case Light:
                    Trace_Output_Verbosity = Settings.Light_Trace_Output_Verbosity;
                    break;
                case Mesh:
                    Trace_Output_Verbosity = Settings.Mesh_Trace_Output_Verbosity;
                    break;
            }

            string scene_object_type = GetType().Name;
            switch (Trace_Output_Verbosity)
            {
                case Verbosity.Brief:
                    Trace.WriteLine(scene_object_type + " changed direction.");
                    break;
                case Verbosity.Detailed:
                case Verbosity.All:
                    Trace.WriteLine("<=========\n" +
                        scene_object_type + " direction changed to:\n" +
                        $"Forward: {WorldDirectionForward}\n" +
                        $"Up: {WorldDirectionUp}\n" +
                        $"Right: {WorldDirectionRight}\n" +
                        "=========>"
                    );
                    break;
            }
        }

        #endregion

        #region Translations

        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the x-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateX(float distance) => World_Origin += new Vector3D(distance, 0, 0);
        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the y-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateY(float distance) => World_Origin += new Vector3D(0, distance, 0);
        /// <summary>
        /// Translates the <see cref="SceneObject"/> in the z-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void TranslateZ(float distance) => World_Origin += new Vector3D(0, 0, distance);
        /// <summary>
        /// Translates the <see cref="SceneObject"/> by the given vector.
        /// </summary>
        /// <param name="displacement">Vector to translate by.</param>
        public virtual void Translate(Vector3D displacement) => World_Origin += displacement;

        #endregion
    }
}