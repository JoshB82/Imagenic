using Imagenic.Core.CascadeBuffers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities.TransformableEntities;

public static class TransformableEntityTransformations
{
    public static TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntity, transformation);
        transformation(transformableEntity);
        return transformableEntity;
    }

    public static TTransformableEntity Transform<TTransformableEntity, TInput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? input)
    {
        ThrowIfParameterIsNull(transformableEntity, transformation);
        transformation(transformableEntity, input);
        return transformableEntity;
    }

    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntity, transformation);
        var output = transformation(transformableEntity);
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? input) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformation);
        var output = transformation(transformableEntity, input);
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation);
        return transformableEntities.Select(transformableEntity =>
        {
            transformation(transformableEntity);
            return transformableEntity;
        });
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, predicate);
        return transformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity))
            {
                transformation(transformableEntity);
            }
            return transformableEntity;
        });
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? transformationInput) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation);
        return transformableEntities.Select(transformableEntity =>
        {
            transformation(transformableEntity, transformationInput);
            return transformableEntity;
        });
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? transformationInput,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, predicate);
        return transformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity, transformationInput))
            {
                transformation(transformableEntity, transformationInput);
            }
            return transformableEntity;
        });
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, transformationInputs);
        return transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            transformation(transformableEntity, transformationInput);
            return transformableEntity;
        });
    }

    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, transformationInputs, predicate);
        return transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            if (predicate(transformableEntity, transformationInput))
            {
                transformation(transformableEntity, transformationInput);
            }
            return transformableEntity;
        });
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity));
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, predicate);
        var outputs = transformableEntities.Select(transformableEntity =>
        {
            return predicate(transformableEntity)
            ? transformation(transformableEntity)
            : default;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? transformationInput) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity, transformationInput));
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? transformationInput,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, predicate);
        var outputs = transformableEntities.Select(transformableEntity =>
        {
            return predicate(transformableEntity, transformationInput)
            ? transformation(transformableEntity, transformationInput)
            : default;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, transformationInputs);
        var outputs = transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            return transformation(transformableEntity, transformationInput);
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfParameterIsNull(transformableEntities, transformation, transformationInputs, predicate);
        var outputs = transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            return predicate(transformableEntity, transformationInput)
            ? transformation(transformableEntity, transformationInput)
            : default;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }
}