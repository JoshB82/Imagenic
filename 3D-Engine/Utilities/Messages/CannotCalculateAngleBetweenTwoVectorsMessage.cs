using System.Collections.Generic;
using System;

namespace Imagenic.Core.Utilities.Messages;

internal class CannotCalculateAngleBetweenTwoVectorsMessage : IMessage<CannotCalculateAngleBetweenTwoVectorsMessage>
{
    public static MessageInterpolatedStringHandler<CannotCalculateAngleBetweenTwoVectorsMessage> BriefText => $"";
    public static MessageInterpolatedStringHandler<CannotCalculateAngleBetweenTwoVectorsMessage> DetailedText => $"";
    public static MessageInterpolatedStringHandler<CannotCalculateAngleBetweenTwoVectorsMessage> AllText => $"";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}