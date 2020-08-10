using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private static bool Clip_Edge(Vector3D plane_point, Vector3D plane_normal, Edge e)
        {
            Vector3D point_1 = new Vector3D(e.P1), point_2 = new Vector3D(e.P2);
            double point_1_distance = Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal);
            double point_2_distance = Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal);

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
        private static (int, Face[]) Clip_Face(Vector3D plane_point, Vector3D plane_normal, Face f)
        {
            Face[] new_triangles = new Face[2];
            Vector3D point_1 = new Vector3D(f.P1), point_2 = new Vector3D(f.P2), point_3 = new Vector3D(f.P3);
            int inside_point_count = 0;
            List<Vector3D> inside_points = new List<Vector3D>(3);
            List<Vector3D> outside_points = new List<Vector3D>(3);
            List<Vector3D> inside_texture_points = new List<Vector3D>(3);
            List<Vector3D> outside_texture_points = new List<Vector3D>(3);

            if (Vector3D.Point_Distance_From_Plane(point_1, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_1);
                inside_texture_points.Add(f.T1);
            }
            else
            {
                outside_points.Add(point_1);
                outside_texture_points.Add(f.T1);
            }

            if (Vector3D.Point_Distance_From_Plane(point_2, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_2);
                inside_texture_points.Add(f.T2);
            }
            else
            {
                outside_points.Add(point_2);
                outside_texture_points.Add(f.T2);
            }

            if (Vector3D.Point_Distance_From_Plane(point_3, plane_point, plane_normal) >= 0)
            {
                inside_point_count++;
                inside_points.Add(point_3);
                inside_texture_points.Add(f.T3);
            }
            else
            {
                outside_points.Add(point_3);
                outside_texture_points.Add(f.T3);
            }

            Vector3D first_intersection, second_intersection;
            double d1, d2;

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to return
                    return (0, new_triangles);
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    first_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1);
                    second_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal, out d2);
                    if (f.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d2 + inside_texture_points[0];
                        new_triangles[0] = new Face(new Vector4D(inside_points[0]), new Vector4D(first_intersection), new Vector4D(second_intersection), inside_texture_points[0], t_intersection_1, t_intersection_2, f.Texture_Object);
                    }
                    else
                    {
                        new_triangles[0] = new Face(new Vector4D(inside_points[0]), new Vector4D(first_intersection), new Vector4D(second_intersection)) { Colour = f.Colour };
                    }
                    
                    return (1, new_triangles);
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    first_intersection = Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1);
                    second_intersection = Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal, out d2);
                    if (f.Has_Texture)
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d2 + inside_texture_points[1];
                        new_triangles[0] = new Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), new Vector4D(first_intersection), inside_texture_points[0], inside_texture_points[1], t_intersection_1, f.Texture_Object);
                        new_triangles[1] = new Face(new Vector4D(inside_points[1]), new Vector4D(first_intersection), new Vector4D(second_intersection), inside_texture_points[1], t_intersection_1, t_intersection_2, f.Texture_Object);
                    }
                    else
                    {
                        new_triangles[0] = new Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), new Vector4D(first_intersection)) { Colour = f.Colour };
                        new_triangles[1] = new Face(new Vector4D(inside_points[1]), new Vector4D(second_intersection), new Vector4D(first_intersection)) { Colour = f.Colour };
                    }

                    return (2, new_triangles);
                case 3:
                    // All points are on the inside, so return the triangle unchanged
                    new_triangles[0] = f;
                    return (1, new_triangles);
            }
            return (0, new_triangles);
        }
    }
}