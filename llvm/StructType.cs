
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Runtime.InteropServices;

namespace LLVM {

public class StructType : CompositeType {

    bool _body;

    internal StructType(IntPtr ptr) : base(ptr)
    {
        _body = false;
    }

    public static StructType Get(Context ctx, string name)
    {
        StructType ty = new StructType(LLVM.StructCreateNamed(ctx, name));
        return ty;
    }

    public string Name
    {
        get {
            IntPtr ptr  = LLVM.GetStructName(this);
            return Marshal.PtrToStringAnsi(ptr);
        }
    }

    public void SetBody(Type[] types, bool packed = false)
    {
        if (!_body) {
            IntPtr[] ptrs = Array.ConvertAll(types, t => (IntPtr)t);
            LLVM.StructSetBody(this, ptrs, (uint)ptrs.Length, packed);
            _body = true;
        } else {
            Console.WriteLine("StructType.SetBody: body already set!");
            // TODO Error body already set.
        }
    }

    public static StructType Get(Context ctx, string name, Type[] types, bool packed = false)
    {
        StructType ty = StructType.Get(ctx, name);
        ty.SetBody(types, packed);
        return ty;
    }

    public uint GetNumElements()
    {
        return LLVM.CountStructElementTypes(this);
    }
}

}
