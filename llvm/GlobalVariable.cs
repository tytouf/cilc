
using System;

namespace LLVM {

public class GlobalVariable : Value {

    public GlobalVariable(Module mod, Type ty, string name) : base(LLVM.AddGlobal(mod, ty, name))
    {
    }

}

}
