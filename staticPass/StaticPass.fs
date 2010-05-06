module StaticPass

//can only open one of these two because they collide on the definition of type exp
open AST
//open AST2

exception VariableDoesNotExist of string
exception RuntimeError of string


(*** TODO: Learn how to make a new empty frame / staticEnv so we can call transform with it.
// the empty frameList
let (emptyFrame : env) = Map.ofList[]

// Returns a function that expects an expression. It will evaluate that expression given the empty environment just defined.
let evalMT = eval emptyEnv

//call with new frame (<string, int>
***)

//let buildNewFrame = (Map.ofList[], ref 0)

let ourDoubleTable = ref Map.empty<double,int>
let ourStringTable = ref Map.empty<string,int>
let ourFunTable = ref Map.empty<AST2.funEntry,int>
let ourFieldNameTable = ref Map.empty<string,int>

(* Checks to see if the given id is anywhere in the frameList. If the id is present somewhere in the frameList,
   this returns the frame # and the offset within that frame where the id was found. If it is not present in the
   list, this function should raise an exception. *)
let rec checkFrameList ( (((frontFrameMap:Map<string,int>), count) as frontFrame), frameList) (id:string) (frameNumber:int) =
    // if the ID wasn't in the first frame
    if (frontFrameMap.TryFind(id) = None)
         //then try to check the rest of the list, as long as the list isn't empty
    then match frameList with
             | [] -> raise (VariableDoesNotExist(sprintf "%s not found in any frame!\n" id))
             | (top :: rest) -> printf "Searching in frame # %A for variable %A\n" frameNumber id
                                checkFrameList (top, rest) id (frameNumber+1)
    else
       (frameNumber, frontFrameMap.Item(id))

(* Recursive function that takes in an AST, and computes and prints out frame # and offset for each variable. *)
let rec transform ourTree ( ( ((frontFrameMap, count) as frontFrame), frameList) as ourSenv) = 
    match ourTree with
          // TODO: Create an AST2.ID of the name, with the two offsets necessary.
        | ID (id:string) -> let (frameNum, offset) = checkFrameList ourSenv id 0
                            //printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                            //()
                            AST2.ID (id, frameNum, offset)
          // TODO: Create an AST2.BoolExp (unchanged)
        | BoolExp (value:bool) -> //()
                                  AST2.BoolExp (value)
          // TODO: Create an AST2.IntExp (unchanged)
        | IntExp (value:int) -> //()
                                AST2.IntExp (value)
(**
          // TODO: Search the doubleTable to find where this double is. If it exists, use that offset, if not create it in the table and use that offset. 
          //       Create a AST2.DoubleExp made of an int offset into the doubleTable where this double can be found.
        | DoubleExp (value:double) -> ()
          // TODO: Search the stringTable to find where this string is. If it exists, use that offset, if not create it in the table and use that offset. 
          //       Create a AST2.StringExp made of an int offset into the stringTable where this string can be found.
        | StringExp (value:string) -> ()
          // TODO: Create a new AST2.PrimExp out of the original AST.prim, and a list of the results of transforming each exp in the exp list.
        | PrimExp (thePrim:prim, expList:exp list) -> for eachExp in expList do
                                                      transform eachExp ourSenv
                                                      ()
          // TODO: Create a new AST2.IfExp out of the results of transforming the three exp's given.
        | IfExp (guardExp:exp, thenExp:exp, elseExp:exp) -> transform guardExp ourSenv
                                                            transform thenExp ourSenv
                                                            transform elseExp ourSenv
                                                            ()
          // Need to create a new empty frame and add to the head of the frameList. Evaluate the bodyExp in context of this new frameList.
          // This is only done for while loops and function applications
        | WhileExp (guardExp:exp, bodyExp:exp) -> transform guardExp ourSenv
                                                  transform bodyExp (buildNewFrame, (frontFrame :: frameList))
                                                  ()
          // Variable Assignment. Extend the frame's map increase the ref to how many variables have been seen in this frame
          // TODO: Replace LetExp with a BeginExp where the first statement is a SetExp for that variable.
        | LetExp (varName:string, valueExp:exp, inExp:exp) -> transform valueExp ourSenv
                                                              let newFrame = (frontFrameMap.Add (varName, (!count)))
                                                              printf "Adding variable %A at offset %A in a new frame\n" varName count
                                                              count := (!count) + 1
                                                              transform inExp ((newFrame, count), frameList)
                                                              ()
// for each of the function bindings, add it to the map, and bump the count
//for each of the function bindings, transform its body and tranfrorm it using a new & extended frame
//after processing each function binding, put it into the function table.
// Something tricky will need to go on here.
        | LetrecExp (funList:funbinding list, validInExp:exp) -> transform validInExp (buildNewFrame, (frontFrame :: frameList))
                                                                 ()
          // TODO: Create a new AST2.ReturnExp out of the result from transforming the given exp
        | ReturnExp (validInExp:exp) -> let newFrameList = transform validInExp ourSenv
                                        ()
        | SetExp (id:string, newValExp:exp) -> let (frameNum, offset) = checkFrameList ourSenv id 0
                                               printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                                               let newFrameList = transform newValExp ourSenv
                                               ()
          // TODO: Before hand, call flattenBegins on the BeginExp that was found, and then use the result in the next step.
          // TODO: Create a new AST2.BeginExp out out of a list of the results from transforming each exp in the given exp list
        | BeginExp (expList: exp list) -> for eachExp in expList do
                                          let newFrameList = transform eachExp ourSenv
                                          ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, throw an error
          //       Create a new AST2.FieldRefExp out of the result of transforming the given exp, coupled with the offset we found.
        | FieldRefExp (objectExp:exp, fieldName:string) -> ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, create one.
          //       Create a new AST2.FieldSetExp out of the result of transforming the given exp, coupled with the offset we found, and the result of transforming the other given exp.
        | FieldSetExp (objectExp:exp, fieldName:string, newValueExp:exp) -> ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, create one.
          //       Create a new AST2.MethodCallExp out of the result from transforming the closureExp, the offset we found, and the a 
          //       list of the results found from transforming each of the exp's in the given argsExpList
        | MethodCallExp (closureExp:exp, funName:string, argsExpList:exp list) -> ()
          // TODO: Create a new AST2.NewExp out of the result from transforming the closureExp, coupled with a list of the results found 
          //       from transforming each of the exp's in the given argsExpList
          // I suspect something tricky will need to go on here.
        | NewExp (closureExp:exp, argsExpList:exp list) -> ()
          // TODO: Create a new AST2.AppExp out of the result from transforming the closureExp, coupled with a list of the results found 
          //       from transforming each of the exp's in the given argsExpList
          // I suspect something tricky will need to go on here.
        | AppExp (closureExp:exp, argExps:exp list) -> transform closureExp ourSenv
                                                       ()
***)

// This function takes in a list of exp
let rec flattenHelper answer theHead = 
   match (theHead) with 
       | [] -> answer
       // if this is the last element in the list of exp's and it is empty, then preserve it
       | theHead::restOfList -> match (theHead) with
                                    | BeginExp(gutsList) -> if (restOfList = [] && gutsList = [])
                                                            then (flattenHelper (List.append answer [theHead]) restOfList)
                                                            else
                                                            flattenHelper answer (List.append gutsList restOfList)
                                    | _ -> let newAnswer = (List.append answer [theHead])
                                           flattenHelper newAnswer restOfList

// This function takes in an BeginExp
let flattenBegins (express:exp) =
   match (express) with
       //find the first beginExp, & call flattenHelper on it's guts
       | BeginExp(gutsList) -> BeginExp(flattenHelper [] gutsList)
       | _ -> raise (RuntimeError("First Exp is not BeginExp"))
       

