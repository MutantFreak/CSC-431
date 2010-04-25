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
let lexerTest1 () =
    (lexString ";37;;") |> should equal [SEMICOLONTok;INTTok 37;SEMICOLONTok;SEMICOLONTok]

[<Fact>]
let t1 () =
    (parseString "34;") |> should equal (BeginExp [(IntExp 34)])

[<Fact>]
let t2 () =
    (parseString "34 ;22 ; ") |> should equal (BeginExp [IntExp 34;IntExp 22])


