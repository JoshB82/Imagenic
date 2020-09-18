namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Draw_Edge(Edge edge,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_camera_view,
            Matrix4x4 camera_view_to_camera_screen)
        {
            // Reset face vertices
            edge.Reset_Vertices();

            // Move the edge from model space to world space
            edge.Apply_Matrix(model_to_world);

            // Move the edge from world space to camera-view space
            edge.Apply_Matrix(world_to_camera_view);

            // Clip the edge in camera-view space
            foreach (Clipping_Plane view_clipping_plane in Render_Camera.Camera_View_Clipping_Planes)
            {
                if (!Clip_Edge(view_clipping_plane.Point, view_clipping_plane.Normal, edge)) return;
            }

            // Move the edge from camera-view space to camera-screen space, including a correction for perspective
            edge.Apply_Matrix(camera_view_to_camera_screen);

            if (Render_Camera is Perspective_Camera)
            {
                edge.P1 /= edge.P1.w;
                edge.P2 /= edge.P2.w; 
            }

            // Clip the edge in camera-screen space
            if (Settings.Screen_Space_Clip)
            {
                foreach (Clipping_Plane screen_clipping_plane in Camera.Camera_Screen_Clipping_Planes)
                {
                    if (!Clip_Edge(screen_clipping_plane.Point, screen_clipping_plane.Normal, edge)) return;
                }
            }

            // Mode the edge from camera-screen space to window space
            edge.Apply_Matrix(screen_to_window);

            // Round the vertices
            int result_point_1_x = edge.P1.x.Round_to_Int();
            int result_point_1_y = edge.P1.y.Round_to_Int();
            float result_point_1_z = edge.P1.z;
            int result_point_2_x = edge.P2.x.Round_to_Int();
            int result_point_2_y = edge.P2.y.Round_to_Int();
            float result_point_2_z = edge.P2.z;

            // Finally draw the line
            Line(edge.Colour,
                result_point_1_x, result_point_1_y, result_point_1_z,
                result_point_2_x, result_point_2_y, result_point_2_z);
        }
    }
}