using Imagenic.Core.CascadeBuffers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.TransformableEntities.TranslatableEntities.OrientatedEntities.PhysicalEntities;

public static class PhysicalEntityTransformations
{
    #region ScaleX method

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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity, predicate);
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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region ScaleY method

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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity, predicate);
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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region ScaleZ method

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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity, predicate);
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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor, e.Scaling.y, e.Scaling.z), predicate);
    }

    #endregion

    #region Scale method

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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity, predicate);
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
        ThrowIfParameterIsNull(physicalEntity);
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
        ThrowIfParameterIsNull(physicalEntity);
        return physicalEntity.Transform(e => e.Scaling = new Vector3D(e.Scaling.x * scaleFactor.x, e.Scaling.y * scaleFactor.y, e.Scaling.z * scaleFactor.z), predicate);
    }

    #endregion
}