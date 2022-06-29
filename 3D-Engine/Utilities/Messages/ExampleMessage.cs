using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

public sealed class ExampleMessage : IMessage<ExampleMessage>
{
    public static MessageInterpolatedStringHandler<ExampleMessage> BriefText => $"Brief text example; parameter: {ConstantParameters[0]}";
    public static MessageInterpolatedStringHandler<ExampleMessage> DetailedText => $"Detailed text example; parameter: {ResolvableParameters[0]}";
    public static MessageInterpolatedStringHandler<ExampleMessage> AllText => $"All text example; parameter: {ConstantParameters[1]}";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }

    /*

    public static string BriefText(MessageBuilder<ExampleMessage> mb) => mb.Resolve(
        $"Brief text example, parameter: {ConstParams.CParam1}");
    public static string DetailedText(MessageBuilder<ExampleMessage> mb) => mb.Resolve(
        $"Detailed text example, parameters: {ResolvableParams.RParam1} and {ConstParams.CParam1}");
    public static string AllText(MessageBuilder<ExampleMessage> mb) => mb.Resolve(
        $"All text example, parameters: {ConstParams.CParam1}, {ConstParams.CParam3}, {ResolvableParams.RParam3}");

    public enum ConstParams
    {
        CParam1,
        CParam2,
        CParam3
    }

    public enum ResolvableParams
    {
        RParam1 = 3,
        RParam2,
        RParam3
    }

    */
}