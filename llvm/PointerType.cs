
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

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
