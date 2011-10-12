
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class GlobalVariable : Value {

    public GlobalVariable(Module mod, Type ty, string name) : base(LLVM.AddGlobal(mod, ty, name))
    {
    }

}

}
