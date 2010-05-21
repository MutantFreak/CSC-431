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
    let (doubleTable,stringTable,functionTable,fieldNameTable) = (StaticPass.transform lex)
    printf "doubleTable=%A\n" doubleTable
    printf "stringTable=%A\n" stringTable
    printf "functionTable=%A\n" functionTable
    printf "fieldNameTable=%A\n" fieldNameTable

    //convert the funbinding map to a list
    let funList = Map.toList !functionTable
    //reverse the list
    let revFunList = List.rev funList
    //convert the list into map
    let funMap = Map.ofList revFunList
    printf "ourTree=%A\n" funMap
    //let gen = (wrapperGenerate ourTree table1 table2 table3 table4)

//testFunc()
testFunc2()
