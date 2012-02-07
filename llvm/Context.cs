
// Copyright 2012, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public class Context : RefBase {
    public Context(IntPtr ptr) : base(ptr) {}
}

}
