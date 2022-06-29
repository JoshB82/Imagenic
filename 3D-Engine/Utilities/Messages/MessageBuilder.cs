using Imagenic.Core.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Imagenic.Core.Utilities.Messages;

public sealed class MessageBuilder<TMessage> where TMessage : IMessage<TMessage>
{
    #region Fields and Properties

    private Verbosity verbosity = Defaults.Default.OutputVerbosity;

    private bool includeTime, includeProjectName;

    public List<string>? ConstantParameters { get; set; }
    public List<Func<string>>? ParametersToBeResolved { get; set; }

    #endregion

    #region Constructors

    private static MessageBuilder<TMessage>? instance;
    public static MessageBuilder<TMessage> Instance(bool includeTime = true, bool includeProjectName = true)
    {
        instance ??= new MessageBuilder<TMessage>();
        instance.includeTime = includeTime;
        instance.includeProjectName = includeProjectName;
        return instance;
    }

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        this.includeTime = includeTime;
        this.includeProjectName = includeProjectName;
    }

    #endregion

    #region Methods

    public MessageBuilder<TMessage> WithVerbosity(Verbosity verbosity)
    {
        this.verbosity = verbosity;
        return this;
    }

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

    public MessageBuilder<TMessage> AddParameter<TParam>(TParam? constantParameter)
    {
        (TMessage.ConstantParameters ??= new List<string>()).Add(constantParameter?.ToString() ?? "null");
        //(ConstantParameters ??= new List<string>()).Add(constantParameter);
        return this;
    }

    public MessageBuilder<TMessage> AddParameter([DisallowNull] Func<string?> resolvableParameter)
    {
        ThrowIfParameterIsNull(resolvableParameter);
        (TMessage.ResolvableParameters ??= new List<Func<string?>>()).Add(resolvableParameter);
        //(ParametersToBeResolved ??= new List<Func<string>>()).Add(resolvableParameter);
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