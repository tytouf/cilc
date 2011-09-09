
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
