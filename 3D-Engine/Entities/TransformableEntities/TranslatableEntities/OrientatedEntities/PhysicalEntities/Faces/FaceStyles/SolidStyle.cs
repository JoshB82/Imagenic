using Imagenic.Core.Enums;
using System.Drawing;

namespace Imagenic.Core.Entities;

public class SolidStyle : FaceStyle
{
    #region Fields and Properties

    private Color colour;
    public Color Colour
    {
        get => colour;
        set
        {
            if (value == colour) return;
            colour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    public static readonly SolidStyle Black = new SolidStyle(Color.Black);
    public static readonly SolidStyle Red = new SolidStyle(Color.Red);

    #endregion

    #region Constructors

    public SolidStyle(Color colour)
    {
        Colour = colour;
    }

    #endregion
}