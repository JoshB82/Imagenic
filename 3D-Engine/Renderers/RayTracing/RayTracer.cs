using _3D_Engine.Entities.SceneObjects;
using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using Imagenic.Core.Images.ImageOptions;
using Imagenic.Core.Renderers.Animations;
using Imagenic.Core.Utilities.Node;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.RayTracing;

public partial class RayTracer<T> : Renderer<T> where T : Image
{
    #region Fields and Properties

    internal bool NewShadowMapNeeded { get; set; }

    #endregion

    #region Constructors

    public RayTracer(RenderingOptions renderingOptions) : this(renderingOptions, null) { }

    public RayTracer(RenderingOptions renderingOptions, IImageOptions<T> imageOptions) : base(renderingOptions, imageOptions) { }

    #endregion

    #region Methods

    public async override Task<T> RenderAsync(CancellationToken token)
    {
        return null;
        //return await GenerateImage();
    }
    /*
    public async override IAsyncEnumerable<T> RenderAsync(PhysicalEntity physicalEntity, Animation animation, [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (Frame frame in animation.Frames)
        {

            //var physicalEntityTweaked = new ;
            var image = await GenerateImage(physicalEntityTweaked);
            yield return image;
        }
    }

    public async override IAsyncEnumerable<T> RenderAsync(IEnumerable<PhysicalEntity> physicalEntities, Animation animation, [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (Frame frame in animation.Frames)
        {

        }
    }

    public async override IAsyncEnumerable<T> RenderAsync(Node<PhysicalEntity> physicalEntityNode, Animation animation [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (Frame frame in animation.Frames)
        {

        }
    }

    public async override IAsyncEnumerable<T> RenderAsync(IEnumerable<Node<PhysicalEntity>> physicalEntityNodes, Animation animation, [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (Frame frame in animation.Frames)
        {

        }
    }

    private async static Task<T> GenerateImage(params PhysicalEntity[] physicalEntities)
    {
        Buffer2D<Color> colourBuffer = await CastRays();
        if (typeof(T) == typeof(Bitmap))
        {
            return new Bitmap(colourBuffer);
        }
    }
    */
    #endregion
}