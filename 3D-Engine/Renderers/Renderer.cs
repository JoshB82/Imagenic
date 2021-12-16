using _3D_Engine.Constants;
using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using _3D_Engine.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers;

public abstract class Renderer<T> where T : Image
{
    #region Fields and Properties

    internal bool NewRenderNeeded { get; set; }

    internal List<Triangle> TriangleBuffer { get; set; }

    private SceneObject sceneObjectsToRender;
    public SceneObject SceneObjectsToRender
    {
        get => sceneObjectsToRender;
        set
        {
            if (value == sceneObjectsToRender) return;
            if (value is null)
            {
                throw new MessageBuilder<ParameterCannotBeNullException>()
                    .AddParameters(nameof(value))
                    .BuildIntoException<ParameterCannotBeNullException>();
            }
            sceneObjectsToRender = value;

            TriangleBuffer.Clear();
            foreach (Mesh child in sceneObjectsToRender.GetAllChildrenAndSelf<Mesh>(x => x.Visible))
            {
                foreach (Face face in child.Structure.Faces)
                {
                    TriangleBuffer.AddRange(face.Triangles);
                }
            }
        }
    }
    public IImageOptions<T> ImageOptions { get; set; }
    public RenderingOptions RenderingOptions { get; set; }

    #endregion

    #region Constructors

    public Renderer(SceneObject toRender)
    {
        SceneObjectsToRender = toRender;
    }

    #endregion

    #region Methods

    public abstract Task<T> RenderAsync(CancellationToken token);

    public abstract Task<T> RenderAsync(RenderingOptions renderingOptions, CancellationToken token);

    public abstract Task<T> RenderAsync(IImageOptions<T> imageOptions, CancellationToken token);

    public abstract Task<T> RenderAsync(RenderingOptions renderingOptions, IImageOptions<T> imageOptions, CancellationToken token);

    #endregion
}