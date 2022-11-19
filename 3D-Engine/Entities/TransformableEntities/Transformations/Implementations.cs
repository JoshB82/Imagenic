using System.Diagnostics.CodeAnalysis;
using System;
using Imagenic.Core.Transitions;
using Imagenic.Core.Attributes;

namespace Imagenic.Core.Entities;

internal static class Implementations
{
    internal static TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        //ThrowIfNull(transformableEntity, transformation, predicate);
        if (predicate(transformableEntity))
        {
            transformation(transformableEntity);
        }
        return transformableEntity;
    }

    internal static TTransformableEntity Start<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        float startTime,
        float endTime,
        out Transition transition) where TTransformableEntity : TransformableEntity
    {
        transition = new(startTime, endTime);
        transformableEntity.Transitions.Add(transition);
        return transformableEntity;
    }

    internal static TTransformableEntity End<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        Transition transition) where TTransformableEntity : TransformableEntity
    {
        transformableEntity.Transitions.Remove(transition);
        return transformableEntity;
    }

    internal static TTransformableEntity End<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        params Transition[] transitions) where TTransformableEntity : TransformableEntity
    {
        foreach (Transition transition in transitions)
        {
            transformableEntity.Transitions.Remove(transition);
        }
        return transformableEntity;
    }
}