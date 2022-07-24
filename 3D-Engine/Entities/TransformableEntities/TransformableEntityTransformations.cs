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
public static class TransformableEntityTransformations
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
    public static TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation);

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

    /*public static TTransformableEntity Transform<TTransformableEntity, TData>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        TransformationType transformationType,
        TData data) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity);
        switch (transformationType)
        {
            case TransformationType.Orientation when typeof(TTransformableEntity) == typeof(OrientatedEntity):
                transformableEntity.TransformationsNode.Add(new OrientationNode<TTransformableEntity>((Orientation)data));
                break;
        }
        return transformableEntity;
    }*/

    /// <summary>
    /// Applies a custom transformation, in this case a <see cref="Action{TTransformableEntity}"/>, that has no inputs and outputs. The transformation is only applied if a specified predicate is satisfied (returns <see langword="true"/>).
    /// <remarks>The <typeparamref name="TTransformableEntity"/>, the transformation and predicate cannot be <see langword="null"/>.</remarks>
    /// <para><example>
    /// Using call chaining, a cube is subjected to multiple transformations, including a custom transformation that occurs only 50% of the time.
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
    public static TTransformableEntity Transform<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity> transformation,
        [DisallowNull] Func<TTransformableEntity, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation, predicate);
        if (predicate(transformableEntity))
        {
            transformation(transformableEntity);
        }
        return transformableEntity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static TTransformableEntity Transform<TTransformableEntity, TInput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? input) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation);
        transformation(transformableEntity, input);
        return transformableEntity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <typeparam name="TInput"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transformation"></param>
    /// <param name="input"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TTransformableEntity Transform<TTransformableEntity, TInput>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] Action<TTransformableEntity, TInput?> transformation,
        TInput? input,
        [DisallowNull] Func<TTransformableEntity, TInput?, bool> predicate) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transformation, predicate);
        if (predicate(transformableEntity, input))
        {
            transformation(transformableEntity, input);
        }
        return transformableEntity;
    }

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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TOutput?> transformation) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity));
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TOutput>(
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
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        TInput? transformationInput) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation);
        var outputs = transformableEntities.Select(transformableEntity => transformation(transformableEntity, transformationInput));
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
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
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] Func<TTransformableEntity, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transformation, transformationInputs);
        var outputs = transformableEntities.Zip(transformationInputs, (transformableEntity, transformationInput) =>
        {
            return transformation(transformableEntity, transformationInput);
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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
    public static CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TTransformableEntity, TInput, TOutput>(
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
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(transformableEntities, outputs);
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