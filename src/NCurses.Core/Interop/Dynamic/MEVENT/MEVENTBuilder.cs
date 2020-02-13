using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.Dynamic.MEVENT
{
    public class MEVENTBuilder
    {
        private static Type MEVENT;// = typeof(MEVENT);

        internal static Type CreateType(ModuleBuilder moduleBuilder, Type chtype)
        {
            if (MEVENT != null)
                return MEVENT;

            FieldBuilder idField, xField, yField, zField, bstateField;
            PropertyBuilder propBuilder;
            ConstructorBuilder ctorBuilder;
            ILGenerator ctorIl, methodIl;
            MethodBuilder methodBuilder;

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "MEVENT",
                TypeAttributes.NotPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType));
            typeBuilder.AddInterfaceImplementation(typeof(IMEVENT));

            idField = typeBuilder.DefineField("id", typeof(short), FieldAttributes.Public);
            xField = typeBuilder.DefineField("x", typeof(int), FieldAttributes.Public);
            yField = typeBuilder.DefineField("y", typeof(int), FieldAttributes.Public);
            zField = typeBuilder.DefineField("z", typeof(int), FieldAttributes.Public);
            bstateField = typeBuilder.DefineField("bstate", chtype, FieldAttributes.Public);

            /* constructors */
            #region public MEVENT(short id, int x, int y, int z, ulong bstate)
            ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(short), typeof(int), typeof(int), typeof(int), typeof(ulong) });
            ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stfld, idField);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Stfld, xField);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Stfld, yField);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_S, (byte)4);
            ctorIl.Emit(OpCodes.Stfld, zField);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_S, (byte)5);
            ctorIl.Emit(OpCodes.Call, chtype.GetMethod("op_Implicit", new Type[] { typeof(ulong) }));
            ctorIl.Emit(OpCodes.Stfld, bstateField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            /* properties */
            #region ID
            methodBuilder = typeBuilder.DefineMethod("get_ID",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(short),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, idField);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("ID",
                PropertyAttributes.None,
                typeof(short),
                new Type[] { typeof(short) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region X
            methodBuilder = typeBuilder.DefineMethod("get_X",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(int),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, xField);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("X",
                PropertyAttributes.None,
                typeof(int),
                new Type[] { typeof(int) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Y
            methodBuilder = typeBuilder.DefineMethod("get_Y",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(int),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, yField);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Y",
                PropertyAttributes.None,
                typeof(int),
                new Type[] { typeof(int) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Z
            methodBuilder = typeBuilder.DefineMethod("get_Z",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(int),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, zField);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Z",
                PropertyAttributes.None,
                typeof(int),
                new Type[] { typeof(int) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region BState
            methodBuilder = typeBuilder.DefineMethod("get_BState",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(ulong),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, bstateField);
            methodIl.Emit(OpCodes.Call, chtype.GetMethod("op_Implicit", new Type[] { chtype }));
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("BState",
                PropertyAttributes.None,
                typeof(ulong),
                new Type[] { typeof(ulong) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            return (MEVENT = typeBuilder.CreateTypeInfo().AsType());
        }
    }
}
