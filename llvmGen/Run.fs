module Run

open Microsoft.FSharp.Text.Lexing
open AST
open AST2
open TypeDef
open Generator


(* Testing function that generates the llvm instruction list, and prints it out. *)
let testFunc () =
    printf "in testFunc\n"
    let declareLine = Declare(I64, "@add_prim")
    let defineLine = Define(I64, "ourFunc", [])
    //let inputAST = (IntExp 4)
    let inputAST = PrimExp(PlusP, [IntExp 3; IntExp 5])
    let (resultList, resultRegister) = generate inputAST
    printLLVM (declareLine::(defineLine::resultList))


(* Testing function that generates the llvm instruction list, and prints it out. *)
let testFunc2 () =
    printf "in printing test-1.footle\n"
    let buffer = new System.IO.StreamReader("../public/clements/footle-examples/test-5.footle")
    let src = (buffer.ReadToEnd())
    //printf "%O\n" src
    let lexbuf = (LexBuffer<byte>.FromBytes (System.Text.Encoding.ASCII.GetBytes src))
    let lex = (Parser.start Lexer.parsetokens lexbuf)
    printf "%O\n" lex
    printf "=======================\n"
    let statPass = (StaticPass.transform lex)
    printf "%A\n" statPass
    //let gen = (generate )

//testFunc()
testFunc2()
