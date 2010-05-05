module StaticPassTest

open Xunit
open FsxUnit.Syntax
open AST
open StaticPass


//flattenBegins (express:exp)
[<Fact>]
let testFlattenBegins1 () =
    (flattenBegins (BeginExp [] ) ) |> should equal (BeginExp [] )
    
[<Fact>]
let testFlattenBegins2 () =
    (flattenBegins (BeginExp [BeginExp []] ) ) |> should equal (BeginExp [BeginExp []] )
    
[<Fact>]
let testFlattenBegins3 () =
    (flattenBegins (BeginExp [BeginExp []; BeginExp []] ) ) |> should equal (BeginExp [BeginExp []] )
    
[<Fact>]
let testFlattenBegins4 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 5]; BeginExp []] ) ) |> should equal (BeginExp [IntExp 5; BeginExp []] )
    
[<Fact>]
let testFlattenBegins5 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 5; StringExp "fdsa"]; BeginExp []] ) ) |> should equal (BeginExp [IntExp 5; StringExp "fdsa"; BeginExp []] )
    
[<Fact>]
let testFlattenBegins6 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 5]; StringExp "fdsa"; BeginExp []] ) ) |> should equal (BeginExp [IntExp 5; StringExp "fdsa"; BeginExp []] )
    
[<Fact>]
let testFlattenBegins7 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 5]; StringExp "fdsa"; BeginExp [IntExp 5]] ) ) |> should equal (BeginExp [IntExp 5; StringExp "fdsa"; IntExp 5] )
    
[<Fact>]
let testFlattenBegins8 () =
    (flattenBegins (BeginExp [BeginExp [BeginExp [BeginExp []]]] ) ) |> should equal (BeginExp [BeginExp []] )
    
[<Fact>]
let testFlattenBegins9 () =
    (flattenBegins (BeginExp [BeginExp [BeginExp [BeginExp [IntExp 6]]]] ) ) |> should equal (BeginExp [IntExp 6] )

[<Fact>]
let testFlattenBegins10 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 4]; BeginExp [IntExp 5]; BeginExp [IntExp 6]]) ) |> should equal (BeginExp [IntExp 4; IntExp 5; IntExp 6] )
    
[<Fact>]
let testFlattenBegins11 () =
    (flattenBegins (BeginExp [BeginExp [IntExp 4]; BeginExp [IntExp 5; StringExp "bb"]; BeginExp [IntExp 6]]) ) |> should equal (BeginExp [IntExp 4; IntExp 5; StringExp "bb"; IntExp 6] )

