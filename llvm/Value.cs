
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

    public static Value GetConstantInt(Type ty, Int64 val)
    {
	return new Value(LLVM.ConstInt(ty, val, true));
    }
/*
    private static Value getType(IntPtr ptr)
    {
	if (_types.ContainsKey(ptr)) {
	    return _types[ptr];
	} else {
	    Type ty = new Type(ptr);
	    _types[ptr] = ty;
	    return ty;
	}
    }
    */
}

}
