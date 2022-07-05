using Imagenic.Core.Enums;
using System.Collections.Generic;
using System;

namespace Imagenic.Core.Utilities.Messages;

internal class VectorCannotBeNormalisedMessage : IMessage<VectorCannotBeNormalisedMessage>
{
    public static MessageInterpolatedStringHandler<VectorCannotBeNormalisedMessage> BriefText => $"A vector could not be normalised.";
    public static MessageInterpolatedStringHandler<VectorCannotBeNormalisedMessage> DetailedText => $"A vector could not be normalised.";
    public static MessageInterpolatedStringHandler<VectorCannotBeNormalisedMessage> AllText => $"The following vector could not be normalised: {0}";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}