using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;

using NCurses.Core.Interop.Wide;
using NCurses.Core.Interop.Small;
using NCurses.Core.Interop.WideStr;
using NCurses.Core.Interop.SmallStr;
using NCurses.Core.Interop.Mouse;

using NCurses.Core.Interop.Dynamic.cchar_t;
using NCurses.Core.Interop.Dynamic.chtype;
using NCurses.Core.Interop.Dynamic.wchar_t;

namespace NCurses.Core.Interop.Dynamic
{
    internal static class DynamicTypeBuilder
    {
        //everything depends on these types, tread carefully
        internal static Type cchar_t { get; private set; }
        internal static Type chtype { get; private set; }
        internal static Type wchar_t { get; private set; }
        internal static Type schar { get; private set; }

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
        }

        #region INCursesWrapper
        internal static Type CreateNCursesWrapper(string dllName)
        {
            TypeBuilder typeBuilder = ModuleBuilder.DefineType("NCursesWrapper", TypeAttributes.Public);
            typeBuilder.AddInterfaceImplementation(typeof(INCursesWrapper));

            //regular base methods
            MethodInfo[] interfaceMethod = typeof(INCursesWrapper).GetTypeInfo().GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            return typeBuilder.CreateTypeInfo().AsType();
        }
        #endregion

        #region char (re)implementation
        internal static Type CreateCharTypeWrapper(string dllName)
        {
            schar = typeof(schar_t);

            TypeBuilder typeBuilder = ModuleBuilder.DefineType("NCursesCharTypeWrapper", TypeAttributes.Public);
            MethodInfo[] interfaceMethod;
            Type interfaceType;

            typeBuilder.AddInterfaceImplementation(typeof(INativeWrapper));

            //small string (char) methods
            interfaceType = typeof(INCursesWrapperSmallStr<>).MakeGenericType(schar);
            typeBuilder.AddInterfaceImplementation(interfaceType);
            interfaceMethod = interfaceType.GetMethods();
            foreach (MethodInfo ifMethod in interfaceMethod)
                createInterfaceImplementation(dllName, typeBuilder, ifMethod);

            return typeBuilder.CreateTypeInfo().AsType();
        }
        #endregion

        #region Custom single/multibyte types
        internal static Type CreateCustomTypeWrapper(string dllName, bool unicodeSuported)
        {
            chtype = chtypeBuilder.CreateType();
            //if (unicodeSuported) //TODO: can be changed at runtime?
            //{
                cchar_t = cchar_tBuilder.CreateType();
                wchar_t = wchar_tBuilder.CreateType();
            //}

            TypeBuilder typeBuilder = ModuleBuilder.DefineType("NCursesCustomTypeWrapper", TypeAttributes.Public);
            MethodInfo[] interfaceMethod;
            Type interfaceType;

            typeBuilder.AddInterfaceImplementation(typeof(INativeWrapper));

            if (unicodeSuported)
            {
                //wide (cchar_t) methods
                interfaceType = typeof(INCursesWrapperWide<,,,>).MakeGenericType(cchar_t, wchar_t, chtype, schar);
                typeBuilder.AddInterfaceImplementation(interfaceType);
                interfaceMethod = interfaceType.GetMethods();
                foreach (MethodInfo ifMethod in interfaceMethod)
                    createInterfaceImplementation(dllName, typeBuilder, ifMethod);

                //wide string (wchar_t) methods
                interfaceType = typeof(INCursesWrapperWideStr<,>).MakeGenericType(wchar_t, schar);
                typeBuilder.AddInterfaceImplementation(interfaceType);
                interfaceMethod = interfaceType.GetMethods();
                foreach (MethodInfo ifMethod in interfaceMethod)
                    createInterfaceImplementation(dllName, typeBuilder, ifMethod);
            }

            //small (chtype) methods
            interfaceType = typeof(INCursesWrapperSmall<,>).MakeGenericType(chtype, schar);
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
            Label lbl;
            LocalBuilder local;
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
                new FieldInfo[] { attrType.GetTypeInfo().GetField("EntryPoint") },
                new object[] { methodName });
            nativeMethod.SetCustomAttribute(attrBuilder);

            //from C# 7.2: add required attributes (in IL .param followed by .custom)
            requiredModifiers = parameterInfo.Select(x => x.GetRequiredCustomModifiers()).ToArray();

            //declare interface method
            interfaceMethod = typeBuilder.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard,
                retType, null, null, parameterTypes, requiredModifiers, null);
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
            if (retType != typeof(void))
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

        #endregion

        #region Mono.Cecil

        //internal static ModuleDefinition ModuleDefinition { get; private set; }
        //internal static AssemblyDefinition AssemblyDefinition { get; private set; }

        //private static MemoryStream NCursesWrapperStream;
        //private static MemoryStream CharWrapperStream;

        //static DynamicTypeBuilder()
        //{
        //    AssemblyNameDefinition asmName = new AssemblyNameDefinition("NCurses.Core.Interop.Dynamic.Generated", new Version(1, 0));

        //    AssemblyDefinition = Mono.Cecil.AssemblyDefinition.CreateAssembly(asmName, asmName.Name, ModuleKind.Dll);
        //    ModuleDefinition = AssemblyDefinition.MainModule;

        //    //CustomAttribute permissionAttr = new CustomAttribute(
        //    //    ModuleDefinition.ImportReference(
        //    //        typeof(System.Security.Permissions.SecurityPermissionAttribute).GetConstructor(new Type[] { typeof(System.Security.Permissions.SecurityAction) })));
        //    //permissionAttr.ConstructorArguments.Add(
        //    //    new CustomAttributeArgument(ModuleDefinition.ImportReference(typeof(System.Security.Permissions.SecurityAction)),
        //    //    System.Security.Permissions.SecurityAction.RequestMinimum));
        //    //permissionAttr.Properties.Add(
        //    //    new Mono.Cecil.CustomAttributeNamedArgument("SkipVerification",
        //    //        new CustomAttributeArgument(ModuleDefinition.TypeSystem.Boolean, true)));
        //    //AssemblyDefinition.CustomAttributes.Add(permissionAttr);

        //    CustomAttribute unverifiableAttr = new CustomAttribute(
        //        ModuleDefinition.ImportReference(typeof(System.Security.UnverifiableCodeAttribute).GetConstructor(Type.EmptyTypes)));
        //    ModuleDefinition.CustomAttributes.Add(unverifiableAttr);
        //}

        //#region INCursesWrapper
        //internal static INCursesWrapper CreateNCursesWrapper(string dllName)
        //{
        //    TypeDefinition typeDefinition = new TypeDefinition(AssemblyDefinition.Name.Name, "NCursesWrapper", Mono.Cecil.TypeAttributes.Public);
        //    typeDefinition.Methods.Add(CreateDefaultConstructor());
        //    ModuleDefinition.Types.Add(typeDefinition);

        //    typeDefinition.Interfaces.Add(
        //        new InterfaceImplementation(ModuleDefinition.ImportReference(typeof(INCursesWrapper))));

        //    //regular base methods
        //    MethodInfo[] interfaceMethod = typeof(INCursesWrapper).GetTypeInfo().GetMethods();
        //    foreach (MethodInfo ifMethod in interfaceMethod)
        //        createInterfaceImplementation(dllName, typeDefinition, ifMethod);

        //    MethodReference currentMethod = ModuleDefinition.ImportReference(typeof(DynamicTypeBuilder).GetMethod("CreateNCursesWrapper", BindingFlags.NonPublic | BindingFlags.Static));
        //    MethodDefinition currentMethodDef = currentMethod.Resolve();
        //    Instruction lastInstruction = currentMethodDef.Body.Instructions[currentMethodDef.Body.Instructions.Count - 4];

        //    ILProcessor il = currentMethodDef.Body.GetILProcessor();
        //    il.InsertBefore(lastInstruction, lastInstruction = Instruction.Create(OpCodes.Newobj, typeDefinition.Methods[0]));
        //    il.InsertAfter(lastInstruction, lastInstruction = Instruction.Create(OpCodes.Ret));

        //    return null;
        //}
        //#endregion

        //#region char (re)implementation
        //internal static Type CreateCharTypeWrapper(string dllName)
        //{
        //    schar = typeof(schar_t);

        //    TypeDefinition typeDefinition = new TypeDefinition(AssemblyDefinition.Name.Name, "NCursesCharTypeWrapper", Mono.Cecil.TypeAttributes.Public);
        //    typeDefinition.Methods.Add(CreateDefaultConstructor());
        //    ModuleDefinition.Types.Add(typeDefinition);

        //    MethodInfo[] interfaceMethod;
        //    Type interfaceType;

        //    typeDefinition.Interfaces.Add(
        //        new InterfaceImplementation(ModuleDefinition.ImportReference(typeof(INativeWrapper))));

        //    //small string (char) methods
        //    interfaceType = typeof(INCursesWrapperSmallStr<>).MakeGenericType(schar);
        //    typeDefinition.Interfaces.Add(
        //        new InterfaceImplementation(ModuleDefinition.ImportReference(interfaceType)));

        //    interfaceMethod = interfaceType.GetMethods();
        //    foreach (MethodInfo ifMethod in interfaceMethod)
        //        createInterfaceImplementation(dllName, typeDefinition, ifMethod);

        //    return typeDefinition.GetType();
        //}
        //#endregion

        //#region Custom single/multibyte types
        //internal static Type CreateCustomTypeWrapper(string dllName, bool unicodeSuported)
        //{
        //    chtype = chtypeBuilder.CreateType();
        //    if (unicodeSuported)
        //    {
        //        cchar_t = cchar_tBuilder.CreateType();
        //        wchar_t = wchar_tBuilder.CreateType();
        //    }

        //    TypeDefinition typeDefinition = new TypeDefinition(AssemblyDefinition.Name.Name, "NCursesCustomTypeWrapper", Mono.Cecil.TypeAttributes.Public);
        //    typeDefinition.Methods.Add(CreateDefaultConstructor());
        //    ModuleDefinition.Types.Add(typeDefinition);

        //    MethodInfo[] interfaceMethod;
        //    Type interfaceType;

        //    typeDefinition.Interfaces.Add(
        //        new InterfaceImplementation(ModuleDefinition.ImportReference(typeof(INativeWrapper))));

        //    if (unicodeSuported)
        //    {
        //        //wide (cchar_t) methods
        //        interfaceType = typeof(INCursesWrapperWide<,,,>).MakeGenericType(cchar_t, wchar_t, chtype, schar);
        //        typeDefinition.Interfaces.Add(
        //            new InterfaceImplementation(ModuleDefinition.ImportReference(interfaceType)));

        //        interfaceMethod = interfaceType.GetMethods();
        //        foreach (MethodInfo ifMethod in interfaceMethod)
        //            createInterfaceImplementation(dllName, typeDefinition, ifMethod);

        //        //wide string (wchar_t) methods
        //        interfaceType = typeof(INCursesWrapperWideStr<,>).MakeGenericType(wchar_t, schar);
        //        typeDefinition.Interfaces.Add(
        //            new InterfaceImplementation(ModuleDefinition.ImportReference(interfaceType)));
        //        interfaceMethod = interfaceType.GetMethods();
        //        foreach (MethodInfo ifMethod in interfaceMethod)
        //            createInterfaceImplementation(dllName, typeDefinition, ifMethod);
        //    }

        //    //small (chtype) methods
        //    interfaceType = typeof(INCursesWrapperSmall<,>).MakeGenericType(chtype, schar);
        //    typeDefinition.Interfaces.Add(
        //        new InterfaceImplementation(ModuleDefinition.ImportReference(interfaceType)));
        //    interfaceMethod = interfaceType.GetMethods();
        //    foreach (MethodInfo ifMethod in interfaceMethod)
        //        createInterfaceImplementation(dllName, typeDefinition, ifMethod);

        //    return typeDefinition.GetType();
        //}
        //#endregion

        //#region add default constructor
        //private static MethodDefinition CreateDefaultConstructor()
        //{
        //    MethodDefinition ctor = new MethodDefinition(".ctor", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.HideBySig
        //        | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, ModuleDefinition.TypeSystem.Void);

        //    // create the constructor's method body
        //    ILProcessor il = ctor.Body.GetILProcessor();

        //    il.Append(il.Create(OpCodes.Ldarg_0));

        //    // call the base constructor
        //    il.Append(il.Create(OpCodes.Call, ModuleDefinition.ImportReference(typeof(object).GetConstructor(Array.Empty<Type>()))));

        //    il.Append(il.Create(OpCodes.Nop));
        //    il.Append(il.Create(OpCodes.Ret));

        //    return ctor;
        //}
        //#endregion

        //#region Native implementation generator
        //private static void createInterfaceImplementation(string dllName, TypeDefinition typeDefinition, MethodInfo ifMethod)
        //{
        //    CustomAttribute dllImportAttr;
        //    Type attrType;
        //    Type[] ctorParams;
        //    ConstructorInfo ctor;
        //    MethodDefinition nativeMethod, interfaceMethod;
        //    Type[][] requiredModifiers;

        //    //get interface method info
        //    string methodName = ifMethod.Name;
        //    Type retType = ifMethod.ReturnType;
        //    ParameterInfo[] parameterInfo = ifMethod.GetParameters();
        //    Type[] parameterTypes = parameterInfo.Select(x => x.ParameterType).ToArray();

        //    //declare native method
        //    nativeMethod = new MethodDefinition(string.Format("ncurses_{0}", methodName),
        //        Mono.Cecil.MethodAttributes.Private | Mono.Cecil.MethodAttributes.PInvokeImpl | Mono.Cecil.MethodAttributes.Static,
        //        ModuleDefinition.ImportReference(retType));
        //    typeDefinition.Methods.Add(nativeMethod);

        //    //declare native method DllImportAttribute
        //    dllImportAttr = new CustomAttribute(
        //        ModuleDefinition.ImportReference(typeof(DllImportAttribute).GetConstructor(new Type[] { typeof(string) } )));
        //    dllImportAttr.ConstructorArguments.Add(
        //        new CustomAttributeArgument(ModuleDefinition.TypeSystem.String, dllName));
        //    dllImportAttr.Fields.Add(
        //        new Mono.Cecil.CustomAttributeNamedArgument("EntryPoint",
        //            new CustomAttributeArgument(ModuleDefinition.TypeSystem.String, methodName)));
        //    nativeMethod.CustomAttributes.Add(dllImportAttr);

        //    //from C# 7.2: add required attributes (in IL .param followed by .custom)
        //    //mono: TODO???
        //    //requiredModifiers = parameterInfo.Select(x => x.GetRequiredCustomModifiers()).ToArray();

        //    //declare interface method
        //    interfaceMethod = new MethodDefinition(methodName,
        //        Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Virtual,
        //        ModuleDefinition.ImportReference(retType));
        //    typeDefinition.Methods.Add(interfaceMethod);

        //    foreach(ParameterInfo p in parameterInfo)
        //    {
        //        Mono.Cecil.ParameterAttributes currentAttr;
        //        if (p.IsIn)
        //            currentAttr = Mono.Cecil.ParameterAttributes.In;
        //        else if (p.IsOut)
        //            currentAttr = Mono.Cecil.ParameterAttributes.Out;
        //        else
        //            currentAttr = Mono.Cecil.ParameterAttributes.None;
        //        interfaceMethod.Parameters.Add(new ParameterDefinition(p.Name, currentAttr, ModuleDefinition.ImportReference(p.ParameterType)));
        //    }

        //    ILProcessor interfaceMethodIL = interfaceMethod.Body.GetILProcessor();
        //    interfaceMethodIL.Emit(OpCodes.Nop);

        //    //load arguments (as interface implementation you need to skip Ldarg_0)
        //    for (int i = 0; i < parameterTypes.Length; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                interfaceMethodIL.Emit(OpCodes.Ldarg_1);
        //                break;
        //            case 1:
        //                interfaceMethodIL.Emit(OpCodes.Ldarg_2);
        //                break;
        //            case 2:
        //                interfaceMethodIL.Emit(OpCodes.Ldarg_3);
        //                break;
        //            default:
        //                interfaceMethodIL.Emit(OpCodes.Ldarg_S, interfaceMethod.Parameters[i]);
        //                break;
        //        }
        //    }

        //    //execute native method
        //    interfaceMethodIL.Emit(OpCodes.Call, nativeMethod);

        //    Instruction lbl;

        //    //store and load return value
        //    if (retType != typeof(void))
        //    {
        //        lbl = Instruction.Create(OpCodes.Nop);

        //        interfaceMethodIL.Append(Instruction.Create(OpCodes.Stloc_0));
        //        interfaceMethodIL.Append(Instruction.Create(OpCodes.Br_S, lbl));
        //        interfaceMethodIL.Append(lbl);
        //        interfaceMethodIL.Append(Instruction.Create(OpCodes.Ldloc_0));
        //    }
        //    else
        //        interfaceMethodIL.Emit(OpCodes.Nop);

        //    interfaceMethodIL.Emit(OpCodes.Ret);
        //}
        //#endregion

        #endregion
    }
}
