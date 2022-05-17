using Imagenic.Core.Maths;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Imagenic.Core.Entities.PositionedEntities.OrientatedEntities;

public static class OrientatedEntityTransformations
{
    /// <summary>
    /// Orientates a <typeparamref name="TOrientatedEntity"/> to the specified <see cref="Orientation"/>.
    /// </summary>
    /// <typeparam name="TOrientatedEntity">The type of the object to be orientated.</typeparam>
    /// <param name="orientatedEntity">The <typeparamref name="TOrientatedEntity"/> being orientated.</param>
    /// <param name="orientation">The new <see cref="Orientation"/> of the <typeparamref name="TOrientatedEntity"/>.</param>
    /// <returns>The <typeparamref name="TOrientatedEntity"/> with the new <see cref="Orientation"/>.</returns>
    public static TOrientatedEntity Orientate<TOrientatedEntity>(this TOrientatedEntity orientatedEntity, Orientation orientation) where TOrientatedEntity : OrientatedEntity
    {
        orientatedEntity.WorldOrientation = orientation;
        return orientatedEntity;
    }

    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>(this IEnumerable<TOrientatedEntity> orientatedEntities, Orientation orientation) where TOrientatedEntity : OrientatedEntity
    {
        return orientatedEntities.Select(orientatedEntity =>
        {
            orientatedEntity.WorldOrientation = orientation;
            return orientatedEntity;
        });
    }

    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>(this IEnumerable<TOrientatedEntity> orientatedEntities, Orientation orientation, Func<TOrientatedEntity, bool> predicate) where TOrientatedEntity : OrientatedEntity
    {
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