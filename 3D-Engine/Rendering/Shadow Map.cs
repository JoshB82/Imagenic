using _3D_Engine.Rendering;

using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        public void Generate_Shadow_Map(Light light)
        {
            foreach (Mesh mesh in Meshes)
            {
                if (mesh.Visible && mesh.Draw_Faces)
                {
                    Matrix4x4 model_to_light_view = light.World_to_Light_View * mesh.Model_to_World;

                    foreach (Face face in mesh.Faces)
                    {
                        if (face.Visible)
                        {
                            Calculate_Depth
                            (
                                face,
                                mesh.Dimension,
                                ref model_to_light_view,
                                light
                            );
                        }
                    }
                }
            }
        }
            
        private void Calculate_Depth(Face face, int dimension,
            ref Matrix4x4 model_to_light_view,
            Light light)
        {
            // Reset the vertices to model space values
            face.Reset_Vertices();

            // Move the face from model space to light view space
            face.Apply_Matrix(model_to_light_view);

            if (dimension == 3)
            {
                // Discard the face if it is not visible from the light's point of view
                if ((Vector3D)face.p1 * Vector3D.Normal_From_Plane((Vector3D)face.p1, (Vector3D)face.p2, (Vector3D)face.p3) >= 0) return;
            }

            // Clip the face in light-view space
            Queue<Face> face_clip = new Queue<Face>();
            face_clip.Enqueue(face);
            if (!Clipping.ClipFaces(face_clip, light.Light_View_Clipping_Planes)) return;

            // Move the new triangles from light-view space to screen space, including a correction for perspective
            foreach (Face clipped_face in face_clip)
            {
                clipped_face.Apply_Matrix(light.Light_View_to_Light_Screen);

                if (light is Point_Light or Spotlight)
                {
                    clipped_face.p1 /= clipped_face.p1.w;
                    clipped_face.p2 /= clipped_face.p2.w;
                    clipped_face.p3 /= clipped_face.p3.w;
                }
            }

            // Clip the face in screen space
            if (Settings.Screen_Space_Clip && !Clipping.ClipFaces(face_clip, Camera.Camera_Screen_Clipping_Planes)) return;

            foreach (Face clipped_face in face_clip)
            {
                // Don't draw anything if the face is flat
                if ((clipped_face.p1.x == clipped_face.p2.x && clipped_face.p2.x == clipped_face.p3.x) ||
                    (clipped_face.p1.y == clipped_face.p2.y && clipped_face.p2.y == clipped_face.p3.y))
                { continue; }

                // Move the new triangles from light-screen space to light-window space
                clipped_face.Apply_Matrix(light.Light_Screen_to_Light_Window);
                
                // Round the vertices
                int x1 = clipped_face.p1.x.Round_to_Int();
                int y1 = clipped_face.p1.y.Round_to_Int();
                float z1 = clipped_face.p1.z;
                int x2 = clipped_face.p2.x.Round_to_Int();
                int y2 = clipped_face.p2.y.Round_to_Int();
                float z2 = clipped_face.p2.z;
                int x3 = clipped_face.p3.x.Round_to_Int();
                int y3 = clipped_face.p3.y.Round_to_Int();
                float z3 = clipped_face.p3.z;

                // Sort the vertices by their y-co-ordinate
                Sort_By_Y
                (
                    ref x1, ref y1, ref z1,
                    ref x2, ref y2, ref z2,
                    ref x3, ref y3, ref z3
                );

                // Interpolate each point in the triangle
                Interpolate_Triangle
                (
                    light, Mesh_Depth_From_Light,
                    x1, y1, z1,
                    x2, y2, z2,
                    x3, y3, z3
                );
            }
        }

        private static void Mesh_Depth_From_Light(object @object, int x, int y, float z)
        {
            Light light = @object as Light;

            if (x < light.Shadow_Map_Width && y < light.Shadow_Map_Height)
            {
                if (z.Approx_Less_Than(light.Shadow_Map.Values[x][y], 1E-4f))
                {
                    light.Shadow_Map.Values[x][y] = z;
                }
            }
        }
    }
}