using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers
{
    public class Rasteriser : Renderer
    {
        #region Fields and Properties
        
        internal bool NewShadowMapNeeded { get; set; }

        #endregion

        #region Methods

        public async override Task<Bitmap> Render(RenderingOptions options)
        {

        }

        #endregion
    }
}