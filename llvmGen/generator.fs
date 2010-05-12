
(* How to call a C function from LLVM. In C, a 64 bit # is a "long long" or i64_t

delcare i64 @and_prim (i64, i64)
use a call instruction to call @and_prim
*)

(* For Milestone 1, we need to be able to handle integers, addition, and subtraction *)

let registerCounter = ref 1;

// Function to produce a fresh register name
let getFreshRegister = let newRegisterName = ("%r" ^ string_of_int(!registerCounter))
                       registerCounter := !registerCounter + 1
                       newRegisterName

(* Function that takes in an AST2, and the existing list of LLVM instructions, and returns a new list of LLVM instructions, 
   tupled with a register where the result is stored *)
let generate ourTree instrList =
    match ourTree with
(*
        ID of (string * int * int)
        | BoolExp of bool
*)
          // Generate an LLVM instruction that stores theNum into a fresh register
        | IntExp (theNum : int) -> let newReg = getFreshRegister
                                   let newInstr = LLVM_Line(RegProdLine(newReg, Add(i64, theNum, i64, 0)))
                                   // Append newInstr to the end of instrList
                                   let newList = List.Append instrList [|newInstr|]
                                   (newList, newReg)
(*
        | DoubleExp of int
        | StringExp of int 
*)
          // Shift theNum by 2 bits (multiply by 4) to make space for the tag, then add in a small number for the tag (i.e. 1 for the tag bits 01, or 2 for the tag bits 10)
          // want an equivalent of %r1 = add i64 theNum, 0
        | PrimExp (thePrim : AST.prim, argsList : exp list) -> match thePrim with
                                                                   | PlusP -> let (letList, leftResultReg) = generate argsList[0]
                                                                              let (rightList, rightResultReg) = generate argsList[1]
                                                                              let addInstr = LLVM_Line(RegProdLine(getFreshRegister, Add(FieldType * LLVM_Arg * FieldType * LLVM_Arg)))
                                                                              //Put the left existingInstrList::leftList::rightList::addInstr::

                                                                   | MinusP -> 
(*
        | IfExp of (exp * exp * exp)
        | WhileExp of (exp * exp) /
        | ReturnExp of exp
        | SetExp of ((string * int * int) * exp)
        | BeginExp of (exp list)
        | FieldRefExp of (exp * int)
        | FieldSetExp of (exp * int * exp)
        | MethodCallExp of (exp * int * exp list)
        | NewExp of (exp * exp list)
        | AppExp of (exp * exp list)
        | CloExp of (string * int) 
        | ScopeExp of (int * string list * exp)
*)


(* Function that eakes a single LLVM instruction, and prints its string representation. *)
let printLLVMHelper singleInstr = 
    let ref buildingString = ""
    match singleInstr with
        | RegProdLine (resultRegister, producingInstr) -> buildingString := resultregister ^ (get the string value of the producingInstr from a helper function)
        | NonRegProdLine (nonProducingInstr) -> buildingString = (get the string value of the nonProducingInstr from a helper function)
    // return the string that we built so it can be printed out by printLLVM
    !buildingString

(* Function that takes in an LLVM instruction list, and prints the result of printLLVMHelper being called on each instruction. *)
let printLLVM instrList =
    for eachInstr in instrList do
    printf "%A\n " (printLLVMHelper eachInstr)

(* Testing function that generates the llvm instruction list, and prints it out. *)
let testFunc =
    let defineLine = 
    let inputAST = 
    let results = generate inputAST [defineLine]
    printLLVM results
