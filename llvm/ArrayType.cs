
using System;

namespace LLVM {

public class ArrayType : SequentialType {

    internal ArrayType(IntPtr ptr) : base(ptr)
    {
    }

    public static ArrayType Get(Type type, uint eltCount)
    {
        return new ArrayType(LLVM.ArrayType(type, eltCount));
    }

    public uint GetNumElements()
    {
        return LLVM.GetArrayLength(this);
    }
}

}
