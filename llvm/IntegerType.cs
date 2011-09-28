
using System;

namespace LLVM {

public class IntegerType: DerivedType {

    internal IntegerType(IntPtr ptr) : base(ptr)
    {
    }

    internal static IntegerType Get(IntPtr ptr)
    {
        return GetIntegerType(ptr);
    }

    public static IntegerType Get(uint bits)
    {
        return GetType(LLVM.IntType(bits)) as IntegerType;
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
