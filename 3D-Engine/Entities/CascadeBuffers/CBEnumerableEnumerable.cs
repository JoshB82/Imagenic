using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Entities.PositionedEntities.OrientatedEntities.PhysicalEntities;
using Imagenic.Core.Entities.TransformableEntities;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class TransformableEntityValuePair<TTransformableEntity, TValue> where TTransformableEntity : TransformableEntity
{
    public TTransformableEntity TransformableEntity { get; set; }
    public TValue Value { get; set; }

    public TransformableEntityValuePair(TTransformableEntity transformableEntity, TValue value)
    {
        TransformableEntity = transformableEntity;
        Value = value;
    }
}

public sealed class CascadeBufferEnumerableEnumerable<TTransformableEntity, TValue> : IEnumerable<TransformableEntityValuePair<TTransformableEntity, TValue>>
    where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public IEnumerable<TTransformableEntity> TransformableEntities { get; }
    public IEnumerable<TValue> Values { get; }

    #endregion

    #region Constructors

    internal CascadeBufferEnumerableEnumerable(IEnumerable<TTransformableEntity> transformableEntities, IEnumerable<TValue> values)
    {
        TransformableEntities = transformableEntities;
        Values = values;
    }

    #endregion

    #region Methods

    public IEnumerable<TTransformableEntity> Transform(Action<TTransformableEntity, TValue> transformation)
    {
        return TransformableEntities.Zip(Values, (entity, value) =>
        {
            //entity.Transitions.Add(new Transition<TTransformableEntity, TValue>()
            transformation(entity, value);
            return entity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>(Action<TTransformableEntity, TValue, TInput> transformation, TInput transformationInput)
    {
        return TransformableEntities.Zip(Values, (entity, value) =>
        {
            transformation(entity, value, transformationInput);
            return entity;
        });
    }

    public IEnumerable<TTransformableEntity> Transform<TInput>(Action<TTransformableEntity, TValue, TInput> transformation, IEnumerable<TInput> transformationInputs)
    {
        return TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            transformation(tuple.First, tuple.Second, tuple.Third);
            return tuple.First;
        });
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TOutput>(Func<TTransformableEntity, TValue, TOutput> transformation)
    {
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TInput, TOutput>(Func<TTransformableEntity, TValue, TInput, TOutput> transformation, TInput transformationInput)
    {
        var outputs = TransformableEntities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value, transformationInput);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
    }

    public CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput> Transform<TInput, TOutput>(Func<TTransformableEntity, TValue, TInput, TOutput> transformation, IEnumerable<TInput> transformationInputs)
    {
        var outputs = TransformableEntities.Zip(Values, transformationInputs).Select(tuple =>
        {
            var output = transformation(tuple.First, tuple.Second, tuple.Third);
            return output;
        });
        return new CascadeBufferEnumerableEnumerable<TTransformableEntity, TOutput>(TransformableEntities, outputs);
    }

    public IEnumerator<TransformableEntityValuePair<TTransformableEntity, TValue>> GetEnumerator()
    {
        return TransformableEntities.Zip(Values, (transformableEntity, value) => new TransformableEntityValuePair<TTransformableEntity, TValue>(transformableEntity, value)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}

public static class CascadeBufferEnumerableEnumerableExtensions
{
    #region TranslateX method

    public static IEnumerable<TTranslatableEntity> TranslateX<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateXC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(value + distance, 0, 0); return value + distance; });
    }

    #endregion

    #region TranslateY method

    public static IEnumerable<TTranslatableEntity> TranslateY<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateYC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, value + distance, 0); return value + distance; });
    }

    #endregion

    #region TranslateZ method

    public static IEnumerable<TTranslatableEntity> TranslateZ<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> TranslateZC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, float> cascadeBuffer, float distance = 0) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += new Vector3D(0, 0, value + distance); return value + distance; });
    }

    #endregion

    #region Translate method

    public static IEnumerable<TTranslatableEntity> Translate<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => { translatableEntity.WorldOrigin += value + displacement; });
    }

    public static CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> TranslateC<TTranslatableEntity>(
        this CascadeBufferEnumerableEnumerable<TTranslatableEntity, Vector3D> cascadeBuffer, Vector3D displacement = new()) where TTranslatableEntity : TranslatableEntity
    {
        return cascadeBuffer.Transform((translatableEntity, value) => translatableEntity.WorldOrigin += value + displacement);
    }

    #endregion

    #region Orientate method

    public static IEnumerable<TOrientatedEntity> Orientate<TOrientatedEntity>(
        this CascadeBufferEnumerableEnumerable<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => { orientatedEntity.WorldOrientation = value; });
    }

    public static CascadeBufferEnumerableEnumerable<TOrientatedEntity, Orientation> OrientateC<TOrientatedEntity>(
        this CascadeBufferEnumerableEnumerable<TOrientatedEntity, Orientation> cascadeBuffer) where TOrientatedEntity : OrientatedEntity
    {
        return cascadeBuffer.Transform((orientatedEntity, value) => orientatedEntity.WorldOrientation = value);
    }

    #endregion

    #region ScaleX method

    public static IEnumerable<TPhysicalEntity> ScaleX<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value * scaleFactor, physicalEntity.Scaling.y, physicalEntity.Scaling.z);
        });
    }

    public static CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleXC<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y * value * scaleFactor, physicalEntity.Scaling.z);
        });
    }

    public static CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleYC<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x, physicalEntity.Scaling.y, physicalEntity.Scaling.z * value * scaleFactor);
        });
    }

    public static CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> ScaleZC<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, float> cascadeBuffer, float scaleFactor = 1) where TPhysicalEntity : PhysicalEntity
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
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
        });
    }

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.y * value.y * scaleFactor.y, scaleFactor.z * value.z * scaleFactor.z);
        });
    }

    public static CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x, physicalEntity.Scaling.y * value.y, physicalEntity.Scaling.z * value.z);
            return value;
        });
    }

    public static CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        this CascadeBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> cascadeBuffer, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        return cascadeBuffer.Transform((physicalEntity, value) =>
        {
            physicalEntity.Scaling = new Vector3D(physicalEntity.Scaling.x * value.x * scaleFactor.x, physicalEntity.Scaling.y * value.y * scaleFactor.y, physicalEntity.Scaling.z * value.z * scaleFactor.z);
            return new Vector3D(value.x * scaleFactor.x, value.y * scaleFactor.y, value.z * scaleFactor.z);
        });
    }

    #endregion
}