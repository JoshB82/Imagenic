using Imagenic.Core.Entities.PositionedEntities;
using Imagenic.Core.Utilities.Node;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Entities.CascadeBuffers;







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