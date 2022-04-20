using Imagenic.Core.Utilities.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities.Recursive;

internal static class RecursorCatalogue
{
    internal static Recursor<NodeCycleCheckParams, bool> cycleRecursor = new Recursor<NodeCycleCheckParams, bool>()
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

    internal static Recursor<AncestorFinderParams, IEnumerable<Node>> ancestorRecursor = new Recursor<AncestorFinderParams, IEnumerable<Node>>()
        .WithRepeatingFunction(parameters =>
        {
            parameters.AncestorNodeList.Add(parameters.TrackedNode.Parent);
            return new AncestorFinderParams
            {
                AncestorNodeList = parameters.AncestorNodeList,
                TrackedNode = parameters.TrackedNode.Parent
            };
        })
        .WithStoppingPredicate(parameters => parameters.TrackedNode.Parent is null)
        .WithReturnSelector(parameters => parameters.AncestorNodeList);
}