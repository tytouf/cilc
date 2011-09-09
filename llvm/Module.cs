
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public class Module : RefBase {
    public Module(string name) : base(LLVM.ModuleCreateWithName(name))
    {
    }
/*
    public Module(string name, Context ctx) : base(LLVM.ModuleCreateWithNameInContext(name, ctx))
    {
    }

    public Context Context
    {
	get
	{
	    return new Context(LLVM.GetModuleContext(this));
	}
    }
*/
    public int WriteToFile(string filename)
    {
        return LLVM.WriteBitcodeToFile(this.getRef(), filename);
    }

    public void Dump()
    {
	LLVM.DumpModule(this);
    }

    public bool AddTypeName(string name, Type type)
    {
	return LLVM.AddTypeName(this, name, type);
    }
}

}
