//composition
let f a =  a + 1
let g b = b + 2
let h d= d + 3 

let gof = g << f
let hog = h << g

(h << gof) 1 = (hog << f) 1

//idnetity
(f << id) 1 = 2

//netural
let neutral = 0
neutral + 1 = 1
