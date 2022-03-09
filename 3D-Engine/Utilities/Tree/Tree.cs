using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Tree;

public class Tree
{
    #region Fields and Properties

    public IEnumerable<Node> Nodes { get; set; }

    #endregion

    #region Constructors

    public Tree(IEnumerable<Node> nodes)
    {
        Nodes = nodes;
    }

    public Tree(params Node[] nodes) : this((IEnumerable<Node>)nodes) { }
    
    #endregion
}