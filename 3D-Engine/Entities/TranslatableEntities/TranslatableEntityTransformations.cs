/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 * 
 * File desc.: Defines a class that provides extension methods for transforming translatable entities.
 */

using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities.TranslatableEntities;

/// <summary>
/// Provides extension methods for transforming translatable entities.
/// </summary>
public static class TranslatableEntityTransformations
{
    #region TranslateX method

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> in the X direction by the specified value.
    /// </summary>
    /// <typeparam name="TTranslatableEntity">The type of the object to be translated.</typeparam>
    /// <param name="translatableEntity">The <typeparamref name="TTranslatableEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TTranslatableEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TTranslatableEntity"/>.</returns>
    public static TTranslatableEntity TranslateX<TTranslatableEntity>(this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        translatableEntity.WorldOrigin += new Vector3D(distance, 0, 0);
        return translatableEntity;
    }

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    public static IEnumerable<T> TranslateX<T>(this IEnumerable<T> translatableEntities, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (T translatableEntity in translatableEntities.Where(predicate))
        {
            translatableEntity.WorldOrigin += displacement;
            yield return translatableEntity;
        }
    }

    public static Node<T> TranslateX<T>(this Node<T> translatableEntityNode, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static Node<T> TranslateX<T>(this Node<T> translatableEntityNode, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static IEnumerable<Node<T>> TranslateX<T>(this IEnumerable<Node<T>> translatableEntityNodes, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return translatableEntityNodes.Select(translatableEntityNode =>
        {
            translatableEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<TranslatableEntity> childtranslatableEntityNode in translatableEntityNode.GetDescendants())
            {
                childtranslatableEntityNode.Content.WorldOrigin += displacement;
            }
            return translatableEntityNode;
        });
    }

    #endregion

    #region TranslateY method

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> in the Y direction by the specified value.
    /// </summary>
    /// <typeparam name="TTranslatableEntity">The type of the object to be translated.</typeparam>
    /// <param name="translatableEntity">The <typeparamref name="TTranslatableEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TTranslatableEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TTranslatableEntity"/>.</returns>
    public static TTranslatableEntity TranslateY<TTranslatableEntity>(this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        translatableEntity.WorldOrigin += new Vector3D(0, distance, 0);
        return translatableEntity;
    }

    public static IEnumerable<TTranslatableEntity> TranslateY<TTranslatableEntity>(this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    public static IEnumerable<T> TranslateY<T>(this IEnumerable<T> translatableEntities, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (T translatableEntity in translatableEntities.Where(predicate))
        {
            translatableEntity.WorldOrigin += displacement;
            yield return translatableEntity;
        }
    }

    public static Node<T> TranslateY<T>(this Node<T> translatableEntityNode, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static Node<T> TranslateY<T>(this Node<T> translatableEntityNode, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static IEnumerable<Node<T>> TranslateY<T>(this IEnumerable<Node<T>> translatableEntityNodes, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, distance, 0);
        return translatableEntityNodes.Select(translatableEntityNode =>
        {
            translatableEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<TranslatableEntity> childtranslatableEntityNode in translatableEntityNode.GetDescendants())
            {
                childtranslatableEntityNode.Content.WorldOrigin += displacement;
            }
            return translatableEntityNode;
        });
    }

    #endregion

    #region TranslateZ method

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> in the Z direction by the specified value.
    /// </summary>
    /// <typeparam name="TTranslatableEntity">The type of the object to be translated.</typeparam>
    /// <param name="translatableEntity">The <typeparamref name="TTranslatableEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TTranslatableEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TTranslatableEntity"/>.</returns>
    public static TTranslatableEntity TranslateZ<TTranslatableEntity>(this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        translatableEntity.WorldOrigin += new Vector3D(0, 0, distance);
        return translatableEntity;
    }

    public static IEnumerable<TTranslatableEntity> TranslateZ<TTranslatableEntity>(this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    public static IEnumerable<T> TranslateZ<T>(this IEnumerable<T> translatableEntities, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (T translatableEntity in translatableEntities.Where(predicate))
        {
            translatableEntity.WorldOrigin += displacement;
            yield return translatableEntity;
        }
    }

    public static Node<T> TranslateZ<T>(this Node<T> translatableEntityNode, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static Node<T> TranslateZ<T>(this Node<T> translatableEntityNode, float distance, Func<T, bool> predicate) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static IEnumerable<Node<T>> TranslateZ<T>(this IEnumerable<Node<T>> translatableEntityNodes, float distance) where T : TranslatableEntity
    {
        var displacement = new Vector3D(0, 0, distance);
        return translatableEntityNodes.Select(translatableEntityNode =>
        {
            translatableEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<TranslatableEntity> childtranslatableEntityNode in translatableEntityNode.GetDescendants())
            {
                childtranslatableEntityNode.Content.WorldOrigin += displacement;
            }
            return translatableEntityNode;
        });
    }

    #endregion

    #region Translate method

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> by the specified value.
    /// </summary>
    /// <typeparam name="TTranslatableEntity">The type of the object to be translated.</typeparam>
    /// <param name="translatableEntity">The <typeparamref name="TTranslatableEntity"/> being translated.</param>
    /// <param name="displacement">A <see cref="Vector3D"/> representing the amounts by which to move the <typeparamref name="TTranslatableEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TTranslatableEntity"/>.</returns>
    public static TTranslatableEntity Translate<TTranslatableEntity>(this TTranslatableEntity translatableEntity, Vector3D displacement) where TTranslatableEntity : TranslatableEntity
    {
        translatableEntity.WorldOrigin += displacement;
        return translatableEntity;
    }

    public static IEnumerable<TTranslatableEntity> Translate<TTranslatableEntity>(this IEnumerable<TTranslatableEntity> translatableEntities, Vector3D displacement) where TTranslatableEntity : TranslatableEntity
    {
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    public static IEnumerable<T> Translate<T>(this IEnumerable<T> translatableEntities, Vector3D displacement, Func<T, bool> predicate) where T : TranslatableEntity
    {
        foreach (T translatableEntity in translatableEntities.Where(predicate))
        {
            translatableEntity.WorldOrigin += displacement;
            yield return translatableEntity;
        }
    }

    public static Node<T> Translate<T>(this Node<T> translatableEntityNode, Vector3D displacement) where T : TranslatableEntity
    {
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>())
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static Node<T> Translate<T>(this Node<T> translatableEntityNode, Vector3D displacement, Func<T, bool> predicate) where T : TranslatableEntity
    {
        foreach (Node<TranslatableEntity> node in translatableEntityNode.GetDescendantsAndSelfOfType<TranslatableEntity>(predicate))
        {
            node.Content.WorldOrigin += displacement;
        }

        return translatableEntityNode;
    }

    public static IEnumerable<Node<T>> Translate<T>(this IEnumerable<Node<T>> translatableEntityNodes, Vector3D displacement) where T : TranslatableEntity
    {
        return translatableEntityNodes.Select(translatableEntityNode =>
        {
            translatableEntityNode.Content.WorldOrigin += displacement;
            foreach (Node<TranslatableEntity> childtranslatableEntityNode in translatableEntityNode.GetDescendants())
            {
                childtranslatableEntityNode.Content.WorldOrigin += displacement;
            }
            return translatableEntityNode;
        });
    }

    #endregion
}