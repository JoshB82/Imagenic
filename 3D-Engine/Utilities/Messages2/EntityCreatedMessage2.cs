namespace Imagenic.Core.Utilities.Messages2;

internal class EntityCreatedMessage2 : IMessage
{
    public static string BriefText => "Created.";
    public static string DetailedText => "Created.";
    public static string AllText => "Created with parameters: {ParameterCollection}";
}