
using System;

namespace LLVM {

public class Function : RefBase {

    public Function(Module mod, string name, FunctionType fnTy) : base(LLVM.AddFunction(mod, name, fnTy))
    {
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
