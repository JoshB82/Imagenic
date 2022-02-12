using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Renderers.Rasterising;
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

        // Reset the vertices to model space values
        triangle.ResetVertices();

        // Move the face from model space to view space
        triangle.ApplyMatrix(modelToView);

        // Back-face culling if the mesh is three-dimensional
        if (meshDimension == 3)
        {
            // Discard the face if it is not visible from the rendering object's point of view
            if ((Vector3D)triangle.P1 * Vector3D.NormalFromPlane((Vector3D)triangle.P1, (Vector3D)triangle.P2, (Vector3D)triangle.P3) >= 0)
            {
                return;
            }
        }

        // Clip the face in view space
        Queue<Triangle> triangleClip = new(); triangleClip.Enqueue(triangle);
        if (!Clipping.ClipTriangles(triangleClip, ViewClippingPlanes))
        {
            return;
        }

        // Move the new triangles from view space to screen space, including a correction for perspective
        foreach (Triangle clippedFace in triangleClip)
        {
            // Move the face from view space to screen space
            clippedFace.ApplyMatrix(ViewToScreen);

            if (this is PerspectiveCamera or Spotlight)
            {
                clippedFace.P1 /= clippedFace.P1.w;
                clippedFace.P2 /= clippedFace.P2.w;
                clippedFace.P3 /= clippedFace.P3.w;

                if (this is PerspectiveCamera && clippedFace is TextureTriangle clippedTextureFace)
                {
                    clippedTextureFace.T1 /= clippedFace.P1.w;
                    clippedTextureFace.T2 /= clippedFace.P2.w;
                    clippedTextureFace.T3 /= clippedFace.P3.w;
                }
            }
        }

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