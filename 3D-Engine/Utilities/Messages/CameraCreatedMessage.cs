namespace Imagenic.Core.Utilities.Messages;

internal class CameraCreatedMessage : RenderingEntityCreatedMessage, IMessage<CameraCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<CameraCreatedMessage> BriefText => $"Camera created.";
    public static new MessageInterpolatedStringHandler<CameraCreatedMessage> DetailedText => $"Camera created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<CameraCreatedMessage> AllText => $"Camera created with parameters {{{0}}}.";
}

internal sealed class OrthogonalCameraCreatedMessage : CameraCreatedMessage, IMessage<OrthogonalCameraCreatedMessage>
{
    public static new MessageInterpolatedStringHandler<OrthogonalCameraCreatedMessage> BriefText => $"OrthogonalCamera created.";
    public static new MessageInterpolatedStringHandler<OrthogonalCameraCreatedMessage> DetailedText => $"OrthogonalCamera created with parameters {{{0}}}.";
    public static new MessageInterpolatedStringHandler<OrthogonalCameraCreatedMessage> AllText => $"OrthogonalCamera created with parameters {{{0}}}.";
}