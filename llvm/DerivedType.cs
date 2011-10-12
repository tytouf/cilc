
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class DerivedType : Type {

    internal DerivedType(IntPtr ptr) : base(ptr)
    {
    }

    public void RefineAbstractTypeTo(Type concreteType)
    {
        LLVM.RefineType(this, concreteType);
    }
}

}
