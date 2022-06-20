using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Imagenic.Core.Utilities.Logging;

public interface IMessage
{
    static abstract string BriefText(List<string> l1, List<Func<string>> l2);
    static abstract string DetailedText(List<string> l1, List<Func<string>> l2);
    static abstract string AllText(List<string> l1, List<Func<string>> l2);

    static abstract string Resolve(Verbosity verbosity,
        List<string>? constantParameters,
        List<Func<string>>? parametersToBeResolved,
        MessageInterpolatedStringHandler builder);
}

public sealed class MessageBuilder<T> where T : IMessage
{
    private bool includeTime, includeProjectName;

    public List<string>? ConstantParameters { get; set; }
    public List<Func<string>>? ParametersToBeResolved { get; set; }

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        this.includeTime = includeTime;
        this.includeProjectName = includeProjectName;
    }

    public MessageBuilder<T> AddConstantParameter(string constantParameter)
    {
        (ConstantParameters ??= new List<string>()).Add(constantParameter);
        return this;
    }

    public MessageBuilder<T> AddConditionalParameter(Func<string> resolver)
    {
        (ParametersToBeResolved ??= new List<Func<string>>()).Add(resolver);
        return this;
    }

    public string Build()
    {
        return testVerbosity switch
        {
            Verbosity.Brief => T.BriefText(ConstantParameters, ParametersToBeResolved),
            Verbosity.Detailed => T.DetailedText(ConstantParameters, ParametersToBeResolved),
            Verbosity.All => T.AllText(ConstantParameters, ParametersToBeResolved),
            _ => ""
        };
    }




    private string GetTime() => DateTime.Now.ToString();

    

    private static Verbosity testVerbosity = Verbosity.All;

    

    private static void Log(string s)
    {
        Trace.WriteLine(s);
    }

    

    public void Display()
    {
        //if (includeTime)
        switch (testVerbosity)
        {
            case Verbosity.Brief:
                Log($"{T.BriefText}");
                break;
        }

        return testVerbosity switch
        {
            Verbosity.None => string.Empty,
            Verbosity.Brief => T.BriefText,
            Verbosity.Detailed => T.DetailedText,
            Verbosity.All => T.AllText,
            _ => ""
        };
    }
}



public sealed class TestMessage : IMessage
{
    public static string BriefText(List<string> l1, List<Func<string>> l2) => Resolve(Verbosity.Brief, l1, l2, $"Brief text with difficult parameter {0}");
    public static string DetailedText(List<string> l1, List<Func<string>> l2) => Resolve(Verbosity.Detailed, l1, l2, $"Detailed text with difficult parameters {0} and {1}");
    public static string AllText(List<string> l1, List<Func<string>> l2) => Resolve(Verbosity.All, l1, l2, $"All text!!! {0}, {1}, {2}");

    public static string Resolve(Verbosity verbosity,
        List<string>? constantParameters,
        List<Func<string>>? parametersToBeResolved,
        [InterpolatedStringHandlerArgument("verbosity", "parametersToBeResolved")] MessageInterpolatedStringHandler builder)
    {
        if (verbosity == Verbosity.None)
        {
            return string.Empty;
        }

        string message = builder.GetFormattedText();



        if (includeTime)
        {
            message = $"[{GetTime()}] {message}";
        }


        return message;
    }


    public static void LogBriefText()
    {
        new MessageBuilder<TestMessage>()
            .Log(Verbosity.All, new List<Func<string>>(), $"This is my brief text with a difficult to calculate parameter: {0}");
    }
}

[InterpolatedStringHandler]
public ref struct MessageInterpolatedStringHandler
{
    private readonly StringBuilder builder;
    private readonly Verbosity messageVerbosity;
    private readonly List<Func<string>> parameterResolvers;

    internal MessageInterpolatedStringHandler(int literalLength, int formattedCount, Verbosity verbosity, List<Func<string>> parameters)
    {
        builder = new StringBuilder(literalLength);
        messageVerbosity = verbosity;
        parameterResolvers = parameters;
    }

    internal void AppendLiteral(string s)
    {
        if (messageVerbosity == Verbosity.None)
        {
            return;
        }
        builder.Append(s);
    }

    internal void AppendFormatted<T>(T t)
    {
        if (messageVerbosity == Verbosity.None)
        {
            return;
        }

        if (t is int paramNumber)
        {
            builder.Append(parameterResolvers[paramNumber]());
        }
        else
        {
            builder.Append(t?.ToString());
        }
    }

    internal string GetFormattedText() => builder.ToString();
}