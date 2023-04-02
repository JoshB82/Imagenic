using Imagenic.Core.Entities;
using Imagenic.Core.Enums;
using Imagenic.Core.Images;
using Imagenic.Core.Renderers.Clippers;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.Rasterising;

public partial class Rasteriser<TImage>
{
    #if DEBUG

    private static MessageBuilder<DecomposeStartMessage> mbDecomposeStart = new MessageBuilder<DecomposeStartMessage>();
    private static MessageBuilder<DecomposeFinishMessage> mbDecomposeFinish = new MessageBuilder<DecomposeFinishMessage>();

    #endif

    internal async Task<bool> Decompose(PhysicalEntity physicalEntity, RenderingEntity renderingEntity, CancellationToken token)
    {
        #if DEBUG

        mbDecomposeStart.UpdateParameter(1, physicalEntity.Id);

        #endif

        switch (physicalEntity)
        {
            case Mesh mesh:
                return await DecomposeMesh(mesh, renderingEntity, token);
            case Face face:
                return await DecomposeFace(face, renderingEntity, token);
        }

        #if DEBUG

        mbDecomposeFinish.Build().DisplayInConsole();

        #endif
    }

    private async static Task<bool> DecomposeMesh(Mesh mesh, RenderingEntity renderingEntity, CancellationToken token)
    {
        foreach (Face face in mesh.Structure.Faces)
        {
            await DecomposeFace(face, renderingEntity, token);
        }
    }

    private async static Task<bool> DecomposeFace(Face face, RenderingEntity renderingEntity, CancellationToken token)
    {
        foreach (Triangle triangle in face.Triangles)
        {
            await TransformTriangle(triangle, renderEntity, ref modelToView);
        }
    }

    private bool TransformTriangle(Triangle triangle,
                                   RenderingEntity renderingEntity,
                                   Dimension meshDimension,
                                   ref Matrix4x4 modelToView,
                                   out List<DrawableTriangle> decomposition)
    {
        decomposition = new List<DrawableTriangle>()
        {
            new DrawableTriangle(new Vector4D(triangle.P1.WorldOrigin, 1),
                          new Vector4D(triangle.P2.WorldOrigin, 1),
                          new Vector4D(triangle.P3.WorldOrigin, 1), null)
        };




        // Reset the vertices to model space values



        // Move the face from model space to view space
        decomposition[0].ApplyMatrix(modelToView);

        // Back-face culling if the mesh is three-dimensional
        if (meshDimension == Dimension.Three)
        {
            // Discard the face if it is not visible from the rendering object's point of view
            if (triangle.P1.WorldOrigin * Vector3D.NormalFromPlane(triangle.P1.WorldOrigin, triangle.P2.WorldOrigin, triangle.P3.WorldOrigin) >= 0)
            {
                return false;
            }
        }

        

        // Clip the face in view space
        var triangleClipper = new TriangleClipper(triangle, renderingEntity.ViewClippingPlanes);
        if (triangleClipper.Clip().Count == 0)
        {
            return false;
        }

        // Move the new triangles from view space to screen space, including a correction for perspective
        foreach (DrawableTriangle clippedTriangle in triangleClipper.TriangleQueue)
        {
            // Move the face from view space to screen space
            clippedTriangle.ApplyMatrix(renderingEntity.ViewToScreen);

            if (renderingEntity is PerspectiveCamera or Spotlight)
            {
                float w1 = clippedTriangle.P1.w;
                float w2 = clippedTriangle.P2.w;
                float w3 = clippedTriangle.P3.w;

                clippedTriangle.P1 /= w1;
                clippedTriangle.P2 /= w2;
                clippedTriangle.P3 /= w3;

                if (renderingEntity is PerspectiveCamera)
                {
                    if (clippedTriangle.faceStyleToBeDrawn is TextureStyle textureFrontStyle)
                    {
                        textureFrontStyle.T1 /= w1;
                        textureFrontStyle.T2 /= w2;
                        textureFrontStyle.T3 /= w3;
                    }

                    /*
                    if (clippedTriangle.BackStyle is TextureStyle textureBackStyle)
                    {
                        textureBackStyle.T1 /= w1;
                        textureBackStyle.T2 /= w2;
                        textureBackStyle.T3 /= w3;
                    }*/
                }
            }
        }

        // Clip the face in screen space
        triangleClipper.ClippingPlanes = ScreenClippingPlanes;
        if (triangleClipper.Clip().Count == 0)
        {
            return false;
        } // anything outside cube?

        foreach (DrawableTriangle clippedTriangle in triangleClipper.TriangleQueue)
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
            //clippedTriangle.Interpolator(this, bufferAction);
            //var drawableTriangle = new DrawableTriangle(clippedTriangle.CalcP1.x, clippedTriangle.);
            decomposition.Add(drawableTriangle);
        }

        return true;
    }
}