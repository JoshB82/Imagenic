using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Recursive;

public class AncestorFinderParams
{
    public Node TrackedNode { get; set; }
    public List<Node> AncestorNodeList { get; set; } = new List<Node>();
}

public class DescendantFinderParams
{

}

public class NodeCycleCheckParams
{
    public bool ReturnParameter { get; set; }

    public List<Node> NodeTrackerList { get; set; } = new List<Node>();
    public Node TrackedNode { get; set; }
}