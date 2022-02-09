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
using System.Drawing;
using Imagenic.Core.Entities.SceneObjects.Meshes.Components.Edges;
using Imagenic.Core.Renderers.Rasterising;

namespace _3D_Engine.Entities.SceneObjects.RenderingObjects.Cameras
{
    public abstract partial class Camera : RenderingObject
    {
        private void Draw_Edge(
            Edge edge,
            ref Matrix4x4 modelToView,
            ref Matrix4x4 viewToScreen)
            => DrawEdge(
                edge.P1.Point,
                edge.P2.Point,
                edge.Colour,
                ref modelToView,
                ref viewToScreen);

        private void DrawEdge(
            Vector4D point1,
            Vector4D point2,
            Color colour,
            ref Matrix4x4 modelToView,
            ref Matrix4x4 viewToScreen)
        {
            // Move the edge from model space to view space
            point1 = modelToView * point1;
            point2 = modelToView * point2;

            // Clip the edge in view space
            if (!Clipping.ClipEdges(ViewClippingPlanes, ref point1, ref point2)) { return; }

            // Move the edge from view space to screen space, including a correction for perspective
            point1 = viewToScreen * point1;
            point2 = viewToScreen * point2;

            if (this is PerspectiveCamera)
            {
                point1 /= point1.w;
                point2 /= point2.w;
            }

            // Clip the edge in screen space
            if (Settings.Screen_Space_Clip)
            {
                if (!Clipping.ClipEdges(ScreenClippingPlanes, ref point1, ref point2)) { return; }
            }

            // Mode the edge from screen space to window space
            point1 = ScreenToWindow * point1;
            point2 = ScreenToWindow * point2;

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