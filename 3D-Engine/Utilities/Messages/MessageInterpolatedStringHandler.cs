using System.Runtime.CompilerServices;
using System.Text;

namespace Imagenic.Core.Utilities.Messages;

[InterpolatedStringHandler]
public ref struct MessageInterpolatedStringHandler<TMessage> where TMessage : IMessage<TMessage>
{
    private readonly StringBuilder builder;

    internal MessageInterpolatedStringHandler(int literalLength, int formattedCount)
    {
        builder = new StringBuilder(literalLength);
        
    }

    internal void AppendLiteral(string s)
    {
        builder.Append(s);
    }

    internal void AppendFormatted<T>(T t)
    {
        builder.Append(t switch
        {
            int paramNum => messageBuilder.ParametersToBeResolved?[paramNum](),
            null => "null",
            _ => t.ToString()
        });

        if (t is int paramNumber)
        {
            builder.Append(messageBuilder.ParametersToBeResolved?[paramNumber]());
        }
        else if (t is null)
        {
            builder.Append("null");
        }
        else
        {
            builder.Append(t?.ToString());
        }
    }

    internal string Build() => builder.ToString();
}