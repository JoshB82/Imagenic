using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Utilities.Tree;

public static class NodeExtensions
{
    public static IEnumerable<Node<T>> GetAllNodesOfType<T>(this IEnumerable<Node> nodes, Predicate<Node<T>> predicate = null)
    {
        foreach (Node<T> node in nodes.Where(x => x is Node<T>))
        {
            if (predicate is null || predicate(node))
            {
                yield return node;
            }
        }
    }

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

    public static IEnumerable<T> GetAllContents<T>(this IEnumerable<Node<T>> nodes, Predicate<Node<T>> predicate = null)
    {
        return nodes.Select(node => node.Content);
    }

    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {
        var uniqueNodes = new List<Node>();
        foreach (Node node in nodes)
        {
            var newAncestors = node.GetAncestors(x => !uniqueNodes.Contains(x) && (predicate is null || predicate(x)));
            foreach (Node newAncestor in newAncestors)
            {
                yield return newAncestor;
            }
            uniqueNodes.AddRange(newAncestors);
        }
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