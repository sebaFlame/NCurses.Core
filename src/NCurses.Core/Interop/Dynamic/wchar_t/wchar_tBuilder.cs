using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.Dynamic.wchar_t
{
    internal static class wchar_tBuilder
    {
        //TODO: remove
        private static Type wchar_t;// = typeof(wchar_t);

        internal static Type CreateType(ModuleBuilder moduleBuilder)
        {
            if (wchar_t != null)
                return wchar_t;

            FieldBuilder fixedElementField, charField;
            CustomAttributeBuilder attrBuilder;
            Type attrType;
            Type[] ctorParams;
            ConstructorInfo ctor;
            ConstructorBuilder charCtorBuilder;
            ILGenerator ctorIl, methodIl;
            MethodBuilder methodBuilder;
            Label lbl1, lbl2, lbl3;
            LocalBuilder lcl0, lcl1, lcl2;

            //default values
            int wcharLength = Constants.SIZEOF_WCHAR_T;

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "wchar_t",
                TypeAttributes.NotPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType));

            // nested value type for the fixed buffer 
            TypeBuilder nestedTypeBuilder = typeBuilder.DefineNestedType(
                "nestedFixedBuffer",
                TypeAttributes.NestedPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType),
                PackingSize.Unspecified,
                wcharLength);

            attrType = typeof(CompilerGeneratedAttribute);
            ctor = attrType.GetConstructor(Array.Empty<Type>());
            attrBuilder = new CustomAttributeBuilder(ctor, Array.Empty<object>());
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            attrType = typeof(UnsafeValueTypeAttribute);
            ctor = attrType.GetConstructor(Array.Empty<Type>());
            attrBuilder = new CustomAttributeBuilder(ctor, Array.Empty<object>());
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            //not necessary
            attrType = typeof(StructLayoutAttribute);
            ctorParams = new Type[] { typeof(LayoutKind) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { LayoutKind.Sequential },
                new FieldInfo[] { attrType.GetTypeInfo().GetField("Size") },
                new object[] { wcharLength });
            nestedTypeBuilder.SetCustomAttribute(attrBuilder);

            fixedElementField = nestedTypeBuilder.DefineField("FixedElementField", typeof(byte), FieldAttributes.Public);

            charField = typeBuilder.DefineField("ch", nestedTypeBuilder.AsType(), FieldAttributes.Public);
            attrType = typeof(FixedBufferAttribute);
            ctorParams = new Type[] { typeof(Type), typeof(int) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { typeof(byte), wcharLength });
            charField.SetCustomAttribute(attrBuilder);

            /* constructors */
            #region public wchar_t(char c)
            charCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(char) });
            ctorIl = charCtorBuilder.GetILGenerator();

            lbl1 = ctorIl.DefineLabel();

            lcl0 = ctorIl.DeclareLocal(typeof(char*));
            lcl1 = ctorIl.DeclareLocal(typeof(byte*));
            lcl2 = ctorIl.DeclareLocal(typeof(byte*), true);

            ctorIl.Emit(OpCodes.Nop);
            //char* charArr = stackalloc char[1];
            ctorIl.Emit(OpCodes.Ldc_I4_2);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Localloc);
            ctorIl.Emit(OpCodes.Stloc_0);
            //charArr[0] = c;
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stind_I2);
            //fixed (byte* bArr = this.ch)
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Stloc_2);
            ctorIl.Emit(OpCodes.Ldloc_2);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_1);
            //NativeNCurses.Encoding.GetBytes(charArr, 1, bArr, wchar_t_size);
            ctorIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetProperty("Encoding", BindingFlags.NonPublic | BindingFlags.Static).GetMethod);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Ldloc_1);
            ctorIl.Emit(OpCodes.Ldc_I4_2);
            ctorIl.Emit(OpCodes.Callvirt, typeof(Encoding).GetMethod(
                "GetBytes",
                new Type[] 
                {
                    typeof(char*), typeof(int),
                    typeof(byte*), typeof(int)
                }));
            ctorIl.Emit(OpCodes.Pop); //TODO: use returned value
            ctorIl.Emit(OpCodes.Nop);
            //end fixed
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_2);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            /* operator overrrides */
            #region public static explicit operator char(wchar_t ch)
            methodBuilder = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(char),
                new Type[] { typeBuilder.AsType() });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(char));
            lcl1 = methodIl.DeclareLocal(typeof(char*));
            lcl0 = methodIl.DeclareLocal(typeof(bool));
            lcl0 = methodIl.DeclareLocal(typeof(char));
            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            //char* charArr = stackalloc char[1];
            methodIl.Emit(OpCodes.Ldc_I4_2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Localloc);
            methodIl.Emit(OpCodes.Stloc_1);
            //if (NativeNCurses.Encoding.GetChars(ch.ch, wchar_t_size, charArr, 1) > 0)
            methodIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetProperty("Encoding", BindingFlags.NonPublic | BindingFlags.Static).GetMethod);
            methodIl.Emit(OpCodes.Ldarga_S, 0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Ldc_I4_2);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Callvirt, typeof(Encoding).GetMethod(
                "GetChars",
                new Type[]
                {
                    typeof(byte*), typeof(int),
                    typeof(char*), typeof(int)
                }));
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Cgt);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //ret = charArr[0];
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ldind_U2);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            //throw new InvalidCastException("Failed to cast to current encoding");
            methodIl.Emit(OpCodes.Ldstr, "Failed to cast to current encoding");
            methodIl.Emit(OpCodes.Newobj, typeof(InvalidCastException).GetTypeInfo().GetConstructor(new Type[] { typeof(string) }));
            methodIl.Emit(OpCodes.Throw);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl3);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static implicit operator wchar_t(char ch)
            methodBuilder = typeBuilder.DefineMethod("op_Implicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeof(char) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Newobj, charCtorBuilder);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            nestedTypeBuilder.CreateTypeInfo().AsType();
            return (wchar_t = typeBuilder.CreateTypeInfo().AsType());
        }
    }
}
