
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class FunctionType : DerivedType {

    public FunctionType(IntPtr ptr) : base(ptr)
    {
    }

    public static FunctionType Get(Type returnTy, Type[] paramsTy,
                                   bool isVarArg)
    {
        IntPtr[] parms = Array.ConvertAll(paramsTy, t => (IntPtr)t);
        return new FunctionType(LLVM.FunctionType(returnTy, parms,
                                (uint) parms.Length, isVarArg));
    }

    public static FunctionType Get(Type returnTy, bool isVarArg)
    {
        return new FunctionType(LLVM.FunctionType(returnTy, null, 0, isVarArg));
    }

}

}
