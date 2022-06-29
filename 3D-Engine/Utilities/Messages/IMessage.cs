using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

public interface IMessage<TMessage> where TMessage : IMessage<TMessage>
{
    static abstract MessageInterpolatedStringHandler<TMessage> BriefText { get; }
    static abstract MessageInterpolatedStringHandler<TMessage> DetailedText { get; }
    static abstract MessageInterpolatedStringHandler<TMessage> AllText { get; }

    static abstract List<string>? ConstantParameters { get; set; }
    static abstract List<Func<string?>>? ResolvableParameters { get; set; }
}