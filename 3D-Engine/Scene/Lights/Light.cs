using System.Drawing;

namespace _3D_Engine
{
    public abstract class Light : Scene_Object
    {
        #region Fields and Properties

        // ID
        /// <summary>
        /// Unique identification number for this light.
        /// </summary>
        public int ID { get; private set; }
        private static int next_id = -1;

        // Origins
        public Vector3D Origin { get; }
        public Vector4D World_Origin { get; set; }

        // Directions
        public Vector3D Model_Direction { get; } = Vector3D.Unit_X;
        public Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        public Vector3D Model_Direction_Right { get; } = Vector3D.Unit_Z;

        // See other files on using methods
        private Vector3D world_direction;
        public Vector3D World_Direction
        {
            get => world_direction;
            set { world_direction = value.Normalise(); }
        }
        public Vector3D World_Direction_Up { get; set; }
        public Vector3D World_Direction_Right { get; set; }

        // Transformations
        public Vector3D Translation { get; protected set; }

        // Appearance
        public Color Colour { get; set; }
        public double Intensity { get; set; }
        public string Icon { get; } = "";

        #endregion

        #region Constructors

        public Light()
        {
            ID = ++next_id;
        }

        #endregion
    }
}