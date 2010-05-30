module Generator
(* How to call a C function from LLVM. In C, a 64 bit # is a "long long" or i64_t

delcare i64 @and_prim (i64, i64)
use a call instruction to call @and_prim
*)

(* For Milestone 1, we need to be able to handle integers, addition, and subtraction. *)
(* For Milestone 2, we need to be able to handle everything except objects and arrays. Namely we need to work on while loops & functions. *)

// for scopeExp's, keep track of what register the current parent eframe is stored in.

open AST2
open TypeDef

exception RuntimeError of string

let GdoubleTable = ref Map.empty<double,int>
let GstringTable = ref Map.empty<string,int>
let GfunctionTable = ref Map.empty<(string * string list * exp * bool),int>
let GfieldNameTable = ref Map.empty<string,int>

//name of the register where our current eframe is stored
let Geframe = ref ""
let Genv = ref "%env"

let registerCounter = ref 1;
let labelCounter = ref 1;

// Function to produce a fresh register name
let getFreshRegister () = 
    let newRegisterName = ("%r" + (string !registerCounter))
    registerCounter := !registerCounter + 1
    newRegisterName

// Function to produce a fresh label name
let getFreshLabel () = 
    let newLabelName = ("label_" + (string !labelCounter))
    labelCounter := !labelCounter + 1
    newLabelName

(* Function that takes in a frameOffset (how many frames to traverse) and a list of frameOffset sets of (gep & load) instrs.
   instrList is the existing instr list - it starts off as [].
   frameWereIn is the frame that we are currently referencing parts of. *)
let rec traverseEframes frameOffset instrList frameWereIn = let loadResultRegister:string = getFreshRegister()
                                                            let gepResultRegister = getFreshRegister()
                                                            // if frameOffset is 0, we're done traversing, so return the list.
                                                            if (frameOffset = 0)
                                                            then (instrList, frameWereIn)
                                                                 // Otherwise generate another gep & load instruction to move up one eframe
                                                            else let newInstr = RegProdLine(Register(loadResultRegister), Load(Eframe0Ptr(frameWereIn), EFramePtrPtr, Register(gepResultRegister)))
                                                                 ((List.append instrList [newInstr]), Register(loadResultRegister)) // supposed to be LLVM_ARG, but was string
(* Function that takes in an AST2, and the existing list of LLVM instructions, and returns a new list of LLVM instructions, 
   tupled with a register where the result is stored *)
let rec generate ourTree =
    match ourTree with
        //get the list of llvm instructions to traverse eframes until frameOffset decrements to 0
        | ID (varName :string, frameOffset : int, fieldOffset : int) -> // Generate one last set of GEP and load from the result of traversing, into a final result.
                                                                        // %reg_51 = getelementptr %eframe* traverseResultReg, i32 0, i32 2, i32 1
                                                                        let frameWereIn = !Genv
                                                                        // traverseInstrList is the list of instructions used to traverse eFrames going upwards.
                                                                        // traverseResultReg is the register where the final eFrame which we want to use is stored.
                                                                        let (traverseInstrList, traverseResultReg) = traverseEframes frameOffset [] (Register(frameWereIn)) //supposed to be LLVM_Arg, but was string
                                                                        let gepReg = getFreshRegister()
                                                                        let loadResultReg = getFreshRegister()
                                                                        (*	Produce something like this. reg_35 is traverseResultReg. reg_51 is gepReg.
                                                                           $reg_52 is loadResultReg.
                                                                           %reg_51 = getelementptr %eframe* %reg_35, i32 0, i32 2, i32 1
	                                                                        // load the i64 from the address in reg_51, into reg_52
	                                                                        %reg_52 = load i64* %reg_51 *)
                                                                        let loadInstr = RegProdLine(Register(loadResultReg), Load(Eframe2Ptr(traverseResultReg, fieldOffset), I64ptr, Register(gepReg)))
                                                                        ((List.append traverseInstrList [loadInstr]), loadResultReg)
          // Generate an LLVM instruction that stores an i1 with 1 for true, 0 for false, into a fresh register.
        | BoolExp (value : bool) -> let newReg = getFreshRegister()
                                    if (value = true)
                                         (* Using the number 14 because it's the number 1, but shifted three times (100), added 4 to 
                                            signify it's a boolean, and added 2 to signify it's a boolean or void. Result is 1110 *)
                                    then let newInstr = RegProdLine(Register(newReg), Add(I64, Number(14), Number(0)))
                                         ([newInstr], newReg)
                                         (* Using the number 6 to signify 0110. Last 2 digits (10) signify it's a boolean or void, and
                                            the extra 1 signifies it is a bool, and the first 0 means it's false. *)
                                    else let newInstr = RegProdLine(Register(newReg), Add(I64, Number(6), Number(0)))
                                         ([newInstr], newReg)
          // Generate an LLVM instruction that stores theNum into a fresh register
        | IntExp (theNum : int) -> let newReg = getFreshRegister()
                                   let newNum = theNum * 4
                                   let newInstr = RegProdLine(Register(newReg), Add(I64, Number(newNum), Number(0)))
                                   ([newInstr], newReg)
        | DoubleExp (index : int) -> printf "WARNING, DoubleExp IS BROKEN\n"
                                     ([], "fakeRegister")
(*        // remap the double table backwards so that it goes int -> double
          // pull the double out of the table
            allocate 8 bytes for double.
            put the double @ given ptr location
            convert the ptr to an int
            add the tag bits

            malloc
            getelement ptr
            store double into 
*)
        | StringExp (index : int) -> printf "WARNING, StringExp IS BROKEN\n"
                                     ([], "fakeRegister")
(*        // remap the string table backwards so that it goes int -> string
          // pull the stringo ut of the table
          
          // Shift theNum by 2 bits (multiply by 4) to make space for the tag, then add in a small number for the tag (i.e. 1 for the tag bits 01, or 2 for the tag bits 10)
          // want an equivalent of %r1 = add i64 theNum, 0
*)
        | PrimExp (thePrim : AST.prim, argsList : exp list) -> match thePrim with
                                                                   | AST.PlusP -> let finalResultReg = getFreshRegister()
                                                                                  let (leftList, leftResultReg) = generate (List.head argsList)
                                                                                  let (rightList, rightResultReg) = generate (List.head (List.tail argsList))
                                                                                  let addInstr = RegProdLine(Register(finalResultReg), Call(I64, "@add_prim", [(I64, Register(leftResultReg)); (I64, Register(rightResultReg))]) )
                                                                                  (List.append leftList (List.append rightList [addInstr]), finalResultReg)
                                                                   | AST.MinusP -> let finalResultReg = getFreshRegister()
                                                                                   let (leftList, leftResultReg) = generate (List.head argsList)
                                                                                   let (rightList, rightResultReg) = generate (List.head (List.tail argsList))
                                                                                   let subInstr = RegProdLine(Register(finalResultReg), Call(I64, "@sub_prim", [(I64, Register(leftResultReg)); (I64, Register(rightResultReg))]) )
                                                                                   (List.append leftList (List.append rightList [subInstr]), finalResultReg)
                                                                   | AST.TimesP -> let finalResultReg = getFreshRegister()
                                                                                   let (leftList, leftResultReg) = generate (List.head argsList)
                                                                                   let (rightList, rightResultReg) = generate (List.head (List.tail argsList))
                                                                                   let timesInstr = RegProdLine(Register(finalResultReg), Call(I64, "@times_prim", [(I64, Register(leftResultReg)); (I64, Register(rightResultReg))]) )
                                                                                   (List.append leftList (List.append rightList [timesInstr]), finalResultReg)
                                                                   | AST.DivP -> let finalResultReg = getFreshRegister()
                                                                                 let (leftList, leftResultReg) = generate (List.head argsList)
                                                                                 let (rightList, rightResultReg) = generate (List.head (List.tail argsList))
                                                                                 let divInstr = RegProdLine(Register(finalResultReg), Call(I64, "@div_prim", [(I64, Register(leftResultReg)); (I64, Register(rightResultReg))]) )
                                                                                 (List.append leftList (List.append rightList [divInstr]), finalResultReg)
                                                                     // TODO: The C function's result should be a double, which we have to store.
                                                                   | AST.SqrtP -> let finalResultReg = getFreshRegister()
                                                                                  let (leftList, leftResultReg) = generate (List.head argsList)
                                                                                  let sqrtInstr = RegProdLine(Register(finalResultReg), Call(I64, "@add_prim", [(I64, Register(leftResultReg))]) )
                                                                                  (List.append leftList [sqrtInstr], finalResultReg)
                                                                   | _ -> raise (RuntimeError (sprintf "Found an invalid prim: %A\n" thePrim))
        | IfExp (ifExp : exp, thenExp : exp, elseExp : exp) -> let (ifList, ifResultReg) = generate (ifExp)
                                                               let (thenList, thenResultReg) = generate (thenExp)
                                                               let (elseList, elseResultReg) = generate (elseExp)
                                                               let thenLabel = getFreshLabel()
                                                               let elseLabel = getFreshLabel()
                                                               let isBoolRegister = getFreshRegister()
                                                                   // Line1 = generate a line of LLVM code which calls a C function to ensure that the ifResultReg is a bool.
                                                                   // Put the result into isBoolRegister(a throw-away register since the result of a call must be stored into something.)
                                                               let callLine = RegProdLine(Register(isBoolRegister), Call(I64, "@expectBool", [(I64, Register(ifResultReg))] ))
                                                                   // Line2 = generate a LLVM line which says %compReg = icmp eq i64 16(16 is a number representing true) ifResultReg
                                                               let compReg = getFreshRegister()
                                                               let compLine = RegProdLine(Register(compReg), ICmp(Eq, I64, Number(16), Register(ifResultReg)))
                                                                   // Line3 = generate a llvm line which says br i1 %compReg, label thenLabel, label elseLabel
                                                               let branchLine = NonRegProdLine(Br(Register(compReg), thenLabel, elseLabel))
                                                               ((List.append ifList (callLine:: (compLine :: (branchLine :: (Label(thenLabel) :: (List.append thenList (List.append elseList [Label(elseLabel)]))))))), ifResultReg)
        | WhileExp (guardExp : exp, bodyExp : exp) ->     // Generate the list of instructions for the guard expression of the while loop
                                                      let (guardInstrList, guardResultReg) = generate guardExp
                                                      let testGuardLabel = getFreshLabel()
                                                      let bodyLabel = getFreshLabel()
                                                      let doneLabel = getFreshLabel()
                                                      let isBoolRegister = getFreshRegister()
                                                          // Line1 = generate a line of LLVM code which calls a C function to ensure that the guardResultReg is a bool.
                                                          // Put the result into isBoolRegister(a throw-away register since the result of a call must be stored into something.)
                                                      let testBoolInstr = RegProdLine(Register(isBoolRegister), Call(I64, "@expectBool", [(I64, Register(guardResultReg))] ))
                                                          // Line2 = generate a LLVM line which says %compReg = icmp eq i64 16(16 is a number representing true) guardResultReg
                                                      let compReg = getFreshRegister()
                                                          // Generate an instruction to chec if guardResultReg is true, and put the answer into compReg
                                                      let testTrueInstr = RegProdLine(Register(compReg), ICmp(Eq, I64, Number(16), Register(guardResultReg)))
                                                          // A list of instructions produced from the bodyExp, as well as ther egister where the result is stored.
                                                      let (bodyInstrList, bodyResultReg) = generate bodyExp
                                                          // Line3 = generate a llvm line which says br i1 %compReg, label bodyLabel, label doneLabel.
                                                          // Goes to bodyLabel if compReg was true, goes to doneLabel otherwise.
                                                      let branchInstr = NonRegProdLine(Br(Register(compReg), bodyLabel, doneLabel))
                                                          // Unconditional branch instruction, placed at the end of the body, which jumps back up to the guard evaluation.
                                                      let unconditionalBrInstr = NonRegProdLine(UnconditionalBr(testGuardLabel))
                                                          // Returns all of the instructions and labels in a list in their correct sequence, tupled with the register where the result of the body is stored.
                                                      ((Label(testGuardLabel) :: (List.append guardInstrList (testBoolInstr :: (testTrueInstr :: (branchInstr :: (Label(bodyLabel) :: (List.append bodyInstrList (unconditionalBrInstr :: [Label(doneLabel)] )))))))), bodyResultReg)
        | ReturnExp (returnExp : exp) ->     // Generate the instructions for this returnExp
                                         let (instrList, resultReg) = generate returnExp
                                             // Generate an LLVM line that is a return statement to the resultReg we just found. ret i64 resultReg
                                         let retLine = NonRegProdLine(Ret(I64, Register(resultReg)))
                                         (List.append instrList [retLine], resultReg)
                                         
        | SetExp ((varName : string, frameOffset : int, fieldOffset : int), theExp : exp) -> let freshReg = getFreshRegister()
                                                                                             printf "we found varName %O in a SetExp\n" varName
                                                                                             let (insideList, insideReg) = generate theExp
                                                                                             let storeInst = NonRegProdLine(Store (Eframe2Ptr(Register(!Geframe), fieldOffset), I64, Register(insideReg), I64ptr, Register(freshReg), Register(freshReg)))
                                                                                             (List.append insideList [storeInst], freshReg)
        | BeginExp (expList : exp list) -> match expList with
                                               // If there are no exp's in this begin, return the assembly equivalent of a voidV
                                               //Generate an instruction that adds i64 (number value of a voidV) and 0 together into a fresh register
                                               | [] -> let finalResultReg = getFreshRegister()
                                                       // Bottom two digits are 10 for bool or void. The next digit up is a 0 to specify a Void.
                                                       let newInstr = RegProdLine(Register(finalResultReg), Add(I64, Number(4), Number(0)))
                                                       ([newInstr], finalResultReg)
                                 
                                               | _ -> let instrList = ref []
                                                      let finalResultReg = ref ""
                                                      for eachExp in expList do
                                                          let (resultList, resultReg) = generate (eachExp)
                                                          instrList := List.append !instrList resultList
                                                          finalResultReg := resultReg
                                                      (!instrList, !finalResultReg)
(*
        | FieldRefExp of (exp * int)
        | FieldSetExp of (exp * int * exp)
        | MethodCallExp of (exp * int * exp list)
        | NewExp of (exp * exp list)
        | AppExp of (exp * exp list)
        | CloExp of (string * int) 
*)
        | ScopeExp (numOfVar : int, paramList : string list, theExp : exp) -> let mallocReg = getFreshRegister()
                                                                              let mallInstr = RegProdLine(Register(mallocReg), Malloc(EFPtr_i64_Arr(numOfVar)))
                                                                              let bitcastReg = getFreshRegister()
                                                                              let bitcastInstr = RegProdLine(Register(bitcastReg), Bitcast(EFPtr_i64_Arr_Ptr(numOfVar), Register(mallocReg), EFramePtr))
                                                                              Geframe := bitcastReg
                                                                              let storeReg = getFreshRegister()
                                                                              let storeInst = NonRegProdLine(Store (Eframe0Ptr(Register(!Geframe)), EFramePtr, Register(!Genv), EFramePtrPtr, Register(storeReg), Register(storeReg)))
                                                                              let storeReg2 = getFreshRegister()
                                                                              let storeInst2 = NonRegProdLine(Store (Eframe1Ptr(Register(!Geframe)), I64, ActualNumber(numOfVar), I64ptr, Register(storeReg2), Register(storeReg2)))
                                                                              let (resultList, resultReg) = generate (theExp)
                                                                              (List.append [mallInstr; bitcastInstr; storeInst; storeInst2 ] resultList, resultReg)                          
        | _ -> raise (RuntimeError (sprintf "Found an expression that is not supported: %A\n" ourTree))


let wrapperGenerate ourTree table1 table2 table3 table4 = 
    GdoubleTable := table1
    GstringTable := table2
    GfunctionTable := table3
    GfieldNameTable := table4
    generate ourTree

(* Function that takes a FieldType and returns its string representation. *)
let printFieldType theField = 
    match theField with
        | F64 -> "f64"
        | I1 -> "i1"
        | I64 -> "i64"
        | I64ptr -> "i64*"
        | EFramePtr -> "%eframe*"
        | EFramePtrPtr -> "%eframe**"
        | EFPtr_i64_Arr (index : int) -> "{%eframe*, i64, [" + sprintf "%d" index + " x i64]}"
        | EFPtr_i64Ptr_Arr (index : int) -> "{%eframe*, i64*, [" + sprintf "%d" index + " x i64]}"
        | EFPtr_i64_Arr_Ptr (index : int) -> "{%eframe*, i64, [" + sprintf "%d" index + " x i64]}*"  //{%eframe*, i64, [_ x i64]}*
        | EFPtr_i64Ptr_Arr_Ptr (index : int) -> "{%eframe*, i64*, [" + sprintf "%d" index + " x i64]}*"
        | CloPtr -> "%closure*"
        | CloPtrPtr -> "%closure**"
        | ArrayPtr -> "ArrayPtr not yet supported."
        | ArrayPtrPtr -> "ArrayPtrPtr not yet supported."
        
(* Function that takes an LLVM_Arg and returns its string representation. *)
let printLLVM_Arg theArg = 
    match theArg with
        | Register (name : string) -> name
          //TODO: Ask about when to do the divide by 4 for shifting right again.
        | Number (theNum : int) -> string (theNum/4)
        | GlobalLabel (theLabel : string) -> theLabel
        | ActualNumber (theNum : int) -> string (theNum)

(* Function to print out an args list. first says whether or not this is the first argument, used to decide whether or not to put a comma in. *)
let rec printArgsList (first:bool) argList =
    match argList with
        | [] -> ""
                                                //If this is the first argument in the list, do not precede it with a comma
          //TODO: Ask about the syntax for this line
        | (theType : FieldType, theArg : LLVM_Arg)::rest -> if (first = true)
                                                            then (printFieldType theType) + " " + (printLLVM_Arg theArg) + (printArgsList false rest)
                                                            else ", " + (printFieldType theType) + " " + (printLLVM_Arg theArg) + (printArgsList false rest)

(* Takes in the type of flavor, and the register the instruction is being put into *)
let printFlavor gepType (leftReg : LLVM_Arg) = 
    match gepType with
        | Eframe0 (argReg : LLVM_Arg) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe " + (printLLVM_Arg argReg) + ", i64 0, i64 0\n"
        | Eframe1 (argReg : LLVM_Arg) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe " + (printLLVM_Arg argReg) + ", i64 0, i64 1\n"
        | Eframe2 (argReg : LLVM_Arg, index : int) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe " + (printLLVM_Arg argReg) + ", i64 0, i64 2, i64 " + sprintf "%d" index + "\n"
        | Eframe0Ptr (argReg : LLVM_Arg) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe* " + (printLLVM_Arg argReg) + ", i64 0, i64 0\n"
        | Eframe1Ptr (argReg : LLVM_Arg) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe* " + (printLLVM_Arg argReg) + ", i64 0, i64 1\n"
        | Eframe2Ptr (argReg : LLVM_Arg, index : int) -> "\t" + (printLLVM_Arg leftReg) + " = getelementptr %eframe* " + (printLLVM_Arg argReg) + ", i64 0, i64 2, i64 " + sprintf "%d" index + "\n"

let printConditionCode code = 
    match code with
        | Eq -> "eq"
        | Ne -> "ne"

(* Function that takes a register producing instruction, and returns its string representation. *)
let printRegProdInstr instr resultRegister =
    match instr with
        | Load (getElementPtrFlavor : Flavor, gepFieldType : FieldType, getElementPtrField : LLVM_Arg) -> (printFlavor getElementPtrFlavor getElementPtrField) + "\t" + (printLLVM_Arg resultRegister) + " = load " + (printFieldType gepFieldType) + " " + (printLLVM_Arg getElementPtrField)
        | Add (arg1Type : FieldType, arg1: LLVM_Arg, arg2 : LLVM_Arg) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "add " + (printFieldType arg1Type) + " " + (printLLVM_Arg arg1) + ", " + (printLLVM_Arg arg2)
          // Format is "call i64 (...)* @add_prim(i64 5, i64 2)"
        | Call (theType : FieldType, name : string, argsList : Arg list) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "call " + (printFieldType theType) + " " + name + " (" + (printArgsList true argsList) + ")"
        | ICmp (code : ConditionCode, theType : FieldType, arg : LLVM_Arg, label : LLVM_Arg) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "icmp " + (printConditionCode code) + " " + (printFieldType theType) + " " + (printLLVM_Arg arg) + ", " +  (printLLVM_Arg label)
        | Malloc (theType : FieldType) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "malloc " + (printFieldType theType) + ", align 4" //%reg_34 = malloc {%eframe*, i64, [3 x i64]}, align 4
        | Bitcast (theType : FieldType, theArg : LLVM_Arg, theType2 : FieldType) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "bitcast " + (printFieldType theType) + " " + (printLLVM_Arg theArg) + " to " + (printFieldType theType2)

(* Function that takes a non register producing instruction, and returns its string representation. *)
let printNonRegProdInstr instr =
    match instr with
        // flavor is the kind of GEP we have, followed by two registers and their fieldtypes.
        // The data in first register is saved in memory at the address found in second register.
        // store i64 3, i64* %reg_37
        | Store (gepType : Flavor, dataType : FieldType, dataReg : LLVM_Arg, addressType : FieldType, addressReg : LLVM_Arg, gepRegister : LLVM_Arg) ->(printFlavor gepType gepRegister) + "\t" + "store " + (printFieldType dataType) + " " + (printLLVM_Arg dataReg) + ", " + (printFieldType addressType) + " " + (printLLVM_Arg addressReg)
        // Br is made up of the i1 field to check (a LLVM_ARG), the label to go to if it's true, and the label to go to if it's false.
        | Br (boolReg : LLVM_Arg, trueLabel : string, falseLabel : string) -> "\t" + "br i1 " + (printLLVM_Arg boolReg) + ", label %" + trueLabel + ", label %" + falseLabel
        | UnconditionalBr (label : string) -> "\t" + "br label %" + label
        | Ret (theType : FieldType, resultReg : LLVM_Arg) -> "\t" + "ret " + (printFieldType theType) + " " + (printLLVM_Arg resultReg)

(* Branch should look like this:
   br i1 %cond, label %IfEqual, label %IfUnequal
*)

(* Function that takes a single LLVM instruction, and returns its string representation. *)
let printLLVMLine singleInstr = 
    match singleInstr with
        | Label (name : string) -> name + ":"
          //changed the function to take in resultRegister and do it in the called function
        | RegProdLine (resultRegister : LLVM_Arg, producingInstr : RegProdInstr) -> (printRegProdInstr producingInstr resultRegister)
        | NonRegProdLine (nonProducingInstr) -> (printNonRegProdInstr nonProducingInstr)
        | Declare (theType: FieldType, name : string) -> "declare " + (printFieldType theType) + " " + name
        | Define (theType : FieldType, name : string, paramsList: Param list) -> "define " + (printFieldType theType) + " " + name + " " + (sprintf "%O" paramsList )

(* Function that takes in an LLVM instruction list, and prints the string representation of each instruction. *)
let printLLVM instrList =
    for eachInstr in instrList do
    printf "%O\n" (printLLVMLine eachInstr)

