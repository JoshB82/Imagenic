using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace NullCheckerSourceGenerator
{
    internal class SyntaxReceiver : ISyntaxReceiver
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
}