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

using _3D_Engine.Entities.Groups;
using _3D_Engine.Entities.SceneObjects.Meshes.Components;
using _3D_Engine.Entities.SceneObjects.Meshes.Components.Faces;
using _3D_Engine.Entities.SceneObjects.Meshes.ThreeDimensions;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Rendering;
using _3D_Engine.Entities.SceneObjects.Meshes;
using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Utilities;
using System;
using System.Collections.Generic;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects
{
    public abstract partial class RenderingObject : SceneObject
    {
        #region Fields and Properties

        protected const byte outOfBoundsValue = 2;

        #endregion

        #region Methods

        internal abstract void ResetBuffers();
        internal abstract void AddPointToBuffers<T>(T data, int x, int y, float z);

        internal void CalculateDepth(SceneObject sceneObject)
        {
            MessageBuilder<GeneratingDepthValuesMessage>.WithType<SceneObject>();

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
                            foreach (Triangle triangle in face.Triangles)
                            {
                                if (triangle.Visible)
                                {
                                    AddTriangleToBuffer(triangle, camera.Icon.Dimension, ref modelToView);
                                }
                            }
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
                            foreach (Triangle triangle in face.Triangles)
                            {
                                if (triangle.Visible)
                                {
                                    AddTriangleToBuffer(triangle, light.Icon.Dimension, ref modelToView);
                                }
                            }
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
                            foreach (Triangle triangle in face.Triangles)
                            {
                                if (triangle.Visible)
                                {
                                    AddTriangleToBuffer(triangle, mesh.Dimension, ref modelToView);
                                }
                            }
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

                    foreach (Triangle face in directionForward.Triangles)
                    {
                        AddTriangleToBuffer(face, 3, ref directionForwardModelToView);
                    }
                    foreach (Triangle face in directionUp.Triangles)
                    {
                        AddTriangleToBuffer(face, 3, ref directionUpModelToView);
                    }
                    foreach (Triangle face in directionRight.Triangles)
                    {
                        AddTriangleToBuffer(face, 3, ref directionRightModelToView);
                    }
                }
            }

            #if DEBUG

            MessageBuilder<GeneratedDepthValuesMessage>.WithType<SceneObject>();

            #endif
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