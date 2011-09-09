
using System;

namespace LLVM {

public class Builder : RefBase {

    public Builder() : base(LLVM.CreateBuilder())
    {
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

    public Value SDivInst(Value left, Value right, string name)
    {
	return new Value(LLVM.BuildSDiv(this.getRef(), left.getRef(), right.getRef(), name));
    }

    public Value UDivInst(Value left, Value right, string name)
    {
	return new Value(LLVM.BuildUDiv(this.getRef(), left.getRef(), right.getRef(), name));
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
}

}
