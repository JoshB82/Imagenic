namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Draw_Edge(Edge edge, string camera_type,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_view,
            Matrix4x4 view_to_screen)
        {
            // Move the edge from model space to world space
            edge.World_P1 = new Vector3D(model_to_world * edge.P1);
            edge.World_P2 = new Vector3D(model_to_world * edge.P2);
            edge.P1 = model_to_world * edge.P1;
            edge.P2 = model_to_world * edge.P2;

            // Move the edge from world space to view space
            edge.P1 = world_to_view * edge.P1;
            edge.P2 = world_to_view * edge.P2;

            // Clip the edge in view space
            foreach (Clipping_Plane view_clipping_plane in Render_Camera.View_Clipping_Planes)
            {
                if (!Clip_Edge(view_clipping_plane.Point, view_clipping_plane.Normal, ref edge)) return;
            }

            // Move the edge from view space to screen space, including a correction for perspective
            edge.P1 = view_to_screen * edge.P1;
            edge.P2 = view_to_screen * edge.P2;

            if (camera_type == "Perspective_Camera")
            {
                edge.P1 /= edge.P1.W;
                edge.P2 /= edge.P2.W;
            }

            // Clip the edge in screen space
            foreach (Clipping_Plane screen_clipping_plane in screen_clipping_planes)
            {
                if (!Clip_Edge(screen_clipping_plane.Point, screen_clipping_plane.Normal, ref edge)) return;
            }

            // Mode the edge from screen space to window space
            edge.P1 = screen_to_window * edge.P1;
            edge.P2 = screen_to_window * edge.P2;

            // Round the vertices
            int result_point_1_x = Round_To_Int(edge.P1.X);
            int result_point_1_y = Round_To_Int(edge.P1.Y);
            double result_point_1_z = edge.P1.Z;
            int result_point_2_x = Round_To_Int(edge.P2.X);
            int result_point_2_y = Round_To_Int(edge.P2.Y);
            double result_point_2_z = edge.P2.Z;

            // Finally draw the line
            Line(edge.Colour,
                result_point_1_x, result_point_1_y, result_point_1_z,
                result_point_2_x, result_point_2_y, result_point_2_z);
        }
    }
}