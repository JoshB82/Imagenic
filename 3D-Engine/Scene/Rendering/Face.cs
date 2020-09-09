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

            // Discard the face if it is not visible
            if (dimension == 3)
            {
                Vector3D camera_to_face = new Vector3D(face.P1) - Render_Camera.World_Origin;
                Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(face.P1), new Vector3D(face.P2), new Vector3D(face.P3));

                if (camera_to_face * normal >= 0)
                {
                    return;
                }
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

                if (Render_Camera.GetType().Name == "Perspective_Camera")
                {
                    clipped_face.P1 /= clipped_face.P1.W;
                    clipped_face.P2 /= clipped_face.P2.W;
                    clipped_face.P3 /= clipped_face.P3.W;
                    
                    if (face.Has_Texture)
                    {
                        clipped_face.T1 /= clipped_face.P1.W;
                        clipped_face.T2 /= clipped_face.P2.W;
                        clipped_face.T3 /= clipped_face.P3.W;
                    }
                }
            }

            // Clip the face in camera-screen space
            if (Settings.Screen_Space_Clip && Queue_Clip_Face(face_clip_queue, camera_screen_clipping_planes) == 0)
            {
                return;
            }// anything outside cube?

            foreach (Face clipped_face in face_clip_queue)
            {
                // Don't draw anything if the face is flat
                if ((clipped_face.P1.X == clipped_face.P2.X && clipped_face.P2.X == clipped_face.P3.X) || (clipped_face.P1.Y == clipped_face.P2.Y && clipped_face.P2.Y == clipped_face.P3.Y))
                {
                    continue;
                }

                // Mode the new triangles from camera-screen space to camera-window space
                clipped_face.Apply_Matrix(screen_to_window);

                // Round the vertices
                int x1 = clipped_face.P1.X.Round_to_Int();
                int y1 = clipped_face.P1.Y.Round_to_Int();
                double z1 = clipped_face.P1.Z;
                int x2 = clipped_face.P2.X.Round_to_Int();
                int y2 = clipped_face.P2.Y.Round_to_Int();
                double z2 = clipped_face.P2.Z;
                int x3 = clipped_face.P3.X.Round_to_Int();
                int y3 = clipped_face.P3.Y.Round_to_Int();
                double z3 = clipped_face.P3.Z;                                
                
                // Check if the face has a texture
                if (face.Has_Texture)
                {
                    // Scale the texture co-ordinates
                    int width = face.Texture_Object.File.Width - 1;
                    int height = face.Texture_Object.File.Height - 1;

                    // afterwards?
                    double tx1 = face.T1.X * width;
                    double ty1 = face.T1.Y * height;
                    double tx2 = face.T2.X * width;
                    double ty2 = face.T2.Y * height;
                    double tx3 = face.T3.X * width;
                    double ty3 = face.T3.Y * height;

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
        private void Z_Buffer_Check(object colour, int x, int y, double z)
        {
            try
            {
                if (z < z_buffer[x][y])
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = (Color)colour;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception($"Attempted to render outside the canvas at ({x}, {y}, {z})");
            }
        }

        // Shadow Map Checks (SMC)
        private void SMC_Camera_Orthogonal(Color point_colour, Matrix4x4 window_to_world, int x, int y, double z)
        {
            // Move the point from window space to world space and apply lighting
            Apply_Lighting(window_to_world * new Vector4D(x, y, z), point_colour, x, y);
        }

        private void SMC_Camera_Perspective(Color point_colour, Matrix4x4 window_to_camera_screen, Matrix4x4 camera_screen_to_world, int x, int y, double z)
        {
            // Move the point from window space to camera-screen space
            Vector4D camera_screen_space_point = window_to_camera_screen * new Vector4D(x, y, z);

            // Move the point from camera-screen space to world space and apply lighting
            camera_screen_space_point *= 2 * Render_Camera.Z_Near * Render_Camera.Z_Far / (Render_Camera.Z_Near + Render_Camera.Z_Far - camera_screen_space_point.Z * (Render_Camera.Z_Far - Render_Camera.Z_Near));

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
                    double distant_intensity = light.Strength / light_to_point.Squared_Magnitude() * 100;
                    Color new_light_colour = light.Colour.Darken(distant_intensity);

                    // Move the point from light-view space to light-screen space
                    Vector4D light_screen_space_point = light.Light_View_to_Light_Screen * light_view_space_point;

                    if (light is Spotlight)
                    {
                        light_screen_space_point /= light_screen_space_point.W;
                    }

                    Vector4D light_window_space_point = light.Light_Screen_to_Light_Window * light_screen_space_point;

                    int light_point_x = light_window_space_point.X.Round_to_Int(); //?
                    int light_point_y = light_window_space_point.Y.Round_to_Int();
                    double light_point_z = light_window_space_point.Z;

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

        //source!
        private int Queue_Clip_Face(Queue<Face> face_clip_queue, Clipping_Plane[] clipping_planes)
        {
            foreach (Clipping_Plane clipping_plane in clipping_planes)
            {
                int no_triangles = face_clip_queue.Count;

                while (no_triangles-- > 0)
                {
                    Clip_Face(face_clip_queue.Dequeue(), face_clip_queue, clipping_plane.Point, clipping_plane.Normal);
                }
            }

            return face_clip_queue.Count;
        }
    }
}