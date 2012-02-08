
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

namespace Cil2Llvm {

public sealed class Module
{
    private LLVM.Module   _llModule;
    private LLVM.Builder  _llBuilder;
    private string        _target;

    private ModuleDefinition _cilModule;

    private Dictionary<TypeReference, Type> _types;
    //private Dictionary<MethodReference, MethodData> _methods;

    public Module(ModuleDefinition cilModule, string target)
    {
        this._cilModule = cilModule;
        this._target    = target;
        this._llModule  = new LLVM.Module(cilModule.Name);
        this._llModule.TargetTriple = target;
        CLR.Initialize(this._llModule);

        _types   = new Dictionary<TypeReference, Type>();
        //_methods = new Dictionary<MethodReference, MethodData>();
    }

    public LLVM.Module LlvmModule {
        get {
            return _llModule;
        }
    }

    public void Emit()
    {
        EmitTypes();
        EmitMethods();
    }

    public Type GetType(TypeReference type)
    {
        Type ty;

        if (!_types.TryGetValue(type, out ty)) {
            ty = new Type(type, this);
            _types[type] = ty;
        }
        return ty;
    }

    public LLVM.Type GetLlvmType(TypeReference type)
    {
        Type ty = GetType(type);
        return ty.LlvmType;
    }

    private void EmitTypes()
    {
        foreach (TypeDefinition type in _cilModule.Types) {
            if (type.FullName == "<Module>")
                continue;
            GetType(type);
        }

        foreach (TypeDefinition type in _cilModule.Types) {
            GetType(type).Emit();
        }
    }


    private void EmitMethods()
    {
        foreach (TypeDefinition type in _cilModule.Types) {
            if (type.FullName == "<Module>") {
                continue;
            }
            foreach (MethodDefinition method in type.Methods) {
                Method m = new Method(method, this);
                m.Emit();
            }
        }
    }

    public void EmitGlobalVariable(TypeReference ty, string name)
    {
        new LLVM.GlobalVariable(_llModule, GetLlvmType(ty), name);
    }

    public void Dump()
    {
        _llModule.Dump();
    }

} // end of Module

} // end of namespace Cil2Llvm
