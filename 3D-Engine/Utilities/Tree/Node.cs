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

    #endregion
}