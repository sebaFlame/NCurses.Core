using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    internal static class DynamicTypeBuilder
    {
        private static ModuleBuilder moduleBuilder;

        private static void createModuleBuilder()
        {
            AssemblyName asmName = new AssemblyName("NCurses.Core.Interop.Dynamic");
            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            moduleBuilder = asmBuilder.DefineDynamicModule(asmName.Name);
        }

        #region NCURSES_CH_T
        internal static Type CreateWCHARType()
        {
            FieldBuilder attrField, extField, charField;
            CustomAttributeBuilder attrBuilder;
            Type attrType;
            Type[] ctorParams;
            ConstructorInfo ctor;
            ConstructorBuilder wcharCtorBuilder, ctorBuilder, charCtorBuilder;
            ILGenerator ctorIl;

            if (moduleBuilder == null)
                createModuleBuilder();

            /* fields */
            TypeBuilder typeBuilder = moduleBuilder.DefineType("NCURSES_CH_T", TypeAttributes.Public, typeof(ValueType));
            attrField = typeBuilder.DefineField("attr", typeof(uint), FieldAttributes.Public);

            /* chars field with attribute */
            charField = typeBuilder.DefineField("chars", typeof(byte[]), FieldAttributes.Public);
            attrType = typeof(MarshalAsAttribute);
            ctorParams = new Type[] { typeof(UnmanagedType) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { UnmanagedType.ByValArray },
                new FieldInfo[] { attrType.GetTypeInfo().GetField("SizeConst") },
                new object[] { (Constants.SIZEOF_WCHAR_T * 5) });
            charField.SetCustomAttribute(attrBuilder);

            extField = typeBuilder.DefineField("ext_color", typeof(int), FieldAttributes.Public);

            /* struct attribute - not necessary? */
            attrType = typeof(StructLayoutAttribute);
            ctorParams = new Type[] { typeof(LayoutKind) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { LayoutKind.Sequential });
            typeBuilder.SetCustomAttribute(attrBuilder);

            MethodInfo mInfo;

            /* constructors */
            /* NCURSES_CH_T(char ch) */
            wcharCtorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(char) });
            ctorIl = wcharCtorBuilder.GetILGenerator();

            Label lbl1 = ctorIl.DefineLabel(); // 17
            Label lbl2 = ctorIl.DefineLabel(); // 46

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Initobj, typeBuilder.AsType());
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_S, (Constants.SIZEOF_WCHAR_T * Constants.CCHARW_MAX));
            ctorIl.Emit(OpCodes.Newarr, typeof(Byte));
            ctorIl.Emit(OpCodes.Stfld, charField);
            mInfo = typeof(OSPlatform).GetTypeInfo().GetProperty(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" : "Linux").GetMethod;
            ctorIl.Emit(OpCodes.Call, mInfo);
            mInfo = typeof(RuntimeInformation).GetTypeInfo().GetMethod("IsOSPlatform");
            ctorIl.Emit(OpCodes.Call, mInfo);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ceq);
            ctorIl.Emit(OpCodes.Stloc_3);
            ctorIl.Emit(OpCodes.Ldloc_3);
            ctorIl.Emit(OpCodes.Brfalse_S, lbl1);
            ctorIl.Emit(OpCodes.Ldstr, "Incorrect struct for this platform");
            ctor = typeof(InvalidOperationException).GetTypeInfo().GetConstructor(new Type[] { typeof(string) });
            ctorIl.Emit(OpCodes.Newobj, ctor);
            ctorIl.Emit(OpCodes.Throw);
            ctorIl.MarkLabel(lbl1);
            mInfo = typeof(Encoding).GetTypeInfo().GetProperty(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Unicode" : "UTF32").GetMethod;
            ctorIl.Emit(OpCodes.Call, mInfo);
            mInfo = typeof(Encoding).GetTypeInfo().GetMethod("GetEncoder");
            ctorIl.Emit(OpCodes.Callvirt, mInfo);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Newarr, typeof(Char));
            ctorIl.Emit(OpCodes.Dup);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stelem_I2);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ldc_I4_1);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, charField);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ldc_I4_S, (Constants.SIZEOF_WCHAR_T * Constants.CCHARW_MAX));
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ldloca_S, ctorIl.DeclareLocal(typeof(Int32)));
            ctorIl.Emit(OpCodes.Ldloca_S, ctorIl.DeclareLocal(typeof(Int32)));
            ctorIl.Emit(OpCodes.Ldloca_S, ctorIl.DeclareLocal(typeof(bool)));
            mInfo = typeof(Encoder).GetTypeInfo().GetMethod("Convert", new Type[] { typeof(char[]), typeof(int), typeof(int),
                typeof(byte[]), typeof(int), typeof(int), typeof(bool), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(bool).MakeByRefType() });
            ctorIl.Emit(OpCodes.Callvirt, mInfo);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldloc_0);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            ctorIl.Emit(OpCodes.Ceq);
            LocalBuilder newBool = ctorIl.DeclareLocal(typeof(bool));
            ctorIl.Emit(OpCodes.Stloc_S, newBool);
            ctorIl.Emit(OpCodes.Ldloc_S, newBool);
            ctorIl.Emit(OpCodes.Brfalse_S, lbl2);
            ctorIl.Emit(OpCodes.Ldstr, "Failed to convert character for marshaling");
            ctorIl.Emit(OpCodes.Newobj, ctor);
            ctorIl.Emit(OpCodes.Throw);
            ctorIl.MarkLabel(lbl2);
            ctorIl.Emit(OpCodes.Ret);

            /* NCURSES_CH_T(char ch, uint attr) */
            ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(char), typeof(uint) });
            ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Call, wcharCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);

            /* NCURSES_CH_T(uint c) */
            charCtorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(uint) });
            ctorIl = charCtorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Initobj, typeBuilder.AsType());
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldc_I4_S, (Constants.SIZEOF_WCHAR_T * Constants.CCHARW_MAX));
            ctorIl.Emit(OpCodes.Newarr, typeof(Byte));
            ctorIl.Emit(OpCodes.Stfld, charField);
            ctorIl.Emit(OpCodes.Ldarg_1);
            mInfo = typeof(BitConverter).GetTypeInfo().GetMethod("GetBytes", new Type[] { typeof(UInt32) });
            ctorIl.Emit(OpCodes.Call, mInfo);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldfld, charField);
            ctorIl.Emit(OpCodes.Ldc_I4_0);
            mInfo = typeof(Array).GetTypeInfo().GetMethod("CopyTo", new Type[] { typeof(Array), typeof(Int32) });
            ctorIl.Emit(OpCodes.Callvirt, mInfo);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ret);

            /* NCURSES_CH_T(uint c, uint attr) */
            ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(uint), typeof(uint) });
            ctorIl = ctorBuilder.GetILGenerator();

            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Call, charCtorBuilder);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Nop);
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_2);
            ctorIl.Emit(OpCodes.Stfld, attrField);
            ctorIl.Emit(OpCodes.Ret);

            return typeBuilder.CreateTypeInfo().AsType();
        }
        #endregion

        #region NCursesWrapper
        internal static Type CreateNCursesWrapper(string dllName)
        {
            if (moduleBuilder == null)
                createModuleBuilder();

            TypeBuilder typeBuilder = moduleBuilder.DefineType("NCursesWrapper", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeof(INCursesWrapper));

            MethodInfo[] interfaceMethod = typeof(INCursesWrapper).GetTypeInfo().GetMethods();
            foreach(MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            return typeBuilder.CreateTypeInfo().AsType();
        }

        private static void createInterfaceImplementation(string dllName, TypeBuilder typeBuilder, MethodInfo ifMethod)
        {
            CustomAttributeBuilder attrBuilder;
            Type attrType;
            Type[] ctorParams;
            ConstructorInfo ctor;
            MethodBuilder nativeMethod, interfaceMethod;
            ILGenerator interfaceMethodIL;
            Label lbl;
            LocalBuilder local;

            //get interface method info
            string methodName = ifMethod.Name;
            Type retType = ifMethod.ReturnType;
            ParameterInfo[] parameterInfo = ifMethod.GetParameters();
            Type[] parameterTypes = parameterInfo.Select(x => x.ParameterType).ToArray();

            //declare native method
            nativeMethod = typeBuilder.DefineMethod(string.Format("ncurses_{0}", methodName),
                MethodAttributes.Private | MethodAttributes.PinvokeImpl | MethodAttributes.Static,
                retType, parameterTypes);

            //declare native method DllImportAttribute
            attrType = typeof(DllImportAttribute);
            ctorParams = new Type[] { typeof(string) };
            ctor = attrType.GetTypeInfo().GetConstructor(ctorParams);
            attrBuilder = new CustomAttributeBuilder(ctor, new object[] { dllName },
                new FieldInfo[] { attrType.GetTypeInfo().GetField("EntryPoint") },
                new object[] { methodName });
            nativeMethod.SetCustomAttribute(attrBuilder);

            //declare interface method
            interfaceMethod = typeBuilder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Virtual,
                retType, parameterTypes);
            interfaceMethodIL = interfaceMethod.GetILGenerator();

            //if native method has a return type, define a local and a label
            if (retType != typeof(void))
            {
                local = interfaceMethodIL.DeclareLocal(retType);
                lbl = interfaceMethodIL.DefineLabel();
            }

            interfaceMethodIL.Emit(OpCodes.Nop);

            //load arguments (as interface implementation you need to skip Ldarg_0)
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        interfaceMethodIL.Emit(OpCodes.Ldarg_1);
                        break;
                    case 1:
                        interfaceMethodIL.Emit(OpCodes.Ldarg_2);
                        break;
                    case 2:
                        interfaceMethodIL.Emit(OpCodes.Ldarg_3);
                        break;
                    default:
                        interfaceMethodIL.Emit(OpCodes.Ldarg_S, (i + 1));
                        break;
                }
            }

            //execute native method
            interfaceMethodIL.Emit(OpCodes.Call, nativeMethod);

            //store and load return value
            if(retType != typeof(void))
            {
                interfaceMethodIL.Emit(OpCodes.Stloc_0);
                interfaceMethodIL.Emit(OpCodes.Br_S, lbl);
                interfaceMethodIL.MarkLabel(lbl);
                interfaceMethodIL.Emit(OpCodes.Ldloc_0);
            }
            else
                interfaceMethodIL.Emit(OpCodes.Nop);

            interfaceMethodIL.Emit(OpCodes.Ret);

            //define interface implementation as the interface override
            typeBuilder.DefineMethodOverride(interfaceMethod, ifMethod);
        }
        #endregion
    }
}
