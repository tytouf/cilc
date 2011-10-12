
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using LLVM;
//using Mono.Cecil;

namespace LLVM {

class LLVM {
    [DllImport("libLLVM-2.9.so", EntryPoint="LLVMWriteBitcodeToFile")]
    public static extern int WriteBitcodeToFile(Intptr modRef, string path);
}

}
