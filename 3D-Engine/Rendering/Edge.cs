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
            ref Matrix4x4 model_to_camera_view,
            ref Matrix4x4 camera_view_to_camera_screen)
            => Draw_Edge(
                edge.P1.Point,
                edge.P2.Point,
                edge.Colour,
                ref model_to_camera_view,
                ref camera_view_to_camera_screen);

        private void Draw_Edge(
            Vector4D point_1,
            Vector4D point_2,
            Color colour,
            ref Matrix4x4 modelToCameraView,
            ref Matrix4x4 cameraViewToCameraScreen)
        {
            // Move the edge from model space to camera-view space
            point_1 = modelToCameraView * point_1;
            point_2 = modelToCameraView * point_2;

            // Clip the edge in camera-view space
            if (!Clipping.ClipEdges(this.CameraViewClippingPlanes, ref point_1, ref point_2)) { return; }

            // Move the edge from camera-view space to camera-screen space, including a correction for perspective
            point_1 = cameraViewToCameraScreen * point_1;
            point_2 = cameraViewToCameraScreen * point_2;

            if (this is PerspectiveCamera)
            {
                point_1 /= point_1.w;
                point_2 /= point_2.w; 
            }

            // Clip the edge in camera-screen space
            if (Settings.Screen_Space_Clip)
            {
                if (!Clipping.ClipEdges(Camera.CameraScreenClippingPlanes, ref point_1, ref point_2)) { return; }
            }

            // Mode the edge from camera-screen space to window space
            point_1 = screenToWindow * point_1;
            point_2 = screenToWindow * point_2;

            // Round the vertices
            int result_point_1_x = point_1.x.RoundToInt();
            int result_point_1_y = point_1.y.RoundToInt();
            float result_point_1_z = point_1.z;
            int result_point_2_x = point_2.x.RoundToInt();
            int result_point_2_y = point_2.y.RoundToInt();
            float result_point_2_z = point_2.z;

            // Finally draw the line
            Line(colour,
                result_point_1_x, result_point_1_y, result_point_1_z,
                result_point_2_x, result_point_2_y, result_point_2_z);
        }
    }
}