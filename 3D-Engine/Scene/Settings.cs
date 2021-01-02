using System.Diagnostics;

namespace _3D_Engine
{
    public enum Resolution
    {
        Int, // Less memory, more speed
        Float,
        Double // More memory, less speed
        //Decimal?
    }

    /// <summary>
    /// Options for the style of the viewport.??????????????
    /// </summary>
    public enum Viewport
    {
        Single,
        Float_Left_Right,
        Float_Top_Bottom
    }

    /// <summary>
    /// Options for how verbose output should be.??????????????
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

        public static Resolution Z_Buffer_Resolution = Resolution.Float;

        public static Viewport Viewport_Style = Viewport.Single;

        // Clipping
        public static bool Screen_Space_Clip { get; set; }

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