﻿using Imagenic.Core.Utilities.Tree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Imagenic.Core.Utilities;

public interface IRecursiveParameters<TReturn>
{
    TReturn ReturnParameter { get; set; }
}

public class Recursor<TParams, TReturn> where TParams : IRecursiveParameters<TReturn>
{
    #region Fields and Properties

    public int? MaxRunCount { get; set; }
    public int RunCount { get; private set; }

    public Func<TParams, TReturn> ReturnSelector { get; set; }
    public Func<TParams, TParams> RepeatingFunction { get; set; }
    public Predicate<TParams> StoppingPredicate { get; set; }

    #endregion

    #region Methods

    public Recursor<TParams, TReturn> WithRepeatingFunction(Func<TParams, TParams> repeatingFunction)
    {
        RepeatingFunction = repeatingFunction;
        return this;
    }

    public Recursor<TParams, TReturn> WithStoppingPredicate(Predicate<TParams> predicate)
    {
        StoppingPredicate = predicate;
        return this;
    }

    public Recursor<TParams, TReturn> WithReturnSelector(Func<TParams, TReturn> returnSelector)
    {
        ReturnSelector = returnSelector;
        return this;
    }

    private TReturn Repeat(TParams initialParams)
    {
        TParams persistingParameters = initialParams;

        while (!StoppingPredicate(persistingParameters))
        {
            if (MaxRunCount == RunCount)
            {
                break;
            }
            persistingParameters = RepeatingFunction(persistingParameters);
            RunCount++;
        }

        return ReturnSelector(persistingParameters);
    }

    public TReturn Run(TParams initialParams) => Repeat(initialParams);
    public async Task<TReturn> RunAsync(TParams initialParams) => await Task.Run(() => Repeat(initialParams));

    #endregion

    /*
    private TReturn Repeat(TParams parameters, Func<TParams, RepeatingFunctionResult<TParams, TReturn>> repeatingFunction)
    {
        RunCount++;
        var result = repeatingFunction(parameters);
        if (result.CeaseRecursion)
        {
            return result.ReturnParameter;
        }
        return Repeat(result.NewParameters, repeatingFunction);
    }*/

    /*
    public TReturn Run(TParams parameters, Func<TParams, RepeatingFunctionResult<TParams, TReturn>> repeatingFunction)
    {
        return Repeat(parameters, repeatingFunction);
    }*/
}

public class RepeatingFunctionResult<TParams>
{
    public TParams NewParameters { get; set; }
}

public class NodeCycleCheckParams : IRecursiveParameters<bool>
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