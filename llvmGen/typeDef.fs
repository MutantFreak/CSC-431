module TypeDef

type DefinedVar = 
     | Obj
     | Closure
     | Strobj
     | Floatobj
     | Eframe
     | Slots

type FieldType = 
     | Double
     | F64
     | I1
     | I8
     | I8Ptr
     | I8PtrPtr
     | I64
     | I64Ptr
     | EFramePtr
     | EFramePtrPtr
     | EFPtr_i64_Arr of (int)
     | EFPtr_i64Ptr_Arr of (int)
     | EFPtr_i64_Arr_Ptr of (int)
     | EFPtr_i64Ptr_Arr_Ptr of (int)
     | Closure
     | ClosurePtr
     | ClosurePtrPtr
     | ArrayPtr
     | ArrayPtrPtr
       // eg. [9 x i8] or [0 x i64]
     | Array of (int * FieldType)
     | StrObj
     | StrObjPtr
     | SlotsPtr
     | SlotsPtrPtr
     | Float
     | FloatPtr
     | FloatObj
     | FloatObjPtr
     | PackedArgsPtr
     | Void

// Anywhere you can use a register, you can also use a number
type LLVM_Arg =
     | Register of string
     | Number of int
       // A global label is something like @stuff
     | GlobalLabel of string
     | ActualNumber of int
     | Constant of string
       // Another ugly hack, this one is necessary to deal with floats
       // should look like: fptrunc(double 0x400921f9f01b866e to float). First FieldType = Double, string = 0x400921f9f01b866e, 2nd FieldType = float.
     | Fptrunc of (FieldType * string * FieldType)

type ConditionCode =
     | Eq
     | Ne

type Param = (FieldType * string)

                        // could be a Register or a number
type Arg = (FieldType * LLVM_Arg)

                 // The type of the number, the ActualNumber (offset number into function table for this function), and the label to go to for it
type Case = (FieldType * LLVM_Arg * LLVM_Arg)

type LLVM_Line =
     | Label of string
       // This is the register where the result is being stored, along with the producing instruction that makes the result.
     | RegProdLine of (LLVM_Arg * RegProdInstr)
       // These are non register-producing instructions, i.e. return, br, store, trunc, etc
     | NonRegProdLine of NonRegProdInstr
       // Format is "declare i64 @add_prim(...)"
       // FieldType with a GlobalLabel
     | Declare of (FieldType * string)
       //Fieldtype with a GlobalLabel, with a param list
     | Define of (FieldType * string * Param list)
     | CloseBracket
     | Unreachable

and  RegProdInstr = 
       // Load is always a getelementptr of some flavor, followed by a load.
       // The flavor of getelementptr, with the LLVM_Arg (%r5 in my p.8 example), with the temp Register where the getelementptr result is stored into.
     | Load of (Flavor * FieldType * LLVM_Arg)
     | Add of (FieldType * LLVM_Arg * LLVM_Arg)
       // Format is "call i64 (...)* @add_prim(i64 5, i64 2)"
     | Call of (FieldType * string * Arg list)
     | ICmp of (ConditionCode * FieldType * LLVM_Arg * LLVM_Arg)
     | Malloc of (FieldType)
     | Bitcast of (FieldType * LLVM_Arg * FieldType)
       // Ugly special case necessary for StringExp's, where there are two GEP lines in a row, one of which is not associated with a load / store.
       // example in variableCreation.s: %reg_41 = getelementptr %strobj* %reg_38, i32 0, i32 2
     | GEP of (Flavor)
       // The original type, the register we're converting, and the result type we want. Looks like: %reg_42 = ptrtoint %strobj* %reg_38 to i64
     | PtrToInt of (FieldType * LLVM_Arg * FieldType)
     | IntToPtr of (FieldType * LLVM_Arg * FieldType)
       // Used to put on tag bits. The second LLVM_Arg should be an ActualNumber. Looks like: %reg_43 = or i64 %reg_42, 1. 
     | Or of (FieldType * LLVM_Arg * LLVM_Arg)
     | And of (FieldType * LLVM_Arg * LLVM_Arg)
     

and  NonRegProdInstr = 
     // flavor is the kind of GEP we have, followed by two registers and their fieldtypes.
     // The data in first register is saved in memory at the address found in second register, with the temp Register where the getelementptr result is stored into.
     | Store of (Flavor * FieldType * LLVM_Arg * FieldType * LLVM_Arg * LLVM_Arg)
       // Br is made up of the i1 field to check (a LLVM_ARG), the label to go to if it's true, and the label to go to if it's false.
     | Br of (LLVM_Arg * string * string)
       // Looks like br %Label3. Used to unconditionally branch to %Label3
     | UnconditionalBr of (string)
       // Looks like ret i64 %r3
     | Ret of (FieldType * LLVM_Arg)
       // Yet ANOTHER ugly hack, due to the late-discovered fact that call can be either a register producing instruction, or a non-register producing instruction.
       // Looks like: call void @halt_with_error_int(i64 5,i64 %fun_val) nounwind noreturn
     | AloneCall of (FieldType * string * Arg list)
       // Looks like: switch i64 %reg_7, label %L_nomatch_34 [i64 4, label %L_jump_to_1_1 i64 0, label %L_jump_to_0_2 ]
       // The FieldType of the value being switched on, the value being switched, the fall through Label, and the Case list
     | Switch of (FieldType * LLVM_Arg * LLVM_Arg * Case list)

// These are all the different types of getelementptr's. Each of them has implicit field types + numbers based off their names.
and  Flavor =
       // The register it's using, and possibly the int offset into the array if it's on 2
     | Eframe0 of (LLVM_Arg)
     | Eframe1 of (LLVM_Arg)
     | Eframe2 of (LLVM_Arg * int)
     | Eframe0Ptr of (LLVM_Arg)
     | Eframe1Ptr of (LLVM_Arg)
     | Eframe2Ptr of (LLVM_Arg * int)
     | StrObj0Ptr of (LLVM_Arg)
     | StrObj1Ptr of (LLVM_Arg)
     | StrObj2Ptr of (LLVM_Arg)
       // something like: getelementptr([9 x i8]* @stringconst_0s, i64 0, i64 0). "[9 x i8]" is the Array FieldType. @stringconst_0s is the LLVM_Arg
     | Array0Ptr of (FieldType * LLVM_Arg)
     | FloatObj0Ptr of (LLVM_Arg)
     | FloatObj1Ptr of (LLVM_Arg)
     | FloatObj2Ptr of (LLVM_Arg * int)
     | Closure0Ptr of (LLVM_Arg)
     | Closure1Ptr of (LLVM_Arg)
     | Closure2Ptr of (LLVM_Arg)
     | PackedArgs0Ptr of (LLVM_Arg)
(*
     | EframeParent
     | EFrameCount
     | EframeEntry of int
     | ClsoureTag
     | ClosureEnv
     | ClosureRefNum
*)
(*
footle: 3 + 4
AST / AST2: PrimExp (PlusP, [Int 3, Int 4])
            => RegisterProducer (getFreshRegister, ())


add i64 %r3, i64 %r4
add i64 7, i64 9
*)
