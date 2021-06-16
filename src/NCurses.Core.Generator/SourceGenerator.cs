using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NCurses.Core.Generator
{
    [Generator]
    public class SourceGenerator : ISourceGenerator
    {
        private SyntaxReceiver _syntaxReceiver;

        private const string _ChtypeCharWithAttrField = "charWithAttr";
        private const string _CCharTypeAttrField = "attr";
        private const string _CharGlobalLengthField = "_CharGlobalLength";
        private const string _WcharSizeField = "_WcharTSize";
        private const string _BStateField = "bstate";
        private const string _CharType = "schar_t";

        public void Initialize(GeneratorInitializationContext context)
        {
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}

            context.RegisterForSyntaxNotifications(this.GetSyntaxReceiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            string className;
            SourceText sourceText;
            SyntaxTree charSyntaxTree;
            //string code;

            HashSet<string> chTypeClasses = new HashSet<string>();
            HashSet<string> ccharTypeClasses = new HashSet<string>();
            HashSet<string> wcharTypeClassess = new HashSet<string>();
            HashSet<string> mouseEventTypeClasses = new HashSet<string>();
            HashSet<string> panelWrapperClasses = new HashSet<string>();
            HashSet<string> ncursesCharWrapperClasses = new HashSet<string>();
            HashSet<string> ncursesWrapperClasses = new HashSet<string>();

            //generate native char types
            charSyntaxTree = CSharpSyntaxTree.ParseText
            (
                context.AdditionalFiles.Single(x => x.Path.EndsWith("chtype.cs")).GetText(),
                context.ParseOptions as CSharpParseOptions
            );

            foreach (string type in this._syntaxReceiver.Chtypes)
            {
                sourceText = GeneratSingleByteClass
                (
                    "SingleByteChar",
                    type,
                    charSyntaxTree,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                chTypeClasses.Add(className);
            }

            charSyntaxTree = CSharpSyntaxTree.ParseText
            (
                context.AdditionalFiles.Single(x => x.Path.EndsWith("cchar_t.cs")).GetText(),
                context.ParseOptions as CSharpParseOptions
            );

            foreach (string chTypeClass in chTypeClasses)
            {
                foreach (int wcharSize in this._syntaxReceiver.WCharSizes)
                {
                    sourceText = GenerateMultiByteClass
                    (
                        "MultiByteChar",
                        chTypeClass,
                        wcharSize,
                        this._syntaxReceiver.CCharMaxSize,
                        charSyntaxTree,
                        context.ParseOptions,
                        out className
                    );

                    //code = sourceText.ToString();

                    context.AddSource
                    (
                        className,
                        sourceText
                    );

                    ccharTypeClasses.Add(className);
                }
            }

            charSyntaxTree = CSharpSyntaxTree.ParseText
            (
                context.AdditionalFiles.Single(x => x.Path.EndsWith("wchar_t.cs")).GetText(),
                context.ParseOptions as CSharpParseOptions
            );

            foreach (int wcharSize in this._syntaxReceiver.WCharSizes)
            {
                sourceText = GenerateWideCharClass
                (
                    "WideChar",
                    wcharSize,
                    charSyntaxTree,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                wcharTypeClassess.Add(className);
            }

            charSyntaxTree = CSharpSyntaxTree.ParseText
            (
                context.AdditionalFiles.Single(x => x.Path.EndsWith("MEVENT.cs")).GetText(),
                context.ParseOptions as CSharpParseOptions
            );

            foreach (string chTypeClass in chTypeClasses)
            {
                sourceText = GenerateMouseEventClass
                (
                    "MouseEvent",
                    chTypeClass,
                    charSyntaxTree,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                mouseEventTypeClasses.Add(className);
            }

            //generate interop NCurses Panel classes
            foreach (string panelDll in this._syntaxReceiver.PanelNCures)
            {
                sourceText = GeneratePanelInteropClass
                (
                    panelDll,
                    "PanelWrapper",
                    this._syntaxReceiver.PanelInterfaces,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                panelWrapperClasses.Add(className);
            }

            //generate interop NCurses classes
            foreach (string nativeDll in this._syntaxReceiver.NativeNCurses)
            {
                sourceText = GenerateNCursesInteropClass
                (
                    nativeDll,
                    "NCursesWrapper",
                    this._syntaxReceiver.NCursesInterfaces,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                ncursesWrapperClasses.Add(className);
            }

            foreach (NCursesConfig config in this._syntaxReceiver.Configs)
            {
                sourceText = GenerateNCursesCharInteropClass
                (
                    config.NativeNCurses,
                    "NCursesCharWrapper",
                    string.Concat("MultiByteChar", "_", string.Concat("SingleByteChar", "_", config.Chtype), "_", config.WCharSize),
                    string.Concat("WideChar", "_", config.WCharSize),
                    string.Concat("SingleByteChar", "_", config.Chtype),
                    _CharType,
                    string.Concat("MouseEvent", "_", string.Concat("SingleByteChar", "_", config.Chtype)),
                    this._syntaxReceiver.NCursesCharInterfaces,
                    context.ParseOptions,
                    out className
                );

                //code = sourceText.ToString();

                context.AddSource
                (
                    className,
                    sourceText
                );

                ncursesCharWrapperClasses.Add(className);
            }
        }

        private ISyntaxReceiver GetSyntaxReceiver()
        {
            this._syntaxReceiver = new SyntaxReceiver();
            return this._syntaxReceiver;
        }

        private static SourceText GeneratSingleByteClass(
            string hintName,
            string charWithAttrType, 
            SyntaxTree singleByteTree,
            ParseOptions parseOptions,
            out string className)
        {
            StructDeclarationSyntax newRoot;

            className = string.Concat(hintName, "_", charWithAttrType.Replace(".", "_"));
            string concreteClassName = className;

            StructDeclarationSyntax structNode = singleByteTree
                .GetRoot()
                .DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Single();

            newRoot = structNode.ReplaceTokens
            (
                structNode
                    .DescendantTokens()
                    .Where(x => string.Equals("chtype", x.ValueText)),
                (x, y) => SyntaxFactory.Identifier(x.LeadingTrivia, concreteClassName, x.TrailingTrivia)
            );

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.SingleByte"))
                )
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers(newRoot)
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GenerateMultiByteClass(
            string hintName,
            string chTypeClass,
            int wcharSize,
            int maxCharSize,
            SyntaxTree multiByteTree,
            ParseOptions parseOptions,
            out string className)
        {
            StructDeclarationSyntax newRoot;

            className = string.Concat(hintName, "_", chTypeClass.Replace(".", "_"), "_", wcharSize);
            string concreteClassName = className;

            StructDeclarationSyntax structNode = multiByteTree
                .GetRoot()
                .DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Single();

            VariableDeclaratorSyntax attrField = structNode
                .DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => string.Equals(_CCharTypeAttrField, x.Identifier.ValueText))
                .Single();

            VariableDeclarationSyntax attrDeclaration = attrField.Parent as VariableDeclarationSyntax;

            newRoot = structNode
                .ReplaceNode
                (
                    attrDeclaration,
                    attrDeclaration.WithType
                    (
                        SyntaxFactory.IdentifierName(chTypeClass)
                    )
                    .NormalizeWhitespace()
                );

            newRoot = newRoot.ReplaceTokens
            (
                newRoot
                    .DescendantTokens()
                    .Where(x => string.Equals("cchar_t", x.ValueText)),
                (x, y) => SyntaxFactory.Identifier(x.LeadingTrivia, concreteClassName, x.TrailingTrivia)
            );

            VariableDeclaratorSyntax maxSizeField = newRoot
                .DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => string.Equals(_CharGlobalLengthField, x.Identifier.ValueText))
                .Single();

            newRoot = newRoot
                .ReplaceNode
                (
                    maxSizeField,
                    maxSizeField.WithInitializer
                    (
                        maxSizeField.Initializer.WithValue
                        (
                            SyntaxFactory.LiteralExpression
                            (
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(wcharSize * maxCharSize)
                            )
                        )
                    )
                )
                .NormalizeWhitespace();

            VariableDeclaratorSyntax sizeField = newRoot
                .DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => string.Equals(_WcharSizeField, x.Identifier.ValueText))
                .Single();

            newRoot = newRoot
                .ReplaceNode
                (
                    sizeField,
                    sizeField.WithInitializer
                    (
                        sizeField.Initializer.WithValue
                        (
                            SyntaxFactory.LiteralExpression
                            (
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(wcharSize)
                            )
                        )
                    )
                )
                .NormalizeWhitespace();

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.MultiByte"))
                )
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers
                    (
                        newRoot
                            //.WithIdentifier(SyntaxFactory.Identifier(concreteClassName))
                    )
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GenerateWideCharClass(
            string hintName,
            int wcharSize,
            SyntaxTree wideCharTree,
            ParseOptions parseOptions,
            out string className)
        {
            StructDeclarationSyntax newRoot;

            className = string.Concat(hintName, "_", wcharSize);
            string concreteClassName = className;

            StructDeclarationSyntax structNode = wideCharTree
                .GetRoot()
                .DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Single();

            VariableDeclaratorSyntax sizeField = structNode
                .DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => string.Equals(_WcharSizeField, x.Identifier.ValueText))
                .Single();

            newRoot = structNode
                .ReplaceNode
                (
                    sizeField,
                    sizeField.WithInitializer
                    (
                        sizeField.Initializer.WithValue
                        (
                            SyntaxFactory.LiteralExpression
                            (
                                SyntaxKind.NumericLiteralExpression,
                                SyntaxFactory.Literal(wcharSize)
                            )
                        )
                    )
                )
                .NormalizeWhitespace();

            newRoot = newRoot.ReplaceTokens
            (
                newRoot
                    .DescendantTokens()
                    .Where(x => string.Equals("wchar_t", x.ValueText)),
                (x, y) => SyntaxFactory.Identifier(x.LeadingTrivia, concreteClassName, x.TrailingTrivia)
            );

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.CompilerServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop"))
                )
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers
                    (
                        newRoot
                            //.WithIdentifier(SyntaxFactory.Identifier(concreteClassName))
                    )
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GenerateMouseEventClass(
            string hintName,
            string chTypeClass,
            SyntaxTree mouseEventTree,
            ParseOptions parseOptions,
            out string className)
        {
            StructDeclarationSyntax newRoot;

            className = string.Concat(hintName, "_", chTypeClass.Replace(".", "_"));
            string concreteClassName = className;

            StructDeclarationSyntax structNode = mouseEventTree
                .GetRoot()
                .DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Single();

            VariableDeclaratorSyntax battrField = structNode
                .DescendantNodes()
                .OfType<VariableDeclaratorSyntax>()
                .Where(x => string.Equals(_BStateField, x.Identifier.ValueText))
                .Single();

            VariableDeclarationSyntax bAttrDeclaration = battrField.Parent as VariableDeclarationSyntax;

            newRoot = structNode
                .ReplaceNode
                (
                    bAttrDeclaration,
                    bAttrDeclaration.WithType
                    (
                        SyntaxFactory.IdentifierName(chTypeClass)
                    )
                    .NormalizeWhitespace()
                );

            newRoot = newRoot.ReplaceTokens
            (
                newRoot
                    .DescendantTokens()
                    .Where(x => string.Equals("MEVENT", x.ValueText)),
                (x, y) => SyntaxFactory.Identifier(x.LeadingTrivia, concreteClassName, x.TrailingTrivia)
            );

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.Mouse"))
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers
                    (
                        newRoot
                            //.WithIdentifier(SyntaxFactory.Identifier(concreteClassName))
                    )
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GeneratePanelInteropClass(
            string nativeDll, 
            string nameHint,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces, 
            ParseOptions parseOptions,
            out string className)
        {
            className = string.Concat(nameHint, "_", nativeDll.Replace(".", "_"));

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.SafeHandles")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.Panel"))
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers
                    (
                        CreatePanelInteropClass(className, nativeDll, interfaces)
                    )
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GenerateNCursesInteropClass(
            string nativeDll,
            string nameHint,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces,
            ParseOptions parseOptions,
            out string className)
        {
            className = string.Concat(nameHint, "_", nativeDll.Replace(".", "_"));

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                .AddUsings
                (
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop")),
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.SafeHandles"))
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                    .AddMembers
                    (
                        CreateNCursesInteropClass(className, nativeDll, interfaces)
                    )
                    .NormalizeWhitespace()
                )
                .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static SourceText GenerateNCursesCharInteropClass(
            string nativeDll,
            string nameHint,
            string multiByteCharType,
            string wideCharType,
            string singleByteCharType,
            string charType,
            string mouseEventType,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces,
            ParseOptions parseOptions,
            out string className)
        {
            className = string.Concat
            (
                nameHint, 
                "_", 
                nativeDll.Replace(".", "_"),
                "_",
                multiByteCharType,
                "_",
                wideCharType,
                "_",
                singleByteCharType,
                "_",
                charType,
                "_",
                mouseEventType
            );

            SyntaxTree syntaxTree = CSharpSyntaxTree.Create
            (
                SyntaxFactory.CompilationUnit()
                    .AddUsings
                    (
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Runtime.InteropServices")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.SafeHandles")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.SingleByte")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.MultiByte")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.Mouse")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.WideChar")),
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("NCurses.Core.Interop.Char"))
                    )
                    .NormalizeWhitespace()
                    .AddMembers
                    (
                        SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("NCurses.Core.Generated"))
                            .AddMembers
                            (
                                CreateNCursesClass
                                (
                                    className, 
                                    nativeDll,
                                    multiByteCharType,
                                    wideCharType,
                                    singleByteCharType,
                                    charType,
                                    mouseEventType,
                                    interfaces
                                )
                            )
                            .NormalizeWhitespace()
                    )
                    .NormalizeWhitespace(),
                parseOptions as CSharpParseOptions,
                "",
                Encoding.Unicode
            );

            return syntaxTree.GetText();
        }

        private static ClassDeclarationSyntax CreatePanelInteropClass(
            string className,
            string nativeDll,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces)
        {
            return SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(className))
                .AddBaseListTypes
                (
                    interfaces.Select
                    (
                        x => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(x.Identifier.Text))
                    )
                    .ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateNativeMethod(z, nativeDll))
                        ).ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateImplementationMethod(z))
                        ).ToArray()
                )
                .NormalizeWhitespace();
        }

        private static ClassDeclarationSyntax CreateNCursesInteropClass(
            string className,
            string nativeDll,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces)
        {
            return SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(className))
                .AddBaseListTypes
                (
                    interfaces.Select
                    (
                        x => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(x.Identifier.Text))
                    )
                    .ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateNativeMethod(z, nativeDll))
                        ).ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateImplementationMethod(z))
                        ).ToArray()
                )
                .NormalizeWhitespace();
        }

        private static ClassDeclarationSyntax CreateNCursesClass(
            string className, 
            string nativeDll,
            string multiByteCharType,
            string wideCharType,
            string singelByteCharType,
            string charType,
            string mouseEventType,
            IReadOnlyList<InterfaceDeclarationSyntax> interfaces)
        {
            ClassDeclarationSyntax classDeclaration = SyntaxFactory.ClassDeclaration(SyntaxFactory.Identifier(className))
                .AddBaseListTypes
                (
                    interfaces.Select
                    (
                        x => SyntaxFactory.SimpleBaseType
                        (
                            x.TypeParameterList is null || x.TypeParameterList.Parameters.Count == 0
                                ? SyntaxFactory.ParseTypeName(x.Identifier.Text)
                                : SyntaxFactory.GenericName
                                (
                                    SyntaxFactory.Identifier(x.Identifier.Text),
                                    SyntaxFactory.TypeArgumentList
                                    (
                                        SyntaxFactory.SeparatedList
                                        (
                                            x.TypeParameterList.Parameters.Select
                                            (
                                                p => SyntaxFactory.ParseTypeName(p.Identifier.Text)
                                            )
                                        )
                                    )
                                )
                        )
                    )
                    .ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateNativeMethod(z, nativeDll))
                        ).ToArray()
                )
                .NormalizeWhitespace()
                .AddMembers
                (
                    interfaces
                        .SelectMany
                        (
                            x => x
                                .DescendantNodes()
                                .OfType<MethodDeclarationSyntax>()
                                .Select(z => CreateImplementationMethod(z))
                        ).ToArray()
                )
                .NormalizeWhitespace();

            IEnumerable<IdentifierNameSyntax> identifiers = classDeclaration
                .DescendantNodes()
                .OfType<IdentifierNameSyntax>();

            return classDeclaration
                .ReplaceNodes
                (
                    identifiers,
                    (x, y) => x.CreateGenericImplementation
                    (
                            multiByteCharType,
                            wideCharType,
                            singelByteCharType,
                            charType,
                            mouseEventType
                    )
                );
        }

        private static MethodDeclarationSyntax CreateNativeMethod(
            MethodDeclarationSyntax interfaceMethod, 
            string nativeDll)
        {
            string nativeMethodName = $"ncurses_{interfaceMethod.Identifier.ValueText}";

            return SyntaxFactory.MethodDeclaration
            (
                SyntaxFactory.List
                (
                    new AttributeListSyntax[]
                    {
                        SyntaxFactory.AttributeList
                        (
                            SyntaxFactory.SeparatedList
                            (
                                new AttributeSyntax[]
                                {
                                    SyntaxFactory.Attribute
                                    (
                                        SyntaxFactory.ParseName("DllImport"),
                                        SyntaxFactory.AttributeArgumentList
                                        (
                                            SyntaxFactory.SeparatedList
                                            (
                                                new AttributeArgumentSyntax[]
                                                {
                                                    SyntaxFactory.AttributeArgument
                                                    (
                                                        SyntaxFactory.LiteralExpression
                                                        (
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(nativeDll)
                                                        )
                                                    ),
                                                    SyntaxFactory.AttributeArgument
                                                    (
                                                        SyntaxFactory.NameEquals
                                                        (
                                                            SyntaxFactory.IdentifierName("EntryPoint")
                                                        ),
                                                        null,
                                                        SyntaxFactory.LiteralExpression
                                                        (
                                                            SyntaxKind.StringLiteralExpression,
                                                            SyntaxFactory.Literal(interfaceMethod.Identifier.ValueText)
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                ),
                SyntaxFactory.TokenList
                (
                    SyntaxFactory.Token(SyntaxKind.PrivateKeyword),
                    SyntaxFactory.Token(SyntaxKind.StaticKeyword),
                    SyntaxFactory.Token(SyntaxKind.ExternKeyword)
                ),
                interfaceMethod.ReturnType.WithoutTrivia(),
                null,
                SyntaxFactory.Identifier(nativeMethodName),
                null,
                SyntaxFactory.ParameterList
                (
                    interfaceMethod.ParameterList.Parameters
                ),
                interfaceMethod.ConstraintClauses, //should always be empty
                null,
                null,
                SyntaxFactory.Token(SyntaxKind.SemicolonToken)
            )
            .NormalizeWhitespace();
        }

        private static MethodDeclarationSyntax CreateImplementationMethod(
            MethodDeclarationSyntax interfaceMethod)
        {
            string nativeMethodName = $"ncurses_{interfaceMethod.Identifier.ValueText}";

            return SyntaxFactory.MethodDeclaration
            (
                SyntaxFactory.List<AttributeListSyntax>(),
                SyntaxFactory.TokenList
                (
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                ),
                interfaceMethod.ReturnType.WithoutTrivia(),
                null,
                SyntaxFactory.Identifier(interfaceMethod.Identifier.ValueText),
                null,
                SyntaxFactory.ParameterList
                (
                    interfaceMethod.ParameterList.Parameters
                ),
                interfaceMethod.ConstraintClauses,
                null,
                SyntaxFactory.ArrowExpressionClause
                (
                    interfaceMethod.ReturnType is RefTypeSyntax
                    ? SyntaxFactory.RefExpression
                    (
                        SyntaxFactory.InvocationExpression
                        (
                            SyntaxFactory.IdentifierName(nativeMethodName),
                            SyntaxFactory.ArgumentList
                            (
                                SyntaxFactory.SeparatedList
                                (
                                    interfaceMethod.ParameterList.Parameters.Select
                                    (
                                        x => x.Modifiers.Any()
                                            ? SyntaxFactory.Argument
                                            (
                                                null,
                                                x.Modifiers.First(), //should always be single
                                                SyntaxFactory.IdentifierName(x.Identifier.WithoutTrivia())
                                            )
                                            : SyntaxFactory.Argument
                                            (
                                                SyntaxFactory.IdentifierName(x.Identifier.WithoutTrivia())
                                            )
                                    )
                                )
                            )
                        )
                    )
                    : SyntaxFactory.InvocationExpression
                    (
                        SyntaxFactory.IdentifierName(nativeMethodName),
                        SyntaxFactory.ArgumentList
                        (
                            SyntaxFactory.SeparatedList
                            (
                                interfaceMethod.ParameterList.Parameters.Select
                                (
                                    x => x.Modifiers.Any()
                                        ? SyntaxFactory.Argument
                                        (
                                            null,
                                            x.Modifiers.First(), //should always be single
                                            SyntaxFactory.IdentifierName(x.Identifier.WithoutTrivia())
                                        )
                                        : SyntaxFactory.Argument
                                        (
                                            SyntaxFactory.IdentifierName(x.Identifier.WithoutTrivia())
                                        )
                                )
                            )
                        )
                    )

                ),
                SyntaxFactory.Token(SyntaxKind.SemicolonToken)
            )
            .NormalizeWhitespace();
        }
    }
}
