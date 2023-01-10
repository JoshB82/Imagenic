/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a spotlight.
 */

using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

public sealed class Spotlight : Light
{
    #region Constructors

    public Spotlight(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float strength, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar)
    {
        Strength = strength;
    }

    #endregion

    /*
    internal override void Calculate_Light_View_Clipping_Planes()
    {
        float semi_width = (float)shadow_map_width / 2, semi_height = (float)shadow_map_height / 2, z_ratio = shadow_map_z_far / shadow_map_z_near;

        Vector3D near_top_left_point = new Vector3D(-semi_width, semi_height, shadow_map_z_near);
        Vector3D near_top_right_point = new Vector3D(semi_width, semi_height, shadow_map_z_near);
        Vector3D near_bottom_left_point = new Vector3D(-semi_width, -semi_height, shadow_map_z_near);

        Vector3D far_top_right_point = new Vector3D(semi_width * z_ratio, semi_height * z_ratio, shadow_map_z_far);
        Vector3D far_bottom_left_point = new Vector3D(-semi_width * z_ratio, -semi_height * z_ratio, shadow_map_z_far);
        Vector3D far_bottom_right_point = new Vector3D(semi_width * z_ratio, -semi_height * z_ratio, shadow_map_z_far);

        Vector3D bottom_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_right_point, far_bottom_left_point);
        Vector3D top_normal = Vector3D.Normal_From_Plane(near_top_left_point, far_top_right_point, near_top_right_point);
        Vector3D left_normal = Vector3D.Normal_From_Plane(near_bottom_left_point, far_bottom_left_point, near_top_left_point);
        Vector3D right_normal = Vector3D.Normal_From_Plane(near_top_right_point, far_top_right_point, far_bottom_right_point);

        Light_View_Clipping_Planes = new Clipping_Plane[]
        {
            new Clipping_Plane(near_bottom_left_point, bottom_normal), // Bottom
            new Clipping_Plane(near_top_left_point, top_normal), // Top
            new Clipping_Plane(near_top_left_point, left_normal), // Left
            new Clipping_Plane(near_top_right_point, right_normal), // Right
            new Clipping_Plane(near_top_left_point, Vector3D.Unit_Z), // Near
            new Clipping_Plane(far_top_right_point, Vector3D.Unit_Negative_Z) // Far
        };
    }
    */
}