using System;
using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Generate_Z_Buffer(Face face, int dimension,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_camera_view,
            Matrix4x4 camera_view_to_camera_screen)
        {
            // Reset the vertices to model space values
            face.Reset_Vertices();

            // Move face from model space to world space
            face.Apply_Matrix(model_to_world);

            // Discard the face if it is not visible from the camera's point of view
            if (dimension == 3)
            {
                Vector3D camera_to_face = new Vector3D(face.P1) - Render_Camera.World_Origin;
                Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(face.P1), new Vector3D(face.P2), new Vector3D(face.P3));

                if (camera_to_face * normal >= 0) return;
            }

            // Draw outline if needed
            if (face.Draw_Outline)//
            {
                Vertex vp1 = new Vertex(face.P1), vp2 = new Vertex(face.P2), vp3 = new Vertex(face.P3);
                Draw_Edge(new Edge(vp1, vp2), model_to_world, world_to_camera_view, camera_view_to_camera_screen);
                Draw_Edge(new Edge(vp1, vp3), model_to_world, world_to_camera_view, camera_view_to_camera_screen);
                Draw_Edge(new Edge(vp2, vp3), model_to_world, world_to_camera_view, camera_view_to_camera_screen);
            }

            // Move the face from world space to camera-view space
            face.Apply_Matrix(world_to_camera_view);

            // Clip the face in camera-view space
            Queue<Face> face_clip_queue = new Queue<Face>();
            face_clip_queue.Enqueue(face);

            if (Settings.View_Space_Clip && Queue_Clip_Face(face_clip_queue, Render_Camera.Camera_View_Clipping_Planes) == 0)
            {
                return;
            }

            // Move the new triangles from camera-view space to camera-screen space, including a correction for perspective
            foreach (Face clipped_face in face_clip_queue)
            {
                clipped_face.Apply_Matrix(camera_view_to_camera_screen);

                if (Render_Camera is Perspective_Camera)
                {
                    clipped_face.P1 /= clipped_face.P1.w;
                    clipped_face.P2 /= clipped_face.P2.w;
                    clipped_face.P3 /= clipped_face.P3.w;
                    
                    if (face.Has_Texture)
                    {
                        clipped_face.T1 /= clipped_face.P1.w;
                        clipped_face.T2 /= clipped_face.P2.w;
                        clipped_face.T3 /= clipped_face.P3.w;
                    }
                }
            }

            // Clip the face in camera-screen space
            if (Settings.Screen_Space_Clip && Queue_Clip_Face(face_clip_queue, Camera.Camera_Screen_Clipping_Planes) == 0)
            {
                return;
            }// anything outside cube?

            foreach (Face clipped_face in face_clip_queue)
            {
                // Don't draw anything if the face is flat
                if ((clipped_face.P1.x == clipped_face.P2.x && clipped_face.P2.x == clipped_face.P3.x) ||
                    (clipped_face.P1.y == clipped_face.P2.y && clipped_face.P2.y == clipped_face.P3.y))
                {
                    continue;
                }

                // Mode the new triangles from camera-screen space to camera-window space
                clipped_face.Apply_Matrix(screen_to_window);

                // Round the vertices
                int x1 = clipped_face.P1.x.Round_to_Int();
                int y1 = clipped_face.P1.y.Round_to_Int();
                float z1 = clipped_face.P1.z;
                int x2 = clipped_face.P2.x.Round_to_Int();
                int y2 = clipped_face.P2.y.Round_to_Int();
                float z2 = clipped_face.P2.z;
                int x3 = clipped_face.P3.x.Round_to_Int();
                int y3 = clipped_face.P3.y.Round_to_Int();
                float z3 = clipped_face.P3.z;                                
                
                // Check if the face has a texture
                if (face.Has_Texture)
                {
                    // Scale the texture co-ordinates
                    int texture_width = face.Texture_Object.File.Width - 1;
                    int texture_height = face.Texture_Object.File.Height - 1;

                    // afterwards?
                    float tx1 = face.T1.x * texture_width;
                    float ty1 = face.T1.y * texture_height;
                    float tx2 = face.T2.x * texture_width;
                    float ty2 = face.T2.y * texture_height;
                    float tx3 = face.T3.x * texture_width;
                    float ty3 = face.T3.y * texture_height;

                    // Sort the vertices by their y-co-ordinate
                    Textured_Sort_By_Y(
                        ref x1, ref y1, ref z1, ref tx1, ref ty1,
                        ref x2, ref y2, ref z2, ref tx2, ref ty2,
                        ref x3, ref y3, ref z3, ref tx3, ref ty3);

                    Textured_Triangle(face.Texture_Object.File,
                        x1, y1, z1, tx1, ty1,
                        x2, y2, z2, tx2, ty2,
                        x3, y3, z3, tx3, ty3);
                }
                else
                {
                    // Sort the vertices by their y-co-ordinate
                    Sort_By_Y(
                        ref x1, ref y1, ref z1,
                        ref x2, ref y2, ref z2,
                        ref x3, ref y3, ref z3);

                    // Generate z-buffer
                    Interpolate_Triangle(face.Colour, Z_Buffer_Check,
                        x1, y1, z1,
                        x2, y2, z2,
                        x3, y3, z3);
                }
            }
        }
        
        // Check if point is visible from the camera
        private void Z_Buffer_Check(object colour, int x, int y, float z)
        {
            try
            {
                if (z < z_buffer[x][y])
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = (Color)colour;
                }
            }
            catch (IndexOutOfRangeException e)
            {
                throw new IndexOutOfRangeException($"Attempted to render outside the canvas at ({x}, {y}, {z})", e);
            }
        }

        // Shadow Map Checks (SMC)
        private void SMC_Camera_Orthogonal(Color point_colour, Matrix4x4 window_to_world, int x, int y, float z)
        {
            // Move the point from window space to world space and apply lighting
            Apply_Lighting(window_to_world * new Vector4D(x, y, z), point_colour, x, y);
        }

        private void SMC_Camera_Perspective(Color point_colour, Matrix4x4 window_to_camera_screen, Matrix4x4 camera_screen_to_world, int x, int y, float z)
        {
            // Move the point from window space to camera-screen space
            Vector4D camera_screen_space_point = window_to_camera_screen * new Vector4D(x, y, z);

            // Move the point from camera-screen space to world space
            camera_screen_space_point *= 2 * Render_Camera.Z_Near * Render_Camera.Z_Far / (Render_Camera.Z_Near + Render_Camera.Z_Far - camera_screen_space_point.z * (Render_Camera.Z_Far - Render_Camera.Z_Near));

            // Apply lighting
            Apply_Lighting(camera_screen_to_world * camera_screen_space_point, point_colour, x, y);
        }

        // Lighting
        private void Apply_Lighting(Vector4D world_space_point, Color point_colour, int x, int y)
        {
            bool light_applied = false;

            foreach (Light light in Lights)
            {
                if (light.Visible)
                {
                    // Move the point from world space to light-view space
                    Vector4D light_view_space_point = light.World_to_Light_View * world_space_point;

                    // Darken the light's colour based on how far away the point is from the light
                    Vector3D light_to_point = new Vector3D(light_view_space_point);
                    float distant_intensity = light.Strength / light_to_point.Squared_Magnitude() * 100;
                    Color new_light_colour = light.Colour.Darken(distant_intensity);

                    // Move the point from light-view space to light-screen space
                    Vector4D light_screen_space_point = light.Light_View_to_Light_Screen * light_view_space_point;

                    if (light is Spotlight) light_screen_space_point /= light_screen_space_point.w;

                    Vector4D light_window_space_point = light.Light_Screen_to_Light_Window * light_screen_space_point;

                    int light_point_x = light_window_space_point.x.Round_to_Int(); //?
                    int light_point_y = light_window_space_point.y.Round_to_Int();
                    float light_point_z = light_window_space_point.z;

                    if (light_point_x >= 0 && light_point_x < light.Shadow_Map_Width && light_point_y >= 0 && light_point_y < light.Shadow_Map_Height)
                    {
                        if (light_point_z <= light.Shadow_Map[light_point_x][light_point_y]) // ??????
                        {
                            // Point is not in shadow and light does contribute to the point's overall colour
                            //point_colour = Color.Green;
                            point_colour = point_colour.Mix(new_light_colour);
                            light_applied = true;
                        }
                    }
                }
            }

            // Update the colour buffer (use black if there are no lights affecting the point)
            colour_buffer[x][y] = light_applied ? point_colour : Color.Black;
        }
    }
}