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
    #region Fields and Properties

    public override float ViewWidth
    {
        get => base.ViewWidth;
        set
        {
            base.ViewWidth = value;

            viewToScreen.m00 = 2 * ZNear / base.ViewWidth;
            float semiWidth = base.ViewWidth / 2, semiHeight = ViewHeight / 2;
            ViewClippingPlanes[0].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, -semiHeight, ZNear), new Vector3D(-semiWidth, semiHeight, ZNear));
            ViewClippingPlanes[3].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, semiHeight, ZNear), new Vector3D(semiWidth, -semiHeight, ZNear));
        }
    }

    public override float ViewHeight
    {
        get => base.ViewHeight;
        set
        {
            base.ViewHeight = value;

            // Update view-to-screen matrix
            viewToScreen.m11 = 2 * ZNear / base.ViewHeight;

            // Update top and bottom clipping planes
            float semiWidth = base.ViewWidth / 2, semiHeight = base.ViewHeight / 2;
            ViewClippingPlanes[4].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(-semiWidth, semiHeight, ZNear), new Vector3D(semiWidth, semiHeight, ZNear));
            ViewClippingPlanes[1].Normal = Vector3D.NormalFromPlane(Vector3D.Zero, new Vector3D(semiWidth, -semiHeight, ZNear), new Vector3D(-semiWidth, -semiHeight, ZNear));
        }
    }

    public override float ZNear
    {
        get => base.ZNear;
        set
        {
            base.ZNear = value;

            // Update view-to-screen matrix
            viewToScreen.m00 = 2 * base.ZNear / base.ViewWidth;
            viewToScreen.m11 = 2 * base.ZNear / base.ViewHeight;
            viewToScreen.m22 = (base.ZFar + base.ZNear) / (base.ZFar - base.ZNear);
            viewToScreen.m23 = -(2 * base.ZFar * base.ZNear) / (base.ZFar - base.ZNear);

            // Update near clipping plane
            ViewClippingPlanes[2].Point.z = base.ZNear;
        }
    }

    public override float ZFar
    {
        get => base.ZFar;
        set
        {
            base.ZNear = value;

            // Update view-to-screen matrix
            viewToScreen.m22 = (base.ZFar + base.ZNear) / (base.ZFar - base.ZNear);
            viewToScreen.m23 = -(2 * base.ZFar * base.ZNear) / (base.ZFar - base.ZNear);

            // Update far clipping plane
            ViewClippingPlanes[5].Point.z = base.ZFar;
        }
    }

    #endregion

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