
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

    public MethodBuilder(LLVM.Builder builder, MethodData method)
    {
        _builder   = builder;
        _method    = method;
        _stack     = new Stack<LLVM.Value>();
        _params    = new List<LLVM.Value>();
        _variables = new List<LLVM.Value>();
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

        foreach(Instruction inst in body.Instructions) {
            Console.WriteLine("{0}", inst);

//TODO: pre-select instruction by type (flowcontrol, ...)

            if (inst.OpCode == OpCodes.Nop) /* Nothing */;
            else if (inst.OpCode == OpCodes.Break) /* TODO */;
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
            else if (inst.OpCode == OpCodes.Ldnull) /* TODO */;
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
            else if (inst.OpCode == OpCodes.Dup) /* TODO */;
            else if (inst.OpCode == OpCodes.Pop) /* TODO */;
            else if (inst.OpCode == OpCodes.Ret) EmitOpCodeRet();
#if TOTO
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
/*
            case 0x5D:  // rem
            case 0x5E:  // rem.un
            case 0x5F:  // and
            case 0x60:  // or
            case 0x61:  // xor
            case 0x62:  // shl
            case 0x63:  // shr
            case 0x64:  // shr.un
            case 0x65:  // neg
            case 0x66:  // not
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
            else if (inst.OpCode == OpCodes.Newobj) EmitOpCodeNewobj(inst.Operand as MethodReference);
#endif
            else if (inst.OpCode == OpCodes.Ldc_I4_S) EmitOpCodeLdc(4, Convert.ToInt64(inst.Operand));
            else if (inst.OpCode == OpCodes.Ldc_I4) EmitOpCodeLdc(4, Convert.ToInt64(inst.Operand));
        }

    }
#if TOTO
    internal void EmitOpCodeNewobj(MethodReference method)
    {
        Trace.Assert(method != null);
        //TODO Trace.Assert(_params,Length > n);
        TypeReference type = method.DeclaringType;
        Trace.Assert(type != null); // cannot be null, we are creating an obj

        LLVM.Type ty       = CodeGenType.GetType(type);
        LLVM.Value[] args  = { ty.getSize() };
        Value newobj       = _builder.Call(CLR.Newobj, args, "newobj");
newobj.dump();
        Value obj          = _builder.Convert(newobj, ty.getPointer());
obj.dump();

        _stack.Push(obj);
/*
    // Cast to object type, call ctor and push on the stack
    //
    convertValue(newobj, PointerType::get(ClassTy, 0), bb);
    callMethod(newobj, token, bb);
    _Stack.push_back(newobj);
*/
    }

    internal void EmitOpCodeCall(MethodReference method)
    {
        Trace.Assert(method != null);
        //TODO Trace.Assert(_params,Length > n);
        TypeReference type = method.DeclaringType;
        //LLVM.Type ty       = CodeGenType.GetType(type);

        LLVM.Value[] args;
        if (method.HasParameters) {
            int count = method.Parameters.Count;
            args = new LLVM.Value[count];
            for (int i = count - 1; i >= 0; i--) {
                args[i] = _stack.Pop();
            }
        }
        //TODO FIXME
        /*
        Value newobj       = _builder.Call(CLR.Newobj, args, "call");
        Value obj          = _builder.Convert(newobj, ty.getPointer());
obj.dump();
        _stack.Push(obj);
*/

/*
    // Cast to object type, call ctor and push on the stack
    //
    convertValue(newobj, PointerType::get(ClassTy, 0), bb);
    callMethod(newobj, token, bb);
    _Stack.push_back(newobj);
*/
    }
#endif

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

#if TOTO

    internal void EmitOpCodeDiv()
    {
        // Debug.Assert(_stack.Length >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.SDivInst(A, B, "div"));
    }

    internal void EmitOpCodeDivUn()
    {
        // Debug.Assert(_stack.Length >= 2);
        LLVM.Value B = (LLVM.Value)_stack.Pop();
        LLVM.Value A = (LLVM.Value)_stack.Pop();
        // TODO: check types + pointer arith
        _stack.Push(_builder.UDivInst(A, B, "div"));
    }

#endif
    private void EmitOpCodeRet()
    {
        LLVM.Type retTy = _method.Function.GetReturnType();

	System.Console.WriteLine("EmitRet {0}", _stack.Count);
        if (retTy.Equals(CLR.Void)) {
	    System.Console.WriteLine("void");
	    Trace.Assert(_stack.Count == 0);
            _builder.CreateRetVoid();
        } else {
	    System.Console.WriteLine("ret 1");
            // TODO.FIXME must deref return Value
	    Trace.Assert(_stack.Count >= 1);
            LLVM.Value ret = _stack.Pop();
            _builder.CreateRet(ret);
        }
    }
} // end of MethodBuilder

} // end of namespace Cilc
