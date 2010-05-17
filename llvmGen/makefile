all : llvmGen.dll Run.exe

FSCFLAGS = -I:~FSharpPowerPack-1.9.9.9/bin/
FSC = mono ~/FSharp/bin/fsc.exe
FSLEX = mono ~/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe

llvmGen.dll : parser.dll StaticPass.fs typeDef.fs generator.fs AST.fs AST2.fs
	~jcswenso/fsc -a -o llvmGen.dll -r:../lib/FSharp.PowerPack.dll -r:parser.dll AST2.fs typeDef.fs StaticPass.fs generator.fs
          
Run.exe : llvmGen.dll Run.fs
	~jcswenso/fsc -o Run.exe -r:parser.dll AST2.fs typeDef.fs StaticPass.fs generator.fs  Run.fs
