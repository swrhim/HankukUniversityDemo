let x = [1] @ [2] @ [3] 
let y =[ [1] ; [2] ; [3] ] |> List.reduce (@)
x = y

//Practical
type ProductLine = {
    Id : string
    Qty : int
    Price : float
    LineTotal : float
}

type TotalLine = {
    Qty : int
    OrderTotal : float
}

type OrderLine =
| Product of ProductLine
| Total of TotalLine
| Empty

let addLine order1 order2 =
    match order1, order2 with
    | _, Empty -> order1
    | Empty, _ -> order2
    | Product p1, Product p2 ->
        Total {
            TotalLine.Qty = p1.Qty + p2.Qty
            OrderTotal = p1.LineTotal + p2.LineTotal
        }
    | Product p1, Total t ->
        Total {
            TotalLine.Qty = p1.Qty + t.Qty
            OrderTotal = p1.LineTotal + t.OrderTotal
        }
    | Total t, Product p2 ->
        Total {
            TotalLine.Qty = p2.Qty + t.Qty
            OrderTotal = p2.LineTotal + t.OrderTotal
        } 
    | Total t1, Total t2 ->
        Total {
            TotalLine.Qty = t1.Qty + t2.Qty
            OrderTotal = t1.OrderTotal + t2.OrderTotal
        }

let zero = Empty

let productLine1 = Product {Id="AAA"; Qty=2; Price=9.99; LineTotal=19.98}
let productLine2 = Product {Id="BBB"; Qty=3; Price=10.; LineTotal=43.21}
let productLine3 = Product {Id="CCC"; Qty=4; Price=11.23; LineTotal=21.23}

let lines = [productLine1 ; productLine2 ; productLine3]

let r = 
    lines
    |> List.reduce(addLine)

sprintf "%A" r