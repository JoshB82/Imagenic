using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public abstract partial class Scene_Object
    {
        // ID
        /// <summary>
        /// Identification number.
        /// </summary>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Origins
        internal Vector4D Origin { get; set; } = Vector4D.Zero;
        /// <summary>
        /// The position of the <see cref="Scene_Object"/> in world space.
        /// </summary>
        public virtual Vector3D World_Origin { get; set; }

        // Directions
        internal Vector3D Model_Direction_Forward { get; } = Vector3D.Unit_Z;
        internal Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        internal Vector3D Model_Direction_Right { get; } = Vector3D.Unit_X;

        /// <summary>
        /// The forward direction of the <see cref="Scene_Object"/> in world space.
        /// </summary>
        public Vector3D World_Direction_Forward { get; private set; }
        /// <summary>
        /// The up direction of the <see cref="Scene_Object"/> in world space.
        /// </summary>
        public Vector3D World_Direction_Up { get; private set; }
        /// <summary>
        /// The right direction of the <see cref="Scene_Object"/> in world space.
        /// </summary>
        public Vector3D World_Direction_Right { get; private set; }

        internal Group Direction_Arrows { get; }
        internal bool Has_Direction_Arrows { get; set; }

        // Appearance
        /// <summary>
        /// Determines whether the <see cref="Scene_Object"/> directions are shown or not.
        /// </summary>
        public bool Display_Direction_Arrows { get; set; } = false;
        /// <summary>
        /// Determines whether the <see cref="Scene_Object"/> is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        internal Scene_Object(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true)
        {
            ID = ++next_id;

            World_Origin = origin;
            Set_Direction_1(direction_forward, direction_up);

            if (Has_Direction_Arrows = has_direction_arrows)
            {
                int resolution = 20, body_radius = 20, tip_radius = 30, body_length = 20, tip_length = 5;
                List<Scene_Object> direction_arrows = new List<Scene_Object>
                {
                    new Arrow(origin, World_Direction_Forward, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Blue },
                    new Arrow(origin, World_Direction_Up, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Green },
                    new Arrow(origin, World_Direction_Right, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Red }
                };
                Direction_Arrows = new Group(origin, direction_forward, direction_up, direction_arrows, false);
            }

            Debug.WriteLine(GetType().Name + $" created at {origin}");
        }
    }
}