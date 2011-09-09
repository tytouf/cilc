
using System;

namespace LLVM {

public class BasicBlock : Value {

    public BasicBlock(IntPtr ptr) : base(ptr)
    {
    }

    public BasicBlock(Function func, string name = "bb") : base(LLVM.AppendBasicBlock(func, name))
    {
    }

}

}
