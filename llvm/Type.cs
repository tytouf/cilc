
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace LLVM {

public enum TypeKind {
    Void,        /**< type with no size */
    Float,       /**< 32 bit floating point type */
    Double,      /**< 64 bit floating point type */
    X86_FP80,    /**< 80 bit floating point type (X87) */
    FP128,       /**< 128 bit floating point type (112-bit mantissa)*/
    PPC_FP128,   /**< 128 bit floating point type (two 64-bits) */
    Label,       /**< Labels */
    Integer,     /**< Arbitrary bit width integers */
    Function,    /**< Functions */
    Struct,      /**< Structures */
    Array,       /**< Arrays */
    Pointer,     /**< Pointers */
    Opaque,      /**< Opaque: type with unknown structure */
    Vector,      /**< SIMD 'packed' format, or other vector type */
    Metadata     /**< Metadata */
};

public class Type : RefBase {

    internal Type(IntPtr ptr) : base(ptr) { }
    static Dictionary<IntPtr, Type> types = new Dictionary<IntPtr, Type>();

    public static Type GetType(IntPtr ptr)
    {
	System.Console.WriteLine("GetType {0}", ptr);
	if (types.ContainsKey(ptr)) {
	    return types[ptr];
	} else {
	    return new Type(ptr);
	}
    }

    public bool Equals(Type obj)
    {
	if (obj == null)
	{
	    return false;
	}
	Console.WriteLine("{0} == {1}", _ref, obj._ref);
	return _ref == obj._ref;
    }

    public override string ToString()
    {
	return _ref.ToString();
    }

    public static IntegerType GetInt1()
    {
        return new IntegerType(LLVM.Int1Type());
    }

    public static IntegerType GetInt8()
    {
        return new IntegerType(LLVM.Int8Type());
    }

    public static IntegerType GetInt16()
    {
        return new IntegerType(LLVM.Int16Type());
    }

    public static IntegerType GetInt32()
    {
        return new IntegerType(LLVM.Int32Type());
    }

    public static IntegerType GetInt64()
    {
        return new IntegerType(LLVM.Int64Type());
    }

    public static IntegerType GetIntN(uint bits)
    {
        return new IntegerType(LLVM.IntType(bits));
    }

    public static Type GetVoid() { System.Console.WriteLine("GetVoid {0}", LLVM.VoidType()); return GetType(LLVM.VoidType()); }
    public static Type GetLabel() { return GetType(LLVM.LabelType()); }
    public static Type GetOpaque() { return GetType(LLVM.OpaqueType()); }

    public TypeKind Kind
    {
	get
	{
	    return (TypeKind)LLVM.GetTypeKind(this);
	}
    }

    public bool isInteger()
    {
	return this.Kind == TypeKind.Integer;
    }

    public bool isPointer()
    {
	return this.Kind == TypeKind.Pointer;
    }

    public bool isOpaque()
    {
	return this.Kind == TypeKind.Opaque;
    }

    public Value Zero
    {
        get
	{
	    return new Value(LLVM.GetZero(this));
        }
    }

    public Value Size
    {
        get
	{
	    return new Value(LLVM.SizeOf(this));
        }
    }

    public Value Align
    {
        get
	{
	    return new Value(LLVM.AlignOf(this));
        }
    }

    public PointerType GetPointerTo(uint addressSpace = 0)
    {
	return PointerType.Get(this, addressSpace);
    }

}

}
