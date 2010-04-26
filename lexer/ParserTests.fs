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


