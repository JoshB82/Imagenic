using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Utilities.Tree;

public static class NodeExtensions
{
    public static IEnumerable<Node<T>> ToNodes<T>(this IEnumerable<T> elements)
    {
        return elements.Select(element => new Node<T>(element));
    }

    public static bool IsPartOfCycle(this IEnumerable<Node> nodes)
    {

    }

    #region Merge

    public static Node<IEnumerable<object>> MergeWith(this IEnumerable<Node> nodes, Node newParent, IEnumerable<Node> otherNodes)
    {
        return new Node<IEnumerable<object>>(nodes.Select(node => node.Content).Concat(otherNodes.Select(node => node.Content)), newParent);
    }

    public static Node<IEnumerable<T>> MergeWith<T>(this IEnumerable<Node<T>> nodes, Node newParent, IEnumerable<Node<T>> otherNodes)
    {
        return new Node<IEnumerable<T>>(nodes.Select(node => node.Content).Concat(otherNodes.Select(node => node.Content)), newParent);
    }

    public static Node<IEnumerable<object>> MergeWith(this IEnumerable<Node> nodes, Node newParent, params Node[] otherNodes) => nodes.MergeWith(newParent, (IEnumerable<Node>)otherNodes);

    public static Node<IEnumerable<T>> MergeWith<T>(this IEnumerable<Node<T>> nodes, Node newParent, params Node<T>[] otherNodes) => nodes.MergeWith(newParent, (IEnumerable<Node<T>>)otherNodes);

    #endregion

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

    /// <summary>
    /// Retrieves all contents from a sequence of <see cref="Node">Nodes</see>.
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static IEnumerable<object> GetAllContents(this IEnumerable<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            yield return ((dynamic)node).Content;
        }
    }

    /// <summary>
    /// Retrieves all contents from a sequence of <see cref="Node">Nodes</see> that satisfy a specified predicate.
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<object> GetAllContents(this IEnumerable<Node> nodes, Predicate<Node> predicate)
    {
        foreach (Node node in nodes)
        {
            if (predicate(node))
            {
                yield return ((dynamic)node).Content;
            }
        }
    }

    public static IEnumerable<T> GetAllContents<T>(this IEnumerable<Node<T>> nodes, Predicate<Node<T>> predicate = null)
    {
        return nodes.Select(node => node.Content);
    }

    #region GetAncestors

    /// <summary>
    /// Retrieves all distinct ancestor <see cref="Node">Nodes</see> from a sequence of <see cref="Node">Nodes</see>.
    /// </summary>
    /// <param name="nodes">The sequence to retrieve ancestor <see cref="Node">Nodes</see> from.</param>
    /// <returns>All distinct ancestor <see cref="Node">Nodes</see>.</returns>
    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes)
    {
        var uniqueNodes = new List<Node>();
        foreach (Node node in nodes)
        {
            var newAncestors = node.GetAncestors(x => !uniqueNodes.Contains(x));
            foreach (Node newAncestor in newAncestors)
            {
                yield return newAncestor;
            }
            uniqueNodes.AddRange(newAncestors);
        }
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

    /// <summary>
    /// Retrieves all distinct ancestor <see cref="Node">Nodes</see> from a sequence of <see cref="Node">Nodes</see>, where the level difference between any two <see cref="Node">Nodes</see> cannot exceed a specified value.
    /// </summary>
    /// <param name="nodes">The sequence to retrieve ancestor <see cref="Node">Nodes</see> from.</param>
    /// <param name="maxLevelDiff">The maximum difference in level between two <see cref="Node">Nodes</see>.</param>
    /// <returns>All distinct ancestor <see cref="Node">Nodes</see>.</returns>
    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes, int maxLevelDiff)
    {
        var uniqueNodes = new List<Node>();
        foreach (Node node in nodes)
        {

        }
    }

    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes, int maxLevelDiff, Predicate<Node> predicate)
    {
        var uniqueNodes = new List<Node>();
        foreach (Node node in nodes)
        {

        }
    }

    #endregion

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