/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines an orthogonal camera.
 */

using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// Defines a <see cref="OrthogonalCamera"/>.
/// </summary>
public sealed class OrthogonalCamera : Camera
{
    #region Fields and Properties

    #if DEBUG

    private protected override IMessageBuilder<OrthogonalCameraCreatedMessage>? MessageBuilder => (IMessageBuilder<OrthogonalCameraCreatedMessage>?)base.MessageBuilder;

    #endif

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

    public override float ZNear
    {
        get => base.ZNear;
        set
        {
            base.ZNear = value;

            // Update view-to-screen matrix
            viewToScreen.m22 = 2 / (base.ZFar - base.ZNear);
            viewToScreen.m23 = -(base.ZFar + base.ZNear) / (base.ZFar - base.ZNear);

            // Update near clipping plane
            ViewClippingPlanes[2].Point.z = base.ZNear;
        }
    }

    public override float ZFar
    {
        get => base.ZFar;
        set
        {
            base.ZFar = value;

            // Update view-to-screen matrix
            viewToScreen.m22 = 2 / (base.ZFar - base.ZNear);
            viewToScreen.m23 = -(base.ZFar + base.ZNear) / (base.ZFar - base.ZNear);

            // Update far clipping plane
            ViewClippingPlanes[5].Point.z = base.ZFar;
        }
    }

    #endregion

    #region Constructors

    public OrthogonalCamera(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation) : this(worldOrigin, worldOrientation, Defaults.Default.CameraWidth, Defaults.Default.CameraHeight, Defaults.Default.CameraZNear, Defaults.Default.CameraZFar) { }

    public OrthogonalCamera(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float viewWidth, float viewHeight, float zNear, float zFar) : base(worldOrigin, worldOrientation, viewWidth, viewHeight, zNear, zFar
        #if DEBUG
        , MessageBuilder<OrthogonalCameraCreatedMessage>.Instance()
        #endif
        )
    { }

    public static OrthogonalCamera OrthogonalCameraAngle(Vector3D worldOrigin, [DisallowNull] Orientation worldOrientation, float fovX, float fovY, float zNear, float zFar) => new OrthogonalCamera(worldOrigin, worldOrientation, Tan(fovX / 2) * zNear * 2, Tan(fovY / 2) * zNear * 2, zNear, zFar);

    public OrthogonalCamera(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp) : this(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp)) { }

    public OrthogonalCamera(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp, float viewWidth, float viewHeight, float zNear, float zFar) : this(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp), viewWidth, viewHeight, zNear, zFar) { }

    public static OrthogonalCamera OrthogonalCameraAngle(Vector3D worldOrigin, [DisallowNull] TranslatableEntity pointedAt, Vector3D directionUp, float fovX, float fovY, float zNear, float zFar) => OrthogonalCameraAngle(worldOrigin, GenerateOrientation(worldOrigin, pointedAt, directionUp), fovX, fovY, zNear, zFar);

    #endregion

    #region Methods




    /*
    internal override void ProcessLighting(Group sceneToRender)
    {
        Matrix4x4 windowToWorld = ViewToWorld * ScreenToView * WindowToScreen;

        for (int x = 0; x < RenderWidth; x++)
        {
            for (int y = 0; y < RenderHeight; y++)
            {
                if (zBuffer.Values[x][y] != outOfBoundsValue)
                {
                    // Move the point from window space to world space
                    Vector4D worldSpacePoint = windowToWorld * new Vector4D(x, y, zBuffer.Values[x][y], 1);

                    // Apply lighting
                    ApplyLighting(worldSpacePoint, ref colourBuffer.Values[x][y], x, y, sceneToRender);
                }
            }
        }
    }
    */
    #endregion
}