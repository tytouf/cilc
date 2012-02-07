
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public class Module : RefBase {
    Context _ctx;

    public Module(string name) : base(LLVM.ModuleCreateWithName(name))
    {
        _ctx = null;
    }
/*
    public Module(string name, Context ctx) : base(LLVM.ModuleCreateWithNameInContext(name, ctx))
    {
    }
*/
    public Context Context
    {
        get
        {
            if (_ctx == null) {
                _ctx = new Context(LLVM.GetModuleContext(this));
            }
            return _ctx;
        }
    }

    public string DataLayout
    {
        get { return LLVM.GetDataLayout(this); }
        set { LLVM.SetDataLayout(this, value); }
    }

    public string TargetTriple
    {
        get { return LLVM.GetTarget(this); }
        set { LLVM.SetTarget(this, value); }
    }

    public int WriteToFile(string filename)
    {
        return LLVM.WriteBitcodeToFile(this.getRef(), filename);
    }

    public void Dump()
    {
        LLVM.DumpModule(this);
    }
}

}
