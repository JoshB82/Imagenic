using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities;

public sealed class CascadeBuffer<TEntity, TValue>
{
    #region Fields and Properties

    public TEntity Entity { get; set; }
    public TValue Value { get; set; }

    #endregion

    #region Constructors

    public CascadeBuffer(TEntity entity, TValue value)
    {
        Entity = entity;
        Value = value;
    }

    #endregion

    #region Methods

    public TEntity Transform(Action<TEntity, TValue> transformation)
    {
        transformation(Entity, Value);
        return Entity;
    }

    public CascadeBuffer<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var output = transformation(Entity, Value);
        return new CascadeBuffer<TEntity, TOutput>(Entity, output);
    }

    #endregion
}

public sealed class CascadeBufferEnumerable<TEntity, TValue>
{
    #region Fields and Properties

    public IEnumerable<TEntity> Entities { get; set; }
    public IEnumerable<TValue> Values { get; set; }

    #endregion

    #region Constructors

    public CascadeBufferEnumerable(IEnumerable<TEntity> entities, IEnumerable<TValue> values)
    {
        Entities = entities;
        Values = values;
    }

    #endregion

    #region Methods

    public IEnumerable<TEntity> Transform(Action<TEntity, TValue> transformation)
    {
        return Entities.Zip(Values, (entity, value) =>
        {
            transformation(entity, value);
            return entity;
        });
    }

    public CascadeBufferEnumerable<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var outputs = Entities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new CascadeBufferEnumerable<TEntity, TOutput>(Entities, outputs);
    }

    #endregion
}