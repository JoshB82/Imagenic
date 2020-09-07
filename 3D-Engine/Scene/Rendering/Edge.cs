﻿namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Draw_Edge(Edge edge,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_view,
            Matrix4x4 view_to_screen)
        {
            // Reset face vertices
            edge.P1 = edge.Model_P1.Point;
            edge.P2 = edge.Model_P2.Point;

            // Move the edge from model space to world space
            edge.P1 = model_to_world * edge.P1;
            edge.P2 = model_to_world * edge.P2;
            edge.World_P1 = new Vector3D(edge.P1);
            edge.World_P2 = new Vector3D(edge.P2);

            // Move the edge from world space to view space
            edge.P1 = world_to_view * edge.P1;
            edge.P2 = world_to_view * edge.P2;

            // Clip the edge in view space
            if (Settings.View_Space_Clip)
            {
                foreach (Clipping_Plane view_clipping_plane in Render_Camera.Camera_View_Clipping_Planes)
                {
                    if (!Clip_Edge(view_clipping_plane.Point, view_clipping_plane.Normal, edge)) return;
                }
            }

            // Move the edge from view space to screen space, including a correction for perspective
            edge.P1 = view_to_screen * edge.P1;
            edge.P2 = view_to_screen * edge.P2;

            if (Render_Camera.GetType().Name == "Perspective_Camera")
            {
                edge.P1 /= edge.P1.W;
                edge.P2 /= edge.P2.W; 
            }

            // Clip the edge in screen space
            if (Settings.Screen_Space_Clip)
            {
                foreach (Clipping_Plane screen_clipping_plane in camera_screen_clipping_planes)
                {
                    if (!Clip_Edge(screen_clipping_plane.Point, screen_clipping_plane.Normal, edge)) return;
                }
            }

            // Mode the edge from screen space to window space
            edge.P1 = screen_to_window * edge.P1;
            edge.P2 = screen_to_window * edge.P2;

            // Round the vertices
            int result_point_1_x = edge.P1.X.Round_to_Int();
            int result_point_1_y = edge.P1.Y.Round_to_Int();
            double result_point_1_z = edge.P1.Z;
            int result_point_2_x = edge.P2.X.Round_to_Int();
            int result_point_2_y = edge.P2.Y.Round_to_Int();
            double result_point_2_z = edge.P2.Z;

            // Finally draw the line
            Line(edge.Colour,
                result_point_1_x, result_point_1_y, result_point_1_z,
                result_point_2_x, result_point_2_y, result_point_2_z);
        }
    }
}