using Imagenic.Core.Entities;
using Imagenic.Core.Enums;
using Imagenic.Core.Renderers.Clippers;
using Imagenic.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Renderers.Rasterising;

public partial class Rasteriser<TImage>
{
    #if DEBUG

    private static MessageBuilder<DecomposeStartMessage> mbDecomposeStart = new MessageBuilder<DecomposeStartMessage>();
    private static MessageBuilder<DecomposeFinishMessage> mbDecomposeFinish = new MessageBuilder<DecomposeFinishMessage>();

    #endif

    internal bool Decompose(PhysicalEntity physicalEntity,
                            RenderingEntity renderingEntity,
                            List<RenderTriangle>? renderTriangles = null)
    {
        #if DEBUG

        mbDecomposeStart.UpdateParameter(1, physicalEntity.Id);

        #endif

        renderTriangles ??= new();
        Matrix4x4 modelToView = renderingEntity.WorldToView * physicalEntity.ModelToWorld;

        switch (physicalEntity)
        {
            case Mesh mesh:
                return DecomposeMesh(mesh, renderingEntity, ref modelToView, renderTriangles);
            //case Face face:
                //return await DecomposeFace(face, renderingEntity, token);
        }

        #if DEBUG

        mbDecomposeFinish.Build().DisplayInConsole();

        #endif

        return renderTriangles.Any();
    }

    private bool DecomposeMesh(Mesh mesh,
                               RenderingEntity renderingEntity,
                               ref Matrix4x4 modelToView,
                               List<RenderTriangle>? renderTriangles = null)
    {
        renderTriangles ??= new();

        if (mesh.Structure.Faces is not null)
        {
            foreach (Face face in mesh.Structure.Faces)
            {
                // Back-face culling if the mesh is three-dimensional
                if (mesh.Dimension == Dimension.Three)
                {
                    // Skip the face if it is not visible from the rendering object's point of view
                    Triangle triangle = face.Triangles[0];
                    if (triangle.P1.WorldOrigin * Vector3D.NormalFromPlane(triangle.P1.WorldOrigin, triangle.P2.WorldOrigin, triangle.P3.WorldOrigin) >= 0)
                    {
                        continue;
                    }
                }

                DecomposeFace(face, renderingEntity, ref modelToView, renderTriangles);
            }
        }

        return renderTriangles.Any();
    }

    private bool DecomposeFace(Face face,
                               RenderingEntity renderingEntity,
                               ref Matrix4x4 modelToView,
                               List<RenderTriangle>? renderTriangles = null)
    {
        renderTriangles ??= new();

        foreach (Triangle triangle in face.Triangles)
        {
            TransformTriangle(triangle, renderingEntity, ref modelToView, renderTriangles);
        }

        return renderTriangles.Any();
    }

    private bool TransformTriangle(Triangle triangle,
                                   RenderingEntity renderingEntity,
                                   ref Matrix4x4 modelToView,
                                   List<RenderTriangle>? renderTriangles = null)
    {
        renderTriangles ??= new();

        // Create copy of triangle to transform
        RenderTriangle startingRenderTriangle = triangle.FrontStyle switch
        {
            SolidStyle solidStyle => new SolidRenderTriangle(triangle.P1.WorldOrigin, triangle.P2.WorldOrigin, triangle.P3.WorldOrigin, solidStyle),
            TextureStyle textureStyle => new TextureRenderTriangle(triangle.P1.WorldOrigin, triangle.P2.WorldOrigin, triangle.P3.WorldOrigin, textureStyle.T1, textureStyle.T2, textureStyle.T3, textureStyle),
            _ => throw new System.Exception("Unsupported triangle style.")
        };

        // Move the face from model space to view space
        startingRenderTriangle.ApplyMatrix(modelToView);

        // Clip the face in view space
        var triangleClipper = new TriangleClipper(startingRenderTriangle, renderingEntity.ViewClippingPlanes);
        if (triangleClipper.Clip().Count == 0)
        {
            return false;
        }

        // Move the new triangles from view space to screen space, including a correction for perspective
        foreach (RenderTriangle clippedTriangle in triangleClipper.TriangleQueue)
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
                    if (clippedTriangle is TextureRenderTriangle textureTriangle)
                    {
                        textureTriangle.T1 /= w1;
                        textureTriangle.T2 /= w2;
                        textureTriangle.T3 /= w3;
                    }
                }
            }
        }

        // Clip the face in screen space
        triangleClipper.ClippingPlanes = ScreenClippingPlanes;
        if (triangleClipper.Clip().Count == 0)
        {
            return false;
        } // anything outside cube?

        foreach (RenderTriangle clippedTriangle in triangleClipper.TriangleQueue)
        {
            // Skip the face if it is flat
            if ((clippedTriangle.P1.x == clippedTriangle.P2.x && clippedTriangle.P2.x == clippedTriangle.P3.x) ||
                (clippedTriangle.P1.y == clippedTriangle.P2.y && clippedTriangle.P2.y == clippedTriangle.P3.y))
            {
                continue;
            }

            // Move the face from screen space to window space
            clippedTriangle.ApplyMatrix(ScreenToWindow);

            // Call the required interpolator method
            renderTriangles.Add(clippedTriangle);
        }

        return renderTriangles.Any();
    }
}