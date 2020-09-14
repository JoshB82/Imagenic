using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    /// <summary>
    /// Handles creation of a <see cref="Scene_Object"/>.
    /// </summary>
    public abstract partial class Scene_Object
    {
        #region Fields and Properties

        // ID
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.ID']/*"/>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Matrices
        internal Matrix4x4 Model_to_World;

        internal virtual void Calculate_Model_to_World()
        {
            Matrix4x4 direction_forward_rotation = Transform.Rotate_Between_Vectors(Model_Direction_Forward, World_Direction_Forward);
            Matrix4x4 direction_up_rotation = Transform.Rotate_Between_Vectors(new Vector3D(direction_forward_rotation * new Vector4D(Model_Direction_Up)), World_Direction_Up);
            Matrix4x4 translation = Transform.Translate(World_Origin);

            // String the transformations together in the following order:
            // 1) Rotation around direction forward vector
            // 2) Rotation around direction up vector
            // 3) Translation to final position in world space
            Model_to_World = translation * direction_up_rotation * direction_forward_rotation;
        }

        // Origins
        internal Vector4D Origin = Vector4D.Zero;
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.World_Origin']/*"/>
        public virtual Vector3D World_Origin { get; set; }

        // Directions
        internal Vector3D Model_Direction_Forward => Vector3D.Unit_Z;
        internal Vector3D Model_Direction_Up => Vector3D.Unit_Y;
        internal Vector3D Model_Direction_Right => Vector3D.Unit_X;

        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.World_Direction_Forward']/*"/>
        public Vector3D World_Direction_Forward { get; private set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.World_Direction_Up']/*"/>
        public Vector3D World_Direction_Up { get; private set; }
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.World_Direction_Right']/*"/>
        public Vector3D World_Direction_Right { get; private set; }

        // Direction Arrows
        internal Group Direction_Arrows { get; }
        internal bool Has_Direction_Arrows;
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.Display_Direction_Arrows']/*"/>
        public bool Display_Direction_Arrows { get; set; } = false;

        // Appearance
        /// <include file="Help_5.xml" path="doc/members/member[@name='P:_3D_Engine.Scene_Object.Visible']/*"/>
        public bool Visible { get; set; } = true;

        #endregion

        #region Constructors

        internal Scene_Object(Vector3D origin, Vector3D direction_forward, Vector3D direction_up, bool has_direction_arrows = true)
        {
            ID = ++next_id;

            World_Origin = origin;
            Set_Direction_1(direction_forward, direction_up);

            if (Has_Direction_Arrows = has_direction_arrows)
            {
                int resolution = 30, body_radius = 10, tip_radius = 20, body_length = 10, tip_length = 5;
                List<Scene_Object> direction_arrows = new List<Scene_Object>
                {
                    new Arrow(origin, World_Direction_Forward, World_Direction_Up, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Blue },
                    new Arrow(origin, World_Direction_Up, -World_Direction_Forward, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Green },
                    new Arrow(origin, World_Direction_Right, -World_Direction_Up, body_length, body_radius, tip_length, tip_radius, resolution, false) { Face_Colour = Color.Red }
                };
                Direction_Arrows = new Group(origin, direction_forward, direction_up, direction_arrows, false);
            }

            Trace.WriteLine($"{GetType().Name} created at {origin}");
        }

        #endregion
    }

    public enum View_Outline
    {
        Near,
        Far,
        Entire
    }
}