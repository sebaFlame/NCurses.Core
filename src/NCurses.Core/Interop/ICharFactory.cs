using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface ICharFactory<TChar, TString>
        where TChar : INCursesChar
        where TString : INCursesCharString
    {
        ICharFactory<TChar, TString> Instance { get; }

        void GetNativeEmptyChar(out TChar res);
        void GetNativeChar(char ch, out TChar res);
        void GetNativeChar(char ch, ulong attrs, out TChar res);
        void GetNativeChar(char ch, ulong attrs, short colorPair, out TChar res);

        void GetNativeEmptyString(int length, out TString res);
        void GetNativeString(string str, out TString res);
        void GetNativeString(string str, ulong attrs, out TString res);
        void GetNativeString(string str, ulong attrs, short colorPair, out TString res);
    }
}
