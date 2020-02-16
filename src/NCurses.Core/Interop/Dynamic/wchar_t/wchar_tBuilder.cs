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
            ConstructorBuilder charCtorBuilder, spanCtorBuilder, arrCtorBuilder, intCtorBuilder;
            ILGenerator ctorIl, methodIl;
            MethodBuilder methodBuilder, opExplicitChar, opEquality, wchEquals;
            PropertyBuilder propBuilder;
            Label lbl1, lbl2, lbl3, lbl4;
            LocalBuilder lcl0, lcl1, lcl2, lcl3, lcl4;
            ParameterBuilder parameterBuilder;

            //default values
            int wcharLength = Constants.SIZEOF_WCHAR_T;

            //TODO: is built into netcoreapp/.NET framework
            Type readOnlyAttribute = typeof(wchar_tBuilder).Assembly.GetType("System.Runtime.CompilerServices.IsReadOnlyAttribute");

            TypeBuilder typeBuilder = moduleBuilder.DefineType(
                "wchar_t",
                TypeAttributes.NotPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType));
            typeBuilder.AddInterfaceImplementation(typeof(IMultiByteChar));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<>).MakeGenericType(typeBuilder.AsType()));

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
            ctorIl.Emit(OpCodes.Ldc_I4_S, Constants.SIZEOF_WCHAR_T);
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

            #region wchar_t(Span<byte> encodedBytesChar)
            spanCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(Span<byte>) });
            ctorIl = spanCtorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(int));
            lcl1 = ctorIl.DeclareLocal(typeof(bool));

            lbl1 = ctorIl.DefineLabel();
            lbl2 = ctorIl.DefineLabel();

            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Stloc_S, lcl0);
            ctorIl.Emit(OpCodes.Br_S, lbl1);
            ctorIl.MarkLabel(lbl2);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Ldarga_S, 1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Call, typeof(Span<byte>).GetMethod("get_Item"));
            ctorIl.Emit(OpCodes.Ldind_U1);
            ctorIl.Emit(OpCodes.Stind_I1);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Stloc_S, lcl0);
            ctorIl.MarkLabel(lbl1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(Span<byte>).GetMethod("get_Length"));
            ctorIl.Emit(OpCodes.Clt);
            ctorIl.Emit(OpCodes.Stloc_S, lcl1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl1);
            ctorIl.Emit(OpCodes.Brtrue_S, lbl2);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region wchar_t(ArraySegment<byte> encodedBytesChar)
            arrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(ArraySegment<byte>) });
            ctorIl = arrCtorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(int));
            lcl1 = ctorIl.DeclareLocal(typeof(bool));

            lbl1 = ctorIl.DefineLabel();
            lbl2 = ctorIl.DefineLabel();

            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Stloc_S, lcl0);
            ctorIl.Emit(OpCodes.Br_S, lbl1);
            ctorIl.MarkLabel(lbl2);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Array").GetMethod);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Offset").GetMethod);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Ldelem_U1);
            ctorIl.Emit(OpCodes.Stind_I1);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Add);
            ctorIl.Emit(OpCodes.Stloc_S, lcl0);
            ctorIl.MarkLabel(lbl1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Ldarga_S, (byte)1);
            ctorIl.Emit(OpCodes.Call, typeof(ArraySegment<byte>).GetProperty("Count").GetMethod);
            ctorIl.Emit(OpCodes.Clt);
            ctorIl.Emit(OpCodes.Stloc_S, lcl1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl1);
            ctorIl.Emit(OpCodes.Brtrue_S, lbl2);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region public wchar_t(int c)
            intCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(int) });
            ctorIl = intCtorBuilder.GetILGenerator();

            lcl0 = ctorIl.DeclareLocal(typeof(byte*));
            lcl1 = ctorIl.DeclareLocal(typeof(byte*), true);

            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldflda, charField);
            ctorIl.Emit(OpCodes.Ldflda, fixedElementField);
            ctorIl.Emit(OpCodes.Stloc_S, lcl1);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl1);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_S, lcl0);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldloc_S, lcl0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            MethodInfo unsafeMethod = typeof(Unsafe).GetMethod(nameof(Unsafe.Write)).MakeGenericMethod(typeof(int));
            ctorIl.Emit(OpCodes.Call, unsafeMethod);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Conv_U);
            ctorIl.Emit(OpCodes.Stloc_S, lcl1);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            /* operator overrrides */
            #region public static bool operator ==(in wchar_t wchLeft, in wchar_t wchRight)
            opEquality = typeBuilder.DefineMethod("op_Equality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });
            methodIl = opEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(byte*));
            lcl1 = methodIl.DeclareLocal(typeof(byte*));
            lcl2 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl3 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl4 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();

            parameterBuilder = opEquality.DefineParameter(1, ParameterAttributes.In, "wchLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = opEquality.DefineParameter(2, ParameterAttributes.In, "wchRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_S, lcl3);
            methodIl.Emit(OpCodes.Ldloc_S, lcl3);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetMethod(nameof(NativeNCurses.EqualBytesLongUnrolled), BindingFlags.NonPublic | BindingFlags.Static));
            methodIl.Emit(OpCodes.Stloc_S, lcl4);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl4);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static bool operator !=(in wchar_t wchLeft, in wchar_t wchRight)
            opEquality = typeBuilder.DefineMethod("op_Inequality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });
            methodIl = opEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(byte*));
            lcl1 = methodIl.DeclareLocal(typeof(byte*));
            lcl2 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl3 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl4 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();

            parameterBuilder = opEquality.DefineParameter(1, ParameterAttributes.In, "wchLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = opEquality.DefineParameter(2, ParameterAttributes.In, "wchRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_S, lcl3);
            methodIl.Emit(OpCodes.Ldloc_S, lcl3);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)wcharLength);
            methodIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetMethod(nameof(NativeNCurses.EqualBytesLongUnrolled), BindingFlags.NonPublic | BindingFlags.Static));
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Stloc_S, lcl4);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl4);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator char(wchar_t ch)
            opExplicitChar = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(char),
                new Type[] { typeBuilder.AsType() });
            methodIl = opExplicitChar.GetILGenerator();

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
            methodIl.Emit(OpCodes.Ldc_I4_S, Constants.SIZEOF_WCHAR_T);
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

            /* methods */
            #region public bool Equals(wchar_t other)
            wchEquals = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeBuilder.AsType() });
            methodIl = wchEquals.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(bool));
            lbl1 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldarga_S, 1);
            methodIl.Emit(OpCodes.Call, opEquality);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public override bool Equals(object obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(object) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Call, wchEquals);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public override bool Equals(IChar obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(IChar) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Call, wchEquals);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public override bool Equals(IMultiByteChar obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(IMultiByteChar) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Call, wchEquals);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            /* properties */
            #region Char
            methodBuilder = typeBuilder.DefineMethod("get_Char",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(char),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldobj, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Call, opExplicitChar);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Char",
                PropertyAttributes.None,
                typeof(char),
                new Type[] { typeof(char) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region EncodedChar
            methodBuilder = typeBuilder.DefineMethod("get_EncodedChar",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(Span<byte>),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(byte*));
            lcl1 = methodIl.DeclareLocal(typeof(byte*), true);
            lcl2 = methodIl.DeclareLocal(typeof(Span<byte>));

            lbl1 = methodIl.DefineLabel();

            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charField);
            methodIl.Emit(OpCodes.Ldflda, fixedElementField);
            methodIl.Emit(OpCodes.Stloc_S, lcl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl1);
            methodIl.Emit(OpCodes.Conv_U);
            methodIl.Emit(OpCodes.Stloc_S, lcl0);
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldloc_S, lcl0);
            methodIl.Emit(OpCodes.Ldc_I4_S, (byte)Constants.SIZEOF_WCHAR_T);
            methodIl.Emit(OpCodes.Newobj, typeof(Span<byte>).GetTypeInfo().GetConstructor(new Type[] { typeof(void*), typeof(int) }));
            methodIl.Emit(OpCodes.Stloc_S, lcl2);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_S, lcl2);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("EncodedChar",
                PropertyAttributes.None,
                typeof(Span<byte>),
                new Type[] { typeof(Span<byte>) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            nestedTypeBuilder.CreateTypeInfo().AsType();
            return (wchar_t = typeBuilder.CreateTypeInfo().AsType());
        }
    }
}
