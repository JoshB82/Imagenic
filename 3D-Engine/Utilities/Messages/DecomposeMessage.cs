using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal sealed class DecomposeStartMessage : IMessage<DecomposeStartMessage>
{
    public static new MessageInterpolatedStringHandler<DecomposeStartMessage> BriefText => $"";
    public static new MessageInterpolatedStringHandler<DecomposeStartMessage> DetailedText => $"";
    public static new MessageInterpolatedStringHandler<DecomposeStartMessage> AllText => $"";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}

internal sealed class DecomposeFinishMessage : IMessage<DecomposeFinishMessage>
{
    public static new MessageInterpolatedStringHandler<DecomposeFinishMessage> BriefText => $"";
    public static new MessageInterpolatedStringHandler<DecomposeFinishMessage> DetailedText => $"";
    public static new MessageInterpolatedStringHandler<DecomposeFinishMessage> AllText => $"";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}