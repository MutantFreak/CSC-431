module ParserTests

open Xunit
open FsxUnit.Syntax
open Microsoft.FSharp.Text.Lexing
open Lexer
open Parser
open AST

// given a string, produce the list of tokens
// use this to test the Lexer
let lexString (str : string) =
    let lexbuf = (LexBuffer<byte>.FromBytes (System.Text.Encoding.ASCII.GetBytes str))
    let rec readLoop () =
        let nextToken = parsetokens lexbuf
        if (nextToken = EOFTok)
        then []
        else nextToken::readLoop()
    readLoop()

// given a string, produce the AST
// use this to test the Parser
let parseString (str : string) =
    let lexbuf = (LexBuffer<byte>.FromBytes (System.Text.Encoding.ASCII.GetBytes str))
    Parser.start Lexer.parsetokens lexbuf

[<Fact>]
let lexerTests () =
    (lexString "")                        |> should equal []
    (lexString ";37;;")                   |> should equal [SEMICOLONTok;INTTok 37;SEMICOLONTok;SEMICOLONTok]
    (lexString "a b 2.4")                 |> should equal [IDTok "a";IDTok "b";DOUBLETok 2.4]
    (lexString "a.2.3 b")                 |> should equal [IDTok "a";DOTTok;DOUBLETok 2.3;IDTok "b"]
    (lexString "true false new else if")  |> should equal [TRUETok;FALSETok;NEWTok;ELSETok;IFTok]
    (lexString "while function var q")    |> should equal [WHILETok;FUNCTIONTok;VARTok;IDTok "q"]
    (lexString "var return")              |> should equal [VARTok;RETURNTok]
    (lexString "&& || + - *")             |> should equal [ANDTok;ORTok;PLUSTok;MINUSTok;TIMESTok]
    (lexString "/ < > <= >=")             |> should equal [DIVTok;LTTok;GTTok;LEQTok;GEQTok]
    (lexString "= == =")                  |> should equal [EQTok;DOUBLEEQTok;EQTok]
    (lexString "( ) { }")                 |> should equal [LPARENTok;RPARENTok;LCURLYTok;RCURLYTok]
    (lexString "! . ,")                   |> should equal [BANGTok;DOTTok;COMMATok]

[<Fact>]
let t1 () =
    (parseString "34;") |> should equal (BeginExp [(IntExp 34)])

[<Fact>]
let t2 () =
    (parseString "34 ;22 ; ") |> should equal (BeginExp [IntExp 34;IntExp 22])

// Testing Double
[<Fact>]
let DoubleTest () =
    (parseString "32.22 ;533.3 ; ") |> should equal (BeginExp [DoubleExp 32.22;DoubleExp 533.3])


// Testing "10+55;"
[<Fact>]
let test1 () =
    (parseString "10+55;") |> should equal (BeginExp [PrimExp (PlusP, [IntExp 10; IntExp 55])])
    
// Testing "10+4*5;"
[<Fact>]
let test2 () =
    (parseString "10+4*5;") |> should equal (BeginExp [PrimExp (PlusP, [IntExp 10; PrimExp (TimesP, [IntExp 4; IntExp 5])])] )

// Testing "var x = 3;"
[<Fact>]
let test3 () =
    (parseString "var x = 3;") |> should equal (BeginExp [LetExp ("x", IntExp 3, BeginExp [])] )

// Testing "if(true){3;}"
[<Fact>]
let test4 () =
    (parseString "if(true){3;}") |> should equal (BeginExp [IfExp (BoolExp true, BeginExp [IntExp 3], BeginExp [])] )






