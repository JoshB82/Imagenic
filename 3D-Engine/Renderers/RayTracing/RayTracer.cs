using _3D_Engine.Images;
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

        public async override Task<Image> Render(RenderingOptions options)
        {
            await CastRays();
        }

        #endregion
    }
}