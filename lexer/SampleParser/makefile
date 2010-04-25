all : main.exe

PARSERSOURCES = AST.fs RealParser.fsi RealParser.fs Lexer.fs

FSCFLAGS = -I:/Users/clements/FSharpPowerPack-1.9.9.9/bin/
FSC = mono ~/FSharp/bin/fsc.exe
FSLEX = mono ~/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe
CONSOLETESTER = ~/xunit/xunit.console.exe

Parser.dll :${PARSERSOURCES}
	${FSC} ${FSCFLAGS} -a -o parser.dll -r:FSharp.PowerPack.dll ${PARSERSOURCES}

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

runtests : ParserTests.exe
	mono ${CONSOLETESTER} ParserTests.exe

