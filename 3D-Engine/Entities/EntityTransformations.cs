using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities;

public static class EntityTransformations
{
    #region Entity

    public static TEntity Transform<TEntity>(this TEntity entity, Action<TEntity> transformation) where TEntity : Entity
    {
        transformation(entity);
        return entity;
    }

    public static CascadeBuffer<TEntity, TOutput> Transform<TEntity, TOutput>(this TEntity entity, Func<TEntity, TOutput> transformation) where TEntity : Entity
    {
        var output = transformation(entity);
        return new CascadeBuffer<TEntity, TOutput>(entity, output);
    }

    public static TEntity Transform<TEntity, TInput>(this TEntity entity, Action<TEntity, TInput> transformation, TInput transformationInput) where TEntity : Entity
    {
        transformation(entity, transformationInput);
        return entity;
    }

    public static CascadeBuffer<TEntity, TOutput> Transform<TEntity, TInput, TOutput>(this TEntity entity, Func<TEntity, TInput, TOutput> transformation, TInput transformationInput) where TEntity : Entity
    {
        var output = transformation(entity, transformationInput);
        return new CascadeBuffer<TEntity, TOutput>(entity, output);
    }

    #endregion

    #region Entity enumerable

    public static IEnumerable<TEntity> Transform<TEntity>(
        this IEnumerable<TEntity> entities, Action<TEntity> transformation) where TEntity : Entity
    {
        return entities.Select(entity =>
        {
            transformation(entity);
            return entity;
        });
    }

    public static CascadeBufferEnumerable<TEntity, TOutput> Transform<TEntity, TOutput>(this IEnumerable<TEntity> entities, Func<TEntity, TOutput> transformation) where TEntity : Entity
    {
        var outputs = entities.Select(entity =>
        {
            var output = transformation(entity);
            return output;
        });
        return new CascadeBufferEnumerable<TEntity, TOutput>(entities, outputs);
    }

    public static IEnumerable<TEntity> Transform<TEntity, TInput>(this IEnumerable<TEntity> entities, Action<TEntity, TInput> transformation, TInput transformationInput) where TEntity : Entity
    {
        return entities.Select(entity =>
        {
            transformation(entity, transformationInput);
            return entity;
        });
    }

    public static CascadeBufferEnumerable<TEntity, TOutput> Transform<TEntity, TInput, TOutput>(this IEnumerable<TEntity> entities, Func<TEntity, TInput, TOutput> transformation, TInput transformationInput) where TEntity : Entity
    {
        var outputs = entities.Select(entity =>
        {
            var output = transformation(entity, transformationInput);
            return output;
        });
        return new CascadeBufferEnumerable<TEntity, TOutput>(entities, outputs);
    }

    public static IEnumerable<TEntity> Transform<TEntity, TInput>(this IEnumerable<TEntity> entities, Action<TEntity, TInput> transformation, IEnumerable<TInput> transformationInputs) where TEntity : Entity
    {
        return entities.Zip(transformationInputs, (entity, input) =>
        {
            transformation(entity, input);
            return entity;
        });
    }

    public static CascadeBufferEnumerable<TEntity, TOutput> Transform<TEntity, TInput, TOutput>(this IEnumerable<TEntity> entities, Func<TEntity, TInput, TOutput> transformation, IEnumerable<TInput> transformationInputs) where TEntity : Entity
    {
        var outputs = entities.Zip(transformationInputs, (entity, input) =>
        {
            var output = transformation(entity, input);
            return output;
        });
        return new CascadeBufferEnumerable<TEntity, TOutput>(entities, outputs);
    }

    #endregion

    #region Object enumerable

    #endregion

    #region Entity node

    #endregion

    #region Entity node enumerable

    #endregion










    public static TEntity Transform<TEntity, TInput>(this TEntity entity, Action<TEntity, TInput> transformation, TInput startValue, TInput endValue, float timeStart, float timeEnd) where TEntity : Entity
    {
        entity.Transitions.Add(new Transition<TInput>(startValue, endValue, timeStart, timeEnd) { Transformation = transformation });
        return entity;
    }
}