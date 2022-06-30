using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal sealed class ArgumentCannotBeNullMessage : IMessage<ArgumentCannotBeNullMessage>
{
    public static MessageInterpolatedStringHandler<ArgumentCannotBeNullMessage> BriefText => $"";
    public static MessageInterpolatedStringHandler<ArgumentCannotBeNullMessage> DetailedText => $"";
    public static MessageInterpolatedStringHandler<ArgumentCannotBeNullMessage> AllText => $"";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}