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

printf "======WORKS======\n"
printf "AST=: %A\n" (parseString "10+55;")
printf "AST=: %A\n" (parseString "10+4*5;")
printf "AST=: %A\n" (parseString "10+4+5;")
printf "AST=: %A\n" (parseString "10*4+5;")
printf "AST=: %A\n" (parseString "true;")
printf "AST=: %A\n" (parseString "4 == 2;")
printf "AST=: %A\n" (parseString "return 0;")
printf "AST=: %A\n" (parseString "x.y;")
printf "AST=: %A\n" (parseString "var x = 3;")
printf "AST=: %A\n" (parseString "!true;")
printf "AST=: %A\n" (parseString "3;")
printf "AST=: %A\n" (parseString "if(true){}")
printf "AST=: %A\n" (parseString "if(true){3;}")
printf "AST=: %A\n" (parseString "if(true){return 0;}")
printf "AST=: %A\n" (parseString "if(true){return 0;}else{return 1;}")
printf "AST=: %A\n" (parseString "while(true){return 0;}")


printf "======TESTING======\n"
printf "Token=: %A\n" (lexString "!x.y;") 
//printf "Token=: %A\n" (lexString "\"abc\"")
printf "AST=: %A\n" (parseString "!x.y;") 
printf "AST=: %A\n" (parseString "2 + 1 > 2;") 
printf "AST=: %A\n" (parseString "3 <= 23 +3;") 

try
  printf "AST=: %A\n" (parseString "if(true){}")
with
  | _ -> printf "crashed with 1\n"

try
  printf "AST=: %A\n" (parseString "if(true){return 0;}else{return 1;}")
with
  | _ -> printf "crashed with 2\n"
  
try
  printf "AST=: %A\n" (parseString "(10);")
with
  | _ -> printf "crashed with 3\n"
  
try
  printf "AST=: %A\n" (parseString "function x(){return 0;}")
  printf "AST=: %A\n" (parseString "function x(y){return 0;}")
  printf "AST=: %A\n" (parseString "function x(y, z){return z;}")
with
  | _ -> printf "crashed with 4\n"

try
  printf "AST=: %A\n" (parseString "while(true){return 0;}")
with
  | _ -> printf "crashed with 5\n"


