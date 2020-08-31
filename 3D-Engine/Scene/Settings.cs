using System.Diagnostics;

namespace _3D_Engine
{
    public enum Resolution
    {
        Int, // Less memory (??)
        Float,
        Double,
        Decimal // More memory (??)
    }

    /// <summary>
    /// Options for the style of the viewport.
    /// </summary>
    public enum Viewport
    {
        Single,
        Double_Left_Right,
        Double_Top_Bottom
    }

    /// <summary>
    /// Options for how verbose output should be.
    /// </summary>
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

        // Trace
        /// <summary>
        /// Determines if any <see cref="Trace"/> text is outputted.
        /// </summary>
        public static bool Trace_Output = false;
        public static Verbosity Camera_Trace_Output_Verbosity = Verbosity.None;
        public static Verbosity Light_Trace_Output_Verbosity = Verbosity.None;
        public static Verbosity Mesh_Trace_Output_Verbosity = Verbosity.None;

        #endregion
    }
}