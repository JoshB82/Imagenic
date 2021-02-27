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
    /// Collection of <see cref="Settings"/>.
    /// </summary>
    public static class Settings
    {
        #region Engine Settings

        public static Resolution Z_Buffer_Resolution = Resolution.Float;

        public static Viewport Viewport_Style = Viewport.Single;

        // Clipping
        public static bool Screen_Space_Clip { get; set; }

        #endregion
    }
}