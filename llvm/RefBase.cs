
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public abstract class RefBase
{
    protected IntPtr _ref;

    public override bool Equals(Object obj)
    {
	if (obj == null)
	{
	    return false;
	}
	RefBase rb = obj as RefBase;
	Console.WriteLine("{0} == {1}", _ref, rb._ref);
	return _ref == rb._ref;
    }

    public bool Equals(RefBase obj)
    {
	if (obj == null)
	{
	    return false;
	}
	Console.WriteLine("{0} == {1}", _ref, obj._ref);
	return _ref == obj._ref;
    }

    /*
    public static bool operator ==(RefBase a, RefBase b)
    {
	if (System.Object.ReferenceEquals(a, b)) {
	    return true;
	}
        if (a == null || b == null) {
	    return false;
	}
	return a._ref == b._ref;
    }

    public static bool operator !=(RefBase a, RefBase b)
    {
	return !(a == b);
    }
    */

    public override int GetHashCode()
    {
        return 0;
    }

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
