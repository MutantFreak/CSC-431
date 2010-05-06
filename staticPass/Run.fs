module Run

open AST
open StaticPass

printf "-------------------------------------\n\n"

//let testTree = (LetExp ("x", (IntExp 9), BeginExp ([LetExp("y", (IntExp 7), BeginExp ([(ID "y"); (ID "x")])); (ID "x")]) ) )
//let resultTree = walk testTree (buildNewFrame, [])

let testTree2 = (LetExp ("x", (IntExp 9), BeginExp ([WhileExp ((BoolExp true), BeginExp ([(ID "x"); WhileExp((BoolExp true), (ID "x"))] ) ); (ID "x")]) ) )
let resultTree2 = walk testTree2 (buildNewFrame, [])

//Should result in x being at frame 1 offset 0
//Should result in x being at frame 2 offset 0
//Should result in x being at frame 0 offset 0

(*
Expected output:
Found id "x" at frame: 1, offset 0
Found id "x" at frame: 2, offset 0
Found id "x" at frame: 0, offset 0
*)


printf "testTree = %O\n" testTree2
