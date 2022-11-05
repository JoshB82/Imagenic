using System.Diagnostics.CodeAnalysis;
using System;

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
}