/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides methods for generating data required to draw edges.
 */

using _3D_Engine.Maths;
using _3D_Engine.Maths.Vectors;
using _3D_Engine.Rendering;
using _3D_Engine.SceneObjects.Meshes.Components;
using System.Drawing;

namespace _3D_Engine.SceneObjects.Cameras
{
    public abstract partial class Camera : SceneObject
    {
        private void Draw_Edge(
            Edge edge,
            ref Matrix4x4 modelToCameraView,
            ref Matrix4x4 cameraViewToCameraScreen)
            => Draw_Edge(
                edge.P1.Point,
                edge.P2.Point,
                edge.Colour,
                ref modelToCameraView,
                ref cameraViewToCameraScreen);

        private void Draw_Edge(
            Vector4D point1,
            Vector4D point2,
            Color colour,
            ref Matrix4x4 modelToCameraView,
            ref Matrix4x4 cameraViewToCameraScreen)
        {
            // Move the edge from model space to camera-view space
            point1 = modelToCameraView * point1;
            point2 = modelToCameraView * point2;

            // Clip the edge in camera-view space
            if (!Clipping.ClipEdges(this.CameraViewClippingPlanes, ref point1, ref point2)) { return; }

            // Move the edge from camera-view space to camera-screen space, including a correction for perspective
            point1 = cameraViewToCameraScreen * point1;
            point2 = cameraViewToCameraScreen * point2;

            if (this is PerspectiveCamera)
            {
                point1 /= point1.w;
                point2 /= point2.w; 
            }

            // Clip the edge in camera-screen space
            if (Settings.Screen_Space_Clip)
            {
                if (!Clipping.ClipEdges(Camera.CameraScreenClippingPlanes, ref point1, ref point2)) { return; }
            }

            // Mode the edge from camera-screen space to window space
            point1 = cameraScreenToWindow * point1;
            point2 = cameraScreenToWindow * point2;

            // Round the vertices
            int resultPoint1X = point1.x.RoundToInt();
            int resultPoint1Y = point1.y.RoundToInt();
            float resultPoint1Z = point1.z;
            int resultPoint2X = point2.x.RoundToInt();
            int resultPoint2Y = point2.y.RoundToInt();
            float resultPoint2Z = point2.z;

            // Finally draw the line
            Line(colour,
                resultPoint1X, resultPoint1Y, resultPoint1Z,
                resultPoint2X, resultPoint2Y, resultPoint2Z);
        }
    }
}