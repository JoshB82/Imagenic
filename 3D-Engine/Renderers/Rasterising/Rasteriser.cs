using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
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

        private Action shadowMapDelegate;

        internal bool NewShadowMapNeeded { get; set; }

        
        public override SceneObject SceneObjectsToRender
        {
            get => base.SceneObjectsToRender;
            set
            {
                foreach (Triangle triangle in TriangleBuffer)
                {
                    triangle.ShadowMapAlteringPropertyChanged -= shadowMapDelegate;
                }

                base.SceneObjectsToRender.ForEach(s =>
                {
                    s.ShadowMapAlteringPropertyChanged -= shadowMapDelegate;
                });

                base.SceneObjectsToRender = value;

                base.SceneObjectsToRender.ForEach(s =>
                {
                    s.ShadowMapAlteringPropertyChanged += shadowMapDelegate;
                });

                foreach (Triangle triangle in TriangleBuffer)
                {
                    triangle.ShadowMapAlteringPropertyChanged += shadowMapDelegate;
                }
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

        public async override Task<T> Render<T>(IImageOptions<T> imageOptions, RenderingOptions options, CancellationToken token)
        {

        }

        #endregion
    }
}