
using System;
using LLVM;

abstract class CLR {
       public static LLVM.Type Void;
       public static LLVM.Type Bool;
       public static LLVM.Type Char;
       public static LLVM.Type Int8;
       public static LLVM.Type Int16;
       public static LLVM.Type Int32;
       public static LLVM.Type Int64;
       public static LLVM.Type Native;
       public static LLVM.Type Ptr;
      
       public static LLVM.Type String;
       public static LLVM.Type Object;

       public static LLVM.Value Const_0;
       public static LLVM.Value Const_1;
       public static LLVM.Value Const_2;
       public static LLVM.Value Const_3;
       public static LLVM.Value Const_4;
       public static LLVM.Value Const_5;
       public static LLVM.Value Const_6;
       public static LLVM.Value Const_7;
       public static LLVM.Value Const_8;
       public static LLVM.Value Const_m1;

         public static LLVM.Function Newobj;
         public static LLVM.Function Newstr;
         public static LLVM.Function Newarr;
      
       public static void Initialize(LLVM.Module module)
       {
            TargetData tgt = new TargetData("e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:32:64-f32:32:32-f64:32:64-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32");

          Void = LLVM.Type.GetVoid();
          module.AddTypeName("type System.Void", Void);
          Bool = LLVM.Type.GetInt8();
          module.AddTypeName("type System.Bool", Bool);
          Char = LLVM.Type.GetInt8();
          module.AddTypeName("type System.Char", Char);
          Int8 = LLVM.Type.GetInt8();
          module.AddTypeName("type System.Int8", Int8);
          Int16 = LLVM.Type.GetInt16();
          module.AddTypeName("type System.Int16", Int16);
          Int32 = LLVM.Type.GetInt32();
          module.AddTypeName("type System.Int32", Int32);
          Int64 = LLVM.Type.GetInt64();
          module.AddTypeName("type System.Int64", Int64);

            Native = tgt.GetIntPtrType();
          module.AddTypeName("type System.Native", Native);

            Ptr = Native.GetPointerTo(0);
          module.AddTypeName("type System.Ptr", Ptr);
          
          Object = LLVM.StructType.Get(new LLVM.Type[2] { Int32, Int32 }, false);
          module.AddTypeName("type System.Object", Object);

          String = LLVM.Type.GetInt8(); // FIXME, TODO
          module.AddTypeName("type System.String", String);

          // Initialize constants
          //
          Const_0 = GetConstant(0);
          Const_1 = GetConstant(1);
          Const_2 = GetConstant(2);
          Const_3 = GetConstant(3);
          Const_4 = GetConstant(4);
          Const_5 = GetConstant(5);
          Const_6 = GetConstant(6);
          Const_7 = GetConstant(7);
          Const_8 = GetConstant(8);
          Const_m1 = GetConstant(-1);

          LLVM.FunctionType ft = LLVM.FunctionType.Get(Object.GetPointerTo(0),
                                    new LLVM.Type[1] { Native }, false);
            Newobj = new LLVM.Function(module, "newobj", ft);

          ft = LLVM.FunctionType.Get(String.GetPointerTo(0),
                               new LLVM.Type[1] { Int8.GetPointerTo(0) }, false);
            Newstr = new LLVM.Function(module, "newstring", ft);

          ft = LLVM.FunctionType.Get(Object.GetPointerTo(0),
                                  new LLVM.Type[2] { Native, Native }, false);
            Newarr = new LLVM.Function(module, "newarr", ft);
      }

      static internal LLVM.Value GetConstant(Int64 val)
      {
          return Value.GetConstantInt(Int32, val);
      }
}


