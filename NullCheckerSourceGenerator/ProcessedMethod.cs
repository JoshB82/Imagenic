using Microsoft.CodeAnalysis;

namespace NullCheckerSourceGenerator
{
    internal sealed class ProcessedMethod
    {
        internal IMethodSymbol MethodSymbol { get; set; }
        internal bool NullCheckRequired { get; set; }
    }
}