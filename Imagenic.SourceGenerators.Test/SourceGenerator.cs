using System.Collections.Generic;
using System.Text;

namespace Imagenic.SourceGenerators.CountableContexts
{
    internal static class SourceGenerator
    {
        #region Fields and Properties

        internal static StringBuilder SourceBuilder { get; } = new StringBuilder();

        #endregion

        #region Methods

        internal static void BeginNamespaceAndClass()
        {
            SourceBuilder.AppendLine($@"namespace GeneratedContextIterations
                                        {{
                                            public class GeneratedContext");
        }

        internal static string GenerateContextTypeName(string contextGeneralName, int iteration)
        {
            return $"{iteration switch
            {
                1 => string.Empty,
                2 => "Double",
                3 => "Triple",
                _ => $"{iteration}_Iteration"
            }}{contextGeneralName}Context";
        }

        /*internal static string GenerateContextPropertyName(string propertyGeneralName, int iteration)
        {
            return $"{iteration switch
            {
                
                _ => $"{iteration}_Iteration"
            }}";
        }*/

        // New context class
        internal static void BeginNewContextClass(string contextName, int iteration)
        {
            string contextTypeName = GenerateContextTypeName(contextName, iteration);

            string inheritance = iteration > 1
                               ? " : "
                               : string.Empty;

            SourceBuilder.AppendLine($@"public class {contextTypeName}
                                        {{");
        }

        internal static void EndNewContextClass()
        {
            SourceBuilder.AppendLine("}");
        }

        // Constructor
        internal static void AddConstructor(string accessibilityModifier, string contextName, int iteration, List<string> constructorParameters, string listName)
        {
            string contextTypeName = GenerateContextTypeName(contextName, iteration);
            string constructorParametersFormatted = string.Join(", ", constructorParameters);

            /* Example:
            * public DoubleTransitionContext(Transition secondTransition)
            * {
            *   SecondTransition = secondTransition;
            * }
            */
            SourceBuilder.AppendLine($@"{accessibilityModifier} {contextTypeName}({constructorParametersFormatted})
                                        {{
                                            {listName}.Add({constructorParametersFormatted[0]});
                                        }}");
        }

        #endregion
    }
}