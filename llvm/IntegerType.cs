
using System;

namespace LLVM {

public class IntegerType: DerivedType {

    internal IntegerType(IntPtr ptr) : base(ptr)
    {
    }

    public static IntegerType Get(uint bits)
    {
        return new IntegerType(LLVM.IntType(bits));
    }

    public uint GetBitWidth()
    {
        return LLVM.GetIntTypeWidth(this);
    }

    public UInt64 GetBitMask()
    {
        return UInt64.MaxValue >> (64 - (int)GetBitWidth());
    }

}

}
