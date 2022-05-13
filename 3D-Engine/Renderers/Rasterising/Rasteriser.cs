using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.Rasterising;

public class Rasteriser<T> : Renderer<T> where T : Image
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

    public override SceneEntity SceneObjectsToRender
    {
        get => base.SceneObjectsToRender;
        set
        {
            UpdateSubscribers(base.SceneObjectsToRender, false);

            base.SceneObjectsToRender = value;

            UpdateSubscribers(base.SceneObjectsToRender, true);
        }
    }

    #endregion

    #region Constructors

    public Rasteriser()
    {
        shadowMapDelegate = () => NewShadowMapNeeded = true;
    }

    #endregion

    #region Methods

    private void UpdateSubscribers(SceneEntity sceneObject, bool addSubscription)
    {
        Action<Entity> updater = addSubscription
        ? e => e.ShadowMapAlteringPropertyChanged += shadowMapDelegate
        : e => e.ShadowMapAlteringPropertyChanged -= shadowMapDelegate;

        ApplyUpdater(updater, sceneObject);
    }

    /*protected override void ApplyUpdater(Action<Entity> updater)
    {
        foreach (Triangle triangle in TriangleBuffer)
        {
            updater(triangle);
        }

        sceneObject.ForEach(s => updater(s));
    }*/

    public async override Task<T> RenderAsync(CancellationToken token = default)
    {

    }



    #endregion
}