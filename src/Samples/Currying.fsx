let x = 1
let y = 2

//Explicit currying
let addingTwoNumbers x =
    let subFunction y =
        x + y
    subFunction

//Assumed
let addingTwoNumbersAssumedVersion x y = 
    x + y

let xOpt = Some 1
let yOpt = Some 2

let apply fopt xopt =
    match fopt, xopt with
    | Some f, Some x -> Some (f x)
    | _ -> None

apply (apply (pure addingTwoNumbers) xOpt) yOpt