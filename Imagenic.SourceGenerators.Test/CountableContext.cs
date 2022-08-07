using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagenic.SourceGenerators.Test
{
    [Generator]
    public class CountableContextGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Console.WriteLine("Source generator execution started.");

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
                                               
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            throw new System.NotImplementedException();
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