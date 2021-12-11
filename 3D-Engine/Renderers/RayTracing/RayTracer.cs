using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers.RayTracing
{
    public partial class RayTracer : Renderer
    {
        #region Fields and Properties

        internal bool NewShadowMapNeeded { get; set; }

        #endregion

        #region Methods

        public async override Task<T> Render<T>(IImageOptions<T> imageOptions, RenderingOptions options)
        {
            await CastRays();
        }

        #endregion
    }
}