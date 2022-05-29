using Imagenic.Core.Utilities.Node;
using System;

namespace Imagenic.Core.Entities.CascadeBuffers;

public sealed class CascadeBufferNode<TEntity, TValue>
{
    #region Fields and Properties

    public Node<TEntity> EntityNode { get; set; }
    public Node<TValue> ValueNode { get; set; }

    #endregion

    #region Constructors

    public CascadeBufferNode(Node<TEntity> entityNode, Node<TValue> valueNode)
    {
        EntityNode = entityNode;
        ValueNode = valueNode;
    }

    #endregion

    #region Methods

    public Node<TEntity> Transform(Action<TEntity, TValue> transformation)
    {
        CascadeBufferHelper.ApplyTransformationToComplexObject(EntityNode, transformation, ValueNode);
        return EntityNode;
    }

    public CascadeBufferNode<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {

    }

    #endregion
}