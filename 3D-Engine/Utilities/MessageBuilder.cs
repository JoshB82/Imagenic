﻿/*
 *       -3D-Engine-
 *     (c) Josh Bryant
 * https://joshdbryant.com
 *
 * Full license is available in the GitHub repository:
 * https://github.com/JoshB82/3D-Engine/blob/master/LICENSE
 *
 * Code description for this file:
 * Defines a MessageBuilder<T> which creates customisable messages.
 */

using _3D_Engine.Constants;
using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Imagenic.Core.Utilities;

/*
public class MessageBuilder<T> where T : IVerbose, new()
{
    #region Fields and Properties

    private readonly StringBuilder sb = new();
    private readonly List<string> parameters = new();

    #endregion

    #region Constructors

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        if (includeTime)
        {
            sb.Append($"[{GetTime()}]");
        }
        if (includeProjectName)
        {
            AddToMessage($"[{Constants.ProjectName}]");
        }
    }

    #endregion

    #region Methods

    private StringBuilder AddToMessage(string value)
    {
        if (sb.Length > 0)
        {
            sb.Append(' ');
        }

        return sb.Append(value);
    }

    

    public MessageBuilder<T> AddMessageType(MessageType messageType)
    {
        AddToMessage($"[{messageType}]");
        return this;
    }

    public MessageBuilder<T> AddType(Type type)
    {
        AddToMessage($"[{type}]");
        return this;
    }

    public MessageBuilder<T> AddType<U>() => AddType(typeof(U));
    
    public MessageBuilder<T> AddParameters(IEnumerable<string> parameters)
    {
        this.parameters.AddRange(parameters);
        return this;
    }

    public MessageBuilder<T> AddParameters(params string[] parameters) => AddParameters((IEnumerable<string>)parameters);

    private static string GetMessage()
    {
        T text = new();
        return Properties.Settings.Default.Verbosity switch
        {
            Verbosity.Brief => text.BriefVerbosityText,
            Verbosity.Detailed => text.DetailedVerbosityText,
            Verbosity.All => text.AllVerbosityText,
            _ => throw new Exception("Cannot handle setting.")
        };
    }

    public string Build()
    {
        if (Properties.Settings.Default.Verbosity == Verbosity.None)
        {
            return string.Empty;
        }
        else if (parameters.Count == 0)
        {
            return AddToMessage(GetMessage()).ToString();
        }
        else
        {
            return AddToMessage(string.Format(GetMessage(), parameters.ToArray())).ToString();
        }
    }

    

    #endregion
}*/

public static class MessageHelper
{
    public static void DisplayInConsole(this string message)
    {
        if (message == string.Empty)
        {
            return;
        }

        Trace.WriteLine(message);
    }
}



internal enum RangeType
{
    InclusiveStartInclusiveFinish,
    InclusiveStartExclusiveFinish,
    ExclusiveStartInclusiveFinish,
    ExclusiveStartExclusiveFinish
}

#region Messages

internal enum RangeViolationType
{
    TooHigh,
    TooLow
}

internal class NumberOfItemsOutOfRangeMessage : IVerbose
{
    internal string ItemsName { get; set; }
    internal string ContainerName { get; set; }
    internal RangeViolationType ClosestBoundary { get; set; }
    internal float BoundaryValue { get; set; }

    public string BriefVerbosityText => $"The number of {ItemsName} is invalid.";
    public string DetailedVerbosityText => $"The number of {ItemsName} is {(ClosestBoundary == RangeViolationType.Maximum ? "greater" : "less")} than the {ClosestBoundary} number allowed ({BoundaryValue}).";
    public string AllVerbosityText => $"The number of {ItemsName} in {ContainerName} is {(ClosestBoundary == RangeViolationType.Maximum ? "greater" : "less")} than the {ClosestBoundary} number allowed ({BoundaryValue}).";
}

internal class InvalidFileContentMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Invalid file.";
    public string DetailedVerbosityText { get; set; } = "The file contained content that could not be parsed.";
    public string AllVerbosityText { get; set; } = "The file contained the following content that could not be parsed: {0}";
}



internal class ParameterNotSupportedMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Parameter not supported.";
    public string DetailedVerbosityText { get; set; } = "{0} is not supported.";
    public string AllVerbosityText { get; set; } = "The parameter {0} is not supported.";
}



internal class OrientationChangedMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Changed orientation.";
    public string DetailedVerbosityText { get; set; } = "Changed orientation.";
    public string AllVerbosityText { get; set; } = "Changed orientation to: Forward: {0}, Up: {1}, Right: {2}";
}



internal class GeneratingDepthValuesMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Generating values...";
    public string DetailedVerbosityText { get; set; } = "Generating depth values...";
    public string AllVerbosityText { get; set; } = "Generating depth values...";
}

internal class GeneratedDepthValuesMessage : IVerbose
{
    public string BriefVerbosityText { get; set; } = "Generated values.";
    public string DetailedVerbosityText { get; set; } = "Generated depth values.";
    public string AllVerbosityText { get; set; } = "Generated depth values";
}

#endregion