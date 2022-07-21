﻿using Imagenic.Core.Entities;
using Imagenic.Core.Entities.TransformableEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Transitions;

public class Transition : Entity
{
    #region Fields and Properties

    internal List<TransformationNode> TransformationNodes { get; } = new();

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

    private Transition(float timeStart, float timeEnd, TransformationNode transformationNode)
    {
        IsInstantaneous = timeEnd - timeStart == 0;

        TimeStart = timeStart;
        TimeEnd = timeEnd;

        TransformationNodes = transformationNode;
    }

    #endregion

    #region Methods

    // Input: None
    // Output: None
    protected void Append<TEntity>(Action<TEntity> transformation) where TEntity : Entity
    {
        TransformationNodes.Add(new TransformationNode<TEntity>(transformation));
    }

    // Input: Non-output value
    // Output: None
    protected void Append<TEntity, TInput>(Action<TEntity, TInput> transformation, TInput input) where TEntity : Entity
    {
        var valueNode = new TransformationInputNode<TInput>(input);
        var transformationNode = new TransformationNode<TEntity, TInput>(transformation) { Parent = valueNode };
        TransformationNodes.Add(transformationNode);
    }

    // Input: Output value
    // Output: None
    protected void Append<TEntity, TInput, TOutput>(Action<TEntity, TInput> transformation, TransformationCNode<TEntity, TOutput> inputNode) where TEntity : Entity
    {
        var transformationNode = new TransformationNode<TEntity, TInput>(transformation) { Parent = inputNode };
        TransformationNodes.Add(transformationNode);
    }

    // Input: Output value
    // Output: None
    protected void Append<TEntity, TInput1, TInput2, TOutput>(Action<TEntity, TInput1> transformation, TransformationCNode<TEntity, TInput2, TOutput> inputNode) where TEntity : Entity
    {
        var transformationNode = new TransformationNode<TEntity, TInput1>(transformation) { Parent = inputNode };
        TransformationNodes.Add(transformationNode);
    }

    //...


    protected void Append<TEntity, TOutput>(Func<TEntity, TOutput> transformation) where TEntity : Entity
    {
        var transformationNode = new TransformationCNode<TEntity, TOutput>(transformation);
        
    }

    protected void Append<TEntity, TInput, TOutput>(Func<TEntity, TInput, TOutput> transformation) where TEntity : Entity
    {
        var transformationNode = new TransformationCNode<TEntity, TInput, TOutput>(transformation);
        
    }

    #endregion
}








/*
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
*/

/*
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
*/

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
        out Transition transition) where TTransformableEntity : TransformableEntity
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