using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public abstract class Renderer
{
    #region Fields and Properties

    internal bool NewRenderNeeded { get; set; }

    internal IEnumerable<Triangle> TriangleBuffer { get; set; }

    #endregion

    #region Methods

    public abstract Task<T> Render<T>(IImageOptions<T> imageOptions, RenderingOptions options) where T : Image;

    #endregion
}