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
let footleTest (str : string) =
    
    printf "\nPrinting source=======================\n"
    let buffer = new System.IO.StreamReader(str)
    let src = "5; "//(buffer.ReadToEnd())
    printf "%O\n" src
    
    printf "\nPrinting Lex/Parser result=======================\n"
    let lexbuf = (LexBuffer<byte>.FromBytes (System.Text.Encoding.ASCII.GetBytes src))
    let lex = (Parser.start Lexer.parsetokens lexbuf)
    printf "%O\n" lex
    
    printf "\nPrinting StaticPass result=======================\n"
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
    printf "revFunList=%O\n" revFunList
    let ((theStr : string , theStrList : string list , theExp : exp , theBool : bool) , theInt : int) = (List.head revFunList)
    printf "theExp=%O\n" theExp
    let (generatedList, finalResultRegister) = (wrapperGenerate theExp !doubleTable !stringTable !functionTable !fieldNameTable)
    //printf "gen=%O\n" gen
    printLLVM generatedList

//(string * string list * exp * bool) * int
//( ( ((frontFrameMap, count) as frontFrame), frameList) as ourSenv)
//testFunc()

footleTest("../public/clements/footle-examples/test-1.footle")

