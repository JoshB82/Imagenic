/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Provides static methods for clipping edges and faces against specified planes.
 */

using System.Collections.Generic;
using static _3D_Engine.Vector3D;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private static bool Clip_Edge(Vector3D plane_point, Vector3D plane_normal, Edge e)
        {
            float point_1_distance = Point_Distance_From_Plane(e.P1, plane_point, plane_normal);
            float point_2_distance = Point_Distance_From_Plane(e.P2, plane_point, plane_normal);

            if (point_1_distance >= 0)
            {
                if (point_2_distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the line
                    Vector3D intersection = Line_Intersect_Plane(e.P1, e.P2, plane_point, plane_normal, out _);
                    e.P2 = intersection;
                }
                // If above condition fails, both points are on the inside, so return line unchanged

                return true;
            }
            
            if (point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector3D intersection = Line_Intersect_Plane(e.P2, e.P1, plane_point, plane_normal, out _);
                e.P1 = e.P2;
                e.P2 = intersection;
                return true;
            }

            // Both points are on the outside, so discard the line
            return false;
        }

        //source!
        private static bool Clip_Faces_In_Queue(Queue<Face> face_clip_queue, Clipping_Plane[] clipping_planes)
        {
            foreach (Clipping_Plane clipping_plane in clipping_planes)
            {
                int no_triangles = face_clip_queue.Count;

                while (no_triangles-- > 0)
                {
                    Clip_Face(face_clip_queue.Dequeue(), face_clip_queue, clipping_plane.Point, clipping_plane.Normal);
                }
            }

            return face_clip_queue.Count > 0;
        }

        // check clockwise/anticlockwise stuff
        // source
        private static void Clip_Face(Face face_to_clip, Queue<Face> face_clip_queue, Vector3D plane_point, Vector3D plane_normal)
        {
            Vector4D[] inside_points = new Vector4D[3], outside_points = new Vector4D[3];
            Vector3D[] inside_texture_points = new Vector3D[3], outside_texture_points = new Vector3D[3];
            int inside_point_count = 0, outside_point_count = 0;

            if (Point_Distance_From_Plane(face_to_clip.P1, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.P1;
                inside_texture_points[inside_point_count++] = face_to_clip.T1;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.P1;
                outside_texture_points[outside_point_count++] = face_to_clip.T1;
            }

            if (Point_Distance_From_Plane(face_to_clip.P2, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.P2;
                inside_texture_points[inside_point_count++] = face_to_clip.T2;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.P2;
                outside_texture_points[outside_point_count++] = face_to_clip.T2;
            }

            if (Point_Distance_From_Plane(face_to_clip.P3, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.P3;
                inside_texture_points[inside_point_count++] = face_to_clip.T3;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.P3;
                outside_texture_points[outside_point_count] = face_to_clip.T3;
            }

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    Vector4D intersection_1 = Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out float d1);
                    Vector4D intersection_2 = Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal, out float d2);

                    Face face_1;
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d2 + inside_texture_points[0];

                        face_1 = new Face(inside_points[0], intersection_1, intersection_2, inside_texture_points[0], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object);
                    }
                    else
                    {
                        face_1 = new Face(inside_points[0], intersection_1, intersection_2) { Colour = face_to_clip.Colour };
                    }

                    face_clip_queue.Enqueue(face_1);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    intersection_1 = Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1);
                    intersection_2 = Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal, out d2);

                    Face face_2;
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d2 + inside_texture_points[1];

                        face_1 = new Face(inside_points[0], intersection_1, inside_points[1], inside_texture_points[0], t_intersection_1, inside_texture_points[1], face_to_clip.Texture_Object);
                        face_2 = new Face(inside_points[1], intersection_1, intersection_2, inside_texture_points[1], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object);
                    }
                    else
                    {
                        face_1 = new Face(inside_points[0], intersection_1, inside_points[1]) { Colour = face_to_clip.Colour };
                        face_2 = new Face(inside_points[1], intersection_1, intersection_2) { Colour = face_to_clip.Colour };
                    }

                    face_clip_queue.Enqueue(face_1);
                    face_clip_queue.Enqueue(face_2);
                    break;
                case 3:
                    // All points are on the inside, so enqueue the triangle unchanged
                    face_clip_queue.Enqueue(face_to_clip);
                    break;
            }
        }
    }
}