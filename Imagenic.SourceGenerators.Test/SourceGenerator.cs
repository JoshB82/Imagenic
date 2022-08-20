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

        internal static void AddContextClass(string contextName, int iteration)
        {
            string numberName = iteration switch
            {
                1 => string.Empty,
                2 => "Double",
                3 => "Triple",
                _ => $"{iteration}_Iteration"
            };
            SourceBuilder.AppendLine($@"public class {numberName}{contextName}Context
                                        {{");
        }

        #endregion
    }
}