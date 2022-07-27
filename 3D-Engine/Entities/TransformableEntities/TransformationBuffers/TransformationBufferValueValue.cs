using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

public sealed class TransformationBufferValueValue<TTransformableEntity, TValue> : TransformableEntity, ITransformationBuffer
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TTransformableEntity TransformableEntity { get; }
    public IList<TValue?> Values { get; }

    #endregion

    #region Constructors

    public TransformationBufferValueValue([DisallowNull] TTransformableEntity transformableEntity, [DisallowNull] IList<TValue?> values)
    {
        TransformableEntity = transformableEntity;
        Values = values;
    }

    #endregion

    #region Methods

    public ITransformationBuffer Transform([DisallowNull] Action<TTransformableEntity> transformation)
    {
        ThrowIfNull(transformation);
        transformation(TransformableEntity);
        return this;
    }

    public ITransformationBuffer Transform<TInput>([DisallowNull] Action<TTransformableEntity, TInput?> transformation, TInput? input)
    {
        ThrowIfNull(transformation);
        transformation(TransformableEntity, input);
        return this;
    }

    public ITransformationBuffer Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation, int? bufferIndex = null)
    {
        ThrowIfNull(transformation);

        TValue? selectedValue;

        if (bufferIndex is null)
        {
            selectedValue = Values.Last();
        }
        else if (bufferIndex < Values.Count)
        {
            selectedValue = Values[bufferIndex.Value];
        }
        else
        {
            // Throw exception
            throw new Exception();
        }

        transformation(TransformableEntity, selectedValue);

        return this;
    }

    public ITransformationBuffer Transform([DisallowNull] Action<TTransformableEntity, IEnumerable<TValue?>> transformation,
        [DisallowNull] Func<TValue?, TValue?> selector)
    {
        ThrowIfNull(transformation, selector);

        transformation(TransformableEntity, Values.Select(selector));

        return this;
    }

    public TransformationBufferValueValue<TTransformableEntity, TValue?> Transform()
    {

    }

    #endregion
}