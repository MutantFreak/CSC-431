module StaticPass

//can only open one of these two because they collide on the definition of type exp
open AST
//open AST2

exception VariableDoesNotExist of string
exception RuntimeError of string
//type frame = (Map<string, int>, int ref)
//type staticEnv = frame list


(*** TODO: Learn how to make a new empty frame / staticEnv so we can call walk with it.
// the empty frameList
let (emptyFrame : env) = Map.ofList[]

// Returns a function that expects an expression. It will evaluate that expression given the empty environment just defined.
let evalMT = eval emptyEnv

//call with new frame (<string, int>
***)

let buildNewFrame = (Map.ofList[], ref 0)

(* Checks to see if the given id is anywhere in the frameList. If the id is present somewhere in the frameList,
   this returns the frame # and the offset within that frame where the id was found. If it is not present in the
   list, this function should raise an exception. *)
let rec checkFrameList ( (((frontFrameMap:Map<string,int>), count) as frontFrame), frameList) (id:string) (frameNumber:int) =
    // if the ID wasn't in the first frame
    if (frontFrameMap.TryFind(id) = None)
         //then try to check the rest of the list, as long as the list isn't empty
    then match frameList with
             | [] -> raise (VariableDoesNotExist(sprintf "%s not found in any frame!" id))
             | (top :: rest) -> checkFrameList (top, rest) id (frameNumber+1)
    else
       (frameNumber, frontFrameMap.Item(id))

(* Recursive function that takes in an AST, and computes and prints out frame # and offset for each variable. *)
let rec walk ourTree ( ( ((frontFrameMap, count) as frontFrame), frameList) as ourSenv) = 
    match ourTree with
        | ID (id:string) -> let (frameNum, offset) = checkFrameList ourSenv id 0
                            printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                            ()
        | BoolExp (value:bool) -> ()
        | IntExp (value:int) -> ()
        | DoubleExp (value:double) -> ()
        | StringExp (value:string) -> ()
        | PrimExp (thePrim:prim, expList:exp list) -> for eachExp in expList do
                                                      walk eachExp ourSenv
                                                      ()
        | IfExp (guardExp:exp, thenExp:exp, elseExp:exp) -> walk guardExp ourSenv
                                                            walk thenExp ourSenv
                                                            walk elseExp ourSenv
                                                            ()
          // Need to create a new empty frame and add to the head of the frameList. Evaluate the bodyExp in context of this new frameList.
          // This is only done for while loops and function applications
        | WhileExp (guardExp:exp, bodyExp:exp) -> walk guardExp ourSenv
                                                  walk bodyExp (buildNewFrame, (frontFrame :: frameList))
                                                  ()
          // Variable Assignment. Extend the frame's map increase the ref to how many variables have been seen in this frame
        | LetExp (varName:string, valueExp:exp, inExp:exp) -> walk valueExp ourSenv
                                                              count := (!count) + 1
                                                              let newFrame = (frontFrameMap.Add (varName, (!count)))
                                                              walk inExp ((newFrame, count), frameList)
                                                              ()
        | LetrecExp (funList:funbinding list, validInExp:exp) -> walk validInExp (buildNewFrame, (frontFrame :: frameList))
// for each of the function bindings, add it to the map, and bump the count
//for each of the function bindings, walk over its body and tranfrorm it using a new & extended frame
//after processing each function binding, put it into the function table.
                                                                 ()
        | ReturnExp (validInExp:exp) -> let newFrameList = walk validInExp ourSenv
                                        ()
        | SetExp (id:string, newValExp:exp) -> let (frameNum, offset) = checkFrameList ourSenv id 0
                                               printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                                               let newFrameList = walk newValExp ourSenv
                                               ()
        | BeginExp (expList: exp list) -> for eachExp in expList do
                                          let newFrameList = walk eachExp ourSenv
                                          ()
        | FieldRefExp (objectExp:exp, fieldName:string) -> ()
        | FieldSetExp (objectExp:exp, fieldName:string, newValueExp:exp) -> ()
        | MethodCallExp (closureExp:exp, funName:string, argsExpList:exp list) -> ()
        | NewExp (closureExp:exp, argsExpList:exp list) -> ()
        | AppExp (closureExp:exp, argExps:exp list) -> walk closureExp ourSenv
                                                       ()



// This function takes in a list of exp
let rec flattenHelper answer theHead = 
   match (theHead) with 
       | [] -> answer
       // if this is the last element in the list of exp's and it is empty, then preserve it
       | theHead::restOfList -> match (theHead) with
                                    | BeginExp(gutsList) -> if (restOfList = [] && gutsList = [])
                                                            then (flattenHelper (Array.append answer [theHead]) restOfList)
                                                            else
                                                            flattenHelper answer (gutsList)::restOfList
                                    | _ -> answer::theHead
                                           flattenHelper answer restOfList

// This function takes in an BeginExp
let flattenBegins (express:exp) =
   match (express) with
       //find the first beginExp, & call flattenHelper on it's guts
       | BeginExp(gutsList) -> (flattenHelper [] gutsList)
       | _ -> raise (RuntimeError("First Exp is not BeginExp"))
       

