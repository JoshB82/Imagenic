using System.Collections.Generic;
using System;
using System.Linq;
using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Entities.TransformableEntities;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferEnumerableEnumerable<TTransformableEntity, TValue> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public IEnumerable<TTransformableEntity> TransformableEntities { get; }
    public IEnumerable<TValue> Values { get; }

    #endregion

    #region Constructors

    internal CascadeBufferEnumerableEnumerable(IEnumerable<TTransformableEntity> transformableEntities, IEnumerable<TValue> values)
    {
        TransformableEntities = transformableEntities;
        Values = values;
    }

    #endregion

    #region Methods

    public IEnumerable<TTransformableEntity> Transform(Action<TTransformableEntity, TValue> transformation)
    {
        return TransformableEntities.Zip(Values, (entity, value) =>
        {
            //entity.Transitions.Add(new Transition<TTransformableEntity, TValue>()
            transformation(entity, value);
            return entity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>(Action<TTransformableEntity, TValue, TInput> transformation, TInput transformationInput)
    {
        return TransformableEntities.Zip(Values, (entity, value) =>
        {
            transformation(entity, value, transformationInput);
            return entity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>(Action<TTransformableEntity, TValue, TInput> transformation, IEnumerable<TInput> transformationInputs)
    {
        return TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            transformation(tuple.First, tuple.Second, tuple.Third);
            return tuple.First;
        });
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TOutput>(Func<TTransformableEntity, TValue, TOutput> transformation)
    {
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TInput, TOutput>(Func<TTransformableEntity, TValue, TInput, TOutput> transformation, TInput transformationInput)
    {
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value, transformationInput);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TInput, TOutput>(Func<TTransformableEntity, TValue, TInput, TOutput> transformation, IEnumerable<TInput> transformationInputs)
    {
        var outputs = TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            var output = transformation(tuple.First, tuple.Second, tuple.Third);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
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

    public static IEnumerable<TTranslatableEntity> Translate<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += value + displacement; });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => translatableEntity.WorldOrigin += value + displacement);
    }

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