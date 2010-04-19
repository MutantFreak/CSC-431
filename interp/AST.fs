module AST

type exp =
    ID of string //works
    | BoolExp of bool //works
    | IntExp of int //works
    | DoubleExp of double //works 
    | StringExp of string //works
    | PrimExp of (prim * exp list)
    | IfExp of (exp * exp * exp) //works
    | WhileExp of (exp * exp) //works
    | LetExp of (string * exp * exp) //works. LetExp is Variable Binding on the assignment page
    | LetrecExp of (funbinding list * exp) // coded, but untested.
    | ReturnExp of exp // Coded, but untested. Throw an exception, and catch the exception from wherever the function was called
    | SetExp of (string * exp) //works. This is for updating a variable's value
    | BeginExp of (exp list) //works. This is a list of expressions, evaluated one at a time.
                             // The value returned is the result of evaulating the last item in the list.
    | FieldRefExp of (exp * string) // works. Coded, but untested. Retrieving fields of objects
    | FieldSetExp of (exp * string * exp) // works. Coded, but untested. Setting fields of objects
    | MethodCallExp of (exp * string * exp list) // Coded, but untested. Pretty much like Application, except you pull the function out of an ObjectV first. 
                                                 // The identifier 'this' is bound to the result of evaluating the 'object' expression
    | NewExp of (exp * exp list) // coded, but untested. Consists of a function expression and 0 or more argument expressions. First evaluate the exp, if it 
                                 // is not a closure, error. // If it is a closure create a new object with one slot, and bind the name 'constructor' to the 
                                 // closure. Then call the closure as described in Application, except there is one additional binding: the identifier 'this' 
                                 // is bound to the newly created object.
    | AppExp of (exp * exp list) // Coded, but untested. See Application. Evaluate the exp, if it is not a closure, error.
                                 // If it is a closure, evaluate the arguments list and put them into the closure's environment.
                                 // Then evaluate the body of the closure using the closure's newly updated environment.
    with override self.ToString () = (sprintf "%A" self)

and funbinding = string * string list * exp //Name of the function, its arguments, and its body exp.

and prim =
    AndP | OrP | NotP //works
    | TimesP | DivP | PlusP | MinusP | SqrtP //works
    | GTP | GEqP | LTP | LEqP //works
    | StringLengthP | StringAppendP | SubstringP | StringEqP | StringLTP //works
    | EqP //works
    | InstanceOfP //works
    | IsIntP | IsBoolP | IsDoubleP | IsVoidP | IsStringP | IsClosureP | IsArrayP | IsObjectP //works
    | PrintP | ReadLineP //works
    | NewArrayP | ArrayRefP | ArraySetP | ArrayMaxP
    with override self.ToString () = (sprintf "%A" self)
