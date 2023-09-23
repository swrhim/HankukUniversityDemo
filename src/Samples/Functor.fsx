type [<Struct>] Maybe<'a> = 
| Just of 'a 
| Nothing

let fmap (f: 'a -> 'b) (x : Maybe<'a>) =
    match x with
    | Just x' -> Just (f x')
    | Nothing -> Nothing

let getSafeHead a =
    match a with
    | [] -> None
    | h :: t ->
        Some h

