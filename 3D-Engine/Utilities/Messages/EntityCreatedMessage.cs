using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal class EntityCreatedMessage : IMessage<EntityCreatedMessage>
{
    public static MessageInterpolatedStringHandler<EntityCreatedMessage> BriefText => $"Created.";
    public static MessageInterpolatedStringHandler<EntityCreatedMessage> DetailedText => $"Created at {0}.";
    public static MessageInterpolatedStringHandler<EntityCreatedMessage> AllText => $"Entity created at {0}.";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}