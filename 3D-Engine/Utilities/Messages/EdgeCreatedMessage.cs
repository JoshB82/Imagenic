namespace Imagenic.Core.Utilities.Messages;

internal class EdgeCreatedMessage : PhysicalEntityCreatedMessage, IMessage<EdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> BriefText => $"Edge created.";

    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> DetailedText => $"Edge created with parameters {{{0}}}.";

    public static new MessageInterpolatedStringHandler<EdgeCreatedMessage> AllText => $"Edge created with parameters {{{0}}}.";
}

internal sealed class DashedEdgeCreatedMessage : EdgeCreatedMessage, IMessage<DashedEdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> BriefText => $"DashedEdge created.";

    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> DetailedText => $"DashedEdge created with sections {{{0}}}.";

    public static new MessageInterpolatedStringHandler<DashedEdgeCreatedMessage> AllText => $"DashedEdge created with sections {{{0}}}.";
}

internal sealed class GradientEdgeCreatedMessage : EdgeCreatedMessage, IMessage<GradientEdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<GradientEdgeCreatedMessage> BriefText => $"GradientEdge created.";

    public static new MessageInterpolatedStringHandler<GradientEdgeCreatedMessage> DetailedText => $"GradientEdge created with parameters {{{0}}}.";

    public static new MessageInterpolatedStringHandler<GradientEdgeCreatedMessage> AllText => $"GradientEdge created with parameters {{{0}}}.";
}

internal sealed class SolidEdgeCreatedMessage : EdgeCreatedMessage, IMessage<SolidEdgeCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<SolidEdgeCreatedMessage> BriefText => $"SolidEdge created.";

    public static new MessageInterpolatedStringHandler<SolidEdgeCreatedMessage> DetailedText => $"SolidEdge created with parameters {{{0}}}.";

    public static new MessageInterpolatedStringHandler<SolidEdgeCreatedMessage> AllText => $"SolidEdge created with parameters {{{0}}}.";
}

internal sealed class DashedEdgeSectionCreatedMessage : EntityCreatedMessage, IMessage<DashedEdgeSectionCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<DashedEdgeSectionCreatedMessage> BriefText => $"DashedEdgeSection created.";
    public static new MessageInterpolatedStringHandler<DashedEdgeSectionCreatedMessage> DetailedText => $"DashedEdgeSection created with {{{0}}}.";
    public static new MessageInterpolatedStringHandler<DashedEdgeSectionCreatedMessage> AllText => $"DashedEdgeSection created with {{{0}}}.";
}