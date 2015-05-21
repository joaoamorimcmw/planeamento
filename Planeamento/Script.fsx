open System

let rec sort list =
    match list with
    | [] -> []
    | h :: t -> 
        let (f,s) = List.fold (fun (a,b) x -> if x <= h then ([x] @ a,b) else (a,[x] @ b)) ([],[]) t
        (sort f) @ [h] @ (sort s)

let rec last list =
    match list with
    | [] -> None
    | [x] -> Some(x)
    | h :: t -> last t

let rec last2 list =
    match list with
    | [] -> None
    | [x] -> None
    | [a;b] -> Some ((a,b))
    | h :: t -> last2 t

let rec kth n list =
    match n, list with
    | _, [] -> None
    | 1, h::t -> Some (h)
    | n, h::t -> kth (n-1) t

let lSum list = List.fold (+) 0 list

let rec reverse list =
    match list with
    | [] -> []
    | h :: t -> (reverse t) @ [h]

let is_palindrome list =
    let rec lEquals list1 list2 =
        match list1,list2 with
        | [],[] -> true
        | _,[] -> false
        | [],_ -> false
        | x::xs,y::ys -> (x = y) && (lEquals xs ys)
    lEquals list (reverse list)

type 'a node =
    | One of 'a 
    | Many of 'a node list

let rec flatten list =
    match list with
    | [] -> []
    | (One (x)) :: t -> x :: (flatten t)
    | (Many (x)) :: t -> (flatten x) @ (flatten t)

let compress list =
    let rec compressAux opt list =
        match opt,list with
        | _,[] -> []
        | None, h::t -> h :: compressAux (Some(h)) t
        | Some(x), h::t -> if (x=h) then compressAux (Some(x)) t else h :: compressAux (Some(h)) t
    compressAux None list

let pack list =
    let rec packAux cur list =
        match cur,list with
        | x,[] -> x
        | [], h::t -> packAux [h] t
        | l, h::t -> if (List.head(l)=h) then packAux (l @ [h]) t else [l] @ (packAux [h] t)
    packAux [] list

let list = [1;5;2;6;1;6;3;9;12;4;2]
let listNode = [One(1); Many([One(5); One(2); Many([One(6)])])]
let listDups = [1;1;1;1;2;3;3;4;1;1;5;6;6;7]
let p01 = sort list
let p02 = last list
let p03 = last2 list
let p04 = kth 11 list
let p05 = lSum list
let p06 = reverse list
let p07 = is_palindrome list
let p08 = flatten listNode
let p09 = compress listDups