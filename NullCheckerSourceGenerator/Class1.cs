using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullCheckerSourceGenerator
{
    [Generator]
    public class NullCheckerGenerator : ISourceGenerator
    {
        #region Methods

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;

            var iesList = syntaxReceiver.InvocationSites;
            var mdsList = syntaxReceiver.CandidateMethods;

            if (mdsList.Count <= 0 || iesList.Count <= 0)
            {
                return;
            }

            SourceText sourceText = SourceText.From($@"namespace NullCheckerSourceGenerator
                                                       {{
                                                            public static class
                                                            {{
                                                            }}
                                                       }}");

            var clientMethods = new List<IMethodSymbol>();

            foreach (var mds in mdsList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(mds.SyntaxTree);
                var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(mds, context.CancellationToken).Symbol;
                if (methodSymbol.Parameters.Any(p => p.GetAttributes().Any(a => Type.GetType(a.AttributeClass.Name) == typeof(ThrowIfNullAttribute))))
                {
                    // Method is a client for a null check.
                    clientMethods.Add(methodSymbol);
                }
            }

            var sb = new StringBuilder();

            foreach (var clientMethod in clientMethods)
            {
                /* Example generated method:
                 * public static void ThrowIfNull_ClientMethodName(string param1, string param2)
                 * {
                 *      ThrowIfNull(param1, param2);
                 * }
                 */

                sb.AppendLine($@"public static void ThrowIfNull_{clientMethod.Name}({string.Join(", ", clientMethod.Parameters.Select(p => $"{p.Type.Name} {p.Name}"))})
                                {{
                                    ThrowIfNull({string.Join(", ", $"{clientMethod.Parameters.Select(p => p.Name)}")});
                                }}");
            }

            foreach (var ies in iesList)
            {
                var arguments = ies.ArgumentList.Arguments;
                var nullCheckMethodGenerationPossible = arguments.Any(a => !context.Compilation.GetSemanticModel(a.SyntaxTree)
                                                                                               .GetNullableContext(0)
                                                                                               .WarningsEnabled());
                var semanticModel = context.Compilation.GetSemanticModel(ies.SyntaxTree);
                if (semanticModel.GetSymbolInfo(ies, context.CancellationToken).Symbol.Name))
                {

                }
            }

            

            context.AddSource();

            // ---

            // Find methods that contain the ThrowIfNull attribute
            var relevantMethods = context.Compilation.GlobalNamespace.GetTypeMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.Parameters.SelectMany(p => p.GetAttributes())
                                        .Any(a => Type.GetType(a.AttributeClass.Name) == typeof(ThrowIfNullAttribute)));

            // Identify call sites
            Task.Run(async () =>
            {
                foreach (var method in relevantMethods)
                {
                    var workspace = Microsoft.CodeAnalysis.MSBuild.MSBuildWorkspace.
                    //var solution = Assembly.GetCallingAssembly().
                    await SymbolFinder.FindCallersAsync(method, null, context.CancellationToken);
                }
            }, context.CancellationToken);
            
            context.Compilation.GlobalNamespace.
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        #endregion
    }

    public class SyntaxReceiver : ISyntaxReceiver
    {
        #region Fields and Properties

        public List<InvocationExpressionSyntax> InvocationSites { get; private set; } = new List<InvocationExpressionSyntax>();
        public List<MethodDeclarationSyntax> CandidateMethods { get; private set; } = new List<MethodDeclarationSyntax>();

        #endregion

        #region Methods

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            switch (syntaxNode)
            {
                case InvocationExpressionSyntax ies:
                    InvocationSites.Add(ies);
                    break;
                case MethodDeclarationSyntax mds:
                    CandidateMethods.Add(mds);
                    break;
            }
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ThrowIfNullAttribute : Attribute { }
}