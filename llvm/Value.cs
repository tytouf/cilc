
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public class Value : RefBase {
    public Value(IntPtr ptr) : base(ptr) { }

    public Type Type
    {
        get
        {
            return Type.GetType(LLVM.TypeOf(this));
        }
    }

    public string Name
    {
        set { LLVM.SetValueName(this, value); }
        get { return LLVM.GetValueName(this); }
    }

    public void Dump()
    {  
        LLVM.DumpValue(this);
    }

    public static Value GetConstantInt(Type ty, Int64 val)
    {
        return new Value(LLVM.ConstInt(ty, val, true));
    }


}

}
