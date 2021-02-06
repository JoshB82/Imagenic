using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Cameras;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using System.Collections.Generic;

namespace _3D_Engine.SceneObjects.Lights
{
    public abstract partial class Light : SceneObject
    {
        public void GenerateShadowMap(Group group)
        {
            foreach (Camera camera in group.Cameras)
            {
                if (camera.DrawIcon)
                {
                    Matrix4x4 modelToLightView = WorldToLightView * camera.Icon.ModelToWorld;

                    foreach (Face face in camera.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                camera.Icon.Dimension,
                                ref modelToLightView
                            );
                        }
                    }
                }
            }
            
            foreach (Light light in group.Lights)
            {
                if (light.DrawIcon)
                {
                    Matrix4x4 modelToLightView = WorldToLightView * light.Icon.ModelToWorld;

                    foreach (Face face in light.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                light.Icon.Dimension,
                                ref modelToLightView
                            );
                        }
                    }
                }
            }
            
            foreach (Mesh mesh in group.Meshes)
            {
                if (mesh.Visible && mesh.DrawFaces)
                {
                    Matrix4x4 modelToLightView = WorldToLightView * mesh.ModelToWorld;

                    foreach (Face face in mesh.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                mesh.Dimension,
                                ref modelToLightView
                            );
                        }
                    }
                }
            }
        }

        private void DirectionArrowsShadowMap()
        {

        }

        private void CalculateDepth(
            Face face,
            int meshDimension,
            ref Matrix4x4 modelToLightView)
        {
            // Reset the vertices to model space values
            face.ResetVertices();

            // Move the face from model space to light view space
            face.ApplyMatrix(modelToLightView);

            if (meshDimension == 3)
            {
                // Discard the face if it is not visible from the light's point of view
                if ((Vector3D)face.p1 * Vector3D.NormalFromPlane((Vector3D)face.p1, (Vector3D)face.p2, (Vector3D)face.p3) >= 0)
                { return; }
            }

            // Clip the face in light-view space
            Queue<Face> faceClip = new();
            faceClip.Enqueue(face);
            if (!Clipping.ClipFaces(faceClip, LightViewClippingPlanes)) { return; }

            // Move the new triangles from light-view space to screen space, including a correction for perspective
            foreach (Face clippedFace in faceClip)
            {
                clippedFace.ApplyMatrix(LightViewToLightScreen);

                if (this is PointLight or Spotlight)
                {
                    clippedFace.p1 /= clippedFace.p1.w;
                    clippedFace.p2 /= clippedFace.p2.w;
                    clippedFace.p3 /= clippedFace.p3.w;
                }
            }

            // Clip the face in screen space
            if (Settings.Screen_Space_Clip && !Clipping.ClipFaces(faceClip, Camera.CameraScreenClippingPlanes)) { return; }

            foreach (Face clippedFace in faceClip)
            {
                // Don't draw anything if the face is flat
                if ((clippedFace.p1.x == clippedFace.p2.x && clippedFace.p2.x == clippedFace.p3.x) ||
                    (clippedFace.p1.y == clippedFace.p2.y && clippedFace.p2.y == clippedFace.p3.y))
                { continue; }

                // Move the new triangles from light-screen space to light-window space
                clippedFace.ApplyMatrix(LightScreenToLightWindow);
                
                // Round the vertices
                int x1 = clippedFace.p1.x.RoundToInt();
                int y1 = clippedFace.p1.y.RoundToInt();
                float z1 = clippedFace.p1.z;
                int x2 = clippedFace.p2.x.RoundToInt();
                int y2 = clippedFace.p2.y.RoundToInt();
                float z2 = clippedFace.p2.z;
                int x3 = clippedFace.p3.x.RoundToInt();
                int y3 = clippedFace.p3.y.RoundToInt();
                float z3 = clippedFace.p3.z;

                // Sort the vertices by their y-co-ordinate
                NumericManipulation.SortByY
                (
                    ref x1, ref y1, ref z1,
                    ref x2, ref y2, ref z2,
                    ref x3, ref y3, ref z3
                );

                // Interpolate each point in the triangle
                Interpolation.InterpolateTriangle
                (
                    MeshDepthFromLight,
                    null,
                    x1, y1, z1,
                    x2, y2, z2,
                    x3, y3, z3
                );
            }
        }

        private void MeshDepthFromLight(object @object, int x, int y, float z)
        {
            if (x < ShadowMapWidth && y < ShadowMapHeight)
            {
                if (z.ApproxLessThan(ShadowMap.Values[x][y], 1E-4f))
                {
                    ShadowMap.Values[x][y] = z;
                }
            }
        }
    }
}