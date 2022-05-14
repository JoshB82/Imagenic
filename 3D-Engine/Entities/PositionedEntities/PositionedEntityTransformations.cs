using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities.PositionedEntities;

public static class PositionedEntityTransformations
{
    #region TranslateX method

    public static T TranslateX<T>(this T positionedEntity, float distance) where T : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(distance, 0, 0);
        return positionedEntity;
    }

    public static IEnumerable<T> TranslateX<T>(this IEnumerable<T> positionedEntities, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (T positionedEntity in positionedEntities)
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static IEnumerable<T> TranslateX<T>(this IEnumerable<T> positionedEntities, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (T positionedEntity in positionedEntities.Where(predicate))
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static Node<T> TranslateX<T>(this Node<T> positionedEntityNode, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    public static Node<T> TranslateX<T>(this Node<T> positionedEntityNode, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    #endregion

    #region TranslateY method

    public static T TranslateY<T>(this T positionedEntity, float distance) where T : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(0, distance, 0);
        return positionedEntity;
    }

    public static IEnumerable<T> TranslateY<T>(this IEnumerable<T> positionedEntities, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (T positionedEntity in positionedEntities)
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static IEnumerable<T> TranslateY<T>(this IEnumerable<T> positionedEntities, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (T positionedEntity in positionedEntities.Where(predicate))
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static Node<T> TranslateY<T>(this Node<T> positionedEntityNode, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    public static Node<T> TranslateY<T>(this Node<T> positionedEntityNode, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    #endregion

    #region TranslateZ method

    public static T TranslateZ<T>(this T positionedEntity, float distance) where T : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(0, 0, distance);
        return positionedEntity;
    }

    public static IEnumerable<T> TranslateZ<T>(this IEnumerable<T> positionedEntities, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (T positionedEntity in positionedEntities)
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static IEnumerable<T> TranslateZ<T>(this IEnumerable<T> positionedEntities, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (T positionedEntity in positionedEntities.Where(predicate))
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static Node<T> TranslateZ<T>(this Node<T> positionedEntityNode, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    public static Node<T> TranslateZ<T>(this Node<T> positionedEntityNode, float distance, Func<T, bool> predicate) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    #endregion

    #region Translate method

    public static T Translate<T>(this T positionedEntity, Vector3D displacement) where T : PositionedEntity
    {
        positionedEntity.WorldOrigin += displacement;
        return positionedEntity;
    }

    public static IEnumerable<T> Translate<T>(this IEnumerable<T> positionedEntities, Vector3D displacement) where T : PositionedEntity
    {
        foreach (T positionedEntity in positionedEntities)
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static IEnumerable<T> Translate<T>(this IEnumerable<T> positionedEntities, Vector3D displacement, Func<T, bool> predicate) where T : PositionedEntity
    {
        foreach (T positionedEntity in positionedEntities.Where(predicate))
        {
            positionedEntity.WorldOrigin += displacement;
            yield return positionedEntity;
        }
    }

    public static Node<T> Translate<T>(this Node<T> positionedEntityNode, Vector3D displacement) where T : PositionedEntity
    {
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    public static Node<T> Translate<T>(this Node<T> positionedEntityNode, Vector3D displacement, Func<T, bool> predicate) where T : PositionedEntity
    {
        foreach (Node<PositionedEntity> node in positionedEntityNode.GetDescendantsAndSelfOfType<PositionedEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return positionedEntityNode;
    }

    #endregion
}