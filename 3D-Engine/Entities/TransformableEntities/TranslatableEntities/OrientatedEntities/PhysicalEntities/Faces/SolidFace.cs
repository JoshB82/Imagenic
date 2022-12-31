using Imagenic.Core.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace Imagenic.Core.Entities;

public sealed class SolidFace : Face
{
    #region Fields and Properties

    private Color frontColour, backColour;
    public Color FrontColour
    {
        get => frontColour;
        set
        {
            if (value == frontColour) return;
            frontColour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }
    public Color BackColour
    {
        get => backColour;
        set
        {
            if (value == backColour) return;
            backColour = value;
            InvokeRenderEvent(RenderUpdate.NewRender);
        }
    }

    #endregion

    #region Constructors

    public SolidFace(params Triangle[] triangles) : this(triangles)
    {

    }

    public SolidFace(IList<Triangle> triangles) : base(triangles)
    {

    }

    #endregion
}