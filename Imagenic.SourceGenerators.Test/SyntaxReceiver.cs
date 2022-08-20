using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Imagenic.SourceGenerators.CountableContexts
{
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        #region Fields and Properties

        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        #endregion

        #region Methods

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds)
            {
                CandidateClasses.Add(cds);
            }
        }

        #endregion
    }
}