using System.Collections.Generic;
using System;

namespace Imagenic.Core.Entities.TransformableEntities;

[Serializable]
public abstract class TransformableEntity : Entity
{
    #region Methods

    public override TransformableEntity ShallowCopy() => (TransformableEntity)MemberwiseClone();
    public override TransformableEntity DeepCopy()
    {
        var transformableEntity = (TransformableEntity)base.DeepCopy();
        return transformableEntity;
    }

    #endregion


    //public EventList<Transition> Transitions { get; set; } = new();
    internal Queue<Transition> Transformations { get; set; } = new();

    //ExpressionTree

    #region Methods

    public void Compress()
    {

    }

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

public abstract class Transition
{
    public Delegate Transformation => ((dynamic)this).Transformation;
    public object? Input => ((dynamic)this).Input;
}

public sealed class Transition<TDelegate, TInput> : Transition where TDelegate : Delegate
{
    public new TDelegate Transformation { get; }
    public new TInput? Input { get; }

    public Transition(TDelegate transformation, TInput? input)
    {
        Transformation = transformation;
        Input = input;
    }
}