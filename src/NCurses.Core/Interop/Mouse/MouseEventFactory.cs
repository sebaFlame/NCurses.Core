using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.Mouse
{
    public class MouseEventFactory
    {
        private static Type MouseEventType;

        private static Func<short, int, int, int, ulong, IMEVENT> createMouseEvent;

        private static MouseEventFactory instance;
        public static MouseEventFactory Instance => instance ?? (instance = new MouseEventFactory());

        static MouseEventFactory()
        {
            instance = new MouseEventFactory();

            MouseEventType = Constants.MouseEvent;

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3, par4, par5;

            par1 = Expression.Parameter(typeof(short));
            par2 = Expression.Parameter(typeof(int));
            par3 = Expression.Parameter(typeof(int));
            par4 = Expression.Parameter(typeof(int));
            par5 = Expression.Parameter(typeof(ulong));
            ctor = MouseEventType.GetConstructor(new Type[] { typeof(short), typeof(int), typeof(int), typeof(int), typeof(ulong) });
            createMouseEvent = Expression.Lambda<Func<short, int, int, int, ulong, IMEVENT>>(
                    Expression.Convert(
                        Expression.New(ctor,
                            par1,
                            par2,
                            par3,
                            par4,
                            par5),
                        typeof(IMEVENT)),
                    par1, par2, par3, par4, par5).Compile();
        }

        public void GetMouseEvent(short id, int x, int y, int z, ulong mask, out IMEVENT res)
        {
            res = createMouseEvent(id, x, y, z, mask);
        }
    }
}
