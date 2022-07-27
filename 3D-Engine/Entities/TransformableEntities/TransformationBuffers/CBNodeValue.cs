using Imagenic.Core.Utilities.Node;

namespace Imagenic.Core.CascadeBuffers;

public sealed class CascadeBufferNodeValue<TTransformableEntity, TValue>
{
    #region Fields and Properties

    public Node<TTransformableEntity> TransformableEntityNode { get; }
    public TValue? Value { get; }

    #endregion

    #region Constructors

    public CascadeBufferNodeValue(Node<TTransformableEntity> transformableEntityNode, TValue? value)
    {
        TransformableEntityNode = transformableEntityNode;
        Value = value;
    }

    #endregion

    #region Methods



    #endregion
}

public static class CascadeBufferNodeValueExtensions
{

}