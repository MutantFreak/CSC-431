module Run

open Microsoft.FSharp.Text.Lexing
open AST
open AST2
open TypeDef
open Generator

// 

(* Testing function that generates the llvm instruction list, and prints it out. *)
let footleTest (str : string) =
    
    //printf "\nPrinting source=======================\n"
    let buffer = new System.IO.StreamReader(str)
    let src = (buffer.ReadToEnd())
    //printf "%O\n" src
    
    //printf "\nPrinting Lex/Parser result=======================\n"
    let lexbuf = (LexBuffer<byte>.FromBytes (System.Text.Encoding.ASCII.GetBytes src))
    let lex = (Parser.start Lexer.parsetokens lexbuf)
    //printf "%O\n" lex
    
    //printf "\nPrinting StaticPass result=======================\n"
    let (doubleTable,stringTable,functionTable,fieldNameTable) = (StaticPass.transform lex)
    //printf "doubleTable=%A\n" doubleTable
    //printf "stringTable=%A\n" stringTable
    printf "functionTable=%A\n" functionTable
    //printf "fieldNameTable=%A\n" fieldNameTable

    //printf "\nPrinting llvmGen result=======================\n"
    let (generatedList, finalResultRegister) = (wrapperGenerate doubleTable stringTable functionTable fieldNameTable)
    //printf "generatedList=%A\n" generatedList
    //printf "finalResultRegister=%A\n" finalResultRegister
    //printf "\n"
    printLLVM generatedList


//footleTest("../public/clements/footle-examples/test-1.footle")
//footleTest("testCases/variableCreation.footle")
//footleTest("testCases/whileLoop.footle")
footleTest("testCases/simpleFunction.footle")

