/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a shadow map.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Miscellaneous;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using System.Collections.Generic;

namespace _3D_Engine.SceneObjects.RenderingObjects.Lights
{
    public abstract partial class Light : RenderingObject
    {
        private const byte outOfBoundsValue = 2;

        public void GenerateShadowMap(Group scene)
        {
            ShadowMap.SetAllToValue(outOfBoundsValue);

            foreach (Camera camera in scene.Cameras)
            {
                if (camera.DrawIcon)
                {
                    Matrix4x4 modelToView = WorldToView * camera.Icon.ModelToWorld;

                    foreach (Face face in camera.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                camera.Icon.Dimension,
                                ref modelToView
                            );
                        }
                    }
                }
            }

            foreach (Light light in scene.Lights)
            {
                if (light.DrawIcon)
                {
                    Matrix4x4 modelToView = WorldToView * light.Icon.ModelToWorld;

                    foreach (Face face in light.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                light.Icon.Dimension,
                                ref modelToView
                            );
                        }
                    }
                }
            }

            foreach (Mesh mesh in scene.Meshes)
            {
                if (mesh.Visible && mesh.DrawFaces)
                {
                    Matrix4x4 modelToView = WorldToView * mesh.ModelToWorld;

                    foreach (Face face in mesh.Faces)
                    {
                        if (face.Visible)
                        {
                            CalculateDepth
                            (
                                face,
                                mesh.Dimension,
                                ref modelToView
                            );
                        }
                    }
                }
            }

            foreach (SceneObject sceneObject in scene.SceneObjects)
            {
                if (sceneObject.HasDirectionArrows && sceneObject.DisplayDirectionArrows)
                {
                    Arrow directionForward = sceneObject.DirectionArrows.SceneObjects[0] as Arrow;
                    Arrow directionUp = sceneObject.DirectionArrows.SceneObjects[1] as Arrow;
                    Arrow directionRight = sceneObject.DirectionArrows.SceneObjects[2] as Arrow;

                    Matrix4x4 directionForwardModelToView = WorldToView * directionForward.ModelToWorld;
                    Matrix4x4 directionUpModelToView = WorldToView * directionUp.ModelToWorld;
                    Matrix4x4 directionRightModelToView = WorldToView * directionRight.ModelToWorld;

                    foreach (Face face in directionForward.Faces)
                    {
                        CalculateDepth
                        (
                            face,
                            3,
                            ref directionForwardModelToView
                        );
                    }  
                    foreach (Face face in directionUp.Faces)
                    {
                        CalculateDepth
                        (
                            face,
                            3,
                            ref directionUpModelToView
                        );
                    }
                    foreach (Face face in directionRight.Faces)
                    {
                        CalculateDepth
                        (
                            face,
                            3,
                            ref directionRightModelToView
                        );
                    }
                }
            }

            ConsoleOutput.DisplayMessageFromObject(this, "Generated shadow map.");
        }

        private void CalculateDepth(
            Face face,
            int meshDimension,
            ref Matrix4x4 modelToView)
        {
            // Reset the vertices to model space values
            face.ResetVertices();

            // Move the face from model space to view space
            face.ApplyMatrix(modelToView);

            if (meshDimension == 3)
            {
                // Discard the face if it is not visible from the light's point of view
                if ((Vector3D)face.p1 * Vector3D.NormalFromPlane((Vector3D)face.p1, (Vector3D)face.p2, (Vector3D)face.p3) >= 0)
                { return; }
            }

            // Clip the face in view space
            Queue<Face> faceClip = new();
            faceClip.Enqueue(face);
            if (!Clipping.ClipFaces(faceClip, ViewClippingPlanes)) { return; }

            // Move the new triangles from view space to screen space, including a correction for perspective
            foreach (Face clippedFace in faceClip)
            {
                clippedFace.ApplyMatrix(ViewToScreen);

                if (this is PointLight or Spotlight)
                {
                    clippedFace.p1 /= clippedFace.p1.w;
                    clippedFace.p2 /= clippedFace.p2.w;
                    clippedFace.p3 /= clippedFace.p3.w;
                }
            }

            // Clip the face in screen space
            if (Settings.Screen_Space_Clip && !Clipping.ClipFaces(faceClip, ScreenClippingPlanes)) { return; }

            foreach (Face clippedFace in faceClip)
            {
                // Don't draw anything if the face is flat
                if ((clippedFace.p1.x == clippedFace.p2.x && clippedFace.p2.x == clippedFace.p3.x) ||
                    (clippedFace.p1.y == clippedFace.p2.y && clippedFace.p2.y == clippedFace.p3.y))
                { continue; }

                // Move the new triangles from screen space to window space
                clippedFace.ApplyMatrix(ScreenToWindow);
                
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
            if (x < RenderWidth && y < RenderHeight)
            {
                if (z.ApproxLessThan(ShadowMap.Values[x][y], 1E-4f))
                {
                    ShadowMap.Values[x][y] = z;
                }
            }
        }
    }
}