
// Copyright 2011, Christophe Augier. All rights reserved.
//
// This file is distributed under the Simplified BSD License. See LICENSE.TXT
// for details.

using System;

namespace LLVM {

public class Builder : RefBase {

    public Builder() : base(LLVM.CreateBuilder())
    {
    }

    public void PositionAtEnd(BasicBlock bb)
    {
        LLVM.PositionBuilderAtEnd(this, bb);
    }

#region Arithmetic
    public Value CreateAdd(Value left, Value right, string name = "add")
    {
        return new Value(LLVM.BuildAdd(this, left, right, name));
    }

    public Value CreateNSWAdd(Value left, Value right, string name = "nswadd")
    {
        return new Value(LLVM.BuildNSWAdd(this, left, right, name));
    }

    public Value CreateNUWAdd(Value left, Value right, string name = "nuwadd")
    {
        return new Value(LLVM.BuildNUWAdd(this, left, right, name));
    }

    public Value CreateFAdd(Value left, Value right, string name = "fadd")
    {
        return new Value(LLVM.BuildFAdd(this, left, right, name));
    }

    public Value CreateSub(Value left, Value right, string name = "sub")
    {
        return new Value(LLVM.BuildSub(this, left, right, name));
    }

    public Value CreateNSWSub(Value left, Value right, string name = "nswsub")
    {
        return new Value(LLVM.BuildNSWSub(this, left, right, name));
    }

    public Value CreateNUWSub(Value left, Value right, string name = "nuwsub")
    {
        return new Value(LLVM.BuildNUWSub(this, left, right, name));
    }

    public Value CreateFSub(Value left, Value right, string name = "fsub")
    {
        return new Value(LLVM.BuildFSub(this, left, right, name));
    }

    public Value CreateMul(Value left, Value right, string name = "mul")
    {
        return new Value(LLVM.BuildMul(this, left, right, name));
    }

    public Value CreateNSWMul(Value left, Value right, string name = "nswmul")
    {
        return new Value(LLVM.BuildNSWMul(this, left, right, name));
    }

    public Value CreateNUWMul(Value left, Value right, string name = "nuwmul")
    {
        return new Value(LLVM.BuildNUWMul(this, left, right, name));
    }

    public Value CreateFMul(Value left, Value right, string name = "fmul")
    {
        return new Value(LLVM.BuildFMul(this, left, right, name));
    }

    public Value CreateSDivInst(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildSDiv(this, left, right, name));
    }

    public Value CreateUDivInst(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildUDiv(this, left, right, name));
    }

#endregion

#region Comparison
    public Value CreateICmpEQ(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.EQ, left, right, name));
    }

    public Value CreateICmpNE(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.NE, left, right, name));
    }

    public Value CreateICmpUGT(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.UGT, left, right, name));
    }

    public Value CreateICmpUGE(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.UGE, left, right, name));
    }

    public Value CreateICmpULT(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.ULT, left, right, name));
    }

    public Value CreateICmpULE(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.ULE, left, right, name));
    }

    public Value CreateICmpSGT(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.SGT, left, right, name));
    }

    public Value CreateICmpSGE(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.SGE, left, right, name));
    }

    public Value CreateICmpSLT(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.SLT, left, right, name));
    }

    public Value CreateICmpSLE(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildICmp(this, IntPredicate.SLE, left, right, name));
    }
#endregion

#region Logic
    public Value CreateAnd(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildAnd(this, left, right, name));
    }

    public Value CreateOr(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildOr(this, left, right, name));
    }

    public Value CreateXor(Value left, Value right, string name)
    {
        return new Value(LLVM.BuildXor(this, left, right, name));
    }

    public Value CreateNot(Value v, string name)
    {
        return new Value(LLVM.BuildNot(this, v, name));
    }
#endregion

#region Memory Instructions

    public Value CreateAlloca(Type type, string name = "alloca")
    {
        return new Value(LLVM.BuildAlloca(this, type, name));
    }

    public Value CreateLoad(Value val, string name = "load")
    {
        return new Value(LLVM.BuildLoad(this, val, name));
    }

    public Value CreateStore(Value val, Value ptr)
    {
        return new Value(LLVM.BuildStore(this, val, ptr));
    }

#endregion

#region Other Instructions

    public Value CreateCall(Value callee, string name = "")
    {
        return new Value(LLVM.BuildCall(this, callee, null, 0,  name));
    }

    public Value CreateCall(Value callee, Value[] args, string name = "")
    {
        IntPtr[] argsPtr = Array.ConvertAll(args, t => (IntPtr)t);
        return new Value(LLVM.BuildCall(this, callee, argsPtr, (uint)args.Length, name));
    }

    public Value CreateCall(Value callee, Value arg1, string name = "")
    {
        Value[] args = { arg1 };
        return CreateCall(callee, args, name);
    }

    public Value CreateCall(Value callee, Value arg1, Value arg2, string name = "")
    {
        Value[] args = { arg1, arg2 };
        return CreateCall(callee, args, name);
    }

    public Value CreateCall(Value callee, Value arg1, Value arg2, Value arg3, string name = "")
    {
        Value[] args = { arg1, arg2, arg3 };
        return CreateCall(callee, args, name);
    }

    public Value CreateCall(Value callee, Value arg1, Value arg2, Value arg3, Value arg4, string name = "")
    {
        Value[] args = { arg1, arg2, arg3, arg4 };
        return CreateCall(callee, args, name);
    }

#endregion

#region Casts
    public Value CreateIntCast(Value val, Type destTy, string name = "")
    {
        return new Value(LLVM.BuildIntCast(this, val, destTy, name));
    }

    public Value CreateBitCast(Value val, Type destTy, string name = "")
    {
        return new Value(LLVM.BuildBitCast(this, val, destTy, name));
    }

#endregion

#region Struct
    public Value CreateStructGEP(Value val, uint idx, string name = "")
    {
        return new Value(LLVM.BuildStructGEP(this, val, idx, name));
    }
#endregion

    public Value CreateRet(Value val)
    {
        return new Value(LLVM.BuildRet(this, val));
    }

    public Value CreateRetVoid()
    {
        return new Value(LLVM.BuildRetVoid(this));
    }

    public Value CreateBr(BasicBlock bb)
    {
        return new Value(LLVM.BuildBr(this, bb));
    }

    public Value CreateCondBr(Value cond, BasicBlock bbtrue, BasicBlock bbfalse)
    {
        return new Value(LLVM.BuildCondBr(this, cond, bbtrue, bbfalse));
    }
}

}
