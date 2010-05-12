module typedef

type FieldType = 
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

                          // could be a Register or a number
type Param = (FieldType * LLVM_Arg)

type Arg = (FieldType * Register)

type LLVM_Line =
     | Label of string
       // This is the register where the result is being stored, along with the producing instruction that makes the result.
     | RegProdLine of (Register * RegProdInstr)
       // These are non register-producing instructions, i.e. return, br, store, trunc, etc
     | NonRegProdLine of NonRegProdInstr
       // Format is "declare i64 @add_prim(...)"
     | Declare of (FieldType * GlobalLabel)
     | Define of (FieldType * GlobalLabel * Arg list)

type RegProdInstr = 
       // Load is always a getelementptr of some flavor, followed by a load.
       // The flavor of getelementptr, with the LLVM_Arg (%r5 in my p.8 example), with the temp Register where the getelementptr result is stored into.
     | Load of (Flavor * LLVM_Arg * Register)
     | Add of (FieldType * LLVM_Arg * FieldType * LLVM_Arg)
       // Format is "call i64 (...)* @add_prim(i64 5, i64 2)"
     | Call of (FieldType * GlobalLabel * Param list)

type NonRegProdInstr = 
     | Return of (FieldType * LLVM_Arg)
       //should have a list of (FieldType * int)
//     | GetElementPtr of (FieldType * LLVM_Arg * FieldType * int * FieldType * int)
     | Store of (FieldType * LLVM_Arg * LLVM_Arg)

// These are all the different types of getelementptr's
type Flavor = 
     | EframeParent
     | EFrameCount
     | EframeEntry of int
     | ClsoureTag
     | ClosureEnv
     | ClosureRefNum

(*
footle: 3 + 4
AST / AST2: PrimExp (PlusP, [Int 3, Int 4])
            => RegisterProducer (getFreshRegister, ())


add i64 %r3, i64 %r4
add i64 7, i64 9
*)