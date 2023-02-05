using System.Diagnostics.CodeAnalysis;
using System;
using Imagenic.Core.Transitions;
using Imagenic.Core.Attributes;
using Imagenic.Core.Entities.TransformableEntities;

namespace Imagenic.Core.Entities;

internal static class TransformableEntityTransformations_Implementations
{
    internal static TTransformableEntity Transform<TTransformableEntity>(
        this TTransformableEntity transformableEntity,
        Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        if (transformableEntity.Transitions.Count > 0)
        {
            foreach (Transition transition in transformableEntity.Transitions)
            {
                var transformationNode = new TransformationNode<TTransformableEntity>(transformation);
                transition.TransformationNodes.Add(transformationNode);
            }
        }
        else
        {
            transformation(transformableEntity);
        }
        return transformableEntity;
    }

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

    internal static TTransformableEntity Transform<TTransformableEntity, TOutput>(
        this TTransformableEntity transformableEntity,
        Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        if (transformableEntity.Transitions.Count > 0)
        {
            foreach (Transition transition in transformableEntity.Transitions)
            {
                //var transformation = new TransformationN
            }
        }
        else
        {

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