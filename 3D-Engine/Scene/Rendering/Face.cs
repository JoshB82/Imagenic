using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private void Draw_Face(Face face, string camera_type, string shape_type,
            Matrix4x4 model_to_world,
            Matrix4x4 world_to_view,
            Matrix4x4 view_to_screen)
        {
            // Move the face from model space to world space
            face.World_P1 = new Vector3D(model_to_world * face.P1);
            face.World_P2 = new Vector3D(model_to_world * face.P2);
            face.P1 = model_to_world * face.P1;
            face.P2 = model_to_world * face.P2;
            
            Vector3D camera_to_face = new Vector3D(face.P1 - new Vector4D(Render_Camera.World_Origin));
            Vector3D normal = Vector3D.Normal_From_Plane(face.World_P1, face.World_P2, face.World_P3);

            // Discard face if its not visible
            if (camera_to_face * normal >= 0 && shape_type != "Plane") return;

            // Draw outline if needed
            if (face.Draw_Outline)
            {
                Draw_Edge(new Edge(face.P1, face.P2), camera_type, model_to_world, world_to_view, view_to_screen);
                Draw_Edge(new Edge(face.P1, face.P3), camera_type, model_to_world, world_to_view, view_to_screen);
                Draw_Edge(new Edge(face.P2, face.P3), camera_type, model_to_world, world_to_view, view_to_screen);
            }

            // Move the face from world space to view space
            face.P1 = world_to_view * face.P1;
            face.P2 = world_to_view * face.P2;

            // Clip the face in view space
            Queue<Face> view_face_clip = new Queue<Face>(); view_face_clip.Enqueue(face);
            if (Queue_Clip_Face(view_face_clip, Render_Camera.View_Clipping_Planes, out Face[] new_view_triangles) == 0) return;

            // Not entirely sure why can't use foreach loop :/ (check how for loop works for 0,1,2,etc. for all for loops)
            for (int i = 0; i < new_view_triangles.Length; i++)
            {
                // Move the new triangles from view space to screen space, including a correction for perspective
                new_view_triangles[i].P1 = view_to_screen * new_view_triangles[i].P1;
                new_view_triangles[i].P2 = view_to_screen * new_view_triangles[i].P2;
                new_view_triangles[i].P3 = view_to_screen * new_view_triangles[i].P3;

                if (camera_type == "Perspective_Camera")
                {
                    new_view_triangles[i].P1 /= new_view_triangles[i].P1.W;
                    new_view_triangles[i].P2 /= new_view_triangles[i].P2.W;
                    new_view_triangles[i].P3 /= new_view_triangles[i].P3.W;
                    
                    if (face.Texture_Object.File != null)
                    {
                        new_view_triangles[i].T1 /= new_view_triangles[i].P1.W;
                        new_view_triangles[i].T2 /= new_view_triangles[i].P1.W;
                        new_view_triangles[i].T3 /= new_view_triangles[i].P1.W;
                    }
                }
            }

            // Clip the face in screen space
            Queue<Face> screen_face_clip = new Queue<Face>(new_view_triangles);
            if (Queue_Clip_Face(screen_face_clip, screen_clipping_planes, out Face[] new_screen_triangles) == 0) return;

            for (int i = 0; i < new_screen_triangles.Length; i++)
            {
                // Mode the new triangles from screen space to window space
                new_screen_triangles[i].P1 = screen_to_window * new_screen_triangles[i].P1;
                new_screen_triangles[i].P2 = screen_to_window * new_screen_triangles[i].P2;
                new_screen_triangles[i].P3 = screen_to_window * new_screen_triangles[i].P3;

                // Round the vertices
                int result_point_1_x = Round_To_Int(new_screen_triangles[i].P1.X);
                int result_point_1_y = Round_To_Int(new_screen_triangles[i].P1.Y);
                double result_point_1_z = new_screen_triangles[i].P1.Z;
                int result_point_2_x = Round_To_Int(new_screen_triangles[i].P2.X);
                int result_point_2_y = Round_To_Int(new_screen_triangles[i].P2.Y);
                double result_point_2_z = new_screen_triangles[i].P2.Z;
                int result_point_3_x = Round_To_Int(new_screen_triangles[i].P3.X);
                int result_point_3_y = Round_To_Int(new_screen_triangles[i].P3.Y);
                double result_point_3_z = new_screen_triangles[i].P3.Z;

                // Finally draw the triangle
                if (face.Texture_Object.File == null)
                {
                    Solid_Triangle(new_screen_triangles[i].Colour,
                        result_point_1_x, result_point_1_y, result_point_1_z,
                        result_point_2_x, result_point_2_y, result_point_2_z,
                        result_point_3_x, result_point_3_y, result_point_3_z);
                }
                else
                {
                    // Scale the texture co-ordinates
                    int width = face.Texture_Object.File.Width - 1;
                    int height = face.Texture_Object.File.Height - 1;

                    // AFTERWARDS?
                    int result_texture_point_1_x = Round_To_Int(face.T1.X * width);
                    int result_texture_point_1_y = Round_To_Int(face.T1.Y * height);
                    int result_texture_point_2_x = Round_To_Int(face.T2.X * width);
                    int result_texture_point_2_y = Round_To_Int(face.T2.Y * height);
                    int result_texture_point_3_x = Round_To_Int(face.T3.X * width);
                    int result_texture_point_3_y = Round_To_Int(face.T3.Y * height);

                    Textured_Triangle(face.Texture_Object.File,
                        result_point_1_x, result_point_1_y, result_point_1_z, result_texture_point_1_x, result_texture_point_1_y,
                        result_point_2_x, result_point_2_y, result_point_2_z, result_texture_point_2_x, result_texture_point_2_y,
                        result_point_3_x, result_point_3_y, result_point_3_z, result_texture_point_3_x, result_texture_point_3_y);
                }
            }
            // RANGE TO DRAW X: [0,WIDTH-1] Y: [0,HEIGHT-1]
        }

        private int Queue_Clip_Face(Queue<Face> face_clip, Clipping_Plane[] clipping_planes, out Face[] new_triangles)
        {
            int no_triangles = 1;

            foreach (Clipping_Plane clipping_plane in clipping_planes)
            {
                while (no_triangles > 0)
                {
                    Face triangle = face_clip.Dequeue();
                    Face[] triangles = new Face[2];
                    int num_intersection_points = Clip_Face(clipping_plane.Point, clipping_plane.Normal, triangle, out triangles[0], out triangles[1]); //OUT?
                    for (int i = 0; i < num_intersection_points; i++) face_clip.Enqueue(triangles[i]);
                    no_triangles--;
                }
                no_triangles = face_clip.Count;
            }

            new_triangles = face_clip.ToArray();
            return no_triangles;
        }
    }
}

/*
    // Adjust colour based on lighting
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
                    Vector3D light_to_shape = new Vector3D(point_1 - light.World_Origin);
                    if (light_to_shape.Angle(light.World_Direction) > ((Spotlight)light).Angle || light_to_shape * light.World_Direction > ((Spotlight)light).Distance) continue;
                    true_intensity = Math.Max(0, -light.World_Direction * normal) * light.Intensity;
                    break;
                case "Ambient_Light":
                    break;
            }
            double scaled_intensity = true_intensity / max_intensity;

            if (face.Texture == null)
            {

            }

            byte new_red = (byte)Round_To_Int((face.Colour.R + light.Colour.R) * 255 / 510 * scaled_intensity);
            byte new_green = (byte)Round_To_Int((face.Colour.G + light.Colour.G) * 255 / 510 * scaled_intensity);
            byte new_blue = (byte)Round_To_Int((face.Colour.B + light.Colour.B) * 255 / 510 * scaled_intensity);

            face_colour = Color.FromArgb(face.Colour.A, new_red, new_green, new_blue);
        }
    }
*/