namespace Imagenic.Core.Utilities.Messages;

internal class NodeContentTypeCannotBeNodeMessage : IMessage<NodeContentTypeCannotBeNodeMessage>
{
    public static string BriefVerbosityText { get; } = "Node content type cannot be Node.";
    public static string DetailedVerbosityText { get; } = "Node content type cannot be Node.";
    public static string AllVerbosityText { get; } = "Node content type cannot be Node.";

    public static string BriefText(MessageBuilder<NodeContentTypeCannotBeNodeMessage> mb) => mb.Resolve(
        $"Node content type cannot be Node.");
    

    
}