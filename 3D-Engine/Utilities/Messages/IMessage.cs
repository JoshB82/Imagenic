namespace Imagenic.Core.Utilities.Messages;

public interface IMessage<TMessage> where TMessage : IMessage<TMessage>
{
    static abstract string BriefText(MessageBuilder<TMessage> messageBuilder);
    static abstract string DetailedText(MessageBuilder<TMessage> messageBuilder);
    static abstract string AllText(MessageBuilder<TMessage> messageBuilder);

    //static MessageBuilder<TMessage> mb { get; }
}