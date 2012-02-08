
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Cecil;

namespace Cilc {

public class Cilc
{
    static int Main(string[] args)
    {
      if (args.Length < 2) {
          Console.WriteLine("Usage: cilc.exe file.exe file.ll");
          return 1;
      }
        string filename = args[0];

        ModuleDefinition cilModule = ModuleDefinition.ReadModule(filename);
        // target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32"
        // target triple = "i386-pc-linux-gnu"
        //module.DataLayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32";
        string target = "i386-pc-linux-gnu";

        Cil2Llvm.Module mod = new Cil2Llvm.Module(cilModule, target);
        mod.Emit();
        mod.Dump();

        //LLVM.Analysis.VerifyModule(module, LLVM.VerifierFailureAction.PrintMessageAction, out msg);
        //Console.WriteLine(msg);
        return 0;
    }
}

} // end of namespace Cilc
