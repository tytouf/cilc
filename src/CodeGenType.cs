
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace Cilc {

sealed class CodeGenType
{
    bool             _constructed;
    LLVM.OpaqueType  _opaque;
    LLVM.TypeHandle  _handle;
    TypeReference    _typeRef;
    Dictionary<FieldDefinition, uint> _fieldsOffsets;

    public CodeGenType(TypeReference tr)
    {
        _fieldsOffsets = new Dictionary<FieldDefinition, uint>();
        _constructed   = false;
        _opaque   = LLVM.OpaqueType.Get(); //type opaque
        _handle   = new LLVM.TypeHandle(_opaque);
        _typeRef  = tr;
    }

    public LLVM.Type Type
    {
        get
        {
            if (!_constructed) {
                LLVM.Type type = ConstructType(_typeRef);
                _opaque.RefineAbstractTypeTo(type);
            }
            return _handle.Resolve();
        }
    }

    private LLVM.Type ConstructType(TypeReference type)
    {
        _constructed = true;
        LLVM.Type retTy = null;

        switch(type.MetadataType) {
            case MetadataType.Void:
                retTy = CLR.Void;
                break;
            case MetadataType.Boolean:
                retTy = CLR.Bool;
                break;
            case MetadataType.Char:
                retTy = CLR.Char;
                break;
            case MetadataType.SByte:
                retTy = CLR.Int8;
                break;
            case MetadataType.Byte:
                retTy = CLR.Int8;
                break;
            case MetadataType.Int16:
                retTy = CLR.Int16;
                break;
            case MetadataType.Int32:
                retTy = CLR.Int32;
                break;
            case MetadataType.String:
                retTy = CLR.String;
                break;
            case MetadataType.Object:
                retTy = CLR.Object;
                break;
            case MetadataType.ValueType:
                retTy = ConstructValueType(type);
                break;
            case MetadataType.Class:
                retTy = ConstructClassType(type);
                break;
            default:
                Console.WriteLine(">> not primitive {0}, {1}", type.MetadataType, type.IsDefinition);
                break;
        }
    
        return retTy;
    }

    private LLVM.Type ConstructValueType(TypeReference type)
    {
        if (type.IsDefinition) {
            TypeDefinition td = (TypeDefinition) type;
            List<LLVM.Type> l = new List<LLVM.Type>();
            uint offset = 0;
            foreach (FieldDefinition f in td.Fields) {
                if (f.IsStatic) {
                    continue;
                }
                LLVM.Type ty = Cil2Llvm.GetType(f.FieldType);
                if (!f.FieldType.IsPrimitive) {
                    ty = ty.GetPointerTo();
                }
                l.Add(ty);
                _fieldsOffsets[f] = offset;
                offset++;

            }
            return LLVM.StructType.Get(l.ToArray(), false);
        } else {
            Trace.Assert(false, "type is a TypeReference");
        }
        return null;
    }

    private LLVM.Type ConstructClassType(TypeReference type)
    {
        if (type.IsDefinition) {
            TypeDefinition td = (TypeDefinition) type;
            List<LLVM.Type> l = new List<LLVM.Type>();
            TypeReference baseType = td.BaseType;
            if (baseType != null) {
                l.Add(Cil2Llvm.GetType(baseType));
            } else {
                l.Add(CLR.Object);
            }
            uint offset = 1;
            foreach (FieldDefinition f in td.Fields) {
                if (f.IsStatic) {
                    continue;
                }
                LLVM.Type ty = Cil2Llvm.GetType(f.FieldType);
                if (!f.FieldType.IsPrimitive) {
                    ty = ty.GetPointerTo();
                }
                l.Add(ty);
                _fieldsOffsets[f] = offset;
                offset++;
            }
            return LLVM.StructType.Get(l.ToArray(), false);
        } else {
            Trace.Assert(false, "type is a TypeReference");
        }
        return null;
    }

} // end of class CodeGenType

} // end of namespace cilc
