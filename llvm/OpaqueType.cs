
using System;

namespace LLVM {

public class OpaqueType : DerivedType {

    internal OpaqueType(IntPtr ptr) : base(ptr)
    {
    }

    public static OpaqueType Get()
    {
	return new OpaqueType(LLVM.OpaqueType());
    }
}

}
