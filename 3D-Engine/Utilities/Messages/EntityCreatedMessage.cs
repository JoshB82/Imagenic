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

internal class TranslatableEntityCreatedMessage : TransformableEntityCreatedMessage, IMessage<TranslatableEntityCreatedMessage>
{
    public static MessageInterpolatedStringHandler<TranslatableEntityCreatedMessage> BriefText => $"Position: {0}";
    public static MessageInterpolatedStringHandler<TranslatableEntityCreatedMessage> DetailedText => $"Position: {0}";
    public static MessageInterpolatedStringHandler<TranslatableEntityCreatedMessage> AllText => $"Position: {0}";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}

internal class OrientatedEntityCreatedMessage : TranslatableEntityCreatedMessage, IMessage<OrientatedEntityCreatedMessage>
{
    public static MessageInterpolatedStringHandler<OrientatedEntityCreatedMessage> BriefText => $"Orientation: {0}";
    public static MessageInterpolatedStringHandler<OrientatedEntityCreatedMessage> DetailedText => $"Orientation: {0}";
    public static MessageInterpolatedStringHandler<OrientatedEntityCreatedMessage> AllText => $"Orientation: {0}";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}