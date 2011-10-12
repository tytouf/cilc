
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
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

using LLVM;

namespace Cilc {

public class Cilc
{
    LLVM.Module      _llvmMod;
    LLVM.Builder     _builder;
    ModuleDefinition _mod;
    
    public Cilc(LLVM.Module module, string filename)
    {
        _llvmMod = module;
        _mod     = ModuleDefinition.ReadModule(filename);
        _builder = new LLVM.Builder();
      Cil2Llvm.Init(module, _builder);
    }

    public LLVM.Module Module { get { return _llvmMod; } }
    public LLVM.Builder Builder { get { return _builder; } }
    
    public void EmitTypes()
    {
        foreach (TypeDefinition type in _mod.Types) {
          if (type.FullName == "<Module>") {
            continue;
          }
            Console.WriteLine(type.FullName);
    
            Cil2Llvm.EmitType(type);
          EmitTypeMethods(type);
        }
    }

    public void EmitTypeMethods(TypeDefinition type)
    {
        foreach (MethodDefinition method in type.Methods) {
          if (type.FullName == "<Module>") {
            continue;
          }
            Console.WriteLine(method.FullName);
            Cil2Llvm.EmitDecl(method);
            Cil2Llvm.EmitBody(method);
        }
    }

    static int Main(string[] args)
    {
      if (args.Length < 2) {
          Console.WriteLine("Usage: cilc.exe file.exe file.ll");
          return 1;
      }
        Module module = new Module(args[0]);
        Cilc cilc = new Cilc(module, args[0]);
    
        // target datalayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32"
        // target triple = "i386-pc-linux-gnu"
        module.DataLayout = "e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32";
        module.TargetTriple = "i386-pc-linux-gnu";

        CLR.Initialize(module);
    
        cilc.EmitTypes();
    
        module.Dump();
      module.WriteToFile(args[1]);
        string msg;
        LLVM.Analysis.VerifyModule(module, LLVM.VerifierFailureAction.PrintMessageAction, out msg);
        Console.WriteLine(msg);
        return 0;
    }
}

} // end of namespace Cilc
