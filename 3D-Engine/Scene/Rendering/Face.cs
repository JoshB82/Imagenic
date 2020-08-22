using System;
using System.Collections.Generic;
using System.Drawing;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Light_Face(Light light, Face face,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_light_view,
            Matrix4x4 light_view_to_light_screen)
        {
            // Reset the vertices to model space values
            face.Reset_Vertices();

            // Move face from model space to world space
            face.Apply_Matrix(model_to_world);

            // Move the face from world space to light-view space
            face.Apply_Matrix(world_to_light_view);

            // ohter clipping?

            // Clip the face in light view space
            Queue<Face> light_view_face_clip = new Queue<Face>();
            light_view_face_clip.Enqueue(face);

            if (Settings.View_Space_Clip && Queue_Clip_Face(light_view_face_clip, light.Light_View_Clipping_Planes) == 0) return;
            Face[] new_light_view_triangles = light_view_face_clip.ToArray();

            // Move the new triangles from light space to screen space, including a correction for perspective
            for (int i = 0; i < new_light_view_triangles.Length; i++)
            {
                new_light_view_triangles[i].Apply_Matrix(light_view_to_light_screen);

                if (Render_Camera.GetType().Name == "Perspective_Camera")
                {
                    new_light_view_triangles[i].P1 /= new_light_view_triangles[i].P1.W;
                    new_light_view_triangles[i].P2 /= new_light_view_triangles[i].P2.W;
                    new_light_view_triangles[i].P3 /= new_light_view_triangles[i].P3.W;
                }
            }

            // Clip the face in screen space
            Queue<Face> light_screen_face_clip = new Queue<Face>(new_light_view_triangles);

            if (Settings.Screen_Space_Clip && Queue_Clip_Face(light_screen_face_clip, camera_screen_clipping_planes) == 0) return;
            Face[] new_light_screen_triangles = light_screen_face_clip.ToArray();

            foreach (Face new_light_screen_triangle in new_light_screen_triangles) // check other loops
            {
                // Mode the new triangles from light-screen space to window space
                new_light_screen_triangle.Apply_Matrix(screen_to_window);
            }

            // Assign the correct method that will be called for each point in the triangle
            Action<object, int, int, double> depth = Mesh_Depth_From_Light;

            foreach (Face new_face in new_light_screen_triangles)
            {
                // Round the vertices
                int x1 = Round_To_Int(new_face.P1.X);
                int y1 = Round_To_Int(new_face.P1.Y);
                double z1 = new_face.P1.Z;
                int x2 = Round_To_Int(new_face.P2.X);
                int y2 = Round_To_Int(new_face.P2.Y);
                double z2 = new_face.P2.Z;
                int x3 = Round_To_Int(new_face.P3.X);
                int y3 = Round_To_Int(new_face.P3.Y);
                double z3 = new_face.P3.Z;

                // Don't interpolate anything if triangle is flat
                if (x1 == x2 && x2 == x3) return;
                if (y1 == y2 && y2 == y3) return;

                // Sort the vertices by their y-co-ordinate
                Sort_By_Y(
                    ref x1, ref y1, ref z1,
                    ref x2, ref y2, ref z2,
                    ref x3, ref y3, ref z3);

                // Interpolate each point in the triangle
                Interpolate_Triangle(light, depth,
                    x1, y1, z1,
                    x2, y2, z2,
                    x3, y3, z3);
            }
        }

        private void Mesh_Depth_From_Light(object light, int x, int y, double z)
        {
            // Check against z-buffer
            Light_Check_Against_Z_Buffer((Light)light, x, y, z); // Why the explicit cast?
        }

        private void Draw_Face(Face face, string mesh_type,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_camera_view,
            Matrix4x4 camera_view_to_camera_screen)
        {
            // Reset the vertices to model space values
            face.Reset_Vertices();

            // Move face from model space to world space
            face.Apply_Matrix(model_to_world);

            // Discard the face if it is not visible
            Vector3D camera_to_face = new Vector3D(face.P1) - Render_Camera.World_Origin;
            Vector3D normal = Vector3D.Normal_From_Plane(new Vector3D(face.P1), new Vector3D(face.P2), new Vector3D(face.P3));
            
            if (camera_to_face * normal >= 0
                && mesh_type != "Circle"
                && mesh_type != "Ellipse"
                && mesh_type != "Plane"
                && mesh_type != "Ring"
                && mesh_type != "Square"
                && mesh_type != "Text2D")
            return;

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
            Queue<Face> camera_view_face_clip = new Queue<Face>();
            camera_view_face_clip.Enqueue(face);

            if (Settings.View_Space_Clip && Queue_Clip_Face(camera_view_face_clip, Render_Camera.Camera_View_Clipping_Planes) == 0) return;
            Face[] new_camera_view_triangles = camera_view_face_clip.ToArray();

            // Move the new triangles from camera-view space to camera-screen space, including a correction for perspective
            for (int i = 0; i < new_camera_view_triangles.Length; i++)
            {
                new_camera_view_triangles[i].Apply_Matrix(camera_view_to_camera_screen);

                if (Render_Camera.GetType().Name == "Perspective_Camera")
                {
                    new_camera_view_triangles[i].P1 /= new_camera_view_triangles[i].P1.W;
                    new_camera_view_triangles[i].P2 /= new_camera_view_triangles[i].P2.W;
                    new_camera_view_triangles[i].P3 /= new_camera_view_triangles[i].P3.W;
                    
                    if (face.Has_Texture)
                    {
                        new_camera_view_triangles[i].T1 /= new_camera_view_triangles[i].P1.W;
                        new_camera_view_triangles[i].T2 /= new_camera_view_triangles[i].P2.W;
                        new_camera_view_triangles[i].T3 /= new_camera_view_triangles[i].P3.W;
                    }
                }
            }

            // Clip the face in camera-screen space
            Queue<Face> camera_screen_face_clip = new Queue<Face>(new_camera_view_triangles);

            if (Settings.Screen_Space_Clip && Queue_Clip_Face(camera_screen_face_clip, camera_screen_clipping_planes) == 0) return; // anything outside cube?
            Face[] new_camera_screen_triangles = camera_screen_face_clip.ToArray();

            for (int i = 0; i < new_camera_screen_triangles.Length; i++)
            {
                // Mode the new triangles from camera-screen space to camera-window space
                new_camera_screen_triangles[i].Apply_Matrix(screen_to_window);
                
                // Round the vertices
                int x1 = Round_To_Int(new_camera_screen_triangles[i].P1.X);
                int y1 = Round_To_Int(new_camera_screen_triangles[i].P1.Y);
                double z1 = new_camera_screen_triangles[i].P1.Z;
                int x2 = Round_To_Int(new_camera_screen_triangles[i].P2.X);
                int y2 = Round_To_Int(new_camera_screen_triangles[i].P2.Y);
                double z2 = new_camera_screen_triangles[i].P2.Z;
                int x3 = Round_To_Int(new_camera_screen_triangles[i].P3.X);
                int y3 = Round_To_Int(new_camera_screen_triangles[i].P3.Y);
                double z3 = new_camera_screen_triangles[i].P3.Z;

                // Don't draw anything if triangle is flat
                if (x1 == x2 && x2 == x3) return;
                if (y1 == y2 && y2 == y3) return;

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
                    Action<object, int, int, double> camera_depth = Mesh_Depth_From_Camera;

                    // Sort the vertices by their y-co-ordinate
                    Sort_By_Y(
                        ref x1, ref y1, ref z1,
                        ref x2, ref y2, ref z2,
                        ref x3, ref y3, ref z3);

                    Interpolate_Triangle(face, camera_depth,
                        x1, y1, z1,
                        x2, y2, z2,
                        x3, y3, z3);
                }
            }
        }

        private void Mesh_Depth_From_Camera(object face, int x, int y, double z)
        {
            // Check if point is visible from the camera
            try
            {
                if (z < z_buffer[x][y])
                {
                    z_buffer[x][y] = z;
                    colour_buffer[x][y] = ((Face)face).Colour;
                }
                else
                {
                    return;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Attempted to draw outside the canvas.");
            }
            
            return;
            // Move the point from camera-screen space to camera-view space
            double camera_view_space_z = 2 * Render_Camera.Z_Near * Render_Camera.Z_Far / (Render_Camera.Z_Near + Render_Camera.Z_Far - z * (Render_Camera.Z_Far - Render_Camera.Z_Near));
            double camera_view_space_x = Render_Camera.Width / (2 * Render_Camera.Z_Near) * camera_view_space_z * x;
            double camera_view_space_y = Render_Camera.Height / (2 * Render_Camera.Z_Near) * camera_view_space_z * y;
            Vector4D camera_view_space_point = new Vector4D(camera_view_space_x, camera_view_space_y, camera_view_space_z);

            // Move the point from camera-view space to world space
            Vector4D world_space_point = Render_Camera.Model_to_World * camera_view_space_point;
        
            // Apply light colour correction
            Color point_colour = ((Face)face).Colour;

            int shadow_count = 0;
            foreach (Light light in Lights)
            {
                // Move the point from world space to light-view space
                Vector4D light_space_point = light.World_to_Light_View * world_space_point;


                //double distant_intensity = light.Strength / Math.Pow(z, 2); // omg
                Color new_light_colour = light.Colour;//.Darken(distant_intensity);

                // use extension methods or not?
                // Check if point is in shadow
                if (light_space_point.Z <= light.z_buffer[x][y]) // ??????
                {
                    // Point is not in shadow and light does contribute to the point's overall colour
                    point_colour = point_colour.Mix(new_light_colour);
                }
                else
                {
                    // Point is in shadow and light does not contribute to the point's overall colour
                    shadow_count++;
                }
            }

            if (shadow_count == Lights.Count) point_colour = Color.Black;

            colour_buffer[x][y] = point_colour;
        }

        private int Queue_Clip_Face(Queue<Face> face_clip, Clipping_Plane[] clipping_planes)
        {
            int no_triangles = 1;

            foreach (Clipping_Plane clipping_plane in clipping_planes)
            {
                while (no_triangles > 0)
                {
                    (int intersection_triangles, Face[] triangles) = Clip_Face(clipping_plane.Point, clipping_plane.Normal, face_clip.Dequeue());
                    for (int i = 0; i < intersection_triangles; i++) face_clip.Enqueue(triangles[i]);
                    no_triangles--;
                }
                no_triangles = face_clip.Count;
            }

            return no_triangles;
        }
    }
}

/*
    
    Color face_colour = face.Colour;
    if (Light_List.Count > 0)
    {
        double max_intensity = 0, true_intensity = 0;
        foreach (Light light in Light_List) max_intensity += light.Intensity;
        foreach (Light light in Light_List)
        {
            switch (light.GetType().Name)
            {
                case "Distant_Light":
                    true_intensity = Math.Max(0, -light.World_Direction * normal) * light.Intensity;
                    break;
                case "Point_Light":
                    true_intensity = Math.Max(0, -new Vector3D(point_1 - light.World_Origin).Normalise() * normal) * light.Intensity;
                    break;
                case "Spot_Light":
                    Vector3D light_to_mesh = new Vector3D(point_1 - light.World_Origin);
                    if (light_to_mesh.Angle(light.World_Direction) > ((Spotlight)light).Angle || light_to_mesh * light.World_Direction > ((Spotlight)light).Distance) continue;
                    true_intensity = Math.Max(0, -light.World_Direction * normal) * light.Intensity;
                    break;
                case "Ambient_Light":
                    break;
            }
            double scaled_intensity = true_intensity / max_intensity;

            byte new_red = (byte)Round_To_Int((face.Colour.R + light.Colour.R) * 255 / 510 * scaled_intensity);
            byte new_green = (byte)Round_To_Int((face.Colour.G + light.Colour.G) * 255 / 510 * scaled_intensity);
            byte new_blue = (byte)Round_To_Int((face.Colour.B + light.Colour.B) * 255 / 510 * scaled_intensity);

            face_colour = Color.FromArgb(face.Colour.A, new_red, new_green, new_blue);
        }
    }
*/