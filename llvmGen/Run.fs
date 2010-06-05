module Run

open Microsoft.FSharp.Text.Lexing
open AST
open AST2
open TypeDef
open Generator

exception NumArgs of string


(* Testing function that generates the llvm instruction list, and prints it out. *)
let runFootle (str : string) =
    
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
    //printf "functionTable=%A\n" functionTable
    //printf "fieldNameTable=%A\n" fieldNameTable

    //printf "\nPrinting llvmGen result=======================\n"
    let (generatedList, finalResultRegister) = (wrapperGenerate doubleTable stringTable functionTable fieldNameTable)
    //printf "generatedList=%A\n" generatedList
    //printf "finalResultRegister=%A\n" finalResultRegister
    //printf "\n"
    printLLVM generatedList


//runFootle("../public/clements/footle-examples/test-1.footle")
//runFootle("testCases/variableCreation.footle")
//runFootle("testCases/whileLoop.footle")
//runFootle("testCases/simpleFunction.footle")

let args = System.Environment.GetCommandLineArgs ()
if((Array.length args) = 1)
then raise (NumArgs (sprintf "wrong number of args\n"))
runFootle((Array.get args 1))


