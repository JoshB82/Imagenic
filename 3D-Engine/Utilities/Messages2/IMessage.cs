namespace Imagenic.Core.Utilities.Messages2;

public interface IMessage
{
    abstract static string BriefText { get; }
    abstract static string DetailedText { get; }
    abstract static string AllText { get; }
}