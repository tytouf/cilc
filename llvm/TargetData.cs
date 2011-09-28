
using System;

namespace LLVM {

public class TargetData : RefBase {
    internal TargetData(IntPtr ptr) : base(ptr) { }

    public TargetData(string rep) : base(LLVM.CreateTargetData(rep))
    {
    }

    public uint GetPointerSize()
    {
        return LLVM.PointerSize(this);
    }

    public IntegerType GetIntPtrType()
    {
        IntPtr ptr = LLVM.IntPtrType(this);
        Console.WriteLine(ptr);
        IntegerType ty = IntegerType.Get(ptr);
        Console.WriteLine(ty.getRef());
        return ty;
    }

}

} // end of namespace LLVM
