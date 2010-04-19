# If we just say "make", check rules for lexer.fs lexer.exe
all : copy Lexer.fs lexer.exe

SAMPLESOURCES = Tokens.fs Lexer.fs Main.fs
FSC = ~jcswenso/fsc
DLLS = -r:FSharp.PowerPack.Compatibility.dll

#If lexer.fsl has changed, we need to redo lexer.fs

copy :
	cp ~clements/FSharpPowerPack-1.9.9.9/bin/FSharp.Core.dll .

#if Lexer.fsl is newer than Lexer.fs, do it.
Lexer.fs : Lexer.fsl
	mono fslex.exe Lexer.fsl

lexer.exe : ${SAMPLESOURCES}
	${FSC} ${DLLS} -o lexer.exe ${SAMPLESOURCES}

run :
	mono lexer.exe

clean :
	rm lexer.exe Lexer.fs
