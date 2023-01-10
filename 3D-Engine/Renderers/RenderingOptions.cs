using Imagenic.Core.Entities;
using Imagenic.Core.Renderers.Animations;
using System.Collections.Generic;

namespace Imagenic.Core.Renderers;

public sealed class RenderingOptions
{
    #region Fields and Properties

    public int RenderWidth { get; set; } = 1920;
    public int RenderHeight { get; set; } = 1080;

    public IEnumerable<PhysicalEntity>? PhysicalEntitiesToRender { get; set; } = new List<PhysicalEntity>();
    public Animation? AnimationToRender { get; set; }

    public bool RenderAnimation { get; set; } = true;

    #endregion
}