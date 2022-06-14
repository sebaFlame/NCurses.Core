using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NippyWard.NCurses.Generator
{
    public class SyntaxReceiver : ISyntaxReceiver
    {
        public IReadOnlyList<InterfaceDeclarationSyntax> NCursesCharInterfaces => this._ncursesCharInterfaces;
        public IReadOnlyList<InterfaceDeclarationSyntax> PanelInterfaces => this._panelInterfaces;
        public IReadOnlyList<InterfaceDeclarationSyntax> NCursesInterfaces => this._ncursesInterfaces;

        public IReadOnlyCollection<string> NativeNCurses => this._nativeNcursesDll;
        public IReadOnlyCollection<string> PanelNCures => this._nativePanelDll;
        public IReadOnlyCollection<string> Chtypes => this._chtypeTypes;
        public IReadOnlyCollection<int> WCharSizes => this._wcharSizes;
        public int CCharMaxSize { get; private set; }

        public IReadOnlyList<NCursesConfig> Configs => this._configs;

        private const string _ConstantsClassName = "Constants";
        private const string _NativeNcursesDllField = "DLLNAME";
        private const string _NativePanelDllField = "DLLPANELNAME";
        private const string _WCharSizeField = "SIZEOF_WCHAR_T";
        private const string _ChtypeField = "CHTYPE_TYPE";
        private const string _CCharwMaxField = "CCHARW_MAX";

        private static string[] _NCursesCharInteropInterfaces = new string[]
        {
            "IMultiByteWrapper",
            "IWideCharWrapper",
            "ISingleByteWrapper",
            "ICharWrapper"
        };

        private static string[] _PanelInteropInterfaces = new string[]
        {
            "INCursesPanelWrapper"
        };

        private static string[] _NCursesInteropInterfaces = new string[]
        {
            "INCursesWrapper"
        };

        private List<InterfaceDeclarationSyntax> _ncursesCharInterfaces;
        private List<InterfaceDeclarationSyntax> _ncursesInterfaces;
        private List<InterfaceDeclarationSyntax> _panelInterfaces;

        private HashSet<string> _nativeNcursesDll;
        private HashSet<string> _nativePanelDll;
        private HashSet<string> _chtypeTypes;
        private HashSet<int> _wcharSizes;

        private List<NCursesConfig> _configs;

        public SyntaxReceiver()
        {
            this._ncursesCharInterfaces = new List<InterfaceDeclarationSyntax>();
            this._panelInterfaces = new List<InterfaceDeclarationSyntax>();
            this._ncursesInterfaces = new List<InterfaceDeclarationSyntax>();

            this._nativeNcursesDll = new HashSet<string>();
            this._nativePanelDll = new HashSet<string>();
            this._chtypeTypes = new HashSet<string>();
            this._wcharSizes = new HashSet<int>();

            this._configs = new List<NCursesConfig>();
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclaration)
            {
                if (_NCursesCharInteropInterfaces.Any(x => string.Equals(x, interfaceDeclaration.Identifier.Text)))
                {
                    this._ncursesCharInterfaces.Add(interfaceDeclaration);
                }

                if (_PanelInteropInterfaces.Any(x => string.Equals(x, interfaceDeclaration.Identifier.Text)))
                {
                    this._panelInterfaces.Add(interfaceDeclaration);
                }

                if (_NCursesInteropInterfaces.Any(x => string.Equals(x, interfaceDeclaration.Identifier.Text)))
                {
                    this._ncursesInterfaces.Add(interfaceDeclaration);
                }
            }

            if (syntaxNode is ClassDeclarationSyntax classDeclaration
                && string.Equals(_ConstantsClassName, classDeclaration.Identifier.Text))
            {
                ConstructorDeclarationSyntax constructor = classDeclaration.DescendantNodes()
                    .OfType<ConstructorDeclarationSyntax>()
                    .Where(x => x.Modifiers.IndexOf(SyntaxKind.StaticKeyword) >= 0)
                    .Single();

                foreach (SwitchSectionSyntax switchSection in constructor.DescendantNodes().OfType<SwitchSectionSyntax>())
                {
                    this._configs.Add
                    (
                        new NCursesConfig
                        (
                            FindFieldAssignments<string>(switchSection, _NativeNcursesDllField, this._nativeNcursesDll),
                            FindFieldAssignments<string>(switchSection, _NativePanelDllField, this._nativePanelDll),
                            FindFieldAssignments<int>(switchSection, _WCharSizeField, this._wcharSizes),
                            FindTypeAssignments(switchSection, _ChtypeField, this._chtypeTypes)
                        )
                    );
                }

                VariableDeclaratorSyntax maxSizeField = classDeclaration
                    .DescendantNodes()
                    .OfType<VariableDeclaratorSyntax>()
                    .Where(x => string.Equals(_CCharwMaxField, x.Identifier.ValueText))
                    .Single();

                this.CCharMaxSize = int.Parse((maxSizeField.Initializer.Value as LiteralExpressionSyntax).Token.ValueText);
            }
        }

        private static T FindFieldAssignments<T>(
            SwitchSectionSyntax switchSection,
            string assignmentField,
            HashSet<T> nativeDllCollection)
        {
            //find all assignments
            LiteralExpressionSyntax literalExpression = switchSection.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(x => x.Left is IdentifierNameSyntax identifierNameSyntax
                    && string.Equals(assignmentField, identifierNameSyntax.Identifier.Text))
                .Select(y => y.Right as LiteralExpressionSyntax)
                .Single();

            string value = literalExpression.Token.ValueText;
            T typedValue = (T)Convert.ChangeType(value, typeof(T));
            nativeDllCollection.Add(typedValue);

            return typedValue;
        }

        private static string FindTypeAssignments(
            SwitchSectionSyntax switchSection,
            string typeField,
            HashSet<string> typeCollection)
        {
            ;

            TypeOfExpressionSyntax typeofExpressionSyntax = switchSection.DescendantNodes()
                .OfType<AssignmentExpressionSyntax>()
                .Where(x => x.Left is IdentifierNameSyntax identifierNameSyntax
                        && string.Equals(typeField, identifierNameSyntax.Identifier.Text))
                .Select(y => y.Right as TypeOfExpressionSyntax)
                .Single();

            string typeName = (typeofExpressionSyntax.Type as IdentifierNameSyntax).Identifier.ValueText;
            typeCollection.Add(typeName);

            return typeName;
        }
    }
}
