namespace Imagenic.Core.Utilities.Messages;

internal class EdgeCreatedMessage : PhysicalEntityCreatedMessage, IMessage<EdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> BriefText => $"Edge created.";

    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> DetailedText => $"Edge created with parameters {{{0}}}.";

    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> AllText => $"Edge created with parameters {{{0}}}.";
}

internal class DashedEdgeCreatedMessage : EdgeCreatedMessage, IMessage<DashedEdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> BriefText => $"DashedEdge created.";

    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> DetailedText => $"DashedEdge created with sections {{{0}}}.";

    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> AllText => $"DashedEdge created with sections {{{0}}}.";
}