using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public abstract class Renderer<T> where T : Image
{
    #region Fields and Properties

    internal bool NewRenderNeeded { get; set; }

    internal IEnumerable<Triangle> TriangleBuffer { get; set; }

    public SceneObject SceneObjectsToRender { get; set; }
    public IImageOptions<T> ImageOptions { get; set; }
    public RenderingOptions RenderingOptions { get; set; }

    #endregion

    #region Methods

    public abstract Task<T> RenderAsync(CancellationToken token);

    public abstract Task<T> RenderAsync(RenderingOptions renderingOptions, CancellationToken token);

    public abstract Task<T> RenderAsync(IImageOptions<T> imageOptions, CancellationToken token);

    public abstract Task<T> RenderAsync(RenderingOptions renderingOptions, IImageOptions<T> imageOptions, CancellationToken token);

    #endregion
}