using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
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

        private static bool Clip_Edge(Vector3D plane_point, Vector3D plane_normal, Edge e)
        {
            Vector3D point_1 = new Vector3D(e.P1), point_2 = new Vector3D(e.P2);
            float point_1_distance = Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal);
            float point_2_distance = Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal);

            if (point_1_distance >= 0 && point_2_distance >= 0)
            {
                // Both points are on the inside, so return line unchanged
                return true;
            }
            if (point_1_distance >= 0 && point_2_distance < 0)
            {
                // One point is on the inside, the other on the outside, so clip the line
                Vector3D intersection = Vector3D.Line_Intersect_Plane(point_1, point_2, plane_point, plane_normal, out _);
                e.P1 = e.P1;
                e.P2 = new Vector4D(intersection);
                return true;
            }
            if (point_1_distance < 0 && point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector3D intersection = Vector3D.Line_Intersect_Plane(point_2, point_1, plane_point, plane_normal, out _);
                e.P1 = e.P2;
                e.P2 = new Vector4D(intersection);
                return true;
            }
            // Both points are on the outside, so discard the line
            return false;
        }

        // check clockwise/anticlockwise stuff
        // source as well
        private static void Clip_Face(Face face_to_clip, Queue<Face> face_clip_queue, Vector3D plane_point, Vector3D plane_normal)
        {
            Vector3D point_1 = new Vector3D(face_to_clip.P1), point_2 = new Vector3D(face_to_clip.P2), point_3 = new Vector3D(face_to_clip.P3);
            int inside_point_count = 0;
            List<Vector3D> inside_points = new List<Vector3D>(3);
            List<Vector3D> outside_points = new List<Vector3D>(3);
            List<Vector3D> inside_texture_points = new List<Vector3D>(3);
            List<Vector3D> outside_texture_points = new List<Vector3D>(3);

             if (Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_1);
                inside_texture_points.Add(face_to_clip.T1);
            }
            else
            {
                outside_points.Add(point_1);
                outside_texture_points.Add(face_to_clip.T1);
            }

            if (Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_2);
                inside_texture_points.Add(face_to_clip.T2);
            }
            else
            {
                outside_points.Add(point_2);
                outside_texture_points.Add(face_to_clip.T2);
            }

            if (Vector3D.Point_Distance_From_Plane(point_3, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_3);
                inside_texture_points.Add(face_to_clip.T3);
            }
            else
            {
                outside_points.Add(point_3);
                outside_texture_points.Add(face_to_clip.T3);
            }

            Vector3D intersection_1, intersection_2;
            float d1, d2;

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to enqueue
                    break;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    Face new_face;

                    intersection_1 = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1);
                    intersection_2 = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal, out d2);
                    
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d2 + inside_texture_points[0];

                        new_face = new Face(new Vector4D(inside_points[0]), new Vector4D(intersection_1), new Vector4D(intersection_2), inside_texture_points[0], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object);
                    }
                    else
                    {
                        new_face = new Face(new Vector4D(inside_points[0]), new Vector4D(intersection_1), new Vector4D(intersection_2)) { Colour = face_to_clip.Colour };
                    }

                    face_clip_queue.Enqueue(new_face);
                    break;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    Face face_1, face_2;

                    intersection_1 = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1);
                    intersection_2 = Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal, out d2);
                    
                    if (face_to_clip.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d2 + inside_texture_points[1];

                        face_1 = new Face(new Vector4D(inside_points[0]), new Vector4D(intersection_1), new Vector4D(inside_points[1]), inside_texture_points[0], t_intersection_1, inside_texture_points[1], face_to_clip.Texture_Object);
                        face_2 = new Face(new Vector4D(inside_points[1]), new Vector4D(intersection_1), new Vector4D(intersection_2), inside_texture_points[1], t_intersection_1, t_intersection_2, face_to_clip.Texture_Object);
                    }
                    else
                    {
                        face_1 = new Face(new Vector4D(inside_points[0]), new Vector4D(intersection_1), new Vector4D(inside_points[1])) { Colour = face_to_clip.Colour };
                        face_2 = new Face(new Vector4D(inside_points[1]), new Vector4D(intersection_1), new Vector4D(intersection_2)) { Colour = face_to_clip.Colour };
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