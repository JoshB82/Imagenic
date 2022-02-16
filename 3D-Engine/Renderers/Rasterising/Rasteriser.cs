﻿using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities;
using Imagenic.Core.Images;
using Imagenic.Core.Images.ImageOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers
{
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

        public override SceneObject SceneObjectsToRender
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

        private void UpdateSubscribers(SceneObject sceneObject, bool addSubscription)
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

        public async override Task<T> Render<T>(IImageOptions<T> imageOptions, RenderingOptions options, CancellationToken token)
        {

        }



        #endregion
    }
}