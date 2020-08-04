using System.Diagnostics;
using System.Drawing;

namespace _3D_Engine
{
    public enum Resolution
    {
        Int, // Less memory (??)
        Float,
        Double,
        Decimal // More memory (??)
    }

    public enum Viewport
    {
        Single,
        Double_Left_Right,
        Double_Top_Bottom
    }

    public enum Verbosity
    {
        None,
        Brief,
        Detailed,
        All
    }

    /// <summary>
    /// Collection of <see cref="Settings"/>.
    /// </summary>
    public static class Settings
    {
        #region Engine Settings

        public static Resolution Z_Buffer_Resolution = Resolution.Double;

        public static Viewport Viewport_Style = Viewport.Single;

        // Clipping
        private static bool view_space_clip = true, screen_space_clip = true;
        public static bool View_Space_Clip
        {
            get => view_space_clip;
            set
            {
                if (value == false && screen_space_clip == false)
                {
                    view_space_clip = true;
                }
                else
                {
                    view_space_clip = value;
                }
            }
        }
        public static bool Screen_Space_Clip
        {
            get => screen_space_clip;
            set
            {
                if (value == false && view_space_clip == false)
                {
                    screen_space_clip = true;
                }
                else
                {
                    screen_space_clip = value;
                }
            }
        }

        // Debug
        /// <summary>
        /// Determines if any <see cref="Debug"/> text is outputted.
        /// </summary>
        public static bool Debug_Output = false;
        public static Verbosity Camera_Debug_Output_Verbosity = Verbosity.None;
        public static Verbosity Light_Debug_Output_Verbosity = Verbosity.None;
        public static Verbosity Mesh_Debug_Output_Verbosity = Verbosity.None;

        // Colours
        public static Color Default_Edge_Colour = Color.Black;
        public static Color Default_Face_Colour = Color.BlueViolet;

        #endregion
    }
}