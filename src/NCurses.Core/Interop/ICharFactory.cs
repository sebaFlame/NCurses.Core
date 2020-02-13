using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface ICharFactory<TChar, TString>
        where TChar : IChar
        where TString : ICharString
    {
        TChar GetNativeEmptyChar();
        TChar GetNativeChar(char ch);

        TString GetNativeEmptyString(int length);
        TString GetNativeString(string str);

        unsafe TString GetNativeEmptyString(byte* buffer, int length);
        TString GetNativeEmptyString(byte[] buffer);

        unsafe TString GetNativeString(byte* buffer, int length, string str);
        TString GetNativeString(byte[] buffer, string str);

        int GetByteCount(string str);
        int GetByteCount(int length);
        int GetCharLength();
    }

    public interface INCursesCharFactory<TChar, TString> : ICharFactory<TChar, TString>
        where TChar : INCursesChar
        where TString : INCursesCharString
    {
        TChar GetNativeChar(char ch, ulong attrs);
        TChar GetNativeChar(char ch, ulong attrs, short colorPair);

        TString GetNativeString(string str, ulong attrs);
        TString GetNativeString(string str, ulong attrs, short colorPair);

        unsafe TString GetNativeString(byte* buffer, int length, string str, ulong attrs);
        TString GetNativeString(byte[] buffer, string str, ulong attrs);
        unsafe TString GetNativeString(byte* buffer, int length, string str, ulong attrs, short colorPair);
        TString GetNativeString(byte[] buffer, string str, ulong attrs, short colorPair);
    }

    internal interface ICharFactoryInternal<TChar, TString>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TString : struct, ICharString
    {
        TChar GetNativeEmptyCharInternal();
        TChar GetNativeCharInternal(char ch);

        unsafe TString GetNativeEmptyStringInternal(byte* buffer, int length);
        TString GetNativeEmptyStringInternal(byte[] buffer);
        unsafe TString GetNativeStringInternal(byte* buffer, int length, string str);
        TString GetNativeStringInternal(byte[] buffer, string str);

        TString CreateNativeString(ref TChar strRef);

        int GetByteCount(string str, bool addNullTerminator = true);
        int GetByteCount(int length, bool addNullTerminator = true);
        int GetCharLength();
    }

    internal interface INCursesCharFactoryInternal<TCharType, TStringType, TChar, TString> : 
        ICharFactoryInternal<TChar, TString>, INCursesCharFactory<TCharType, TStringType>
        where TCharType : INCursesChar
        where TStringType : INCursesCharString, IEnumerable<TCharType>
        where TChar : unmanaged, TCharType, IEquatable<TChar>, IEquatable<TCharType>
        where TString : struct, TStringType, IEnumerable<TCharType>
    {
        TChar GetNativeCharInternal(char ch, ulong attrs);
        TChar GetNativeCharInternal(char ch, ulong attrs, short colorPair);

        unsafe TString GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs);
        TString GetNativeStringInternal(byte[] buffer, string str, ulong attrs);
        unsafe TString GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs, short colorPair);
        TString GetNativeStringInternal(byte[] buffer, string str, ulong attrs, short colorPair);
    }
}
