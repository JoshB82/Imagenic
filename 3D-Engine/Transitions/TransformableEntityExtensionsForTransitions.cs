using Imagenic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Transitions;

/// <summary>
/// Provides extension methods for deriving Transitions from transformations.
/// </summary>
public static class TransformableEntityExtensionsForTransitions
{
    #region Fields and Properties

    private static readonly List<Transition> activeTransitions = new();

    #endregion

    #region Methods

    internal static bool AddToActiveTransitions<TEntity>(Action<TEntity> transformation) where TEntity : Entity
    {
        if (activeTransitions.Count > 0)
        {
            foreach (Transition activeTransition in activeTransitions)
            {
                activeTransition.Append(transformation);
            }
            return false;
        }
        return true;
    }

    /// <summary>
    /// Starts a <see cref="Transition"/> at the specified time lasting until the specified end time.
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="startTime">The start time of the created <see cref="Transition"/>.</param>
    /// <param name="endTime">The end time of the created <see cref="Transition"/>.</param>
    /// <param name="transition">The created <see cref="Transition"/>. Terminate this transition with an End method.</param>
    /// <returns></returns>
    public static TTransformableEntity Start<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        float startTime,
        float endTime,
        out Transition transition) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity);

        transition = new(startTime, endTime);
        activeTransitions.Add(transition);

        return transformableEntity;
    }

    /// <summary>
    /// Starts a <see cref="Transition"/> at the specified time lasting the specified <see cref="TimeSpan"/>.
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntity">The <typeparamref name="TTransformableEntity"/> being transformed by the created <see cref="Transition"/>.</param>
    /// <param name="startTime">The start time of the created <see cref="Transition"/>.</param>
    /// <param name="timeSpan"></param>
    /// <param name="transition">The created <see cref="Transition"/>. Terminate this transition with an End method.</param>
    /// <returns></returns>
    public static TTransformableEntity Start<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        float startTime,
        TimeSpan timeSpan,
        out Transition transition) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity);

        transition = new(startTime, startTime + (float)timeSpan.TotalSeconds);
        activeTransitions.Add(transition);

        return transformableEntity;
    }

    /// <summary>
    /// Ends one or more <see cref="Transition">Transitions</see>.
    /// </summary>
    /// <typeparam name="TTransformableEntity"></typeparam>
    /// <param name="transformableEntity"></param>
    /// <param name="transitions">The transitions to end.</param>
    /// <returns></returns>
    public static TTransformableEntity End<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        [DisallowNull] params Transition[] transitions) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity, transitions);
        if (transitions.Length == 0)
        {
            // throw exception?
        }

        foreach (Transition transition in transitions)
        {
            if (activeTransitions.Remove(transition))
            {
                AddTransitionToTransformableEntity(transformableEntity, transition);
            }
        }

        return transformableEntity;
    }

    public static TTransformableEntity EndAll<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity) where TTransformableEntity : TransformableEntity
    {
        foreach (Transition transition in activeTransitions)
        {
            if (activeTransitions.Remove(transition))
            {
                AddTransitionToTransformableEntity(transformableEntity, transition);
            }
        }

        return transformableEntity;
    }

    public static Transition<T, int> ToTransition<T>(this Range range, float timeStart, float timeEnd, Action<T, int> transformation) where T : Entity
    {
        return new(range.Start.Value, range.End.Value + 1, timeStart, timeEnd, transformation);
    }

    #endregion
}