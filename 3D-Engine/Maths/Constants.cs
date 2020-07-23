using System;

namespace _3D_Engine
{
    /// <summary>
    /// Collection of frequently used constants.
    /// </summary>
    public static class Constants
    {
        #region Physics Constants

        /// <summary>
        /// Gravitational acceleration near the surface of the Earth.
        /// </summary>
        public const double Grav_Acc = -9.81;

        #endregion
    }

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
        All
    }

    /// <summary>
    /// Collection of settings.
    /// </summary>
    public static class Settings
    {
        #region Engine Settings

        public static Resolution Z_Buffer_Resolution = Resolution.Double;
        public static bool View_Space_Clip = true;
        public static bool Screen_Space_Clip = true;

        public static Viewport Viewport_Style = Viewport.Single;

        public static Verbosity Debug_Output_Verbosity = Verbosity.None;
        public static Verbosity Camera_Debug_Output_Verbosity = Verbosity.None;
        public static Verbosity Light_Debug_Output_Verbosity = Verbosity.None;
        public static Verbosity Mesh_Debug_Output_Verbosity = Verbosity.None;

        #endregion
    }
}
