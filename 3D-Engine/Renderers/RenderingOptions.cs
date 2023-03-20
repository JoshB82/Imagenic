using Imagenic.Core.Entities;
using Imagenic.Core.Renderers.Animations;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Renderers;

public sealed class RenderingOptions
{
    #region Fields and Properties

    // Render size
    private int renderWidth = 1920, renderHeight = 1080;

    public int RenderWidth
    {
        get => renderWidth;
        set
        {
            if (value == renderWidth) return;
            renderWidth = value;
            InvokeRenderSizeChangedEvent(renderWidth, renderHeight);
        }
    }
    public int RenderHeight
    {
        get => renderHeight;
        set
        {
            if (value == renderHeight) return;
            renderHeight = value;
            InvokeRenderSizeChangedEvent(renderWidth, renderHeight);
        }
    }

    internal event Action<int, int>? RenderSizeChanged;
    internal void InvokeRenderSizeChangedEvent(int newRenderWidth, int newRenderHeight)
    {
        RenderSizeChanged?.Invoke(newRenderWidth, newRenderHeight);
    }

    // Render contents
    public IEnumerable<PhysicalEntity>? PhysicalEntitiesToRender { get; set; } = new List<PhysicalEntity>();
    public Animation? AnimationToRender { get; set; }

    public bool RenderAnimation { get; set; } = true;

    

    #endregion
}