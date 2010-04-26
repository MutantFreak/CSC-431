module AST

type exp =
    ID of string 
    | BoolExp of bool
    | IntExp of int
    | DoubleExp of double
    | StringExp of string
    | PrimExp of (prim * exp list)
    | IfExp of (exp * exp * exp)
    | WhileExp of (exp * exp)
    | LetExp of (string * exp * exp)
    | LetrecExp of (funbinding list * exp)
    | ReturnExp of exp
    | SetExp of (string * exp)
    | BeginExp of (exp list)
    | FieldRefExp of (exp * string)
    | FieldSetExp of (exp * string * exp)
    | MethodCallExp of (exp * string * exp list)
    | NewExp of (exp * exp list)
    | AppExp of (exp * exp list)
    with override self.ToString () = (sprintf "%A" self)

and funbinding = string * string list * exp //Name of the function, its arguments, and its body exp.

and prim =
    AndP | OrP | NotP
    | TimesP | DivP | PlusP | MinusP | SqrtP
    | GTP | GEqP | LTP | LEqP
    | StringLengthP | StringAppendP | SubstringP | StringEqP | StringLTP
    | EqP
    | InstanceOfP
    | IsIntP | IsBoolP | IsDoubleP | IsVoidP | IsStringP | IsClosureP | IsArrayP | IsObjectP
    | PrintP | ReadLineP
    | NewArrayP | ArrayRefP | ArraySetP | ArrayMaxP
    with override self.ToString () = (sprintf "%A" self)
