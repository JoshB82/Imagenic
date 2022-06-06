using System.Collections.Generic;
using System;
using System.Linq;
using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Entities.TransformableEntities;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferEnumerableValue<TTransformableEntity, TValue> :
    IEnumerable<TransformableEntityValuePair<TTransformableEntity, TValue?>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Parameters

    public IEnumerable<TTransformableEntity> TransformableEntities { get; }
    public TValue? Value { get; }

    #endregion

    #region Constructors

    internal CascadeBufferEnumerableValue(IEnumerable<TTransformableEntity> transformableEntities, TValue? value)
    {
        TransformableEntities = transformableEntities;
        Value = value;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Applies a transformation, in this case an <see cref="Action{TTransformableEntity, TValue?}"/>, to all elements of the stored sequence.
    /// Each transformation is supplied with a <typeparamref name="TTransformableEntity"/> and the same stored <typeparamref name="TValue"/>.
    /// <remarks>The specified transformation cannot be <see langword="null"/>.</remarks>
    /// </summary>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation)
    {
        ThrowIfParameterIsNull(transformation);
        return TransformableEntities.Select(entity =>
        {
            transformation(entity, Value);
            return entity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation,
                                                       [DisallowNull] Func<TTransformableEntity, bool> predicate)
    {
        ThrowIfParameterIsNull(transformation, predicate);
        return TransformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity))
            {
                transformation(transformableEntity, Value);
            }
            return transformableEntity;
        });
    }

    public CascadeBufferEnumerableValue<TTransformableEntity, TOutput> Transform<TOutput>(Func<TTransformableEntity, TValue, TOutput> transformation)
    {
        var outputs = TransformableEntities.Select(entity =>
        {
            var output = transformation(entity, Value);
            return output;
        });
        return new CascadeBufferEnumerableValue<TTransformableEntity, TOutput>(TransformableEntities, outputs.Last());
    }

    public IEnumerator<TransformableEntityValuePair<TTransformableEntity, TValue?>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    #endregion
}

public static class CascadeBufferEnumerableValueExtensions
{
    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((e, value) => { e.WorldOrigin += new Vector3D(value, 0, 0); });
    }
}