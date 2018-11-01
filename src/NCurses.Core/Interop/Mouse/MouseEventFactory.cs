using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.Mouse
{
    public class MouseEventFactory
    {
        private static Type MouseEventType;

        private static Func<short, int, int, int, ulong, IMEVENT> createMouseEvent;

        static MouseEventFactory()
        {
            MouseEventType = typeof(MEVENT<>).MakeGenericType(DynamicTypeBuilder.chtype);

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3, par4, par5;

            MethodInfo createChtype = typeof(SmallCharFactory).GetMethod("GetAttribute");

            par1 = Expression.Parameter(typeof(short));
            par2 = Expression.Parameter(typeof(int));
            par3 = Expression.Parameter(typeof(int));
            par4 = Expression.Parameter(typeof(int));
            par5 = Expression.Parameter(typeof(ulong));
            ctor = MouseEventType.GetConstructor(new Type[] { typeof(short), typeof(int), typeof(int), typeof(int), DynamicTypeBuilder.chtype });
            createMouseEvent = Expression.Lambda<Func<short, int, int, int, ulong, IMEVENT>>(
                    Expression.Convert(
                        Expression.New(ctor,
                            par1,
                            par2,
                            par3,
                            par4,
                            Expression.Convert(
                                Expression.Call(createChtype, par5),
                                DynamicTypeBuilder.chtype)),
                        typeof(IMEVENT)),
                    par1, par2, par3, par4, par5).Compile();
        }

        public static IMEVENT GetMouseEvent(short id, int x, int y, int z, ulong mask)
        {
            return createMouseEvent(id, x, y, z, mask);
        }
    }
}
