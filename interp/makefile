all : interp.exe

SAMPLESOURCES = AST.fs Values.fs interp.fs
FSC = ~jcswenso/fsc
DLLS = -r:xunit.dll -r:FsTest.dll

interp.exe : ${SAMPLESOURCES}
	${FSC} ${DLLS} -o interp.exe ${SAMPLESOURCES}

run :
	mono ./xunit.console.exe ./interp.exe

clean :
	rm interp.exe
