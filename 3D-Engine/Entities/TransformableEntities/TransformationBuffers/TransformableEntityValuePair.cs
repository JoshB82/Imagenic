namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class TransformableEntityValuePair<TTransformableEntity, TValue> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TTransformableEntity TransformableEntity { get; set; }
    public TValue? Value { get; set; }

    #endregion

    #region Constructors

    public TransformableEntityValuePair(TTransformableEntity transformableEntity, TValue? value)
    {
        TransformableEntity = transformableEntity;
        Value = value;
    }

    #endregion

    #region Methods

    public void Desconstruct(out TTransformableEntity transformableEntity, out TValue? value)
    {
        transformableEntity = TransformableEntity;
        value = Value;
    }

    #endregion
}