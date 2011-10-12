
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class TypeHandle : RefBase {

    public TypeHandle(Type type) : base(LLVM.CreateTypeHandle(type))
    {
    }

    public Type Resolve()
    {
        return new Type(LLVM.ResolveTypeHandle(this));
    }

}

}
