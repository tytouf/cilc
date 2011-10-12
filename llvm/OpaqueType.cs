
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class OpaqueType : DerivedType {

    internal OpaqueType(IntPtr ptr) : base(ptr)
    {
    }

    public static OpaqueType Get()
    {
        return new OpaqueType(LLVM.OpaqueType());
    }
}

}
