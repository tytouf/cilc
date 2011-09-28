
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Cilc {

static class Cil2Llvm
{
    // Keep a reference locally of LLVM Module and Builder instances.
    //
    static LLVM.Module   _module;
    static LLVM.Builder  _builder;

    // Dictionnary to keep track of already codegen types and methods.
    //
    static Dictionary<TypeReference, CodeGenType> _types;
    static Dictionary<MethodReference, MethodData> _methods;

    public static void Init(LLVM.Module module, LLVM.Builder builder)
    {
        if (_module == null) {
            _module  = module;
            _builder = builder;
            _types   = new Dictionary<TypeReference, CodeGenType>();
            _methods = new Dictionary<MethodReference, MethodData>();
        }
    }

#region Emit Module
    public static void EmitModule()
    {
        // TODO
    }
#endregion

#region Emit Types
    // Methods used to generate types declaration
    //
    public static CodeGenType GetCodeGenType(TypeReference type)
    {
            if (_types.ContainsKey(type)) {
            return _types[type]; // skip, type has already been emitted.
        }
        
        // Create and register type
        //
        CodeGenType td  = new CodeGenType(type);
        _types[type] = td;

        return td;
    }
    public static LLVM.Type GetType(TypeReference type)
    {
        return GetCodeGenType(type).Type;
    }

    public static void EmitType(TypeReference type)
    {
        _module.AddTypeName("type " + type.FullName, GetType(type));

        if (!type.IsDefinition) {
            return;
        }

        TypeDefinition td = (TypeDefinition) type;

        if (td.HasFields) {
            foreach (FieldDefinition f in td.Fields) {
        	if (f.IsStatic) {
        	    EmitStaticField(f);
        	}
            }
        }

        if (td.HasNestedTypes) {
            foreach (TypeDefinition t in td.NestedTypes) {
        	EmitType(t);
            }
        }

    }

    static void EmitStaticField(FieldDefinition f)
    {
        new LLVM.GlobalVariable(_module, GetType(f.FieldType), f.FullName);
    }
#endregion

#region Emit Methods
    // Methods used to generate methods declaration and body.
    // 
    public static MethodData GetMethodData(MethodReference method)
    {
        if (_methods.ContainsKey(method)) {
            return _methods[method];
        }

        MethodData md = new MethodData(method, _module);
        _methods[method] = md;

        return md;
    }

    public static LLVM.Function GetMethod(MethodReference method)
    {
        return GetMethodData(method).Function;
    }

    public static void EmitDecl(MethodReference method)
    {
        LLVM.Function func = GetMethodData(method).Function;
    }

    public static void EmitBody(MethodReference method)
    {
        MethodBuilder mb = new MethodBuilder(_builder, GetMethodData(method));
        mb.EmitBody();
    }
#endregion

} // end of Cil2Llvm

} // end of namespace Cilc
