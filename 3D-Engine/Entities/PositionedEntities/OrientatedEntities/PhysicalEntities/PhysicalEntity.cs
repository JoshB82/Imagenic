using Imagenic.Core.Maths.Transformations;

namespace Imagenic.Core.Entities;

public abstract class PhysicalEntity : OrientatedEntity
{
    #region Fields and Properties

    private Matrix4x4 scalingMatrix;

    private bool visible = true;
    /// <summary>
    /// Determines whether the <see cref="SceneEntity"/> is visible or not.
    /// </summary>
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

    private Vector3D scaling = Vector3D.One;
    public Vector3D Scaling
    {
        get => scaling;
        set
        {
            if (value == scaling) return;
            scaling = value;
            CalculateModelToWorldMatrix();
            RequestNewRenders();
        }
    }

    #endregion

    #region Constructors

    protected PhysicalEntity(Vector3D worldOrigin, Orientation worldOrientation) : base(worldOrigin, worldOrientation)
    {

    }

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