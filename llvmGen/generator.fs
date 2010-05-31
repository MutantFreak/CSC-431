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

let GdoubleTable = ref Map.empty<int,double>
let GstringTable = ref Map.empty<int,string>
let GfunctionTable = ref Map.empty<int,(string * string list * exp * bool)>
let GfieldNameTable = ref Map.empty<int,string>

//name of the register where our current eframe is stored
let Geframe = ref ""
let Genv = ref "%env"

let registerCounter = ref 1;
let labelCounter = ref 1;
let stringNameCounter = ref 1;

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

// Function to produce a fresh string name. eg. @stringconst_0s
let getFreshStringName () = 
    let newStringName = ("@stringconst_" + (string !stringNameCounter) + "s")
    stringNameCounter := !stringNameCounter + 1
    newStringName

let printHead () = 
    printf "\n\
\n\
;; C helpers\n\
\n\
declare void @print_val(i64)\n\
declare void @halt_with_error(i64,i64)\n\
declare void @halt_with_error_noval(i64)\n\
declare void @halt_with_error_firstword(i64,i64)\n\
declare void @halt_with_error_int(i64,i64)\n\
declare i64  @find_slot(%%slots*,i64)\n\
declare i64  @try_to_set_slot(%%slots*,i64,i64)\n\
declare void @ding()\n\
declare void @print_int(i64)\n\
\n\
;; C primitives:\n\
\n\
declare i64 @equal_prim(i64,i64)\n\
declare i64 @and_prim(i64,i64)\n\
declare i64 @or_prim(i64,i64)\n\
declare i64 @not_prim(i64)\n\
declare i64 @print_prim(i64)\n\
declare i64 @flexiplus_prim(i64,i64)\n\
declare i64 @fleximinus_prim(i64,i64)\n\
declare i64 @flexitimes_prim(i64,i64)\n\
declare i64 @flexidivide_prim(i64,i64)\n\
declare i64 @flexilessthan_prim(i64,i64)\n\
declare i64 @flexigreaterthan_prim(i64,i64)\n\
declare i64 @flexilessequalthan_prim(i64,i64)\n\
declare i64 @flexigreaterequalthan_prim(i64,i64)\n\
declare i64 @stringLength_prim(i64)\n\
declare i64 @subString_prim(i64,i64,i64)\n\
declare i64 @stringAppend_prim(i64,i64)\n\
declare i64 @stringEqualHuh_prim(i64, i64)\n\
declare i64 @stringLessThanHuh_prim(i64, i64)\n\
declare i64 @stringHuh_prim(i64)\n\
declare i64 @floatHuh_prim(i64)\n\
declare i64 @plainHuh_prim(i64)\n\
declare i64 @closureHuh_prim(i64)\n\
declare i64 @instanceof_prim(i64,i64)\n\
declare i64 @sqrt_prim(i64)\n\
\n\
;; the values in memory:\n\
\n\
%%obj =      type {i64, %%slots*} ;; (this works for any memory val other than floats)\n\
%%closure =      type {i64, %%slots*, %%eframe*}\n\
%%strobj   = type {i64, %%slots*, i8*}\n\
%%floatobj = type {i64, float}\n\

;; environments and slots:\n\
\n\
%%eframe = type {%%eframe*, i64, [0 x i64]}\n\
%%slots = type {%%slots*, i64, i64}\n\
\n\
@empty_env = constant %%eframe undef\n\
@empty_slots = constant %%slots undef\n\
\n\
%%packed_args = type {i64, [0 x i64]}\n\n"

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
                                                                 ((List.append instrList [newInstr]), Register(loadResultRegister))

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
                                                                        let (traverseInstrList, traverseResultReg) = traverseEframes frameOffset [] (Register(frameWereIn))
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
          // pull the string out of the table
          let mallocResultReg = getFreshRegister()
          let gep0ResultReg = getFreshRegister()
          let store0ResultReg = getFreshRegister()
          let gep1ResultReg = getFreshRegister()
          let store1ResultReg = getFreshRegister()
          let gep2ResultReg = getFreshRegister()
          let store2ResultReg = getFreshRegister()
          let gep3ResultReg = getFreshRegister()
          let ptrToIntResultReg = getFreshRegister()
          let orResultReg = getFreshRegister()

         let line1 = RegProdLine(mallocResultReg, Malloc(StrObj))
           %reg_38 = malloc %strobj, align 4
	         // put a reference to the first field of the strobj into reg_39
	         %reg_39 = getelementptr %strobj* %reg_38, i32 0, i32 0
         let line2 = RegProdLine(store0ResultReg, Store(StrObj0Ptr(mallocResultReg), I64, ActualNumber(1), I64Ptr, gep0ResultReg, gep0ResultReg))
         let line3 = RegProdLine(store1ResultReg, Store(StrObj1Ptr(mallocResultReg), SlotsPtr, Constant("@empty_slots"), SlotsPtrPtr, gep1ResultReg, gep1ResultReg))
         let line4 = RegProdLine(gep2ResultReg, StrObjPtr(mallocResultReg))
         let (stringName, constName) = lookup in map (index of this string)
         let line5 = RegProdLine(store2ResultReg, Store(Array0Ptr(Array((stringLength stringName), I8), Constant(constName)), I8Ptr, gep3ResultReg, I8PtrPtr, gep2ResultReg, gep3ResultReg ))
         let line6 = RegProdLine(ptrToIntResultReg, PtrToInt(StrObjPtr, mallocResultReg, I64))
             // Put on the tag bits for a string.
         let line7 = RegProdLine(orResultReg, Or(I64, ptrToIntResultReg, ActualNumber(1)))

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


let reverseMap theMap = 
    let theList = Map.toList !theMap

    let revList = ref []
    for tuple in theList do
        match tuple with
            | (a, b) -> revList := (List.append !revList [(b,a)] )
    let revMap = Map.ofList !revList
    revMap

let wrapperGenerate doubleT stringT functionT fieldT = 
    //fill out the tables
    GdoubleTable := reverseMap doubleT
    GstringTable := reverseMap stringT
    GfunctionTable := reverseMap functionT
    GfieldNameTable := reverseMap fieldT
    let llvmLines = ref []
    let mainFinalReg = ref ""
    let funList = Map.toList !GfunctionTable
    for funEntry in funList do
        match funEntry with
            | (theInt : int, (theStr : string , theStrList : string list , theExp : exp , theBool : bool)) -> let (generatedLLVM, finalReg) = generate theExp
                                                                                                              let defineLine = Define(I64, "@"+theStr, [(EFramePtr, "%env")])
                                                                                                              llvmLines := (List.append !llvmLines [defineLine])
                                                                                                              llvmLines := (List.append !llvmLines generatedLLVM)
                                                                                                              if (theStr = "main")
                                                                                                              then mainFinalReg := finalReg
                                                                                                              else ()
    (!llvmLines, !mainFinalReg)

(* Function that takes a FieldType and returns its string representation. *)
let rec printFieldType theField = 
        match theField with
            | F64 -> "f64"
            | I1 -> "i1"
            | I8 -> "i8"
            | I8Ptr -> "i8*"
            | I8PtrPtr -> "i8**"
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
            | Array (num : int, field : FieldType) -> "[" + string num + " x " + printFieldType field + "]"
            | StrObj -> "%strobj"
        
(* Function that takes an LLVM_Arg and returns its string representation. *)
let printLLVM_Arg theArg = 
    match theArg with
        | Register (name : string) -> name
          //TODO: Ask about when to do the divide by 4 for shifting right again.
        | Number (theNum : int) -> string (theNum/4)
        | GlobalLabel (theLabel : string) -> theLabel
        | ActualNumber (theNum : int) -> string (theNum)
        | Constant (name : string) -> name

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
        | StrObj0Ptr (argReg : LLVM_Arg) -> (printLLVM_Arg leftReg) + " = getelementptr %strobj* " + (printLLVM_Arg argReg) + ", i64 0, i64 0\n"
        | StrObj1Ptr (argReg : LLVM_Arg) -> (printLLVM_Arg leftReg) + " = getelementptr %strobj* " + (printLLVM_Arg argReg) + ", i64 0, i64 1\n"
        | StrObj2Ptr (argReg : LLVM_Arg, index : int) -> (printLLVM_Arg leftReg) + " = getelementptr %strobj* " + (printLLVM_Arg argReg) + ", i64 0, i64 2, i64 " + (string index) + "\n"
        | Array0Ptr (argType : FieldType, argReg : LLVM_Arg) -> sprintf "argType=%O argReg=%O" argType argReg
        
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
        | GEP (getElementPtrFlavor : Flavor) -> sprintf "getElementPtrFlavor=%O" getElementPtrFlavor
        //%reg_42 = ptrtoint %strobj* %reg_38 to i64
        | PtrToInt (originalType : FieldType, originalReg : LLVM_Arg, resultType : FieldType) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "ptrtoint " + (printLLVM_Arg originalReg) + " " + (printFieldType resultType)
        //%reg_43 = or i64 %reg_42, 1. 
        | Or (theType : FieldType, argReg : LLVM_Arg, num : LLVM_Arg) -> "\t" + (printLLVM_Arg resultRegister) + " = " + "or " + (printFieldType theType) + " " + (printLLVM_Arg argReg) + " " + (printLLVM_Arg num)


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
        | Define (theType : FieldType, name : string, paramsList: Param list) -> "define " + (printFieldType theType) + " " + name + " " + (sprintf "%O" paramsList ) + " {"

(* Function that takes in an LLVM instruction list, and prints the string representation of each instruction. *)
let printLLVM instrList =
    printHead ()
    for eachInstr in instrList do
    printf "%O\n" (printLLVMLine eachInstr)

