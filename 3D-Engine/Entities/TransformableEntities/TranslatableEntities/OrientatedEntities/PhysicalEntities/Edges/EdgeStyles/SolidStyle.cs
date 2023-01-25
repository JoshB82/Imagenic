using Imagenic.Core.Enums;
using System.Drawing;

namespace Imagenic.Core.Entities;

internal class SolidEdgeStyle : EdgeStyle
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

    public static readonly SolidEdgeStyle Black = new SolidEdgeStyle(Color.Black);
    public static readonly SolidEdgeStyle Red = new SolidEdgeStyle(Color.Red);

    #endregion

    #region Constructors

    public SolidEdgeStyle(Color colour)
    {
        Colour = colour;
    }

    #endregion
}