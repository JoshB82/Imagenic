using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NullCheckerSourceGenerator
{
    [Generator]
    public class NullCheckerGenerator : ISourceGenerator
    {
        #region Methods

        public void Execute(GeneratorExecutionContext context)
        {
            Trace.WriteLine("Execute method called.");

            var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;

            var iesList = syntaxReceiver.InvocationSites;
            var mdsList = syntaxReceiver.CandidateMethods;

            if (mdsList.Count <= 0 || iesList.Count <= 0)
            {
                return;
            }

            var consumerMethods = new List<IMethodSymbol>();
            var libraryMethods = new List<IMethodSymbol>();

            /* A null check is required on a method if:
             * - It contains one or more parameters decorated with the ThrowIfNull attribute;
             * - All call sites (including those in the consumer's code) have one or more of those parameters as possibly null.
             */

            foreach (var ies in iesList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(ies.SyntaxTree);
                var methodSymbol = semanticModel.GetSymbolInfo(ies, context.CancellationToken).Symbol as IMethodSymbol;

                if (ContainsAllNotNullArguments(semanticModel, ies, context.CancellationToken))
                {
                    consumerMethods.Add(methodSymbol);
                }
            }

            foreach (var mds in mdsList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(mds.SyntaxTree);
                var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(mds, context.CancellationToken).Symbol;

                if (ContainsParametersWithThrowIfNullAttribute(methodSymbol))
                {
                    libraryMethods.Add(methodSymbol);
                }
            }

            // Begin namespace and class
            SourceBuilder.BeginNamepsaceAndClass();

            foreach (var libraryMethod in libraryMethods)
            {
                bool isNullCheckRequired = consumerMethods.Count(m => m == libraryMethod) > 0;

                SourceBuilder.AddMethod(new ProcessedMethod
                {
                    MethodSymbol = libraryMethod,
                    NullCheckRequired = isNullCheckRequired
                });
            }

            // End class and namespace
            SourceBuilder.EndClassAndNamespaceClass();

            context.AddSource("NullChecks.generated.cs", SourceBuilder.sb.ToString());

            Trace.WriteLine("Execute method finished.");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public static bool ContainsParametersWithThrowIfNullAttribute(IMethodSymbol methodSymbol)
        {
            return methodSymbol.Parameters.Any(p => p.GetAttributes().Any(a => Type.GetType(a.AttributeClass.Name) == typeof(ThrowIfNullAttribute)));
        }

        public static bool ContainsAllNotNullArguments(SemanticModel semanticModel, InvocationExpressionSyntax ies, CancellationToken ct)
        {
            return !ies.ArgumentList.Arguments.Any(a => GetNullableFlowState(semanticModel, a, ct) != NullableFlowState.NotNull);
        }

        public static NullableFlowState GetNullableFlowState(SemanticModel semanticModel, ArgumentSyntax argumentSyntax, CancellationToken ct)
        {
            var info = semanticModel.GetTypeInfo(argumentSyntax, ct);
            return info.Nullability.FlowState;
        }

        #endregion
    }
}

/*SourceText sourceText = SourceText.From($@"namespace NullCheckerSourceGenerator
                                                       {{
                                                            public static class
                                                            {{
                                                            }}
                                                       }}");*/