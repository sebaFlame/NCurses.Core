using System;
using System.Reflection;
using System.IO;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NippyWard.NCurses.Generator;

namespace NippyWard.NCurses.Generator.Tests
{
    class Program
    {
        private static Compilation _Compilation;
        private static SourceGenerator _SourceGenerator;

        static Program()
        {
            // Create the 'input' compilation that the generator will act on
            _Compilation = CreateCompilation();
            _SourceGenerator = new SourceGenerator();
        }

        private static Compilation CreateCompilation()
            => CSharpCompilation.Create("compilation",
                new[] 
                { 
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\Constants.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\IMultiByteWrapper.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\ISingleByteWrapper.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\IWideCharWrapper.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\ICharWrapper.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\INCursesPanelWrapper.cs"), Encoding.Unicode)),
                    CSharpSyntaxTree.ParseText(SourceText.From(File.OpenRead(@"Deps\INCursesWrapper.cs"), Encoding.Unicode))
                },
                new[] { MetadataReference.CreateFromFile(typeof(System.Reflection.Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        static void Main(string[] args)
        {
            GeneratorDriver driver = CSharpGeneratorDriver.Create
            (
                new ISourceGenerator[]
                {
                    _SourceGenerator
                },
                new AdditionalText[]
                {
                    new InternalAdditionalText(@"Deps\cchar_t.cs"),
                    new InternalAdditionalText(@"Deps\chtype.cs"),
                    new InternalAdditionalText(@"Deps\MEVENT.cs"),
                    new InternalAdditionalText(@"Deps\wchar_t.cs")
                }
            );

            driver = driver.RunGeneratorsAndUpdateCompilation(_Compilation, out var outputCompilation, out var diagnostics);

            GeneratorDriverRunResult runResult = driver.GetRunResult();
        }

        /*
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Console.ReadKey();
        }
        */
    }
}
