using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public abstract class Renderer
{
    #region Fields and Properties

    internal bool NewRenderNeeded { get; set; }

    internal IEnumerable<Face> FaceBuffer { get; set; }

    #endregion

    #region Methods

    public abstract Task<Bitmap> Render(RenderingOptions options);

    #endregion
}