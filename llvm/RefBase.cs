
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public abstract class RefBase
{
    private IntPtr _ref;

    protected internal RefBase(IntPtr reference)
    {
	if (reference == IntPtr.Zero)
	    throw new ArgumentNullException("reference");

	_ref = reference;
    }

    public static implicit operator IntPtr(RefBase This)
    {
	return This._ref;
    }


    public IntPtr getRef()
    {
	return _ref;
    }
}

}
