using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Imagenic.Core.Utilities.Messages;

public sealed class MessageBuilder<TMessage> where TMessage : IMessage<TMessage>
{
    #region Fields and Properties

    private Verbosity verbosity = Defaults.Default.OutputVerbosity;

    private bool includeTime, includeProjectName;

    public List<Type>? Types { get; set; }

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

    public MessageBuilder<TMessage> AddType<TType>() => AddType(typeof(TType));

    public MessageBuilder<TMessage> AddType([DisallowNull] Type type)
    {
        (Types ??= new List<Type>()).Add(type);
        return this;
    }

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
        return this;
    }

    public MessageBuilder<TMessage> AddParameter([DisallowNull] Func<string?> resolvableParameter)
    {
        ThrowIfParameterIsNull(resolvableParameter);
        (TMessage.ResolvableParameters ??= new List<Func<string?>>()).Add(resolvableParameter);
        return this;
    }

    public MessageBuilder<TMessage> ClearParameters()
    {
        TMessage.ConstantParameters?.Clear();
        TMessage.ResolvableParameters?.Clear();
        return this;
    }

    private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

    public string Build()
    {
        if (verbosity == Verbosity.None)
        {
            return string.Empty;
        }

        string? result = null;

        if (includeTime)
        {
            result = $"[{GetTime()}] ";
        }
        if (includeProjectName)
        {
            result += $"[{Constants.ProjectName}] ";
        }
        if (Types is not null)
        {
            result += $"[{string.Join(", ", Types)}] ";
        }

        return result + verbosity switch
        {
            Verbosity.Brief => TMessage.BriefText.Build(),
            Verbosity.Detailed => TMessage.DetailedText.Build(),
            Verbosity.All => TMessage.AllText.Build(),
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

    #endregion
}