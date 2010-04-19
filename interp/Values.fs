module Values

open AST

type fVal =
    BoolV of bool
    | IntV of int
    | DoubleV of double
    | StringV of string
    | ClosureV of (string list * exp * env)
    | ObjectV of (slots ref)
    | ArrayV of ((fVal []) ref)
    | VoidV

and slots = Map<string,fVal ref>

and env = Map<string,fVal ref> 
