all : llvmGen.dll

FSCFLAGS = -I:~FSharpPowerPack-1.9.9.9/bin/
FSC = mono ~/FSharp/bin/fsc.exe
FSLEX = mono ~/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe

llvmGen.dll : parser.dll StaticPass.fs typeDef.fs generator.fs AST.fs AST2.fs
	~jcswenso/fsc -a -o parser.dll -r:../lib/FSharp.PowerPack.dll -r:parser.dll AST2.fs typeDef.fs StaticPass.fs generator.fs
          
Run.exe : llvmGen.dll Run.exe
	${FSC} ${FSCFLAGS} -r:FParsec.dll AST.fs ParseAST.fs AST2.fs typeDef.fs StaticPass.fs
