using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public sealed class RenderingOptions
{
    #region Fields and Properties

    public Camera RenderingCamera { get; set; }

    public int RenderWidth { get; set; }
    public int RenderHeight { get; set; }

    #endregion
}