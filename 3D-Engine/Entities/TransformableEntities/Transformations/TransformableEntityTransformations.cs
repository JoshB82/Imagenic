﻿using Imagenic.Core.Attributes;
using Imagenic.Core.CascadeBuffers;
using Imagenic.Core.Transitions;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

/// <summary>
/// Provides extension methods for transforming transformable entities.
/// </summary>
public static partial class TransformableEntityTransformations
{
    #region TTransformableEntity

    /// <summary>
    /// Applies a custom transformation, in this case an <see cref=""/>, that has no inputs and outputs.
    /// <remarks>The specified transformation and <typeparamref name="TTransformableEntity"/> cannot be <see langword="null"/>.</remarks>
    /// <para><example>
    /// Using call chaining, a cube is subjected to multiple transformations, including a custom transformation.
    /// <code>
    /// var cube = new Cube(Vector3D.Zero, Orientation.OrientationXY, 10);
    /// 
    /// var displacement = new Vector3D(5, 10, 15);
    /// var scaleFactor = Vector3D.One * 4.5f;
    /// 
    /// cube = cube.Translate(displacement)
    ///            <strong>.Transform(e => { e.SideLength = 10; })</strong><br/>
    ///            .Scale(scaleFactor);
    /// </code>
    /// </example></para>
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntity">The <typeparamref name="TTransformableEntity"/> being transformed.</param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">None of this method's parameters can be null.</exception>
    public static partial TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        [DisallowNull][ThrowIfNull] Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity;
    //{
    //ThrowIfNull(transformableEntity, transformation);


    //}

    /// <summary>
    /// Applies a custom transformation, in this case a <see cref="Action{TTransformableEntity}"/>, that has no inputs and outputs. The transformation is only applied if a specified predicate is satisfied (returns <see langword="true"/>).
    /// <remarks>The <typeparamref name="TTransformableEntity"/>, the transformation and predicate cannot be <see langword="null"/>.</remarks>
    /// <para><example>
    /// <em>Example:</em> Using call chaining, a cube is subjected to multiple transformations, including a custom transformation that occurs only 50% of the time.
    /// <code>
    /// var cube = new Cube(Vector3D.Zero, Orientation.OrientationXY, 10);
    /// 
    /// var displacement = new Vector3D(5, 10, 15);
    /// var scaleFactor = Vector3D.One * 4.5f;
    /// var random = new Random();
    /// 
    /// cube = cube.Translate(displacement)
    ///            <strong>.Transform(e => { e.SideLength = 10; }, _ => random.NextInt64() % 2 == 0)</strong><br/>
    ///            .Scale(scaleFactor);
    /// </code>
    /// </example></para>
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static partial TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        [DisallowNull][ThrowIfNull] Action<TTransformableEntity> transformation,
        [DisallowNull][ThrowIfNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity;

    /// <summary>
    /// Starts a transition which tracks transformations until an "end" method is called.
    /// </summary>
    /// <typeparam name="TTransformableEntity">The type of the <typeparamref name="TTransformableEntity"/> being transformed.</typeparam>
    /// <param name="transformableEntity">The <typeparamref name="TTransformableEntity"/> being transformed.</param>
    /// <param name="startTime">The time when the transition should begin.</param>
    /// <param name="endTime">The time when the transition should end.</param>
    /// <param name="transition">The created transition.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static partial TTransformableEntity Start<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        float startTime,
        float endTime,
        out Transition transition) where TTransformableEntity : TransformableEntity;

    /// <summary>
    /// Ends the specified transition.
    /// </summary>
    /// <typeparam name="TTransformableEntity">The type of the <typeparamref name="TTransformableEntity"/> being transformed.</typeparam>
    /// <param name="transformableEntity">The <typeparamref name="TTransformableEntity"/> being transformed.</param>
    /// <param name="transition"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static partial TTransformableEntity End<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        Transition transition) where TTransformableEntity : TransformableEntity;

    /// <summary>
    /// Ends the specified transitions.
    /// </summary>
    /// <typeparam name="TTransformableEntity">The type of the <typeparamref name="TTransformableEntity"/> being transformed.</typeparam>
    /// <param name="transformableEntity">The <typeparamref name="TTransformableEntity"/> being transformed.</param>
    /// <param name="transitions"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static partial TTransformableEntity End<TTransformableEntity>(
        [DisallowNull][ThrowIfNull] this TTransformableEntity transformableEntity,
        params Transition[] transitions) where TTransformableEntity : TransformableEntity;

    





    

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation);
        var output = transformation(transformableEntity);
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation, predicate);
        var output = predicate(transformableEntity)
            ? transformation(transformableEntity)
            : default;
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? input) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformation);
        var output = transformation(transformableEntity, input);
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="input"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? input,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation, predicate);
        var output = predicate(transformableEntity, input)
            ? transformation(transformableEntity, input)
            : default;
        return new CascadeBufferValueValue<TTransformableEntity, TOutput?>(transformableEntity, output);
    }

    #endregion

    #region IEnumerable<TTransformableEntity>

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        return transformableEntities.Select(transformableEntity =>
        {
            transformation(transformableEntity);
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, predicate);
        return transformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity))
            {
                transformation(transformableEntity);
            }
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInput"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? transformationInput) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        return transformableEntities.Select(transformableEntity =>
        {
            transformation(transformableEntity, transformationInput);
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInput"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? transformationInput,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, predicate);
        return transformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity, transformationInput))
            {
                transformation(transformableEntity, transformationInput);
            }
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInputs"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, transformationInputs);
        return transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            transformation(transformableEntity, transformationInput);
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInputs"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, transformationInputs, predicate);
        return transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            if (predicate(transformableEntity, transformationInput))
            {
                transformation(transformableEntity, transformationInput);
            }
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity));
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, predicate);
        var outputs = transformableEntities.Select(transformableEntity =>
        {
            return predicate(transformableEntity)
            ? transformation(transformableEntity)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInput"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? transformationInput) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity, transformationInput));
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInput"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? transformationInput,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, predicate);
        var outputs = transformableEntities.Select(transformableEntity =>
        {
            return predicate(transformableEntity, transformationInput)
            ? transformation(transformableEntity, transformationInput)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInputs"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, transformationInputs);
        var outputs = transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            return transformation(transformableEntity, transformationInput);
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    /// <param name="transformableEntities"></param>
    /// <param name="transformation"></param>
    /// <param name="transformationInputs"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, transformationInputs, predicate);
        var outputs = transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            return predicate(transformableEntity, transformationInput)
            ? transformation(transformableEntity, transformationInput)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
    }

    #endregion

    #region Node<TTransformableEntity>

    public static Node<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>())
        {
            transformation(node.Content);
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity> transformation,
        [DisallowNull] Func<TransformableEntity, bool> transformationPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, transformationPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>())
        {
            if (transformationPredicate(node.Content))
            {
                transformation(node.Content);
            }
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity> transformation,
        [DisallowNull] Func<Node<TransformableEntity>, bool> nodeSelectionPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, nodeSelectionPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>(nodeSelectionPredicate))
        {
            transformation(node.Content);
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity> transformation,
        [DisallowNull] Func<TransformableEntity, bool> transformationPredicate,
        [DisallowNull] Func<Node<TransformableEntity>, bool> nodeSelectionPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, transformationPredicate, nodeSelectionPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>(nodeSelectionPredicate))
        {
            if (transformationPredicate(node.Content))
            {
                transformation(node.Content);
            }
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity, TInput?> transformation,
        TInput? input) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>())
        {
            transformation(node.Content, input);
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity, TInput?> transformation,
        TInput? input,
        [DisallowNull] Func<TransformableEntity, TInput?, bool> transformationPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, transformationPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>())
        {
            if (transformationPredicate(node.Content, input))
            {
                transformation(node.Content, input);
            }
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity, TInput?> transformation,
        TInput? input,
        [DisallowNull] Func<Node<TransformableEntity>, bool> nodeSelectionPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, nodeSelectionPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>(nodeSelectionPredicate))
        {
            transformation(node.Content, input);
        }
        return transformableEntityNode;
    }

    public static Node<TTransformableEntity> Transform<TTransformableEntity, TInput>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Action<TransformableEntity, TInput?> transformation,
        TInput? input,
        [DisallowNull] Func<TransformableEntity, TInput?, bool> transformationPredicate,
        [DisallowNull] Func<Node<TransformableEntity>, bool> nodeSelectionPredicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation, transformationPredicate, nodeSelectionPredicate);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>(nodeSelectionPredicate))
        {
            if (transformationPredicate(node.Content, input))
            {
                transformation(node.Content, input);
            }
        }
        return transformableEntityNode;
    }

    public static CascadeBufferNodeNode<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this Node<TTransformableEntity> transformableEntityNode,
        [DisallowNull] Func<TransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntityNode, transformation);
        foreach (Node<TransformableEntity> node in transformableEntityNode.GetDescendantsAndSelfOfType<TransformableEntity>())
        {
            var output = transformation(node.Content);
        }

    }

    #endregion

    #region IEnumerable<Node<TTransformableEntity>>

    #endregion
}