/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for generating data required to generate depth values.
 */

using _3D_Engine.Groups;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.SceneObjects.Meshes;
using _3D_Engine.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Utilities;
using System;
using System.Collections.Generic;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;

namespace _3D_Engine.SceneObjects.RenderingObjects
{
    public abstract partial class RenderingObject : SceneObject
    {
        #region Fields and Properties

        protected const byte outOfBoundsValue = 2;

        #endregion

        #region Methods

        internal abstract void ResetBuffers();
        internal abstract void AddPointToBuffers(object data, int x, int y, float z);

        internal void CalculateDepth(Group scene)
        {
            ResetBuffers();

            foreach (Camera camera in scene.Cameras)
            {
                if (camera.Icon.Visible)
                {
                    Matrix4x4 modelToView = WorldToView * camera.Icon.ModelToWorld;

                    foreach (Face face in camera.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            AddFaceToZBuffer(face, camera.Icon.Dimension, ref modelToView);
                        }
                    }
                }
            }

            foreach (Light light in scene.Lights)
            {
                if (light.Icon.Visible)
                {
                    Matrix4x4 modelToView = WorldToView * light.Icon.ModelToWorld;

                    foreach (Face face in light.Icon.Faces)
                    {
                        if (face.Visible)
                        {
                            AddFaceToZBuffer(face, light.Icon.Dimension, ref modelToView);
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
                            AddFaceToZBuffer(face, mesh.Dimension, ref modelToView);
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
                        AddFaceToZBuffer(face, 3, ref directionForwardModelToView);
                    }
                    foreach (Face face in directionUp.Faces)
                    {
                        AddFaceToZBuffer(face, 3, ref directionUpModelToView);
                    }
                    foreach (Face face in directionRight.Faces)
                    {
                        AddFaceToZBuffer(face, 3, ref directionRightModelToView);
                    }
                }
            }

            #if DEBUG

            ConsoleOutput.DisplayMessageFromObject(this, "Generated depth values.");

            #endif
        }

        private void AddFaceToZBuffer(Face face, int meshDimension, ref Matrix4x4 modelToView)
        {
            Action<object, int, int, float> bufferAction = (this is Light || face is SolidFace) ? AddPointToBuffers : null;

            // Reset the vertices to model space values
            face.ResetVertices();

            // Move the face from model space to view space
            face.ApplyMatrix(modelToView);

            // Back-face culling if the mesh is three-dimensional
            if (meshDimension == 3)
            {
                // Discard the face if it is not visible from the rendering object's point of view
                if ((Vector3D)face.P1 * Vector3D.NormalFromPlane((Vector3D)face.P1, (Vector3D)face.P2, (Vector3D)face.P3) >= 0) { return; }
            }

            // Clip the face in view space
            Queue<Face> faceClip = new(); faceClip.Enqueue(face);
            if (!Clipping.ClipFaces(faceClip, ViewClippingPlanes)) { return; }

            // Move the new triangles from view space to screen space, including a correction for perspective
            foreach (Face clippedFace in faceClip)
            {
                // Move the face from view space to screen space
                clippedFace.ApplyMatrix(ViewToScreen);

                if (this is PerspectiveCamera or Spotlight)
                {
                    clippedFace.P1 /= clippedFace.P1.w;
                    clippedFace.P2 /= clippedFace.P2.w;
                    clippedFace.P3 /= clippedFace.P3.w;

                    if (this is PerspectiveCamera && clippedFace is TextureFace)
                    {
                        ((TextureFace)clippedFace).T1 /= clippedFace.P1.w;
                        ((TextureFace)clippedFace).T2 /= clippedFace.P2.w;
                        ((TextureFace)clippedFace).T3 /= clippedFace.P3.w;
                    }
                }
            }

            // Clip the face in screen space
            if (!Clipping.ClipFaces(faceClip, ScreenClippingPlanes)) { return; } // anything outside cube?

            foreach (Face clippedFace in faceClip)
            {
                // Skip the face if it is flat
                if ((clippedFace.P1.x == clippedFace.P2.x && clippedFace.P2.x == clippedFace.P3.x) ||
                    (clippedFace.P1.y == clippedFace.P2.y && clippedFace.P2.y == clippedFace.P3.y))
                { continue; }

                // Move the face from screen space to window space
                clippedFace.ApplyMatrix(ScreenToWindow);

                // Call the required interpolator method
                clippedFace.Interpolator(this, bufferAction);
            }
        }

        #endregion
    }
}

/*
// Draw outline if needed ??
if (face.DrawOutline)
{
    DrawEdge(face.p1, face.p2, Color.Black, ref modelToView, ref viewToScreen);
    DrawEdge(face.p1, face.p3, Color.Black, ref modelToView, ref viewToScreen);
    DrawEdge(face.p2, face.p3, Color.Black, ref modelToView, ref viewToScreen);
}*/