using System.Diagnostics;

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
        public Vector3D World_Origin { get; set; }

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

        // Appearance
        /// <summary>
        /// Determines whether the <see cref="Scene_Object"/> is visible or not.
        /// </summary>
        public bool Visible { get; set; } = true;

        internal Scene_Object(Vector3D origin, Vector3D direction_forward, Vector3D direction_up)
        {
            ID = ++next_id;

            World_Origin = origin;
            Set_Direction_1(direction_forward, direction_up);

            Debug.WriteLine(GetType().Name + $" created at {origin}");
        }
    }
}