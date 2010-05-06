module Run

open AST
open StaticPass


(* Checks to see if the given id is anywhere in the frameList. If the id is present somewhere in the frameList,
   this returns the frame # and the offset within that frame where the id was found. If it is not present in the
   list, this function should raise an exception. *)
let rec checkFrameList2 ( (((frontFrameMap:Map<string,int>), count) as frontFrame), frameList) (id:string) (frameNumber:int) =
    // if the ID wasn't in the first frame
    if (frontFrameMap.TryFind(id) = None)
         //then try to check the rest of the list, as long as the list isn't empty
    then match frameList with
             | [] -> raise (VariableDoesNotExist(sprintf "%s not found in any frame!\n" id))
             | (top :: rest) -> printf "Searching in frame # %A for variable %A\n" frameNumber id
                                checkFrameList2 (top, rest) id (frameNumber+1)
    else
       (frameNumber, frontFrameMap.Item(id))

(* Recursive function that takes in an AST, and computes and prints out frame # and offset for each variable. *)
let rec walk ourTree ( ( ((frontFrameMap, count) as frontFrame), frameList) as ourSenv) = 
    match ourTree with
          // TODO: Create an AST2.ID of the name, with the two offsets necessary.
        | ID (id:string) -> let (frameNum, offset) = checkFrameList2 ourSenv id 0
                            printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                            ()
          // TODO: Create an AST2.BoolExp (unchanged)
        | BoolExp (value:bool) -> ()
          // TODO: Create an AST2.IntExp (unchanged)
        | IntExp (value:int) -> ()
          // TODO: Search the doubleTable to find where this double is. If it exists, use that offset, if not (maybe throw a error) create it in the table and use that offset. 
          //       Create a AST2.DoubleExp made of an int offset into the doubleTable where this double can be found.
        | DoubleExp (value:double) -> ()
          // TODO: Search the stringTable to find where this string is. If it exists, use that offset, if not (maybe throw a error) create it in the table and use that offset. 
          //       Create a AST2.StringExp made of an int offset into the stringTable where this string can be found.
        | StringExp (value:string) -> ()
          // TODO: Create a new AST2.PrimExp out of the original AST.prim, and a list of the results of walking each exp in the exp list.
        | PrimExp (thePrim:prim, expList:exp list) -> for eachExp in expList do
                                                      walk eachExp ourSenv
                                                      ()
          // TODO: Create a new AST2.IfExp out of the results of walking the three exp's given.
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
          // TODO: Replace LetExp with a BeginExp where the first statement is a SetExp for that variable.
        | LetExp (varName:string, valueExp:exp, inExp:exp) -> walk valueExp ourSenv
                                                              let newFrame = (frontFrameMap.Add (varName, (!count)))
                                                              printf "Adding variable %A at offset %A in a new frame\n" varName count
                                                              count := (!count) + 1
                                                              walk inExp ((newFrame, count), frameList)
                                                              ()
// for each of the function bindings, add it to the map, and bump the count
//for each of the function bindings, walk over its body and tranfrorm it using a new & extended frame
//after processing each function binding, put it into the function table.
// Something tricky will need to go on here.
        | LetrecExp (funList:funbinding list, validInExp:exp) -> walk validInExp (buildNewFrame, (frontFrame :: frameList))
                                                                 ()
          // TODO: Create a new AST2.ReturnExp out of the result from walking the given exp
        | ReturnExp (validInExp:exp) -> let newFrameList = walk validInExp ourSenv
                                        ()
        | SetExp (id:string, newValExp:exp) -> let (frameNum, offset) = checkFrameList2 ourSenv id 0
                                               printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                                               let newFrameList = walk newValExp ourSenv
                                               ()
          // TODO: Before hand, call flattenBegins on the BeginExp that was found, and then use the result in the next step.
          // TODO: Create a new AST2.BeginExp out out of a list of the results from walking each exp in the given exp list
        | BeginExp (expList: exp list) -> for eachExp in expList do
                                          let newFrameList = walk eachExp ourSenv
                                          ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, create one.
          //       Create a new AST2.FieldRefExp out of the result of walking the given exp, coupled with the offset we found.
        | FieldRefExp (objectExp:exp, fieldName:string) -> ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, create one.
          //       Create a new AST2.FieldSetExp out of the result of walking the given exp, coupled with the offset we found, and the result of walking the other given exp.
        | FieldSetExp (objectExp:exp, fieldName:string, newValueExp:exp) -> ()
          // TODO: Lookup the given fieldName in the fieldNameTable - if it exists, remember it, if it doesn't, create one.
          //       Create a new AST2.MethodCallExp out of the result from walking the closureExp, the offset we found, and the a 
          //       list of the results found from walking each of the exp's in the given argsExpList
        | MethodCallExp (closureExp:exp, funName:string, argsExpList:exp list) -> ()
          // TODO: Create a new AST2.NewExp out of the result from walking the closureExp, coupled with a list of the results found 
          //       from walking each of the exp's in the given argsExpList
          // I suspect something tricky will need to go on here.
        | NewExp (closureExp:exp, argsExpList:exp list) -> ()
          // TODO: Create a new AST2.AppExp out of the result from walking the closureExp, coupled with a list of the results found 
          //       from walking each of the exp's in the given argsExpList
          // I suspect something tricky will need to go on here.
        | AppExp (closureExp:exp, argExps:exp list) -> walk closureExp ourSenv
                                                       ()




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

