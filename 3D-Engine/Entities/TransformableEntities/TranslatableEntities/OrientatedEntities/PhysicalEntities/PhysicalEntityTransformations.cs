using Imagenic.Core.CascadeBuffers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities;

/// <summary>
/// Provides extension methods for transforming physical entities.
/// </summary>
public static class PhysicalEntityTransformations
{
    #region ScaleX method

    #region TPhysicalEntity

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleX<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleX<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity, predicate);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z); }, predicate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleXC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleXC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor, Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region IEnumerable<TPhysicalEntity>

    public static IEnumerable<TPhysicalEntity> ScaleX<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z); });
    }
    
    public static IEnumerable<TPhysicalEntity> ScaleX<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z); }, predicate);
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleXC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z));
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleXC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #endregion

    #region ScaleY method

    #region TPhysicalEntity

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleY<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleY<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity, predicate);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z); }, predicate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleYC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleYC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor, Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region IEnumerable<TPhysicalEntity>

    public static IEnumerable<TPhysicalEntity> ScaleY<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z); });
    }

    public static IEnumerable<TPhysicalEntity> ScaleY<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z); }, predicate);
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleYC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z));
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleYC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y * scaleFactor, e.Scaling.z), predicate);
    }

    #endregion

    #endregion

    #region ScaleZ method

    #region TPhysicalEntity

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleZ<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TPhysicalEntity ScaleZ<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity, predicate);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor); }, predicate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleZC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleZC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, float scaleFactor, Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region IEnumerable<TPhysicalEntity>

    public static IEnumerable<TPhysicalEntity> ScaleZ<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor); });
    }

    public static IEnumerable<TPhysicalEntity> ScaleZ<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor); }, predicate);
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleZC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, float scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor));
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleZC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        float scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x, e.Scaling.y, e.Scaling.z * scaleFactor), predicate);
    }

    #endregion

    #endregion

    #region Scale method

    #region TPhysicalEntity

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static TPhysicalEntity Scale<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z); });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static TPhysicalEntity Scale<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity,
        Vector3D scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity, predicate);
        return physicalEntity.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z); }, predicate);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPhysicalEntity"></typeparam>
    /// <param name="physicalEntity"></param>
    /// <param name="scaleFactor"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static CascadeBufferValueValue<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        [DisallowNull] this TPhysicalEntity physicalEntity, Vector3D scaleFactor, Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z), predicate);
    }

    #endregion

    #region IEnumerable<TPhysicalEntity>

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z); });
    }

    public static IEnumerable<TPhysicalEntity> Scale<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        Vector3D scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => { e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z); }, predicate);
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities, Vector3D scaleFactor) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z));
    }

    public static TransformationBufferEnumerableEnumerable<TPhysicalEntity, Vector3D> ScaleC<TPhysicalEntity>(
        [DisallowNull] this IEnumerable<TPhysicalEntity> physicalEntities,
        Vector3D scaleFactor,
        [DisallowNull] Func<TPhysicalEntity, bool> predicate) where TPhysicalEntity : PhysicalEntity
    {
        ThrowIfNull(physicalEntities, predicate);
        return physicalEntities.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z), predicate);
    }

    #endregion

    #endregion
}