all : ParserTests.exe

LLVMGENSOURCE = AST.fs AST2.fs typeDef.fs generator.fs 

FSCFLAGS = #-I:bin/
FSC = ~jcswenso/fsc
FSLEX = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe -v
CONSOLETESTER = xunit.console.exe



ParserTests.exe : ${LLVMGENSOURCE}
	${FSC} ${FSCFLAGS} LLVMGENSOURCE
	
Run.exe : Parser.dll Run.fs
	${FSC} ${FSCFLAGS} -r:Parser.dll Run.fs

run : Run.exe
	mono Run.exe
	
test : ParserTests.exe
	mono ${CONSOLETESTER} ParserTests.exe

clean : 
	rm Lexer.fs Run.exe ParserTests.exe RealParser.fs RealParser.fsi Parser.fsi Parser.fs Parser.dll
