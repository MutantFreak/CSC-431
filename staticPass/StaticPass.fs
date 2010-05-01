open AST

exception variableDoesNotExist of string

type staticEnv = frame list
type frame = (Map <string, int>, int ref)

(*** TODO: Learn how to make a new empty frame / staticEnv so we can call walk with it.
// the empty frameList
let (emptyFrame : env) = Map.ofList[]

// Returns a function that expects an expression. It will evaluate that expression given the empty environment just defined.
let evalMT = eval emptyEnv

//call with new frame (<string, int>
***)

let buildNewFrame = (Map.ofList [], ref 0)

(* Recursive function that takes in an AST, and computes and prints out frame # and offset for each variable. *)
let rec walk ourTree (frameList( (thisFrame(theMap, count) )::rest)) = 
    match ourTree with
        | ID (id:string) -> let (frameNum, offset) = checkFrameList frameList id
                            printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                            ()
        | BoolExp (val:bool) -> ()
        | IntExp (val:int) -> ()
        | DoubleExp (val:double) -> ()
        | StringExp (val:string) -> ()
        | PrimExp (thePrim:prim, expList:exp list) -> for eachExp in expList do
                                                      walk eachExp frameList
                                                      ()
        | IfExp (guardExp:exp, thenExp:exp, elseExp:exp) -> walk guardExp frameList
                                                            walk thenExp frameList
                                                            walk elseExp frameList
                                                            ()
          // Need to create a new empty frame and add to the head of the frameList. Evaluate the bodyExp in context of this new frameList.
          // This is only done for while loops and function applications
        | WhileExp (guardExp:exp, bodyExp:exp) -> walk guardExp frameList
                                                  walk bodyExp (buildNewFrame :: frameList)
                                                  ()
          // Variable Assignment. Extend the frame's map increase the ref to how many variables have been seen in this frame
        | LetExp (varName:string, valueExp:exp, inExp:exp) -> let newFrameList1 = walk valueExp frameList
//will have to separate into diff lines.
                                                              newFrame = (Map.add varName count, (count := count + 1))
                                                              newFrameList2 = newFrame::frameList
                                                              let newFrameList3 = walk inExp newFrameList2
                                                              ()
        | LetrecExp (funList:funbinding list, validInExp:exp) -> walk validInExp (buildNewFrame :: frameList)
// for each of the function bindings, add to the map, and bump the count
//for each of the function bindings, walk over its body and tranfrorm it using a new & extended frame
//after processing each function binding, put it into the function table.
                                                                 ()
        | ReturnExp exp -> let newFrameList = walk validInExp frameList
                           ()
        | SetExp (id:string, newValExp:exp) -> let (frameNum, offset) = checkFrameList frameList id
                                               printf "Found id %A at frame: %A, offset %A\n" id frameNum offset
                                               let newFrameList = walk newValExp frameList
                                               ()
        | BeginExp (exp list) -> for eachExp in expList do
                                 let newFrameList = walk eachExp frameList
                                 ()
        | FieldRefExp (exp * string) -> 
        | FieldSetExp (exp * string * exp) -> 
        | MethodCallExp (exp * string * exp list) -> 
        | NewExp (exp * exp list) -> 
        | AppExp (closureExp:exp, argExps:exp list) -> walk closureExp frameList
                                                       

(* Checks to see if the given id is anywhere in the frameList. If the id is present somewhere in the frameList,
   this returns the frame # and the offset within that frame where the id was found. If it is not present in the
   list, this function should raise an exception. *)
let rec checkFrameList (thisFrame::frameList) id:string frameNumber:int=
   if(thisFrame.IsEmpty)
   then raise (OuterError("%s not found in this frame!" id))
   else
       if(thisFrame.TryFind(id) == None)
       then checkFrameList frameList id (frameNumber+1)
       else
           (frameNumber,thisFrame.Item(id))

