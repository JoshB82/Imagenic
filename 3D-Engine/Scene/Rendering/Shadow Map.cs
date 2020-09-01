using System;
using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        // other clipping?
        public void Generate_Shadow_Map(Light light)
        {
            foreach (Mesh mesh in Meshes)
            {
                if (mesh.Draw_Faces)
                {
                    foreach (Face face in mesh.Faces)
                    {
                        if (face.Visible) Calculate_Depth(mesh.Model_to_World, face, light);
                    }
                }
            }
        }
            
        private void Calculate_Depth(Matrix4x4 model_to_world, Face face, Light light)
        {
            // Reset the vertices to model space values
            face.Reset_Vertices();

            // Move face from model space to world space
            face.Apply_Matrix(model_to_world);

            // Move the face from world space to light-view space
            face.Apply_Matrix(light.World_to_Light_View);

            // Clip the face in light-view space
            Queue<Face> face_clip = new Queue<Face>();
            face_clip.Enqueue(face);

            if (Settings.View_Space_Clip && Queue_Clip_Face(face_clip, light.Light_View_Clipping_Planes) == 0) return;

            // Move the new triangles from light-view space to screen space, including a correction for perspective
            foreach (Face clipped_face in face_clip)
            {
                clipped_face.Apply_Matrix(light.Light_View_to_Light_Screen);

                if (light.GetType().Name == "Point_Light")
                {
                    clipped_face.P1 /= clipped_face.P1.W;
                    clipped_face.P2 /= clipped_face.P2.W;
                    clipped_face.P3 /= clipped_face.P3.W;
                }
            }

            // Clip the face in screen space
            if (Settings.Screen_Space_Clip && Queue_Clip_Face(face_clip, camera_screen_clipping_planes) == 0) return;

            foreach (Face new_light_screen_triangle in face_clip)
            {
                // Move the new triangles from light-screen space to window space
                new_light_screen_triangle.Apply_Matrix(screen_to_window);
            }

            // Assign the correct method that will be called for each point in the triangle
            Action<object, int, int, double> depth = Mesh_Depth_From_Light;

            foreach (Face new_face in face_clip)
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

        private void Mesh_Depth_From_Light(object @object, int x, int y, double z)
        {
            Light light = (Light)@object;  // Why the explicit cast?

            try
            {
                if (z < light.Shadow_Map[x][y])
                {
                    light.Shadow_Map[x][y] = z;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("Attempted to check points outside the shadow map.");
            }
        }
    }
}