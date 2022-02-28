using _3D_Engine.Entities.SceneObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Tree;

public abstract class Node
{
    #region Fields and Properties

    private IList<Node> children = new List<Node>();
    public bool IsReadOnly => children.IsReadOnly;
    public int Count => children.Count;

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

    #endregion

    #region Methods

    #region Add

    public void Add(Node item)
    {
        Children.Add(item);
    }

    public void Add<T>(T item)
    {
        Children.Add(new Node<T> { Content = item });
    }

    public void AddChildren(IEnumerable<object> children)
    {
        foreach (object child in children)
        {
            var nodeChild = new Node<object> { Content = child, Parent = this };
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

    public void RemoveChildren<T>(Predicate<T> predicate = null)
    {
        foreach (Node child in Children)
        {
            if (child is T t && ((predicate is not null && predicate(t)) || predicate is null))
            {
                Children.Remove(child);
            }
        }
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
        bool removeFlag = true;
        foreach (Node child in children)
        {
            if (Children.Remove(child))
            {
                child.Parent = null;
            }
            else
            {
                removeFlag = false;
            }
        }

        return removeFlag;
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

    #endregion
}

public class Node<T> : Node
{
    #region Fields and Properties

    public T Content { get; set; }

    #endregion

    #region Constructors

    public Node(T content)
    {
        Content = content;
    }

    #endregion

    #region Methods

    #region Add

    public void AddChildren(IEnumerable<T> children)
    {
        foreach (T child in children)
        {
            Children.Add(new Node<T> { Content = child });
        }
    }

    public void AddChildren(params T[] children) => AddChildren((IEnumerable<T>)children);

    #endregion

    #region Remove

    public bool RemoveChildren(IEnumerable<T> children)
    {

    }

    public bool RemoveChildren(params T[] children) => RemoveChildren((IEnumerable<T>)children);

    public void RemoveChildren(Predicate<Node> predicate)
    {
        foreach (Node child in Children)
        {
            if (predicate(child) && Children.Remove(child))
            {
                child.Parent = null;
            }
        }
    }

    public void RemoveChildren(Predicate<T> predicate)
    {
        foreach (Node child in Children)
        {
            if (child is Node<T> node &&
                predicate(node.Content) &&
                Children.Remove(child))
            {
                child.Parent = null;
            }
        }
    }

    #endregion

    #endregion
}