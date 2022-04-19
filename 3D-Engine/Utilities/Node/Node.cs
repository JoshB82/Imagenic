using _3D_Engine.Entities.SceneObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.Core.Utilities.Tree;

public abstract class Node
{
    #region Fields and Properties

    /// <summary>
    /// The content of the <see cref="Node"/>. Use this property when the type of the <see cref="Node{T}"/> is unknown.
    /// </summary>
    public object Content
    {
        get => ((dynamic)this).Content;
        set => ((dynamic)this).Content = value;
    }

    private IList<Node> children = new List<Node>();
    /// <summary>
    /// All child <see cref="Node">Nodes</see> linked to this <see cref="Node"/>.
    /// </summary>
    public virtual IList<Node> Children
    {
        get => children;
        set
        {
            foreach (Node child in children)
            {
                child.Parent = null;
            }
            children = value;
            foreach (Node child in children)
            {
                child.Parent = this;
            }
        }
    }

    private Node parent;
    public Node Parent
    {
        get => parent;
        set
        {
            parent?.RemoveChildren(this);
            (parent = value)?.AddChildren(this);
        }
    }

    #endregion

    #region Methods

    #region Add

    /// <summary>
    /// Links a child <see cref="Node"/> to this <see cref="Node"/>.
    /// </summary>
    /// <param name="item">The child <see cref="Node"/> to be linked.</param>
    public void Add(Node item)
    {
        children.Add(item);
    }

    /// <summary>
    /// Links a child <see cref="Node"/> to this <see cref="Node"/>.
    /// </summary>
    /// <typeparam name="T">The type of the content the <see cref="Node"/> to be added contains.</typeparam>
    /// <param name="item">The content of the child <see cref="Node"/> to be linked.</param>
    public void Add<T>(T item)
    {
        children.Add(new Node<T>(item));
    }

    public void AddChildren(IEnumerable<object> children)
    {
        foreach (object child in children)
        {
            var nodeChild = new Node<object>(child, this);
            this.children.Add(nodeChild);
        }
    }

    public void AddChildren(params object[] children) => AddChildren((IEnumerable<object>)children);

    public void AddChildren(IEnumerable<Node> children)
    {
        foreach (Node child in children)
        {
            this.children.Add(child);
            child.Parent = this;
        }
    }

    public void AddChildren(params Node[] children) => AddChildren((IEnumerable<Node>)children);

    #endregion

    #region Remove

    public bool RemoveChildren(Predicate<Node> predicate)
    {
        bool removalSuccess = true;

        foreach (Node child in children)
        {
            if (predicate(child) && children.Remove(child))
            {
                child.Parent = null;
            }
            else
            {
                removalSuccess = false;
            }
        }

        return removalSuccess;
    }

    public bool RemoveChildrenOfType<T>(Predicate<T> predicate = null)
    {
        bool removalSuccess = true;

        foreach (Node child in children)
        {
            if (child is T t && (predicate is null || predicate(t)) && children.Remove(child))
            {
                child.parent = null;
            }
            else
            {
                removalSuccess = false;
            }
        }

        return removalSuccess;
    }

    public bool RemoveChildren(IEnumerable<object> children)
    {
        foreach (object child in children)
        {
            //Children.FirstOrDefault(x => x.GetType().GetProperty("Content").GetValue() == child);
        }

        return Children.Count == 0;
    }

    public bool RemoveChildren(IEnumerable<Node> children)
    {
        bool noRemovalFailures = true;

        foreach (Node child in children)
        {
            if (Children.Remove(child))
            {
                child.Parent = null;
            }
            else
            {
                noRemovalFailures = false;
            }
        }

        return noRemovalFailures;
    }

    public bool RemoveChildren(params Node[] children) => RemoveChildren((IEnumerable<Node>)children);

    public void Clear()
    {
        foreach (Node child in children)
        {
            this.RemoveChildren(child);
        }
    }

    #endregion

    #region Get

    public IEnumerable<Node> GetAncestorsAndSelf(Predicate<Node> predicate = null)
    {
        foreach (Node node in this.GetAncestors(predicate))
        {
            yield return node;
        }

        if (predicate is null || predicate(this))
        {
            yield return this;
        }
    }

    public IEnumerable<T> GetAllParentsAndSelf<T>(Predicate<T> predicate = null) where T : SceneEntity
    {
        return this.GetAncestorsAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
    }

    /// <summary>
    /// Gets all children and this that are of type <typeparamref name="T"/> and an optional predicate.
    /// </summary>
    /// <typeparam name="T">The type of all the returned <see cref="SceneEntity">SceneObjects</see>.</typeparam>
    /// <param name="predicate">A <see cref="Predicate{T}"/> that all returned <see cref="SceneEntity">SceneObjects</see> must satisfy.</param>
    /// <returns></returns>
    public IEnumerable<Node<T>> GetDescendantsAndSelfOfType<T>(Predicate<T> predicate = null)
    {
        return this.GetDescendantsAndSelf(x => x is T t && predicate(t)) as IEnumerable<T>;
    }

    #region GetAncestors

    public IEnumerable<Node> GetAncestors()
    {
        var uniqueNodes = new List<Node>();
        if (parent is not null)
        {

        }
    }

    public IEnumerable<Node> GetAncestors(Predicate<Node> predicate = null)
    {
        List<Node> parents = new();
        if (Parent is not null && ((predicate is not null && predicate(Parent)) || predicate is null))
        {
            parents.Add(Parent);
            parents.AddRange(Parent.GetAncestors(predicate));
        }
        return parents;
    }

    #endregion

    public IEnumerable<Node<T>> GetAncestorsOfType<T>(Predicate<T> predicate = null)
    {
        return this.GetAncestors(x => x is T t && predicate(t)) as IEnumerable<Node<T>>;
    }

    public IEnumerable<Node> GetDescendants(Predicate<Node> predicate = null)
    {
        List<SceneEntity> children = new();
        foreach (SceneEntity child in Children)
        {
            if (predicate is not null && !predicate(child))
            {
                continue;
            }

            children.Add(child);
            children.AddRange(child.GetDescendants(predicate));
        }
        return children;
    }

    public IEnumerable<Node> GetDescendantsAndSelf(Predicate<Node> predicate = null)
    {
        List<Node> nodes = this.GetDescendantsOfType(predicate).ToList();
        if (predicate is null || predicate(this))
        {
            nodes.Add(this);
        }
        return nodes;
    }

    public IEnumerable<Node<T>> GetDescendantsOfType<T>(Predicate<T> predicate = null)
    {
        return this.GetDescendants(x => x is T t && predicate(t)) as IEnumerable<Node<T>>;
    }

    #endregion

    #region Merge

    public Node<IEnumerable<object>> MergeWith(Node newParent, IEnumerable<Node> otherNodes)
    {
        return new Node<IEnumerable<object>>((new[] { this.Content }).Concat(otherNodes.Select(node => node.Content)), newParent);
    }

    public Node<IEnumerable<object>> MergeWith(Node newParent, params Node[] otherNodes) => this.MergeWith(newParent, (IEnumerable<Node>)otherNodes);

    #endregion

    private static readonly Recursor<NodeCycleCheckParams, bool> cycleRecursor = new Recursor<NodeCycleCheckParams, bool>()
            .WithRepeatingFunction(parameters =>
            {
                parameters.NodeTrackerList.Add(parameters.TrackedNode);
                return new NodeCycleCheckParams
                {
                    NodeTrackerList = parameters.NodeTrackerList,
                    TrackedNode = parameters.TrackedNode.Parent
                };

            })
            .WithStoppingPredicate(parameters =>
            {
                if (parameters.TrackedNode is null)
                {
                    parameters.ReturnParameter = false;
                    return true;
                }

                if (parameters.NodeTrackerList.Contains(parameters.TrackedNode))
                {
                    return parameters.ReturnParameter = true;
                }

                return false;
            })
            .WithReturnSelector(parameters => parameters.ReturnParameter);

    public bool IsPartOfCycle()
    {
        return cycleRecursor.Reset().Run(new NodeCycleCheckParams { TrackedNode = this });
    }
    
    #endregion
}

public class Node<T> : Node
{
    #region Fields and Properties

    /// <summary>
    /// The content of the <see cref="Node"/>.
    /// </summary>
    public new T Content { get; set; }

    #endregion

    #region Constructors

    public Node() { }

    public Node(T content) => Content = content;
    public Node(T content, Node parent) => (Content, Parent) = (content, parent);
    public Node(T content, IList<Node> children) => (Content, Children) = (content, children);
    public Node(T content, Node parent, IList<Node> children) => (Content, Parent, Children) = (content, parent, children);

    #endregion

    #region Methods

    #region Add

    public void AddChildren(IEnumerable<T> children)
    {
        foreach (T child in children)
        {
            Children.Add(new Node<T>(child));
        }
    }

    public void AddChildren(params T[] children) => AddChildren((IEnumerable<T>)children);

    #endregion

    #region Remove

    public bool RemoveChildren(IEnumerable<T> children)
    {
        bool removalSuccess = true;

        var validTypeNodes = Children.GetAllNodesOfType<T>();

        foreach (T child in children)
        {
            var nodeToRemove = validTypeNodes.FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.Content, child));

            if (nodeToRemove is not null && Children.Remove(nodeToRemove))
            {
                nodeToRemove.Parent = null;
            }
            else
            {
                removalSuccess = false;
            }
        }

        return removalSuccess;
    }

    public bool RemoveChildren(params T[] children) => RemoveChildren((IEnumerable<T>)children);

    public bool RemoveChildren(Predicate<T> predicate)
    {
        bool removalSuccess = true;

        foreach (Node child in Children)
        {
            if (child is Node<T> node &&
                predicate(node.Content) &&
                Children.Remove(child))
            {
                child.Parent = null;
            }
            else
            {
                removalSuccess = false;
            }
        }

        return removalSuccess;
    }

    #endregion

    #region Merge

    public Node<IEnumerable<T>> MergeWith(Node newParent, IEnumerable<Node<T>> otherNodes)
    {
        return new Node<IEnumerable<T>>((new[] { this.Content }).Concat(otherNodes.Select(node => node.Content)), newParent);
    }

    public Node<IEnumerable<T>> MergeWith(Node newParent, params Node<T>[] otherNodes) => this.MergeWith(newParent, (IEnumerable<Node<T>>)otherNodes);

    #endregion

    #endregion
}

public class ConstrainedNode<T> : Node<T>
{
    #region Fields and Parameters

    private int minNumberOfChildren;
    private int? maxNumberOfChildren;

    /// <summary>
    /// The minimum number of child nodes this <see cref="Node"/> can have.
    /// <remarks>The value of this property must be non-negative and less than or equal to <see cref="MaxNumberOfChildren"/> if it is not null.</remarks>
    /// </summary>
    public int MinNumberOfChildren
    {
        get => minNumberOfChildren;
        set
        {
            if (value >= 0 && (maxNumberOfChildren is null || value <= maxNumberOfChildren.Value))
            {
                minNumberOfChildren = value;
            }
            else
            {
                // throw exception
            }
        }
    }

    /// <summary>
    /// The maximum number of child nodes this <see cref="Node"/> can have.
    /// <remarks>If this property is null, this limit is removed, otherwise the value of this property must be non-negative and more than or equal to <see cref="MinNumberOfChildren"/>.</remarks>
    /// </summary>
    public int? MaxNumberOfChildren
    {
        get => maxNumberOfChildren;
        set
        {
            if (value is null || (value >= 0 && value >= minNumberOfChildren))
            {
                maxNumberOfChildren = value.Value;
            }
            else
            {
                // throw exception
            }
        }
    }

    public override IList<Node> Children
    {
        get => base.Children;
        set
        {
            if (value.Count < minNumberOfChildren || value.Count > maxNumberOfChildren)
            {
                // throw exception
            }
            base.Children = value;
        }
    }

    #endregion
}