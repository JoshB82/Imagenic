using Imagenic.Core.Entities;
using Imagenic.Core.Utilities.Node;
using System;

namespace Imagenic.Core.Transitions;

public abstract class TransformationNode : Node { }

public class TransformationNode<TEntity> : TransformationNode where TEntity : Entity
{
    #region Fields and Properties

    public Action<TEntity> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationNode(Action<TEntity> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

// Parent node is input
public class TransformationNode<TEntity, TInput> : TransformationNode where TEntity : Entity
{
    #region Fields and Properties

    public Action<TEntity, TInput> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationNode(Action<TEntity, TInput> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

public class TransformationCNode<TEntity, TOutput> : TransformationNode where TEntity : Entity
{
    #region Fields and Properties

    public Func<TEntity, TOutput> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationCNode(Func<TEntity, TOutput> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

// Parent node is input
public class TransformationCNode<TEntity, TInput, TOutput> : TransformationNode where TEntity : Entity
{
    #region Fields and Properties

    public Func<TEntity, TInput, TOutput> Transformation { get; }

    #endregion

    #region Constructors

    public TransformationCNode(Func<TEntity, TInput, TOutput> transformation)
    {
        Transformation = transformation;
    }

    #endregion
}

public class TransformationInputNode<TInput> : TransformationNode
{
    public TInput Input { get; }

    public TransformationInputNode(TInput input)
    {
        Input = input;
    }
}