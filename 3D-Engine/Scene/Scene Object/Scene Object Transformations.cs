using System;
using System.Diagnostics;

namespace _3D_Engine
{
    public abstract partial class Scene_Object
    {
        #region Rotations

        /// <summary>
        /// Sets the forward, up and right directions given the forward and up directions.
        /// </summary>
        /// <param name="new_world_direction_forward">The forward direction.</param>
        /// <param name="new_world_direction_up">The up direction.</param>
        public virtual void Set_Direction_1(Vector3D new_world_direction_forward, Vector3D new_world_direction_up)
        {
            if (new_world_direction_forward == Vector3D.Zero || new_world_direction_up == Vector3D.Zero) throw new Exception("New direction vector(s) cannot be set to zero vector.");
            // if (new_world_direction_forward * new_world_direction_up != 0) throw new Exception("Direction vectors are not orthogonal.");

            new_world_direction_forward = new_world_direction_forward.Normalise();
            new_world_direction_up = new_world_direction_up.Normalise();

            Adjust_Vectors(
                new_world_direction_forward,
                new_world_direction_up,
                Transform.Calculate_Direction_Right(new_world_direction_forward, new_world_direction_up)
            );
            Output_Direction();
        }
        /// <summary>
        /// Sets the forward, up and right directions given the up and right directions.
        /// </summary>
        /// <param name="new_world_direction_up">The up direction.</param>
        /// <param name="new_world_direction_right">The right direction.</param>
        public virtual void Set_Direction_2(Vector3D new_world_direction_up, Vector3D new_world_direction_right)
        {
            if (new_world_direction_up == Vector3D.Zero || new_world_direction_right == Vector3D.Zero) throw new Exception("New direction vector(s) cannot be set to zero vector.");
            // if (new_world_direction_up * new_world_direction_right != 0) throw new Exception("Direction vectors are not orthogonal.");

            new_world_direction_up = new_world_direction_up.Normalise();
            new_world_direction_right = new_world_direction_right.Normalise();

            Adjust_Vectors(
                Transform.Calculate_Direction_Forward(new_world_direction_up, new_world_direction_right),
                new_world_direction_up,
                new_world_direction_right
            );
            Output_Direction();
        }
        /// <summary>
        /// Sets the forward, up and right directions given the right and forward directions.
        /// </summary>
        /// <param name="new_world_direction_right">The right direction.</param>
        /// <param name="new_world_direction_forward">The forward direction.</param>
        public virtual void Set_Direction_3(Vector3D new_world_direction_right, Vector3D new_world_direction_forward)
        {
            if (new_world_direction_right == Vector3D.Zero || new_world_direction_forward == Vector3D.Zero) throw new Exception("New direction vector(s) cannot be set to zero vector.");
            // if (new_world_direction_right * new_world_direction_forward != 0) throw new Exception("Direction vectors are not orthogonal.");

            new_world_direction_forward = new_world_direction_forward.Normalise();
            new_world_direction_right = new_world_direction_right.Normalise();

            Adjust_Vectors(
                new_world_direction_forward,
                Transform.Calculate_Direction_Up(new_world_direction_right, new_world_direction_forward),
                new_world_direction_right
            );
            Output_Direction();
        }

        private void Adjust_Vectors(Vector3D direction_forward, Vector3D direction_up, Vector3D direction_right)
        {
            World_Direction_Forward = direction_forward;
            World_Direction_Up = direction_up;
            World_Direction_Right = direction_right;

            if (Has_Direction_Arrows && Direction_Arrows != null)
            {
                ((Arrow)Direction_Arrows.Scene_Objects[0]).Unit_Vector = direction_forward;
                ((Arrow)Direction_Arrows.Scene_Objects[1]).Unit_Vector = direction_up;
                ((Arrow)Direction_Arrows.Scene_Objects[2]).Unit_Vector = direction_right;
            }
        }
        private void Output_Direction()
        {
            if (!Settings.Debug_Output) return;

            Verbosity Debug_Output_Verbosity = Verbosity.None;
            switch (GetType().BaseType.Name) // ??????????????????????????
            {
                case "Camera":
                    Debug_Output_Verbosity = Settings.Camera_Debug_Output_Verbosity;
                    break;
                case "Light":
                    Debug_Output_Verbosity = Settings.Light_Debug_Output_Verbosity;
                    break;
                case "Mesh":
                    Debug_Output_Verbosity = Settings.Mesh_Debug_Output_Verbosity;
                    break;
            }

            string scene_object_type = GetType().Name;
            switch (Debug_Output_Verbosity)
            {
                case Verbosity.Brief:
                    Debug.WriteLine(scene_object_type + " changed direction.");
                    break;
                case Verbosity.Detailed:
                case Verbosity.All:
                    Debug.WriteLine("<=========\n" +
                        scene_object_type + " direction changed to:\n" +
                        $"Forward: {World_Direction_Forward}\n" +
                        $"Up: {World_Direction_Up}\n" +
                        $"Right: {World_Direction_Right}\n" +
                        "=========>"
                    );
                    break;
            }
        }

        #endregion

        #region Translations

        /// <summary>
        /// Translates the <see cref="Scene_Object"/> in the x-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void Translate_X(double distance) => World_Origin += new Vector3D(distance, 0, 0);
        /// <summary>
        /// Translates the <see cref="Scene_Object"/> in the y-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void Translate_Y(double distance) => World_Origin += new Vector3D(0, distance, 0);
        /// <summary>
        /// Translates the <see cref="Scene_Object"/> in the z-direction.
        /// </summary>
        /// <param name="distance">Amount to translate by.</param>
        public virtual void Translate_Z(double distance) => World_Origin += new Vector3D(0, 0, distance);
        /// <summary>
        /// Translates the <see cref="Scene_Object"/> by the given vector.
        /// </summary>
        /// <param name="displacement">Vector to translate by.</param>
        public virtual void Translate(Vector3D displacement) => World_Origin += displacement;

        #endregion
    }
}