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
    
// Testing "!x.y;"
[<Fact>]
let test5 () =
    (parseString "!x.y;") |> should equal (BeginExp [PrimExp (NotP, [FieldRefExp (ID "x", "y")])] )
    
// Testing "while(x == 4){return x+1;}"
[<Fact>]
let test6 () =
    (parseString "while(x == 4){return x+1;}") |> should equal (BeginExp [WhileExp (PrimExp (EqP, [ID "x"; IntExp 4]), BeginExp [ReturnExp (PrimExp (PlusP, [ID "x"; IntExp 1]))])] )


// Testing "var x = \"wordword\"; int?(x);"
[<Fact>]
let test7 () =
    (parseString "var x = \"wordword\"; int?(x);") |> should equal (BeginExp [LetExp ("x", StringExp "wordword", BeginExp [PrimExp (IsIntP, [ID "x"])])] )


// Testing "function y ( ){  6  ;}"
[<Fact>]
let test8 () =
    (parseString "function y ( ){  6  ;}") |> should equal (BeginExp [LetrecExp ([("y", [], BeginExp [IntExp 6])], BeginExp [])] )
       

    
// Testing "var x = 3 + 4 / ! c . abc * 6 == 5 + 6 + 7 && true || false;"
[<Fact>]
let test9 () =
    (parseString "var x = 3 + 4 / ! c . abc * 6 == 5 + 6 + 7 && true || false;") |> should equal (BeginExp [LetExp ("x", PrimExp (EqP, [PrimExp (PlusP, [IntExp 3; PrimExp (TimesP, [PrimExp (DivP, [IntExp 4; PrimExp (NotP, [FieldRefExp (ID "c", "abc")])]); IntExp 6])]); PrimExp (OrP, [PrimExp (AndP, [PrimExp (PlusP, [PrimExp (PlusP, [IntExp 5; IntExp 6]); IntExp 7]); BoolExp true]); BoolExp false])]), BeginExp [])] )

    
    
    



