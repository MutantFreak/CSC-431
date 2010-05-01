all : ParserTests.exe

PARSERSOURCES = AST.fs RealParser.fsi RealParser.fs Lexer.fs

FSCFLAGS = #-I:bin/
FSC = ~jcswenso/fsc
FSLEX = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe -v
CONSOLETESTER = xunit.console.exe

Parser.dll :${PARSERSOURCES}
	${FSC} ${FSCFLAGS} -a -o Parser.dll -r:FSharp.PowerPack.dll ${PARSERSOURCES}

# use fslex to compile the lexer spec
Lexer.fs : Lexer.fsl
	${FSLEX} Lexer.fsl

# use fsyacc to compile the parser spec
# N.B.: have to use goofy tricks to get the 'module' in the front...
RealParser.fs : Parser.fsy
	${FSYACC} Parser.fsy
	echo "module Parser" > RealParser.fs
	cat Parser.fs >> RealParser.fs
	echo "module Parser" > RealParser.fsi
	cat Parser.fsi >> RealParser.fsi

RealParser.fsi : RealParser.fs


ParserTests.exe : Parser.dll ParserTests.fs
	${FSC} ${FSCFLAGS} -r:xunit.dll -r:FsTest.dll -r:Parser.dll ParserTests.fs
	
Run.exe : Parser.dll Run.fs
	${FSC} ${FSCFLAGS} -r:Parser.dll Run.fs

run : Run.exe
	mono Run.exe
	
test : ParserTests.exe
	mono ${CONSOLETESTER} ParserTests.exe

clean : 
	rm Lexer.fs Run.exe ParserTests.exe RealParser.fs RealParser.fsi Parser.fsi Parser.fs Parser.dll
