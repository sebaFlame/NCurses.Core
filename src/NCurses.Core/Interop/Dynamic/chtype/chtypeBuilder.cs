using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.Dynamic.chtype
{
    public static class chtypeBuilder
    {
        private static Type chtype;// = typeof(chtype);

        internal static Type CreateType()
        {
            if (chtype != null)
                return chtype;

            FieldBuilder charWithAttrField;
            PropertyBuilder propBuilder, charPropertyBuilder;
            CustomAttributeBuilder attrBuilder;
            ConstructorBuilder ctorBuilder, charCtorBuilder, charAttrCtorBuilder, attrCtorBuilder;
            ILGenerator ctorIl, methodIl;
            MethodBuilder methodBuilder, opEquality, chEquality;
            ParameterBuilder parameterBuilder;
            Label lbl1, lbl2, lbl3, lbl4;
            LocalBuilder lcl0, lcl1, lcl2, lcl3;

            bool isLong = false;
            if (Constants.CHTYPE_TYPE == typeof(UInt64))
                isLong = true;
            //TODO: is built into netcoreapp/.NET framework
            Type readOnlyAttribute = typeof(DynamicTypeBuilder).Assembly.GetType("System.Runtime.CompilerServices.IsReadOnlyAttribute");

            TypeBuilder typeBuilder = DynamicTypeBuilder.ModuleBuilder.DefineType(
                "chtype",
                TypeAttributes.NotPublic | TypeAttributes.SequentialLayout | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit,
                typeof(ValueType));
            typeBuilder.AddInterfaceImplementation(typeof(ISingleByteChar));
            typeBuilder.AddInterfaceImplementation(typeof(INCursesChar));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<INCursesChar>));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<ISingleByteChar>));
            typeBuilder.AddInterfaceImplementation(typeof(IEquatable<>).MakeGenericType(typeBuilder.AsType()));

            /* fields */
            charWithAttrField = typeBuilder.DefineField("charWithAttr", Constants.CHTYPE_TYPE, FieldAttributes.Public);

            /* constructors */
            #region public chtype(sbyte ch)
            charCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(sbyte) });
            ctorIl = charCtorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            //this.charWithAttr = (UInt32)ch;
            ctorIl.Emit(OpCodes.Ldarg_1);
            if(isLong)
                ctorIl.Emit(OpCodes.Conv_I8);
            ctorIl.Emit(OpCodes.Stfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region public chtype(sbyte ch, ulong attr)
            charAttrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(sbyte), typeof(ulong) });
            ctorIl = charAttrCtorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            //:this(ch)
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Call, charCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //this.charWithAttr |= (UInt32)attr;
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ldarg_2);
            if(!isLong)
                ctorIl.Emit(OpCodes.Conv_U4);
            ctorIl.Emit(OpCodes.Or);
            ctorIl.Emit(OpCodes.Stfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region public chtype(sbyte ch, ulong attr, short pair)
            ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            //: this(ch, attr)
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Call, charAttrCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            //this.charWithAttr |= (UInt32)NativeNCurses.COLOR_PAIR(pair);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ldarg_3);
            ctorIl.Emit(OpCodes.Call, typeof(NativeNCurses).GetMethod("COLOR_PAIR"));
            if(isLong)
                ctorIl.Emit(OpCodes.Conv_U8);
            ctorIl.Emit(OpCodes.Or);
            ctorIl.Emit(OpCodes.Stfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            #region public chtype(ulong attr)
            attrCtorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                new Type[] { typeof(ulong) });
            ctorIl = attrCtorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            //this.charWithAttr = (UInt32)attr;
            ctorIl.Emit(OpCodes.Ldarg_1);
            if(!isLong)
                ctorIl.Emit(OpCodes.Conv_U4);
            ctorIl.Emit(OpCodes.Stfld, charWithAttrField);
            ctorIl.Emit(OpCodes.Ret);
            #endregion

            /* properties */
            #region Char
            methodBuilder = typeBuilder.DefineMethod("get_Char",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(char),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            if(!isLong)
                methodIl.Emit(OpCodes.Conv_U8);
            methodIl.Emit(OpCodes.Ldsfld, typeof(Attrs).GetField("CHARTEXT"));
            methodIl.Emit(OpCodes.And);
            methodIl.Emit(OpCodes.Conv_U2);
            methodIl.Emit(OpCodes.Ret);

            charPropertyBuilder = typeBuilder.DefineProperty("Char",
                PropertyAttributes.None,
                typeof(char),
                new Type[] { typeof(char) });
            charPropertyBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Atttributes
            methodBuilder = typeBuilder.DefineMethod("get_Attributes",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(ulong),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            //(ulong)((this.charWithAttr ^ (this.charWithAttr & Attrs.COLOR)) & Attrs.ATTRIBUTES)
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            if (!isLong)
                methodIl.Emit(OpCodes.Conv_U8);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            if (!isLong)
                methodIl.Emit(OpCodes.Conv_U8);
            methodIl.Emit(OpCodes.Ldsfld, typeof(Attrs).GetField("COLOR"));
            methodIl.Emit(OpCodes.And);
            methodIl.Emit(OpCodes.Xor);
            methodIl.Emit(OpCodes.Ldsfld, typeof(Attrs).GetField("ATTRIBUTES"));
            methodIl.Emit(OpCodes.And);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Attributes",
                PropertyAttributes.None,
                typeof(ulong),
                new Type[] { typeof(ulong) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            #region Color
            methodBuilder = typeBuilder.DefineMethod("get_Color",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(short),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            if (!isLong)
                methodIl.Emit(OpCodes.Conv_U8);
            methodIl.Emit(OpCodes.Call, typeof(Constants).GetMethod("PAIR_NUMBER"));
            methodIl.Emit(OpCodes.Conv_I2);
            methodIl.Emit(OpCodes.Ret);

            propBuilder = typeBuilder.DefineProperty("Color",
                PropertyAttributes.None,
                typeof(short),
                new Type[] { typeof(short) });
            propBuilder.SetGetMethod(methodBuilder);
            #endregion

            /* operator overrides */
            #region public static chtype operator | (chtype ch, ulong attr)
            methodBuilder = typeBuilder.DefineMethod("op_BitwiseOr",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeBuilder.AsType(), typeof(ulong) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            //ch.charWithAttr |= (UInt32)attr;
            methodIl.Emit(OpCodes.Ldarg_S, (byte)0);
            methodIl.Emit(OpCodes.Ldflda, charWithAttrField);
            methodIl.Emit(OpCodes.Dup);
            if(isLong)
                methodIl.Emit(OpCodes.Ldind_I8);
            else
                methodIl.Emit(OpCodes.Ldind_U4);
            methodIl.Emit(OpCodes.Ldarg_1);
            if(!isLong)
                methodIl.Emit(OpCodes.Conv_U4);
            methodIl.Emit(OpCodes.Or);
            if(isLong)
                methodIl.Emit(OpCodes.Stind_I8);
            else
                methodIl.Emit(OpCodes.Stind_I4);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static chtype operator & (chtype ch, ulong attr)
            methodBuilder = typeBuilder.DefineMethod("op_BitwiseAnd",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeBuilder.AsType(), typeof(ulong) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            //ch.charWithAttr &= (UInt32)attr;
            methodIl.Emit(OpCodes.Ldarg_S, (byte)0);
            methodIl.Emit(OpCodes.Ldflda, charWithAttrField);
            methodIl.Emit(OpCodes.Dup);
            if (isLong)
                methodIl.Emit(OpCodes.Ldind_I8);
            else
                methodIl.Emit(OpCodes.Ldind_U4);
            methodIl.Emit(OpCodes.Ldarg_1);
            if(!isLong)
                methodIl.Emit(OpCodes.Conv_U4);
            methodIl.Emit(OpCodes.And);
            if(isLong)
                methodIl.Emit(OpCodes.Stind_I8);
            else
                methodIl.Emit(OpCodes.Stind_I4);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator char(chtype ch)
            methodBuilder = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(char),
                new Type[] { typeBuilder.AsType() });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(char));
            lbl1 = methodIl.DefineLabel();

            //return ch.Char;
            methodIl.Emit(OpCodes.Ldarga_S, (byte)0);
            methodIl.Emit(OpCodes.Call, charPropertyBuilder.GetMethod);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator sbyte(chtype ch)
            methodBuilder = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(sbyte),
                new Type[] { typeBuilder.AsType() });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(sbyte));
            lbl1 = methodIl.DefineLabel();

            //return (sbyte)ch.Char;
            methodIl.Emit(OpCodes.Ldarga_S, (byte)0);
            methodIl.Emit(OpCodes.Call, charPropertyBuilder.GetMethod);
            methodIl.Emit(OpCodes.Conv_I1);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator chtype(char ch)
            methodBuilder = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeof(char) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(bool));
            lcl1 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();

            //if (ch > sbyte.MaxValue)
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldc_I4_S, sbyte.MaxValue);
            methodIl.Emit(OpCodes.Cgt);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldstr, "This character can not be expressed in 1 byte");
            methodIl.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetTypeInfo().GetConstructor(new Type[] { typeof(string) }));
            methodIl.Emit(OpCodes.Throw);
            methodIl.MarkLabel(lbl1);
            //return new chtype((sbyte)ch);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Conv_I1);
            methodIl.Emit(OpCodes.Newobj, charCtorBuilder);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static explicit operator chtype(sbyte ch)
            methodBuilder = typeBuilder.DefineMethod("op_Explicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeof(sbyte) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            //return new chtype(ch);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Newobj, charCtorBuilder);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static implicit operator ulong(chtype ch)
            methodBuilder = typeBuilder.DefineMethod("op_Implicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(ulong),
                new Type[] { typeBuilder.AsType() });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(ulong));
            lbl1 = methodIl.DefineLabel();

            //return ch.charWithAttr;
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            if(!isLong)
                methodIl.Emit(OpCodes.Conv_U8);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static implicit operator chtype(ulong val)
            methodBuilder = typeBuilder.DefineMethod("op_Implicit",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeBuilder.AsType(),
                new Type[] { typeof(ulong) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lbl1 = methodIl.DefineLabel();

            //return new chtype(val);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Newobj, attrCtorBuilder);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static bool operator ==(in chtype chLeft, in chtype chRight)
            opEquality = typeBuilder.DefineMethod("op_Equality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });

            parameterBuilder = opEquality.DefineParameter(1, ParameterAttributes.In, "chLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = opEquality.DefineParameter(2, ParameterAttributes.In, "chRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            methodIl = opEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(bool));
            lbl1 = methodIl.DefineLabel();

            //return chLeft.charWithAttr == chRight.charWithAttr;
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public static bool operator !=(in chtype chLeft, in chtype chRight)
            methodBuilder = typeBuilder.DefineMethod("op_Inequality",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Static,
                typeof(bool),
                new Type[] { typeBuilder.AsType().MakeByRefType(), typeBuilder.AsType().MakeByRefType() });

            parameterBuilder = methodBuilder.DefineParameter(1, ParameterAttributes.In, "chLeft");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            parameterBuilder = methodBuilder.DefineParameter(2, ParameterAttributes.In, "chRight");
            attrBuilder = new CustomAttributeBuilder(typeof(InAttribute).GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);
            attrBuilder = new CustomAttributeBuilder(readOnlyAttribute.GetConstructor(Array.Empty<Type>()), Array.Empty<object>());
            parameterBuilder.SetCustomAttribute(attrBuilder);

            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(bool));
            lbl1 = methodIl.DefineLabel();

            //return chLeft.charWithAttr != chRight.charWithAttr;
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Ldfld, charWithAttrField);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Ceq);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public bool Equals(INCursesSCHAR obj)
            chEquality = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(ISingleByteChar) });
            methodIl = chEquality.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeBuilder.AsType());
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(ISingleByteChar));
            lcl3 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            //if (obj is chtype other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            //this == other
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloca_S, lcl0);
            methodIl.Emit(OpCodes.Call, opEquality);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public bool Equals(INCursesChar obj)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeof(INCursesChar) });
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(ISingleByteChar));
            lcl1 = methodIl.DeclareLocal(typeof(bool));
            lcl2 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();

            //if (obj is INCursesSCHAR other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Isinst, typeof(ISingleByteChar));
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldnull);
            methodIl.Emit(OpCodes.Cgt_Un);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            //this.Equals(other);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Call, chEquality);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public bool Equals(chtype other)
            methodBuilder = typeBuilder.DefineMethod("Equals",
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(bool),
                new Type[] { typeBuilder.AsType() });
            methodIl = methodBuilder.GetILGenerator();

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
            lcl2 = methodIl.DeclareLocal(typeof(object));
            lcl3 = methodIl.DeclareLocal(typeof(bool));

            lbl1 = methodIl.DefineLabel();
            lbl2 = methodIl.DefineLabel();
            lbl3 = methodIl.DefineLabel();
            lbl4 = methodIl.DefineLabel();

            //if (obj is chtype other)
            methodIl.Emit(OpCodes.Nop);
            methodIl.Emit(OpCodes.Ldarg_1);
            methodIl.Emit(OpCodes.Dup);
            methodIl.Emit(OpCodes.Stloc_2);
            methodIl.Emit(OpCodes.Isinst, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Brfalse_S, lbl1);
            methodIl.Emit(OpCodes.Ldloc_2);
            methodIl.Emit(OpCodes.Unbox_Any, typeBuilder.AsType());
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Ldc_I4_1);
            methodIl.Emit(OpCodes.Br_S, lbl2);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.MarkLabel(lbl2);
            methodIl.Emit(OpCodes.Stloc_1);
            methodIl.Emit(OpCodes.Ldloc_1);
            methodIl.Emit(OpCodes.Brfalse_S, lbl3);
            //this == other
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldloca_S, lcl0);
            methodIl.Emit(OpCodes.Call, opEquality);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl3);
            methodIl.Emit(OpCodes.Ldc_I4_0);
            methodIl.Emit(OpCodes.Stloc_3);
            methodIl.Emit(OpCodes.Br_S, lbl4);
            methodIl.MarkLabel(lbl4);
            methodIl.Emit(OpCodes.Ldloc_3);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            #region public override int GetHashCode()
            methodBuilder = typeBuilder.DefineMethod("GetHashCode",
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
                typeof(int),
                Array.Empty<Type>());
            methodIl = methodBuilder.GetILGenerator();

            lcl0 = methodIl.DeclareLocal(typeof(int));
            lbl1 = methodIl.DefineLabel();

            //return -1027107954 + this.charWithAttr.GetHashCode();
            methodIl.Emit(OpCodes.Ldc_I4, -1027107954);
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldflda, charWithAttrField);
            methodIl.Emit(OpCodes.Call, Constants.CHTYPE_TYPE.GetMethod("GetHashCode"));
            methodIl.Emit(OpCodes.Add);
            methodIl.Emit(OpCodes.Stloc_0);
            methodIl.Emit(OpCodes.Br_S, lbl1);
            methodIl.MarkLabel(lbl1);
            methodIl.Emit(OpCodes.Ldloc_0);
            methodIl.Emit(OpCodes.Ret);
            #endregion

            return (chtype = typeBuilder.CreateTypeInfo().AsType());
        }
    }
}
