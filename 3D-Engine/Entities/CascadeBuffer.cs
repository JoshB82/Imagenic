using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Imagenic.Core.Entities;

public sealed class CascadeBuffer<TEntity, TValue>
{
    #region Fields and Properties

    public TEntity Entity { get; set; }
    public TValue Value { get; set; }

    #endregion

    #region Constructors

    public CascadeBuffer(TEntity entity, TValue value)
    {
        Entity = entity;
        Value = value;
    }

    #endregion

    #region Methods

    public TEntity Transform(Action<TEntity, TValue> transformation)
    {
        transformation(Entity, Value);
        return Entity;
    }

    public CascadeBuffer<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var output = transformation(Entity, Value);
        return new CascadeBuffer<TEntity, TOutput>(Entity, output);
    }

    #endregion
}

public sealed class CascadeBufferEnumerable<TEntity, TValue>
{
    #region Fields and Properties

    public IEnumerable<TEntity> Entities { get; set; }
    public IEnumerable<TValue> Values { get; set; }

    #endregion

    #region Constructors

    public CascadeBufferEnumerable(IEnumerable<TEntity> entities, IEnumerable<TValue> values)
    {
        Entities = entities;
        Values = values;
    }

    #endregion

    #region Methods

    public IEnumerable<TEntity> Transform(Action<TEntity, TValue> transformation)
    {
        return Entities.Zip(Values, (entity, value) =>
        {
            transformation(entity, value);
            return entity;
        });
    }

    public CascadeBufferEnumerable<TEntity, TOutput> Transform<TOutput>(Func<TEntity, TValue, TOutput> transformation)
    {
        var outputs = Entities.Zip(Values, (entity, value) =>
        {
            var output = transformation(entity, value);
            return output;
        });
        return new CascadeBufferEnumerable<TEntity, TOutput>(Entities, outputs);
    }

    #endregion
}

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

internal static class CascadeBufferHelper
{
    internal static void ApplyTransformationToComplexObject<TInspect, TEntity, TValue>(TInspect toBeInspected, Action<TEntity, TValue> transformation, Node<TValue> values)
    {
        switch (toBeInspected)
        {
            case TEntity entity:
                transformation(entity, values);
                break;
            case Node node:
                foreach (Node child in node.GetDescendantsAndSelf())
                {
                    ApplyTransformationToComplexObject(child.Content, transformation, values);
                }
                break;
            case IEnumerable<object> objects:
                foreach (object @object in objects)
                {
                    ApplyTransformationToComplexObject(@object, transformation, values);
                }
                break;
        }
    }

    internal static TOutput ApplyTransformationToComplexObject<TInspect, TEntity, TValue, TOutput>(TInspect toBeInspected, Func<TEntity, TValue, TOutput> transformation, TValue value, TOutput outputs)
    {
        return transformation
    }

    internal static Node<TOutput> ApplyTransformationToComplexObject<TInspect, TEntity, TValue, TOutput>(TInspect toBeInspected,
                                                                                                         Func<TEntity, TValue, TOutput> transformation,
                                                                                                         ICollection<TValue> values,
                                                                                                         int valuesPosition,
                                                                                                         Node<TOutput> outputs)
    {
        switch (toBeInspected)
        {
            case TEntity entity:
                return new Node<TOutput>(transformation(entity, values));
            case Node node:
                outputs.Add(node.GetDescendantsAndSelf().Select(child =>
                {
                    if (child.Content is TEntity entity)
                    {
                        transformation(entity, transformation, values, valuesPosition++, )
                    }
                    else
                    {
                        ApplyTransformationToComplexObject(child.Content, transformation, values, outputs);
                    }
                }));
                return outputs;
            case IEnumerable<object> objects:
                outputs.Add(objects.Select(@object => ApplyTransformationToComplexObject(@object, transformation, values, outputs)));
                return outputs;
        }
        return outputs;
    }

    internal static IEnumerable<TOutput> ApplyTransformationToComplexObject<TInspect, TEntity, TValue, TOutput>(TInspect tobeInspected, Func<TEntity, TValue, TOutput> transformation, IEnumerable<TValue> values, IEnumerable<TOutput> outputs)
    {
        
    }    
}