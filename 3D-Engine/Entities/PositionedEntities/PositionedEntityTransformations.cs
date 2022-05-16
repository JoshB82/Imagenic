/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 * 
 * File desc.: Defines a class that provides extension methods for transforming positioned entities.
 */

using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities.PositionedEntities;

/// <summary>
/// Provides extension methods for transforming positioned entities.
/// </summary>
public static class PositionedEntityTransformations
{
    #region TranslateX method

    /// <summary>
    /// Translates a <typeparamref name="TPositionedEntity"/> in the X direction by the specified value.
    /// </summary>
    /// <typeparam name="TPositionedEntity">The type of the object to be translated.</typeparam>
    /// <param name="positionedEntity">The <typeparamref name="TPositionedEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TPositionedEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TPositionedEntity"/>.</returns>
    public static TPositionedEntity TranslateX<TPositionedEntity>(this TPositionedEntity positionedEntity, float distance) where TPositionedEntity : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(distance, 0, 0);
        return positionedEntity;
    }

    public static IEnumerable<TPositionedEntity> TranslateX<TPositionedEntity>(this IEnumerable<TPositionedEntity> positionedEntities, float distance) where TPositionedEntity : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return positionedEntities.Select(positionedEntity =>
        {
            positionedEntity.WorldOrigin += displacement;
            return positionedEntity;
        });
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

    public static IEnumerable<Node<T>> TranslateX<T>(this IEnumerable<Node<T>> positionedEntityNodes, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return positionedEntityNodes.Select(positionedEntityNode =>
        {
            positionedEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<PositionedEntity> childPositionedEntityNode in positionedEntityNode.GetDescendants())
            {
                childPositionedEntityNode.Content.WorldOrigin += displacement;
            }
            return positionedEntityNode;
        });
    }

    #endregion

    #region TranslateY method

    /// <summary>
    /// Translates a <typeparamref name="TPositionedEntity"/> in the Y direction by the specified value.
    /// </summary>
    /// <typeparam name="TPositionedEntity">The type of the object to be translated.</typeparam>
    /// <param name="positionedEntity">The <typeparamref name="TPositionedEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TPositionedEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TPositionedEntity"/>.</returns>
    public static TPositionedEntity TranslateY<TPositionedEntity>(this TPositionedEntity positionedEntity, float distance) where TPositionedEntity : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(0, distance, 0);
        return positionedEntity;
    }

    public static IEnumerable<TPositionedEntity> TranslateY<TPositionedEntity>(this IEnumerable<TPositionedEntity> positionedEntities, float distance) where TPositionedEntity : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        return positionedEntities.Select(positionedEntity =>
        {
            positionedEntity.WorldOrigin += displacement;
            return positionedEntity;
        });
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

    public static IEnumerable<Node<T>> TranslateY<T>(this IEnumerable<Node<T>> positionedEntityNodes, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        return positionedEntityNodes.Select(positionedEntityNode =>
        {
            positionedEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<PositionedEntity> childPositionedEntityNode in positionedEntityNode.GetDescendants())
            {
                childPositionedEntityNode.Content.WorldOrigin += displacement;
            }
            return positionedEntityNode;
        });
    }

    #endregion

    #region TranslateZ method

    /// <summary>
    /// Translates a <typeparamref name="TPositionedEntity"/> in the Z direction by the specified value.
    /// </summary>
    /// <typeparam name="TPositionedEntity">The type of the object to be translated.</typeparam>
    /// <param name="positionedEntity">The <typeparamref name="TPositionedEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TPositionedEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TPositionedEntity"/>.</returns>
    public static TPositionedEntity TranslateZ<TPositionedEntity>(this TPositionedEntity positionedEntity, float distance) where TPositionedEntity : PositionedEntity
    {
        positionedEntity.WorldOrigin += new Vector3D(0, 0, distance);
        return positionedEntity;
    }

    public static IEnumerable<TPositionedEntity> TranslateZ<TPositionedEntity>(this IEnumerable<TPositionedEntity> positionedEntities, float distance) where TPositionedEntity : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        return positionedEntities.Select(positionedEntity =>
        {
            positionedEntity.WorldOrigin += displacement;
            return positionedEntity;
        });
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

    public static IEnumerable<Node<T>> TranslateZ<T>(this IEnumerable<Node<T>> positionedEntityNodes, float distance) where T : PositionedEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        return positionedEntityNodes.Select(positionedEntityNode =>
        {
            positionedEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<PositionedEntity> childPositionedEntityNode in positionedEntityNode.GetDescendants())
            {
                childPositionedEntityNode.Content.WorldOrigin += displacement;
            }
            return positionedEntityNode;
        });
    }

    #endregion

    #region Translate method

    /// <summary>
    /// Translates a <typeparamref name="TPositionedEntity"/> by the specified value.
    /// </summary>
    /// <typeparam name="TPositionedEntity">The type of the object to be translated.</typeparam>
    /// <param name="positionedEntity">The <typeparamref name="TPositionedEntity"/> being translated.</param>
    /// <param name="displacement">A <see cref="Vector3D"/> representing the amounts by which to move the <typeparamref name="TPositionedEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TPositionedEntity"/>.</returns>
    public static TPositionedEntity Translate<TPositionedEntity>(this TPositionedEntity positionedEntity, Vector3D displacement) where TPositionedEntity : PositionedEntity
    {
        positionedEntity.WorldOrigin += displacement;
        return positionedEntity;
    }

    public static IEnumerable<TPositionedEntity> Translate<TPositionedEntity>(this IEnumerable<TPositionedEntity> positionedEntities, Vector3D displacement) where TPositionedEntity : PositionedEntity
    {
        return positionedEntities.Select(positionedEntity =>
        {
            positionedEntity.WorldOrigin += displacement;
            return positionedEntity;
        });
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

    public static IEnumerable<Node<T>> Translate<T>(this IEnumerable<Node<T>> positionedEntityNodes, Vector3D displacement) where T : PositionedEntity
    {
        return positionedEntityNodes.Select(positionedEntityNode =>
        {
            positionedEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<PositionedEntity> childPositionedEntityNode in positionedEntityNode.GetDescendants())
            {
                childPositionedEntityNode.Content.WorldOrigin += displacement;
            }
            return positionedEntityNode;
        });
    }

    #endregion
}