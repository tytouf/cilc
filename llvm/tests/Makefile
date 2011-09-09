
LIBS = -r:Mono.Cecil.dll
MODS = -addmodule:../llvm.dll

%.exe: %.cs
	echo $@
	gmcs -debug -lib:../lib $(MODS) $(LIBS) -out:$@ $<

all: module.exe type.exe
