
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using LLVM;

namespace LLVM {

class LLVM {
    const string LLVM_LIB = "libLLVM-2.9.so";

    [DllImport(LLVM_LIB, EntryPoint="LLVMContextCreate")]
    public static extern IntPtr ContextCreate();

    [DllImport(LLVM_LIB, EntryPoint="LLVMGetGlobalContext")]
    public static extern IntPtr GetGlobalContext();

#region Type
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetTypeKind")]
    public static extern uint GetTypeKind(IntPtr ptr);

    [DllImport(LLVM_LIB, EntryPoint="LLVMVoidType")]
    public static extern IntPtr VoidType();
    [DllImport(LLVM_LIB, EntryPoint="LLVMLabelType")]
    public static extern IntPtr LabelType();
    [DllImport(LLVM_LIB, EntryPoint="LLVMOpaqueType")]
    public static extern IntPtr OpaqueType();

    [DllImport(LLVM_LIB, EntryPoint="LLVMInt1Type")]
    public static extern IntPtr Int1Type();
    [DllImport(LLVM_LIB, EntryPoint="LLVMInt8Type")]
    public static extern IntPtr Int8Type();
    [DllImport(LLVM_LIB, EntryPoint="LLVMInt16Type")]
    public static extern IntPtr Int16Type();
    [DllImport(LLVM_LIB, EntryPoint="LLVMInt32Type")]
    public static extern IntPtr Int32Type();
    [DllImport(LLVM_LIB, EntryPoint="LLVMInt64Type")]
    public static extern IntPtr Int64Type();

    [DllImport(LLVM_LIB, EntryPoint="LLVMIntType")]
    public static extern IntPtr IntType(uint numBits);

    [DllImport(LLVM_LIB, EntryPoint="LLVMGetIntTypeWidth")]
    public static extern uint GetIntTypeWidth(IntPtr ptr);
    [DllImport(LLVM_LIB, EntryPoint="LLVMCreateTypeHandle")]
    public static extern IntPtr CreateTypeHandle(IntPtr potentiallyAbstractType);
    [DllImport(LLVM_LIB, EntryPoint="LLVMResolveTypeHandle")]
    public static extern IntPtr ResolveTypeHandle(IntPtr typeHandleRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMDisposeTypeHandle")]
    public static extern void DisposeTypeHandle(IntPtr typeHandleRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMRefineType")]
    public static extern void RefineType(IntPtr abstractType, IntPtr ty);

    [DllImport(LLVM_LIB, EntryPoint="LLVMStructType")]
    public static extern IntPtr StructType(IntPtr[] typesRef, uint count, bool packed);
    [DllImport(LLVM_LIB, EntryPoint="LLVMCountStructElementTypes")]
    public static extern uint CountStructElementTypes(IntPtr typeRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMArrayType")]
    public static extern IntPtr ArrayType(IntPtr ty, uint count);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetArrayLength")]
    public static extern uint GetArrayLength(IntPtr arrayRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMPointerType")]
    public static extern IntPtr PointerType(IntPtr ty, uint addressSpace);

    [DllImport(LLVM_LIB, EntryPoint="LLVMVectorType")]
    public static extern IntPtr VectorType(IntPtr ty, uint count);

    [DllImport(LLVM_LIB, EntryPoint="LLVMFunctionType")]
    public static extern IntPtr FunctionType(IntPtr returnTy, IntPtr[] paramsTy, uint paramsCount, bool isVarArg);
    [DllImport(LLVM_LIB, EntryPoint="LLVMIsFunctionVarArg")]
    public static extern bool IsFunctionVarArg(IntPtr funcTy);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetReturnType")]
    public static extern IntPtr GetReturnType(IntPtr funcTy);
#endregion

#region Function
    [DllImport(LLVM_LIB, EntryPoint="LLVMAddFunction")]
    public static extern IntPtr AddFunction(IntPtr module, string name, IntPtr funcTy);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetNamedFunction")]
    public static extern IntPtr GetNamedFunction(IntPtr module, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMCountParams")]
    public static extern uint CountParams(IntPtr func);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetParams")]
    public static extern void GetParams(IntPtr func, out IntPtr[] paramsTy);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetParam")]
    public static extern IntPtr GetParam(IntPtr func, uint idx);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetFirstParam")]
    public static extern IntPtr GetFirstParam(IntPtr func);
#endregion

#region Value
    [DllImport(LLVM_LIB, EntryPoint="LLVMSetValueName")]
    public static extern void SetValueName(IntPtr val, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetValueName")]
    public static extern string GetValueName(IntPtr val);
    [DllImport(LLVM_LIB, EntryPoint="LLVMDumpValue")]
    public static extern void DumpValue(IntPtr val);
    [DllImport(LLVM_LIB, EntryPoint="LLVMTypeOf")]
    public static extern IntPtr TypeOf(IntPtr val);
#endregion

#region Global
    [DllImport(LLVM_LIB, EntryPoint="LLVMAddGlobal")]
    public static extern IntPtr AddGlobal(IntPtr mod, IntPtr ty, string name);
#endregion

#region Module
    [DllImport(LLVM_LIB, EntryPoint="LLVMModuleCreateWithName")]
    public static extern IntPtr ModuleCreateWithName(string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMModuleCreateWithNameInContext")]
    public static extern IntPtr
        ModuleCreateWithNameInContext(string name, IntPtr ctxRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMGetModuleContext")]
    public static extern IntPtr GetModuleContext(IntPtr modRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMDisposeModule")]
    public static extern void DisposeModule(IntPtr modRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMGetDataLayout")]
    public static extern string GetDataLayout(IntPtr modRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMSetDataLayout")]
    public static extern void SetDataLayout(IntPtr modRef, string triple);

    [DllImport(LLVM_LIB, EntryPoint="LLVMGetTarget")]
    public static extern string GetTarget(IntPtr modRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMSetTarget")]
    public static extern void SetTarget(IntPtr modRef, string triple);

    [DllImport(LLVM_LIB, EntryPoint="LLVMDumpModule")]
    public static extern void DumpModule(IntPtr modRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMAddTypeName")]
    public static extern bool AddTypeName(IntPtr modRef, string name, IntPtr typeRef);
#endregion

#region BasicBlock
    [DllImport(LLVM_LIB, EntryPoint="LLVMAppendBasicBlock")]
    public static extern IntPtr AppendBasicBlock(IntPtr func, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMInsertBasicBlock")]
    public static extern IntPtr InsertBasicBlock(IntPtr bb, string name);
#endregion

#region Builder
    [DllImport(LLVM_LIB, EntryPoint="LLVMCreateBuilder")]
    public static extern IntPtr CreateBuilder();
    [DllImport(LLVM_LIB, EntryPoint="LLVMPositionBuilderAtEnd")]
    public static extern void PositionBuilderAtEnd(IntPtr bldRef, IntPtr bbRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildRetVoid")]
    public static extern IntPtr BuildRetVoid(IntPtr bldRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildRet")]
    public static extern IntPtr BuildRet(IntPtr bldRef, IntPtr valRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildBr")]
    public static extern IntPtr BuildBr(IntPtr bldRef, IntPtr bbRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildAlloca")]
    public static extern IntPtr BuildAlloca(IntPtr bldRef, IntPtr tyRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildLoad")]
    public static extern IntPtr BuildLoad(IntPtr bldRef, IntPtr valRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildStore")]
    public static extern IntPtr BuildStore(IntPtr bldRef, IntPtr valRef, IntPtr ptrRef);

    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildAdd")]
    public static extern IntPtr BuildAdd(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildiNSWAdd")]
    public static extern IntPtr BuildNSWAdd(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNUWAdd")]
    public static extern IntPtr BuildNUWAdd(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildFAdd")]
    public static extern IntPtr BuildFAdd(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildSub")]
    public static extern IntPtr BuildSub(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNSWSub")]
    public static extern IntPtr BuildNSWSub(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNUWSub")]
    public static extern IntPtr BuildNUWSub(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildFSub")]
    public static extern IntPtr BuildFSub(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildMul")]
    public static extern IntPtr BuildMul(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNSWMul")]
    public static extern IntPtr BuildNSWMul(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNUWMul")]
    public static extern IntPtr BuildNUWMul(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildFMul")]
    public static extern IntPtr BuildFMul(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildSDiv")]
    public static extern IntPtr BuildSDiv(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildUDiv")]
    public static extern IntPtr BuildUDiv(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);

    // Logic
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildAnd")]
    public static extern IntPtr BuildAnd(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildOr")]
    public static extern IntPtr BuildOr(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildXor")]
    public static extern IntPtr BuildXor(IntPtr bldRef, IntPtr leftRef, IntPtr rightRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildNot")]
    public static extern IntPtr BuildNot(IntPtr bldRef, IntPtr vRef, string name);

    //
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildTrunc")]
    public static extern IntPtr BuildTrunc(IntPtr bldRef, IntPtr val, IntPtr destTy, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildSExt")]
    public static extern IntPtr BuildSExt(IntPtr bldRef, IntPtr val, IntPtr destTy, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildIntCast")]
    public static extern IntPtr BuildIntCast(IntPtr bldRef, IntPtr val, IntPtr destTy, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildBitCast")]
    public static extern IntPtr BuildBitCast(IntPtr bldRef, IntPtr val, IntPtr destTy, string name);

    //
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildCall")]
    public static extern IntPtr BuildCall(IntPtr bldRef, IntPtr fnRef, IntPtr[] argsRef, uint numArgs, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildIsNull")]
    public static extern IntPtr BuildIsNull(IntPtr bldRef, IntPtr valRef, string name);
    [DllImport(LLVM_LIB, EntryPoint="LLVMBuildIsNotNull")]
    public static extern IntPtr BuildIsNotNull(IntPtr bldRef, IntPtr valRef, string name);
#endregion

#region Constant
    [DllImport(LLVM_LIB, EntryPoint="LLVMConstInt")]
    public static extern IntPtr ConstInt(IntPtr tyRef, long val, bool signExtend);
    [DllImport(LLVM_LIB, EntryPoint="LLVMSizeOf")]
    public static extern IntPtr SizeOf(IntPtr tyRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMAlignOf")]
    public static extern IntPtr AlignOf(IntPtr tyRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMGetZero")]
    public static extern IntPtr GetZero(IntPtr tyRef);
#endregion

#region BitWriter
    [DllImport(LLVM_LIB, EntryPoint="LLVMWriteBitcodeToFile")]
    public static extern int WriteBitcodeToFile(IntPtr modRef, string path);
#endregion

#region Analysis
    [DllImport(LLVM_LIB, EntryPoint="LLVMVerifyModule")]
    public static extern bool VerifyModule(IntPtr modRef, uint action, out string outMessage);
    [DllImport(LLVM_LIB, EntryPoint="LLVMVerifyFunction")]
    public static extern bool VerifyFunction(IntPtr fnRef, uint action);
#endregion

#region Targets
    [DllImport(LLVM_LIB, EntryPoint="LLVMCreateTargetData")]
    public static extern IntPtr CreateTargetData(string rep);
    [DllImport(LLVM_LIB, EntryPoint="LLVMByteOrder")]
    public static extern uint ByteOrder(IntPtr tgtDataRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMPointerSize")]
    public static extern uint PointerSize(IntPtr tgtDataRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMIntPtrType")]
    public static extern IntPtr IntPtrType(IntPtr tgtDataRef);
    [DllImport(LLVM_LIB, EntryPoint="LLVMDisposeTargetData")]
    public static extern void DisposeTargetData(IntPtr tgtDataRef);
#endregion
} // end of class LLVM

} // end of namespace LLVM
