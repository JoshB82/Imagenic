namespace Imagenic.Core.Utilities.Messages;

internal class TriangleCreatedMessage : PhysicalEntityCreatedMessage, IMessage<TriangleCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<TriangleCreatedMessage> BriefText => $"Triangle created.";
    public static new MessageInterpolatedStringHandler<TriangleCreatedMessage> DetailedText => $"Triangle created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<TriangleCreatedMessage> AllText => $"Triangle created with parameters {{{0}}}.";
}

internal sealed class GradientTriangleCreatedMessage : TriangleCreatedMessage, IMessage<GradientTriangleCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<GradientTriangleCreatedMessage> BriefText => $"GradientTriangle created.";
    public static new MessageInterpolatedStringHandler<GradientTriangleCreatedMessage> DetailedText => $"GradientTriangle created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<GradientTriangleCreatedMessage> AllText => $"GradientTriangle created with parameters {{{0}}}.";
}

internal sealed class SolidTriangleCreatedMessage : TriangleCreatedMessage, IMessage<SolidTriangleCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<SolidTriangleCreatedMessage> BriefText => $"SolidTriangle created.";
    public static new MessageInterpolatedStringHandler<SolidTriangleCreatedMessage> DetailedText => $"SolidTriangle created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<SolidTriangleCreatedMessage> AllText => $"SolidTriangle created with parameters {{{0}}}.";
}

internal sealed class TextureTriangleCreatedMessage : TriangleCreatedMessage, IMessage<TextureTriangleCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<TextureTriangleCreatedMessage> BriefText => $"TextureTriangle created.";
    public static new MessageInterpolatedStringHandler<TextureTriangleCreatedMessage> DetailedText => $"TextureTriangle created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<TextureTriangleCreatedMessage> AllText => $"TextureTriangle created with parameters {{{0}}}.";
}