using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Imagenic.Core.Utilities.Logging;

public interface IMessage<TMessage> where TMessage : IMessage<TMessage>
{
    static abstract string BriefText(MessageBuilder<TMessage> messageBuilder);
    static abstract string DetailedText(MessageBuilder<TMessage> messageBuilder);
    static abstract string AllText(MessageBuilder<TMessage> messageBuilder);
}

public sealed class MessageBuilder<TMessage> where TMessage : IMessage<TMessage>
{
    private bool includeTime, includeProjectName;

    public List<string>? ConstantParameters { get; set; }
    public List<Func<string>>? ParametersToBeResolved { get; set; }

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        this.includeTime = includeTime;
        this.includeProjectName = includeProjectName;
    }

    public MessageBuilder<TMessage> AddConstantParameter(string constantParameter)
    {
        (ConstantParameters ??= new List<string>()).Add(constantParameter);
        return this;
    }

    public MessageBuilder<TMessage> AddConditionalParameter(Func<string> resolver)
    {
        (ParametersToBeResolved ??= new List<Func<string>>()).Add(resolver);
        return this;
    }

    public string Build()
    {
        return testVerbosity switch
        {
            Verbosity.Brief => TMessage.BriefText(this),
            Verbosity.Detailed => TMessage.DetailedText(this),
            Verbosity.All => TMessage.AllText(this),
            _ => string.Empty
        };
    }

    internal string Resolve([InterpolatedStringHandlerArgument("")] MessageInterpolatedStringHandler<TMessage> builder)
    {
        if (testVerbosity == Verbosity.None)
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

public sealed class TestMessage : IMessage<TestMessage>
{
    public static string BriefText(MessageBuilder<TestMessage> mb) => mb.Resolve($"Brief text with difficult parameter {0}");
    public static string DetailedText(MessageBuilder<TestMessage> mb) => mb.Resolve($"Detailed text with difficult parameters {0} and {1}");
    public static string AllText(MessageBuilder<TestMessage> mb) => mb.Resolve($"All text!!! {0}, {1}, {2}");
}

[InterpolatedStringHandler]
public ref struct MessageInterpolatedStringHandler<TMessage> where TMessage : IMessage<TMessage>
{
    private readonly StringBuilder builder;
    private readonly MessageBuilder<TMessage> messageBuilder;

    internal MessageInterpolatedStringHandler(int literalLength, int formattedCount, MessageBuilder<TMessage> messageBuilder)
    {
        builder = new StringBuilder(literalLength);
        this.messageBuilder = messageBuilder;
    }

    internal void AppendLiteral(string s)
    {
        builder.Append(s);
    }

    internal void AppendFormatted<T>(T t)
    {
        if (t is int paramNumber)
        {
            builder.Append(messageBuilder.ParametersToBeResolved?[paramNumber]());
        }
        else
        {
            builder.Append(t?.ToString());
        }
    }

    internal string GetFormattedText() => builder.ToString();
}