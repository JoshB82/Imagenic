using Imagenic.SourceGenerators.Test;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Imagenic.SourceGenerators.CountableContexts
{
    [Generator]
    public class CountableContextGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Trace.WriteLine("Execute method called.");

            var syntaxReceiver = (SyntaxReceiver)context.SyntaxReceiver;

            var cdsList = syntaxReceiver.CandidateClasses;

            if (cdsList.Count == 0)
            {
                return;
            }

            var libraryTypes = new List<ITypeSymbol>();

            bool ends = true;

            foreach (var cds in cdsList)
            {
                var semanticModel = context.Compilation.GetSemanticModel(cds.SyntaxTree);
                var typeSymbol = semanticModel.GetSymbolInfo(cds, context.CancellationToken).Symbol as ITypeSymbol;

                if (ContainsCountableAttribute(typeSymbol))
                {
                    libraryTypes.Add(typeSymbol);
                }
            }

            int max;

            if (ends)
            {
                
            }
            else
            {

            }

            

            // -------------

            //var t = from context.Compilation.GlobalNamespace.GetMembers()
            //where
            var throwIfNullParamInfos = new List<ThrowIfNullParamInfo>();

            //var members = context.Compilation.SyntaxTrees.Where(st => st.GetRoot().DescendantNodes(n => n is MethodDeclarationSyntax));

            var members = context.Compilation.GlobalNamespace.GetMembers();
            foreach (INamespaceOrTypeSymbol namespaceOrTypeSymbol in members)
            {
                if (namespaceOrTypeSymbol is IMethodSymbol methodSymbol)
                {
                    var relevantParameters = methodSymbol.Parameters.Where(p => p.GetAttributes()
                                                                    .Any(a => typeof(ThrowIfNullAttribute) == Type.GetType(a.AttributeClass.Name)));

                }
            }

            Trace.WriteLine("Execute method finished.");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public static bool ContainsCountableAttribute(ITypeSymbol typeSymbol)
        {
            return typeSymbol.GetAttributes().Any(a => Type.GetType(a.AttributeClass.Name) == typeof(CountableAttribute));
        }

        internal static string GenerateThrowIfNullMethodParams() => "";
    }

    internal sealed class ThrowIfNullParamInfo
    {
        public string ParamName { get; set; }
        public string ContainingMethod { get; set; }
    }

    public static class MethodTexts
    {
        public static readonly string TransformText =
        $@"public static partial TTransformableEntity Transform<TTransformableEntity>(
            this TTransformableEntity transformableEntity,
            Action<TTransformableEntity> transformation) where TTransformableEntity : TransformableEntity
        {{
            ThrowIfNull();
        }}";
    }
}