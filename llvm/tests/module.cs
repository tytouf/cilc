
using System;

public class Test {
    static void Main()
    {
        LLVM.Module module = new LLVM.Module("test_module");
	module.Dump();
    }
}

