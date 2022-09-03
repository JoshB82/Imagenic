using System;

namespace Imagenic.Core.Entities;

internal sealed class RenderUpdateArgs
{
    public bool NewRender { get; set; }
    public bool NewShadowMap { get; set; }
}

[Flags]
internal enum RenderUpdate
{
    NewRender,
    NewShadowMap
}