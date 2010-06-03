mono ~clements/FSharp-2.0.0.0/bin/fsc.exe -a -o llvmGen.dll  -r:parser.dll AST2.fs typeDef.fs StaticPass.fs generator.fs
mono ~clements/FSharp-2.0.0.0/bin/fsc.exe -o Run.exe -r:parser.dll typeDef.fs AST2.fs StaticPass.fs generator.fs Run.fs
mono Run.exe > generated.s

llvm-as -f generated.s
llvm-gcc -emit-llvm -c main.c
llvm-ld -o generated.exe generated.s.bc main.o
./generated.exe

rm generated.s.bc generated.exe.bc generated.exe main.o 