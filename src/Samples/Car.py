class Car:
    def __init__(self,year,make):
        self.year_model = year
        self.make = make
        self.speed = 0

    def accelerate(self):
        self.speed += 5

    def brake(self):
        self.speed -= 5

    def get_speed(self):
        return self.speed


def getFasterCar (car1, car2):
    if car1.get_speed() >= car2.get_speed():
        return car1
    else:
        return car2
    
car1 = Car(2023, "Tesla")
car2 = Car(2023, "Hyundai")

car1.accelerate()
car2.accelerate()
car2.brake()
getFasterCar (car1, car2)

l = [car1, car2]
l[0]

car1 = Car(2023, "Rivian")
print(car1.make)