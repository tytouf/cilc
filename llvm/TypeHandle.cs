
using System;

namespace LLVM {

public class TypeHandle : RefBase {

    public TypeHandle(Type type) : base(LLVM.CreateTypeHandle(type))
    {
    }

    public Type Resolve()
    {
        return new Type(LLVM.ResolveTypeHandle(this));
    }

}

}
