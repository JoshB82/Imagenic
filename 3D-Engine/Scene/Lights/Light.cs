using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace _3D_Engine
{
    public abstract partial class Light : Scene_Object
    {
        #region Fields and Properties

        // ID
        /// <summary>
        /// Unique identification number for this <see cref="Light"/>.
        /// </summary>
        public int ID { get; private set; } // ?
        private static int next_id = -1;

        // Origins
        internal Vector3D Origin { get; }
        public Vector3D World_Origin { get; set; }

        // Directions
        internal Vector3D Model_Direction { get; } = Vector3D.Unit_X;
        internal Vector3D Model_Direction_Up { get; } = Vector3D.Unit_Y;
        internal Vector3D Model_Direction_Right { get; } = Vector3D.Unit_Z;

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
        public double Strength { get; set; }

        /*private double intensity; // should it be set to something initially?
        public double Intensity
        {
            get => intensity;
            set { intensity = (value < 0) ? 0 : (value > 1) ? 1 : intensity; }
        }
        */
        public string Icon { get; } = "";

        internal double[][] z_buffer;

        #endregion

        #region Constructors

        internal Light() // ?
        {
            ID = ++next_id;
        }

        #endregion
    }
}