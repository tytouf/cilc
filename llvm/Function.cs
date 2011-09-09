
using System;

namespace LLVM {

public class Function : RefBase {

    public Function(Module mod, string name, FunctionType fnTy) : base(LLVM.AddFunction(mod, name, fnTy))
    {
    }

}

}
