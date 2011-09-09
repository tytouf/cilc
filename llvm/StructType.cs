

using System;

namespace LLVM {

public class StructType : CompositeType {

    internal StructType(IntPtr ptr) : base(ptr)
    {
    }

    public static StructType Get(Type[] types, bool packed = false)
    {
	IntPtr[] ptrs = Array.ConvertAll(types, t => (IntPtr)t);

	return new StructType(LLVM.StructType(ptrs, (uint)ptrs.Length, packed));
    }

    public uint GetNumElements()
    {
        return LLVM.CountStructElementTypes(this);
    }
/* TODO
  const Type *getElementType(unsigned N) const {
    assert(N < NumContainedTys && "Element number out of range!");              
    return ContainedTys[N];
  }
*/

}

}
