module Run

open AST
open AST2
open TypeDef
open Generator

(* Testing function that generates the llvm instruction list, and prints it out. *)
let testFunc =
    let declareLine = Declare(I64, "@add_prim")
    let defineLine = Define(I64, "ourFunc", [])
    //let inputAST = (IntExp 4)
    let inputAST = PrimExp(PlusP, [IntExp 3; IntExp 5])
    let (resultList, resultRegister) = generate inputAST (declareLine::[defineLine])
    printLLVM (declareLine::(defineLine::resultList))
