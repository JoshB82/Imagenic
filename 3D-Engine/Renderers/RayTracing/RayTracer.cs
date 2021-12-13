using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using _3D_Engine.Images;
using _3D_Engine.Images.ImageOptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers.RayTracing
{
    public partial class RayTracer<T> : Renderer<T> where T : Image
    {
        #region Fields and Properties

        internal bool NewShadowMapNeeded { get; set; }



        #endregion

        #region Methods

        public async override Task<T> RenderAsync(CancellationToken token)
        {
            return await GenerateImage();
        }

        public async override Task<T> Render<T>(IImageOptions<T> imageOptions, RenderingOptions options, CancellationToken token)
        {
            
        }

        private async static Task<T> GenerateImage()
        {
            Buffer2D<Color> colourBuffer = await CastRays();
            if (typeof(T) == typeof(Bitmap))
            {
                return new Bitmap(colourBuffer);
            }
        }

        #endregion
    }
}