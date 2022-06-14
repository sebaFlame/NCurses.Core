using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace NippyWard.NCurses.Generator.Tests
{
    internal class InternalAdditionalText : AdditionalText
    {
        public override string Path { get; }

        private string _code;

        internal InternalAdditionalText(string path)
        {
            this.Path = path;
        }

        internal InternalAdditionalText(string path, string code)
            : this(path)
        {
            this._code = code;
        }

        public override SourceText GetText(CancellationToken cancellationToken = default)
        {
            string txt;
            if (string.IsNullOrEmpty(this._code))
            {
                txt = File.ReadAllText(this.Path);
            }
            else
            {
                txt = this._code;
            }
            
            return SourceText.From(txt);
        }
    }
}
