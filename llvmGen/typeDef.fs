module TypeDef

type FieldType = 
     | F64
     | I1
     | I64
     | I64ptr
     | EFramePtr
     | EFramePtrPtr
     | CloPtr
     | CloPtrPtr
     | ArrayPtr
     | ArrayPtrPtr

// Anywhere you can use a register, you can also use a number
type LLVM_Arg =
     | Register of string
     | Number of int
       // A global label is something like @stuff
     | GlobalLabel of string

type ConditionCode =
     | Eq
     | Ne

type Param = (FieldType * string)

                        // could be a Register or a number
type Arg = (FieldType * LLVM_Arg)

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

and  RegProdInstr = 
       // Load is always a getelementptr of some flavor, followed by a load.
       // The flavor of getelementptr, with the LLVM_Arg (%r5 in my p.8 example), with the temp Register where the getelementptr result is stored into.
     | Load of (Flavor * FieldType * LLVM_Arg)
     | Add of (FieldType * LLVM_Arg * FieldType * LLVM_Arg)
       // Format is "call i64 (...)* @add_prim(i64 5, i64 2)"
     | Call of (FieldType * string * Arg list)
     | ICmp of (ConditionCode * FieldType * LLVM_Arg * LLVM_Arg)
       //Should look like ret i64 %r3
     

and  NonRegProdInstr = 
     | Return of (FieldType * LLVM_Arg)
       //should have a list of (FieldType * int)
//     | GetElementPtr of (FieldType * LLVM_Arg * FieldType * int * FieldType * int)
     // flavor is the kind of GEP we have, followed by two registers and their fieldtypes.
     // The data in first register is saved in memory at the address found in second register.
     | Store of (Flavor * FieldType * LLVM_Arg * FieldType * LLVM_Arg)
       // Br is made up of the i1 field to check (a LLVM_ARG), the label to go to if it's true, and the label to go to if it's false.
     | Br of (LLVM_Arg * string * string)
     | UnconditionalBr of (string)
       // Looks like ret i64 %r3
     | Ret of (FieldType * LLVM_Arg)

// These are all the different types of getelementptr's. Each of them has implicit field types + numbers.
and  Flavor =
       // The register it's being stored into, tupled with the register it's using, and possibly the int offset into the array if it's on 2
     | Eframe0 of (LLVM_Arg * LLVM_Arg)
     | Eframe1 of (LLVM_Arg * LLVM_Arg)
     | Eframe2 of (LLVM_Arg * LLVM_Arg * int)
     | Eframe0Ptr of (LLVM_Arg * LLVM_Arg)
     | Eframe1Ptr of (LLVM_Arg * LLVM_Arg)
     | Eframe2Ptr of (LLVM_Arg * LLVM_Arg * int)
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
