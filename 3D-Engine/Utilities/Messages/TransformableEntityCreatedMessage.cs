using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal class TransformableEntityCreatedMessage : EntityCreatedMessage, IMessage<TransformableEntityCreatedMessage>
{
    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> BriefText => throw new NotImplementedException();

    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> DetailedText => throw new NotImplementedException();

    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> AllText => throw new NotImplementedException();

    public static List<string>? ConstantParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public static List<Func<string?>>? ResolvableParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}