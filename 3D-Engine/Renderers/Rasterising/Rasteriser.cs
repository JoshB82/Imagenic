using _3D_Engine.Entities.SceneObjects;
using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.Rasterising;

public partial class Rasteriser<TImage> : Renderer<TImage> where TImage : Image
{
    #region Fields and Properties

    internal bool NewShadowMapNeeded { get; set; }
    private readonly Action shadowMapDelegate;

    public override Camera RenderCamera
    {
        get => base.RenderCamera;
        set
        {
            base.RenderCamera = value;
            NewShadowMapNeeded = true;
        }
    }

    /*
    public override SceneEntity SceneObjectsToRender
    {
        get => base.SceneObjectsToRender;
        set
        {
            UpdateSubscribers(base.SceneObjectsToRender, false);

            base.SceneObjectsToRender = value;

            UpdateSubscribers(base.SceneObjectsToRender, true);
        }
    }*/

    #endregion

    #region Constructors

    public Rasteriser(RenderingOptions renderingOptions) : base(renderingOptions)
    {
        shadowMapDelegate = () => NewShadowMapNeeded = true;
    }

    #endregion

    #region Methods

    private static Action<SolidRenderTriangle, Buffer2D<Color>, int, int, float> SolidStyleInterpolation = (triangle, colourBuffer, x, y, z) =>
                            {
                                
                            };

    public async override Task<TImage?> RenderAsync(CancellationToken token = default)
    {
        // Check if there is anything to render.
        if (RenderingOptions.PhysicalEntitiesToRender is null)
        {
            return null;
        }

        foreach (PhysicalEntity physicalEntity in RenderingOptions.PhysicalEntitiesToRender)
        {
            var renderTriangles = new List<RenderTriangle>();
            var drawRequired = Decompose(physicalEntity, RenderCamera, renderTriangles);
            if (drawRequired)
            {
                var colourBuffer = new Buffer2D<Color>(RenderingOptions.RenderWidth, RenderingOptions.RenderHeight);
                var zBuffer = new Buffer2D<float>(RenderingOptions.RenderWidth, RenderingOptions.RenderHeight);

                foreach (RenderTriangle triangleToBeDrawn in renderTriangles)
                {
                    triangleToBeDrawn.Interpolate(colourBuffer, zBuffer);


                    /*
                    switch (triangleToBeDrawn)
                    {
                        case SolidRenderTriangle:
                            
                            Interpolator.InterpolateSolidStyle(triangleToBeDrawn, colourBuffer, action);
                            break;
                        case TextureRenderTriangle:
                            await Interpolator.InterpolateTextureStyle();
                            break;
                        case GradientStyle:
                            await Interpolator.InterpolateGradientStyle();
                            break;
                    }
                    */
                }
            }
        }

        return null;
    }

    /*
    public async override IAsyncEnumerable<TImage> RenderAsync(PhysicalEntity physicalEntity, CancellationToken token = default)
    {
        await DecomposePhysicalEntity.Decompose(physicalEntity, RenderCamera, token);
    }*/

    /*
    private void UpdateSubscribers(SceneEntity sceneObject, bool addSubscription)
    {
        Action<Entity> updater = addSubscription
        ? e => e.ShadowMapAlteringPropertyChanged += shadowMapDelegate
        : e => e.ShadowMapAlteringPropertyChanged -= shadowMapDelegate;

        ApplyUpdater(updater, sceneObject);
    }
    */
    /*protected override void ApplyUpdater(Action<Entity> updater)
    {
        foreach (Triangle triangle in TriangleBuffer)
        {
            updater(triangle);
        }

        sceneObject.ForEach(s => updater(s));
    }*/

    #endregion
}