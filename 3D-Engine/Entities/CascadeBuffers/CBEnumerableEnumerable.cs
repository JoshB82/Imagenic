﻿using System.Collections.Generic;
using System;
using System.Linq;
using Imagenic.Core.Entities.PositionedEntities;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferEnumerableEnumerable<TEntity, TValue>
{
    #region Fields and Properties

    public IEnumerable<TEntity> Entities { get; set; }
    public IEnumerable<TValue> Values { get; set; }

    #endregion

    #region Constructors

    public CascadeBufferEnumerableEnumerable(IEnumerable<TEntity> entities, IEnumerable<TValue> values)
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

    public CascadeBufferEnumerableEnumerable<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var outputs = Entities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TEntity, TOutput>(Entities, outputs);
    }

    #endregion
}

public static class CascadeBufferEnumerableEnumerableExtensions
{
    #region TranslateX method

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateXC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); return value + distance; });
    }

    #endregion

    #region TranslateY method

    public static IEnumerable<TTranslatableEntity> TranslateY<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateYC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); return value + distance; });
    }

    #endregion

    #region TranslateZ method

    public static IEnumerable<TTranslatableEntity> TranslateZ<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateZC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); return value + distance; });
    }

    #endregion

    #region Translate method

    #endregion

    #region Orientate method

    #endregion

    #region ScaleX method

    #endregion

    #region ScaleY method

    #endregion

    #region ScaleZ method

    #endregion

    #region Scale method

    #endregion
}