﻿/*
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
        private static bool Clip_Edges(Clipping_Plane[] clipping_planes, ref Vector4D point_1, ref Vector4D point_2)
        {
            foreach (Clipping_Plane clipping_plane in clipping_planes)
            {
                if (!Clip_Edge(clipping_plane.Point, clipping_plane.Normal, ref point_1, ref point_2)) return false;
            }
            return true;
        }

        private static bool Clip_Edge(Vector3D plane_point, Vector3D plane_normal, ref Vector4D point_1, ref Vector4D point_2)
        {
            float point_1_distance = Point_Distance_From_Plane((Vector3D)point_1, plane_point, plane_normal);
            float point_2_distance = Point_Distance_From_Plane((Vector3D)point_2, plane_point, plane_normal);

            if (point_1_distance >= 0)
            {
                if (point_2_distance < 0)
                {
                    // One point is on the inside, the other on the outside, so clip the line
                    point_2 = Line_Intersect_Plane((Vector3D)point_1, (Vector3D)point_2, plane_point, plane_normal, out _);
                }
                // If above condition fails, both points are on the inside, so return line unchanged
                return true;
            }
            
            if (point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector3D intersection = Line_Intersect_Plane((Vector3D)point_2, (Vector3D)point_1, plane_point, plane_normal, out _);
                point_1 = point_2;
                point_2 = intersection;
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
                int no_faces = face_clip_queue.Count;
                while (no_faces-- > 0) Clip_Face(face_clip_queue.Dequeue(), face_clip_queue, clipping_plane.Point, clipping_plane.Normal);
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

            if (Point_Distance_From_Plane((Vector3D)face_to_clip.p1, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.p1;
                inside_texture_points[inside_point_count++] = face_to_clip.t1;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.p1;
                outside_texture_points[outside_point_count++] = face_to_clip.t1;
            }

            if (Point_Distance_From_Plane((Vector3D)face_to_clip.p2, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.p2;
                inside_texture_points[inside_point_count++] = face_to_clip.t2;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.p2;
                outside_texture_points[outside_point_count++] = face_to_clip.t2;
            }

            if (Point_Distance_From_Plane((Vector3D)face_to_clip.p3, plane_point, plane_normal) >= 0)
            {
                inside_points[inside_point_count] = face_to_clip.p3;
                inside_texture_points[inside_point_count++] = face_to_clip.t3;
            }
            else
            {
                outside_points[outside_point_count] = face_to_clip.p3;
                outside_texture_points[outside_point_count] = face_to_clip.T3;
            }

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid faces to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller face is needed
                    Vector4D intersection_1 = Line_Intersect_Plane((Vector3D)inside_points[0], (Vector3D)outside_points[0], plane_point, plane_normal, out float d1);
                    Vector4D intersection_2 = Line_Intersect_Plane((Vector3D)inside_points[0], (Vector3D)outside_points[1], plane_point, plane_normal, out float d2);

                    Face face_1;
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d2 + inside_texture_points[0];

                        face_1 = new Face(inside_points[0], intersection_1, intersection_2, inside_texture_points[0], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object) { Has_Texture = true };
                    }
                    else
                    {
                        face_1 = new Face(inside_points[0], intersection_1, intersection_2) { Colour = face_to_clip.Colour };
                    }

                    face_clip_queue.Enqueue(face_1);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two faces
                    intersection_1 = Line_Intersect_Plane((Vector3D)inside_points[0], (Vector3D)outside_points[0], plane_point, plane_normal, out d1);
                    intersection_2 = Line_Intersect_Plane((Vector3D)inside_points[1], (Vector3D)outside_points[0], plane_point, plane_normal, out d2);

                    Face face_2;
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d2 + inside_texture_points[1];

                        face_1 = new Face(inside_points[0], intersection_1, inside_points[1], inside_texture_points[0], t_intersection_1, inside_texture_points[1], face_to_clip.Texture_Object) { Has_Texture = true };
                        face_2 = new Face(inside_points[1], intersection_1, intersection_2, inside_texture_points[1], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object) { Has_Texture = true };
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
                    // All points are on the inside, so enqueue the face unchanged
                    face_clip_queue.Enqueue(face_to_clip);
                    break;
            }
        }
    }
}