using _3D_Engine.Entities.SceneObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Tree;

public abstract class Node { }

public class Node<T> : Node
{
    #region Fields and Properties

    public bool IsReadOnly => children.IsReadOnly;

    public int Count => children.Count;

    public T Content { get; set; }

    private Node parent;
    public Node Parent
    {
        get => parent;
        set
        {
            parent.RemoveChildren(this);
            parent = value;
            parent.AddChildren(this);
        }
    }

    private IList<Node> children = new List<Node>();
    public IList<Node> Children
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

    #endregion

    #region Constructors

    #endregion

    #region Methods

    public void AddChildren(params Node[] children)
    {
        foreach (Node child in children)
        {
            Children.Add(child);
            child.Parent = this;
        }
    }

    public void AddChildren(IEnumerable<Node> children) => AddChildren(children.ToArray());

    public void RemoveChildren(params Node[] children)
    {
        foreach (Node child in children)
        {
            Children.Remove(child);
            child.Parent = null;
        }
    }

    public void RemoveChildren(IEnumerable<Node> children) => RemoveChildren(children.ToArray());

    public void RemoveChildren(Predicate<Node> predicate)
    {
        foreach (Node child in Children)
        {
            if (predicate(child))
            {
                Children.Remove(child);
            }
        }
    }

    public void RemoveChildren(Predicate<T> predicate)
    {
        foreach (Node child in Children)
        {
            if (predicate(((Node<T>)child).Content))
            {
                Children.Remove(child);
            }
        }
    }

    #endregion
}