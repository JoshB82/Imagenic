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

/*using Imagenic.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System;
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
}*/
/*
using Imagenic.Core.Entities;
using System.Diagnostics.CodeAnalysis;
using System;
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
}*/