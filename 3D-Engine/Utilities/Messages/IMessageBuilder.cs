using Imagenic.Core.Attributes;
using Imagenic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Imagenic.Core.Utilities.Messages;

public interface IMessageBuilder<out TMessage>
{
    //public static abstract IMessageBuilder<TMessage> Instance();

    #region Properties

    public List<string>? TypeNames { get; }

    #endregion

    #region Methods

    public IMessageBuilder<TMessage> AddParameter<TParam>(TParam? parameter,
                                                          bool includeParamName = false,
                                                          [CallerArgumentExpression("parameter")] string? paramName = default);

    public IMessageBuilder<TMessage> UpdateParameter<TValue>(int index, TValue? value);

    public IMessageBuilder<TMessage> AddTypeName<TType>();

    public IMessageBuilder<TMessage> AddTypeName([DisallowNull] [ThrowIfNull] string typeName);

    public IMessageBuilder<TMessage> WithVerbosity(Verbosity verbosity);

    public string Build();

    public TException BuildIntoException<TException>(Exception? innerException = null) where TException : Exception;

    #endregion
}