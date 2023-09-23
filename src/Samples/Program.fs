let f a =  a + 1
let g b = b + 2
let g c = c + 3

let h d= d + 4 

let gof = g << f
let hog = h << g

let r = (h << gof) 1 = (hog << f) 1
printf "%A" r
