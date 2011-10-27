
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

namespace Cilc {

public class MethodBuilder
{
    LLVM.Builder       _builder;
    MethodData         _method;

    List<LLVM.Value>   _variables;
    Stack<LLVM.Value>  _stack;
    List<LLVM.Value>   _params;
    Dictionary<int, LLVM.BasicBlock> _labels;

    public MethodBuilder(LLVM.Builder builder, MethodData method)
    {
        _builder   = builder;
        _method    = method;
        _stack     = new Stack<LLVM.Value>();
        _params    = new List<LLVM.Value>();
        _variables = new List<LLVM.Value>();
        _labels    = new Dictionary<int, LLVM.BasicBlock>();
    }

    public void EmitBody()
    {
        uint hasThis = 0;
        MethodDefinition meth = _method.Method as MethodDefinition;
        LLVM.Function func = _method.Function;

        // Process parameters
        //
        if (meth.HasThis) {
            LLVM.Value val = func.GetParam(0);
            val.Name = "this";
            _params.Add(val);
            hasThis = 1;
        }
        for (uint i = 0 ; i < meth.Parameters.Count ; i++) {
            LLVM.Value val = func.GetParam(i + hasThis);
            val.Name = meth.Parameters[(int)i].Name;
            _params.Add(val);
        }

        MethodBody body = meth.Body;

        if (body == null) {
            // Method may not have a body. For instance in the case of
            // external methods
            return; // end here
        }
        LLVM.BasicBlock bb = new LLVM.BasicBlock(func, "IL_0000");
        _builder.PositionAtEnd(bb);

        // Process local variables
        //
        if (body.HasVariables) {
            _variables = new List<LLVM.Value>();
            foreach(VariableDefinition variable in body.Variables) {
                LLVM.Type  ty  = Cil2Llvm.GetType(variable.VariableType);

                // FIXME: check when we want to use pointers and when we
                // want to use primitive type. Do we want to use struct
                // for ValueTypes ?
                //
                if (!variable.VariableType.IsPrimitive &&
                    !variable.VariableType.IsValueType) {
                    ty = ty.GetPointerTo();
                }
            string name = "V_";
            if (variable.Name.Length != 0) {
                name = variable.Name;
            }
                LLVM.Value val = _builder.CreateAlloca(ty, name);
                _variables.Add(val);
            }
        }

        // parse all flowcontrol instructions a first time to create
        // basic blocks with the labels these instructions point to.
        //
        List<int> labels = new List<int>();
        foreach(Instruction inst in body.Instructions) {
            if (inst.OpCode.OperandType == OperandType.InlineBrTarget ||
                inst.OpCode.OperandType == OperandType.ShortInlineBrTarget) {
                if (inst.OpCode.FlowControl == FlowControl.Cond_Branch) {
                    labels.Add(inst.Next.Offset);
                }
                Instruction br = inst.Operand as Instruction;
                labels.Add(br.Offset);
            }
        }
        labels.Sort();

        foreach(int offset in labels) {
            LLVM.BasicBlock lbl = func.AppendBasicBlock("IL_" + offset.ToString("x4"));
            _labels[offset] = lbl;
        }

        var lEnum = labels.GetEnumerator();
        bool hasLabels = lEnum.MoveNext();

        foreach(Instruction inst in body.Instructions) {
            Console.WriteLine("{0}", inst);

            if (hasLabels && lEnum.Current == inst.Offset) {
                LLVM.BasicBlock lbl = _labels[inst.Offset];
                Console.WriteLine("offset " + inst.Offset);
                if (!bb.HasTerminator) {
                    _builder.CreateBr(lbl);
                }
                _builder.PositionAtEnd(lbl);
                bb = lbl;
                while (lEnum.Current == inst.Offset
                       && (hasLabels = lEnum.MoveNext())) { }
            }

//TODO: pre-select instruction by type (flowcontrol, ...)

            if (inst.OpCode == OpCodes.Nop) { } /* Nothing */
            else if (inst.OpCode == OpCodes.Break) EmitUnimplemented();
            else if (inst.OpCode == OpCodes.Ldarg_0) EmitOpCodeLdarg(0);
            else if (inst.OpCode == OpCodes.Ldarg_1) EmitOpCodeLdarg(1);
            else if (inst.OpCode == OpCodes.Ldarg_2) EmitOpCodeLdarg(2);
            else if (inst.OpCode == OpCodes.Ldarg_3) EmitOpCodeLdarg(3);
            else if (inst.OpCode == OpCodes.Ldloc_0) EmitOpCodeLdloc(0);
            else if (inst.OpCode == OpCodes.Ldloc_1) EmitOpCodeLdloc(1);
            else if (inst.OpCode == OpCodes.Ldloc_2) EmitOpCodeLdloc(2);
            else if (inst.OpCode == OpCodes.Ldloc_3) EmitOpCodeLdloc(3);
            else if (inst.OpCode == OpCodes.Stloc_0) EmitOpCodeStloc(0);
            else if (inst.OpCode == OpCodes.Stloc_1) EmitOpCodeStloc(1);
            else if (inst.OpCode == OpCodes.Stloc_2) EmitOpCodeStloc(2);
            else if (inst.OpCode == OpCodes.Stloc_3) EmitOpCodeStloc(3);
            else if (inst.OpCode == OpCodes.Ldnull) EmitUnimplemented();
            else if (inst.OpCode == OpCodes.Ldc_I4_M1) EmitOpCodeLdc(4, Convert.ToInt64(-1));
            else if (inst.OpCode == OpCodes.Ldc_I4_0) EmitOpCodeLdc(4, 0);
            else if (inst.OpCode == OpCodes.Ldc_I4_1) EmitOpCodeLdc(4, 1);
            else if (inst.OpCode == OpCodes.Ldc_I4_2) EmitOpCodeLdc(4, 2);
            else if (inst.OpCode == OpCodes.Ldc_I4_3) EmitOpCodeLdc(4, 3);
            else if (inst.OpCode == OpCodes.Ldc_I4_4) EmitOpCodeLdc(4, 4);
            else if (inst.OpCode == OpCodes.Ldc_I4_5) EmitOpCodeLdc(4, 5);
            else if (inst.OpCode == OpCodes.Ldc_I4_6) EmitOpCodeLdc(4, 6);
            else if (inst.OpCode == OpCodes.Ldc_I4_7) EmitOpCodeLdc(4, 7);
            else if (inst.OpCode == OpCodes.Ldc_I4_8) EmitOpCodeLdc(4, 8);
            else if (inst.OpCode == OpCodes.Dup) EmitOpCodeDup();
            else if (inst.OpCode == OpCodes.Pop) EmitOpCodePop();
            else if (inst.OpCode == OpCodes.Ret) EmitOpCodeRet();
/*
            case 0x46:  // ldind.i1
            case 0x47:  // ldind.u1
            case 0x48:  // ldind.i2
            case 0x49:  // ldind.u2
            case 0x4A:  // ldind.i4
            case 0x4B:  // ldind.u4
            case 0x4C:  // ldind.i8,u8
            case 0x4D:  // ldind.i
            case 0x4E:  // ldind.r4
            case 0x4F:  // ldind.r8
            case 0x50:  // ldind.ref
            case 0x51:  // stind.ref
            case 0x52:  // stind.i1
            case 0x53:  // stind.i2
            case 0x54:  // stind.i4
            case 0x55:  // stind.i8
            case 0x56:  // stind.r4
            case 0x57:  // stind.r8
*/
            else if (inst.OpCode == OpCodes.Call) EmitOpCodeCall(inst.Operand as MethodReference);

            else if (inst.OpCode == OpCodes.Add) EmitOpCodeAdd();
            else if (inst.OpCode == OpCodes.Sub) EmitOpCodeSub();
            else if (inst.OpCode == OpCodes.Mul) EmitOpCodeMul();
            else if (inst.OpCode == OpCodes.Div) EmitOpCodeDiv();
            else if (inst.OpCode == OpCodes.Div_Un) EmitOpCodeDivUn();

            else if (inst.OpCode == OpCodes.And) EmitOpCodeAnd();
            else if (inst.OpCode == OpCodes.Or) EmitOpCodeOr();
            else if (inst.OpCode == OpCodes.Xor) EmitOpCodeXor();
            else if (inst.OpCode == OpCodes.Not) EmitOpCodeNot();

            else if (inst.OpCode == OpCodes.Ldfld) EmitOpCodeLdfld(inst.Operand as FieldReference);
            else if (inst.OpCode == OpCodes.Stfld) EmitOpCodeStfld(inst.Operand as FieldReference);

            else if (inst.OpCode == OpCodes.Bge) EmitOpCodeBge(inst);
            else if (inst.OpCode == OpCodes.Ble) EmitOpCodeBle(inst);
#if UNIMPLEMENTED
/*
            case 0x5D:  // rem
            case 0x5E:  // rem.un
            case 0x62:  // shl
            case 0x63:  // shr
            case 0x64:  // shr.un
            case 0x65:  // neg
            case 0x67:  // conv.i1
            case 0x68:  // conv.i2
            case 0x69:  // conv.i4
            case 0x6A:  // conv.i8
            case 0x6B:  // conv.r4
            case 0x6C:  // conv.r8
            case 0x6D:  // conv.u4
            case 0x6E:  // conv.u8
            case 0x76:  // conv.r.un
            case 0x82:  // conv.ovf.i1.un
            case 0x83:  // conv.ovfi.i2.un
            case 0x84:  // conv.ovf.i4.un
            case 0x85:  // conv.ovf.i8.un
            case 0x86:  // conv.ovf.u1.un
            case 0x87:  // conv.ovf.u2.un
            case 0x88:  // conv.ovf.u4.un
            case 0x89:  // conv.ovf.u8.un
            case 0x8A:  // conv.ovf.i.un
            case 0x8B:  // conv.ovf.u.un
            case 0xB3:  // conv.ovf.i1
            case 0xB4:  // conv.ovf.u1
            case 0xB5:  // conv.ovfi.i2
            case 0xB6:  // conv.ovf.u2
            case 0xB7:  // conv.ovf.i4
            case 0xB8:  // conv.ovf.u4
            case 0xB9:  // conv.ovf.i8
            case 0xBA:  // conv.ovf.u8
            case 0xC3:  // ckfinite
            case 0xD1:  // conv.u2
            case 0xD2:  // conv.u1
            case 0xD3:  // conv.i (native int)
            case 0xD4:  // conv.ovf.i
            case 0xD5:  // conv.ovf.u
            case 0xD6:  // add.ovf
            case 0xD7:  // add.ovf.un
            case 0xD8:  // mul.ovf
            case 0xD9:  // mul.ovf.un
            case 0xDA:  // sub.ovf
            case 0xDB:  // sub.ovf.un
            case 0xDC:  // endfault, endfinally
            case 0xDD:  // leave
            case 0xDE:  // leave.s
            case 0xDF:  // stind.i
            case 0xE0:  // conv.u (native int)

*/
#endif
            else if (inst.OpCode == OpCodes.Newobj) EmitOpCodeNewobj(inst.Operand as MethodReference);
            else if (inst.OpCode == OpCodes.Ldc_I4_S) EmitOpCodeLdc(4, Convert.ToInt64(inst.Operand));
            else if (inst.OpCode == OpCodes.Ldc_I4) EmitOpCodeLdc(4, Convert.ToInt64(inst.Operand));
            else EmitUnimplemented();
        }

    }

    private void EmitUnimplemented()
    {
        Console.WriteLine("Opcode is unimplemented.");
    }

    private LLVM.Value ConvertToType(LLVM.Value v, LLVM.Type toType)
    {
        LLVM.Type vType = v.Type;
        if (vType.isPointer() && toType.isPointer()) {
            return ConvertPointer(v, toType);
        }
        Console.WriteLine("v was not converted to toType");
        return v; // TODO, FIXME
    }

    private LLVM.Value ConvertPointer(LLVM.Value v, LLVM.Type toType)
    {
        Trace.Assert(v.Type.isPointer() && toType.isPointer());
        return _builder.CreateBitCast(v, toType);
    }

    private LLVM.Value ConvertInteger(LLVM.Value v, LLVM.Type toType)
    {
        Trace.Assert(v.Type.isInteger() && toType.isInteger());
        return _builder.CreateIntCast(v, toType); // signed cast
    }

    private void EmitOpCodeNewobj(MethodReference method)
    {
        Trace.Assert(method != null);
        TypeReference type = method.DeclaringType;
        Trace.Assert(type != null); // cannot be null, we are creating an obj

        LLVM.Type ty      = Cil2Llvm.GetType(type);
        LLVM.Value size   = ConvertInteger(ty.Size, CLR.Native);
        LLVM.Value newobj = _builder.CreateCall(CLR.Newobj, size, "newobj");
newobj.Dump();
        LLVM.Value obj    = ConvertPointer(newobj, ty.GetPointerTo());
//obj.dump();

        _stack.Push(obj);
        //TODO: Trace.Assert(method.ReturnType == Void);
        EmitOpCodeCall(method);
        _stack.Push(obj);
/*
    // Cast to object type, call ctor and push on the stack
    //
    convertValue(newobj, PointerType::get(ClassTy, 0), bb);
    callMethod(newobj, token, bb);
    _Stack.push_back(newobj);
*/
    }

    private void EmitOpCodeLdfld(FieldReference field)
    {
        Trace.Assert(field != null);
        Trace.Assert(_stack.Count >= 1);
        Trace.Assert(field.IsDefinition);

        CodeGenType ty = Cil2Llvm.GetCodeGenType(field.DeclaringType);
        FieldDefinition f = field as FieldDefinition;

        uint offset = ty.GetFieldOffset(f);
        LLVM.Value obj = _stack.Pop();
        Trace.Assert(obj.Type == ty.Type.GetPointerTo());
        LLVM.Value ptr = _builder.CreateStructGEP(obj, offset, field.Name + " pointer");
        LLVM.Value fld = _builder.CreateLoad(ptr, "ldfld");
        _stack.Push(fld);
    }

    private void EmitOpCodeStfld(FieldReference field)
    {
        Trace.Assert(field != null);
        Trace.Assert(_stack.Count >= 2);
        Trace.Assert(field.IsDefinition);

        CodeGenType ty = Cil2Llvm.GetCodeGenType(field.DeclaringType);
        FieldDefinition f = field as FieldDefinition;

        uint offset = ty.GetFieldOffset(f);
        LLVM.Value val = _stack.Pop();
        LLVM.Value obj = _stack.Pop();
        LLVM.Value ptr = _builder.CreateStructGEP(obj, offset, field.Name + " pointer");
        _builder.CreateStore(val, ptr);
    }

    private void EmitOpCodeCall(MethodReference method)
    {
        Trace.Assert(method != null);

        List<LLVM.Type> argsTy = new List<LLVM.Type>();

        if (method.HasThis) {
            argsTy.Add(Cil2Llvm.GetType(method.DeclaringType).GetPointerTo());
        }

        if (method.HasParameters) {
            foreach(ParameterDefinition p in method.Parameters) {
                argsTy.Add(Cil2Llvm.GetType(p.ParameterType));
            }
        }

        Trace.Assert(_stack.Count >= argsTy.Count);
        LLVM.Value[] args = new LLVM.Value[argsTy.Count];

        for (int i = argsTy.Count - 1; i >= 0; i--) {
            args[i] = ConvertToType(_stack.Pop(), argsTy[i]);

        }

        LLVM.Value ret = _builder.CreateCall(Cil2Llvm.GetMethod(method), args);
        LLVM.Type retTy = Cil2Llvm.GetType(method.ReturnType);

        if (!retTy.Equals(CLR.Void)) {
            _stack.Push(ret);
        }
/*
    // Cast to object type, call ctor and push on the stack
    //
    convertValue(newobj, PointerType::get(ClassTy, 0), bb);
    callMethod(newobj, token, bb);
    _Stack.push_back(newobj);
*/
    }

    private void EmitOpCodeLdarg(uint n)
    {
        Trace.Assert(_params.Count >= n);
        _stack.Push(_params[(int)n]);
    }

    private void EmitOpCodeLdloc(uint n)
    {
        Trace.Assert(_variables.Count >= n);
        _stack.Push(_builder.CreateLoad(_variables[(int)n], "ldloc_"+n+"_"));
    }

    private void EmitOpCodeStloc(uint n)
    {
        Trace.Assert(_variables.Count >= n);
        Trace.Assert(_stack.Count > 0);
        _builder.CreateStore(_stack.Pop(), _variables[(int)n]);
    }

    private void EmitOpCodeLdc(uint n, Int64 constant)
    {
        LLVM.Type ty;
        switch(n) {
            case 1:
                ty = CLR.Int8;
                break;
            case 2:
                ty = CLR.Int16;
                break;
            case 4:
                ty = CLR.Int32;
                break;
            default:
                Trace.Assert(false);
                ty = CLR.Void;
                break;
        }
        _stack.Push(LLVM.Value.GetConstantInt(ty, constant));
    }

    private void EmitOpCodeAdd()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = _stack.Pop();
        LLVM.Value A = _stack.Pop();
        _stack.Push(_builder.CreateAdd(A, B, "add"));
    }

    private void EmitOpCodeSub()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = _stack.Pop();
        LLVM.Value A = _stack.Pop();
        _stack.Push(_builder.CreateSub(A, B, "sub"));
    }

    private void EmitOpCodeMul()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = _stack.Pop();
        LLVM.Value A = _stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateMul(A, B, "mul"));
    }

    private void EmitOpCodeDiv()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateSDivInst(A, B, "div"));
    }

    private void EmitOpCodeDivUn()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateUDivInst(A, B, "div"));
    }

    private void EmitOpCodeAnd()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateAnd(A, B, "and"));
    }

    private void EmitOpCodeOr()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateOr(A, B, "or"));
    }

    private void EmitOpCodeXor()
    {
        Trace.Assert(_stack.Count >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateXor(A, B, "xor"));
    }

    private void EmitOpCodeNot()
    {
        Trace.Assert(_stack.Count >= 1);
        LLVM.Value V = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.CreateNot(V, "not"));
    }

    private void EmitOpCodeDup()
    {
        Trace.Assert(_stack.Count >= 1);
        LLVM.Value V = _stack.Peek();
        _stack.Push(V);
    }

    private void EmitOpCodePop()
    {
        Trace.Assert(_stack.Count >= 1);
        _stack.Pop();
    }

    private void EmitOpCodeRet()
    {
        LLVM.Type retTy = _method.Function.GetReturnType();

        if (retTy.Equals(CLR.Void)) {
            Trace.Assert(_stack.Count == 0);
            _builder.CreateRetVoid();
        } else {
            // TODO.FIXME must deref return Value
            Trace.Assert(_stack.Count >= 1);
            LLVM.Value ret = _stack.Pop();
            _builder.CreateRet(ConvertToType(ret, retTy));
        }
    }

#region Branch Opcodes
    private void EmitOpCodeBge(Instruction inst)
    {
        // left >= right
        LLVM.Value right = _stack.Pop();
        LLVM.Value left  = _stack.Pop();
        LLVM.Value cond  = _builder.CreateICmpSGE(left, right, "bge");
        LLVM.BasicBlock bb2 = _labels[inst.Next.Offset];
        Instruction jumpTo = inst.Operand as Instruction;
        LLVM.BasicBlock bb1 = _labels[jumpTo.Offset];
        System.Console.WriteLine("then " + inst.Next.Offset + " else " + jumpTo.Offset);
        _builder.CreateCondBr(cond, bb1, bb2);
    }

    private void EmitOpCodeBle(Instruction inst)
    {
        // left >= right
        LLVM.Value right = _stack.Pop();
        LLVM.Value left  = _stack.Pop();
        LLVM.Value cond  = _builder.CreateICmpSLE(left, right, "ble");
        LLVM.BasicBlock bb2 = _labels[inst.Next.Offset];
        Instruction jumpTo = inst.Operand as Instruction;
        LLVM.BasicBlock bb1 = _labels[jumpTo.Offset];
        System.Console.WriteLine("then " + inst.Next.Offset + " else " + jumpTo.Offset);
        _builder.CreateCondBr(cond, bb1, bb2);
    }
#endregion

} // end of MethodBuilder

} // end of namespace Cilc
