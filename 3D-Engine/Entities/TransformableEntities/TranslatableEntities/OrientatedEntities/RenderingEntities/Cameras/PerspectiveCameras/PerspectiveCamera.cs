/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Encapsulates creation of a perspective camera.
 */

using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// Defines a <see cref="PerspectiveCamera"/>.
/// </summary>
public sealed class PerspectiveCamera : Camera
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

    #endregion

    #region Constructors

    public PerspectiveCamera(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation) : this(worldOrigin, worldOrientation, Defaults.Default.CameraWidth, Defaults.Default.CameraHeight, Defaults.Default.CameraZNear, Defaults.Default.CameraZFar) { }

    public PerspectiveCamera(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar
        #if DEBUG
        , MessageBuilder<PerspectiveCameraCreatedMessage>.Instance()
        #endif
        )
    {
        ScreenToView = Matrix4x4.Zero;
        ScreenToView.m23 = 1;
    }

    public static PerspectiveCamera PerspectiveCameraAngle(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float fovX, float fovY, float zNear, float zFar) => new(worldOrigin, worldOrientation, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

    public PerspectiveCamera(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp) : this(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp)) { }

    public PerspectiveCamera(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar) : this(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp), viewWidth, viewHeight, zNear, zFar) { }

    public static PerspectiveCamera PerspectiveCameraAngle(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => PerspectiveCameraAngle(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp), fovX, fovY, zNear, zFar);

    #endregion

    #region Methods
    /*
    internal override void ProcessLighting(Group sceneToRender)
    {
        for (int x = 0; x < RenderWidth; x++)
        {
            for (int y = 0; y < RenderHeight; y++)
            {
                if (zBuffer.Values[x][y] != outOfBoundsValue)
                {
                    // Move the point from window space to screen space
                    Vector4D screenSpacePoint = WindowToScreen * new Vector4D(x, y, zBuffer.Values[x][y], 1);

                    // Move the point from screen space to world space and apply lighting
                    screenSpacePoint *= 2 * ZNear * ZFar / (ZNear + ZFar - screenSpacePoint.z * (ZFar - ZNear));
                    ApplyLighting(ScreenToWorld * screenSpacePoint, ref colourBuffer.Values[x][y], x, y, sceneToRender);
                }
            }
        }
    }
    */
    #endregion
}