using Imagenic.Core.Attributes;
using Imagenic.Core.Entities;
using Imagenic.SourceGenerators.Test;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Transitions;

[Countable]
public class TransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public Transition FirstTransition { get; }

    public List<Transition> Transitions { get; }

    public TTransformableEntity TransformableEntity { get; }

    #endregion

    #region Constructors

    public TransitionContext(Transition transition)
    {
        FirstTransition = transition;
    }

    #endregion

    #region Methods

    public virtual DoubleTransitionContext<TTransformableEntity> Start(float startTime, float endTime)
    {
        return new DoubleTransitionContext<TTransformableEntity>(FirstTransition, new Transition(startTime, endTime));
    }

    public virtual TTransformableEntity End(Transition transition)
    {

    }

    public TTransformableEntity EndAll([DisallowNull] [ThrowIfNull] params Transition[] transitions)
    {
        if (transitions.Length == 0)
        {
            // throw exception?
        }

        foreach (Transition transition in transitions)
        {
            if (Transitions.Remove(transition))
            {
                TransformableEntity.Transitions.Add(transition);
            }
        }

        return TransformableEntity;
    }

    #endregion
}

public class DoubleTransitionContext<TTransformableEntity> : TransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public Transition SecondTransition { get; }

    #endregion

    #region Constructors

    public DoubleTransitionContext(Transition firstTransition, Transition secondTransition) : base(firstTransition)
    {
        SecondTransition = secondTransition;
    }

    #endregion

    #region Methods

    public override TripleTransitionContext<TTransformableEntity> Start(float startTime, float endTime)
    {
        return new TripleTransitionContext<TTransformableEntity>(FirstTransition, SecondTransition, new Transition(startTime, endTime));
    }

    public TransitionContext<TTransformableEntity> End()
    {

    }

    #endregion
}

public sealed class TripleTransitionContext<TTransformableEntity> : DoubleTransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public Transition ThirdTransition { get; }

    #endregion

    #region Constructors

    public TripleTransitionContext(Transition firstTransition, Transition secondTransition, Transition thirdTransition) : base(firstTransition, secondTransition)
    {
        ThirdTransition = thirdTransition;
    }

    #endregion

    #region Methods

    public DoubleTransitionContext<TTransformableEntity> End(Transition transition)
    {
        return new DoubleTransitionContext<TTransformableEntity>();
    }

    #endregion
}