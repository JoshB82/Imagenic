using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Entities.TransformableEntities;

public sealed class TransitionLock // Just use Transition?
{
    public float StartTime { get; set; }
    public float EndTime { get; set; }

    internal TransitionLock(float startTime, float endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}

public static class TransformableEntityTransitions
{
    

    #region TTransformableEntity

    

    

    

    

    #endregion

    #region IEnumerable<TTransformableEntity>

    public static IEnumerable<TTransformableEntity> Start<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        float startTime,
        float endTime,
        out TransitionLock transitionLock) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities);
        transitionLock = new(startTime, endTime);
        activeTransitionLocks.Add(transitionLock);
        return transformableEntities;
    }

    public static IEnumerable<TTransformableEntity> End<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities,
        [DisallowNull] params TransitionLock[] transitionLocks) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities, transitionLocks);

        foreach (TransitionLock transitionLock in transitionLocks)
        {
            if (activeTransitionLocks.Remove(transitionLock))
            {
                foreach (TransformableEntity transformableEntity in transformableEntities)
                {
                    AddTransitionToTransformableEntity(transformableEntity, transitionLock);
                }
            }
        }

        return transformableEntities;
    }

    public static IEnumerable<TTransformableEntity> EndAll<TTransformableEntity>(
        [DisallowNull] this IEnumerable<TTransformableEntity> transformableEntities) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntities);

        foreach (TransitionLock transitionLock in activeTransitionLocks)
        {
            if (activeTransitionLocks.Remove(transitionLock))
            {
                foreach (TransformableEntity transformableEntity in transformableEntities)
                {
                    AddTransitionToTransformableEntity(transformableEntity, transitionLock);
                }
            }
        }

        return transformableEntities;
    }

    #endregion
}