/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a distant light.
 */

using _3D_Engine.Entities.SceneObjects;
using _3D_Engine.Entities.SceneObjects.RenderingObjects.Lights;
using Imagenic.Core.Entities.SceneObjects.Meshes;
using System;
using System.Drawing;

namespace Imagenic.Core.Entities;

/// <summary>
/// Encapsulates creation of a <see cref="DistantLight"/>.
/// </summary>
public sealed class DistantLight : Light
{
    #region Fields and Properties

    public override float ViewWidth
    {
        get => base.ViewWidth;
        set
        {
            base.ViewWidth = value;

            viewToScreen.m00 = 2 / base.ViewWidth;
            // Update left and right clipping planes
            ViewClippingPlanes[0].Point.x = -base.ViewWidth / 2;
            ViewClippingPlanes[3].Point.x = base.ViewWidth / 2;
        }
    }

    public override float ViewHeight
    {
        get => base.ViewHeight;
        set
        {
            base.ViewHeight = value;

            // Update view-to-screen matrix
            viewToScreen.m11 = 2 / base.ViewHeight;

            // Update top and bottom clipping planes
            ViewClippingPlanes[1].Point.y = -base.ViewHeight / 2;
            ViewClippingPlanes[4].Point.y = base.ViewHeight / 2;
        }
    }

    #endregion

    #region Constructors

    public DistantLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp) : this(origin, directionForward, directionUp, Default.LightStrength, Default.ShadowMapViewWidth, Default.ShadowMapViewHeight, Default.ShadowMapZNear, Default.ShadowMapZFar, Default.ShadowMapRenderWidth, Default.ShadowMapRenderHeight) { }

    public DistantLight(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength) : this(origin, directionForward, directionUp, strength, Default.ShadowMapViewWidth, Default.ShadowMapViewHeight, Default.ShadowMapZNear, Default.ShadowMapZFar, Default.ShadowMapRenderWidth, Default.ShadowMapRenderHeight) { }

    public DistantLight(Vector3D worldOrigin, Orientation worldOrientation, float strength, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar)
    {
        Strength = strength;

        string[] iconObjData = Properties.Resources.DistantLight.Split(Environment.NewLine);
        Icon = new Custom(worldOrigin, directionForward, directionUp, iconObjData) { Dimension = 3 };
        Icon.ColourAllSolidFaces(Color.DarkCyan);
        Icon.Scale(5);
    }

    public static DistantLight DistantLightAngle(Vector3D origin, Vector3D directionForward, Vector3D directionUp, float strength, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => new DistantLight(origin, directionForward, directionUp, strength, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar, renderWidth, renderHeight);

    public DistantLight(Vector3D origin, SceneEntity pointedAt, Vector3D directionUp) : this(origin, pointedAt.WorldOrigin - origin, directionUp) { }

    public DistantLight(Vector3D origin, SceneEntity pointedAt, Vector3D directionUp, float strength) : this(origin, pointedAt.WorldOrigin - origin, directionUp, strength) { }

    public DistantLight(Vector3D origin, SceneEntity pointedAt, Vector3D directionUp, float strength, float viewWidth, float viewHeight, float zNear, float zFar, int renderWidth, int renderHeight) : this(origin, pointedAt.WorldOrigin - origin, directionUp, strength, viewWidth, viewHeight, zNear, zFar, renderWidth, renderHeight) { }

    public static DistantLight DistantLightAngle(Vector3D origin, SceneEntity pointedAt, Vector3D directionUp, float strength, float fovX, float fovY, float zNear, float zFar, int renderWidth, int renderHeight) => DistantLightAngle(origin, pointedAt.WorldOrigin - origin, directionUp, strength, fovX, fovY, zNear, zFar, renderWidth, renderHeight);

    #endregion
}