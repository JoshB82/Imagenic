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

internal class TransformableEntityCreatedMessage : EntityCreatedMessage, IMessage<TransformableEntityCreatedMessage>
{
    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> BriefText => throw new NotImplementedException();

    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> DetailedText => throw new NotImplementedException();

    public static MessageInterpolatedStringHandler<TransformableEntityCreatedMessage> AllText => throw new NotImplementedException();

    public static List<string>? ConstantParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public static List<Func<string?>>? ResolvableParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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

internal class PhysicalEntityCreatedMessage : OrientatedEntityCreatedMessage, IMessage<PhysicalEntityCreatedMessage>
{
    public new static MessageInterpolatedStringHandler<PhysicalEntityCreatedMessage> BriefText => $"Parameters: {{{0}, {1}, {2}, {3}}}";

    public new static MessageInterpolatedStringHandler<PhysicalEntityCreatedMessage> DetailedText => $"With parameters: {{{0}, {1}, {2}, {3}}}";

    public new static MessageInterpolatedStringHandler<PhysicalEntityCreatedMessage> AllText => $"With parameters: {{{0}, {1}, {2}, {3}}}";
}

internal class VertexCreatedMessage : TranslatableEntityCreatedMessage, IMessage<VertexCreatedMessage>
{
    public new static MessageInterpolatedStringHandler<VertexCreatedMessage> BriefText => $"Parameters: {{{0}, {1}}}";
    public new static MessageInterpolatedStringHandler<VertexCreatedMessage> DetailedText => $"With parameters: {{{0}, {1}}}";
    public new static MessageInterpolatedStringHandler<VertexCreatedMessage> AllText => $"With parameters: {{{0}, {1}}}";
}

internal class RenderingEntityCreatedMessage : OrientatedEntityCreatedMessage, IMessage<RenderingEntityCreatedMessage>
{

}

internal class MeshCreatedMessage : PhysicalEntityCreatedMessage, IMessage<MeshCreatedMessage>
{

}