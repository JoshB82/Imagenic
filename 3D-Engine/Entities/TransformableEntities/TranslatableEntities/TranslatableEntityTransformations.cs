﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 * 
 * File desc.: Defines a class that provides extension methods for transforming translatable entities.
 */

using Imagenic.Core.Attributes;
using Imagenic.Core.CascadeBuffers;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

/// <summary>
/// Provides extension methods for transforming translatable entities.
/// </summary>
public static partial class TranslatableEntityTransformations
{
    #region TranslateX method

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> in the X direction by the specified value.
    /// <para><example>
    /// Example: Translate a cube by 10 units in the positive X direction.
    /// <code>
    /// var cube = new Cube();
    /// cube.<strong>TranslateX(10)</strong>;
    /// </code>
    /// </example></para>
    /// </summary>
    /// <typeparam name="TTranslatableEntity">The type of the object to be translated.</typeparam>
    /// <param name="translatableEntity">The <typeparamref name="TTranslatableEntity"/> being translated.</param>
    /// <param name="distance">The amount by which to move the <typeparamref name="TTranslatableEntity"/>.</param>
    /// <returns>The translated <typeparamref name="TTranslatableEntity"/>.</returns>
    public static partial TTranslatableEntity TranslateX<TTranslatableEntity>(
        [DisallowNull][ThrowIfNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity;
    
        
    

    /// <summary>
    /// Translates a <typeparamref name="TTranslatableEntity"/> in the X direction by the specified value.
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntity"></param>
    /// <param name="distance"></param>
    /// <returns>The new position of the <typeparamref name="TTranslatableEntity"/> for cascading.</returns>
    public static CascadeBufferValueValue<TTranslatableEntity, Vector3D> TranslateXC<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => e.WorldOrigin += new Vector3D(distance, 0, 0));
    }

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        [DisallowNull] this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        var displacement = new Vector3D(distance, 0, 0);
        return translatableEntities.Select(translatableEntity =>
        {
            translatableEntity.WorldOrigin += displacement;
            return translatableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntities"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> TranslateXC<TTranslatableEntity>(
        [DisallowNull] this IEnumerable<TTranslatableEntity> translatableEntities, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntities);
        var displacement = new Vector3D(distance, 0, 0);
        return translatableEntities.Transform(e => e.WorldOrigin += displacement);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntities"></param>
    /// <param name="distances"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> TranslateXC<TTranslatableEntity>(
        [DisallowNull] this IEnumerable<TTranslatableEntity> translatableEntities,
        [DisallowNull] IEnumerable<float> distances) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntities, distances);
        return translatableEntities.Transform((e, i) => e.WorldOrigin += new Vector3D(i, 0, 0), distances);
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
    public static TTranslatableEntity TranslateY<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => { e.WorldOrigin += new Vector3D(0, distance, 0); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntity"></param>
    /// <param name="distance"></param>
    /// <returns>The position in world space of the <typeparamref name="TTranslatableEntity"/> post translation.</returns>
    public static CascadeBufferValueValue<TTranslatableEntity, Vector3D> TranslateYC<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => e.WorldOrigin += new Vector3D(0, distance, 0));
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
    public static TTranslatableEntity TranslateZ<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => { e.WorldOrigin += new Vector3D(0, 0, distance); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntity"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTranslatableEntity, Vector3D> TranslateZC<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, float distance) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => e.WorldOrigin += new Vector3D(0, 0, distance));
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
    public static TTranslatableEntity Translate<TTranslatableEntity>(
        [DisallowNull] this TTranslatableEntity translatableEntity, Vector3D displacement) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => { e.WorldOrigin += displacement; });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTranslatableEntity"></typeparam>
    /// <param name="translatableEntity"></param>
    /// <param name="displacement"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        [DisallowNull] TTranslatableEntity translatableEntity, Vector3D displacement) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(translatableEntity);
        return translatableEntity.Transform(e => e.WorldOrigin += displacement);
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