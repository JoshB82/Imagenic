using Imagenic.Core.Maths.Transformations;

namespace Imagenic.Core.Entities.TransformableEntities.TranslatableEntities;

public abstract class TranslatableEntity : TransformableEntity
{
    #region Fields and Properties

    // Matrices
    private Matrix4x4 translationMatrix;
    public Matrix4x4 ModelToWorld { get; protected set; }

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
            RegenerateTranslationMatrix();
            InvokeRenderingEvents();
        }
    }

    #endregion

    #region Constructors

    protected TranslatableEntity(Vector3D worldOrigin)
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