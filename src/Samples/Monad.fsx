type State      = Map<string,obj>
type State<'T>  = State -> State*'T

// sreturn & sbind completes the State Monad
let sreturn v : State<'T> =
 fun state ->
   state, v

let sbind (t : State<'T>) (uf : 'T -> State<'U>) : State<'U> =
 fun state ->
   let ts, tv = t state
   let u = uf tv
   u ts
let inline (>>=) t uf   = sbind t uf

// Manipulates the implicit state
let sget k : State<'T option> =
 fun state ->
  match Map.tryFind k state with
  | Some (:? 'T as v) -> state, Some v
  | _ -> state, None

let sset k v : State<unit> =
 fun state -> Map.add k v state, ()

type StateBuilder () =
  member inline x.Return v = sreturn v
  member inline x.Bind   (t, uf)   = sbind   t uf

let state = StateBuilder ()

type Customer =
  {
    Id        : string
    FirstName : string
    LastName  : string
    Age       : int
  }

let createNewCustomer fn ln a =
  state {
    let id = System.Guid.NewGuid () |> string
    let customer : Customer = { Id = id; FirstName = fn; LastName = ln; Age = a }
    do! sset id customer
    return id
  }

let updateAgeOfCustomer id newAge =
  state {
    let! (customer : Customer option) = sget id
    match customer with
    | Some customer ->
      let customer = { customer with Age = newAge }
      do! sset id customer
    | _ -> return ()
  }

let testRun =
  state {
    let! id = createNewCustomer "Justin" "Bieber" 20
    do! updateAgeOfCustomer id 21
  }

let testRunAlternative =
  createNewCustomer "Justin" "Bieber" 20 >>= fun id -> updateAgeOfCustomer id 21

[<EntryPoint>]
let main argv =
  printfn "Test after run: %A" <| testRun             Map.empty
  printfn "Test after run: %A" <| testRunAlternative  Map.empty

  0