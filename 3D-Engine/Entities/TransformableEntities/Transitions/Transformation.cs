using System;
using System.Collections.Generic;

namespace Imagenic.Core.Entities.TransformableEntities.Transitions;

#region testing

public class test
{
    public static void Test()
    {
        Cube cube = new Cube();

        //cube.Start(out var t1)
        //    .End(t1);

        //cube.Start(out var t1)
        //    .Start(out var t2)
        //    .End(t1)
        //    .End(t2);

        cube.Start(out var t1)
            .Transform(e => { Console.WriteLine("Hello!"); })
            .Start(out var t2)
            .Start(out var t3)
            .End(t1)
            .End(t2)
            .End(t3);
    }
}

#endregion

#region TransitionContexts

public sealed class TransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TTransformableEntity TransformableEntity { get; }
    public List<Transition<TTransformableEntity>> ActiveTransitions { get; init; } = new();
    public List<Transition<TTransformableEntity>> ResolvedTransitions { get; init; } = new();

    #endregion

    #region Constructors

    public TransitionContext(TTransformableEntity transformableEntity)
    {
        TransformableEntity = transformableEntity;
    }

    public TransitionContext(TTransformableEntity transformableEntity, List<Transition<TTransformableEntity>> activeTransformations, List<Transition<TTransformableEntity>> resolvedTransitions)
    {
        TransformableEntity = transformableEntity;
        ActiveTransitions = activeTransformations;
        ResolvedTransitions = resolvedTransitions;
    }

    #endregion

    #region Methods

    public SecondTransitionContext<TTransformableEntity> Start(out Transition<TTransformableEntity> transition)
    {
        transition = new Transition<TTransformableEntity>();
        ActiveTransitions.Add(transition);

        // Transfer parent entity and transition list information to second context.
        return new SecondTransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
    }

    public (TTransformableEntity, List<Transition<TTransformableEntity>>) End(Transition<TTransformableEntity> transition)
    {
        ActiveTransitions.Remove(transition);
        ResolvedTransitions.Add(transition);
        return (TransformableEntity, ResolvedTransitions);
    }

    // Transformations
    public TransitionContext<TTransformableEntity> Transform(Action<TTransformableEntity> transformation)
    {
        var transformationNode = new TransformationNode<TTransformableEntity>(transformation);
        foreach (Transition<TTransformableEntity> transition in ActiveTransitions)
        {
            if (transition.TransformationNode is null)
            {
                transition.TransformationNode = transformationNode;
            }
            else
            {
                transition.TransformationNode.Add(transformationNode);
            }
        }

        return this;
    }

    #endregion
}

public sealed class SecondTransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TTransformableEntity TransformableEntity { get; }
    public List<Transition<TTransformableEntity>> ActiveTransitions { get; }
    public List<Transition<TTransformableEntity>> ResolvedTransitions { get; }

    #endregion

    #region Constructors

    public SecondTransitionContext(TTransformableEntity transformableEntity, List<Transition<TTransformableEntity>> activeTransitions, List<Transition<TTransformableEntity>> resolvedTransitions)
    {
        TransformableEntity = transformableEntity;
        ActiveTransitions = activeTransitions;
        ResolvedTransitions = resolvedTransitions;
    }

    #endregion

    #region Methods

    public ThirdTransitionContext<TTransformableEntity> Start(out Transition<TTransformableEntity> transition)
    {
        transition = new Transition<TTransformableEntity>();
        ActiveTransitions.Add(transition);

        return new ThirdTransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
    }

    public TransitionContext<TTransformableEntity> End(Transition<TTransformableEntity> transition)
    {
        ActiveTransitions.Remove(transition);
        ResolvedTransitions.Add(transition);
        return new TransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
    }

    public SecondTransitionContext<TTransformableEntity> Transform(Action<TTransformableEntity> transformation)
    {
        var transformationNode = new TransformationNode<TTransformableEntity>(transformation);
        foreach (Transition<TTransformableEntity> transition in ActiveTransitions)
        {
            if (transition.TransformationNode is null)
            {
                transition.TransformationNode = transformationNode;
            }
            else
            {
                transition.TransformationNode.Add(transformationNode);
            }
        }

        return this;
    }

    #endregion
}

public sealed class ThirdTransitionContext<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TTransformableEntity TransformableEntity { get; }
    public List<Transition<TTransformableEntity>> ActiveTransitions { get; }
    public List<Transition<TTransformableEntity>> ResolvedTransitions { get; }

    #endregion

    #region Constructors

    public ThirdTransitionContext(TTransformableEntity transformableEntity, List<Transition<TTransformableEntity>> activeTransitions, List<Transition<TTransformableEntity>> resolvedTransitions)
    {
        TransformableEntity = transformableEntity;
        ActiveTransitions = activeTransitions;
        ResolvedTransitions = resolvedTransitions;
    }

    #endregion

    #region Methods

    /*
    public FourthTransitionContext<TTransformableEntity> Start(out Transition<TTransformableEntity> transition)
    {
        transition = new Transition<TTransformableEntity>();
        ActiveTransitions.Add(transition);

        return new FourthTransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
    }
    */

    public SecondTransitionContext<TTransformableEntity> End(Transition<TTransformableEntity> transition)
    {
        ActiveTransitions.Remove(transition);
        ResolvedTransitions.Add(transition);
        return new SecondTransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
    }

    public ThirdTransitionContext<TTransformableEntity> Transform(Action<TTransformableEntity> transformation)
    {
        var transformationNode = new TransformationNode<TTransformableEntity>(transformation);
        foreach (Transition<TTransformableEntity> transition in ActiveTransitions)
        {
            if (transition.TransformationNode is null)
            {
                transition.TransformationNode = transformationNode;
            }
            else
            {
                transition.TransformationNode.Add(transformationNode);
            }
        }

        return this;
    }

    #endregion
}

/* TransitionContext Template
 * 
 * public sealed class [NUM]TransitionContext
 * {
 *      #region Fields and Properties
 *
 *      public TTransformableEntity TransformableEntity { get; }
 *      public List<Transition<TTransformableEntity>> ActiveTransitions { get; }
 *      public List<Transition<TTransformableEntity>> ResolvedTransitions { get; }
 *
 *      #endregion
 *
 *      #region Constructors
 *
 *      public [NUM]TransitionContext(TTransformableEntity transformableEntity, List<Transition<TTransformableEntity>> activeTransitions)
 *      {
 *          TransformableEntity = transformableEntity;
 *          ActiveTransitions = activeTransitions;
 *      }
 *
 *      #endregion
 *
 *      #region Methods
 *  
 *      public [NUM+1]TransitionContext<TTransformableEntity> Start(out Transition<TTransformableEntity> transition)
 *      {
 *          transition = new Transition<TTransformableEntity>();
 *          ActiveTransitions.Add(transition);
 *
 *          return new [NUM+1]TransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
 *      }
 *
 *      public [NUM-1]TransitionContext<TTransformableEntity> End(Transition<TTransformableEntity> transition)
 *      {
 *          ActiveTransitions.Remove(transition);
 *          ResolvedTransitions.Add(transition);
 *          return new [NUM-1]TransitionContext<TTransformableEntity>(TransformableEntity, ActiveTransitions, ResolvedTransitions);
 *      }
 *
 *      public [NUM]TransitionContext<TTransformableEntity> Transform(Action<TTransformableEntity> transformation)
 *      {
 *          var transformationNode = new TransformationNode<TTransformableEntity>(transformation);
 *          foreach (Transition<TTransformableEntity> transition in ActiveTransitions)
 *          {
 *              if (transition.TransformationNode is null)
 *              {
 *                  transition.TransformationNode = transformationNode;
 *              }
 *              else
 *              {
 *                  transition.TransformationNode.Add(transformationNode);
 *              }
 *          }
 *
 *          return this;
 *      }
 *
 *      #endregion
 * }
 * 
 */

#endregion



public sealed class Transition<TTransformableEntity> where TTransformableEntity : TransformableEntity
{
    #region Fields and Properties

    public TransformationNode<TTransformableEntity> TransformationNode { get; set; }
    

    #endregion

    

    #region Methods

    

    /// <summary>
    /// Run the transition.
    /// </summary>
    /// <returns></returns>
    /*
    public TTransformableEntity Run()
    {
        var nodes = TransformationNode.GetDescendantsAndSelf();
        foreach (TransformationNode<TTransformableEntity> transformationNode in nodes)
        {
            transformationNode.Transformation.Invoke(TransformableEntity);
        }

        return TransformableEntity;
    }*/

    #endregion
}

public static class TransformationExtensions
{
    public static TransitionContext<TTransformableEntity> Start<TTransformableEntity>(this TTransformableEntity transformableEntity, out Transition<TTransformableEntity> transition) where TTransformableEntity : TransformableEntity
    {
        transition = new Transition<TTransformableEntity>();
        return new TransitionContext<TTransformableEntity>(transformableEntity);
    }
}