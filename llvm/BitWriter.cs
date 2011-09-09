
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
