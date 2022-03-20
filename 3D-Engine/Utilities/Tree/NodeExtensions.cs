using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Tree;

public static class NodeExtensions
{
    public static IEnumerable<Node> GetAncestors(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetAncestorsAndSelf(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetAncestors<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetAncestorsAndSelf<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetDescendants(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node> GetDescendantsAndSelf(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetDescendants<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }

    public static IEnumerable<Node<T>> GetDescendantsAndSelf<T>(this IEnumerable<Node> nodes, Predicate<Node> predicate = null)
    {

    }
}