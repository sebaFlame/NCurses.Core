using System;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.SingleByteString;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop
{
    internal static class NativePad
    {
        #region Custom type wrapper fields
        private static INativePadWide widePadWrapper;
        private static INativePadWide WidePadWrapper => NativeNCurses.HasUnicodeSupport
              ? widePadWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);

        private static INativePadSmall smallPadWrapper;
        private static INativePadSmall SmallPadWrapper => smallPadWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        #endregion

        #region custom type initialization
        internal static void CreateCharCustomWrappers()
        {
            //do nothing
        }

        internal static void CreateCustomTypeWrappers()
        {
            if ((DynamicTypeBuilder.chtype is null
                || DynamicTypeBuilder.schar is null)
                || (NativeNCurses.HasUnicodeSupport
                    && (DynamicTypeBuilder.cchar_t is null || DynamicTypeBuilder.wchar_t is null)))
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (NativeNCurses.HasUnicodeSupport)
            {
                if (widePadWrapper is null)
                {
                    customType = typeof(NativePadWide<,,,,>).MakeGenericType(DynamicTypeBuilder.cchar_t, DynamicTypeBuilder.wchar_t, 
                        DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                    widePadWrapper = (INativePadWide)Activator.CreateInstance(customType);
                }
            }

            if (smallPadWrapper is null)
            {
                customType = typeof(NativePadSmall<,,>).MakeGenericType(DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                smallPadWrapper = (INativePadSmall)Activator.CreateInstance(customType);
            }
        }
        #endregion

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
        public static void pechochar(IntPtr pad, in INCursesSCHAR ch)
        {
            SmallPadWrapper.pechochar(pad, ch);
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
        public static void pnoutrefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.pnoutrefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol), "pnoutrefresh");
        }
        #endregion

        #region prefresh
        /// <summary>
        /// see <see cref="pnoutrefresh(IntPtr, int, int, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void prefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
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
        public static void pecho_wchar(IntPtr pad, INCursesWCHAR wch)
        {
            WidePadWrapper.pecho_wchar(pad, wch);
        }
        #endregion
    }
}
