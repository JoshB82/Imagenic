/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for generating data required to draw faces.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Groups;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.Meshes.Components;
using _3D_Engine.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.SceneObjects.RenderingObjects.Lights;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine.SceneObjects.RenderingObjects.Cameras
{
    public abstract partial class Camera : RenderingObject
    {
        protected const byte outOfBoundsValue = 2;

        private void GenerateZBuffer(Group scene)
        {
            zBuffer.SetAllToValue(outOfBoundsValue);
            colourBuffer.SetAllToValue(RenderBackgroundColour);

            foreach (Camera camera in scene.Cameras)
            {
                if (camera.DrawIcon)
                {
                    Matrix4x4 modelToView = this.WorldToView * camera.Icon.ModelToWorld;

                    foreach (Face face in camera.Icon.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref modelToView,
                            ref this.ViewToScreen,
                            ref ScreenToWindow
                        );
                    }
                }
            }

            foreach (Light light in scene.Lights)
            {
                if (light.DrawIcon)
                {
                    Matrix4x4 modelToView = this.WorldToView * light.Icon.ModelToWorld;

                    foreach (Face face in light.Icon.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref modelToView,
                            ref this.ViewToScreen,
                            ref ScreenToWindow
                        );
                    }
                }
            }

            foreach (Mesh mesh in scene.Meshes)
            {
                if (mesh.Visible && mesh.DrawFaces)
                {
                    Matrix4x4 modelToView = this.WorldToView * mesh.ModelToWorld;

                    foreach (Face face in mesh.Faces)
                    {
                        if (face.Visible)
                        {
                            AddFaceToZBuffer
                            (
                                face,
                                mesh.Dimension,
                                ref modelToView,
                                ref this.ViewToScreen,
                                ref ScreenToWindow
                            );
                        }
                    }
                }
            }

            foreach (SceneObject sceneObject in scene.SceneObjects)
            {
                if (sceneObject.DisplayDirectionArrows)
                {
                    Arrow directionForward = sceneObject.DirectionArrows.SceneObjects[0] as Arrow;
                    Arrow directionUp = sceneObject.DirectionArrows.SceneObjects[1] as Arrow;
                    Arrow directionRight = sceneObject.DirectionArrows.SceneObjects[2] as Arrow;

                    Matrix4x4 directionForwardModelToView = this.WorldToView * directionForward.ModelToWorld;
                    Matrix4x4 directionUpModelToView = this.WorldToView * directionUp.ModelToWorld;
                    Matrix4x4 directionRightModelToView = this.WorldToView * directionRight.ModelToWorld;

                    foreach (Face face in directionForward.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionForwardModelToView,
                            ref this.ViewToScreen,
                            ref ScreenToWindow
                        );
                    }
                    foreach (Face face in directionUp.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionUpModelToView,
                            ref this.ViewToScreen,
                            ref ScreenToWindow
                        );
                    }
                    foreach (Face face in directionRight.Faces)
                    {
                        AddFaceToZBuffer
                        (
                            face,
                            3,
                            ref directionRightModelToView,
                            ref this.ViewToScreen,
                            ref ScreenToWindow
                        );
                    }
                }
            }
        }

        private void AddFaceToZBuffer(
            Face face,
            int meshDimension,
            ref Matrix4x4 modelToView,
            ref Matrix4x4 viewToScreen,
            ref Matrix4x4 screenToWindow)
        {
            // Reset the vertices to model space values
            face.ResetVertices();

            // Draw outline if needed ??
            if (face.DrawOutline)
            {
                DrawEdge(face.p1, face.p2, Color.Black, ref modelToView, ref viewToScreen);
                DrawEdge(face.p1, face.p3, Color.Black, ref modelToView, ref viewToScreen);
                DrawEdge(face.p2, face.p3, Color.Black, ref modelToView, ref viewToScreen);
            }

            // Move the face from model space to view space
            face.ApplyMatrix(modelToView);

            if (meshDimension == 3)
            {
                // Discard the face if it is not visible from the camera's point of view
                if ((Vector3D)face.p1 * Vector3D.NormalFromPlane((Vector3D)face.p1, (Vector3D)face.p2, (Vector3D)face.p3) >= 0)
                { return; }
            }

            // Clip the face in view space
            Queue<Face> faceClip = new();
            faceClip.Enqueue(face);
            if (!Clipping.ClipFaces(faceClip, this.ViewClippingPlanes)) { return; }

            // Move the new triangles from view space to screen space, including a correction for perspective
            foreach (Face clippedFace in faceClip)
            {
                clippedFace.ApplyMatrix(viewToScreen);

                if (this is PerspectiveCamera)
                {
                    clippedFace.p1 /= clippedFace.p1.w;
                    clippedFace.p2 /= clippedFace.p2.w;
                    clippedFace.p3 /= clippedFace.p3.w;
                    
                    if (face.HasTexture)
                    {
                        clippedFace.t1 /= clippedFace.p1.w;
                        clippedFace.t2 /= clippedFace.p2.w;
                        clippedFace.t3 /= clippedFace.p3.w;
                    }
                }
            }

            // Clip the face in screen space
            if (Settings.Screen_Space_Clip && !Clipping.ClipFaces(faceClip, Camera.CameraScreenClippingPlanes)) { return; } // anything outside cube?

            foreach (Face clippedFace in faceClip)
            {
                // Skip the face if it is flat
                if ((clippedFace.p1.x == clippedFace.p2.x && clippedFace.p2.x == clippedFace.p3.x) ||
                    (clippedFace.p1.y == clippedFace.p2.y && clippedFace.p2.y == clippedFace.p3.y))
                { continue; }

                // Move the new triangles from screen space to window space
                clippedFace.ApplyMatrix(screenToWindow);

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
                
                // Check if the face has a texture
                if (face.HasTexture)
                {
                    // Scale the texture co-ordinates
                    int textureWidth = face.Texture_Object.File.Width - 1;
                    int textureHeight = face.Texture_Object.File.Height - 1;

                    // afterwards?
                    float tx1 = face.T1.x * textureWidth;
                    float ty1 = face.T1.y * textureHeight;
                    float tz1 = face.T1.z;
                    float tx2 = face.T2.x * textureWidth;
                    float ty2 = face.T2.y * textureHeight;
                    float tz2 = face.T2.z;
                    float tx3 = face.T3.x * textureWidth;
                    float ty3 = face.T3.y * textureHeight;
                    float tz3 = face.T3.z;

                    // Sort the vertices by their y-co-ordinate
                    NumericManipulation.TexturedSortByY
                    (
                        ref x1, ref y1, ref tx1, ref ty1, ref tz1,
                        ref x2, ref y2, ref tx2, ref ty2, ref tz2,
                        ref x3, ref y3, ref tx3, ref ty3, ref tz3
                    );

                    // Generate z-buffer
                    Textured_Triangle
                    (
                        face.Texture_Object.File,
                        x1, y1, tx1, ty1, tz1,
                        x2, y2, tx2, ty2, tz2,
                        x3, y3, tx3, ty3, tz3
                    );
                }
                else
                {
                    // Sort the vertices by their y-co-ordinate
                    NumericManipulation.SortByY
                    (
                        ref x1, ref y1, ref z1,
                        ref x2, ref y2, ref z2,
                        ref x3, ref y3, ref z3
                    );

                    // Generate z-buffer
                    Interpolation.InterpolateTriangle
                    (
                        ZBufferCheck,
                        face.Colour,
                        x1, y1, z1,
                        x2, y2, z2,
                        x3, y3, z3
                    );
                }
            }
        }

        // Check if point is visible from the camera
        private void ZBufferCheck(object colour, int x, int y, float z)
        {
            try
            {
                if (z.ApproxLessThan(zBuffer.Values[x][y], 1E-4f))
                {
                    zBuffer.Values[x][y] = z;
                    colourBuffer.Values[x][y] = (Color)colour;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException($"Attempted to render outside the canvas at ({x}, {y}, {z})", e);
            }
        }
    }
}