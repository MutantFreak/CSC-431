module Interp

open AST
open Values
open Xunit
open FsxUnit.Syntax
open System

exception Unimplemented of string
exception RuntimeError of string
exception ReturnException of fVal

// Global ref cell to store the results from return statements
//let returnValue = ref VoidV;

// Match either IntV or DoubleV as double
let (|AsDouble|) input =
    match input with
        | IntV i -> (double i)
        | DoubleV d -> d
        | _ -> raise (RuntimeError (sprintf "expected number, given: %A" input))

// lift a binary primitive in (num num -> num) to one on fVals
let numToNumPrim (p1,p2) args =
    match args with
        | [IntV i1; IntV i2] -> IntV (p1 i1 i2)
        | [(AsDouble d1);(AsDouble d2)] -> DoubleV (p2 d1 d2)
        | _ -> raise (RuntimeError (sprintf "expected two arguments in numToNumPrim, given: %A" args))

// Evaluate a square root operator. Requires some combination of two ints and doubles.
let evalSqrt args =
    match args with
        | [IntV i1] -> DoubleV (sqrt (double i1))
        | [DoubleV d1] -> DoubleV (sqrt (d1))
        | _ -> raise (RuntimeError (sprintf "expected one  int or double argument in evalSqrt, given: %A" args))

// Evaluate a logical and operator. Requires two booleans.
let evalAnd args =
    match args with
        | [BoolV b1; BoolV b2] -> BoolV (b1 && b2)
        | _ -> raise (RuntimeError (sprintf "expected two bool arguments in evalAnd, given: %A" args))

// Evaluate a logical or operator. Requires two booleans.
let evalOr args =
    match args with
        | [BoolV b1; BoolV b2] -> BoolV (b1 || b2)
        | _ -> raise (RuntimeError (sprintf "expected two bool arguments in evalOr, given: %A" args))

// Evaluate a logical not operator. Requires one boolean.
let evalNot args =
    match args with
        | [BoolV b1] -> BoolV (not b1)
        | _ -> raise (RuntimeError (sprintf "expected one bool argument in evalNot, given: %A" args))

let evalStringLength args =
    match args with
        | [StringV s] -> IntV (String.length s)
        | _ -> raise (RuntimeError (sprintf "expected one string argument in evalStringLength, given: %A" args))

let evalStringAppend args =
    match args with
        | [StringV s1; StringV s2] -> StringV (s1+s2)
        | _ -> raise (RuntimeError (sprintf "expected two string arguments in evalStringAppend, given: %A" args))

let evalSubstring args =
    match args with
        | [StringV s; IntV i1; IntV i2] -> StringV ( s.[i1..i2] )
        | _ -> raise (RuntimeError (sprintf "expected three arguments (string, int, int) in evalSubstring, given: %A" args))

let evalStringEq args =
    match args with
        | [StringV s1; StringV s2] -> BoolV (s1 = s2)
        | _ -> raise (RuntimeError (sprintf "expected two string arguments in evalStringEq, given: %A" args))

let evalStringLT args =
    match args with
        | [StringV s1; StringV s2] -> BoolV (s1 < s2)
        | _ -> raise (RuntimeError (sprintf "expected two string arguments in evalStringLT, given: %A" args))

// Evaluate a greater than operator (>)
let evalGT args =
    match args with
        | [IntV i1; IntV i2] -> if(i1 > i2)
                                then BoolV true
                                else BoolV false
        | [(AsDouble d1);(AsDouble d2)] -> if(d1 > d2)
                                           then BoolV true
                                           else BoolV false
        | _ -> raise (RuntimeError (sprintf "expected two int or double arguments in evalGT, given: %A" args))

// Evaluate a greater than or equal to operator (>=)
let evalGEq args =
    match args with
        | [IntV i1; IntV i2] -> if(i1 >= i2)
                                then BoolV true
                                else BoolV false
        | [(AsDouble d1);(AsDouble d2)] -> if(d1 >= d2)
                                           then BoolV true
                                           else BoolV false
        | _ -> raise (RuntimeError (sprintf "expected two int or double arguments in evalGEq, given: %A" args))

// Evaluate a less than or equal to operator (<=)
let evalLE args =
    match args with
        | [IntV i1; IntV i2] -> if(i1 < i2)
                                then BoolV true
                                else BoolV false
        | [(AsDouble d1);(AsDouble d2)] -> if(d1 < d2)
                                           then BoolV true
                                           else BoolV false
        | _ -> raise (RuntimeError (sprintf "expected two int or double arguments in evalLE, given: %A" args))

// Evaluate an equivalency operator (==)
let evalLEq args =
    match args with
        | [IntV i1; IntV i2] -> if(i1 <= i2)
                                then BoolV true
                                else BoolV false
        | [(AsDouble d1);(AsDouble d2)] -> if(d1 <= d2)
                                           then BoolV true
                                           else BoolV false
        | _ -> raise (RuntimeError (sprintf "expected two int or double arguments in evalLEq, given: %A" args))

// Evaluate the equality of two ints, doubles, bools, voids, strings, closures, or objects
let evalEq args =
    match args with
        | [IntV i1; IntV i2] -> if(i1 = i2)
                                then BoolV true
                                else BoolV false
        | [DoubleV d1; DoubleV d2] -> if(d1 = d2)
                                      then BoolV true
                                      else BoolV false
        | [BoolV b1; BoolV b2] -> if(b1 = b2)
                                  then BoolV true
                                  else BoolV false
        | [VoidV; VoidV] -> BoolV true
        | [StringV s1; StringV s2] -> if(s1 = s2)
                                      then BoolV true
                                      else BoolV false
        | [ClosureV (sList1, exp1, env1); ClosureV (sList2, exp2, env2)] -> if ((sList1 = sList2) && (exp1 = exp2) && (env1 = env2))
                                                                            then BoolV true
                                                                            else BoolV false
        | [ObjectV o1; ObjectV o2] -> if(o1 = o2)
                                      then BoolV true
                                      else BoolV false
        // This is some unrecognized pair of objects, so return a BoolV false
        | _ -> BoolV false

// Returns true if the list of values passed in contains one IntV, false otherwise
let evalIsInt args =
    match args with
        | [IntV i] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one BoolV, false otherwise
let evalIsBool args =
    match args with
        | [BoolV b] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one DoubleV, false otherwise
let evalIsDouble args =
    match args with
        | [DoubleV d] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one VoidV, false otherwise
let evalIsVoid args =
    match args with
        | [VoidV] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one StringV, false otherwise
let evalIsString args =
    match args with
        | [StringV s] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one ClosureV, false otherwise
let evalIsClosure args =
    match args with
        | [ClosureV c] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one ArrayV, false otherwise
let evalIsArray args =
    match args with
        | [ArrayV a] -> BoolV (true)
        | _ -> BoolV (false)

// Returns true if the list of values passed in contains one ObjectV, false otherwise
let evalIsObject args =
    match args with
        | [ObjectV o] -> BoolV (true)
        | _ -> BoolV (false)

// Prints the given values
let evalPrint args =
    match args with
        | [BoolV b] -> (printf "BoolV: %A\n" b) |> ignore
                       VoidV
        | [IntV i] -> (printf "IntV: %A\n" i) |> ignore
                      VoidV
        | [DoubleV d] -> (printf "DoubleV: %A\n" d) |> ignore
                         VoidV
        | [StringV s] -> (printf "StringV: %A\n" s) |> ignore
                         VoidV
        | [ClosureV c] -> (printf "ClosureV: %A\n" c) |> ignore
                          VoidV
        | [ObjectV o] -> (printf "ObjectsV: %A\n" o) |> ignore
                         VoidV
        | [VoidV] -> (printf "VoidV\n" ) |> ignore
                     VoidV
        | [ArrayV a] -> (printf "ArrayV: %A\n" a) |> ignore
                        VoidV  
        | _ -> raise (RuntimeError (sprintf "Expected a single fVal, given: %A" args))

// Reads a line and returns it
let evalReadLine args =
    let s = Console.ReadLine()
    StringV (s + "\n")

// Reads a line and returns it
let evalInstanceOf args =
    match args with
        | [ObjectV o; x : fVal ] -> if((!o).ContainsKey "constructor")
                                    then let theConstructor = !(!o).["constructor"]
                                         if(theConstructor = x)
                                         then (BoolV true)
                                         else (BoolV false)
                                    else raise (RuntimeError (sprintf "the ObjectV doesn't have a constructor"))
        | _ -> raise (RuntimeError (sprintf "Expected a single fVal, given: %A" args))

// Creates and returns a new ArrayV
let evalNewArray args =
    ArrayV (ref [| |])

// Returns the value at the specified index int the ArrayV. Raises and error if the index was out of bounds.
let evalArrayRef args =
    match args with
        | [ArrayV valArr; IntV num] -> if (num < (!valArr).Length)
                                        then (!valArr).[num]
                                        else raise (RuntimeError (sprintf "Attempting to reference an index out of bounds: %A" num))
        | _ -> raise (RuntimeError (sprintf "Expected an array and index when evaluating an array reference, given: %A" args))

// Used in evalArraySet to create a list of VoidV's
let rec consVoid newList num =
    if (num > 0)
    then consVoid (VoidV::newList) (num-1)
    else newList


// Helper function for evalArraySet, which takes a list, and index, and an fVal, and gives back a 
// different fVal list which has the index equal to the new fVal. curPos is how many times we've recursed, starting with 0.
// newList is the list being built that will eventually be returned.
let rec evalArraySetHelper fList curPos index newVal newList =
    if (List.isEmpty fList)
    then newList
            // If we're at the position where we need to do the swap
    else if (curPos = index)
              // Then append a list with just the new value in it.
         then let brandNewList = List.append newList [newVal]
              evalArraySetHelper fList.Tail (curPos+1) index newVal brandNewList
              // Otherwise, append the head of the old to the end of the new one
         else let brandNewList = List.append newList [fList.Head]
              evalArraySetHelper fList.Tail (curPos+1) index newVal brandNewList

// Adds the specified value to the ArrayV at the given index. If the index is greater than the length of the array, it
// increases the size of the array appropriately.
let evalArraySet args =
    match args with
                                                           // If the index is within bounds, set that index to the given value.
        | [ArrayV valArr; IntV index; value : fVal] -> if (index < (!valArr).Length)
                                                             // Set index in valArr to be value.
                                                        then let newList = evalArraySetHelper (List.ofArray (!valArr)) 0 index value []
                                                             ArrayV (ref (Array.ofList newList))
                                                             // Otherwise, figure out how far off it is, and cons a new list filled with VoidV's, and ending in that element onto the existing list.
                                                        else let diff = index - (!valArr).Length
                                                             // Get a list of VoidV's of length diff-1
                                                             let newList = Array.ofList (consVoid [] diff)
                                                             let brandNewList = Array.append (Array.append !valArr newList) [|value|]
                                                             ArrayV (ref brandNewList)
        | _ -> raise (RuntimeError (sprintf "Expected an ArrayV, index, and fVal when evaluating an array reference, given: %A" args))

// Returns the length of the ArrayV.
let evalArrayMax args =
    match args with
        | [ArrayV valArr] -> IntV (!valArr).Length
        | _ -> raise (RuntimeError (sprintf "Expected an array when getting array length, given: %A" args))

// given a prim, produce the corresponding function on fVals
// This function is still expecting an args list made of fVal's.
let primFun p =
    match p with
        | PlusP -> numToNumPrim ((+),(+))
        | MinusP -> numToNumPrim ((-),(-))
// +++ Added TimesP, DivP and SqrtP
        | TimesP -> numToNumPrim ((*),(*))
        | DivP -> numToNumPrim ((/),(/))
        | SqrtP -> evalSqrt
        | AndP -> evalAnd
        | OrP -> evalOr
        | NotP -> evalNot
        | StringLengthP -> evalStringLength
        | StringAppendP -> evalStringAppend
        | SubstringP -> evalSubstring
        | StringEqP -> evalStringEq
        | StringLTP -> evalStringLT
        | GTP -> evalGT
        | GEqP -> evalGEq
        | LTP -> evalLE
        | LEqP -> evalLEq
        | EqP -> evalEq
        | IsIntP -> evalIsInt
        | IsBoolP -> evalIsBool
        | IsDoubleP -> evalIsDouble
        | IsVoidP -> evalIsVoid
        | IsStringP -> evalIsString
        | IsClosureP -> evalIsClosure
        | IsArrayP -> evalIsArray
        | IsObjectP -> evalIsObject
        | PrintP -> evalPrint
        | ReadLineP -> evalReadLine
        | InstanceOfP -> evalInstanceOf
        | NewArrayP -> evalNewArray
        | ArrayRefP -> evalArrayRef
        | ArraySetP -> evalArraySet
        | ArrayMaxP -> evalArrayMax

// Helper function to extend the environment with new bindings for each function name, using a bogus VoidV
let rec letrecExpHelper (myEnv : env) (myList : funbinding list) =
           // If we're out of function bindings, then we're done, so return the built up environment
        if (List.isEmpty myList)
        then myEnv
        else match myList.Head with
                   //Name of the function, its arguments, and its body exp.
                 | (funName, paramsList, bodyExp) -> let newEnv = myEnv.Add (funName, ref VoidV)
                                                     letrecExpHelper newEnv myList.Tail

// create closureV from each of the funbindings and point to it in the given env, then return the env
let rec letrecExpHelper2 (myEnv : env) (myList : funbinding list) =
           // If we're out of function bindings, then we're done, so return the built up environment
        if (List.isEmpty myList)
        then myEnv
        else match myList.Head with
                   //Name of the function, its arguments, and its body exp. ClosureV is (string list * exp * env)
                 | (funName, paramsList, bodyExp) -> myEnv.[funName] := ClosureV (paramsList, bodyExp, myEnv)
                                                     letrecExpHelper myEnv myList.Tail

let createNewObj =
    let (s : slots) = Map.empty<string,fVal ref>
    ObjectV(ref s)


// Evaluate an expression in the given environment
let rec eval (env : env) exp =
    let recur = eval env
    match exp with
        | IntExp n -> IntV n
        | DoubleExp d -> DoubleV d
// +++ Added Bool and String
        | BoolExp b -> BoolV b
        | StringExp s -> StringV s
        // Look up the fVal associated with this name in this environment.
        | ID name -> !env.[name]
        | LetExp (name,e1,e2) -> let newEnv = (env.Add (name,(ref (eval env e1))))
                                 eval newEnv e2
        | SetExp (name,e1) -> let newVal = (eval env e1)
                              env.[name] := newVal
                              newVal
        | BeginExp exps -> if(exps.Length = 0)
                           then VoidV
                           else
                           let newList = (List.map recur exps)
                           newList.Item (newList.Length - 1)
// Evaluate each of the args given the environment env. Turns args from list of Exp's to list of fVal's
// For example, call primFun with PlusP and [IntV 4; IntV 5]
        | PrimExp (p,args) -> primFun p (List.map recur args)
        | IfExp (guardExp, thenExp, elseExp) -> if(eval env guardExp = BoolV true)
                                                  then eval env thenExp
                                                  else eval env elseExp
        | WhileExp (guardExp, doThisExp) -> evalWhileLoop env guardExp doThisExp
          // Evaluate the objExp, make sure it is an object
        | FieldSetExp (objExp, name, valueExp) -> let theObj = eval env objExp
                                                  let value = eval env valueExp
                                                  match theObj with
                                                        // If the field exists in the object, update its value
                                                      | ObjectV (objSlots) -> if((!objSlots).ContainsKey name)
                                                                              then (!objSlots).[name] := value
                                                                                   value
                                                        // Otherwise create this field in the object & update its value
                                                                              else let brandNewSlots = (!objSlots).Add (name, ref value)
                                                                                   objSlots := brandNewSlots
                                                                                   value
                                                      | _ -> raise (RuntimeError (sprintf "Expected ObjectV when mutating a field, given: %A" theObj))
        | FieldRefExp (objExp, name) -> let theObj = eval env objExp
                                        match theObj with
                                            | ObjectV (objSlots) -> !(!objSlots).[name]
                                            | _ -> raise (RuntimeError (sprintf "Expected ObjectV when referencing a field, given: %A" theObj))
          // Evaluate the objExp, make sure it is a ObjectV. Name is the name of the method to call inside the ObjectV.
        | MethodCallExp (objExp, name, argsList) -> let theObj = eval env objExp
                                                    match theObj with
                                                        | ObjectV (objSlots) -> if((!objSlots).ContainsKey name)
                                                                                then let theMethodClosure = !(!objSlots).[name]
                                                                                     match theMethodClosure with
                                                                                         | ClosureV (parameters, bodyExp, internalEnv) -> internalEnv.Add ("this", ref theObj) |> ignore
                                                                                                                                          let newInternalEnv = addLocalVars internalEnv argsList parameters
                                                                                                                                          eval newInternalEnv bodyExp
                                                                                         | _ -> raise (RuntimeError (sprintf "%A was something other than a method inside the given object." name))
                                                                                     // Raise an error b/c objSlots did not have the method we were trying to call
                                                                                else raise (RuntimeError (sprintf "ObjectV did not contain the method called: %A" name))
                                                        | _ -> raise (RuntimeError (sprintf "Expected ObjectV when performing a method call, given: %A" theObj))
          // Make sure that the Object's slots contains the key name
          // If the value in the slot when looking up name is not a closure, raise an error

          // Evaluate the funExp, make sure it is a ClosureV
        | NewExp (funExp, argsList) -> let theClosure = eval env funExp
                                       match theClosure with
                                           | ClosureV (parameters, bodyExp, internalEnv) -> let brandNewObj = createNewObj
                                                                                            match brandNewObj with
                                                                                                | ObjectV (fields) -> let newFields = (!fields).Add ("constructor", ref theClosure)
                                                                                                                      let newIntEnv = internalEnv.Add ("this", ref brandNewObj)
                                                                                                                      let newInternalEnv = addLocalVars newIntEnv argsList parameters
                                                                                                                      eval newInternalEnv bodyExp
                                                                                                | _ -> raise (RuntimeError (sprintf "ZOMG SOMETHING WENT TERRIBLY WRONG!!!"))
                                           | _ -> raise (RuntimeError (sprintf "Expected ClosureV when using newExp, given: %A" theClosure))
          // Evaluate the closureExp, make sure it's a closure
        | AppExp (closureExp, argsList) -> let theClosure = eval env closureExp
                                           match theClosure with
                                               | ClosureV (parameters, bodyExp, internalEnv) -> let newInternalEnv = addLocalVars internalEnv argsList parameters
                                                                                                eval newInternalEnv bodyExp
                                               | _ -> raise (RuntimeError (sprintf "Expected ClosureV when using newExp, given: %A" theClosure))
          // Evaluate the LetrecExp. Extend the environment with new bindings for the new function names, initially with dummy VoidV's.
        | LetrecExp (funBindingList, bodyExp) -> let builtEnv = letrecExpHelper env funBindingList
                                                 // then create ClouresV for each function, and replace the dummy variables with it
                                                 let builtEnv2 = letrecExpHelper2 builtEnv funBindingList
                                                 eval builtEnv2 bodyExp
                                                 
          // Evaluate the argExp to find out what value should be returned, then raise an exception with that value.
        | ReturnExp (argExp) -> let trueRef = (ref VoidV)
                                try
                                    trueRef := eval env argExp
                                with
                                    | ReturnException(trueRef) -> raise (RuntimeError (sprintf "Found nexted Return statement. Breaking"))
                                raise (ReturnException (!trueRef))
        //| _ -> raise (Unimplemented (sprintf "other operations not implemented yet: %A" exp))
// Function to evaluate while loops.
// If the result of evaluating the guard expression is not a bool, raises an error.
and evalWhileLoop env guardExp doThisExp =
    let guardResult = eval env guardExp
    match guardResult with
        | BoolV true -> eval env doThisExp |> ignore
                        evalWhileLoop env guardExp doThisExp
        | BoolV false -> VoidV
        | _ -> raise (RuntimeError (sprintf "Expected boolean condition in while loop, given: %A" guardResult))
// Helper function to evaluate each expression, and store each result using each name into theEnv
and addLocalVars theEnv expressions names =
    if((expressions = [] &&  names <> []) || (names = [] && expressions <> []))
    then raise (RuntimeError (sprintf "Adding local variables, ran out of either parameters or arguments: %A" expressions))
    else 
         if (expressions = [] && names = [])
         then theEnv
         else let newVal = eval theEnv expressions.Head
              let brandNewEnv = theEnv.Add (names.Head, ref newVal)
              addLocalVars brandNewEnv expressions names

// the empty environment
let (emptyEnv : env) = Map.ofList[]

// eval with the empty environment
// Returns a function that expects an expression. It will evaluate that expression given the empty environment just defined.
let evalMT = eval emptyEnv

// The master interp function - evaluates a given expression in a given environment, and returns the fVal it evaluates to.
// Catches ReturnException thrown by returnExp, so that the program doesn't crash when it has a returnExp floating around
// not inside of a function.
let interp environment expression =
    try
       eval environment expression
    with
           // Return the result of the whole program - the global returnValue that was stored by returnExp
       | ReturnException(retVal) -> retVal

// TEST CASES (INCOMPLETE!)
[<Fact>]
let valTests () =
    should equal (DoubleV 3.4) (evalMT (DoubleExp 3.4))
    should equal (IntV 124) (evalMT (IntExp 124))
// +++ Added
    should equal (BoolV true) (evalMT (BoolExp true))
    should equal (BoolV false) (evalMT (BoolExp false))
    should equal (StringV "I like cats.") (evalMT (StringExp "I like cats."))
    should equal (VoidV) (evalMT (BeginExp [] ))
//  This used to fail before StringV's were implemented
//    should (throw_exception<Unimplemented>) (fun () -> (ignore (evalMT (StringExp "abc"))))


// Eventually calls numToNumPrim with ((+),(+)) [IntV 4; IntV 5] as parameters.
// This is RPN essentially, and evaluates to "val it : int = 7" in the repl.


[<Fact>]
let primTestPlusP () =
    should equal (IntV 9) (evalMT (PrimExp(PlusP,[IntExp 4;IntExp 5])))
    should equal (DoubleV 9.0) (evalMT (PrimExp(PlusP,[DoubleExp 4.0;IntExp 5])))
    should equal (IntV 15) (evalMT (PrimExp(PlusP,[PrimExp(PlusP,[IntExp 4;IntExp 5]);IntExp 6])))

[<Fact>]
let primTestMinusP () =
    should equal (IntV -1) (evalMT (PrimExp(MinusP,[IntExp 4;IntExp 5])))
    should equal (DoubleV -1.0) (evalMT (PrimExp(MinusP,[DoubleExp 4.0;IntExp 5])))
    should equal (IntV 1) (evalMT (PrimExp(MinusP,[PrimExp(MinusP,[IntExp 10;IntExp 5]);IntExp 4])))

[<Fact>]
let primTestTimesP () =
    should equal (IntV 6) (evalMT (PrimExp(TimesP,[IntExp 2; IntExp 3])))
    should equal (DoubleV 12.0) (evalMT (PrimExp(TimesP,[DoubleExp 4.0; DoubleExp 3.0])))
    should equal (DoubleV 14.0) (evalMT (PrimExp(TimesP,[IntExp 7; DoubleExp 2.0])))
    should equal (DoubleV 2.0) (evalMT (PrimExp(TimesP,[DoubleExp 1.0; IntExp 2])))
    should equal (IntV 2) (evalMT (PrimExp(DivP,[IntExp 4; IntExp 2])))

[<Fact>]
let primTestDivP () =
    should equal (DoubleV 4.0) (evalMT (PrimExp(DivP,[DoubleExp 12.0; DoubleExp 3.0])))
    should equal (DoubleV 2.0) (evalMT (PrimExp(DivP,[IntExp 6; DoubleExp 3.0])))
    should equal (DoubleV 1.0) (evalMT (PrimExp(DivP,[DoubleExp 2.0; IntExp 2])))

[<Fact>]
let primTestSqrtP () =
    should equal (DoubleV 3.0) (evalMT (PrimExp(SqrtP, [IntExp 9])))
    should equal (DoubleV 4.0) (evalMT (PrimExp(SqrtP, [DoubleExp 16.0])))

[<Fact>]
let primTestAndP () =
    should equal (BoolV true) (evalMT (PrimExp(AndP, [BoolExp true; BoolExp true])))
    should equal (BoolV false) (evalMT (PrimExp(AndP, [BoolExp true; BoolExp false])))
    should equal (BoolV false) (evalMT (PrimExp(AndP, [BoolExp false; BoolExp false])))

[<Fact>]
let primTestOrP () =
    should equal (BoolV true) (evalMT (PrimExp(OrP, [BoolExp true; BoolExp true])))
    should equal (BoolV true) (evalMT (PrimExp(OrP, [BoolExp true; BoolExp false])))
    should equal (BoolV false) (evalMT (PrimExp(OrP, [BoolExp false; BoolExp false])))

[<Fact>]
let primTestNotP () =
    should equal (BoolV false) (evalMT (PrimExp(NotP, [BoolExp true])))
    should equal (BoolV true) (evalMT (PrimExp(NotP, [BoolExp false])))

[<Fact>]
let primTestStringLengthP () =
    should equal (IntV 0) (evalMT (PrimExp(StringLengthP, [StringExp ""])))
    should equal (IntV 5) (evalMT (PrimExp(StringLengthP, [StringExp "abcde"])))
    should equal (IntV 8) (evalMT (PrimExp(StringLengthP, [StringExp "hf dd gl"])))

[<Fact>]
let primTestStringAppendP () =
    should equal (StringV "") (evalMT (PrimExp(StringAppendP, [StringExp ""; StringExp ""])))
    should equal (StringV "start") (evalMT (PrimExp(StringAppendP, [StringExp "start"; StringExp ""])))
    should equal (StringV "end") (evalMT (PrimExp(StringAppendP, [StringExp ""; StringExp "end"])))
    should equal (StringV "moof") (evalMT (PrimExp(StringAppendP, [StringExp "moo"; StringExp "f"])))
    should equal (StringV "abcdef") (evalMT (PrimExp(StringAppendP, [StringExp "abc"; StringExp "def"])))

[<Fact>]
let primTestSubstringP () =
    should equal (StringV "abc") (evalMT (PrimExp(SubstringP, [StringExp "abc"; IntExp 0; IntExp 2])))
    should equal (StringV "aaa") (evalMT (PrimExp(SubstringP, [StringExp "--aaa--"; IntExp 2; IntExp 4])))

[<Fact>]
let primTestStringEqP () =
    should equal (BoolV true) (evalMT (PrimExp(StringEqP, [StringExp "abc"; StringExp "abc"])))
    should equal (BoolV false) (evalMT (PrimExp(StringEqP, [StringExp "abc"; StringExp "def"])))

[<Fact>]
let primTestGTP () =
    should equal (BoolV true) (evalMT (PrimExp(GTP, [IntExp 4; IntExp 3])))
    should equal (BoolV false) (evalMT (PrimExp(GTP, [IntExp 2; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(GTP, [IntExp 4; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(GTP, [DoubleExp 2.0; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(GTP, [DoubleExp 4.0; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(GTP, [DoubleExp 2.0; DoubleExp 3.0])))

[<Fact>]
let primTestStringLT () =
    should equal (BoolV true) (evalMT (PrimExp(StringLTP, [StringExp "aaaa"; StringExp "zzzz"])))
    should equal (BoolV true) (evalMT (PrimExp(StringLTP, [StringExp "abc"; StringExp "def"])))
    should equal (BoolV true) (evalMT (PrimExp(StringLTP, [StringExp "aaaa"; StringExp "abc"])))
    should equal (BoolV false) (evalMT (PrimExp(StringLTP, [StringExp "Ryuho"; StringExp "Bob"])))

[<Fact>]
let primTestGEqP () =
    should equal (BoolV true) (evalMT (PrimExp(GEqP, [IntExp 4; IntExp 4])))
    should equal (BoolV false) (evalMT (PrimExp(GEqP, [IntExp 2; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(GEqP, [IntExp 4; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(GEqP, [DoubleExp 2.0; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(GEqP, [DoubleExp 4.0; DoubleExp 3.0])))
    should equal (BoolV true) (evalMT (PrimExp(GEqP, [DoubleExp 4.0; DoubleExp 4.0])))
    should equal (BoolV false) (evalMT (PrimExp(GEqP, [DoubleExp 2.0; DoubleExp 3.0])))

[<Fact>]
let primTestLTP () =
    should equal (BoolV true) (evalMT (PrimExp(LTP, [IntExp 3; IntExp 4])))
    should equal (BoolV false) (evalMT (PrimExp(LTP, [IntExp 3; IntExp 2])))
    should equal (BoolV true) (evalMT (PrimExp(LTP, [IntExp 2; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(LTP, [DoubleExp 4.0; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(LTP, [DoubleExp 3.0; DoubleExp 4.0])))
    should equal (BoolV false) (evalMT (PrimExp(LTP, [DoubleExp 3.0; DoubleExp 3.0])))

[<Fact>]
let primTestLEqP () =
    should equal (BoolV true) (evalMT (PrimExp(LEqP, [IntExp 4; IntExp 4])))
    should equal (BoolV false) (evalMT (PrimExp(LEqP, [IntExp 4; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(LEqP, [IntExp 3; DoubleExp 4.0])))
    should equal (BoolV false) (evalMT (PrimExp(LEqP, [DoubleExp 4.0; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(LEqP, [DoubleExp 3.0; DoubleExp 3.0])))
    should equal (BoolV true) (evalMT (PrimExp(LEqP, [DoubleExp 2.0; DoubleExp 4.0])))
    should equal (BoolV false) (evalMT (PrimExp(LEqP, [DoubleExp 4.0; DoubleExp 3.0])))

[<Fact>]
let primTestEqP () =
    should equal (BoolV true) (evalMT (PrimExp(EqP, [IntExp 2; IntExp 2])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [IntExp 2; IntExp 3])))
    should equal (BoolV true) (evalMT (PrimExp(EqP, [DoubleExp 2.0; DoubleExp 2.0])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [DoubleExp 2.0; DoubleExp 3.0])))
    should equal (BoolV true) (evalMT (PrimExp(EqP, [BoolExp true; BoolExp true])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [BoolExp true; BoolExp false])))
    should equal (BoolV true) (evalMT (PrimExp(EqP, [StringExp "cat"; StringExp "cat"])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [StringExp "cat"; StringExp "dog"])))
    // Test pairs of different types, should all return false
    should equal (BoolV false) (evalMT (PrimExp(EqP, [IntExp 2; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [StringExp "abc"; DoubleExp 3.0])))
    should equal (BoolV false) (evalMT (PrimExp(EqP, [BoolExp true; IntExp 2])))
    // TODO: Add test cases for testing ClosureV and ObjectV and ArrayV equality

[<Fact>]
let primTestID () =
    // Tests ID and LetExp: Creates a new variable called x using LetExp,
    // and have x exist in one more expression where it reads out x.
    should equal (IntV 9) (evalMT (LetExp ("x", (IntExp 9), (ID "x")) ))
    should equal (IntV 4) (evalMT (LetExp ("cats", (PrimExp(PlusP,[IntExp 3; IntExp 1])), (ID "cats")) ))

[<Fact>]
let primTestSetExp () =
    // Tests SetExp
    should equal (IntV 10) (evalMT (LetExp ("x", (IntExp 9), BeginExp [(SetExp ("x", (IntExp 10))); ID "x"]) ))
    should equal (IntV 4) (evalMT (LetExp ("cats", (PrimExp(PlusP,[IntExp 3; IntExp 1])), (ID "cats")) ))

[<Fact>]
let primTestBeginExp () =
    // Tests BeginExp
    should equal (IntV 9) (evalMT (BeginExp [(IntExp 6);(IntExp 7);(IntExp 8);(IntExp 9)]))
    should equal (IntV 4) (evalMT (BeginExp [(IntExp 6);(IntExp 7);(IntExp 8);(PrimExp(PlusP,[IntExp 3; IntExp 1]))]))
    should equal (IntV 4) (evalMT (BeginExp [ LetExp ("cats", (PrimExp(PlusP,[IntExp 3; IntExp 1])), (ID "cats")) ]))

[<Fact>]
let primTestIsX () =
    should equal (BoolV true) (evalMT (PrimExp(IsIntP, [IntExp 5])))
    should equal (BoolV false) (evalMT (PrimExp(IsIntP, [BoolExp false])))
    should equal (BoolV true) (evalMT (PrimExp(IsBoolP, [BoolExp true])))
    should equal (BoolV false) (evalMT (PrimExp(IsBoolP, [IntExp 5])))
    should equal (BoolV true) (evalMT (PrimExp(IsDoubleP, [DoubleExp 9.0])))
    should equal (BoolV false) (evalMT (PrimExp(IsDoubleP, [IntExp 5])))
    should equal (BoolV true) (evalMT (PrimExp(IsStringP, [StringExp "x"])))
    should equal (BoolV false) (evalMT (PrimExp(IsStringP, [IntExp 5])))
    should equal (BoolV true) (evalMT (PrimExp(IsVoidP, [BeginExp []] )))
    should equal (BoolV false) (evalMT (PrimExp(IsVoidP, [IntExp 5] )))
    //TODO actually test these
    //should equal (BoolV true) (evalMT (PrimExp(IsClosureP, [] )))
    //should equal (BoolV false) (evalMT (PrimExp(IsClosureP, [IntExp 5] )))
    //should equal (BoolV true) (evalMT (PrimExp(IsArrayP, [] )))
    //should equal (BoolV false) (evalMT (PrimExp(IsVoidP, [IntExp 5] )))
    //should equal (BoolV true) (evalMT (PrimExp(IsObjectP, [] )))
    //should equal (BoolV false) (evalMT (PrimExp(IsObjectP, [IntExp 5] )))

// Tests IfExp
[<Fact>]
let testIfExp () =
    should equal (StringV "yes") (evalMT (IfExp (PrimExp(OrP, [BoolExp true; BoolExp false]), StringExp "yes", StringExp "no")))
    should equal (StringV "no") (evalMT (IfExp (PrimExp(AndP, [BoolExp true; BoolExp false]), StringExp "yes", StringExp "no")))

// Tests WhileExp
[<Fact>]
let testWhileExp () =
    // This test should loop 0 times, (initially evaluates to false)
    //should equal (VoidV) (evalMT (WhileExp (BoolExp false, IntExp 5)))
    (*
      let x = 0
      1 while x < 4
           x = x + 1
      2 ID "x"
    *)
    // The loop should run 4 times, and X should have a final value of 4
    should equal (IntV 4) (evalMT (LetExp ("X", (IntExp 0), BeginExp [WhileExp ( PrimExp(LTP, [ID "X"; IntExp 4]), SetExp ("X", PrimExp(PlusP, [ID "X"; IntExp 1])) ); ID "X"])))
    (*
      let x = 0
      1 while x < 0
           x = x + 1
      2 ID "x"
    *)
    // X should end up being 0 still since the loop should never run.
    should equal (IntV 0) (evalMT (LetExp ("X", (IntExp 0), BeginExp [WhileExp ( PrimExp(LTP, [ID "X"; IntExp 0]), SetExp ("X", PrimExp(PlusP, [ID "X"; IntExp 1])) ); ID "X"])))

[<Fact>]
let testNewObjectExp () =
    should equal () ()

[<Fact>]
let testFieldRefExp () =
    //FieldRefExp of (exp * string)
    let (MTEnv : env) = Map.empty<string,fVal ref>
    let (MTSlots : slots) = Map.empty<string,fVal ref>
    let testSlots = MTSlots.Add ("test", ref (IntV 5))
    let testEnv = MTEnv.Add ("testObj", ref (ObjectV (ref testSlots)))
    should equal (IntV 5) (eval testEnv (FieldRefExp (ID "testObj", "test") )) 

[<Fact>]
let testFieldSetExp () =
    //FieldSetExp of (exp * string * exp)
    let (MTEnv : env) = Map.empty<string,fVal ref>
    let (MTSlots : slots) = Map.empty<string,fVal ref>
    let testSlots = MTSlots.Add ("test", ref (IntV 5))
    let testEnv = MTEnv.Add ("testObj", ref (ObjectV (ref testSlots)))
    should equal (IntV 10) (eval testEnv (FieldSetExp (ID "testObj", "test", (IntExp 10)))) 
    should equal (IntV 10) (eval testEnv (FieldRefExp (ID "testObj", "test")))
    should equal (StringV "moo") (eval testEnv (FieldSetExp (ID "testObj", "newRandom", (StringExp "moo"))))
    should equal (StringV "moo") (eval testEnv (FieldRefExp (ID "testObj", "newRandom")))
    

[<Fact>]
let testPrint () =
    should equal (VoidV) (evalMT (PrimExp(PrintP, [IntExp 5])))
    should equal (VoidV) (evalMT (PrimExp(PrintP, [BoolExp false])))
    should equal (VoidV) (evalMT (PrimExp(PrintP, [StringExp "abcd"])))
    should equal (VoidV) (evalMT (PrimExp(PrintP, [(BeginExp [] )])))
    //test for ObjectV
    //test for ClosureV
    //test for ArrayV

(*[<Fact>]
let testReadLine () =
    should equal (StringV "\n") (evalMT (PrimExp(ReadLineP, [])))
    should equal (StringV "fdsa\n") (evalMT (PrimExp(ReadLineP, [])))
*)

//TODO actually test these
[<Fact>]
let testInstanceOf () =
    //this operator consumes an object and a value, and returns true when the object has a ’constructor’ slot and that slot’s value is the second argument.
    let (MTEnv : env) = Map.empty<string,fVal ref>
    let (MTSlots : slots) = Map.empty<string,fVal ref>
    let testSlots = MTEnv.Add ("constructor", ref (StringV "meow"))
    let testEnv = MTEnv.Add ("testObj", ref (ObjectV (ref testSlots)))
    should equal (BoolV true) (eval testEnv (PrimExp(InstanceOfP, [(ID "testObj"); StringExp "meow"])))

// Tests newArray creation
[<Fact>]
let testNewArray () =
    should equal (ArrayV (ref [| |])) (evalMT (PrimExp (NewArrayP, [])))
    should equal (ArrayV (ref [| |])) (evalMT (PrimExp (NewArrayP, [IntExp 5])))

// Tests arrayMax creation
[<Fact>]
let testArrayMax () =
    let arr = [ArrayV (ref [|IntV 7; IntV 8; IntV 9|]) ]
    let theSize = evalArrayMax arr
    should equal (IntV 3) (theSize)

// Tests arraySet updating values in an array and arrayRef getting values out of an Array
[<Fact>]
let testArraySetandRef () =
    let theArray = [|IntV 1; IntV 2; IntV 3; IntV 4|]
    // Set index 3 equal to IntV 10 and put the result into newArray
    let newArrayValue = (evalArraySet [ArrayV (ref theArray); IntV 3; IntV 10])
    // Put the result into newArray
    should equal (newArrayValue) (ArrayV (ref [|IntV 1; IntV 2; IntV 3; IntV 10|]))
    // Set index 5 equal to IntV 20 to test extending the length of the array. Put the result into brandNewArray    
    let brandNewArrayValue = (evalArraySet [newArrayValue; IntV 5; IntV 20])
    // Index 4 should have been created inbetween and initialized to VoidV
    should equal (VoidV) (evalArrayRef [brandNewArrayValue; IntV 4])
    // Index 5 should have been created inbetween and initialized to IntV 20
    should equal (IntV 20) (evalArrayRef [brandNewArrayValue; IntV 5])

[<Fact>]
let testReturnExp () =
    //Throw an exception, and catch the exception from wherever the function was called
    //ReturnExp of exp
    let (MTEnv : env) = Map.empty<string,fVal ref>    
    should equal (IntV 2) (interp MTEnv (LetExp ("X", (IntExp 1),(LetExp ("Y", (IntExp 2),(LetExp ("Z", (IntExp 3),ReturnExp (ID "Y"))))))))

    //when tested with double returns (ReturnExp (ReturnExp (VoidV))) it crashes properly



//TODO test LetrecExp MethodCallExp NewExp AppExp

// HORRIBLE HACK; add top-level non-function things to the tuple below.
//  take them out to see what goes wrong.
[<EntryPoint>]
let entry args =
    ignore (evalMT,emptyEnv)
    0

