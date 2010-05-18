module AST2

type exp =
    ID of (string * int * int) // string for debugging. Name of the var, how many links to follow, and what index.
    | BoolExp of bool
    | IntExp of int
    | DoubleExp of int
    | StringExp of int // index into string table
    | PrimExp of (AST.prim * exp list)
    | IfExp of (exp * exp * exp)
    | WhileExp of (exp * exp) // 2nd exp must be a scopeExp
    | ReturnExp of exp
    | SetExp of ((string * int * int) * exp)
    | BeginExp of (exp list)
    | FieldRefExp of (exp * int) //Exp that should evaulate to an object, int offset into fieldNameTable
    | FieldSetExp of (exp * int * exp)
    | MethodCallExp of (exp * int * exp list)
    | NewExp of (exp * exp list)
    | AppExp of (exp * exp list)
    | CloExp of (string * int) // string for debugging + reference to index in the function table.
      //made for each function declaration
    | ScopeExp of (int * string list * exp) //how big is this particular environment frame, what are the fresh new arguments to this function (only non-empty when it's a function, and the body
    with override self.ToString () = (sprintf "%A" self)

// Name of the function, names of args, body, whether or not this function refers to the this pointer.
//things that refer to the this pointer can only be used as methods
type funEntry = string * string list * exp * bool

type doubleTable = Map<double,int>
type stringTable = Map<string,int>
type funTable = Map<funEntry,int>
type fieldNameTable = Map<string,int>

type sframe = (Map<string,int> * int ref)
type senv = (sframe * sframe list)

(***
Take whatever is on the very outside edge of the program, and call it a main function.

When we find a function declaration, make a funentry, put the funentry into the funtable.
In the function that the function declaration is being declared in, we do a SetExp, whose value is a CloExp.

this way any time in main it uses the id "f" it will get the ClosureExp called "f" from the funtable.

***)
