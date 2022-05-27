using System.Collections.Generic;
using System;
using System.Linq;
using Imagenic.Core.Entities.PositionedEntities;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferEnumerableValue<TEntity, TValue> where TEntity : Entity
{
    #region Fields and Parameters

    public IEnumerable<TEntity> Entities { get; set; }
    public TValue Value { get; set; }

    #endregion

    #region Constructors

    public CascadeBufferEnumerableValue(IEnumerable<TEntity> entities, TValue value)
    {
        Entities = entities;
        Value = value;
    }

    #endregion

    #region Methods

    public IEnumerable<TEntity> Transform(Action<TEntity, TValue> transformation)
    {
        return Entities.Select(entity =>
        {
            transformation(entity, Value);
            return entity;
        });
    }

    public CascadeBufferEnumerableValue<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var outputs = Entities.Select(entity =>
        {
            var output = transformation(entity, Value);
            return output;
        });
        return new CascadeBufferEnumerableValue<TEntity, TOutput>(Entities, outputs.Last());
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