using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Tree;

public static class NodeExtensions
{
    public static IEnumerable<object> GetAllContents(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {
        foreach (Node node in nodes)
        {
            if (predicate is null || predicate(node))
            {
                yield return ((dynamic)node).Content;
            }
        }
    }

    public static IEnumerable<T> GetAllContents<T>(this IEnumerable<Node<T>> nodes, Predicate<Node<T>> predicate)
    {
        return nodes.Select(node => node.Content);
    }

    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {
        
    }

    public static IEnumerable<Node> GetAncestorsAndSelf(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {
        var uniqueNodes = nodes.ToList();
        foreach (Node node in nodes)
        {
            yield return node;

            var newAncestors = node.GetAncestors(x => !uniqueNodes.Contains(x) && predicate(x));
            uniqueNodes.AddRange(newAncestors);
            foreach (Node newAncestor in newAncestors)
            {
                yield return newAncestor;
            }
        }
    }

    public static IEnumerable<Node<T>> GetAncestorsOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetAncestorsAndSelfOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetDescendants(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetDescendantsAndSelf(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetDescendantsOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetDescendantsAndSelfOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetAllLinkedNodes(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetAllLinkedNodesAndSelf(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetAllLinkedNodesOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetAllLinkedNodesAndSelfOfType<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }
}