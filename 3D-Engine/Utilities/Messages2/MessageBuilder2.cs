using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Utilities.Messages2;

public class MessageBuilder2<TMessage> where TMessage : IMessage
{
    #region Fields and Properties

    public Verbosity OutputVerbosity { get; set; } = Defaults.Default.OutputVerbosity;
    public bool IncludeTime { get; set; }
    public bool IncludeProjectName { get; set; }

    public Dictionary<string, string> Parameters { get; set; } = new();
    public List<string>? TypeNames { get; set; }

    #endregion

    #region Methods

    public void AddParameter<TValue>(string paramName, TValue? paramValue)
    {
        string paramValueString;

        if (paramValue is null)
        {
            paramValueString = "null";
        }
        else
        {
            paramValueString = paramValue.ToString() ?? "null";
        }

        Parameters.Add(paramName, paramValueString);
    }

    public MessageBuilder2<TMessage> AddTypeName<TType>() => AddTypeName(typeof(TType).Name);
    public MessageBuilder2<TMessage> AddTypeName([DisallowNull] string typeName)
    {
        (TypeNames ??= new List<string>()).Add(typeName);
        return this;
    }
    public MessageBuilder2<TMessage> WithVerbosity(Verbosity verbosity)
    {
        OutputVerbosity = verbosity;
        return this;
    }

    private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

    public string Build()
    {
        if (OutputVerbosity == Verbosity.None)
        {
            return string.Empty;
        }

        string? result = null;

        if (IncludeTime)
        {
            result = $"[{GetTime()}] ";
        }
        if (IncludeProjectName)
        {
            result += $"[{Constants.ProjectName}] ";
        }
        if (TypeNames is not null)
        {
            result += $"[{string.Join(", ", TypeNames)}] ";
        }

        return result + OutputVerbosity switch
        {
            Verbosity.Brief => TMessage.BriefText,
            Verbosity.Detailed => TMessage.DetailedText,
            Verbosity.All => TMessage.AllText,
            _ => string.Empty
        };
    }

    #endregion
}