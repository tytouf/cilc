
using System;

public class Test {
    static void Main()
    {
        LLVM.Module module = new LLVM.Module("test_type");

	module.AddTypeName("int1", LLVM.Type.GetInt1());
	module.AddTypeName("int8", LLVM.Type.GetInt8());
	module.AddTypeName("int16", LLVM.Type.GetInt16());
	module.AddTypeName("int32", LLVM.Type.GetInt32());
	module.AddTypeName("int64", LLVM.Type.GetInt64());
	module.AddTypeName("int24", LLVM.Type.GetIntN(24));
	module.AddTypeName("int48", LLVM.IntegerType.Get(48));

        LLVM.Type[] arr = new LLVM.Type[] {LLVM.Type.GetInt8(), LLVM.Type.GetInt8(), LLVM.Type.GetInt32()};
	module.AddTypeName("structtype", LLVM.StructType.Get(arr));

	module.AddTypeName("arraytype", LLVM.ArrayType.Get(LLVM.Type.GetInt8(), 10));

	module.AddTypeName("pointertype", LLVM.PointerType.Get(LLVM.Type.GetInt8(), 0));

	module.AddTypeName("opaquetype", LLVM.OpaqueType.Get());

        LLVM.OpaqueType o  = LLVM.OpaqueType.Get();
	LLVM.TypeHandle th = new LLVM.TypeHandle(o);
	LLVM.StructType st = LLVM.StructType.Get(new LLVM.Type[] {LLVM.Type.GetInt32(), o});
        o.RefineAbstractTypeTo(st);
	module.AddTypeName("refinedtype", th.Resolve());

	module.Dump();
    }
}
