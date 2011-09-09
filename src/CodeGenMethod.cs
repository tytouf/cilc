

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Cilc {

sealed class MethodData {
    LLVM.Module        _module;
    LLVM.FunctionType  _type;
    LLVM.Function      _function;
    MethodReference    _method;

    public MethodData(MethodReference method, LLVM.Module module)
    {
        _method = method;
        _module = module;
    }

    public LLVM.FunctionType Type
    {
	get
	{
	    if (_type == null) {
		_type = ConstructType();
	    }
	    return _type;
	}
    }

    public LLVM.Function Function
    {
        get
        {
            if (_function == null) {
                _function = new LLVM.Function(_module, _method.FullName,
                                              this.Type);
            }
            return _function;
        }
    }

    private LLVM.FunctionType ConstructType()
    {
        LLVM.Type retTy = Cil2Llvm.GetType(_method.ReturnType).Type;
        List<LLVM.Type> paramsTy = new List<LLVM.Type>();
        
        if (_method.HasThis) {
            paramsTy.Add(Cil2Llvm.GetType(_method.DeclaringType).Type);
        }

	if (_method.HasParameters) {
	    foreach (ParameterDefinition p in _method.Parameters) {
		paramsTy.Add(Cil2Llvm.GetType(p.ParameterType).Type);
	    }
        }

        // TODO false -> varArg
        return LLVM.FunctionType.Get(retTy, paramsTy.ToArray(), false);
    }
} // end of class MethodData

} // end of namespace Cilc
