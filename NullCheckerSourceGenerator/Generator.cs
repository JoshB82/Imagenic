using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // Begin namespace and class
            SourceBuilder.BeginNamepsaceAndClass();

            foreach (var mds in mdsList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(mds.SyntaxTree);
                var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(mds, context.CancellationToken).Symbol;

                /* A null check is required on a method if:
                 * - It contains one or more parameters decorated with the ThrowIfNull attribute;
                 * - All call sites (including those in the consumer's code) have one or more of those parameters as possibly null.
                 */

                bool nullCheckRequired = methodSymbol.Parameters.Any(p => p.GetAttributes().Any(a => Type.GetType(a.AttributeClass.Name) == typeof(ThrowIfNullAttribute)));

                SourceBuilder.AddMethod(new ProcessedMethod
                {
                    MethodSymbol = methodSymbol,
                    NullCheckRequired = nullCheckRequired
                });
            }

            // End class and namespace
            SourceBuilder.EndClassAndNamespaceClass();

            context.AddSource("NullChecks.generated.cs", SourceBuilder.sb.ToString());

            // ---

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

/*SourceText sourceText = SourceText.From($@"namespace NullCheckerSourceGenerator
                                                       {{
                                                            public static class
                                                            {{
                                                            }}
                                                       }}");*/