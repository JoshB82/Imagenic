using Imagenic.Core.Entities;
using Imagenic.Core.Entities.TransformableEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Transitions;

public abstract class Transition : Entity
{
    #region Fields and Properties

    internal TransformationNode TransformationNodes { get; }

    /// <summary>
    /// Indicates if the current <see cref="Transition"/> instance is occurs at a single time. If <see langword="true"/>, <see cref="TimeEnd"/> - <see cref="TimeStart"/> = 0.
    /// </summary>
    public bool IsInstantaneous { get; }

    private float timeStart, timeEnd;

    /// <summary>
    /// The time relative to t = 0s when this transition starts. This cannot be negative.
    /// </summary>
    public float TimeStart
    {
        get => timeStart;
        set
        {
            if (value < 0)
            {
                // Throw exception
            }
            timeStart = value;
        }
    }

    /// <summary>
    /// The time relative to t = 0s when this transition finishes. This cannot be negative.
    /// </summary>
    public float TimeEnd
    {
        get => timeEnd;
        set
        {
            if (value < 0)
            {
                // Throw exception
            }
            timeEnd = value;
        }
    }

    #endregion

    #region Constructors

    protected Transition(float timeStart, float timeEnd)
    {
        IsInstantaneous = timeEnd - timeStart == 0;
        
        TimeStart = timeStart;
        TimeEnd = timeEnd;
    }

    #endregion
}

// Transformation nodes

public abstract class TransformationNode
{

}

public class TransformationNode<TEntity> : TransformationNode where TEntity : Entity
{
    public Action<TEntity> Transformation { get; }
}

public class TransformationNode<TEntity, TInput> : TransformationNode where TEntity : Entity
{
    public Action<TEntity, TInput> Transformation { get; }
}

public class TransformationCNode<TEntity, TOutput> : TransformationNode where TEntity : Entity
{
    public Func<TEntity, TOutput> Transformation { get; }
}

public class TransformationCNode<TEntity, TInput, TOutput> : TransformationNode where TEntity : Entity
{
    public Func<TEntity, TInput, TOutput> Transformation { get; }
}

// --

public class Transition<TEntity> : Transition where TEntity : Entity
{
    #region Fields and Properties

    public List<Action<TEntity>> Transformations { get; set; }

    #endregion

    #region Constructors

    public Transition(float timeStart, float timeEnd, List<Action<TEntity>> transformations) : base(timeStart, timeEnd)
    {
        Transformations = transformations;
    }

    #endregion
}

public class Transition<TEntity, TValue> : Transition where TEntity : Entity
{
    #region Fields and Parameters

    public TValue Start { get; set; }

    public TValue End { get; set; }

    

    internal Action<TEntity, TValue> Transformation { get; set; }

    #endregion

    #region Constructors

    public Transition(TValue start, TValue end, float timeStart, float timeEnd, Action<TEntity, TValue> transformation)
    {
        Start = start;
        End = end;
        TimeStart = timeStart;
        TimeEnd = timeEnd;
        Transformation = transformation;
    }

    #endregion
}

/// <summary>
/// Provides extension methods for deriving Transitions from transformations.
/// </summary>
public static class TransitionExtensions
{
    #region Fields and Properties

    private static readonly List<Transition> activeTransitions = new();

    #endregion

    #region Methods

    private static void AddTransitionToTransformableEntity(TransformableEntity transformableEntity, Transition transition)
    {
        //transformableEntity.Transitions...
    }

    public static TTransformableEntity Start<TTransformableEntity>(
        [DisallowNull] this TTransformableEntity transformableEntity,
        float startTime,
        float endTime,
        out Transition<TTransformableEntity, > transition) where TTransformableEntity : TransformableEntity
    {
        ThrowIfNull(transformableEntity);
        transition = new(startTime, endTime);
        activeTransitions.Add(transition);
        return transformableEntity;
    }

    public static Transition<T, int> ToTransition<T>(this Range range, float timeStart, float timeEnd, Action<T, int> transformation) where T : Entity
    {
        return new(range.Start.Value, range.End.Value + 1, timeStart, timeEnd, transformation);
    }

    #endregion
}