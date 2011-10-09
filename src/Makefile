
SRC_FILES = 	CLR.cs \
		Cilc.cs \
                Cil2Llvm.cs CodeGenType.cs CodeGenMethod.cs \
		MethodBuilder.cs \
		#CodeGenTypes.cs CodeGenType.cs \
		#CodeGenMethod.cs \

LIBS = -r:Mono.Cecil.dll -r:llvm.dll

DEBUG = -debug

cilc: $(SRC_FILES)
	gmcs $(DEBUG) -lib:../lib -out:cilc.exe $(SRC_FILES) $(LIBS)


