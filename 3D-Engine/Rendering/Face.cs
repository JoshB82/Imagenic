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
using _3D_Engine.SceneObjects.Lights;
using _3D_Engine.SceneObjects.Meshes.Components;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Cameras
{
    public abstract partial class Camera : SceneObject
    {
        private void AddFaceToZBuffer(
            Face face,
            int meshDimension,
            ref Matrix4x4 modelToCameraView,
            ref Matrix4x4 cameraViewToCameraScreen,
            ref Matrix4x4 cameraScreenToWindow)
        {
            // Reset the vertices to model space values
            face.ResetVertices();

            // Draw outline if needed ??
            if (face.DrawOutline)
            {
                DrawEdge(face.p1, face.p2, Color.Black, ref modelToCameraView, ref cameraViewToCameraScreen);
                DrawEdge(face.p1, face.p3, Color.Black, ref modelToCameraView, ref cameraViewToCameraScreen);
                DrawEdge(face.p2, face.p3, Color.Black, ref modelToCameraView, ref cameraViewToCameraScreen);
            }

            // Move the face from model space to camera-view space
            face.ApplyMatrix(modelToCameraView);

            if (meshDimension == 3)
            {
                // Discard the face if it is not visible from the camera's point of view
                if ((Vector3D)face.p1 * Vector3D.NormalFromPlane((Vector3D)face.p1, (Vector3D)face.p2, (Vector3D)face.p3) >= 0)
                { return; }
            }

            // Clip the face in camera-view space
            Queue<Face> faceClipQueue = new();
            faceClipQueue.Enqueue(face);
            if (!Clipping.ClipFaces(faceClipQueue, this.CameraViewClippingPlanes)) { return; }

            // Move the new triangles from camera-view space to camera-screen space, including a correction for perspective
            foreach (var clippedFace in faceClipQueue)
            {
                clippedFace.ApplyMatrix(cameraViewToCameraScreen);

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

            // Clip the face in camera-screen space
            if (Settings.Screen_Space_Clip && !Clipping.ClipFaces(faceClipQueue, Camera.CameraScreenClippingPlanes)) { return; } // anything outside cube?

            foreach (Face clippedFace in faceClipQueue)
            {
                // Skip the face if it is flat
                if ((clippedFace.p1.x == clippedFace.p2.x && clippedFace.p2.x == clippedFace.p3.x) ||
                    (clippedFace.p1.y == clippedFace.p2.y && clippedFace.p2.y == clippedFace.p3.y))
                { continue; }

                // Move the new triangles from camera-screen space to camera-window space
                clippedFace.ApplyMatrix(cameraScreenToWindow);

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

        // Shadow Map Check (SMC)
        private void SMCCameraPerspective(
            int x, int y, float z,
            ref Color pointColour,
            ref Matrix4x4 windowToCameraScreen,
            ref Matrix4x4 cameraScreenToWorld)
        {
            // Move the point from window space to camera-screen space
            Vector4D cameraScreenSpacePoint = windowToCameraScreen * new Vector4D(x, y, z, 1);

            // Move the point from camera-screen space to world space
            cameraScreenSpacePoint *= 2 * this.ZNear * this.ZFar / (this.ZNear + this.ZFar - cameraScreenSpacePoint.z * (this.ZFar - this.ZNear));

            // Apply lighting
            ApplyLighting(cameraScreenToWorld * cameraScreenSpacePoint, ref pointColour, x, y);
        }

        // Lighting
        private void ApplyLighting(
            Vector4D worldSpacePoint,
            ref Color pointColour, int x, int y)
        {
            bool lightApplied = false;

            foreach (Light light in Scene.Lights)
            {
                if (light.Visible)
                {
                    // Move the point from world space to light-view space
                    Vector4D lightViewSpacePoint = light.WorldToLightView * worldSpacePoint;

                    Color newLightColour = light.Colour;
                    if (light is PointLight or Spotlight)
                    {
                        // Darken the Light's colour based on how far away the point is from the light
                        Vector3D light_to_point = (Vector3D)lightViewSpacePoint;
                        float distant_intensity = light.Strength / light_to_point.Squared_Magnitude();
                        newLightColour = light.Colour.Darken_Percentage(distant_intensity);
                    }

                    // Move the point from light-view space to light-screen space
                    Vector4D lightScreenSpacePoint = light.LightViewToLightScreen * lightViewSpacePoint;
                    if (light is PointLight or Spotlight)
                    {
                        lightScreenSpacePoint /= lightScreenSpacePoint.w;
                    }

                    Vector4D lightWindowSpacePoint = light.LightScreenToLightWindow * lightScreenSpacePoint;

                    // Round the points
                    int light_point_x = lightWindowSpacePoint.x.RoundToInt();
                    int light_point_y = lightWindowSpacePoint.y.RoundToInt();
                    float light_point_z = lightWindowSpacePoint.z;

                    //Trace.WriteLine("The following light point has been calculated: "+new Vector3D(light_point_x,light_point_y,light_point_z));

                    if (light_point_x >= 0 && light_point_x < light.ShadowMapWidth &&
                        light_point_y >= 0 && light_point_y < light.ShadowMapHeight)
                    {
                        if (light_point_z.Approx_Less_Than_Equals(light.ShadowMap.Values[light_point_x][light_point_y], 1E-4F))
                        {
                            // Point is not in shadow and light does contribute to the point's overall colour
                            pointColour = pointColour.Mix(newLightColour);
                            lightApplied = true;

                            /*if (light_point_z < -1) light_point_z = -1;
                            if (light_point_z > 1) light_point_z = 1;

                            int value = (255 * ((light_point_z + 1) / 2)).Round_to_Int();
                            Color greyscale_colour = Color.FromArgb(255, value, value, value);
                            bitmap.SetPixel(light_point_x, light_point_y, greyscale_colour);*/
                            
                            // Trace.WriteLine("Lighting was added at "+new Vector3D(light_point_x,light_point_y,light_point_z)+" and the shadow map point z was: "+light.Shadow_Map[light_point_x][light_point_y]);
                        }
                    }
                }
            }

            // Update the colour buffer (use black if there are no lights affecting the point)
            colourBuffer.Values[x][y] = lightApplied ? pointColour : Color.Black;
        }
    }
}