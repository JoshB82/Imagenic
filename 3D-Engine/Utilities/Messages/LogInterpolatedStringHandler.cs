using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Imagenic.Core.Utilities.Messages;

public sealed class MessageBuilder<TMessage> where TMessage : IMessage<TMessage>
{
    #region Fields and Properties

    private bool includeTime, includeProjectName;

    public List<string>? ConstantParameters { get; set; }
    public List<Func<string>>? ParametersToBeResolved { get; set; }

    #endregion

    #region Constructors

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        this.includeTime = includeTime;
        this.includeProjectName = includeProjectName;
    }

    #endregion

    #region Methods

    public MessageBuilder<TMessage> AddParameter<TParam>(
        TParam? param,
        bool includeParamName = false,
        [CallerArgumentExpression("param")] string? paramName = default)
    {
        ConstantParameters ??= new List<string>();
        
        if (includeParamName)
        {
            ConstantParameters.Add($"{paramName} : {param?.ToString()}");
        }
        else
        {
            ConstantParameters.Add(param?.ToString());
        }
        
        return this;
    }

    public MessageBuilder<TMessage> AddParameter(string constantParameter)
    {
        (ConstantParameters ??= new List<string>()).Add(constantParameter);
        return this;
    }

    public MessageBuilder<TMessage> AddParameter(Func<string> resolver)
    {
        (ParametersToBeResolved ??= new List<Func<string>>()).Add(resolver);
        return this;
    }

    public string Build()
    {
        return Defaults.Default.OutputVerbosity switch
        {
            Verbosity.Brief => TMessage.BriefText(this),
            Verbosity.Detailed => TMessage.DetailedText(this),
            Verbosity.All => TMessage.AllText(this),
            _ => string.Empty
        };
    }

    public U BuildIntoException<U>(Exception? innerException = null) where U : Exception
    {
        var args = new List<object> { Build() };
        if (innerException is not null)
        {
            args.Add(innerException);
        }

        return (U)Activator.CreateInstance(typeof(U), args.ToArray())!;
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

    #endregion



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

internal enum ConstantParameter { }
internal enum ResolvableParameter { }



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
        builder.Append(t switch
        {
            int paramNum => messageBuilder.ParametersToBeResolved?[paramNum](),
            null => "null",
            _ => t.ToString()
        });

        if (t is int paramNumber)
        {
            builder.Append(messageBuilder.ParametersToBeResolved?[paramNumber]());
        }
        else if (t is null)
        {
            builder.Append("null");
        }
        else
        {
            builder.Append(t?.ToString());
        }
    }

    internal string GetFormattedText() => builder.ToString();
}