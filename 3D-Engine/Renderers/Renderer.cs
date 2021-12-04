using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Images;
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

    public abstract Task<Image> Render(RenderingOptions options);

    #endregion
}