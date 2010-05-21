all : llvmGen.dll Run.exe

llvmGen.dll : parser.dll StaticPass.fs typeDef.fs generator.fs AST.fs AST2.fs
	~jcswenso/fsc -a -o llvmGen.dll -r:../lib/FSharp.PowerPack.dll -r:parser.dll AST2.fs typeDef.fs StaticPass.fs generator.fs
          
Run.exe : llvmGen.dll Run.fs
	~jcswenso/fsc -o Run.exe -r:../lib/FSharp.PowerPack.dll -r:parser.dll typeDef.fs AST2.fs StaticPass.fs generator.fs Run.fs

run : Run.exe
	mono Run.exe

clean :
	rm llvmGen.dll Run.exe 
