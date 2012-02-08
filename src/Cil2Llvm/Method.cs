
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

namespace Cil2Llvm {

public sealed class Method {
    Module             _module;
    MethodReference    _method;
    LLVM.FunctionType  _type;
    LLVM.Function      _function;

    public Method(MethodReference method, Module module)
    {
        _method = method;
        _module = module;
        GetLlvmType();
    }

    private void GetLlvmType()
    {
        LLVM.Type retTy = _module.GetLlvmType(_method.ReturnType);
	if (_method.ReturnType.MetadataType == MetadataType.Class) {
	    retTy = retTy.GetPointerTo();
	}

        List<LLVM.Type> paramsTy = new List<LLVM.Type>();
        
        if (_method.HasThis) {
            paramsTy.Add(_module.GetLlvmType(_method.DeclaringType).GetPointerTo());
        }

        if (_method.HasParameters) {
            foreach (ParameterDefinition p in _method.Parameters) {
                LLVM.Type ty = _module.GetLlvmType(p.ParameterType);
                if (p.ParameterType.MetadataType == MetadataType.Class) {
                    ty = ty.GetPointerTo();
                }
                paramsTy.Add(ty);
            }
        }

        // TODO false -> varArg
        _type = LLVM.FunctionType.Get(retTy, paramsTy.ToArray(), false);
    }
    public LLVM.FunctionType LlvmType
    {
        get
        {
            if (_type == null) {
                GetLlvmType();
            }
            return _type;
        }
    }

    public void Emit()
    {
	_function = new LLVM.Function(_module.LlvmModule, _method.FullName,
                                      this.LlvmType);
    }

} // end of class MethodData

} // end of namespace Cilc
