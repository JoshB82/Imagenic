using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using Imagenic.Core.Entities;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Triangles;
using Imagenic.Core.Entities.SceneObjects.RenderingObjects.Lights;
using Imagenic.Core.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Imagenic.Core.Renderers.Rasterising;

internal class DecomposePhysicalEntity
{
    #if DEBUG

    private static MessageBuilder<> mbDecomposeStart = new MessageBuilder<>();
    private static MessageBuilder<> mbDecomposeFinish = new MessageBuilder<>();

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

        mbDecomposeFinish.Build().

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
        var vertices4D = (triangle.P1, triangle.P2, triangle.P3);

        // Reset the vertices to model space values
        //triangle.ResetVertices();

        // Move the face from model space to view space
        //triangle.ApplyMatrix(modelToView);
        vertices4D = Triangle.ApplyMatrix(modelToView, vertices4D);

        // Back-face culling if the mesh is three-dimensional
        if (meshDimension == Dimension.Three)
        {
            // Discard the face if it is not visible from the rendering object's point of view
            var vertices3D = (p1: (Vector3D)triangle.P1, p2: (Vector3D)triangle.P2, p3: (Vector3D)triangle.P3);
            if (vertices3D.p1 * Vector3D.NormalFromPlane(vertices3D.p1, vertices3D.p2, vertices3D.p3) >= 0)
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
        foreach (Triangle clippedTriangle in triangleClip)
        {
            // Move the face from view space to screen space
            //clippedTriangle.ApplyMatrix(ViewToScreen);
            vertices4D = Triangle.ApplyMatrix(ViewToScreen, vertices4D);

            if (renderingEntity is PerspectiveCamera or Spotlight)
            {
                var clippedVertices4D = (p1: clippedTriangle.P1, p2: clippedTriangle.P2, p3: clippedTriangle.P3);
                clippedVertices4D.p1 /= clippedVertices4D.p1.w;
                clippedVertices4D.p2 /= clippedVertices4D.p2.w;
                clippedVertices4D.p3 /= clippedVertices4D.p3.w;

                if (renderingEntity is PerspectiveCamera && clippedTriangle is TextureTriangle clippedTextureFace)
                {
                    var clippedTextureVertices = (p1: clippedTextureFace.T1, p2: clippedTextureFace.T2, p3: clippedTextureFace.T3);
                    // ??
                    clippedTextureVertices.p1 /= clippedTriangle.P1.w;
                    clippedTextureVertices.p2 /= clippedTriangle.P2.w;
                    clippedTextureVertices.p3 /= clippedTriangle.P3.w;
                }
            }
        }
    }
}