module AST2

type exp =
    ID of (string * int * int) // string for debugging
    | BoolExp of bool
    | IntExp of int
    | DoubleExp of int
    | StringExp of int
    | PrimExp of (AST.prim * exp list)
    | IfExp of (exp * exp * exp)
    | WhileExp of (exp * exp)
    | ReturnExp of exp
    | SetExp of ((string * int * int) * exp)
    | BeginExp of (exp list)
    | FieldRefExp of (exp * int)
    | FieldSetExp of (exp * int * exp)
    | MethodCallExp of (exp * string * exp list)
    | NewExp of (exp * exp list)
    | AppExp of (exp * exp list)
    | CloExp of (string * int) // string for debugging
    | ScopeExp of (int * string list * exp)
    with override self.ToString () = (sprintf "%A" self)

type funEntry = string * string list * exp * bool
 
type doubleTable = Map<double,int>

type stringTable = Map<string,int>

type funTable = Map<funEntry,int>

type fieldNameTable = Map<string,int>

type sframe = (Map<string,int> * int ref)

type senv = (sframe * sframe list)
