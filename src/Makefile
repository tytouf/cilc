
SRC_FILES = 	CLR.cs \
		Cilc.cs \
                Cil2Llvm/Module.cs Cil2Llvm/Type.cs Cil2Llvm/Method.cs \
		#MethodBuilder.cs \

LIBS = -r:Mono.Cecil.dll -r:llvm.dll

DEBUG = -debug

cilc: $(SRC_FILES)
	gmcs $(DEBUG) -d:TRACE -lib:../lib -out:cilc.exe $(SRC_FILES) $(LIBS)

clean: cilc.exe
	rm cilc.exe

