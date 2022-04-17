using Imagenic.Core.Utilities.Tree;
using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities;

public interface IRecursiveParameters<TReturn>
{
    TReturn ReturnParameter { get; set; }
}

public class Recursor<TParams, TReturn> where TParams : IRecursiveParameters<TReturn>
{
    public int RunCount { get; private set; } = 1;

    private TReturn Repeat(TParams parameters, Func<TParams, RepeatingFunctionResult<TParams, TReturn>> repeatingFunction)
    {
        RunCount++;
        var result = repeatingFunction(parameters);
        if (result.CeaseRecursion)
        {
            return result.ReturnParameter;
        }
        return Repeat(result.NewParameters, repeatingFunction);
    }

    public TReturn Run(TParams parameters, Func<TParams, RepeatingFunctionResult<TParams, TReturn>> repeatingFunction)
    {
        return Repeat(parameters, repeatingFunction);
    }
}

public class RepeatingFunctionResult<TParams, TReturn>
{
    public bool CeaseRecursion { get; set; }
    public TReturn ReturnParameter { get; set; }
    public TParams NewParameters { get; set; }
}

public class NodeCycleCheck_RecursiveParameters : IRecursiveParameters<bool>
{
    public bool ReturnParameter { get; set; }

    public List<Node> NodeTrackerList { get; set; } = new List<Node>();
    public Node TrackedNode { get; set; }
}

/*
public class NodeCycleCheck_Recursor : Recursor<NodeCycleCheck_RecursiveParameters, bool>
{
    protected override NodeCycleCheck_RecursiveParameters Repeat(NodeCycleCheck_RecursiveParameters parameters)
    {
        if (parameters.NodeTrackerList.Contains(parameters.TrackedNode))
        {
            return new NodeCycleCheck_RecursiveParameters
            {
                ReturnParameter = true
            };
        }
        else
        {
            parameters.NodeTrackerList.Add(parameters.TrackedNode);
            return base.Repeat(new NodeCycleCheck_RecursiveParameters
            {
                NodeTrackerList = parameters.NodeTrackerList,
                TrackedNode = parameters.TrackedNode.Parent
            });
        }
    }
}*/