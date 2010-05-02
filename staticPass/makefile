all : StaticPass.dll Run.exe StaticPassTest.exe

PASSOURCES = AST.fs AST2.fs StaticPass.fs 

FSCFLAGS = #-I:bin/
FSC = ~jcswenso/fsc
FSLEX = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fslex.exe
FSYACC = mono ~clements/FSharpPowerPack-1.9.9.9/bin/fsyacc.exe -v
CONSOLETESTER = xunit.console.exe

StaticPass.dll :${PASSOURCES}
	${FSC} ${FSCFLAGS} -a -o StaticPass.dll -r:FSharp.PowerPack.dll ${PASSOURCES}

StaticPassTest.exe : StaticPass.dll StaticPassTest.fs
	${FSC} ${FSCFLAGS} -r:xunit.dll -r:FsTest.dll -r:StaticPass.dll StaticPassTest.fs
	
Run.exe : StaticPass.dll Run.fs
	${FSC} ${FSCFLAGS} -r:StaticPass.dll Run.fs

run : Run.exe
	mono Run.exe
	
test : StaticPassTest.exe
	mono ${CONSOLETESTER} StaticPassTest.exe

clean : 
	rm StaticPass.dll StaticPassTest.exe Run.exe
