using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

public sealed class GradientDirectionCannotBeZeroMessage : IMessage<GradientDirectionCannotBeZeroMessage>
{
    public static MessageInterpolatedStringHandler<GradientDirectionCannotBeZeroMessage> BriefText => $"Invalid gradient direction.";
    public static MessageInterpolatedStringHandler<GradientDirectionCannotBeZeroMessage> DetailedText => $"The gradient direction was invalid.";
    public static MessageInterpolatedStringHandler<GradientDirectionCannotBeZeroMessage> AllText => $"The gradient direction was invalid - it must be non-zero.";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}