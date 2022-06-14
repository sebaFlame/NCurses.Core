using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NippyWard.NCurses.Generator
{
    public static class Extensions
    {
        private const string _MultiByteGeneric = "TMultiByte";
        private const string _WideCharGeneric = "TWideChar";
        private const string _SingleByteGeneric = "TSingleByte";
        private const string _CharGeneric = "TChar";
        private const string _MouseEventGeneric = "TMouseEvent";

        public static IdentifierNameSyntax CreateGenericImplementation(
            this IdentifierNameSyntax identifierNameSyntax,
            string multiByteCharType,
            string wideCharType,
            string singelByteCharType,
            string charType,
            string mouseEventType)
        {
            switch (identifierNameSyntax.Identifier.ValueText)
            {
                case _MultiByteGeneric:
                    return SyntaxFactory.IdentifierName(multiByteCharType);
                case _WideCharGeneric:
                    return SyntaxFactory.IdentifierName(wideCharType);
                case _SingleByteGeneric:
                    return SyntaxFactory.IdentifierName(singelByteCharType);
                case _CharGeneric:
                    return SyntaxFactory.IdentifierName(charType);
                case _MouseEventGeneric:
                    return SyntaxFactory.IdentifierName(mouseEventType);
                default:
                    return SyntaxFactory.IdentifierName(identifierNameSyntax.Identifier.ValueText);
            }
        }
    }
}
