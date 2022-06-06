using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferValueValue<TEntity, TValue>
{
    #region Fields and Properties

    public TEntity Entity { get; }
    public TValue? Value { get; }

    #endregion

    #region Constructors

    internal CascadeBufferValueValue(TEntity entity, TValue value)
    {
        Entity = entity;
        Value = value;
    }

    #endregion

    #region Methods

    public TEntity Transform([DisallowNull] Action<TEntity, TValue?> transformation)
    {
        ThrowIfParameterIsNull(transformation);
        transformation(Entity, Value);
        return Entity;
    }

    public TEntity Transform<TInput>([DisallowNull] Action<TEntity, TValue?, TInput?> transformation, TInput? transformationInput)
    {
        ThrowIfParameterIsNull(transformation);
        transformation(Entity, Value, transformationInput);
        return Entity;
    }

    public CascadeBufferValueValue<TEntity, TOutput?> Transform<TOutput>([DisallowNull] Func<TEntity, TValue?, TOutput?> transformation)
    {
        ThrowIfParameterIsNull(transformation);
        var output = transformation(Entity, Value);
        return new CascadeBufferValueValue<TEntity, TOutput?>(Entity, output);
    }

    public CascadeBufferValueValue<TEntity, TOutput?> Transform<TInput, TOutput>([DisallowNull] Func<TEntity, TValue?, TInput?, TOutput?> transformation, TInput? transformationInput)
    {
        ThrowIfParameterIsNull(transformation);
        var output = transformation(Entity, Value, transformationInput);
        return new CascadeBufferValueValue<TEntity, TOutput?>(Entity, output);
    }

    #endregion
}

public static class CascadeBufferValueValueExtensions
{
    #region TranslateX method

    public static TTranslatableEntity TranslateX<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); });
    }

    public static CascadeBufferValueValue<TTranslatableEntity, float> TranslateXC<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); return value + distance; });
    }

    #endregion

    #region TranslateY method

    public static TTranslatableEntity TranslateY<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); });
    }

    public static CascadeBufferValueValue<TTranslatableEntity, float> TranslateYC<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); return value + distance; });
    }

    #endregion

    #region TranslateZ method

    public static TTranslatableEntity TranslateZ<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); });
    }

    public static CascadeBufferValueValue<TTranslatableEntity, float> TranslateZC<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); return value + distance; });
    }

    #endregion

    #region Translate method

    public static TTranslatableEntity Translate<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += value + displacement; });
    }

    public static CascadeBufferValueValue<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        this CascadeBufferValueValue<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => translatableEntity.WorldOrigin += value + displacement);
    }

    #endregion

    #region Orientate method

    public static TOrientatedEntity Orientate<TOrientatedEntity>(
        this CascadeBufferValueValue<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => { orientatedEntity.WorldOrientation = value; });
    }

    public static CascadeBufferValueValue<TOrientatedEntity, Orientation> OrientateC<TOrientatedEntity>(
        this CascadeBufferValueValue<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => orientatedEntity.WorldOrientation = value);
    }

    #endregion

    #region ScaleX method

    public static TPhysicalEntity ScaleX<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z);
        });
    }

    public static CascadeBufferValueValue<TPhysicalEntity, float> ScaleXC<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z);
            return value * scaleFactor;
        });
    } 

    #endregion

    #region ScaleY method

    public static TPhysicalEntity ScaleY<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z);
        });
    }

    public static CascadeBufferValueValue<TPhysicalEntity, float> ScaleYC<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z);
            return value * scaleFactor;
        });
    }

    #endregion

    #region ScaleZ method

    public static TPhysicalEntity ScaleZ<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor);
        });
    }

    public static CascadeBufferValueValue<TPhysicalEntity, float> ScaleZC<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor);
            return value * scaleFactor;
        });
    }

    #endregion

    #region Scale method

    public static TPhysicalEntity Scale<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
        });
    }

    public static TPhysicalEntity Scale<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z);
        });
    }

    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
            return value;
        });
    }

    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferValueValue<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z);
            return new Vector3D(value.x * scaleFactor.x, value.y * scaleFactor.y, value.z * scaleFactor.z);
        });
    }

    #endregion
}