
using System;

namespace LLVM {

public class Function : Value { // should inherit GlobalValue -> Constant -> User -> Value

    FunctionType _type;

    public Function(Module mod, string name, FunctionType fnTy) : base(LLVM.AddFunction(mod, name, fnTy))
    {
        _type = fnTy;
    }

    public Type GetReturnType()
    {
        IntPtr ptr = LLVM.GetReturnType(this._type);
        if (ptr == IntPtr.Zero) {
            return Type.GetVoid();
        }
        return Type.GetType(ptr);
    }

    public uint GetNumParams()
    {
	return LLVM.CountParams(this);
    }

    public void GetParams(Type[] parms)
    {
	//TODO: LLVM.GetParams(this, parms);
    }

    public Value GetParam(uint idx)
    {
	return new Value(LLVM.GetParam(this, idx));
    }

    public Value GetFirstParam()
    {
	return new Value(LLVM.GetFirstParam(this));
    }

}

}
