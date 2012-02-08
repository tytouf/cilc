
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
//using Mono.Cecil.Metadata;

namespace Cil2Llvm {

public sealed class Type
{
    Module         _module;
    LLVM.Type      _llType;
    TypeReference  _cilType;

    bool           _emitted;
    bool           _isValueType;
    bool           _isClass;
    Dictionary<FieldDefinition, uint> _fieldsOffsets;

    public Type(TypeReference tr, Module mod)
    {
        _module  = mod;
        _cilType = tr;
        _llType  = null;

        _emitted = _isValueType = _isClass = false;
        _fieldsOffsets = new Dictionary<FieldDefinition, uint>();
    }

    public void Emit()
    {
        GetLlvmType(); // Force Llvm type resolution

        _emitted = true;
        if (_isValueType) {
            EmitValueType();
        } else if (_isClass) {
            EmitClass();
        }
    }

    public uint GetFieldOffset(FieldDefinition f)
    {
        return _fieldsOffsets[f];
    }

    private void GetLlvmType()
    {
        switch(_cilType.MetadataType) {
            case MetadataType.Void:
                _llType = CLR.Void;
                break;
            case MetadataType.Boolean:
                _llType = CLR.Bool;
                break;
            case MetadataType.Char:
                _llType = CLR.Char;
                break;
            case MetadataType.SByte:
                _llType = CLR.Int8;
                break;
            case MetadataType.Byte:
                _llType = CLR.Int8;
                break;
            case MetadataType.Int16:
                _llType = CLR.Int16;
                break;
            case MetadataType.Int32:
                _llType = CLR.Int32;
                break;
            case MetadataType.String:
                _llType = CLR.String;
                break;
            case MetadataType.Object:
                _llType = CLR.Object;
                break;
            case MetadataType.ValueType:
                _isValueType = true;
                _llType = LLVM.StructType.Get(_module.LlvmModule.Context,
                                            "type " + _cilType.FullName);
                break;
            case MetadataType.Class:
                _isClass = true;
                _llType = LLVM.StructType.Get(_module.LlvmModule.Context,
                                            "type " + _cilType.FullName);
                break;
            default:
                Console.WriteLine(">> not primitive {0}, {1}", _cilType.MetadataType, _cilType.IsDefinition);
                break;
        }
    }

    public LLVM.Type LlvmType
    {
        get
        {
            if (_llType == null) {
                GetLlvmType();
            }
            return _llType;
        }
    }

    private void EmitValueType()
    {
        Console.WriteLine("emitValueType");
        if (!_cilType.IsDefinition) {
            Trace.Assert(false, "type is a TypeReference");
            return;
        }

        TypeDefinition td = (TypeDefinition) _cilType;
        List<LLVM.Type> l = new List<LLVM.Type>();
        uint offset = 0;
        foreach (FieldDefinition f in td.Fields) {
            if (f.IsStatic) {
                continue;
            }
            LLVM.Type ty = _module.GetLlvmType(f.FieldType);
            if (f.FieldType.MetadataType == MetadataType.Class) {
                ty = ty.GetPointerTo();
            }
            l.Add(ty);
            _fieldsOffsets[f] = offset;
            offset++;

        }
        LLVM.StructType sty = LlvmType as LLVM.StructType;
        sty.SetBody(l.ToArray(), false);

        EmitFields();
    }

    private void EmitClass()
    {
        Console.WriteLine("emit Class");
        if (!_cilType.IsDefinition) {
            Trace.Assert(false, "type is a TypeReference");
            return;
        }

        TypeDefinition td = (TypeDefinition) _cilType;
        List<LLVM.Type> l = new List<LLVM.Type>();

        TypeReference baseType = td.BaseType;

        if (baseType != null) {
            l.Add(_module.GetLlvmType(baseType));
        } else {
            l.Add(CLR.Object);
        }

        uint offset = 1;
        foreach (FieldDefinition f in td.Fields) {
            if (f.IsStatic) {
                continue;
            }
            LLVM.Type ty = _module.GetLlvmType(f.FieldType);
            if (f.FieldType.MetadataType == MetadataType.Class) {
                ty = ty.GetPointerTo();
            }

            l.Add(ty);
            _fieldsOffsets[f] = offset;
            offset++;
        }
        Console.WriteLine(" setbody");
        LLVM.StructType sty = LlvmType as LLVM.StructType;
        sty.SetBody(l.ToArray(), false);

        EmitFields();
    }

    private void EmitFields()
    {
        Console.WriteLine("EmitFields()");
        TypeDefinition td = (TypeDefinition) _cilType;

        if (td.HasFields) {
            foreach (FieldDefinition f in td.Fields) {
              if (f.IsStatic) {
                  EmitStaticField(f);
              }
            }
        }

        if (td.HasNestedTypes) {
            foreach (TypeDefinition t in td.NestedTypes) {
              _module.GetType(t).Emit();
            }
        }

    }

    private void EmitStaticField(FieldDefinition f)
    {
        Console.WriteLine("EmitStaticField {0}", f.FullName);
        _module.EmitGlobalVariable(f.FieldType, f.FullName);
    }

    private void EmitMethods()
    {

    }
    // Methods used to generate methods declaration and body.
    // 
/*
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

    public static void EmitBody(MethodReference method)
    {
        MethodBuilder mb = new MethodBuilder(_builder, GetMethodData(method));
        mb.EmitBody();
    }
*/


} // end of class Type

} // end of namespace Cil2Llvm
