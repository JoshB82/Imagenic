using Imagenic.Core.Entities.CascadeBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.Entities;

public sealed class TransformationBufferEnumerableEnumerable<TTransformableEntity, TValue> :
    IEnumerable<TransformableEntityValuePair<TTransformableEntity, TValue?>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    /// <summary>
    /// The transformable entities being transformed.
    /// </summary>
    public IEnumerable<TTransformableEntity> TransformableEntities { get; }
    /// <summary>
    /// The values stored in this buffer that are available for use in transformations.
    /// </summary>
    public List<IEnumerable<TValue?>> Values { get; }

    #endregion

    #region Constructors

    internal TransformationBufferEnumerableEnumerable(IEnumerable<TTransformableEntity> transformableEntities, List<IEnumerable<TValue?>> values)
    {
        TransformableEntities = transformableEntities;
        Values = values;
    }

    #endregion

    #region Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <returns></returns>
    [return: NotNull]
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation)
    {
        ThrowIfNull(transformation);
        return TransformableEntities.Zip(Values.Last(), (entity, value) =>
        {
            //entity.Transitions.Add(new Transition<TTransformableEntity, TValue>()
            transformation(entity, value);
            return entity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <param name="bufferIndex"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation, int bufferIndex)
    {
        ThrowIfNull(transformation);

        IEnumerable<TValue?> selectedValue;

        if (bufferIndex < Values.Count && bufferIndex >= 0)
        {
            selectedValue = Values[bufferIndex];
        }
        else
        {
            // Throw exception
            throw new Exception();
        }

        return TransformableEntities.Zip(selectedValue, (transformableEntity, value) =>
        {
            transformation(transformableEntity, value);
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <param name="bufferIndexRange"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, IEnumerable<TValue?>> transformation, Range bufferIndexRange)
    {
        ThrowIfNull(transformation);

        IEnumerable<IEnumerable<TValue?>> selectedValues;

        if (bufferIndexRange.End.Value < Values.Count && bufferIndexRange.Start.Value >= 0)
        {
            selectedValues = Values.Take(bufferIndexRange);
        }
        else
        {
            // Throw exception
            throw new Exception();
        }

        return TransformableEntities.Zip(selectedValues, (transformableEntity, value) =>
        {
            transformation(transformableEntity, value);
            return transformableEntity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <param name="valueSelector"></param>
    /// <returns></returns>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, IEnumerable<TValue?>> transformation,
                                                       [DisallowNull] Func<IEnumerable<TValue?>, IEnumerable<TValue?>> valueSelector)
    {
        ThrowIfNull(transformation);

        IEnumerable<IEnumerable<TValue?>> selectedValues = Values.Select(valueSelector);

        return TransformableEntities.Zip(selectedValues, (transformableEntity, value) =>
        {
            transformation(transformableEntity, value);
            return transformableEntity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation,
                                                       [DisallowNull] Func<TTransformableEntity, TValue?, bool> predicate)
    {
        ThrowIfNull(transformation, predicate);
        return TransformableEntities.Zip(Values, (transformableEntity, value) =>
        {
            if (predicate(transformableEntity, value))
            {
                transformation(transformableEntity, value);
            }
            return transformableEntity;
        });
    }

    [return: NotNull]
    public IEnumerable<TTransformableEntity> Transform<TInput>([DisallowNull] Action<TTransformableEntity, TValue?, TInput?> transformation, TInput? transformationInput)
    {
        ThrowIfNull(transformation);
        return TransformableEntities.Zip(Values, (entity, value) =>
        {
            transformation(entity, value, transformationInput);
            return entity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>([DisallowNull] Action<TTransformableEntity, TValue?, TInput?> transformation, TInput? transformationInput, [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, bool> predicate)
    {
        ThrowIfNull(transformation, predicate);
        return TransformableEntities.Zip(Values, (transformableEntity, value) =>
        {
            if (predicate(transformableEntity, value, transformationInput))
            {
                transformation(transformableEntity, value, transformationInput);
            }
            return transformableEntity;
        });
    }

    [return: NotNull]
    public IEnumerable<TTransformableEntity> Transform<TInput>(
        [DisallowNull] Action<TTransformableEntity, TValue?, TInput?> transformation, [DisallowNull] IEnumerable<TInput?> transformationInputs)
    {
        ThrowIfNull(transformation, transformationInputs);
        return TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            transformation(tuple.First, tuple.Second, tuple.Third);
            return tuple.First;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>([DisallowNull] Action<TTransformableEntity, TValue?, TInput?> transformation,
                                                               [DisallowNull] IEnumerable<TInput?> transformationInputs,
                                                               [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, bool> predicate)
    {
        ThrowIfNull(transformation, transformationInputs, predicate);
        return TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            if (predicate(tuple.First, tuple.Second, tuple.Third))
            {
                transformation(tuple.First, tuple.Second, tuple.Third);
            }
            return tuple.First;
        });
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TOutput>([DisallowNull] Func<TTransformableEntity, TValue?, TOutput?> transformation)
    {
        ThrowIfNull(transformation);
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TOutput>(
        [DisallowNull] Func<TTransformableEntity, TValue?, TOutput?> transformation,
        [DisallowNull] Func<TTransformableEntity, TValue?, bool> predicate)
    {
        ThrowIfNull(transformation, predicate);
        var outputs = TransformableEntities.Zip(Values, (transformableEntity, value) =>
        {
            return predicate(transformableEntity, value)
            ? transformation(transformableEntity, value)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TInput, TOutput>(
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, TOutput?> transformation, TInput? transformationInput)
    {
        ThrowIfNull(transformation);
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value, transformationInput);
            return output;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TInput, TOutput>(
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, TOutput?> transformation,
        TInput? transformationInput,
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, bool> predicate)
    {
        ThrowIfNull(transformation, predicate);
        var outputs = TransformableEntities.Zip(Values, (transformableEntity, value) =>
        {
            return predicate(transformableEntity, value, transformationInput)
            ? transformation(transformableEntity, value, transformationInput)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TInput, TOutput>(
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs)
    {
        ThrowIfNull(transformation, transformationInputs);
        var outputs = TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            var output = transformation(tuple.First, tuple.Second, tuple.Third);
            return output;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?> Transform<TInput, TOutput>(
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, TOutput?> transformation,
        [DisallowNull] IEnumerable<TInput?> transformationInputs,
        [DisallowNull] Func<TTransformableEntity, TValue?, TInput?, bool> predicate)
    {
        ThrowIfNull(transformation, transformationInputs, predicate);
        var outputs = TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            return predicate(tuple.First, tuple.Second, tuple.Third)
            ? transformation(tuple.First, tuple.Second, tuple.Third)
            : default;
        });
        return new TransformationBufferEnumerableEnumerable<TTransformableEntity, TOutput?>(TransformableEntities, outputs);
    }

    public IEnumerator<TransformableEntityValuePair<TTransformableEntity, TValue?>> GetEnumerator()
    {
        return TransformableEntities.Zip(Values, (transformableEntity, value) => new TransformableEntityValuePair<TTransformableEntity, TValue?>(transformableEntity, value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}

public static class TransformationBufferEnumerableEnumerableExtensions
{
    #region TranslateX method

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); });
    }

    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateXC<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); return value + distance; });
    }

    #endregion

    #region TranslateY method

    public static IEnumerable<TTranslatableEntity> TranslateY<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); });
    }

    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateYC<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); return value + distance; });
    }

    #endregion

    #region TranslateZ method

    public static IEnumerable<TTranslatableEntity> TranslateZ<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); });
    }

    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateZC<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); return value + distance; });
    }

    #endregion

    #region Translate method

    public static IEnumerable<TTranslatableEntity> Translate<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new())
        where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += value + displacement; });
    }

    public static TransformationBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new())
        where TTranslatableEntity : TranslatableEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((translatableEntity, value) => translatableEntity.WorldOrigin += value + displacement);
    }

    #endregion

    #region Orientate method

    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>(
        [DisallowNull] this TransformationBufferEnumerableEnumerable<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        ThrowIfNull(cascadeBuffer);
        return cascadeBuffer.Transform((orientatedEntity, value) => { if (value is not null) orientatedEntity.WorldOrientation = value; });
    }

    public static TransformationBufferEnumerableEnumerable<TOrientatedEntity, Orientation> OrientateC<TOrientatedEntity>(
        this TransformationBufferEnumerableEnumerable<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => value is null ? orientatedEntity.WorldOrientation = value : orientatedEntity.WorldOrientation);
    }

    #endregion

    #region ScaleX method

    public static IEnumerable<TPhysicalEntity> ScaleX<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z);
        });
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleXC<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z);
            return value * scaleFactor;
        });
    }

    #endregion

    #region ScaleY method

    public static IEnumerable<TPhysicalEntity> ScaleY<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z);
        });
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleYC<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z);
            return value * scaleFactor;
        });
    }

    #endregion

    #region ScaleZ method

    public static IEnumerable<TPhysicalEntity> ScaleZ<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor);
        });
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleZC<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor);
            return value * scaleFactor;
        });
    }

    #endregion

    #region Scale method

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
        });
    }

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, scaleFactor.z * value.z * scaleFactor.z);
        });
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
            return value;
        });
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z);
            return new Vector3D(value.x * scaleFactor.x, value.y * scaleFactor.y, value.z * scaleFactor.z);
        });
    }

    #endregion
}