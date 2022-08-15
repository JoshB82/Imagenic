using Imagenic.Core.Transitions;
using System;

namespace Imagenic.Core.Entities.Internal;

internal static class TransformableEntityExtensions_Internal
{
    internal static TTransformableEntity Transform_Internal<TTransformableEntity>(
        this TTransformableEntity transformableEntity,
        Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        if (!TransformableEntityExtensionsForTransitions.AddToActiveTransitions(transformation))
        {
            // If no active transformations, run the transformation straight away with no transitions?
            transformation(transformableEntity);
        }

        /*if (transformableEntity.TransformationsNode is null)
        {
            transformableEntity.TransformationsNode = new TransformationNoInputNoOutputNode<TTransformableEntity>(transformation);
        }
        else
        {
            transformableEntity.TransformationsNode.Add(transformation);
        }*/
        return transformableEntity;
    }
}