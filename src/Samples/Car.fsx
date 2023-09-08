type Car = {
    year : int
    make : string
    speed : float
} 
    with 
    static member Accelerate (car : Car) = { 
            car with speed = car.speed + 5.
        }
    
    static member Brake (car : Car) = {
        car with speed = car.speed - 5.
    }

let getFasterCar car1 car2 =
    if car1.speed >= car2.speed then car1
    else car2

let car1 = { year = 2023; make = "Tesla"; speed = 0. }

let car2 = { year = 2023; make = "Hyundai"; speed =0. }

Car.Accelerate(car1)
Car.Accelerate(car2)
Car.Brake(car2)

getFasterCar car1 car2

let l = [car1 ; car2]
l.Head

//let car1 = {year = 2023; make = "Rivian"; speed = 0.}
