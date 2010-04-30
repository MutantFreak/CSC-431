module Run

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

printf "======TESTING======\n"


try
  printf "AST=: %A\n" (parseString "function x ( ){  6  ;}")
with
  | _ -> printf "crashed with 1\n"

try
  printf "AST=: %A\n" (parseString "function x ( ){  6  ;}function y ( ){  6  ;}")
with
  | _ -> printf "crashed with 2\n"
  
try
  printf "AST=: %A\n" (parseString "function x ( ){  6  ;} var x = y();")
with
  | _ -> printf "crashed with 3\n"
  
try
  printf "AST=: %A\n" (parseString "function x ( ){  6  ;}function y ( ){  6  ;} var x = y();")
with
  | _ -> printf "crashed with 4\n"

try
  printf "Tokens=: %A\n" (lexString "var x = -1;")
  printf "AST=: %A\n" (parseString "var x = -1;")
with
  | _ -> printf "crashed with 5\n"

try
  printf "AST=: %A\n" (parseString "")
with
  | _ -> printf "crashed with 5\n"
  
try
  printf "AST=: %A\n" (parseString "")
with
  | _ -> printf "crashed with 5\n"

