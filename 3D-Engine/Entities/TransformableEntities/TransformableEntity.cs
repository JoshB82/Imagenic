using Imagenic.Core.Transitions;
using Imagenic.Core.Utilities.Messages;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities;

/// <summary>
/// An <see cref="Entity"/> that can be transformed.
/// </summary>
[Serializable]
public abstract class TransformableEntity : Entity
{
    #region Fields and Properties

    public List<Transition> Transitions { get; set; } = new();

    //public List<TransformationNode> TransformationNodes { get; set; } = new();

    #if DEBUG

    //private protected override IMessageBuilder<TransformableEntityCreatedMessage>? MessageBuilder => (IMessageBuilder<TransformableEntityCreatedMessage>?)base.MessageBuilder;

    #endif

    #endregion

    #region Constructors

    /*
    #if DEBUG

    private protected TransformableEntity(IMessageBuilder<TransformableEntityCreatedMessage> mb) : base(mb)
    {

    }

    #else

    public TransformableEntity() : base()
    {
    }

    #endif

    */

    public TransformableEntity()
    {
        
    }

    #endregion

    #region Methods

    // Copy
    public override TransformableEntity ShallowCopy() => (TransformableEntity)MemberwiseClone();
    public override TransformableEntity DeepCopy()
    {
        var transformableEntity = (TransformableEntity)base.DeepCopy();
        return transformableEntity;
    }

    /*public void ResolveNodes()
    {
        foreach (Node node in TransformationsNode.GetDescendantsAndSelf())
        {
            if (node is Action)
        }
    }

    public void Compress()
    {
        var newQueue = new Queue<Transition>();
        Transformations.Zip(Transformations.Skip(1), (a, b) =>
        {
            if (a.TransformationType == b.TransformationType)
            {
                switch (a.TransformationType)
                {
                    case TransformationType.Translation:
                        newQueue.Enqueue(new Transition(a.Transformation, a.Input) { TransformationType = TransformationType.Translation });
                        break;
                }
            }
        });
        foreach (Transition transition in Transformations.Skip(1))
        {
            if (transition.Transformation == transition[])
            {

            }
        }*/
    }

#endregion


    //public EventList<Transition> Transitions { get; set; } = new();
    //internal Queue<Transition> Transformations { get; set; } = new();

    //ExpressionTree
    /*
#region Methods

    
    /*
    public void Resolve() => Resolve<object>(null);

    internal void Resolve<TInput>(TInput? input)
    {
        if (!Transformations.TryDequeue(out Transition? transition)) return;

        switch (transition.Transformation)
        {
            case Action<TransformableEntity> action:
                action(this);
                Resolve<object>(null);
                break;
            case Action<TransformableEntity, object?> action when input is null:
                action(this, transition.Input);
                Resolve<object>(null);
                break;
            case Action<TransformableEntity, TInput?> action:
                action(this, input);
                Resolve<object>(null);
                break;
            case Func<TransformableEntity, TInput?> func:
                Resolve<TInput?>(func(this));
                break;
            case Func<TransformableEntity, object?, object?> func when input is null:
                Resolve<object?>(func(this, transition.Input));
                break;
            case Func<TransformableEntity, TInput?, object?> func:
                Resolve<object?>(func(this, input));
                break;
            default:
                // Unsupported transformation
                break;
        }
    }

#endregion
}

/*
public abstract class Transition
{
    public Delegate Transformation => ((dynamic)this).Transformation;
    public object? Input => ((dynamic)this).Input;

    public TransformationType TransformationType { get; }

    public static Transition<TDelegate, TInput> Combine<TDelegate, TInput>(Transition<TDelegate, TInput> transition) where TDelegate : Delegate
    {

    }

    public bool PassOn { get; }
}*/

/*
public sealed class Translation : Transition
{
    public Vector3D Displacement { get; set; }

    public Translation(Vector3D displacement)
    {
        Displacement = displacement;
    }
}*/

/*
public sealed class Scaling : Transition
{
    public Vector3D ScaleFactor { get; set; }
    public Scaling(Vector3D scaling)
    {
        ScaleFactor = scaling;
    }
}*/

/*
public sealed class Transition<TDelegate, TInput> : Transition where TDelegate : Delegate
{
    public new TDelegate Transformation { get; }
    public new TInput? Input { get; }

    public Transition(TDelegate transformation, TInput? input)
    {
        Transformation = transformation;
        Input = input;
    }
}*/