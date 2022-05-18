﻿using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities.PositionedEntities.OrientatedEntities;

public static class OrientatedEntityTransformations
{
    /// <summary>
    /// Orientates a <typeparamref name="TOrientatedEntity"/> to the specified <see cref="Orientation"/>.
    /// </summary>
    /// <typeparam name="TOrientatedEntity">The type of the object being orientated.</typeparam>
    /// <param name="orientatedEntity">The <typeparamref name="TOrientatedEntity"/> being orientated.</param>
    /// <param name="orientation">The new <see cref="Orientation"/> of the <typeparamref name="TOrientatedEntity"/>.</param>
    /// <returns>The <typeparamref name="TOrientatedEntity"/> with the new <see cref="Orientation"/>.</returns>
    [return: NotNull]
    public static TOrientatedEntity Orientate<TOrientatedEntity>([DisallowNull] this TOrientatedEntity orientatedEntity, [DisallowNull] Orientation orientation) where TOrientatedEntity : OrientatedEntity
    {
        ThrowIfParameterIsNull(orientatedEntity);
        ThrowIfParameterIsNull(orientation);

        orientatedEntity.WorldOrientation = orientation;
        return orientatedEntity;
    }

    /// <summary>
    /// Orientates each element of a <typeparamref name="TOrientatedEntity"/> sequence to the specified <see cref="Orientation"/>.
    /// </summary>
    /// <typeparam name="TOrientatedEntity">The type of the elements being orientated.</typeparam>
    /// <param name="orientatedEntities">The <typeparamref name="TOrientatedEntity"/> sequence containing elements being orientated.</param>
    /// <param name="orientation">The new <see cref="Orientation"/> of each element of the <typeparamref name="TOrientatedEntity"/> sequence.</param>
    /// <returns>The <typeparamref name="TOrientatedEntity"/> sequence with each element having the new <see cref="Orientation"/>.</returns>
    [return: NotNull]
    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>([DisallowNull] this IEnumerable<TOrientatedEntity> orientatedEntities, [DisallowNull] Orientation orientation) where TOrientatedEntity : OrientatedEntity
    {
        ThrowIfParameterIsNull(orientatedEntities);
        ThrowIfParameterIsNull(orientation);

        return orientatedEntities.Select(orientatedEntity =>
        {
            orientatedEntity.WorldOrientation = orientation;
            return orientatedEntity;
        });
    }

    /// <summary>
    /// Orientates each element of a <typeparamref name="TOrientatedEntity"/> sequence that satisfies a specified predicate to the specified <see cref="Orientation"/>.
    /// </summary>
    /// <typeparam name="TOrientatedEntity">The type of the elements being orientated.</typeparam>
    /// <param name="orientatedEntities">The <typeparamref name="TOrientatedEntity"/> sequence containing elements being orientated.</param>
    /// <param name="orientation">The new <see cref="Orientation"/> of each element of the <typeparamref name="TOrientatedEntity"/> sequence.</param>
    /// <param name="predicate">The predicate which determines which elements of the <typeparamref name="TOrientatedEntity"/> sequence are orientated.</param>
    /// <returns>The <typeparamref name="TOrientatedEntity"/> sequence with each element that satisified the predicate having the new <see cref="Orientation"/>.</returns>
    [return: NotNull]
    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>([DisallowNull] this IEnumerable<TOrientatedEntity> orientatedEntities, [DisallowNull] Orientation orientation, [DisallowNull] Func<TOrientatedEntity, bool> predicate) where TOrientatedEntity : OrientatedEntity
    {
        ThrowIfParameterIsNull(orientatedEntities);
        ThrowIfParameterIsNull(orientation);
        ThrowIfParameterIsNull(predicate);

        return orientatedEntities.Select(orientatedEntity =>
        {
            if (predicate(orientatedEntity))
            {
                orientatedEntity.WorldOrientation = orientation;
            }
            return orientatedEntity;
        });
    }

    private static void OrientatePossibleOrientatedEntity(object toBeInspected, Orientation orientation)
    {
        switch (toBeInspected)
        {
            case OrientatedEntity orientatedEntity:
                orientatedEntity.WorldOrientation = orientation;
                break;
            case Node node:
                foreach (Node node1 in node.GetDescendantsAndSelf())
                {
                    OrientatePossibleOrientatedEntity(node1.Content, orientation);
                }
                break;
            case IEnumerable<object> objects:
                foreach (object @object in objects)
                {
                    OrientatePossibleOrientatedEntity(@object, orientation);
                }
                break;
        }
    }

    public static IEnumerable<object> Orientate(this IEnumerable<object> objects, Orientation orientation)
    {
        return objects.Select(@object =>
        {
            OrientatePossibleOrientatedEntity(@object, orientation);
            return @object;
        });
    }

    public static IEnumerable<object> Orientate(this IEnumerable<object> objects, Orientation orientation, Func<OrientatedEntity, bool> predicate)
    {
        return objects.Select(@object =>
        {
            if (@object is OrientatedEntity orientatedEntity && predicate(orientatedEntity))
            {
                orientatedEntity.WorldOrientation = orientation;
            }
            return @object;
        });
    }

    public static Node<TOrientatedEntity> Orientate<TOrientatedEntity>(this Node<TOrientatedEntity> orientatedEntityNode, Orientation orientation) where TOrientatedEntity : OrientatedEntity
    {
        foreach (Node<OrientatedEntity> node in orientatedEntityNode.GetDescendantsAndSelfOfType<OrientatedEntity>())
        {
            node.Content.WorldOrientation = orientation;
        }
        return orientatedEntityNode;
    }

    public static Node<TOrientatedEntity> Orientate<TOrientatedEntity>(this Node<TOrientatedEntity> orientatedEntityNode, Orientation orientation, Func<TOrientatedEntity, bool> predicate) where TOrientatedEntity : OrientatedEntity
    {

    }
}