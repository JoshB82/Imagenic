using System;
using System.Collections.Generic;

namespace Imagenic.Core.Utilities.Messages;

internal class VectorsAreNotOrthogonalMessage : IMessage<VectorsAreNotOrthogonalMessage>
{
    public static MessageInterpolatedStringHandler<VectorsAreNotOrthogonalMessage> BriefText => $"The vectors must be orthogonal.";

    public static MessageInterpolatedStringHandler<VectorsAreNotOrthogonalMessage> DetailedText => $"The vectors must be orthogonal to each other.";

    public static MessageInterpolatedStringHandler<VectorsAreNotOrthogonalMessage> AllText => $"The vectors {0} {1} must be orthogonal to each other.";

    public static List<string>? ConstantParameters { get; set; }
    public static List<Func<string?>>? ResolvableParameters { get; set; }
}