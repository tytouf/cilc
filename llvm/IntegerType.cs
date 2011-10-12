
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

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
