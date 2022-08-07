using Imagenic.Core.Maths.Transformations;
using System;

namespace Imagenic.Core.Entities;

/// <summary>
/// A <see cref="OrientatedEntity"/> that represents a physical object.
/// </summary>
[Serializable]
public abstract class PhysicalEntity : OrientatedEntity
{
    #region Fields and Properties

    // Casts shadows
    private bool castsShadows = true;
    /// <summary>
    /// Toggle for whether or not this <see cref="PhysicalEntity"/> casts shadows.
    /// </summary>
    /// <remarks>The default value of this property is true.</remarks>
    public bool CastsShadows
    {
        get => castsShadows;
        set
        {
            if (castsShadows == value) return;
            castsShadows = value;
            InvokeRenderingEvents();
        }
    }

    // Opacity
    private float opacity = 1f;
    /// <summary>
    /// The opacity of this <see cref="PhysicalEntity"/> on a scale of 0 to 1 inclusive (0 being completely transparent and 1 being completely opaque).
    /// </summary>
    /// <remarks>This property has a lower priority than <see cref="Visible"/> and a default value of 1.</remarks>
    public float Opacity
    {
        get => opacity;
        set
        {
            if (opacity == value) return;
            opacity = value;
            InvokeRenderingEvents();
        }
    }

    // Visible
    private bool visible = true;
    /// <summary>
    /// Toggle for the visibility of this <see cref="PhysicalEntity"/>.
    /// </summary>
    /// <remarks>This property has a higher priority than <see cref="Opacity"/> and a default value of true.</remarks>
    public bool Visible
    {
        get => visible;
        set
        {
            if (value == visible) return;
            visible = value;
            InvokeRenderingEvents();
        }
    }

    // Scaling
    private Matrix4x4 scalingMatrix;
    private Vector3D scaling = Vector3D.One;
    /// <summary>
    /// A <see cref="Vector3D"/> representing the scaling factor of this <see cref="PhysicalEntity"/> from model space to world space.
    /// </summary>
    /// <remarks>The default value of this property is <see cref="Vector3D.One"/>.</remarks>
    public Vector3D Scaling
    {
        get => scaling;
        set
        {
            if (value == scaling) return;
            scaling = value;
            RegenerateScalingMatrix();
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    protected PhysicalEntity(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation) { }

    #endregion

    #region Methods

    private void RegenerateScalingMatrix()
    {
        scalingMatrix = Transform.Scale(Scaling);
        RegenerateModelToWorldMatrix();
    }

    protected override void RegenerateModelToWorldMatrix()
    {
        base.RegenerateModelToWorldMatrix();
        ModelToWorld *= scalingMatrix;
    }

    #endregion
}