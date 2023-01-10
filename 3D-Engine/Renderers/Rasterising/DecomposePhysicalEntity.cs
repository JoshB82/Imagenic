using Imagenic.Core.Entities;
using Imagenic.Core.Enums;
using Imagenic.Core.Renderers.Clippers;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.Rasterising;

internal class DecomposePhysicalEntity
{
    #if DEBUG

    private static MessageBuilder<DecomposeStartMessage> mbDecomposeStart = new MessageBuilder<DecomposeStartMessage>();
    private static MessageBuilder<DecomposeFinishMessage> mbDecomposeFinish = new MessageBuilder<DecomposeFinishMessage>();

    #endif

    internal async static Task Decompose(PhysicalEntity physicalEntity, RenderingEntity renderingEntity, CancellationToken token)
    {
        #if DEBUG

        mbDecomposeStart.UpdateParameter(1, physicalEntity.Id);

        #endif

        switch (physicalEntity)
        {
            case Mesh mesh:
                await DecomposeMesh(mesh, renderingEntity, token);
                break;
            case Face face:
                await DecomposeFace(face, renderingEntity, token);
                break;
        }

        #if DEBUG

        mbDecomposeFinish.Build().DisplayInConsole();

        #endif
    }

    private async static Task DecomposeMesh(Mesh mesh, RenderingEntity renderingEntity, CancellationToken token)
    {
        foreach (Face face in mesh.Structure.Faces)
        {
            await DecomposeFace(face, renderingEntity, token);
        }
    }

    private async static Task DecomposeFace(Face face, RenderingEntity renderingEntity, CancellationToken token)
    {
        foreach (Triangle triangle in face.Triangles)
        {
            await TransformTriangle(triangle, renderEntity, ref modelToView);
        }
    }

    private async static Task TransformTriangle(Triangle triangle, RenderingEntity renderingEntity, Dimension meshDimension, ref Matrix4x4 modelToView)
    {
        //var vertices4D = (triangle.P1, triangle.P2, triangle.P3);
        //var triangle4D = new VectorTriple<Vector4D>(triangle.P1, triangle.P2, triangle.P3);
        //var calcTriangle = triangle.DeepCopy();

        // Reset the vertices to model space values
        //triangle.ResetVertices();
        triangle.CalcP1 = new Vector4D(triangle.P1.WorldOrigin, 1);
        triangle.CalcP2 = new Vector4D(triangle.P2.WorldOrigin, 1);
        triangle.CalcP3 = new Vector4D(triangle.P3.WorldOrigin, 1);

        // Move the face from model space to view space
        //triangle.ApplyMatrix(modelToView);
        //vertices4D = Triangle.ApplyMatrix(modelToView, vertices4D);
        triangle.ApplyMatrix(modelToView);

        // Back-face culling if the mesh is three-dimensional
        if (meshDimension == Dimension.Three)
        {
            // Discard the face if it is not visible from the rendering object's point of view
            //var vertices3D = (p1: (Vector3D)triangle.P1, p2: (Vector3D)triangle.P2, p3: (Vector3D)triangle.P3);
            var vertices3D = new VectorTriple<Vector3D>((Vector3D)triangle.P1, (Vector3D)triangle.P2, (Vector3D)triangle.P3);
            if (vertices3D.p1 * Vector3D.NormalFromPlane(vertices3D.p1, vertices3D.p2, vertices3D.p3) >= 0)
            {
                return;
            }
        }

        // Clip the face in view space
        //Queue<Triangle> triangleClip = new();
        var triangleClipQueue = new Queue<Triangle>();
        triangleClipQueue.Enqueue(triangle);
        if (!Clipping.ClipTriangles(triangleClipQueue, ViewClippingPlanes))
        {
            return;
        }

        if (new TriangleClipper(triangle, ViewClippingPlanes).Clip().Count == 0)
        {
            return;
        }

        // Move the new triangles from view space to screen space, including a correction for perspective
        foreach (Triangle clippedTriangle in triangleClipQueue)
        {
            // Move the face from view space to screen space
            //clippedTriangle.ApplyMatrix(ViewToScreen);
            //vertices4D = Triangle.ApplyMatrix(ViewToScreen, vertices4D);
            clippedTriangle.ApplyMatrix(ViewToScreen);

            if (renderingEntity is PerspectiveCamera or Spotlight)
            {
                //var clippedVertices4D = (p1: clippedTriangle.P1, p2: clippedTriangle.P2, p3: clippedTriangle.P3);
                //var clippedVertices4D = new VectorTriple<Vector4D>(clippedTriangle.P1, clippedTriangle.P2, clippedTriangle.P3);

                float w1 = clippedTriangle.CalcP1.w;
                float w2 = clippedTriangle.CalcP2.w;
                float w3 = clippedTriangle.CalcP3.w;

                clippedTriangle.CalcP1 /= w1;
                clippedTriangle.CalcP2 /= w2;
                clippedTriangle.CalcP3 /= w3;

                if (renderingEntity is PerspectiveCamera)
                {
                    if (clippedTriangle.FrontStyle is TextureStyle textureFrontStyle)
                    {
                        textureFrontStyle.T1 /= w1;
                        textureFrontStyle.T2 /= w2;
                        textureFrontStyle.T3 /= w3;
                    }

                    if (clippedTriangle.BackStyle is TextureStyle textureBackStyle)
                    {
                        textureBackStyle.T1 /= w1;
                        textureBackStyle.T2 /= w2;
                        textureBackStyle.T3 /= w3;
                    }

                    //var clippedTextureVertices = (p1: clippedTextureFace.T1, p2: clippedTextureFace.T2, p3: clippedTextureFace.T3);
                    //var clippedTextureVertices = new VectorTriple<Vector3D>(clippedTextureFace.T1, clippedTextureFace.T2, clippedTextureFace.T3);
                    // ??
                    
                }
            }
        }

        // Clip the face in screen space
        if (new TriangleClipper(triangleClipQueue, ScreenClippingPlanes).Clip().Count == 0)
        {
            return;
        } // anything outside cube?

        foreach (Triangle clippedTriangle in triangleClipQueue)
        {
            // Skip the face if it is flat
            if ((clippedTriangle.P1.WorldOrigin.x == clippedTriangle.P2.WorldOrigin.x && clippedTriangle.P2.WorldOrigin.x == clippedTriangle.P3.WorldOrigin.x) ||
                (clippedTriangle.P1.WorldOrigin.y == clippedTriangle.P2.WorldOrigin.y && clippedTriangle.P2.WorldOrigin.y == clippedTriangle.P3.WorldOrigin.y))
            {
                continue;
            }

            // Move the face from screen space to window space
            clippedTriangle.ApplyMatrix(ScreenToWindow);
            

            // Call the required interpolator method
            clippedTriangle.Interpolator(this, bufferAction);
        }
    }
}