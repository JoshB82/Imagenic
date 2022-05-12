using Imagenic.Core.Maths.Transformations;

namespace Imagenic.Core.Entities;

public abstract class PositionedEntity : Entity
{
    #region Fields and Properties

    // Matrices
    private Matrix4x4 translationMatrix;
    public Matrix4x4 ModelToWorld { get; internal set; }

    // Origins
    internal static readonly Vector4D ModelOrigin = Vector4D.UnitW;
    private Vector3D worldOrigin;
    /// <summary>
    /// The position of the <see cref="SceneEntity"/> in world space.
    /// </summary>
    public virtual Vector3D WorldOrigin
    {
        get => worldOrigin;
        set
        {
            if (value == worldOrigin) return;
            worldOrigin = value;
            CalculateModelToWorldMatrix();
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    protected PositionedEntity(Vector3D worldOrigin)
    {
        this.worldOrigin = worldOrigin;
        RegenerateTranslationMatrix();
    }

    #endregion

    #region Methods

    private void RegenerateTranslationMatrix()
    {
        translationMatrix = Transform.Translate(worldOrigin);
        RegenerateModelToWorldMatrix();
    }

    protected virtual void RegenerateModelToWorldMatrix()
    {
        ModelToWorld = translationMatrix;
    }

    #endregion
}