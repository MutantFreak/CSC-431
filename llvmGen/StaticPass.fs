module StaticPass

open AST2

exception InternalError of string
exception SyntaxError of string

let ignoreSecondLifted f a b =
    let (r1,r2) = f a b
    r1

// STATIC ENVIRONMENTS FOR VARIABLES

// add a variable to a frame, return the new frame
let addToFrame (varMap,intbox) name : (sframe * (string * int * int)) =
    if (name = "this")
    then raise (SyntaxError (sprintf "attempt to bind variable named \"this\""))
    else
        let i = !intbox
        intbox := (i + 1)
        ((Map.add name i varMap,intbox),(name,0,i))

// add a variable to the top frame of a static environment
let addToSenv ((f1,frames) : senv) name : (senv * (string * int * int)) =
    let (newFrame,varInfo) = addToFrame f1 name
    ((newFrame,frames),varInfo)

// find a variable in a list of static frames, signal an error if not found
let lookup ((f1,frames) : senv) (x : string) : (string * int * int) =
    let rec loop senv frameIdx =
        match senv with
            | [] -> raise (SyntaxError (sprintf "no binding for variable: %A" x))
            | (varmap,_)::frames ->
                match Map.tryFind x varmap with
                    | Some i -> (x,frameIdx,i)
                    | None -> loop frames (frameIdx + 1)
    loop (f1::frames) 0

// add a frame to the top of the static environment, add 'this' if desired
let addFrame (topframe,restframes) addThis =
    ((if addThis
      then (Map.ofList[("this",0)],ref 1)
      else (Map.ofList[],ref 0))
      ,(topframe::restframes))

// TABLES


// add an element to a table if necessary, then return the index.
let addToTable (tableBox : Map<'a,int> ref) (thing : 'a) : int =
    let table = !tableBox
    match Map.tryFind thing table with
        | Some i -> i
        | None ->
            let count = table.Count
            tableBox := Map.add thing count table
            count

// add multiple elements to a table, return a list of the new indices
let addToTableMany tableBox things = List.map (addToTable tableBox) things

// add functions to fun table, return the new indices
let addToFunTableMany (funTableBox,_,_,_) funentries =
    addToTableMany funTableBox funentries

// add a double to the double table, return the new index
let addToDoubleTable (_,doubleTableBox,_,_) (d : double) =
    addToTable doubleTableBox d

// add a string to the string table, return the new index
let addToStringTable (_,_,stringTableBox,_) str =
    addToTable stringTableBox str

// add a field name to the field name table (if necessary), return the index
let addToFieldNameTable (_,_,_,fieldNameTable) str =
    addToTable fieldNameTable str

// USESTHIS

// does the (transformed) body refer to 'this'?
// I'm afraid this function is pure boilerplate...
let rec usesThis exp =
    let recur exps = List.exists usesThis exps
    match exp with
        // one of only two interesting lines...
        | ID (s,a,b) -> (a = 0) && (b = 0)
        | BoolExp _ -> false
        | IntExp _ -> false
        | DoubleExp _ -> false
        | StringExp _ -> false
        | PrimExp (_,es) -> recur es
        | IfExp (tst,thn,els) -> recur [tst;thn;els]
        | WhileExp (tst,body) -> recur [tst;body]
        | ReturnExp e -> recur [e]
        // don't allow 'this' here:
        | SetExp ((x,a,b),e) ->
            if (a=0) && (b = 0)
            then raise (SyntaxError (sprintf "attempt to mutate \"this\"."))
            else recur [e]
        | BeginExp es -> recur es
        | FieldRefExp (e,s) -> recur [e]
        | FieldSetExp (e1,s,e2) -> recur [e1;e2]
        | MethodCallExp (e,s,es) -> recur (e::es)
        | NewExp (e,es) -> recur (e::es)
        | AppExp (e,es) -> recur (e::es)
        | CloExp (s,i) -> false
        | ScopeExp (i,vars,e) -> recur [e]




// TRANSFORMATION

// transform an expression from the AST form to the AST2 form.
// update 'state' as required.
let rec transformExp state (senv : senv) (exp : AST.exp) : exp =
    let transExp = transformExp state // when state is unchanged
    let recur = transExp senv         // when senv is unchanged
    match exp with
        | AST.ID str -> ID (lookup senv str)
        | AST.BoolExp b -> BoolExp b
        | AST.IntExp i -> IntExp i
        | AST.DoubleExp d -> DoubleExp (addToDoubleTable state d)
        | AST.StringExp str -> StringExp (addToStringTable state str)
        | AST.PrimExp (p,args) -> PrimExp (p, List.map recur args)
        | AST.IfExp (tst,thn,els) -> IfExp (recur tst, recur thn, recur els)
        | AST.WhileExp (tst,body) ->
            let ((_,refcell),_) as newSenv = addFrame senv false
            let newBody = transExp newSenv body
            WhileExp (recur tst,ScopeExp (!refcell,[],newBody))
        | AST.LetExp (x,rhs,body) ->
            let (newEnv,varInfo) = addToSenv senv x
            BeginExp [(SetExp (varInfo,(recur rhs)));
                      (transExp newEnv body)]
        | AST.LetrecExp (funs,body) ->
            let (newEnv,setters) = extendWithFuns state senv funs
            BeginExp (setters @ [(transExp newEnv body)])
        | AST.ReturnExp e -> ReturnExp (recur e)
        | AST.SetExp (name,newVal) -> SetExp (lookup senv name,recur newVal)
        | AST.BeginExp exps -> BeginExp (List.map recur exps)
        | AST.FieldRefExp (e,field) -> FieldRefExp (recur e, addToFieldNameTable state field)
        | AST.FieldSetExp (e,field,newVal) -> FieldSetExp (recur e, addToFieldNameTable state field, recur newVal)
        | AST.MethodCallExp (e,field,args) -> MethodCallExp (recur e, addToFieldNameTable state field, List.map recur args)
        | AST.NewExp (ctor,args) -> NewExp (recur ctor, List.map recur args)
        | AST.AppExp (fn,args) -> AppExp (recur fn, List.map recur args)


// add a bunch of functions to the function table
and extendWithFuns state (senv : senv) funs =
    let funnames = [for (name,_,_) in funs -> name]
    let (senvWithFuns,revvarinfos) =  (List.fold (fun (senv,varinfos) newname ->
                                                  let (newSenv,newvarinfo) = addToSenv senv newname
                                                  (newSenv,newvarinfo::varinfos))
                                                   (senv,[])
                                                   funnames)
    let varinfos = List.rev revvarinfos
    let funentries = [for (name,args,body) in funs ->
                      let ((_,refcell),_) as newSenv = List.fold (ignoreSecondLifted addToSenv) (addFrame senvWithFuns true) args
                      let newBody = transformExp state newSenv body
                      (name,args,ScopeExp(!refcell,args,newBody),usesThis newBody)]
    let newIndices = addToFunTableMany state funentries
    let setters = List.map3 (fun name varinfo index  -> SetExp(varinfo,CloExp (name,index))) funnames varinfos newIndices
    (senvWithFuns,setters)

let transform exp =
    let doubleTable = ref (Map.ofList[])
    let stringTable = ref (Map.ofList[])
    let functionTable = ref (Map.ofList[])
    let fieldNameTable = ref (Map.ofList[])
    let mainvars = ref 0;
    let newExp = transformExp (functionTable,doubleTable,stringTable,fieldNameTable) ((Map.ofList[],mainvars),[]) exp
    let i = addToTable functionTable ("main",[],ScopeExp(!mainvars,[],newExp),false)
    (doubleTable,stringTable,functionTable,fieldNameTable)

//printf "%A" (transform (ParseAST.parse "(letrec ((f (x) (+ 3 (f x)))) (f 19.34))"))
