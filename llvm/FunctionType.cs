
using System;

namespace LLVM {

public class FunctionType : DerivedType {

    public FunctionType(IntPtr ptr) : base(ptr)
    {
    }

    public static FunctionType Get(Type returnTy, Type[] paramsTy,
                                   bool isVarArg)
    {
	IntPtr[] parms = Array.ConvertAll(paramsTy, t => (IntPtr)t);
	return new FunctionType(LLVM.FunctionType(returnTy, parms,
		       (uint) parms.Length, isVarArg));
    }

    public static FunctionType Get(Type returnTy, bool isVarArg)
    {
        return new FunctionType(LLVM.FunctionType(returnTy, null, 0, isVarArg));
    }

}

}
