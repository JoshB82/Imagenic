using Imagenic.Core.Entities.CascadeBuffers;
using Imagenic.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Imagenic.Core.CascadeBuffers;

public sealed class CascadeBufferEnumerableValue<TTransformableEntity, TValue> :
    IEnumerable<TransformableEntityValuePair<TTransformableEntity, TValue?>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Parameters

    public IEnumerable<TTransformableEntity> TransformableEntities { get; }
    public TValue? Value { get; }

    #endregion

    #region Constructors

    internal CascadeBufferEnumerableValue(IEnumerable<TTransformableEntity> transformableEntities, TValue? value)
    {
        TransformableEntities = transformableEntities;
        Value = value;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Applies a transformation, in this case an <see cref="Action{TTransformableEntity, TValue?}"/>, to all elements of the stored sequence.
    /// Each transformation is supplied with a <typeparamref name="TTransformableEntity"/> and the same stored <typeparamref name="TValue"/>.
    /// <remarks>The specified transformation cannot be <see langword="null"/>.</remarks>
    /// </summary>
    /// <param name="transformation"></param>
    /// <returns></returns>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation)
    {
        ThrowIfNull(transformation);
        return TransformableEntities.Select(entity =>
        {
            transformation(entity, Value);
            return entity;
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transformation"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public IEnumerable<TTransformableEntity> Transform([DisallowNull] Action<TTransformableEntity, TValue?> transformation,
                                                       [DisallowNull] Func<TTransformableEntity, bool> predicate)
    {
        ThrowIfNull(transformation, predicate);
        return TransformableEntities.Select(transformableEntity =>
        {
            if (predicate(transformableEntity))
            {
                transformation(transformableEntity, Value);
            }
            return transformableEntity;
        });
    }

    public CascadeBufferEnumerableValue<TTransformableEntity, TOutput> Transform<TOutput>(Func<TTransformableEntity, TValue, TOutput> transformation)
    {
        var outputs = TransformableEntities.Select(entity =>
        {
            var output = transformation(entity, Value);
            return output;
        });
        return new CascadeBufferEnumerableValue<TTransformableEntity, TOutput>(TransformableEntities, outputs.Last());
    }

    public IEnumerator<TransformableEntityValuePair<TTransformableEntity, TValue?>> GetEnumerator()
    {
        return TransformableEntities.Select(transformableEntity => new TransformableEntityValuePair<TTransformableEntity, TValue?>(transformableEntity, Value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}

public static class CascadeBufferEnumerableValueExtensions
{
    #region TranslateX method

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); });
    }

    public static CascadeBufferEnumerableValue<TTranslatableEntity, float> TranslateXC<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) =>
        {
            translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0);
            return value + distance;
        });
    }

    #endregion

    #region TranslateY method

    public static IEnumerable<TTranslatableEntity> TranslateY<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); });
    }

    public static CascadeBufferEnumerableValue<TTranslatableEntity, float> TranslateYC<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) =>
        {
            translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0);
            return value + distance;
        });
    }

    #endregion

    #region TranslateZ method

    public static IEnumerable<TTranslatableEntity> TranslateZ<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); });
    }

    public static CascadeBufferEnumerableValue<TTranslatableEntity, float> TranslateZC<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) =>
        {
            translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance);
            return value + distance;
        });
    }

    #endregion

    #region Translate method

    public static IEnumerable<TTranslatableEntity> Translate<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += value + displacement; });
    }

    public static CascadeBufferEnumerableValue<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        this CascadeBufferEnumerableValue<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) =>
        {
            translatableEntity.WorldOrigin += value + displacement;
            return value + displacement;
        });
    }

    #endregion

    #region Orientate method

    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>(
        this CascadeBufferEnumerableValue<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => { orientatedEntity.WorldOrientation = value!; });
    }

    public static CascadeBufferEnumerableValue<TOrientatedEntity, Orientation> OrientateC<TOrientatedEntity>(
        this CascadeBufferEnumerableValue<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) =>
        {
            orientatedEntity.WorldOrientation = value!;
            return value;
        });
    }

    #endregion

    #region ScaleX method

    public static IEnumerable<TPhysicalEntity> ScaleX<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) => { physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z); });
    }

    public static CascadeBufferEnumerableValue<TPhysicalEntity, float> ScaleXC<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) => { physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z); });
    }

    public static CascadeBufferEnumerableValue<TPhysicalEntity, float> ScaleYC<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) => { physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor); });
    }

    public static CascadeBufferEnumerableValue<TPhysicalEntity, float> ScaleZC<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) => { physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z); });
    }

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) => { physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z); });
    }

    public static CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
            return value;
        });
    }

    public static CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferEnumerableValue<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z);
            return new Vector3D(value.x * scaleFactor.x, value.y * scaleFactor.y, value.z * scaleFactor.z);
        });
    }

    #endregion
}