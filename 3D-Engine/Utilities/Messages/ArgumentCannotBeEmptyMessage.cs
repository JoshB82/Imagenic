using System.Collections.Generic;
using System;

namespace Imagenic.Core.Utilities.Messages;

internal sealed class ArgumentCannotBeEmptyMessage : IMessage<ArgumentCannotBeEmptyMessage>
{
    public static MessageInterpolatedStringHandler<ArgumentCannotBeEmptyMessage> BriefText => $"Argument cannot be empty.";
    public static MessageInterpolatedStringHandler<ArgumentCannotBeEmptyMessage> DetailedText => $"The supplied argument cannot be empty.";
    public static MessageInterpolatedStringHandler<ArgumentCannotBeEmptyMessage> AllText => $"The argument {0} cannot be empty.";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}