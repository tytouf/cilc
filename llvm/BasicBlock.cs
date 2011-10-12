
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

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
