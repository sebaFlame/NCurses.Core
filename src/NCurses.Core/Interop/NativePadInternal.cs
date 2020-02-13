using System;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Dynamic;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop
{
    internal class NativePadInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
            : INativePadWrapper<
                TMultiByte,
                MultiByteCharString<TMultiByte>,
                TWideChar,
                WideCharString<TWideChar>,
                TSingleByte,
                SingleByteCharString<TSingleByte>,
                TChar,
                CharString<TChar>,
                TMouseEvent>,
            INativePadWrapper<
                IMultiByteChar,
                IMultiByteCharString,
                IChar,
                ICharString,
                ISingleByteChar,
                ISingleByteCharString,
                IChar,
                ICharString,
                IMEVENT>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativePadMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> MultiByteNCursesWrapper { get; }
        internal NativePadSingleByte<TSingleByte, TChar, TMouseEvent> SingleByteNCursesWrapper { get; }

        internal INativePadWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Instance => this;

        public NativePadInternal(
            IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> multiByteWrapper,
            ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> singleByteWrapper)
        {
            MultiByteNCursesWrapper = new NativePadMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(multiByteWrapper);
            SingleByteNCursesWrapper = new NativePadSingleByte<TSingleByte, TChar, TMouseEvent>(singleByteWrapper);
        }

        #region pechochar
        /// <summary>
        /// The pechochar routine is functionally equivalent to a call
        /// to addch followed by a call to refresh(3x), a call to waddch followed by a call to wrefresh, or a  call to  waddch
        /// followed by a call to prefresh.The knowledge that only a
        /// single character is being output is taken into  consideration and,
        /// for non-control characters, a considerable performance gain might be seen by
        /// using  these  routines  instead of their equivalents.In the case of pechochar, the
        /// last location of the pad on the screen is reused  for  the
        /// arguments to prefresh.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pad">a pointer to the pad</param>
        /// <param name="ch">the character you want to echo</param>
        public void pechochar(WindowBaseSafeHandle pad, in TSingleByte ch)
        {
            SingleByteNCursesWrapper.pechochar(pad, in ch);
        }
        #endregion

        #region pnoutrefresh
        /// <summary>
        /// The prefresh  and pnoutrefresh routines are analogous to
        /// wrefresh and wnoutrefresh except that they relate to  pads
        /// instead  of windows.The additional parameters are needed
        /// to indicate what part of the pad and screen are  involved.
        /// The pminrow and pmincol parameters specify the upper left-
        /// hand corner of the rectangle to be displayed in  the pad.
        /// The sminrow, smincol, smaxrow, and smaxcol parameters
        /// specify the edges of the rectangle to be displayed on the
        /// screen.The lower right-hand corner of the rectangle to
        /// be displayed in the pad is calculated from the screen  coordinates, since the  rectangles must be the same size.
        /// Both rectangles must be entirely contained  within their
        /// respective structures.  Negative values of pminrow, pmin-
        /// col, sminrow, or smincol are treated as if they were zero.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pad">a pointer to the pad</param>
        /// <param name="pminrow">line nubmer of the upper-left corner of what to display from the pad</param>
        /// <param name="pmincol">column nubmer of the upper-left corner of what to display from the pad</param>
        /// <param name="sminrow">minimum line number of the screen where to display</param>
        /// <param name="smincol">minimum column number of the screen where to display</param>
        /// <param name="smaxrow">maximum line number of the screen where to display</param>
        /// <param name="smaxcol">minimum column number of the screen where to display</param>
        public void pnoutrefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.pnoutrefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol), "pnoutrefresh");
        }
        #endregion

        #region prefresh
        /// <summary>
        /// see <see cref="pnoutrefresh(IntPtr, int, int, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public void prefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.prefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol), "prefresh");
        }
        #endregion

        #region pecho_wchar
        /// <summary>
        /// The pechochar routine is functionally equivalent to a call
        /// to addch followed by a call to refresh(3x), a call to waddch followed by a call to wrefresh, or a  call to  waddch
        /// followed by a call to prefresh.The knowledge that only a
        /// single character is being output is taken into  consideration and, for non-control characters,
        /// a considerable performance gain might be seen by using  these
        /// routines  instead of their equivalents.In the case of pechochar, the
        /// last location of the pad on the screen is reused  for  the
        /// arguments to prefresh.
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public void pecho_wchar(WindowBaseSafeHandle pad, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.pecho_wchar(pad, in wch);
        }
        #endregion

        #region Interfece implementations
        void INativePadMultiByte<IMultiByteChar, IMultiByteCharString>.pecho_wchar(WindowBaseSafeHandle pad, in IMultiByteChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(in wch);
            this.pecho_wchar(pad, in casted);
        }

        void INativePadSingleByte<ISingleByteChar, ISingleByteCharString>.pechochar(WindowBaseSafeHandle pad, in ISingleByteChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(in ch);
            this.pechochar(pad, in casted);
        }
        #endregion
    }
}
