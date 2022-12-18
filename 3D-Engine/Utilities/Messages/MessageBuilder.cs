using Imagenic.Core.Attributes;
using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Imagenic.Core.Utilities.Messages;

public sealed class MessageBuilder<TMessage> : IMessageBuilder<TMessage> where TMessage : IMessage<TMessage>
{
    #region Fields and Properties

    private Verbosity verbosity = Defaults.Default.OutputVerbosity;

    private bool includeTime, includeProjectName;

    public List<string>? TypeNames { get; set; }

    #endregion

    #region Constructors

    private static MessageBuilder<TMessage>? instance;
    public static MessageBuilder<TMessage> Instance(bool includeTime = true, bool includeProjectName = true)
    {
        /*instance ??= new MessageBuilder<TMessage>();
        instance.includeTime = includeTime;
        instance.includeProjectName = includeProjectName;
        return instance;*/
        return instance ??= new MessageBuilder<TMessage>(includeTime, includeProjectName);
    }

    public MessageBuilder(bool includeTime = true, bool includeProjectName = true)
    {
        this.includeTime = includeTime;
        this.includeProjectName = includeProjectName;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the specified parameter's value to the message being built.
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="param">The parameter whose value is to be added.</param>
    /// <param name="includeParamName">Indicates whether or not to display the expression passed to this method for the first parameter.</param>
    /// <param name="paramName">The expression passed to this method for the first parameter.</param>
    /// <returns>The current instance.</returns>
    public IMessageBuilder<TMessage> AddParameter<TParam>(
        TParam? param,
        bool includeParamName = false,
        [CallerArgumentExpression("param")] string? paramName = default)
    {
        TMessage.ConstantParameters ??= new List<string>();
        string paramFormatted = param?.ToString() ?? "null";

        if (includeParamName)
        {
            TMessage.ConstantParameters.Add($"({paramName} : {paramFormatted})");
        }
        else
        {
            TMessage.ConstantParameters.Add(paramFormatted);
        }

        return this;
    }

    /*
    public MessageBuilder<TMessage> AddParameter<TParam>(TParam? constantParameter)
    {
        (TMessage.ConstantParameters ??= new List<string>()).Add(constantParameter?.ToString() ?? "null");
        return this;
    }*/

    /// <summary>
    /// Adds the specified type names to the message being built.
    /// </summary>
    /// <typeparam name="TType">The type whose name is to be added.</typeparam>
    /// <returns>The current instance.</returns>
    public IMessageBuilder<TMessage> AddTypeName<TType>() => AddTypeName(typeof(TType).Name);

    /// <summary>
    /// Adds the specified type name to the message being built.
    /// </summary>
    /// <param name="typeName">The type name to be added.</param>
    /// <returns>The current instance.</returns>
    public IMessageBuilder<TMessage> AddTypeName([DisallowNull] [ThrowIfNull] string typeName)
    {
        (TypeNames ??= new List<string>()).Add(typeName);
        return this;
    }

    /// <summary>
    /// Adds the specified verbosity's name to the message being built.
    /// </summary>
    /// <param name="verbosity">The verbosity whose name is to be added.</param>
    /// <returns>The current instance.</returns>
    public IMessageBuilder<TMessage> WithVerbosity(Verbosity verbosity)
    {
        this.verbosity = verbosity;
        return this;
    }

    public IMessageBuilder<TMessage> AddParameter([DisallowNull] [ThrowIfNull] Func<string?> resolvableParameter)
    {
        (TMessage.ResolvableParameters ??= new List<Func<string?>>()).Add(resolvableParameter);
        return this;
    }

    public IMessageBuilder<TMessage> ClearParameters()
    {
        TMessage.ConstantParameters?.Clear();
        TMessage.ResolvableParameters?.Clear();
        return this;
    }

    private static string GetTime() => DateTime.Now.ToString("HH:mm:ss");

    /// <summary>
    /// Generates the message string with all added decorations.
    /// </summary>
    /// <returns>The built string.</returns>
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
        if (TypeNames is not null)
        {
            result += $"[{string.Join(", ", TypeNames)}] ";
        }

        return result + verbosity switch
        {
            Verbosity.Brief => TMessage.BriefText.Build(),
            Verbosity.Detailed => TMessage.DetailedText.Build(),
            Verbosity.All => TMessage.AllText.Build(),
            _ => string.Empty
        };
    }

    public TException BuildIntoException<TException>(Exception? innerException = null) where TException : Exception
    {
        var args = new List<object> { Build() };
        if (innerException is not null)
        {
            args.Add(innerException);
        }

        return (TException)Activator.CreateInstance(typeof(TException), args.ToArray())!;
    }

    #endregion
}