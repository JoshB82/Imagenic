/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a rendering object.
 */

using _3D_Engine.Constants;
using Imagenic.Core.Entities.TransformableEntities.TranslatableEntities.OrientatedEntities.RenderingEntities;
using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities;

/// <summary>
/// A <see cref="OrientatedEntity"/> that utilises rendering mechanisms.
/// </summary>
/// <remarks>This class inherits from the <see cref="SceneEntity"/> class.</remarks>
[Serializable]
public abstract partial class RenderingEntity : OrientatedEntity
{
    #region Fields and Properties

    #if DEBUG

    private protected override IMessageBuilder<RenderingEntityCreatedMessage>? MessageBuilder => (IMessageBuilder<RenderingEntityCreatedMessage>?)base.MessageBuilder;

    #endif

    // Clipping Planes
    internal ClippingPlane[] ViewClippingPlanes { get; set; }

    // Matrices
    public Matrix4x4 WorldToView { get; private set; }
    protected override void RegenerateModelToWorldMatrix()
    {
        base.RegenerateModelToWorldMatrix();
        WorldToView = ModelToWorld.Inverse();
    }

    protected Matrix4x4 viewToScreen;
    public Matrix4x4 ViewToScreen
    {
        get => viewToScreen;
        private set => viewToScreen = value;
    }

    // View Volume
    private float viewWidth, viewHeight, zNear, zFar;
    
    public virtual float ViewWidth
    {
        get => viewWidth;
        set
        {
            if (value == viewWidth) return;
            viewWidth = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The height of the <see cref="RenderingEntity">RenderingObject's</see> view/near plane.
    /// </summary>
    public virtual float ViewHeight
    {
        get => viewHeight;
        set
        {
            if (value == viewHeight) return;
            viewHeight = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The depth of the <see cref="RenderingEntity">RenderingObject's</see> view to the near plane.
    /// </summary>
    public virtual float ZNear
    {
        get => zNear;
        set
        {
            if (value == zNear) return;
            zNear = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /// <summary>
    /// The depth of the <see cref="RenderingEntity">RenderingObject's</see> view to the far plane.
    /// </summary>
    public virtual float ZFar
    {
        get => zFar;
        set
        {
            if (value == zFar) return;
            zFar = value;
            InvokeRenderEvent(RenderUpdate.NewRender & RenderUpdate.NewShadowMap);
        }
    }

    /*
    public virtual int RenderWidth
    {
        get => renderWidth;
        set
        {
            if (value == renderWidth) return;
            renderWidth = value;
            UpdateProperties();
            RequestNewRenders();
        }
    }
    public virtual int RenderHeight
    {
        get => renderHeight;
        set
        {
            if (value == renderHeight) return;
            renderHeight = value;
            UpdateProperties();
            RequestNewRenders();
        }
    }*/

    // Buffers

    internal List<Edge> VolumeEdges = new();

    private VolumeOutline volumeStyle = VolumeOutline.None;

    /// <summary>
    /// Determines how the <see cref="RenderingEntity">RenderingObject's</see> view volume outline is drawn.
    /// </summary>
    public VolumeOutline VolumeStyle
    {
        get => volumeStyle;
        set
        {
            if (value == volumeStyle) return;
            volumeStyle = value;
            RequestNewRenders();

            VolumeEdges.Clear();

            
        }
    }

    #endregion

    #region Constructors

    internal RenderingEntity(Vector3D worldOrigin,
                             Orientation worldOrientation,
                             float viewWidth,
                             float viewHeight,
                             float zNear,
                             float zFar
        #if DEBUG
        , IMessageBuilder<RenderingEntityCreatedMessage> mb
        #endif
        ) : base(worldOrigin, worldOrientation
            #if DEBUG
            , mb
            #endif
            )
    {
        #if DEBUG
        
        MessageBuilder!.AddParameter(viewWidth)
                       .AddParameter(viewHeight)
                       .AddParameter(zNear)
                       .AddParameter(zFar);

        #endif

        // Construct view-space clipping planes and matrix
        float semiViewWidth = viewWidth / 2, semiViewHeight = viewHeight / 2;
        Vector3D nearBottomLeftPoint = new(-semiViewWidth, -semiViewHeight, zNear);

        switch (this)
        {
            case OrthogonalCamera or DistantLight:
                viewToScreen = Matrix4x4.Identity;
                viewToScreen.m00 = 2 / viewWidth;
                viewToScreen.m11 = 2 / viewHeight;
                viewToScreen.m22 = 2 / (zFar - zNear);
                viewToScreen.m23 = -(zFar + zNear) / (zFar - zNear);

                Vector3D farTopRightPoint = new(semiViewWidth, semiViewHeight, zFar);

                ViewClippingPlanes = new ClippingPlane[]
                {
                    new(nearBottomLeftPoint, Vector3D.UnitX), // Left
                    new(nearBottomLeftPoint, Vector3D.UnitY), // Bottom
                    new(nearBottomLeftPoint, Vector3D.UnitZ), // Near
                    new(farTopRightPoint, Vector3D.UnitNegativeX), // Right
                    new(farTopRightPoint, Vector3D.UnitNegativeY), // Top
                    new(farTopRightPoint, Vector3D.UnitNegativeZ) // Far
                };

                break;
            case PerspectiveCamera or Spotlight:
                viewToScreen = Matrix4x4.Zero;
                viewToScreen.m00 = 2 * zNear / viewWidth;
                viewToScreen.m11 = 2 * zNear / viewHeight;
                viewToScreen.m22 = (zFar + zNear) / (zFar - zNear);
                viewToScreen.m23 = -2 * zNear * zFar / (zFar - zNear);
                viewToScreen.m32 = 1;

                farTopRightPoint = new(viewWidth * zFar / (2 * zNear), viewHeight * zFar / (2 * zNear), zFar);
                Vector3D nearTopLeftPoint = new(-semiViewWidth, semiViewHeight, zNear);
                Vector3D nearTopRightPoint = new(semiViewWidth, semiViewHeight, zNear);
                Vector3D farBottomLeftPoint = new(-semiViewWidth * zFar / zNear, -semiViewHeight * zFar / zNear, zFar);
                Vector3D farBottomRightPoint = new(semiViewWidth * zFar / zNear, -semiViewHeight * zFar / zNear, zFar);

                ViewClippingPlanes = new ClippingPlane[]
                {
                    new(nearBottomLeftPoint, Vector3D.NormalFromPlane(farBottomLeftPoint, nearTopLeftPoint, nearBottomLeftPoint)), // Left
                    new(nearBottomLeftPoint, Vector3D.NormalFromPlane(nearBottomLeftPoint, farBottomRightPoint, farBottomLeftPoint)), // Bottom
                    new(nearBottomLeftPoint, Vector3D.UnitZ), // Near
                    new(farTopRightPoint, Vector3D.NormalFromPlane(nearTopRightPoint, farTopRightPoint, farBottomRightPoint)), // Right
                    new(farTopRightPoint, Vector3D.NormalFromPlane(nearTopLeftPoint, farTopRightPoint, nearTopRightPoint)), // Top
                    new(farTopRightPoint, Vector3D.UnitNegativeZ) // Far
                };

                break;
            default:
                throw Exceptions.RenderingObjectTypeNotSupported;
        }

        this.viewWidth = viewWidth;
        this.viewHeight = viewHeight;
        this.zNear = zNear;
        this.zFar = zFar;
    }

    #endregion

    #region Methods

    // Update matrix
    private void UpdateMatrixOrthogonal1()
    {
        // Update view-to-screen matrix
        viewToScreen.m00 = 2 / viewWidth;
    }

    private void UpdateMatrixPerspective1()
    {
        // Update view-to-screen matrix
        
    }

    

    

    

    

    // Update clipping planes
    
    
        
    

    private void UpdateClippingPlanePerspective1()
    {
        // Update left and right clipping planes
        
    }

    

    

    

    

    #endregion
}