using Imagenic.Core.Attributes;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

internal static class TranslatableEntityTransformations_Implementations
{
    #region TranslateX method

    internal static TTranslatableEntity TranslateX<TTranslatableEntity>(
        [DisallowNull][ThrowIfNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        return translatableEntity.Transform(e => { e.WorldOrigin += new Vector3D(distance, 0, 0); });
    }

    internal static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        [DisallowNull] this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    internal static Node<TTranslatableEntity> TranslateX<TTranslatableEntity>(this Node<TTranslatableEntity> translatableEntityNode, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    internal static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(this IEnumerable<TTranslatableEntity> translatableEntities, float distance, Func<TTranslatableEntity, bool> predicate) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (TTranslatableEntity translatableEntity in translatableEntities.Where(predicate))
        {
            translatableEntity.WorldOrigin += displacement;
            yield return translatableEntity;
        }
    }

    #endregion
}