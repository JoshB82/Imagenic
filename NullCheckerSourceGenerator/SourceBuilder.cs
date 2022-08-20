using System.Linq;
using System.Text;

namespace NullCheckerSourceGenerator
{
    internal static class SourceBuilder
    {
        #region Fields and Properties

        internal static StringBuilder sb = new StringBuilder();

        #endregion

        #region Methods

        internal static void BeginNamepsaceAndClass()
        {
            sb.AppendLine($@"namespace NullCheckerSourceGenerator
                             {{
                                 public static partial class TransformableEntityTransformations
                                 {{");
        }

        internal static void AddMethod(ProcessedMethod processedMethod)
        {
            // Obtain method info
            var methodName = processedMethod.MethodSymbol.Name;
            var formattedTypeParameters = processedMethod.MethodSymbol.TypeParameters.Length > 0
                                        ? $"<{string.Join(", ", processedMethod.MethodSymbol.TypeParameters)}>"
                                        : string.Empty;
            var parametersWithType = string.Join(", ", processedMethod.MethodSymbol.Parameters.Select(p => $"{p.Type.Name} {p.Name}"));
            var parameters = string.Join(", ", $"{processedMethod.MethodSymbol.Parameters.Select(p => p.Name)}");

            /* Example generated method:
                 * public static void ClientMethodName<TypeParameters>(string param1, string param2)
                 * {
                 *      ThrowIfNull(param1, param2);
                 * }
                 */

            sb.AppendLine($@"public static void {methodName}{formattedTypeParameters}({parametersWithType})
                                {{
                                    {(processedMethod.NullCheckRequired
                                    ? $@"// Null check
                                    ThrowIfNull({parameters});"
                                    : string.Empty)}
                                    

                                    // Rest of method logic
                                    {methodName}_Internal({parameters});
                                }}");
        }

        internal static void EndClassAndNamespaceClass()
        {
            sb.Append($@"      }}
                         }}");
        }

        #endregion
    }
}