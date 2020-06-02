using System.Collections.Generic;

namespace _3D_Engine
{
    public sealed partial class Scene
    {
        private static bool Clip_Edge(Vector3D plane_point, Vector3D plane_normal, ref Edge e)
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
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_1, point_2, plane_point, plane_normal, out double d));
                e.P1 = e.P1;
                e.P2 = intersection;
                return true;
            }
            if (point_1_distance < 0 && point_2_distance >= 0)
            {
                // One point is on the outside, the other on the inside, so clip the line
                Vector4D intersection = new Vector4D(Vector3D.Line_Intersect_Plane(point_2, point_1, plane_point, plane_normal, out double d));
                e.P1 = e.P2;
                e.P2 = intersection;
                return true;
            }
            // Both points are on the outside, so discard the line
            return false;
        }

        private static int Clip_Face(Vector3D plane_point, Vector3D plane_normal, Face f, out Face f1, out Face f2)
        {
            f1 = new Face(); f2 = new Face();
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

            Vector4D first_intersection, second_intersection;
            double d1, d2;
            // check clockwise/anticlockwise stuff

            switch (inside_point_count)
            {
                case 0:
                    // All points are on the outside, so no valid triangles to return
                    return 0;
                case 1:
                    // One point is on the inside, so only a smaller triangle is needed
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[1], plane_point, plane_normal, out d2));
                    if (f.Texture == null)
                    {
                        f1 = new Face(new Vector4D(inside_points[0]), first_intersection, second_intersection, f.Colour);
                    }
                    else
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[1] - inside_texture_points[0]) * d2 + inside_texture_points[0];
                        f1 = new Face(new Vector4D(inside_points[0]), first_intersection, second_intersection, inside_texture_points[0], t_intersection_1, t_intersection_2, f.Texture);
                    }
                    return 1;
                case 2:
                    // Two points are on the inside, so a quadrilateral is formed and split into two triangles
                    first_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[0], outside_points[0], plane_point, plane_normal, out d1));
                    second_intersection = new Vector4D(Vector3D.Line_Intersect_Plane(inside_points[1], outside_points[0], plane_point, plane_normal, out d2));
                    if (f.Texture == null)
                    {
                        f1 = new Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), first_intersection, f.Colour);
                        f2 = new Face(new Vector4D(inside_points[1]), second_intersection, first_intersection, f.Colour);
                    }
                    else
                    {
                        Vector3D t_intersection_1 = (outside_texture_points[0] - inside_texture_points[0]) * d1 + inside_texture_points[0];
                        Vector3D t_intersection_2 = (outside_texture_points[0] - inside_texture_points[1]) * d2 + inside_texture_points[1];
                        f1 = new Face(new Vector4D(inside_points[0]), new Vector4D(inside_points[1]), first_intersection, inside_texture_points[0], inside_texture_points[1], t_intersection_1, f.Texture);
                        f2 = new Face(new Vector4D(inside_points[1]), first_intersection, second_intersection, inside_texture_points[1], t_intersection_1, t_intersection_2, f.Texture);
                    }
                    return 2;
                case 3:
                    // All points are on the inside, so return the triangle unchanged
                    f1 = f;
                    return 1;
            }
            return 0;
        }
    }
}