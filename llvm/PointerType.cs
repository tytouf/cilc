
using System;

namespace LLVM {

public class PointerType : SequentialType {
    internal PointerType(IntPtr ptr) : base(ptr)
    {
    }

    public static PointerType Get(Type type, uint addressSpace)
    {
        return new PointerType(LLVM.PointerType(type, addressSpace));
    }
}

}
