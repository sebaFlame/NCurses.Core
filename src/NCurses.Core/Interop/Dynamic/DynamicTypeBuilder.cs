using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;

using NCurses.Core.Interop.Dynamic.cchar_t;
using NCurses.Core.Interop.Dynamic.chtype;
using NCurses.Core.Interop.Dynamic.wchar_t;
using NCurses.Core.Interop.Dynamic.MEVENT;
using System.Threading;

namespace NCurses.Core.Interop.Dynamic
{
    internal static class DynamicTypeBuilder
    {
        //everything depends on these types, tread carefully
        internal static Type cchar_t { get; }
        internal static Type chtype { get; }
        internal static Type wchar_t { get; }
        internal static Type schar { get; }
        internal static Type MEVENT { get; }

        #region Reflection.Emit implementation

        internal static ModuleBuilder ModuleBuilder { get; private set; }

        static DynamicTypeBuilder()
        {
            AssemblyName asmName = new AssemblyName("NCurses.Core.Interop.Dynamic.Generated");

            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder = asmBuilder.DefineDynamicModule(asmName.Name);

            //allow unsafe code, not necessary??? (because on parent module?)
            CustomAttributeBuilder attrBuilder = new CustomAttributeBuilder(
                typeof(System.Security.UnverifiableCodeAttribute).GetConstructor(Array.Empty<Type>()),
                Array.Empty<object>());
            ModuleBuilder.SetCustomAttribute(attrBuilder);

#if DYNAMICTYPES
            chtype = chtypeBuilder.CreateType(ModuleBuilder);
            MEVENT = MEVENTBuilder.CreateType(ModuleBuilder, chtype);
            cchar_t = cchar_tBuilder.CreateType(ModuleBuilder, chtype);
            wchar_t = wchar_tBuilder.CreateType(ModuleBuilder);
#else
            chtype = typeof(chtype.chtype);
            MEVENT = typeof(MEVENT.MEVENT);
            cchar_t = typeof(cchar_t.cchar_t);
            wchar_t = typeof(wchar_t.wchar_t);
#endif
            schar = typeof(schar_t);
        }

        #region INCursesWrapper
        internal static Type CreateDefaultWrapper<TInterface>(string dllName)
        {
            TypeBuilder typeBuilder = ModuleBuilder.DefineType(typeof(TInterface).Name.Substring(1), TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeof(TInterface));

            //regular base methods
            MethodInfo[] interfaceMethod = typeof(TInterface).GetTypeInfo().GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            return typeBuilder.CreateTypeInfo().AsType();
        }
#endregion

#region Custom single/multibyte types
        internal static Type CreateCustomTypeWrapper(string dllName, Type cchar_t, Type wchar_t, Type chtype, Type schar, Type mevent)
        {
            TypeBuilder typeBuilder = ModuleBuilder.DefineType("NCursesCustomTypeWrapper", TypeAttributes.Public);
            MethodInfo[] interfaceMethod;
            Type interfaceType;

            //wide (cchar_t) methods
            interfaceType = typeof(IMultiByteWrapper<,,,>).MakeGenericType(cchar_t, wchar_t, chtype, schar);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            interfaceMethod = interfaceType.GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            //wide string (wchar_t) methods
            interfaceType = typeof(IWideCharWrapper<,>).MakeGenericType(wchar_t, schar);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            interfaceMethod = interfaceType.GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            //small (chtype) methods
            interfaceType = typeof(ISingleByteWrapper<,,>).MakeGenericType(chtype, schar, mevent);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            interfaceMethod = interfaceType.GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            //small (char) methods
            interfaceType = typeof(ICharWrapper<>).MakeGenericType(schar);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            interfaceMethod = interfaceType.GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            return typeBuilder.CreateTypeInfo().AsType();
        }
#endregion

#region Native implementation generator
        private static void createInterfaceImplementation(string dllName, TypeBuilder typeBuilder, MethodInfo ifMethod)
        {
            CustomAttributeBuilder attrBuilder;
            Type attrType;
            Type[] ctorParams;
            ConstructorInfo ctor;
            MethodBuilder nativeMethod, interfaceMethod;
            ILGenerator interfaceMethodIL;
            Label lblNoLock, lblNoLockFinally;
            LocalBuilder local, enableLock;
            Type[][] requiredModifiers;

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
                new FieldInfo[] { attrType.GetTypeInfo().GetField(nameof(DllImportAttribute.EntryPoint)) },
                new object[] { methodName });
            nativeMethod.SetCustomAttribute(attrBuilder);

            //from C# 7.2: add required attributes (in IL .param followed by .custom)
            requiredModifiers = parameterInfo.Select(x => x.GetRequiredCustomModifiers()).ToArray();

            //declare interface method
            interfaceMethod = typeBuilder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard,
                retType, null, null, parameterTypes, requiredModifiers, null);
            interfaceMethodIL = interfaceMethod.GetILGenerator();

            enableLock = interfaceMethodIL.DeclareLocal(typeof(bool));
            lblNoLock = interfaceMethodIL.DefineLabel();
            lblNoLockFinally = interfaceMethodIL.DefineLabel();

            //if native method has a return type, define a local and a label
            if (retType != typeof(void))
                local = interfaceMethodIL.DeclareLocal(retType);

            //begin of try block
            interfaceMethodIL.BeginExceptionBlock();

            //check if locking is enabled and lock if it is
            interfaceMethodIL.Emit(OpCodes.Nop);
            interfaceMethodIL.Emit(OpCodes.Call, typeof(NativeNCurses).GetProperty(nameof(NativeNCurses.EnableLocking), BindingFlags.NonPublic | BindingFlags.Static).GetMethod);
            interfaceMethodIL.Emit(OpCodes.Stloc_0);
            interfaceMethodIL.Emit(OpCodes.Ldloc_0);
            interfaceMethodIL.Emit(OpCodes.Brfalse_S, lblNoLock);
            interfaceMethodIL.Emit(OpCodes.Ldsfld, typeof(NativeNCurses).GetField(nameof(NativeNCurses.SyncRoot), BindingFlags.NonPublic | BindingFlags.Static));
            interfaceMethodIL.Emit(OpCodes.Call, typeof(Monitor).GetMethod(nameof(Monitor.Enter), new Type[] { typeof(object) }));
            interfaceMethodIL.Emit(OpCodes.Nop);

            //when locking is disabled
            interfaceMethodIL.MarkLabel(lblNoLock);

            //load arguments (as instance you need to skip Ldarg_0)
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
            if (retType != typeof(void))
                interfaceMethodIL.Emit(OpCodes.Stloc_1);

            //begin of finally block
            interfaceMethodIL.BeginFinallyBlock();

            //check if locking is enabled and exit lock if it is
            interfaceMethodIL.Emit(OpCodes.Nop);
            interfaceMethodIL.Emit(OpCodes.Ldloc_0);
            interfaceMethodIL.Emit(OpCodes.Brfalse_S, lblNoLockFinally);
            interfaceMethodIL.Emit(OpCodes.Ldsfld, typeof(NativeNCurses).GetField(nameof(NativeNCurses.SyncRoot), BindingFlags.NonPublic | BindingFlags.Static));
            interfaceMethodIL.Emit(OpCodes.Call, typeof(Monitor).GetMethod(nameof(Monitor.Exit)));
            interfaceMethodIL.Emit(OpCodes.Nop);

            //end of finally block
            interfaceMethodIL.MarkLabel(lblNoLockFinally);

            //end of exception block
            interfaceMethodIL.EndExceptionBlock();

            //return
            if (retType != typeof(void))
                interfaceMethodIL.Emit(OpCodes.Ldloc_1);
            interfaceMethodIL.Emit(OpCodes.Ret);

            //define interface implementation as the interface override
            typeBuilder.DefineMethodOverride(interfaceMethod, ifMethod);
        }
#endregion

#endregion
    }
}
