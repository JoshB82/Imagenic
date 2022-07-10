using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal class ElementInputTooSmallMessage : IMessage<ElementInputTooSmallMessage>
{
    public static MessageInterpolatedStringHandler<ElementInputTooSmallMessage> BriefText => $"Not enough input elements.";

    public static MessageInterpolatedStringHandler<ElementInputTooSmallMessage> DetailedText => $"There were not enough input elements.";

    public static MessageInterpolatedStringHandler<ElementInputTooSmallMessage> AllText => $"There were not enough input elements.";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}