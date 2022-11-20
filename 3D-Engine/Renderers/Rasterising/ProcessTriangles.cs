using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.SceneObjects.RenderingObjects.Lights;
using Imagenic.Core.Renderers.Rasterising;
using Imagenic.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Engine.Renderers.Rasterising;
internal class ProcessTriangles
{
    private async Task ProcessTriangles()
    {
        #if DEBUG

        new MessageBuilder<GeneratingDepthValuesMessage>()
            .AddType(this.GetType())
            .Build()
            .DisplayInConsole();

        #endif

        foreach (Triangle triangle in TriangleBuffer)
        {
            AddTriangleToBuffer(triangle, );
        }

        #if DEBUG

        new MessageBuilder<GeneratedDepthValuesMessage>()
            .AddType(this.GetType())
            .Build()
            .DisplayInConsole();

        #endif
    }

    private void AddTriangleToBuffer(Triangle triangle, int meshDimension, ref Matrix4x4 modelToView)
    {
        Action<object, int, int, float> bufferAction = (this is Light || triangle is SolidTriangle) ? AddPointToBuffers : null;

        

        

        

        // Clip the face in screen space
        if (!Clipping.ClipTriangles(triangleClip, ScreenClippingPlanes))
        {
            return;
        } // anything outside cube?

        foreach (Triangle clippedFace in triangleClip)
        {
            // Skip the face if it is flat
            if ((clippedFace.P1.x == clippedFace.P2.x && clippedFace.P2.x == clippedFace.P3.x) ||
                (clippedFace.P1.y == clippedFace.P2.y && clippedFace.P2.y == clippedFace.P3.y))
            {
                continue;
            }

            // Move the face from screen space to window space
            clippedFace.ApplyMatrix(ScreenToWindow);

            // Call the required interpolator method
            clippedFace.Interpolator(this, bufferAction);
        }
    }
}